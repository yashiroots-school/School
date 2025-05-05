using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class RolePagePermission
    {
        [Key]
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string PageName { get; set; }
        public bool HasPermission { get; set; }
        public string PageViewName { get; set; }
        public int ParentId { get; set; }
    }
}
