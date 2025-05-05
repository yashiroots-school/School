using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_SectionSetup
    {
        [Key]
        public int Section_Id { get; set; }

        public string Section { get; set; }

        public string Class { get; set; }

        public string Class_Id { get;set; }
    }
}