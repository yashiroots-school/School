using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public  class tbl_StudentDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long StudentId { get; set; }

        [Key]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(256)]
        public string StudentName { get; set; }

        [StringLength(256)]
        public string Course { get; set; }
        [StringLength(50)]
        public string Class { get; set; }

        [StringLength(20)]
        public string Years { get; set; }

        [StringLength(20)]
        public string Batch { get; set; }

        [StringLength(256)]
        public string Specialization { get; set; }


        [StringLength(256)]
        public string Sibiling1 { get; set; }

        [StringLength(256)]
        public string Sibiling2 { get; set; }

        [StringLength(256)]
        public string Sibiling3 { get; set; }


        [StringLength(256)]
        public string Sibiling4 { get; set; }

        [StringLength(256)]
        public string Sibiling5 { get; set; }


        [StringLength(256)]
        public string Category { get; set; }

        [StringLength(256)]
        public string FacultyMentor { get; set; }

        [StringLength(256)]
        public string DateofBirth { get; set; }

        [StringLength(5)]
        public string Age { get; set; }

        [StringLength(256)]
        public string Gender { get; set; }

        [StringLength(512)]
        public string CorrespondenceAddress { get; set; }

        [StringLength(256)]
        public string ResidenceLocation { get; set; }

        [StringLength(10)]
        public string CountryCode { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(256)]
        public string EmailId { get; set; }

        [StringLength(256)]
        public string OutStationStudent { get; set; }

        [StringLength(256)]
        public string NativePlace { get; set; }

        [StringLength(256)]
        public string Hostalite { get; set; }

        [StringLength(256)]
        public string FatherName { get; set; }

        [StringLength(256)]
        public string FatherProfession { get; set; }

        [StringLength(10)]
        public string FatherCountryCode { get; set; }

        [StringLength(256)]
        public string FatherMobileNo { get; set; }

        [StringLength(256)]
        public string FatherEmailId { get; set; }

        [StringLength(256)]
        public string FatherCompanyName { get; set; }

        [StringLength(256)]
        public string MotherName { get; set; }

        [StringLength(256)]
        public string MotherProfession { get; set; }

        [StringLength(10)]
        public string MotherCountryCode { get; set; }

        [StringLength(256)]
        public string MotherMobileNo { get; set; }

        [StringLength(256)]
        public string MotherEmailId { get; set; }

        [StringLength(256)]
        public string MotherCompanyName { get; set; }

        [StringLength(20)]
        public string status { get; set; }

        [StringLength(20)]
        public string Addedon { get; set; }

        [StringLength(20)]
        public string Addeby { get; set; }

        [StringLength(20)]
        public string Updatedon { get; set; }

        [StringLength(20)]
        public string Updatedby { get; set; }

        [StringLength(35)]
        public string Spare1 { get; set; }

        [StringLength(35)]
        public string Spare2 { get; set; }

        [StringLength(35)]
        public string Spare3 { get; set; }

        [StringLength(256)]
        public string CMCRemarks { get; set; }


        [StringLength(50)]
        public string DateOn { get; set; }

        [StringLength(50)]
        public string Religious { get; set; }
        [StringLength(50)]
        public string ReligiousOther { get; set; }

        public int Class_Id { get; set; }

        public int Batch_Id { get; set; }
    }
}
