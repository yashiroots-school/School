using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Portions
    {
        [Key]
        public int Portion_Id { get; set; }

        public string Class_Name { get; set; }

        public int Class_Id { get; set; }

        public string Section_Name { get; set; }


        public int Section_Id { get; set; }

        public string Subject_Name { get; set; }

        public int Subject_ID { get; set; }

        public string Portion_Date { get; set; }

        public string Description { get; set; }

        public string CreatedDate { get; set; }
    }
}