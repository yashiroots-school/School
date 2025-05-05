using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class GradingCriteria
    {
        [Key]
        public long CriteriaID { get; set; }
        public decimal MinimumPercentage { get; set; }
        public decimal MaximumPercentage { get; set; }
        public string Grade { get; set; }
        public string GradeDescription { get; set; }
        public long BoardID { get; set; }
      //  public long TestID { get; set; }
        public long ClassID { get; set; }
        public long BatchID { get; set; }
        public long TermID { get; set; }
        //public string TestName { get; set; }
        //public string ClassName { get; set; }
        //public string BatchName { get; set; }






    }

    public class GradingCriteriaModel
    {
        public long CriteriaID { get; set; }
        public decimal MinimumPercentage { get; set; }
        public decimal MaximumPercentage { get; set; }
        public string Grade { get; set; }
        public string GradeDescription { get; set; }
        public long BoardID { get; set; }
        public long TestID { get; set; }
        public long ClassID { get; set; }
        public long BatchID { get; set; }
        public string TestName { get; set; }
        public string ClassName { get; set; }
        public string BatchName { get; set; }
    }
    [Table("StudentAttendanceCount")]
    public class StudentAttendanceCount
    {
        [Key]
        public long StudentId { get; set; }
        public string Name { get; set; }
        public string Last_Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public int PresentDays { get; set; }
        //  public long TestID { get; set; }
        public int TotalDays { get; set; }

    }
}