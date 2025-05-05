using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class StudentResetPassword
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public string ResetKey { get; set; }
        [Required]
        public DateTime CreatedOn { get; set; }
        [Required]
        public int CreatedBy { get; set; }
    }
}
