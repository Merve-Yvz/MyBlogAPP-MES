using System.ComponentModel.DataAnnotations;

namespace BlogAPP.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<User> Users { get; set; }
    }
}
