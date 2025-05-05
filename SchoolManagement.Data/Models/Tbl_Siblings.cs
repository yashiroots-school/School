using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
    public class Tbl_Siblings:BaseEntity
    {
        [Key]
        public int? Siblings_Id { get; set; }

        public int? Student_Id { get; set; }

        public string Studentname { get; set; }

        public int? Class_id { get; set; }

        public string Confirmation { get; set; }

        public int? FamilyDetails_Id { get; set; }

    }
}
