using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
    public class Tbl_RolePermissionNew : BaseEntity
    {
        [Key]
        public int Rolepermission_Id { get; set; }

        public string Role_Id { get; set; }

        public int Menu_Id { get; set; }

        public int Submenu_Id { get; set; }

        public string Submenu_Url { get; set; }

        public bool Create_permission { get; set; }

        public bool Edit_Permission { get; set; }

        public bool Update_Permission { get; set; }

        public bool Delete_Permission { get; set; }

        public string Submenu_Name { get; set; }

        public bool Submenu_permission { get; set; }

        public int Staff_Id { get; set; }

        public string Staff_Name { get; set; }

    }
}
