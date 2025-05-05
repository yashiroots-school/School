using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public  class tbl_WorkExperience
    {
        [Key]
        public long WorkExperienceId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(5)]
        public string TotalExperience { get; set; }

        [StringLength(512)]
        public string CompanyName { get; set; }

        [StringLength(512)]
        public string CompanyProfile { get; set; }

        [StringLength(256)]
        public string Designation { get; set; }


        public int FromDate { get; set; }

        [StringLength(20)]
        public string ToDate { get; set; }

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

        //public virtual tbl_StudentDetail tbl_StudentDetail { get; set; }
    }
}
