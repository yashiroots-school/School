using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_SalaryStatement
    {
        [Key]
        public int SalaryStatement_Id { get; set; }

        public string Employers_Designation { get; set; }

        public string Employee_Name { get; set; }

        public int Employee_Code { get; set; }

        public string Employee_AccountNo { get; set; }

        public string Total_Salary { get; set; }

        public int AccountDetails_Id { get; set; }

        public string Account_Details { get; set; }

        public string Salarystatement_Month { get; set; }

        public string Salarystatement_year { get; set; }

        public string SalaryStatement_Date { get; set; }

        public int StaffCategory_Id { get; set; }
    }
}