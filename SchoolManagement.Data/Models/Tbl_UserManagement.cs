using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class Tbl_UserManagement
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        //public string UserIdLogin { get; set; }
    }
}
