using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BlogAPP.Controllers
{
  
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;   
        }
        public IActionResult Index()
        {
            
            var users = _db.Users.Include(b => b.Role).ToList();
            return View(users);
        
        }
        [HttpGet]
        public IActionResult UserCreate()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UserCreate(User model)
        {
            if (ModelState.IsValid)
            {
                await _db.Users.AddAsync(model);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View();
        }
        public PartialViewResult AdminNavbarPartial()
        {
            return PartialView();   
        }
        public IActionResult DeleteUser(int userID)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserID == userID);

            _db.Users.Remove(user);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult UserEdit(int id)
        {
            
            var user = _db.Users.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult UserEdit(User model)
        {
            if (ModelState.IsValid)
            {
              
                _db.Users.Update(model);
                _db.SaveChanges();

                return RedirectToAction("Index"); 
            }

            return View(model); 
        }


    }
}
