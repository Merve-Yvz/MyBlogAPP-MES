using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogAPP.Controllers
{
    public class RegisterController : Controller
    {
        private readonly ApplicationDbContext _dbContext;
        public RegisterController(ApplicationDbContext dBContext)
        {
            this._dbContext = dBContext;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            
            if (ModelState.IsValid)
            {
                await _dbContext.Users.AddAsync(user);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("UserList");

            }
        
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UserList()
        {
            var user = await _dbContext.Users.ToListAsync();
            return View(user);
        }
    }
}
