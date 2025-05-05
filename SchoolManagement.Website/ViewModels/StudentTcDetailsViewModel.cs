using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentTcDetailsViewModel
    {
        public int StudentId { get; set; }
        //public int SectionId { get; set; }
        public int ClassId { get; set; }
        public int BatchId { get; set; }
        public int ExamStatusId { get; set; }
        public int? PromoteClassId { get; set; }
        public int? PromoteSectionId { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public DateTime SchoolLeftDate { get; set; }
        public string FeePaidUpto { get; set; }
        public string OtherRemarks { get; set; }
        public string TotalAttendance { get; set; }
        public string TotalWorkingDays { get; set; }
        public int? RemarksID { get; set; }
        public int? ReasonID { get; set; }
    }

    
}