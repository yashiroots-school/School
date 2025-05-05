using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Category
    {
        [Key]
        public int Category_Id { get; set; }

        public string Category_Name { get; set; }
    }
}