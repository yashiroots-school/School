using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Caste
    {
        [Key]
        public int Caste_Id { get; set; }

        public string Caste_Name { get; set; }
    }
}