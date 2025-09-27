using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Website.Models
{
    [Table("tbl_Notice")]
    public class tbl_Notice
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(255)]
        public string NoticeName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime NoticeDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CurrentDate { get; set; } // save current date+time

        // REMOVE public int DaysAgo { get; set; }
    }
}
