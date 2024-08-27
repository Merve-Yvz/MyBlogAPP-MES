using BlogAPP.Data;
using BlogAPP.Models;
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
        public async Task<PartialViewResult> AddComment(int blogId, string content, string username)
        {
            var comment = new Comment
            {
                BlogID = blogId,
                CommentContent = content,
                CommentUserName = username
            };

            _db.Comments.Add(comment);
            await _db.SaveChangesAsync();
            return PartialView();
        }
    }
}