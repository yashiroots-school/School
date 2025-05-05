using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class PromotionReportList
    {
        public int SNO { get; set; }
        public string SemesterName { get; set; }
        public int Promoted { get; set; }
        public int AllStudents { get; set; }
        public int NotPromoted { get; set; }
        public string ClassName { get; set; }
        public int ClassId { get; set; }
    }
}