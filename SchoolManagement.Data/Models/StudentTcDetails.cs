using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class StudentTcDetails
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("Student")]
        public int StudentId { get; set; }
        public Student Student { get; set; }
        public int ClassId { get; set; }
        public int BatchId { get; set; }
        public int ExamStatusId { get; set; }
        public int? PromoteClassId { get; set; }
        public int? PromoteSectionId { get; set; }
        public string Remark { get; set; }
        public string FeePaidUpto { get; set; }
        public string OtherRemarks { get; set; }
        public string TotalAttendance { get; set; }
        public string TotalWorkingDays { get; set; }
        public string Reason { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Ispaid { get; set; }
        public long TcId { get; set; }
        public DateTime? SchoolLeftDate { get; set; }
        public int? RemarksID { get; set; }
        public int? ReasonID { get; set; }
        public string TCSNo { get; set; }
    }
    public class StudentTcMaster 
    {
    [Key]
    public int Id { get; set; }
    public string SchoolCode { get; set; }
    public int TcSeriesStartNo {  get; set; }
    public long TcSeriesCurrentNo { get; set; }
    }

}
