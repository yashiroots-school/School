using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models.StaffRemakModel;
using SchoolManagement.Website.Models.StudentRemarkModel;

namespace SchoolManagement.Website.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public int DepartmentId { get; set; }
        public int UserId { get; set; }
        public bool IsEnable { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {

        }
        public DbSet<ClassAndSection> ClassAndSection { get; set; }
        public DbSet<Tbl_StudentRemark> StudentRemark { get; set; }
        public DbSet<StaffRemark> StaffRemaks { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<Departments> Department { get; set; }
        public DbSet<Classrooms> Classrooms { get; set; }
        public DbSet<ExamTypes> ExamTypes { get; set; }
        public DbSet<StafsDetails> StafsDetails { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<TblUserDynamicConfiguration> TblUserDynamicConfiguration { get; set; }
        

        public DbSet<StudentsRegistration> StudentsRegistrations { get; set; }
        public DbSet<StudentRegistrationHistory> StudentsRegistrations_History { get; set; }
        public DbSet<StudentRegNumberMaster> StudentRegNumberMaster { get; set; }

        public DbSet<SMSEMAILSCHEDULE> SMSEMAILSCHEDULE { get; set; }
        public DbSet<SMSEMAILSENDHISTORY> SMSEMAILSENDHISTORY { get; set; }
        public DbSet<SMSEMAILTEMPLETE> SMSEMAILTEMPLETE { get; set; }

        public DbSet<TransportFeeConfiguration> TransportFeeConfiguration { get; set; }
        public DbSet<TransportFeeHeadings> TransportFeeHeadings { get; set; }
        public DbSet<TblTransportFeeReceipts> TblTransportFeeReceipts { get; set; }
        public DbSet<TransportFeePlans> TransportFeePlans { get; set; }

        public DbSet<FamilyDetail> FamilyDetails { get; set; }
        public DbSet<AdditionalInformation> AdditionalInformations { get; set; }
        public DbSet<PastSchoolingReport> PastSchoolingReports { get; set; }
        public DbSet<StudentRemoteAccess> StudentRemoteAccess { get; set; }
        public DbSet<GuardianDetails> GuardianDetails { get; set; }

        public DbSet<FeeHeadings> FeeHeadings { get; set; }
        public DbSet<Accounts> Accounts { get; set; }
        public DbSet<FeeHeadingGroups> FeeHeadingGroups { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<StudentCategorys> StudentCategorys { get; set; }
        public DbSet<FeePlans> FeePlans { get; set; }

        public DbSet<TblLateFees> TblLateFees { get; set; }
        public DbSet<TblFeeReceipts> TblFeeReceipts { get; set; }
        public DbSet<TblStudentFeeSaved> TblStudentFeeSaved { get; set; }

        public DbSet<tbl_StudentDetail> tbl_StudentDetails { get; set; }
        //public DbSet<StafsDetails> StafsDetails { get; set; }
        public DbSet<tbl_SummerInternship> tbl_SummerInternships { get; set; }
        public DbSet<tbl_Declaration> tbl_Declarations { get; set; }
        public DbSet<tbl_WorkExperience> tbl_WorkExperiences { get; set; }
        public DbSet<tbl_AcademicDetail> tbl_AcademicDetails { get; set; }
        public DbSet<tbl_CommonDataListItem> tbl_CommonDataListItems { get; set; }
        public DbSet<tbl_Semester> tbl_Semesters { get; set; }
        public DbSet<tbl_skillset> tbl_skillsets { get; set; }
        public DbSet<tbl_Department> tbl_Departments { get; set; }
        public DbSet<Tbl_UserManagement> Tbl_UserManagement { get; set; }
        public DbSet<UserRolesTable> UserRolesTable { get; set; }
        public DbSet<Tbl_PublishDetail> Tbl_PublishDetail { get; set; }
        public DbSet<RolePagePermission> RolePagePermissions { get; set; }
        public DbSet<Tbl_DataList> DataLists { get; set; }
        public DbSet<Tbl_DataListItem> DataListItems { get; set; }

        public DbSet<Tbl_SignatureSetup> Tbl_SignatureSetup { get;set; }
        public DbSet<Tbl_StudentPromote> Tbl_StudentPromotes { get; set; }

        public DbSet<PeriodSchedule> PeriodSchedule { get; set; }
        public DbSet<TimeSettings> TimeSettings { get; set; }
        public DbSet<Tbl_TcAmount> TcAmount { get; set; }
        public DbSet<Tbl_FreezeData> Tbl_FreezeData { get; set; }

        public DbSet<Frequencys> Frequencys { get; set; }
        public DbSet<StudentTcDetails> Tbl_StudentTcDetails { get; set; }
        public DbSet<TcFeeDetails> TcFeeDetails { get; set; }
        public DbSet<StudentTcMaster> StudentTcMaster { get; set; }
        public DbSet<tbl_PaymentTransactionDetails> tbl_PaymentTransactionDetails { get; set; }
        public DbSet<tbl_PaymentTransactionFeeDetails> tbl_PaymentTransactionFeeDetails { get; set; }
        // TblDueFees
        public DbSet<TblDueFee> TblDueFee { get; set; }

        public DbSet<StudentLoginDetail> StudentLoginDetails { get; set; }
        public DbSet<StudentLoginHistory> StudentLoginHistory { get; set; }
        public DbSet<StudentResetPassword> StudentResetPassword { get; set; }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        public DbSet<Tbl_RoomType> Tbl_RoomTypes { get; set; } 
        public DbSet<Tbl_Room> Tbl_Rooms { get; set; }
        public DbSet<Tbl_ExamTypes> Tbl_ExamTypes { get; set; }
        public DbSet<Tbl_TeacherAllocation> Tbl_TeacherAllocations { get; set; }
        public DbSet<Tbl_Classsetup> tbl_Classsetups { get; set; }
        public DbSet<Tbl_SetTime> tbl_SetTimes { get; set; }
        public DbSet<Tbl_WeekDays> Tbl_WeekDays { get; set; }
        public DbSet<Tbl_Class> Tbl_Class { get; set; }
        public DbSet<Tbl_SectionSetup> Tbl_SectionSetup { get; set; }
        public DbSet<Tbl_TimeTable> Tbl_TimeTable { get; set; }
        public DbSet<Tbl_SubjectsSetup> Tbl_SubjectsSetup { get; set; }
        public DbSet<ClassTimeTableEntry> ClassTimeTableEntries { get; set; }
        public DbSet<Tbl_Assignment> Tbl_Assignments { get; set; }
        public DbSet<Tbl_Revision> Tbl_Revision { get; set; }
        public DbSet<Tbl_Portions> Tbl_Portions { get; set; }
        public DbSet<Tbl_Batches> Tbl_Batches { get; set; }
        public DbSet<Tbl_TimeTableMaster> Tbl_TimeTableMaster { get; set; }
        public DbSet<Tbl_BloodGroup> Tbl_BloodGroup { get; set; }
        public DbSet<Tbl_Category> Tbl_Category { get; set; }
        public DbSet<Tbl_Religion> Tbl_Religion { get; set; }
        public DbSet<Tbl_Caste> Tbl_Caste { get; set; }


        public DbSet<Tbl_StudentAttendance> Tbl_StudentAttendance { get; set; }
        public DbSet<Tbl_TempStudentAttendance> Tbl_TempStudentAttendance { get; set; }
        public DbSet<Tbl_StaffAttendance> Tbl_StaffAttendance { get; set; }
        public DbSet<Tbl_BasicPayDetails> Tbl_BasicPayDetails { get; set; }
        public DbSet<Tbl_StaffSalary> Tbl_StaffSalary { get; set; }
        public DbSet<Tbl_AccountSummary> Tbl_AccountSummary { get; set; }

        public DbSet<Tbl_BasicpayMaster> Tbl_BasicpayMaster { get; set; }
        public DbSet<Tbl_AccountType> Tbl_AccountType { get; set; }
        public DbSet<Tbl_SalaryStatement> Tbl_SalaryStatement { get; set; }
        public DbSet<Tbl_EPFStatement> Tbl_EPFStatement { get; set; }
        public DbSet<Tbl_StaffCategory> Tbl_StaffCategory { get; set; }
        public DbSet<Tbl_ArchieveStaffSalary> Tbl_ArchieveStaffSalary { get; set; }
        public DbSet<Tbl_ArchieveChangeStaffAccounttype> Tbl_ArchieveChangeStaffAccounttype { get; set; }
        public DbSet<TblTransportReducedAmount> TblTransportReducedAmount { get; set; }

        public DbSet<TblEmailArchieve> TblEmailArchieve { get; set; }
        public DbSet<TblCreateSchool> TblCreateSchool { get; set; }
        public DbSet<Tbl_CreateBank> Tbl_CreateBank { get; set; }
        public DbSet<Tbl_CreateBranch> Tbl_CreateBranch { get; set; }
        public DbSet<Tbl_MerchantName> Tbl_MerchantName { get; set; }
        public DbSet<Tbl_CreateMerchantId> Tbl_CreateMerchantId { get; set; }
        public DbSet<Tbl_SchoolSetup> Tbl_SchoolSetup { get; set; }
        public DbSet<Tbl_MenuName> Tbl_MenuName { get; set; }
        public DbSet<Tbl_SubmenuName> Tbl_SubmenuName { get; set; }
        public DbSet<Tbl_RolePermissionNew> Tbl_RolePermissionNew { get; set; }
        public DbSet<Tbl_Siblings> Tbl_Siblings { get; set; }

        public DbSet<Tbl_Deductions> Tbl_Deductions { get; set; }
        public DbSet<Tbl_Arrear> Tbl_Arrear { get; set; }
        public DbSet<MasterReport> MasterReports { get; set; }
        public DbSet<ReportHeading> ReportHeadings { get; set; }

        public DbSet<LabelControl> LabelControls { get; set; }

        public DbSet<MasterLabel> Labels { get; set; }
        public DbSet<Tbl_ConfigureMaxMarks> Tbl_ConfigureMaxMarks { get; set; }
        public DbSet<SchoolBoards> schoolBoards { get; set; }
        public DbSet<GradingCriteria> gradingCriteria { get; set; }
        public DbSet<StudentAttendanceCount> StudentAttendanceCount { get;set; }
        public DbSet<Tbl_Term> tbl_Term { get; set; }
        public DbSet<Tbl_Tests> tbl_Tests { get; set; }
        public DbSet<Tbl_TestRecords> Tbl_TestRecord { get; set; }
        public DbSet<Tbl_CoScholastic> tbl_CoScholastic { get; set; }
        public DbSet<Tbl_CoScholasticClass> tbl_CoScholasticClass{ get; set; }
        public DbSet<Tbl_CoScholastic_Result> tbl_CoScholastic_Result { get; set; }
        public DbSet<tbl_CoScholasticObtainedGrade> tbl_CoScholasticObtainedGrade { get; set; }
        public DbSet<Tbl_TestObtainedMark> tbl_TestObtainedMark { get; set; }
        public DbSet<Tbl_Remark>  tbl_Remark { get; set; }
        public DbSet<Tbl_Student_ElectiveRecord> tbl_Student_ElectiveRecord { get; set; }
        public DbSet<Tbl_ClassSubject> tbl_ClassSubject { get; set; }
        public DbSet<ReportCardSetting> reportCardSetting { get; set; }
        public DbSet<TblTestAssignDate> TblTestAssignDate { get; set; }
        public DbSet<Tbl_HoldDetail> Tbl_HoldDetail { get; set; }

    }
}