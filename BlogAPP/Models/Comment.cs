using System.ComponentModel.DataAnnotations;

namespace BlogAPP.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }
        public string CommentUserName {  get; set; }    
        public string CommentContent { get; set; }

        public int BlogID { get; set; }
        public Blog Blog { get; set; }

        List<Like> Likes { get; set; }
        public DateTime Created_at { get; set; } = DateTime.Now;
    }
}
