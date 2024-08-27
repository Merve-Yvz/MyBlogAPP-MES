using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPP.Models
{
    public class Like
    {
        [Key]
        public int LikeID { get; set; }

        public int CommentID { get; set; }
        [ForeignKey("CategoryID")]
        public Comment Comment { get; set; }

        public int BlogID { get; set; }
        [ForeignKey("BlogID")]
        public Blog Blog { get; set; }

        public DateTime Created_at { get; set; } = DateTime.Now;
    }
}
