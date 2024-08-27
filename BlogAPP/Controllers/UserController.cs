using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPP.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;   
        }
        public IActionResult Index(bool isAdmin = false)
        {
            ViewData["IsAdminLayout"] = isAdmin;
            var users = _db.Users.Include(b => b.Role).ToList();
            return View(users);
        
        }
        public IActionResult AdminPanel()
        {
            return View();
        }
        public PartialViewResult UserNavbarPartial()
        {
            return PartialView();   
        }

    }
}
