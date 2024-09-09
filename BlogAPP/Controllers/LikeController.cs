using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace BlogAPP.Controllers
{
	public class LikeController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger<Like> _logger;

		public LikeController(ApplicationDbContext db, ILogger<Like> logger)
		{
			_db = db;
			_logger = logger;

		}

		[HttpPost]
		public async Task<JsonResult> ToggleLike(int blogId)
		{

			var userID = int.Parse(HttpContext.Session.GetString("UserID"));


			var like = await _db.Likes
				.FirstOrDefaultAsync(l => l.BlogID == blogId && l.UserID == userID);
			bool isLiked;
			if (like != null)
			{
				_db.Likes.Remove(like);

				_logger.LogInformation("User with ID {userID} removed the like from blog with ID {blogId}.", userID, blogId);

				isLiked = false;
			}
			else
			{

				_db.Likes.Add(new Like { BlogID = blogId, UserID = userID });
				_logger.LogInformation("User with ID {userID} liked the blog with ID {blogId}.", userID, blogId);

				isLiked = true;
			}

			await _db.SaveChangesAsync();
			return Json(new { success = true, isLiked = isLiked });
		}
		public IActionResult GetLikeCount(int blogId)
		{
			var likeCount = _db.Likes.Count(l => l.BlogID == blogId);
			return Json(new { count = likeCount });
		}
	}
}