using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_BloodGroup
    {
        [Key]
        public int BloodGroup_Id { get; set; }

        public string Blood_Group { get; set; }
    }
}