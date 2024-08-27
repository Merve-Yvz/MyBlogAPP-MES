using BlogAPP.Data;
using BlogAPP.Models.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogAPP.ViewComponents.Dashboard
{
	public class BlogCountByCategory:ViewComponent
	{
		private readonly ApplicationDbContext _db;
		public BlogCountByCategory(ApplicationDbContext db)
		{
			_db = db;

		}
        public async Task<IViewComponentResult> InvokeAsync(int categoryID)
        {
            var categoryBlogCounts = await _db.Categories
                .Include(c => c.Blogs)
                .Select(c => new BlogCountbyCategory
                {
                    CategoryName = c.CategoryName,
                    BlogCount = c.Blogs.Count()
                }).ToListAsync();

            return View(categoryBlogCounts);
        }
    }
}
