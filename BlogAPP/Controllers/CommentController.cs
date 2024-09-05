using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPP.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CommentController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult AddComment()
        {
            return View();
        }
		[HttpPost]
		public async Task<IActionResult> AddComment(BlogDetailViewModel model)
		{
			if (ModelState.IsValid)
			{
				await _db.Comments.AddAsync(model.Comment);
				await _db.SaveChangesAsync();

				return RedirectToAction("BlogDetails","Home", new { id = model.Comment.BlogID });
			}

			return View();
		}

	}
}