using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_StaffCategory
    {
        [Key]
        public int Staff_Category_Id { get; set; }

        public string Category_Name { get; set; }

        public string Created_Date { get; set; }
    }
}