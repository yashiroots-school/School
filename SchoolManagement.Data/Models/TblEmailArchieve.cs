using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
   public class TblEmailArchieve: BaseEntity
    {
        [Key]
        public int Email_Id { get; set; }

        public int Student_id { get; set; }

        public string ApplicationNumber { get; set; }

        public string Name { get; set; }

        public string Parent_Email { get; set; }

        public string Email_Date { get; set; }

        public string Email_Content { get; set; }

    }
}
