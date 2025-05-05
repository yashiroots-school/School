using SchoolManagement.Entities;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class StudentRegistrationHistory : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StudentRegisterHistoryID { get; set; }
        public long StudentRegisterID { get; set; }
        [Required(ErrorMessage = "Application No is required")]
        public string ApplicationNumber { get; set; }
        [Required]
        public string UIN { get; set; }
        public string Date { get; set; }
        [Required]
        public string Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Gender { get; set; }
        public string RTE { get; set; }
        public string Medium { get; set; }
        public string Caste { get; set; }
        [Display(Name = "Age In Words :")]
        public int AgeInWords { get; set; }
        [Display(Name = "Date of Birth :")]
        public string DOB { get; set; }

        [Display(Name = "Place Of Birth :")]
        public string POB { get; set; }

        public string Nationality { get; set; }
        public string Religion { get; set; }

        [Display(Name = "Mother tongue :")]
        public string MotherTongue { get; set; }
        public string Category { get; set; }

        [Display(Name = "Blood Group :")]
        public string BloodGroup { get; set; }
        [Display(Name = "Medical History :")]
        public string MedicalHistory { get; set; }
        public string Hobbies { get; set; }
        public string Sports { get; set; }
        [Display(Name = "Any other details :")]
        public string OtherDetails { get; set; }
        public string ProfileAvatar { get; set; }
        public string MarkForIdentity { get; set; }
        public string AdharNo { get; set; }
        public string AdharFile { get; set; }
        public string OtherLanguages { get; set; }
        public bool IsApplyforTC { get; set; }
        public bool IsApplyforAdmission { get; set; }
        public int IsApprove { get; set; }
        public int IsApprovePreview { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAdmissionPaid { get; set; }
        public bool? IsInsertFromAd { get; set; }

        public int Class_Id { get; set; }

        public int Category_Id { get; set; }

        public string Parents_Email { get; set; }

    }
}
