using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPP.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }

        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }

        public string BlogTitle { get; set; }
        public string? BlogContent { get; set; }

        public string? BlogImage { get; set; } = "/defaultimage.gif";


        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        [ValidateNever]
        public Category? Category { get; set; }

        public bool BlogStatus { get; set; } = true;

        public DateTime Created_at { get; set; } = DateTime.Now;

        List<Comment> Comments { get; set; }
        List<Like> Likes { get; set; }

    }
}
