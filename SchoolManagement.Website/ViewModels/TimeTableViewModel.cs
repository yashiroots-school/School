using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagement.Website.Models;

namespace SchoolManagement.Website.ViewModels
{
    public class TimeTableViewModel
    {
        public Tbl_SetTime Tbl_SetTime { get; set; }

        public Tbl_TimeTable Tbl_TimeTable { get; set; }
    }
}