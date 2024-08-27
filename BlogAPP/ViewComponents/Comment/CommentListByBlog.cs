using BlogAPP.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogAPP.ViewComponents.Comment
{
	public class CommentListByBlog:ViewComponent
	{
		private readonly ApplicationDbContext _db;

		public CommentListByBlog(ApplicationDbContext db)
		{
			_db = db;
		}


		public IViewComponentResult Invoke(int blogId)
		{
			var commentValues = _db.Comments
						   .Where(c => c.BlogID == blogId)
						   .ToList();

			return View(commentValues);
		}
    }
}
