
using BlogAPP.Data;
using BlogAPP.Models;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Security.Claims;

namespace BlogAPP.Controllers
{

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IDistributedCache _cache;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db, IDistributedCache cache)
        {
            _db = db;
            _logger = logger;
            _cache = cache;
        }


        public async Task<IActionResult> Index()
        {
            //var userEmail = User.Identity.Name;
            //var user = _db.Users
            //          .FirstOrDefault(u => u.UserEmail == userEmail);
            var userName = HttpContext.Session.GetString("UserName");
            var userSurname = HttpContext.Session.GetString("UserSurname");
            var userIDString = HttpContext.Session.GetString("UserID");

            ViewBag.UserFullName = userName + " " + userSurname;
            var blogs = await _db.Blogs
                                 .Include(b => b.Category)
                                 .Include(b => b.User)
                                 .OrderByDescending(b => b.Created_at) // sort by creation date
                                 .Take(3) // latest 3 blogs
                                 .ToListAsync();
           



            return View(blogs);
        }
		



		public async Task<IActionResult> BlogDetails(int id)
		{
			var userIDString = HttpContext.Session.GetString("UserID");
			var blogDetail = await _db.Blogs
				.Include(b => b.Likes)
				.Include(b => b.Comments)
				.FirstOrDefaultAsync(b => b.BlogID == id);

			if (blogDetail == null)
			{
				return NotFound();
			}

			var model = new BlogDetailViewModel
			{
				Blog = blogDetail
			};

			if (userIDString != null)
			{
				var userID = int.Parse(userIDString);
				var userLikesCacheKey = $"UserLikes_{userID}";

				var likedBlogIdsFromCache = await _cache.GetStringAsync(userLikesCacheKey);
				List<int> likedBlogIds;

				if (!string.IsNullOrEmpty(likedBlogIdsFromCache))
				{
					try
					{
						likedBlogIds = JsonConvert.DeserializeObject<List<int>>(likedBlogIdsFromCache) ?? new List<int>();
					}
					catch (JsonException)
					{
						likedBlogIds = new List<int>();
					}
				}
				else
				{
					likedBlogIds = await _db.Likes
						.Where(l => l.UserID == userID)
						.Select(l => l.BlogID)
						.ToListAsync();

					try
					{
						var userLikesToCache = JsonConvert.SerializeObject(likedBlogIds);
						await _cache.SetStringAsync(userLikesCacheKey, userLikesToCache, new DistributedCacheEntryOptions
						{
							AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
						});
					}
					catch (JsonException ex)
					{
						Console.WriteLine($"Serialization error: {ex.Message}");
					}
				}

				ViewBag.LikedBlogIds = likedBlogIds;
			}
			else
			{
				ViewBag.LikedBlogIds = new List<int>();
			}

			return View(model);
		}





    }
}
