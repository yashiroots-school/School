using SchoolManagement.Entities;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class Student : BaseEntity
    {
        [Key]
        public int StudentId { get; set; }
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
        public DateTime AddedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string IP { get; set; }
        public string UserId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreateBy { get; set; }
        public int CurrentYear { get; set; }
        public string InsertBy { get; set; }
        public string MarkForIdentity { get; set; }
        public string OtherLanguages { get; set; }
        public string Medium { get; set; }
        public string Caste { get; set; }
        public string RTE { get; set; }
        public string AdharNo { get; set; }
        public string AdharFile { get; set; }
        public string BatchName { get; set; }
        public bool IsApplyforTC { get; set; }
        public bool IsApplyforAdmission { get; set; }
        public int IsApprove { get; set; }
        public bool IsActive { get; set; }
        public bool? IsInsertFromAd { get; set; }
        public bool? IsAdmissionPaid { get; set; }
        public string RegNumber { get; set; }
        public int Class_Id { get; set; }
        public int Section_Id { get; set; }
        public int Category_Id { get; set; }
        public int Batch_Id { get; set; }
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        public string ParentEmail { get; set; }
        public string AdmissionFeePaid { get; set; }
        public string Last_Name { get; set; }
        public string Transport { get; set; }
        public string Transport_Options { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public int BloodGroup_Id { get; set; }
        public bool isPromoted { get; set; }
        public long? RollNo { get; set; }
        public long? ScholarNo { get; set; }

        public ICollection<GuardianDetails> GuardianDetails { get; set; }
        public ICollection<FamilyDetail> FamilyDetails { get; set; }
        public ICollection<AdditionalInformation> AdditionalInformations { get; set; }
        public ICollection<PastSchoolingReport> PastSchoolingReports { get; set; }
        public ICollection<StudentRemoteAccess> StudentRemoteAccess { get; set; }

    }
}
