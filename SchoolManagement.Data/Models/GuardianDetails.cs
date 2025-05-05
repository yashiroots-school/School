using SchoolManagement.Data.Models;
using SchoolManagement.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class GuardianDetails: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string GuardianName { get; set; }
        public string Qualifications { get; set; }
        public string Occupation { get; set; }
        public string Organization { get; set; }
       
        public string Phone { get; set; }
        
        public string Mobile { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string EMail  { get; set; }
        public string AnnualIncome { get; set; }
        public string ResidentialAddress { get; set; }
        public string PermanentAddress { get; set; }
        ////Foreign key for Standard
        //[ForeignKey("Student")]
        public int StudentRefId { get; set; }
        public Student Student { get; set; }
        public string ApplicationNumber { get; set; }
    }
}