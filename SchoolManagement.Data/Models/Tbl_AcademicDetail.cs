using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public  class tbl_AcademicDetail
    {
        [Key]
        public long AcademicDetailId { get; set; }

        public int NewProperty { get; set; }



        [Required]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(20)]
        public string AcademicYear { get; set; }

        [StringLength(20)]
        public string Qualification { get; set; }

        [StringLength(1024)]
        public string Institution { get; set; }

        [StringLength(1024)]
        public string University { get; set; }


        public decimal Percentage { get; set; }

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


        [StringLength(35)]
        public string Dateon { get; set; }

        [StringLength(256)]
        public string Stream { get; set; }





    }
}
