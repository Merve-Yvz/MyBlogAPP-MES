using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using BlogAPP.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogAPP.Controllers
{

    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {       
            _db = db;
            
        }
       
        public IActionResult Index(int categoryID)
        {
            ViewBag.CategoryList = _db.Categories.ToList();
            var blogValues = _db.Blogs
                               .Include(b => b.Category) 
                               .Include(b => b.User)    
                               .Where(c => categoryID == 0 || c.CategoryID == categoryID)
                               .ToList();

            return View(blogValues);
        }

        
    }
}
