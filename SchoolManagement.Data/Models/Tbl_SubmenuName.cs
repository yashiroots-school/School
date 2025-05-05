using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
   public  class Tbl_SubmenuName:BaseEntity
    {
        [Key]
        public int Submenu_Id { get; set; }

        public string Submenu_Name { get; set; }

        public string Submenu_Url { get; set; }

        public int Menu_Id { get; set; }

        public bool Submenu_permission { get; set; }

    }

}
