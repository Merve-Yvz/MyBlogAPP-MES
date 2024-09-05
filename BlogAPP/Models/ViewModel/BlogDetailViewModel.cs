using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace BlogAPP.Models.ViewModel
{
	public class BlogDetailViewModel
	{
		public Comment Comment { get; set; }
		
		public Blog? Blog { get; set; }
	}
}
