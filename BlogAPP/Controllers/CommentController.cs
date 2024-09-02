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
        public IActionResult Index()
        {
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> AddComment(Comment model)
		{
			if (ModelState.IsValid)
			{
				_db.Comments.Add(model);
				await _db.SaveChangesAsync();

				return RedirectToAction("BlogDetails", "Home", new { id = model.BlogID });
			}
			// Hata durumunda, blog detay sayfasını aynı model ile yeniden yükleyebilirsiniz.
			return View("BlogDetails", model);
		}

	}
}