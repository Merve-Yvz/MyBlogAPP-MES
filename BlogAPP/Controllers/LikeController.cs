﻿using BlogAPP.Data;
using BlogAPP.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPP.Controllers
{
    public class LikeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LikeController(ApplicationDbContext db)
        {
            _db = db;
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
                isLiked = false;
            }
            else
            {
              
                _db.Likes.Add(new Like { BlogID = blogId, UserID = userID });
                isLiked = true;
            }

            await _db.SaveChangesAsync();
            return Json(new { success = true, isLiked = isLiked });
        }
    }
}