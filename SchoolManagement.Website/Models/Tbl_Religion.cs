using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Religion
    {
        [Key]
        public int Religion_Id { get; set; }

        public string Religion_Name { get; set; }
    }
}