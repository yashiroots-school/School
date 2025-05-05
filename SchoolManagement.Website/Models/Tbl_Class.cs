using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Class
    {

        [Key]
        public int Class_Id { get; set; }

        public string Class_Name { get; set; }

    }
}