using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;

namespace BlogAPP.Controllers
{
    //[Authorize]
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BlogController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult GetAllBlogs()
        {
            var blogs = _db.Blogs.Include(b => b.Category) 
                                .Include(b => b.User) 
                                .ToList();

            return View(blogs);
        }

        [HttpGet]
        public IActionResult UpSert(int? id, bool isAdmin = false) // Update Insert
        {
            ViewData["IsAdminLayout"] = isAdmin;

            BlogCategoryViewModel model = new()
            {
                CategoryList = _db.Categories.ToList()
               .Select(u => new SelectListItem
               {
                   Text = u.CategoryName,
                   Value = u.CategoryID.ToString()
               }),
                Blog = new Blog()
            };
            if (id == null || id == 0)
            {
                //create
                return View(model);
            }
            else
            {
                //update
                model.Blog = _db.Blogs.FirstOrDefault(u => id == u.BlogID);
                return View(model);
            }

        }
        [HttpPost]
        public IActionResult Upsert(BlogCategoryViewModel blogVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    // save photo
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    // send to model 
                    blogVM.Blog.BlogImage = fileName;
                }

                //  create or update blog
                if (blogVM.Blog.BlogID == 0)
                {
                   
                    _db.Blogs.Add(blogVM.Blog);
                }
                else
                {
                    
                    _db.Blogs.Update(blogVM.Blog);
                }

                _db.SaveChanges();
                TempData["success"] = "Blog created/updated successfully.";

                return RedirectToAction("Index", "Home");
            }
            else
            {
                blogVM.CategoryList = _db.Categories.ToList()
                .Select(u => new SelectListItem
                {
                    Text = u.CategoryName,
                    Value = u.CategoryID.ToString(),
                });
                return View(blogVM);
            }
        }

        public IActionResult DeleteBlog(int blogID)
        {
            var blog = _db.Blogs.FirstOrDefault(u => u.BlogID == blogID);

            _db.Blogs.Remove(blog);
            _db.SaveChanges();

            return RedirectToAction("GetAllBlogs");
        }
        public IActionResult BlogListByWriter()
        {
            var userEmail = User.Identity.Name;
            var userID =  _db.Users.Where(x => x.UserEmail == userEmail).Select(y => y.UserID).FirstOrDefault();

            var blogs =  _db.Blogs
                                .Include(b => b.Category) 
                                .Include(b => b.User) 
                                .Where(c => userID == c.UserID)
                                .ToList();

            return View(blogs);
        }
    }
}
