//using SchoolManagement.Website.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;


//namespace SchoolManagement.Website.ViewModels
//{ 
//      public class PrintReportCardData
//    {
//        public StudentData StudentData { get; set; } = new StudentData();
//        public List<GroupedSubjects> GroupedSubjects { get; set; } = null;
//        public List<GroupedTerms> GroupedTerms { get; set; } = new List<GroupedTerms>();
//        public List<CoscholasticAreaDatas> CoscholasticAreaData { get; set; } = new List<CoscholasticAreaDatas>();
//        public List<GradingCriteria> GradingCriteria { get; set; } = new List<GradingCriteria>();
//        public List<string> Term { get; set; } = new List<string>();
//        public string Result { get; set; } = string.Empty;
//        public decimal ObtainedPercent { get; set; }
//        public string ObtainedGrade { get; set; } = string.Empty;
//    }
//    //public partial class GradingCriterias
//    //{


//    //    public decimal MinimumPercentage { get; set; }

//    //    public decimal MaximumPercentage { get; set; }

//    //    public string Grade { get; set; }

//    //    public string GradeDescription { get; set; }




//    //}
//    public class CoscholasticAreaDatas
//    {
//        public string Name { get; set; } = string.Empty;
//        public string ObtainedGrade { get; set; } = string.Empty;
//        public string Term { get; set; } = string.Empty;
//        public string GradeTerm1 { get; set; } = string.Empty;
//        public string GradeTerm2 { get; set; } = string.Empty;
//        public string GradeUT1 { get; set; } = string.Empty;
//        public string GradeUT2 { get; set; } = string.Empty;
//        public string GradePre1 { get; internal set; } = string.Empty;
//        public string GradePre2 { get; internal set; } = string.Empty;
//    }
//    public class StudentData
//    {
//        //Student Header Data
//        public long StudentID { get; set; }
//        public int BatchID { get; set; }
//        public int TermID { get; set; }
//        public int SectionID { get; set; }
//        public string SchoolName { get; set; }
//        //public string newAddress { get; set; } 
//        public int CurrentYear { get; set; }
//        public string SchoolLogo { get; set; }
//        public string StudentName { get; set; } = string.Empty;
//        public string FatherName { get; set; } = string.Empty;
//        public string MotherName { get; set; } = string.Empty;
//        public string ScholarNo { get; set; } = string.Empty;
//        public string RollNo { get; set; } = string.Empty;
//        public string DateOfBirth { get; set; } = string.Empty;
//        public string AcademicYear { get; set; } = string.Empty;
//        public string ClassName { get; set; } = string.Empty;
//        public string SectionName { get; set; } = string.Empty;
//        public string Attendance { get; set; } = string.Empty;
//        public string PromotedClass { get; set; } = string.Empty;
//        public string StaffSignatureLink { get; set; } = string.Empty;
//        public string Remark { get; set; } = string.Empty;
//        public int ClassID { get; set; }
//        public string RankInClass { get; set; } = string.Empty;
//        public string PrincipalSign { get; set; } = string.Empty;
//    }
//    public class GroupedTerms
//    {
//        //
//        public string Term { get; set; } = string.Empty;
//        public string TestType { get; set; } = string.Empty;
//        public decimal Total { get; set; }
//        public decimal MaximumMarks { get; set; }
//        public decimal Percentage { get; set; }
//        public string Grade { get; set; } = string.Empty;
//        public decimal totalper { get; set; } = 0;


//    }
//    public class TermNames
//    {
//        public string Term { get; set; } = string.Empty;
//    }
//    public class GroupedSubjects
//    {
//        //
//        public string SubjectName { get; set; } = string.Empty;
//        public List<SubjectTermRecord> Terms { get; set; } = new List<SubjectTermRecord>();
//        public bool IsOptional { get; set; }
//        public long SubjectId { get; set; }
//        public long SerialNumber { get; set; }
//    }

//    public class TermSubjectMarks
//    {
//        public long TestID { get; set; }
//        public string SubjectName { get; set; } = string.Empty;
//        public string TermName { get; set; } = string.Empty;
//        public string TestType { get; set; } = string.Empty;
//        public decimal Mark { get; set; }
//        public decimal MaximumMarks { get; set; }
//        public string Grade { get; set; } = string.Empty;
//        public bool IsOptional { get; set; }
//        public long Studentid { get; set; }
//        public long SubjectId { get; set; }
//        public long SerialNumber { get; set; }
//    }

//    public class SubjectTermRecord
//    {
//        public string Name { get; set; } = string.Empty;
//        public decimal TheoryMark { get; set; }
//        public decimal PracticalMark { get; set; }
//        public decimal TotallMark { get; set; }
//        public decimal MaximumMarks { get; set; }
//        public string Grade { get; set; } = string.Empty;
//        public bool IsOptional { get; set; }
//        public string TestType { get; set; }
//    }









//}

using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace SchoolManagement.Website.ViewModels
{
    public class PrintReportCardData
    {
        public StudentData StudentData { get; set; } = new StudentData();
        public List<GroupedSubjects> GroupedSubjects { get; set; } = null;
        public List<GroupedTerms> GroupedTerms { get; set; } = new List<GroupedTerms>();
        public List<CoscholasticAreaDatas> CoscholasticAreaData { get; set; } = new List<CoscholasticAreaDatas>();
        public List<GradingCriteria> GradingCriteria { get; set; } = new List<GradingCriteria>();
        public List<string> Term { get; set; } = new List<string>();
        public string Result { get; set; } = string.Empty;
        public decimal ObtainedPercent { get; set; }
        public string ObtainedGrade { get; set; } = string.Empty;
    }
    //public partial class GradingCriterias
    //{


    //    public decimal MinimumPercentage { get; set; }

    //    public decimal MaximumPercentage { get; set; }

    //    public string Grade { get; set; }

    //    public string GradeDescription { get; set; }




    //}
    public class CoscholasticAreaDatas
    {
        public string Name { get; set; } = string.Empty;
        public string ObtainedGrade { get; set; } = string.Empty;
        public string Term { get; set; } = string.Empty;
        public string GradeTerm1 { get; set; } = string.Empty;
        public string GradeTerm2 { get; set; } = string.Empty;
        public string GradeUT1 { get; set; } = string.Empty;
        public string GradeUT2 { get; set; } = string.Empty;
        public string GradePre1 { get; internal set; } = string.Empty;
        public string GradePre2 { get; internal set; } = string.Empty;
    }
    public class StudentData
    {
        //Student Header Data
        public long StudentID { get; set; }
        public int BatchID { get; set; }
        public int TermID { get; set; }
        public int SectionID { get; set; }
        public string SchoolName { get; set; }
        public string newAddress { get; set; } 
        public int CurrentYear { get; set; }
        public string SchoolLogo { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string FatherName { get; set; } = string.Empty;
        public string MotherName { get; set; } = string.Empty;
        public string ScholarNo { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string DateOfBirth { get; set; } = string.Empty;
        public string AcademicYear { get; set; } = string.Empty;
        public string ClassName { get; set; } = string.Empty;
        public string SectionName { get; set; } = string.Empty;
        public string Attendance { get; set; } = string.Empty;
        public string PromotedClass { get; set; } = string.Empty;
        public string StaffSignatureLink { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public int ClassID { get; set; }
        public string RankInClass { get; set; } = string.Empty;
        public string PrincipalSign { get; set; } = string.Empty;
    }
    public class GroupedTerms
    {
        //
        public string Term { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public decimal MaximumMarks { get; set; }
        public decimal Percentage { get; set; }
        public string Grade { get; set; } = string.Empty;
        public decimal totalper { get; set; } = 0;


    }
    public class TermNames
    {
        public string Term { get; set; } = string.Empty;
    }
    public class GroupedSubjects
    {
        //
        public string SubjectName { get; set; } = string.Empty;
        public List<SubjectTermRecord> Terms { get; set; } = new List<SubjectTermRecord>();
        public bool IsOptional { get; set; }

        public long SubjectId { get; set; }
        public long SerialNumber { get; set; }
    }

    public class TermSubjectMarks
    {
        public long TestID { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string TermName { get; set; } = string.Empty;
        public string TestType { get; set; } = string.Empty;
        public decimal Mark { get; set; }
        public decimal MaximumMarks { get; set; }
        public string Grade { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
        public int Studentid { get; set; }
        public long SubjectId { get; set; }
        public long SerialNumber { get; set; }
    }

    public class SubjectTermRecord
    {
        public string Name { get; set; } = string.Empty;
        public decimal TheoryMark { get; set; }
        public decimal PracticalMark { get; set; }
        public decimal TotallMark { get; set; }
        public decimal MaximumMarks { get; set; }
        public string Grade { get; set; } = string.Empty;
        public bool IsOptional { get; set; }
        public string TestType { get; set; }
    }









}

