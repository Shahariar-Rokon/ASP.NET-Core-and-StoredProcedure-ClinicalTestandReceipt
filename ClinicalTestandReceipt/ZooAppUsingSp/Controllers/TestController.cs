using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZooAppUsingSp.Data;
using ZooAppUsingSp.ViewModels;
using ZooAppUsingSp.Models;

namespace ZooAppUsingSp.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;
        private IWebHostEnvironment _webHostEnvironment;


        public TestController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<TestVM> productVMs = _context.Tests.Select(p => new TestVM(p.Id, p.Name, p.ImageUrl));
            return View(await productVMs.ToListAsync());
        }

        [HttpPost]
        public async Task<string> GetImageUrl(IFormFile image)
        {
            if (image != null && image.Length > 0)
            {
                string filename = image.FileName;
                string filePath = _webHostEnvironment.WebRootPath + $@"\images\{filename}";
                long size = image.Length;

                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    await image.CopyToAsync(fs);
                    await fs.FlushAsync(CancellationToken.None);
                    await fs.DisposeAsync();
                }
                return $@"/images/{filename}";
            }
            else
            {
                return "Opps!!! No image has uploaded. Try again";
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> AddUpdate(TestVM productVM, string actionType, string token)
        {
            if (actionType == "add")
            {
                await _context.Tests.AddAsync(new Test(productVM.Name, productVM.ImageUrl));
                await _context.SaveChangesAsync();
            }

            if (actionType == "edit")
            {
                Test product = new Test(productVM.Id, productVM.Name, productVM.ImageUrl);
                _context.Tests.Update(product);
                await _context.SaveChangesAsync();
            }

            string trsWithTds = string.Empty;
            List<TestVM> productVMs = _context.Tests.Select(x => new TestVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (TestVM aTestVM in productVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + aTestVM.Name + "</td><td class=\"align-middle\"><img src=\"" + aTestVM.ImageUrl + "\" class=\"img-thumbnail\" /></td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + aTestVM.Id + "\" data-image=\"" + aTestVM.ImageUrl + "\" data-name=\"" + aTestVM.Name + "\" onclick=\"editTest(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteTest('" + token + "', " + aTestVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<ContentResult> Delete(int id, string token)
        {
            Test t = await _context.Tests.FindAsync(id);

            _context.Tests.Remove(t);
            await _context.SaveChangesAsync();

            string trsWithTds = string.Empty;
            List<TestVM> productVMs = _context.Tests.Select(x => new TestVM(x.Id, x.Name, x.ImageUrl)).ToList();
            if (productVMs.Count > 0)
            {
                foreach (TestVM aTestVM in productVMs)
                {
                    trsWithTds += "<tr><td class=\"align-middle\">" + aTestVM.Name + "</td><td class=\"align-middle\"><img src=\"" + aTestVM.ImageUrl + "\" class=\"img-thumbnail\" /></td><td><button type=\"button\" class=\"btn btn-sm text-warning fw-bold\" data-id=\"" + aTestVM.Id + "\" data-image=\"" + aTestVM.ImageUrl + "\" data-name=\"" + aTestVM.Name + "\" onclick=\"editTest(this)\">Edit</button><button type=\"button\" class=\"btn btn-sm text-danger fw-bold\" onclick=\"deleteTest('" + token + "', " + aTestVM.Id + ")\">Delete</button></td></tr>";
                }
            }
            return Content(trsWithTds, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
