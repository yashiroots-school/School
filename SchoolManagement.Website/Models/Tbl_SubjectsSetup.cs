using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_SubjectsSetup
    {
        [Key]
        public int Subject_ID { get; set; }

        public string Subject_Name { get; set; }
        public bool? IsElective { get; set; }
    }
 
}