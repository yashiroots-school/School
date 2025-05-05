using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StaffAttendenceViewmodel
    {
        public int Staff_Id { get; set; }

        public string[] Staff_Name { get; set; }

        public string[] FullDay_Absent { get; set; }

        public string[] HalfDay_Absent { get; set; }

        public string[] Staffid { get; set; }

        public string Fromdate { get; set; }

        public string Todate { get; set; }

        public string[] MarkAttendance { get; set; }

    }
}