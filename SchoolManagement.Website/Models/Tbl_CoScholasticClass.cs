using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_CoScholasticClass
    {
        [Key]
        public long Id { get; set; }
        public long BoardID { get; set; }
        public long ClassID { get; set; }
        public long CoscholasticID { get; set; }
        public string ClassName { get; set; }
    }
}