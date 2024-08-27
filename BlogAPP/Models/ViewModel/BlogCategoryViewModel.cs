using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BlogAPP.Models.ViewModel
{
    public class BlogCategoryViewModel
    {

        [Required]  
        public Blog Blog { get; set; }
        [ValidateNever]
        public IEnumerable<Blog> Blogs { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
