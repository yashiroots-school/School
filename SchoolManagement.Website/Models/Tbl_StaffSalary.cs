using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_StaffSalary
    {
        [Key]
        public int Salary_Id { get; set; }

        public int Staff_ID { get; set; }

        public string Staff_Name { get; set; }

        public int Salary_Amount { get; set; }

        public string CreatedDate { get; set; }

        public int Basic_Amount { get; set; }

    }
}