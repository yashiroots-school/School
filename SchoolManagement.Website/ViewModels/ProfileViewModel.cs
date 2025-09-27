using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudnetsDetailsView
    {
        public long SerialNumber { get; set; } = 0;
        public int StudentID { get; set; } = 0;
        public long ScholarNo { get; set; } =0;
        public string Name { get; set; } = string.Empty;
        public string Last_Name { get; set; } = string.Empty;
        public string School { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string FMobile { get; set; } = string.Empty;
        public string FResidentialAddress { get; set; } = string.Empty;
        public string AdharNo { get; set; } = string.Empty;
        public string FEMail { get; set; } = string.Empty;
        public string Class { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public string CastName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Religion { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string DOB { get; set; } = string.Empty;
        public int CurrentYear { get; set; } = 0;
        public string ParentEmail { get; set; } = string.Empty;
        public string BloodGroup { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string STATE { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;
        public string AdmissionDate { get; set; } = string.Empty;
        public string Promotion_Date { get; set; } = string.Empty;
        public string SSSMIdNumber { get; set; } = string.Empty;
        public string BankAccount { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string BankACHolder { get; set; } = string.Empty;
        public string BankIFSC { get; set; } = string.Empty;
        public string Subjects { get; set; } = string.Empty;
        public string OptionalSubjects { get; set; } = string.Empty;
        public string ApplicationNumber { get; set; } = string.Empty;
        public string ApaarId { get; set; } = string.Empty;
        public string PerEduNumber { get; set; } = string.Empty;
        public int TotalRecords { get; set; } = 0;
        public decimal TotalDaysPresent { get; set; } = 0;
        public decimal PaidAmount { get; set; } = 0;
        public decimal Percentage { get; set; } = 0;
        public string ProfilePicture { get; set; } = string.Empty;
        public int ClassId { get; set; } = 0;
        public int SectionId { get; set; } = 0;
        public int BatchId { get; set; }
        public string FOccupation { get; set; }
        public string MOccupation { get; set; }
    }
    public class CoScholasticResult
    {
        public long StudentID { get; set; }
        public int TermID { get; set; }            // TermID from tbl_Term
        public long BatchId { get; set; }          // BatchId from tbl_Batches
        public string Batch_Name { get; set; } = string.Empty;  // Batch_Name from query
        public string TermName { get; set; } = string.Empty;    // Term name
        public string ClassName { get; set; } = string.Empty;  // Class name
        public string SectionName { get; set; } = string.Empty;// Section name
        public string CoscholasticGrades { get; set; } = string.Empty; // Concatenated grades
    }

    public class StudentAttendanceSummary
    {
        public long StudentRegisterId { get; set; }    // Student ID
        public long BatchId { get; set; }              // Batch ID
        public string BatchName { get; set; } = string.Empty; // Batch name
        public string ClassName { get; set; } = string.Empty; // Class name
        public string Section { get; set; } = string.Empty;   // Section name
        public int TotalDays { get; set; }             // Total days attended (full + half)
        public decimal AttendancePercentage { get; set; } // Attendance % (calculated)
        // Add more fields as per view definition
    }

    public class StudentTestPercentage
    {
        public long StudentId { get; set; }
        public int TermId { get; set; }
        public string TermName { get; set; } = string.Empty;
        public long BatchId { get; set; }
        public string BatchName { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;
        public decimal TotalObtainedMarks { get; set; } // Sum of marks per term
        // Add more fields as per view definition
    }
    public class StudentFeeDetails
    {
        public decimal PaidAmount { get; set; }
        public decimal DueAmount { get; set; }

        // Add more fields as per view definition
    }

    public class StudentProfileSummaryBatchWise
    {
        public List<CoScholasticResult> CoScholasticResults { get; set; } = new List<CoScholasticResult>();
        public List<StudentAttendanceSummary> AttendanceSummaries { get; set; } = new List<StudentAttendanceSummary>();
        public List<StudentTestPercentage> TestPercentages { get; set; } = new List<StudentTestPercentage>();
        public List<StudentFeeDetails> FeeDetails { get; set; } = new List<StudentFeeDetails>();
    }
}