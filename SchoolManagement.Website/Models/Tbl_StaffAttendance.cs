using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Website.Models
{
    public class Tbl_StaffAttendance : BaseEntity
    {
        [Key]
        public int StaffAtte_Id { get; set; }

        public int Staff_Id { get; set; }

        public string Staff_Name { get; set; }

        public string Mark_FullDayAbsent { get; set; }

        public string Mark_HalfDayAbsent { get; set; }

        public string Mark_FullDayPresent { get; set; }
        public string Mark_Other { get; set; }
        public string Mark_CL { get; set; }
        public string Mark_ML { get; set; }
        public string Mark_L { get; set; }

        public string Attendence_Date { get; set; }

        public int Attendence_Day { get; set; }

        public string Attendence_Month { get; set; }

        public string Attendence_Year { get; set; }

      
        public string Total { get; set; }

        public string Employee_Code { get; set; }


    }
}