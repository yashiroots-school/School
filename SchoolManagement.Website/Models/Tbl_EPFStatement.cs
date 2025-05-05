using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_EPFStatement
    {
        [Key]
        public int EPFstatement_Id { get; set; }

        public string Employee_Code { get; set; }

        public string UIN { get; set; }

        public string Employee_Name { get; set; }

        public int Gross_Wages { get; set; }

        public int Epf_Wages { get; set; }

        public int EPs_Wages { get; set; }

        public int EDLIWages { get; set; }

        public int Employe_Contribution { get; set; }

        public int Employer_Contribution { get; set; }

        public int EPS_Pension { get; set; }

        public int NCP_Days { get; set; }

        public int Refund_Advances { get; set; }

        public string Added_Date { get; set; }

        public string Added_Day { get; set; }

        public string Added_Month { get; set; }

        public string Added_Year { get; set; }

        public int StaffCategory_Id { get; set; }
    }
}