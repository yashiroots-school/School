using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class AllStudentDetailsViewModel
    {
        public int StudentId { get; set; }


        public string ApplicationNumber { get; set; }


        public string UIN { get; set; }

        public string Date { get; set; }

        public string Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Gender { get; set; }

        public string RTE { get; set; }
        public string Medium { get; set; }
        public string Caste { get; set; }
        public int AgeInWords { get; set; }

        public string DOB { get; set; }

        public string POB { get; set; }

        public string Nationality { get; set; }
        public string Religion { get; set; }

        public string MotherTongue { get; set; }
        public string Category { get; set; }

        public string BloodGroup { get; set; }

        public string MedicalHistory { get; set; }

        public string Hobbies { get; set; }
        public string Sports { get; set; }

        public string OtherDetails { get; set; }
        public string ProfileAvatar { get; set; }
        public string MarkForIdentity { get; set; }
        public string AdharNo { get; set; }
        public string AdharFile { get; set; }
        public string OtherLanguages { get; set; }
        public bool IsApplyforTC { get; set; }
        public bool IsApplyforAdmission { get; set; }
        public bool IsApprove { get; set; }
        public string Iapprovestudent { get; set; }
        public bool IsActive { get; set; }
        public bool? IsAdmissionPaid { get; set; }
        public bool? IsInsertFromAd { get; set; }
        public string RegNumber { get; set; }

        public int Class_Id { get; set; }
        public int Category_Id { get; set; }
        public int Batch_Id { get; set; }
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

        public string FatherName { get; set; }

        public string MotherName { get; set; }

        public string FMobile { get; set; }

        public string MMobile { get; set; }

        public string FResidentialAddress { get; set; }

        public string Added_Year { get; set; }

        public string Promotion_Year { get; set; }

        public string Promotion_Date { get; set; }

        public DateTime Added_Date { get; set; }

        public string Registration_Date { get; set; }

        public long StudentRegisterID { get; set; }

        public string BatchName { get; set; }

        public string Tappermission { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

        public long? RollNo { get; set; }
        public long? ScholarNo { get; set; }
    }
}