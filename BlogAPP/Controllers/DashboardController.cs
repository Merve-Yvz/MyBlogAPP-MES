using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;

namespace BlogAPP.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var userID= await _db.Users.Where(x=>x.UserEmail==userEmail).Select(y=>y.UserID).FirstOrDefaultAsync();
            ViewBag.BlogCount = await _db.Blogs.CountAsync();
            ViewBag.AdminBlogCount = await _db.Blogs.Where(c => userID == c.UserID).CountAsync();
            ViewBag.UserCount = await _db.Users.CountAsync();
            
            return View();
        }
        [HttpGet]
        public async Task<PartialViewResult> CategoryBlogCounts()
        {
            var categoryBlogCounts = await _db.Categories
                .Include(c => c.Blogs)
                .Select(c => new BlogCountbyCategory
                {
                    CategoryName = c.CategoryName,
                    BlogCount = c.Blogs.Count()
                }).ToListAsync();

            return PartialView(categoryBlogCounts);
        }


    }
}
