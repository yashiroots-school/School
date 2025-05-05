using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Tests
    {
        [Key]
        public long TestID { get; set; }
        public long ClassID { get; set; }
        public long SubjectID { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public decimal MaximumMarks { get; set; }

        public decimal MinimumMarks { get; set; }
        public long TermID { get; set; }
        public long BoardID { get; set; }
        public bool? IsOptional { get; set; }
    }
}