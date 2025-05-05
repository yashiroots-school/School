using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentFeeInputModel
    {
        public string ScholarNumer { get; set; }
        public string Batch { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }
        public string Section { get; set; }

        public long StudentId { get; set; }
        public string ClassName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}
