using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolManagement.Entities;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class Tbl_MenuName : BaseEntity
    {
        [Key]
        public int Menu_Id { get; set; }
            
        public string Menu_Name { get; set; }

        public string Upload_Image { get; set; }

    }
}
