using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_WeekDays
    {
        [Key]
        public int Day_Id { get; set; }

        public string Week_day { get; set; }
    }
}