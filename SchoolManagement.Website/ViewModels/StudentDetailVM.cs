using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentDetailVM
    {
        public long StudentId { get; set; }
        public string ScholarNumber { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
        public string Years { get; set; }
        public string Batch { get; set; }
        public string Specialization { get; set; }
        public string Sibiling1 { get; set; }
        public string Sibiling2 { get; set; }
        public string Sibiling3 { get; set; }
        public string Sibiling4 { get; set; }
        public string Sibiling5 { get; set; }
        public string Category { get; set; }
        public string FacultyMentor { get; set; }
        public string DateofBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string CorrespondenceAddress { get; set; }
        public string ResidenceLocation { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string OutStationStudent { get; set; }
        public string NativePlace { get; set; }
        public string Hostalite { get; set; }
        public string FatherName { get; set; }
        public string FatherProfession { get; set; }
        public string FatherMobileNo { get; set; }
        public string FatherEmailId { get; set; }
        public string FatherCompanyName { get; set; }
        public string MotherName { get; set; }
        public string MotherProfession { get; set; }
        public string MotherMobileNo { get; set; }
        public string MotherEmailId { get; set; }
        public string MotherCompanyName { get; set; }
        public string status { get; set; }
        public string Addedon { get; set; }
        public string Addeby { get; set; }
        public string Updatedon { get; set; }
        public string Updatedby { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string DateOn { get; set; }
        public int TotalExperience { get; set; }
        public string Semester { get; set; }
        public int Studentid { get; set; }
        public string Classname { get; set; }
        public int Class_Id { get; set; }
        public string Name { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
        public string Class { get; set; }
        public string BatchName { get; set; }
        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

        public string AmissionFee { get; set; }
        public string ApplicationNo { get; set; }
    }

    public class AdmissionStudentDetailVM
    {
        public long StudentId { get; set; }
        public string ScholarNumber { get; set; }
        public string StudentName { get; set; }
        public string Course { get; set; }
        public string Years { get; set; }
        public string Batch { get; set; }
        public string Specialization { get; set; }
        public string Sibiling1 { get; set; }
        public string Sibiling2 { get; set; }
        public string Sibiling3 { get; set; }
        public string Sibiling4 { get; set; }
        public string Sibiling5 { get; set; }
        public string Category { get; set; }
        public int Category_Id { get; set; }
        public int Class_Id { get; set; }
        public string Class { get; set; }
        public string FacultyMentor { get; set; }
        public string DateofBirth { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string CorrespondenceAddress { get; set; }
        public string ResidenceLocation { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public string OutStationStudent { get; set; }
        public string NativePlace { get; set; }
        public string Hostalite { get; set; }
        public string FatherName { get; set; }
        public string FatherProfession { get; set; }
        public string FatherMobileNo { get; set; }
        public string FatherEmailId { get; set; }
        public string FatherCompanyName { get; set; }
        public string MotherName { get; set; }
        public string MotherProfession { get; set; }
        public string MotherMobileNo { get; set; }
        public string MotherEmailId { get; set; }
        public string MotherCompanyName { get; set; }
        public string status { get; set; }
        public string Addedon { get; set; }
        public string Addeby { get; set; }
        public string Updatedon { get; set; }
        public string Updatedby { get; set; }
        public string Spare1 { get; set; }
        public string Spare2 { get; set; }
        public string Spare3 { get; set; }
        public string DateOn { get; set; }
        public int TotalExperience { get; set; }
        public string Semester { get; set; }
        public string isapprove { get; set; }
        public string AdmissionFeePaid { get; set; }
        public string AdmissionNumber { get; set; }
        public string LastName { get; set; }
        public string DOB { get; set; }
        public List<listitem> StudCate { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }



    }
    public class listitem
    {
        public string DataListItemId { get; set; }
        public string DataListItemName { get; set; }
    }

    public class StudentAdmissionModel
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string Category { get; set; }
        public string Course { get; set; }
        public string isapprove { get; set; }
        public string AdmissionStatusID { get; set; }
        public string ApplicationNumber { get; set; }
        public string Last_Name { get; set; }
        public string DOB { get; set; }
    }
    public class StudentAdmissionStatusUpdate
    {
        public int IsApprove { get; set; }
    }
    public class StudentStatusUpdate
    {
        public bool IsActive { get; set; }
    }
    public class studentupdate
    {
        public string RegNumber { get; set; }
    }
    public class studentRegNumberUpdate
    {
        public int RegLastNumber { get; set; }
        public string RegStatus { get; set; }
    }
    public class StudentRegMasterModel
    {
        public string Class { get; set; }
        public string BatchName { get; set; }
        public string RegPrefix { get; set; }
        public int RegLength { get; set; }
        public int RegNumberStartWith { get; set; }
        public string RegStatus { get; set; }
        public string StartRegNumber { get; set; }
        public string LastRegNumber { get; set; }
        public int Class_Id { get; set; }
        public string FromDate { get; set; }
        public int AdmissionProcess_Id { get; set; }
        public string LastName { get; set; }
    }
    public class StudentRegFilterModel
    {
        public int ClassId { get; set; }
        public int BatchId { get; set; }
        public int SectionId { get; set; }
        public int AdmissionProcessId { get; set; }
        public string FromDate { get; set; }
    }
    public class StudentTcFilterModel
    {
        public int BatchId { get; set; }
        public int SectionId { get; set; }
        public string Class { get; set; }
    }

    public class StudentApproveStatusModel
    {
        public int StudentID { get; set; }
        public int ApproveCode { get; set; }
    }
    public class StudentPromoteVM
    {
        public string ApplicationNumber { get; set; }

        public string Class { get; set; }

        public int Class_ID { get; set; }

        public int Student_Id { get; set; }

        public string IsApprove { get; set; }

        public string DOB { get; set; }
        public string FirstName { get; set; }

        public string Last_Name { get; set; }

        public string Registration_date { get; set; }

        public string Promoted_Year { get; set; }
    }
}