using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
   public  class TblCreateSchool:BaseEntity
    {
        [Key]
        public int School_Id { get; set; }

        public string School_Name { get; set; }

        public string Address { get; set; }

        public string Website { get; set; }

        public string Copyright { get; set; }

        public string Date { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }

        public string Upload_Image { get; set; }

        public string Status { get; set; }
        public long BoardID { get; set; }


    }
}
