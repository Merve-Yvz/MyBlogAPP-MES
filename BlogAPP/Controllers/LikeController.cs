using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;


namespace BlogAPP.Controllers
{
	public class LikeController : Controller
	{
		private readonly ApplicationDbContext _db;
		private readonly ILogger<Like> _logger;
        private readonly IDistributedCache _cache;
        public LikeController(ApplicationDbContext db, ILogger<Like> logger,IDistributedCache cache)
		{
			_db = db;
			_logger = logger;
            _cache = cache;
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

            
            var userLikesCacheKey = $"UserLikes_{userID}";
            await _cache.RemoveAsync(userLikesCacheKey);

            var blogLikesCacheKey = $"BlogLikes_{blogId}";
            await _cache.RemoveAsync(blogLikesCacheKey);

            return Json(new { success = true, isLiked = isLiked });
        }

        public async Task<IActionResult> GetLikeCount(int blogId)
        {
            var cacheKey = $"BlogLikes_{blogId}";

            var likeCountFromCache = await _cache.GetStringAsync(cacheKey);
            int likeCount;

            if (!string.IsNullOrEmpty(likeCountFromCache))
            {
                likeCount = int.Parse(likeCountFromCache);
            }
            else
            {
                likeCount = await _db.Likes.CountAsync(l => l.BlogID == blogId);

                await _cache.SetStringAsync(cacheKey, likeCount.ToString(), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                });
            }

            return Json(new { count = likeCount });
        }
    }
}