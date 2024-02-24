using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooAppUsingSp.Data;
using ZooAppUsingSp.ViewModels;
using ZooAppUsingSp.Models;

namespace ZooAppUsingSp.Controllers
{
    public class TestUnitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestUnitController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<TestUnitVM> measurementUnitVMs = _context.TestUnits.Select(x => new TestUnitVM(x.Id, x.Name));
            return View(await measurementUnitVMs.ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(TestUnitVM aTestUnitVM, string actionType, string token)
        {

            if (actionType == "add")
            {
                await _context.TestUnits.AddAsync(new TestUnit(aTestUnitVM.Name));
                await _context.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                TestUnit measurementUnit = new TestUnit(aTestUnitVM.Id, aTestUnitVM.Name);
                _context.TestUnits.Update(measurementUnit);
                await _context.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<TestUnitVM> measurementUnitVMs = _context.TestUnits.Select(x => new TestUnitVM(x.Id, x.Name)).ToList();
            if (measurementUnitVMs.Count > 0)
            {
                foreach (TestUnitVM measurementUnitVM in measurementUnitVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + measurementUnitVM.Name + "</td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + measurementUnitVM.Id + "\" data-name=\"" + measurementUnitVM.Name + "\" onclick=\"editTestUnit(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteTestUnit('" + token + "'," + measurementUnitVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            TestUnit measurementUnit = await _context.TestUnits.FindAsync(id);

            _context.TestUnits.Remove(measurementUnit);
            await _context.SaveChangesAsync();

            string trsWithTds = string.Empty;
            List<TestUnitVM> measurementUnitVMs = _context.TestUnits.Select(x => new TestUnitVM(x.Id, x.Name)).ToList();
            if (measurementUnitVMs.Count > 0)
            {
                foreach (TestUnitVM measurementUnitVM in measurementUnitVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + measurementUnitVM.Name + "</td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + measurementUnitVM.Id + "\" data-name=\"" + measurementUnitVM.Name + "\" onclick=\"editTestUnit(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteTestUnit('" + token + "'," + measurementUnitVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
