using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_TimeTable
    {
        

        [Key]
        public int TimeTable_Id { get; set; }

        public string Class_Name { get; set; }

        public int Class_Id { get; set; }

        public string Section_Name { get; set; }

        public int Section_Id { get; set; }

        public string Staff_Name { get; set; }

        public int StafId { get; set; }

        public int Room_Id { get; set; }

        public string Room_Name { get; set; }

        public int Day_Time_Id { get; set; }

        public string CreatedDate { get; set; }

        public int Subject_ID { get; set; }

        public string Subject_Name { get; set; }

        public string Date { get; set; }

        public int Day_ID { get; set; }

        public int Time_ID { get; set; }


    }
}