using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StaffDetailsVM
    {

        public int StafId { get; set; }
        public string ApplicationNumber { get; set; }
        public string UIN { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int AgeInWords { get; set; }
        public DateTime DOB { get; set; }
        public string POB { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Qualification { get; set; }
        public string WorkExperience { get; set; }
        public string MotherTongue { get; set; }
        public string Category { get; set; }
        public string BloodGroup { get; set; }
        public string MedicalHistory { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string BesicSallery { get; set; }
        public string PerksSallery { get; set; }
        public string GrossSallery { get; set; }
        public string LastOrganizationofEmployment { get; set; }
        public string NoofYearsattheLastAssignment { get; set; }
        public string RelievingLetter { get; set; }
        public string PerformanceLetter { get; set; }
        public string File { get; set; }
        public string OtherDetails { get; set; }
        public string EmpId { get; set; }
        public string OtherLanguages { get; set; }
        public string EmpDate { get; set; }
        public string FormalitiesCheck { get; set; }
        public string Designation { get; set; }
        public int NetPay { get; set; }
        public string EmployeeCode { get; set; }
    }
}