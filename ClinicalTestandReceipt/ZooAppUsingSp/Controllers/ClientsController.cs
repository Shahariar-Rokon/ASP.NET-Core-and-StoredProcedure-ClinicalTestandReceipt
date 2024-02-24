using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ZooAppUsingSp.Data;
using ZooAppUsingSp.Models;
using ZooAppUsingSp.ReceiptNoGenaration;
using ZooAppUsingSp.ViewModels;

namespace ZooAppUsingSp.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["TestUnitOptions"] = new SelectList(_context.TestUnits, "Id", "Name");
            ViewData["TestOptions"] = new SelectList(_context.Tests, "Id", "Name");

            var testWithDetails = await _context.ClientHeaders.Include(p => p.testDetails).ThenInclude(p => p.TestUnit).ToListAsync();

            var tests = await _context.Tests.ToArrayAsync();

            List<ClientHeaderVM> clientHeaderVMs = new List<ClientHeaderVM>();

            if (testWithDetails.Count > 0)
            {
                foreach (var test in testWithDetails)
                {
                    List<TestDetailVM> testDetailVMs = new List<TestDetailVM>();

                    if (test.testDetails.Count > 0)
                    {
                        foreach (var pd in test.testDetails)
                        {
                            string testName = tests.Single(p => p.Id == pd.TestId).Name;
                            testDetailVMs.Add(new TestDetailVM(pd.Id, pd.TestId, pd.Quantity, pd.TestUnitId, pd.TestUnitPrice, pd.TestTotalPrice, pd.ClientHeaderId, testName, pd.TestUnit.Name));
                        }
                    }

                    if (testDetailVMs.Count > 0)
                    {
                        clientHeaderVMs.Add(new ClientHeaderVM
                        {
                            Id = test.Id,
                            ClientName = test.ClientName,
                            ClientEmailAddress = test.ClientEmailAddress,
                            ClientPhoneNumber = test.ClientPhoneNumber,
                            ReceiptNumber = test.ReceiptNumber,
                            TestDate = test.TestDate,
                            TotalAmount = test.TotalBill,
                            TestDetails = testDetailVMs
                        });
                    }
                    else
                    {
                        clientHeaderVMs.Add(new ClientHeaderVM
                        {
                            Id = test.Id,
                            ClientName = test.ClientName,
                            ClientEmailAddress = test.ClientEmailAddress,
                            ClientPhoneNumber = test.ClientPhoneNumber,
                            ReceiptNumber = test.ReceiptNumber,
                            TestDate = test.TestDate,
                            TotalAmount = test.TotalBill,

                        });
                    }

                }
            }
            return View(clientHeaderVMs);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TestDetail(TestDetailVM testDetailVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return PartialView("_testDetail", testDetailVM);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientHeaderVM clientHeaderVM, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ClientHeader clientHeader = new ClientHeader(clientHeaderVM.Id, ReceiptNo.Get(), clientHeaderVM.TestDate, clientHeaderVM.TestDetails.Sum(p => p.UnitPrice * p.Quantity), clientHeaderVM.ClientName, clientHeaderVM.ClientPhoneNumber, clientHeaderVM.ClientEmailAddress);

                    foreach (TestDetailVM pdvm in clientHeaderVM.TestDetails)
                    {
                        clientHeader.testDetails.Add(new TestDetail(pdvm.Id, pdvm.TestId, pdvm.Quantity, pdvm.TestUnitId, pdvm.UnitPrice, pdvm.Quantity * pdvm.UnitPrice, pdvm.ClientHeaderId));
                    }

                    await _context.ClientHeaders.AddAsync(clientHeader);
                    await _context.SaveChangesAsync();

                    var products = await _context.Tests.ToListAsync();
                    var testUnits = await _context.TestUnits.ToListAsync();

                    ClientHeaderVM phvm = new ClientHeaderVM(clientHeader.Id, clientHeader.ClientName,clientHeader.ClientPhoneNumber, clientHeader.ClientEmailAddress, clientHeader.ReceiptNumber, clientHeader.TestDate, clientHeader.TotalBill);

                    foreach (TestDetail pd in clientHeader.testDetails)
                    {
                        string testUnitName = testUnits.Single(m => m.Id == pd.TestUnitId).Name;
                        string productName = products.Single(p => p.Id == pd.TestId).Name;

                        phvm.TestDetails.Add(new TestDetailVM(pd.Id, pd.TestId, pd.Quantity, pd.TestUnitId, pd.TestUnitPrice, pd.TestTotalPrice, pd.ClientHeaderId, productName, testUnitName));
                    }
                    return PartialView("_testInfo", phvm);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateExecutingSP(ClientHeaderVM clientHeaderVM, string token)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var testDetailDataTable = new DataTable();
                    testDetailDataTable.Columns.Add("TestId", typeof(int));
                    testDetailDataTable.Columns.Add("Quantity", typeof(decimal));
                    testDetailDataTable.Columns.Add("TestUnitId", typeof(int));
                    testDetailDataTable.Columns.Add("TestUnitPrice", typeof(decimal));
                    testDetailDataTable.Columns.Add("ClientHeaderId", typeof(int));

                    foreach (TestDetailVM pdvm in clientHeaderVM.TestDetails)
                    {
                        DataRow dataRow = testDetailDataTable.NewRow();
                        dataRow["TestId"] = pdvm.TestId;
                        dataRow["Quantity"] = pdvm.Quantity;
                        dataRow["TestUnitId"] = pdvm.TestUnitId;
                        dataRow["TestUnitPrice"] = pdvm.UnitPrice;
                        dataRow["ClientHeaderId"] = pdvm.ClientHeaderId;
                        testDetailDataTable.Rows.Add(dataRow);
                    }

                    SqlParameter clientName = new SqlParameter
                    {
                        ParameterName = "@ClientName",
                        SqlDbType = SqlDbType.NVarChar,
                        Size = 32767,
                        Value = clientHeaderVM.ClientName
                    };

                    SqlParameter clientPhoneNumber = new SqlParameter
                    {
                        ParameterName = "@ClientPhoneNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = clientHeaderVM.ClientPhoneNumber
                    };

                    SqlParameter clientEmailAddress = new SqlParameter
                    {
                        ParameterName = "@ClientEmailAddress",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = clientHeaderVM.ClientEmailAddress
                    };

                    SqlParameter receiptNumber = new SqlParameter
                    {
                        ParameterName = "@ReceiptNumber",
                        SqlDbType = SqlDbType.NVarChar,
                        Value = ReceiptNo.Get()
                    };

                    SqlParameter testDate = new SqlParameter
                    {
                        ParameterName = "@TestDate",
                        SqlDbType = SqlDbType.Date,
                        Value = clientHeaderVM.TestDate
                    };

                    SqlParameter tvp = new SqlParameter
                    {
                        ParameterName = "@TVP",
                        SqlDbType = SqlDbType.Structured,
                        TypeName = "dbo.ParamTestDetail",
                        Value = testDetailDataTable
                    };

                    await _context.Database.ExecuteSqlRawAsync(
                    "EXEC ClientSP @ClientName, @ClientPhoneNumber, @ClientEmailAddress, @ReceiptNumber, @TestDate, @TVP",
                     clientName,
                     clientPhoneNumber,
                     clientEmailAddress,
                     receiptNumber,
                     testDate,
                     tvp
                       );
                    var model = _context.ClientHeaders.FirstOrDefault();
                    //return PartialView("_testDetail", model);
                    return Content("Succeeded", "text/html", System.Text.Encoding.UTF8);
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
//ExecuteSqlRawAsync added
