using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentAttendance
    {
        public string BatchName { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; }
        public string AttendanceDate { get; set; }
        public int TotalDays { get; set; }
        public decimal AttendancePercentage { get; set; }
    }
    public class BatchAttendance
    {
        public string BatchName { get; set; }
        public int TotalDaysSum { get; set; }
        public decimal AverageAttendancePercentage { get; set; }
        public List<AttendanceDetail> AttendanceDetails { get; set; }
    }

    public class AttendanceDetail
    {
        public string ClassName { get; set; }
        public string Section { get; set; }
        public int TotalDays { get; set; }
        public decimal AttendancePercentage { get; set; }
    }
    public class StudentMarkBatchWise
    {
        public string BatchName { get; set; }
        public string ClassName { get; set; }
        public string Section { get; set; }
        public string TermName { get; set; }
        public long TermId { get; set; }
        public string TotalObtainedMarks { get; set; }
    }
    public class CoScholasticReportViewModel
    {
        public string BatchName { get; set; }
        public string TermName { get; set; }
        public string ClassName { get; set; }
        public string SectionName { get; set; }
        public string CoscholasticGrades { get; set; }
        public string AvgGrade { get; set; }
    }

}