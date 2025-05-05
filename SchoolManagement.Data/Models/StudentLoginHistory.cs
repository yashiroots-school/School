using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class StudentLoginHistory
    {
        [Key][Required]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string UserPassword { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
}
