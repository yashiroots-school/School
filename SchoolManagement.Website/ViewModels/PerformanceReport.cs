using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class SubjectMarksRangeViewModel
    {
        public string MarksRange { get; set; } // e.g., "91-100"
        public Dictionary<string, int> SubjectCounts { get; set; } = new Dictionary<string, int>();
    }
    public class MarksRangeResult
    {
        public string Subject_Name { get; set; }
        public string MarksRange { get; set; }
        public int Count { get; set; }
    }
    //public class ClassSubjectRangeReportViewModel
    //{
    //    public string RangeLabel { get; set; }
    //    public Dictionary<string, int> ClassSubjectCounts { get; set; } = new Dictionary<string, int>();
    //}
}