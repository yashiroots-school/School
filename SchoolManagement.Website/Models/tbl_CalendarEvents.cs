using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Website.Models
{
    [Table("tbl_CalendarEvents")]
    public class tbl_CalendarEvents
    {
        [Key]
        public int ID { get; set; }   // Primary key

        [Required]
        [StringLength(255)]
        public string EventName { get; set; }  // Event ka naam

        [Required]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; } // Date

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EventTime { get; set; } // Time
    }
}
