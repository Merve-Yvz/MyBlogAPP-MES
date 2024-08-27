using BlogAPP.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPP.ViewComponents.Comment
{
	public class AddComment:ViewComponent
	{
		private readonly ApplicationDbContext _db;

		public AddComment(ApplicationDbContext db)
		{
			_db = db;
		}


        public IViewComponentResult Invoke(int blogId)
        {
            //It doesn't save comments, check it last
            var newComment = new BlogAPP.Models.Comment
            {
                BlogID = blogId
            };

            return View(newComment);
        }

        [HttpPost]
        public async Task<IViewComponentResult> AddCommentAsync(BlogAPP.Models.Comment comment)
        {
			
				_db.Comments.Add(comment);
				await _db.SaveChangesAsync();
			return View();
				
		

			
		}
    }
}
