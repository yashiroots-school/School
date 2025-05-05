using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class SchoolBoards
    {
        [Key]
        public long BoardID { get; set; }
        public string BoardName { get; set; }
    }
}