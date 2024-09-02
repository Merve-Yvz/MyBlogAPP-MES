using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace BlogAPP.Controllers
{
    [AllowAnonymous]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            //var userEmail = User.Identity.Name;
            //var user = _db.Users
            //          .FirstOrDefault(u => u.UserEmail == userEmail);
            var userName = HttpContext.Session.GetString("UserName");
            var userSurname = HttpContext.Session.GetString("UserSurname");
            var userID = int.Parse(HttpContext.Session.GetString("UserID"));

            ViewBag.UserFullName = userName +" "+ userSurname;
            var blogs = await _db.Blogs
                                 .Include(b => b.Category) 
                                 .Include(b => b.User) 
                                 .OrderByDescending(b => b.Created_at) // sort by creation date
                                 .Take(3) // latest 3 blogs
                                 .ToListAsync();

            var likedBlogIds = await _db.Likes
                                .Where(l => l.UserID == userID)
                                .Select(l => l.BlogID)
                                .ToListAsync();

            ViewBag.LikedBlogIds = likedBlogIds;


            return View(blogs);
        }

        public IActionResult BlogDetails(int id)
        {
            Blog model = new Blog();

            model = _db.Blogs.FirstOrDefault(u => id == u.BlogID);
            return View(model);
        }
      

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
