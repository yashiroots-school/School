using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class UserRolesTable
    {
        [Key]
        public int UserRoleId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public string UserIDHash { get; set; }
        public string Email { get; set; }
        public string PassWordHash { get; set; }
    }
}
