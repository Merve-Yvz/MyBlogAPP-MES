using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlogAPP.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserSurname { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }

        public int RoleID { get; set; } = 2;
        [ForeignKey("RoleID")]
        public Role? Role { get; set; }

        public bool UserStatus { get; set; } = true;
        List<Blog> Blogs { get; set; }

        public DateTime Created_at { get; set; } = DateTime.Now;
    }
}
