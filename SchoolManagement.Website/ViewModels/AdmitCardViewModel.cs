using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentDto
    {
        public long StudentId { get; set; }
        public string ApplicationNumber { get; set; }
        public string UIN { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string BatchName { get; set; }

        public string Section { get; set; }
        public string Gender { get; set; }
        public string DOB { get; set; }
        public long? RollNo { get; set; }
        public long? ScholarNo { get; set; }
        public string ProfileAvatar { get; set; }
        public string Mobile { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string RegNumber { get; set; }
        // aur jo bhi extra fields chahiye aap rakh sakte ho
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string staf { get; set; }
        public string TermName { get; set; }
    }

    public class TestDto
    {
        public long TestID { get; set; }
        public string TestName { get; set; }
        public string TestType { get; set; }
        public int MaximumMarks { get; set; }
        public int MinimumMarks { get; set; }
        public string ExamDate { get; set; }
        public string ExamTime { get; set; }
    }
    public class AdmitsCardViewModel
    {
        public StudentDto Student { get; set; }
        public List<TestDto> Tests { get; set; } = new List<TestDto>();
        // You can add other properties like Rank, Remarks, etc.
    }
}