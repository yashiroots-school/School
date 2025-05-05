using SchoolManagement.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentAdmissionViewModel
    {
        [Required]
        public string Name { get; set; }
        public string StudentId { get; set; }
        [Required]
        public string Class { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string DOB { get; set; }
        [Required]
        public string POB { get; set; }
        [Required]
        public string Nationality { get; set; }
        [Required]
        public string MotherTongue { get; set; }
        [Required]
        public string Religion { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public string Caste { get; set; }
        public string BloodGroup { get; set; }
        public bool IsApproved { get; set; }
        public HttpPostedFileBase ProfileAvatar { get; set; }
        [Required]
        public string Email { get; set; }
        public string LaststudiedSchoolName { get; set; }
        public FamilyDetail FamilyDetail { get; set; }
    }
}