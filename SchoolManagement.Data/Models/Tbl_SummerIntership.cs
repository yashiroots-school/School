using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class tbl_SummerInternship
    {
        [Key]
        public long SummerInternshipId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(512)]
        public string CompanyName { get; set; }

        [StringLength(20)]
        public string StartDate { get; set; }

        [StringLength(20)]
        public string MobileNo { get; set; }

        [StringLength(20)]
        public string EndDate { get; set; }

        [StringLength(256)]
        public string ProjectTitle { get; set; }

        [StringLength(256)]
        public string FacultyProjectGuide { get; set; }

        [StringLength(20)]
        public string FacultyGuideMobileNo { get; set; }

        [StringLength(256)]
        public string IndustryGuideName { get; set; }

        [StringLength(256)]
        public string IndustryGuideDesignation { get; set; }

        [StringLength(20)]
        public string IndustryGuideTelNo { get; set; }

        [StringLength(20)]
        public string IndustryGuideMobileNo { get; set; }

        [StringLength(512)]
        public string IndustryGuideEmail { get; set; }

        [StringLength(20)]
        public string StipendinThousands { get; set; }

        [StringLength(512)]
        public string ProjectDescription { get; set; }

        [StringLength(10)]
        public string ProjectSubmission { get; set; }

        [StringLength(512)]
        public string ReasonforNoSubmission { get; set; }

        [StringLength(10)]
        public string PrePlacementOfferReceived { get; set; }

        [StringLength(512)]
        public string Feedback { get; set; }

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

        public virtual tbl_StudentDetail tbl_StudentDetail { get; set; }
    }
}
