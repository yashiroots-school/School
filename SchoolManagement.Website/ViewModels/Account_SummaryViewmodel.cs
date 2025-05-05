using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class Account_SummaryViewmodel
    {
        public int Summary_Id { get; set; }

        public int Staff_Id { get; set; }

        public string Staff_Name { get; set; }

        public int NetPay { get; set; }

        public int PF { get; set; }

        public int Basic_Salary { get; set; }

        public int DA { get; set; }

        public int Professional_Tax { get; set; }

        public string Added_Date { get; set; }

        public string Added_Month { get; set; }

        public string Added_Year { get; set; }

        public string Added_Day { get; set; }

        public int Employee_Contribution { get; set; }

        public int Employer_Contribution { get; set; }

        public int Net_Pay { get; set; }

        public int Attendence_Percentage { get; set; }

        public int ESI { get; set; }

        public int Gross { get; set; }

        public int Total_Salary { get; set; }

        public double LOP { get; set; }

        public int CCA { get; set; }

        public int HRA { get; set; }

        public int OtherALlowance { get; set; }

        public string NoOfdayspresent { get; set; }

        public int TotalPercentage { get; set; }

        public int Deduction_Amt { get; set; }

        public int Arrear_Amt { get; set; }

        public string Arrear { get; set; }


        public int Deductions_Id { get; set; }
        public string DeductionRemarks { get; set; }

        public int Arrear_Id { get; set; }
        public string Arrear_remarks { get; set; }

        public string Pastarreardate { get; set; }
        public int Pastarrearamt { get; set; }
        public int Pastdeductionamt { get; set; }
    }
}