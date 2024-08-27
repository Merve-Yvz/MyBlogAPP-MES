using BlogAPP.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogAPP.ViewComponents.Category
{
	public class BlogListByCategory:ViewComponent
	{
		private readonly ApplicationDbContext _db;
        public BlogListByCategory(ApplicationDbContext db)
        {
            _db= db;
            
        }
		public IViewComponentResult Invoke(int categoryID)
		{
			var blogValues = _db.Blogs
						   .Where(c => c.CategoryID == categoryID)
						   .ToList();

			return View(blogValues);
		}
	}
}
