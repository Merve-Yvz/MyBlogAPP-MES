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
        //[JwtAuthentication]
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

        //public IActionResult Index(int? categoryId)
        //{
        //    var viewModel = new BlogCategoryViewModel
        //    {
        //        CategoryList = _db.Categories.ToList(),
        //        Blogs = categoryId.HasValue
        //      ? _db.Blogs.Include(b => b.Category).Include(b => b.User).Where(b => b.CategoryID == categoryId.Value).ToList()
        //      : _db.Blogs.Include(b => b.Category).Include(b => b.User).ToList()
        //    };

        //    return View(viewModel);

        //}
        /* public IActionResult BlogsByCategory(int? categoryId)
         {
             var viewModel = new BlogCategoryViewModel
             {
                 Blogs = _db.Blogs
                     .Include(b => b.Category)
                     .Where(b => !categoryId.HasValue || b.CategoryID == categoryId.Value)
                     .ToList(),

                 CategoryList = _db.Categories.Select(c => new SelectListItem
                 {
                     Value = c.CategoryID.ToString(),
                     Text = c.CategoryName
                 }).ToList()
             };

             // Ensure CategoryList is not null
             if (viewModel.CategoryList == null)
             {
                 throw new InvalidOperationException("CategoryList cannot be null.");
             }

             return View(viewModel);
         }*/


    }
}
