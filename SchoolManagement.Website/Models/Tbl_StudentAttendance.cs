using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_StudentAttendance
    {
        [Key]
        public int Attendance_Id { get; set; }

        public int Class_Id { get; set; }

        public int Section_Id { get; set; }

        public string Class_Name { get; set; }

        public string Section_Name { get; set; }

        public string Mark_FullDayAbsent { get; set; }

        public string Mark_HalfDayAbsent { get; set; }

        public int StudentRegisterID { get; set; }

        public string Student_Name { get; set; }

        public string Created_Date { get; set; }

        public string Day { get; set; }

        public string Created_By { get; set; }

        public string Others { get; set; }
        public int? BatchId { get; set; }
    }
}