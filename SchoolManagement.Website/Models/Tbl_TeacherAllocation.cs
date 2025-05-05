using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_TeacherAllocation
    {
        [Key]
        public int Allocate_Id { get; set; }

        public string Teacher_Name { get; set; }

        public string Class_Name { get; set; }

        public string Subject_Name { get; set; }

        public int StaffId { get; set; }

        public int Class_Id { get; set; }

        public int Subject_ID { get; set; }

    }
}