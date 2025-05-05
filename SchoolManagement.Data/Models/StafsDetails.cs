using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class StafsDetails: BaseEntity
    {
        [Key]
        public int StafId { get; set; }
      
        [Display(Name = "UIN (Unique identification No) :")]
        public string UIN { get; set; }

        public string Date { get; set; } 
        //[Required]
        public string Name { get; set; }
        //[Required]
      
        public string Gender { get; set; }

        [Display(Name = "Age In Words :")]
        public int AgeInWords { get; set; }

        [Display(Name = "Date of Birth :")]
        public string DOB { get; set; }

        [Display(Name = "Place Of Birth :")]
        public string POB { get; set; }

        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Qualification { get; set; }
        public string WorkExperience { get; set; }

        [Display(Name = "Mother tongue :")]
        public string MotherTongue { get; set; }
        public string Category { get; set; }

        [Display(Name = "Blood Group :")]
        public string BloodGroup { get; set; }

        [Display(Name = "Medical History :")]
        public string MedicalHistory { get; set; }

        public string Address { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string BesicSallery { get; set; }
        public string PerksSallery { get; set; }
        public string GrossSallery { get; set; }
        public string FatherOrHusbandName { get; set; }
        public string MothersName { get; set; }
        public string MariedStatus { get; set; }
        public string Children { get; set; }
        public string BesicSallery1 { get; set; }
        public string PerksSallery1 { get; set; }
        public string GrossSallery1 { get; set; }
        public string Caste { get; set; }
        public string DateofReliving { get; set; }
        public string LastOrganizationofEmployment { get; set; }
        public string NoofYearsattheLastAssignment { get; set; }
        public string AdharNo { get; set; }
        public string AdharFile { get; set; }
        public string PanNo { get; set; }
        public string PanFile { get; set; }
         
        public string BankACNo { get; set; }
        public string RelievingLetter { get; set; }
        public string PerformanceLetter  { get; set; }
        public string File { get; set; }
        public string StaffSignatureFile { get; set; }

        [Display(Name = "Any other details :")]
        public string OtherDetails { get; set; }
        public string EmpId { get; set; }
        public string OtherLanguages { get; set; }
        public string EmpDate { get; set; }
        public string FormalitiesCheck { get; set; }
        public string Designation { get; set; }
        public string EmployeeCode { get; set; }

        public string Bank_Name { get; set; }
        public string Account_No { get; set; }
        public string IFSC_Code { get; set; }
        public string Employee_Designation { get; set; }
        public int Employee_AccountId { get; set; }
        public string Employee_AccountName { get; set; }
        public int Category_Id { get; set; }
        public string Staff_CategoryName { get; set; }
        public bool? IsActive { get; set; }
        public int?  StaffCategory { get; set; }
    }
}
