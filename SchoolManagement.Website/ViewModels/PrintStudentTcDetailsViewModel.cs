using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class PrintStudentTcDetailsViewModel
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Gender { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }

        public string Rvill { get; set; }
        public string Rpost { get; set; }
        public string Rdist { get; set; }

        public string Rstate { get; set; }

        public string ClassName { get; set; }
        public string BatchYear { get; set; }
        public string ExamStatus { get; set; }
        public string QualifiedForPromotion { get; set; }
        public string PromoteClassName { get; set; }
        public string PromoteClassNameWord { get; set; }
        public string PromoteSectionName { get; set; }
        public string Remark { get; set; }
        public string Reason { get; set; }
        public string SchoolName { get; set; }
        public string SchoolEmail { get; set; }
        public string SchoolWebsite { get; set; }
        public string SchoolLogo { get; set; }
        public string TCDate { get; set; }
        public string AdmissionDate { get; set; }
        public string AdmittedDate { get; set; }
        public string ApplicationNo { get; set; }
        public string AdmissionDateWord { get; set; }
        public string SchoolLeftDate { get; set; }
        public string SNo { get; set; }
        public string Nationality { get; set; }
        public string PerEduNumber { get; set; }
        public string FamilySSSMID { get; set; }
        public string SSSMIdNumber { get; set; }
        public string ScholarNo { get; set; }
        public string CastCategory { get; set; }
        public string DOB { get; set; }
        public string DOBWord { get; set; }
        public string LastClass { get; set; }
        public string LastClassWord { get; set; }
        public int TotAttendance { get; set; }
        public int TotWorkingDays { get; set; }
        public string GeneralConduct { get; set; }
        public string TCApplyDate { get; set; }
        public string OtherRemarks { get; set; }

        


        public string FeePaidUpto { get; set; }
        public string TCSNo { get; set; }
        public string ApaarId { get; set; }
    }

    
}