using System.ComponentModel.DataAnnotations;

namespace BlogAPP.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public  List<Blog> Blogs { get; set; }
    }
}
