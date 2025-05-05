using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_ExamTypes
    {
        [Key]
        public int Exam_Id { get; set; }

        public string Exam_Type { get; set; }
    }
}