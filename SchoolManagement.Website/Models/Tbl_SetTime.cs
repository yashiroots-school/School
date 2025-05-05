using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_SetTime
    {
        [Key]
        public int Time_Id { get; set; }

        public string Time { get; set; }
    }
}