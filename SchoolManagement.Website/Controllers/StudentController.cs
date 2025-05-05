using ClosedXML.Excel;
using EmployeeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.Models.DataAccess;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Net;
using System.Globalization;
using System.Net.Mail;
using SchoolManagement.Website.Models.StudentRemarkModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using Org.BouncyCastle.Asn1.X509;
using iTextSharp.tool.xml.css;
using Microsoft.Ajax.Utilities;
using DocumentFormat.OpenXml.Wordprocessing;

namespace SchoolManagement.Website.Controllers
{
    public class StudentController : Controller
    {


        private ApplicationDbContext _context = new ApplicationDbContext();
        private AcademicDetail acad = new AcademicDetail();
        private Semister sem = new Semister();
        private WorkExperience work = new WorkExperience();
        private SummerInternship summr = new SummerInternship();
        Skillset skill = new Skillset();
        private readonly IRepository<Student> _studentRepository = null;
        private IRepository<StudentsRegistration> _StudentsRegistration = null;
        private IRepository<FamilyDetail> _FamilyDetail = null;
        private RefreshMode refreshMode;
        private readonly IRepository<TblFeeReceipts> _TblFeeReceiptsRepository = null;
        public StudentController()
        {
            _studentRepository = new Repository<Student>();
            _TblFeeReceiptsRepository = new Repository<TblFeeReceipts>();
            _StudentsRegistration = new Repository<StudentsRegistration>();
            _FamilyDetail = new Repository<FamilyDetail>();
        }

        public ActionResult Index()
        {

            return View();
        }
        public ActionResult StudentProfile()
        {
            return View();
        }
        /// <summary>
        /// Add new Student record
        /// </summary>
        /// <returns></returns>
        public ActionResult AddStudent()
        {
            if (Session["rolename"].ToString() != "Administrator")
                return RedirectToAction("Login", "Account");

            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            ViewBag.TransportVehicleNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "TransportVehicleNo").DataListId.ToString()).ToList();

            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

            //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //ViewBag.StudentNo = _context.Students.Count() + 101;

            // ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");

            ViewBag.classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission  == true ||false )*/.ToList();
            ViewBag.BatcheNames = batches;// new SelectList(batches, "Batch_Id", "Batch_Name");
            return View();
        }



        public ActionResult StudentRegstraton()
        {
            ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");
            ViewBag.Batches = new SelectList(_context.Tbl_Batches.ToList().OrderBy(x => x.Batch_Name).ToList(), "id", "BatchName");
            return View();
        }

        [HttpPost]
        public ActionResult AddStudent(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel)
        {
            ViewBag.UIN = "EMIS/" + _context.Students.Count();
            try
            {
                if (!string.IsNullOrEmpty(studentViewModel.Student.Name))
                {
                    Student student = new Student()
                    {
                        CreateBy = 1,
                        BloodGroup = studentViewModel.Student.BloodGroup,
                        AgeInWords = studentViewModel.Student.AgeInWords,
                        ApplicationNumber = studentViewModel.Student.ApplicationNumber,
                        Category = studentViewModel.Student.Category,
                        Class = studentViewModel.Student.Class,
                        Date = studentViewModel.Student.Date,
                        DOB = studentViewModel.Student.DOB,
                        Gender = studentViewModel.Student.Gender,
                        Hobbies = studentViewModel.Student.Hobbies,
                        MedicalHistory = studentViewModel.Student.MedicalHistory,
                        MotherTongue = studentViewModel.Student.MotherTongue,
                        Name = studentViewModel.Student.Name,
                        Medium = studentViewModel.Student.Medium,
                        Caste = studentViewModel.Student.Caste,
                        Nationality = studentViewModel.Student.Nationality,
                        OtherDetails = studentViewModel.Student.OtherDetails,
                        POB = studentViewModel.Student.POB,
                        ProfileAvatar = studentViewModel.Student.ProfileAvatar,
                        Religion = studentViewModel.Student.Religion,
                        Section = studentViewModel.Student.Section,
                        Sports = studentViewModel.Student.Sports,
                        UIN = studentViewModel.Student.UIN,
                        AdharNo = studentViewModel.Student.AdharNo,
                        AdharFile = studentViewModel.Student.AdharFile,
                        OtherLanguages = studentViewModel.Student.OtherLanguages,
                        MarkForIdentity = studentViewModel.Student.MarkForIdentity,
                        BatchName = studentViewModel.Student.BatchName,
                        IsApplyforTC = studentViewModel.Student.IsApplyforTC,
                        IsApplyforAdmission = studentViewModel.Student.IsApplyforAdmission,
                        IsApprove = studentViewModel.Student.IsApprove

                    };

                    if (uploadFilesViewModel.ProfileAvatar != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), fileName);
                            uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                            student.ProfileAvatar = fileName;
                        }
                    }
                    if (uploadFilesViewModel.AdharFile != null)
                    {
                        if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                            uploadFilesViewModel.AdharFile.SaveAs(path);
                            student.AdharFile = fileName;
                        }
                    }

                    Student result = _context.Students.Add(student);
                    int data = _context.SaveChanges();
                    if (data > 0)
                    {
                        Student studentDetail = GetAllStudents()
                                       .FirstOrDefault(x => x.Name.ToLower() == student.Name.ToLower());

                        if (studentDetail != null)
                        {
                            int studentId = student.StudentId;
                            string ApplicationNumber = student.ApplicationNumber;
                            // student family details
                            studentViewModel.FamilyDetail.StudentRefId = studentId;
                            studentViewModel.AdditionalInformation.StudentRefId = studentId;
                            studentViewModel.GuardianDetails.StudentRefId = studentId;
                            studentViewModel.PastSchoolingReport.StudentRefId = studentId;
                            studentViewModel.FamilyDetail.ApplicationNumber = ApplicationNumber;
                            studentViewModel.AdditionalInformation.ApplicationNumber = ApplicationNumber;
                            studentViewModel.GuardianDetails.ApplicationNumber = ApplicationNumber;
                            studentViewModel.PastSchoolingReport.ApplicationNumber = ApplicationNumber;
                            //studentViewModel.StudentRemoteAccess.StudentRefId = studentId;
                            _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                            _context.SaveChanges();


                            if (uploadFilesViewModel.BirthCertificateAvatar != null)
                            {
                                if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                    studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                                }
                            }
                            if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                            {
                                if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                    studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                                }
                            }
                            if (uploadFilesViewModel.ProgressReport != null)
                            {
                                if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.ProgressReport.SaveAs(path);
                                    studentViewModel.AdditionalInformation.ProgressReport = fileName;
                                }
                            }
                            if (uploadFilesViewModel.MigrationCertificate != null)
                            {
                                if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                    studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                                }
                            }

                            _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                            _context.SaveChanges();
                            _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                            _context.SaveChanges();

                            if (uploadFilesViewModel.TCAvatar != null)
                            {
                                if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.TCAvatar.SaveAs(path);
                                    studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                                }
                            }
                            if (uploadFilesViewModel.MarksCardAvatar != null)
                            {
                                if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                    studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                                }
                            }
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                            {
                                if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                    var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                    uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                    studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                                }
                            }
                            _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                            _context.SaveChanges();

                            //_context.StudentRemoteAccess.Add(studentViewModel.StudentRemoteAccess);

                            //Remote Access
                            var studLoginDet = new StudentLoginDetail()
                            {
                                StudentId = student.StudentId,
                                CreatedBy = student.CreateBy,
                                CreatedOn = DateTime.Now,
                                UserName = student.UIN,
                                UserPassword = GenerateRandomUserPassword(),
                                ModifiedOn = DateTime.Now
                            };
                            _context.StudentLoginDetails.Add(studLoginDet);
                            _context.SaveChanges();
                        }
                    }
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully!');location.replace('AdmissionPortal')</script>");

                return RedirectToAction("AddStudent");
            }
            catch (System.Exception ex)
            {
                ViewBag.Error("Some error occured,Please contact with Administrator");
                return RedirectToAction("AddStudent");
            }

        }

        /// <summary>
        /// Student List 
        /// </summary>
        /// <returns></returns>
        public ActionResult AllStudents()
        {
            int allStudent = 0;
            List<Student> studentList = new List<Student>();
            List<Student> allStudents = GetAllStudents();
            if (allStudents != null)
            {
                studentList = allStudents;
                allStudent = studentList.Count();

            }
            ViewBag.allStudents = allStudent;
            return View(studentList);
        }
        public ActionResult GetAllStudentDetails()
        {
            int allStudent = 0;
            List<Student> studentList = new List<Student>();
            List<Student> allStudents = GetAllStudents();
            if (allStudents != null)
            {
                studentList = allStudents;
                allStudent = studentList.Count();

            }
            ViewBag.allStudents = allStudent;
            return View(studentList);
        }
        /// <summary>
        /// Student Detail by Student Id
        /// </summary>
        /// <returns></returns>
        public ActionResult StudentDetails(int? studentId)
        {
            return View();
        }

        /// <summary>
        /// Edit Student by Student ID
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public ActionResult EditStudent(int studentId)
        {
            StudentViewModel studentData = GetCompleteDetails(studentId);
            if (studentData != null)
            {
                return View(studentData);
            }
            return View();
        }

        /// <summary>
        /// Get All Students records
        /// </summary>
        /// <returns></returns>
        public List<Student> GetAllStudents()
        {
            List<Student> studentList = new List<Student>();
            System.Data.Entity.DbSet<Student> studentId2 = _context.Students;
            foreach (Student item in studentId2)
            {
                studentList.Add(item);
            }
            return studentList;
        }
        public JsonResult GetAllStudents1()
        {
            List<StudentsRegistration> studentList = new List<StudentsRegistration>();
            //System.Data.Entity.DbSet<StudentsRegistration> studentId2 = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            var batch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();
            int batchId = 0;
            if (batch != null)
                batchId = batch.Batch_Id;

            var studentdata = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Batch_Id == batchId).OrderBy(x => x.Name).ToList();
            foreach (StudentsRegistration item in studentdata)
            {
                item.Name = item.Name + " " + item.Last_Name;
                studentList.Add(new StudentsRegistration { StudentRegisterID = item.StudentRegisterID, Name = item.Name });
            }
            return Json(studentList, JsonRequestBehavior.AllowGet);
        }

        private string GenerateRandomString(int length)
        {
            char[] randString = new char[length];
            string _allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789_";
            Random randNum = new Random();
            for (int i = 0; i < length; i++)
                randString[i] = _allowedChars[Convert.ToInt32((_allowedChars.Length - 1) * randNum.NextDouble())];
            return new string(randString);
        }

        private string GenerateRandomUserName()
        {
            return GenerateRandomString(12);
        }

        private string GenerateRandomUserPassword()
        {
            return GenerateRandomString(8);
        }

        public StudentViewModel GetCompleteDetails(int studentId)
        {
            StudentViewModel studentViewModel = new StudentViewModel();
            System.Data.Entity.DbSet<Student> data = _context.Students;
            System.Data.Entity.DbSet<AdditionalInformation> AdditionalInformations = _context.AdditionalInformations;
            System.Data.Entity.DbSet<FamilyDetail> FamilyDetails = _context.FamilyDetails;
            return null;
        }
        public JsonResult GetStudentDetailsById(string ApplicationNumber)
        {
            int studentId = (int)_context.Students.FirstOrDefault(x => x.ApplicationNumber == ApplicationNumber).StudentId;  //xrnik
            StudentDetailViewModel studentModel = new StudentDetailViewModel();

            //Student studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            FamilyDetail familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentId);
            {
                //StudentsRegistration studentsRegistration = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                Student student = _context.Students.FirstOrDefault(x => x.ApplicationNumber == ApplicationNumber);
                studentModel.StudentName = student == null ? "" : student.Name;

                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

                var clasname = Classes.FirstOrDefault(x => x.DataListItemId == student.Class_Id)?.DataListItemName;
                studentModel.Class = clasname;
                studentModel.ClassId = student.Class_Id;
                studentModel.RoleNumber = student == null ? "" : Convert.ToString(student.StudentId);
                studentModel.FatherName = student == null ? "" : familyDetail?.FatherName;
                studentModel.Contact = student == null ? "" : familyDetail?.FMobile;
            }
            List<TblFeeReceipts> tblFees = new List<TblFeeReceipts>();
            tblFees = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
            float oldbalance = 0;
            if (tblFees.Count > 0)
            {
                foreach (var item in tblFees)
                {
                    oldbalance = oldbalance + item.OldBalance;
                }
            }
            //TblFeeReceipts tblFeeReceipt = _TblFeeReceiptsRepository.GetAll().LastOrDefault(x => x.StudentId == studentId);
            studentModel.OldBalance = tblFees == null ? 0 : oldbalance;

            return Json(studentModel, JsonRequestBehavior.AllowGet);
        }
        public async Task<JsonResult> GetStudentTCDetailsById(int studentId, bool isAdmissionStudent = false)
        {
            StudentTcDetailViewModel studentModel = new StudentTcDetailViewModel();
            var data = 0;
            var amount = new decimal();
            Student studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            FamilyDetail familyDetail = await _context.FamilyDetails.FirstOrDefaultAsync(x => x.StudentRefId == studentId);
            var tcDetails = new StudentTcDetails();
            if (isAdmissionStudent)
            {
                if (!string.IsNullOrEmpty(studentDetail.Class))
                {
                    studentDetail.Class = studentDetail.Class.Trim();
                }
                if (studentDetail.Class == "XI - I - MAT/PHY/CHE/BIO " || studentDetail.Class == "XI - II - MAT/PHY/CHE/BOT/ZOO " ||
                    studentDetail.Class == "XI - III - MAT/PHY/C.SCI" || studentDetail.Class == "XII - II - BIOLOGY "
                    || studentDetail.Class == "XI - IV - ECO/COMM/ACC"
                    || studentDetail.Class == "XII - I - COM.SCI"
                    || studentDetail.Class == "XII - III - COM / BUS.MATHS "
                    || studentDetail.Class == "XII - IV - PURE SCIENCE"
                    || studentDetail.Class == "XII - V - COMM / COM.SCIENCE")
                {

                    data = await _context.DataListItems.Where(x => x.DataListItemName.Contains("Admission")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
                }
                else
                {
                    data = await _context.DataListItems.Where(x => x.DataListItemName.Contains("Junior Admission Fee")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
                }

                amount = await _context.TcAmount.Where(x => x.Type == data).Select(x => x.Amount).FirstOrDefaultAsync();
            }
            else
            {
                tcDetails = await _context.Tbl_StudentTcDetails.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
                if (tcDetails != null)
                {
                    amount = await _context.TcAmount.Where(x => x.Type == tcDetails.TcId).Select(x => x.Amount).FirstOrDefaultAsync();
                }
            }

            studentModel.StudentName = studentDetail == null ? "" : studentDetail.Name;
            studentModel.FatherName = studentDetail == null ? "" : familyDetail?.FatherName;
            studentModel.Contact = studentDetail == null ? "" : familyDetail?.FMobile;
            studentModel.Class = studentDetail == null ? "" : studentDetail.Class;
            studentModel.Category = studentDetail == null ? "" : studentDetail.Category;
            studentModel.RoleNumber = studentDetail == null ? "" : Convert.ToString(studentDetail.StudentId);
            studentModel.TCBal = amount;
            studentModel.Batch = studentDetail == null ? string.Empty : studentDetail.BatchName;
            studentModel.studentTcid = tcDetails?.Id;
            return Json(studentModel, JsonRequestBehavior.AllowGet);
        }


        //Pre registration Page 
        public ActionResult PreRegisterStudent()
        {
            var url = Request.Url.AbsoluteUri;
            if (url.Contains("UpdateId"))
            {
                ViewBag.IsEdit = true;
            }
            else
            {
                ViewBag.IsEdit = false;
            }
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
            ViewBag.AllYears = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "year").DataListId.ToString()).ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "semester").DataListId.ToString()).ToList();
            ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            ViewBag.AllSpecializations = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "specialization").DataListId.ToString()).ToList();
            ViewBag.AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.AllFacultyMentors = _context.tbl_CommonDataListItems.Where(e => e.DataListName == "FacultyMentor").ToList();
            ViewBag.AllQualifications = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "qualification").DataListId.ToString()).ToList();
            ViewBag.AllSemesters = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "semester").DataListId.ToString()).ToList();
            ViewBag.AllFacultyMentors = _context.tbl_CommonDataListItems.Where(e => e.DataListName == "FacultyMentor").ToList();

            return View();

        }

        [HttpPost]
        public JsonResult ScholarNoExists(string RegNo, string StudentId)
        {
            bool ischeck = false;
            if (!string.IsNullOrEmpty(StudentId))
            {
                int nStudentId = Convert.ToInt32(StudentId);
                ischeck = _context.tbl_StudentDetails.Where(c => c.ScholarNumber == RegNo && c.StudentId != nStudentId).Any();
                return Json(ischeck, JsonRequestBehavior.AllowGet);
            }
            return Json(ischeck, JsonRequestBehavior.AllowGet);

        }


        #region AcademicDetails
        [HttpPost]
        public JsonResult AddAcademic(tbl_AcademicDetail AcademicDetail)
        {
            return Json(acad.Add(AcademicDetail), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateAcademic(tbl_AcademicDetail AcademicDetail)
        {
            return Json(acad.Update(AcademicDetail));
        }

        [HttpPost]
        public JsonResult DeleteAcademicDetail(int id)
        {
            return Json(acad.Delete(id));
        }

        [HttpPost]
        public JsonResult getAcademicDetailsById(int id)
        {
            return Json(acad.GetById(id));
        }

        [HttpPost]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetAllAcademicDetailsByRegNo(string RegNo)
        {
            return Json(acad.GetByRegNo(RegNo));
        }
        #endregion

        #region Semester
        [HttpPost]
        public JsonResult AddSem(tbl_Semester AcademicDetail)
        {
            return Json(sem.Add(AcademicDetail));
        }

        [HttpPost]
        public JsonResult UpdateSem(tbl_Semester AcademicDetail)
        {
            return Json(sem.Update(AcademicDetail));
        }
        [HttpPost]
        public JsonResult DeleteSem(int id)
        {
            return Json(sem.Delete(id));
        }
        [HttpPost]
        public JsonResult getSemDetailsById(int id)
        {
            return Json(sem.GetById(id));
        }

        public JsonResult getAllSemDetailsByRegNo(string RegNo)
        {
            return Json(sem.GetByRegNo(RegNo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region WorkExperience
        [HttpPost]
        public JsonResult AddWorkExp(tbl_WorkExperience WorkExp)
        {
            return Json(work.Add(WorkExp));
        }

        [HttpPost]
        public JsonResult UpdateWorkExp(tbl_WorkExperience WorkExp)
        {
            return Json(work.Update(WorkExp));
        }

        public JsonResult DeleteWorkExp(int id)
        {
            return Json(work.Delete(id), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetWorkExpDetailsById(int id)
        {
            return Json(work.GetById(id));
        }

        public JsonResult GetAllWorkExpDetailsByRegNo(string RegNo)
        {
            return Json(work.GetByRegNo(RegNo), JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult AddUpdateStudent(tbl_StudentDetail StudentDetails)
        {
            tbl_StudentDetail studentDetails = _context.tbl_StudentDetails.FirstOrDefault(x => x.ScholarNumber.ToLower() == StudentDetails.ScholarNumber.ToLower());
            try
            {
                if (studentDetails == null)
                {
                    _context.tbl_StudentDetails.Add(StudentDetails);
                }
                else
                {
                    StudentDetails.StudentId = studentDetails.StudentId;
                    _context.tbl_StudentDetails.AddOrUpdate(StudentDetails);
                }
                _context.SaveChanges();
                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json("Fail");
            }
        }


        [HttpPost]
        public JsonResult AddUpdateIntership(tbl_SummerInternship SummerInternship)
        {
            tbl_SummerInternship studentDetails = _context.tbl_SummerInternships.FirstOrDefault(x => x.ScholarNumber.ToLower() == SummerInternship.ScholarNumber.ToLower());
            try
            {
                if (studentDetails == null)
                {
                    _context.tbl_SummerInternships.Add(SummerInternship);
                }
                else
                {
                    SummerInternship.SummerInternshipId = studentDetails.SummerInternshipId;
                    _context.tbl_SummerInternships.AddOrUpdate(SummerInternship);
                }
                _context.SaveChanges();
                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Fail");
            }
        }

        [HttpPost]
        public JsonResult AddUpdateSkill(tbl_skillset StudentDetails)
        {
            tbl_skillset studentDetails = _context.tbl_skillsets.FirstOrDefault(x => x.ScholarNumber.ToLower() == StudentDetails.ScholarNumber.ToLower());
            try
            {
                if (studentDetails == null)
                {
                    _context.tbl_skillsets.Add(StudentDetails);
                }
                else
                {
                    StudentDetails.SkillsetId = studentDetails.SkillsetId;
                    _context.tbl_skillsets.AddOrUpdate(StudentDetails);
                }
                _context.SaveChanges();
                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Fail");
            }
        }

        [HttpPost]
        public JsonResult AddUpdateDeclarations(tbl_Declaration declaration)
        {
            tbl_Declaration studentDetails = _context.tbl_Declarations.FirstOrDefault(x => x.ScholarNumber.ToLower() == declaration.ScholarNumber.ToLower());
            try
            {
                if (studentDetails == null)
                {
                    _context.tbl_Declarations.Add(declaration);
                }
                else
                {
                    declaration.DeclarationId = studentDetails.DeclarationId;
                    _context.tbl_Declarations.AddOrUpdate(declaration);
                }
                _context.SaveChanges();
                return Json("Success");
            }
            catch (Exception)
            {
                return Json("Fail");
            }
        }

        public async Task<ActionResult> ManageStudent()
        {
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
            ViewBag.AllYears = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "year").DataListId.ToString()).ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            List<StudentDetailVM> studentls = new List<StudentDetailVM>();
            //var result = await _context.Students.Where(x => x.AddedDate != null).OrderByDescending(x => x.AddedDate).ToListAsync();
            var result = await _context.Students.Where(x => x.AddedDate != null && x.IsApplyforTC != true).OrderByDescending(x => x.AddedDate).ToListAsync();
            foreach (var res in result)
            {
                StudentDetailVM studentobj = new StudentDetailVM();

                studentobj.ScholarNumber = res.StudentId.ToString();
                studentobj.StudentName = res.Name;
                studentobj.Category = res.Category;
                studentobj.Course = res.Class;
                studentobj.Semester = res.Class;
                studentobj.StudentId = res.StudentId;
                studentls.Add(studentobj);
            }
            ViewBag.StudentList = studentls;
            ViewBag.sudentcounts = _context.Students.Where(x => x.IsApprove == 217).ToList().Count();
            return View();
        }

        //For Grid 
        public ActionResult GetGrid_Student(int? firstItem, int? lastitem, AllStudentDetailsViewModel student, string SearchTxt)
        {
            try
            {
                //string Html = "";
                string url = Request.UrlReferrer.AbsoluteUri;
                ViewBag.StartValue = firstItem;
                Student students = new Student();
                List<StudentsRegistration> studentdetails = new List<StudentsRegistration>();
                studentdetails = _context.StudentsRegistrations.Where(x => x.IsApprove == 217).ToList();
                var ColumnShow = _context.TblUserDynamicConfiguration.Where(x => x.ListType == "1").FirstOrDefault();
                ViewBag.ColumnShow = ColumnShow == null ? "" : Convert.ToString(ColumnShow.ListData);

                List<AllStudentDetailsViewModel> alldata = new List<AllStudentDetailsViewModel>();
                FamilyDetail familydetails = new FamilyDetail();

                if (!string.IsNullOrWhiteSpace(SearchTxt))
                {
                    List<AllStudentDetailsViewModel> data = (List<AllStudentDetailsViewModel>)Session["AllDtudentdata"];
                    if (data != null && data.Count > 0)
                    {
                        return SearchGetgGrid_student(data, firstItem, lastitem, SearchTxt);
                    }
                }

                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();

                var Bloodgroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();

                var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Category").DataListId.ToString()).ToList();

                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

                var Religious = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();

                var Admissionprocess = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Admission Process").DataListId.ToString()).ToList();

                foreach (var items in studentdetails)
                {
                    //var studentappno = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == items.ApplicationNumber && x.IsApprove != 192);
                    //if(studentappno != null)
                    {
                        familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == items.ApplicationNumber);
                        students = _context.Students.FirstOrDefault(x => x.ApplicationNumber == items.ApplicationNumber);
                    }
                    if (students != null)
                    {
                        alldata.Add(new AllStudentDetailsViewModel
                        {
                            StudentId = students.StudentId, //Convert.ToInt32(items.StudentRegisterID),
                            ApplicationNumber = items.ApplicationNumber,
                            UIN = items.UIN,
                            Date = items.Date,
                            Name = items.Name,
                            Class = Classes.FirstOrDefault(x => x.DataListItemId == items.Class_Id)?.DataListItemName,
                            Section = Section.FirstOrDefault(x => x.DataListItemId == items.Section_Id)?.DataListItemName,
                            Gender = items.Gender,
                            RTE = items.RTE,
                            Medium = items.Medium,
                            Caste = items.Caste,
                            AgeInWords = items.AgeInWords,
                            DOB = items.DOB,
                            POB = items.POB,
                            Nationality = items.Nationality,
                            Religion = Religious.FirstOrDefault(x => x.DataListItemId == items.Religion_Id)?.DataListItemName,
                            MotherTongue = items.MotherTongue,
                            Category = Category.FirstOrDefault(x => x.DataListItemId == items.Category_Id)?.DataListItemName,
                            BloodGroup = Bloodgroup.FirstOrDefault(x => x.DataListItemId == items.BloodGroup_Id)?.DataListItemName,
                            MedicalHistory = items.MedicalHistory,
                            Hobbies = items.Hobbies,
                            Sports = items.Sports,
                            OtherDetails = items.OtherDetails,
                            ProfileAvatar = items.ProfileAvatar,
                            MarkForIdentity = items.MarkForIdentity,
                            AdharNo = items.AdharNo,
                            AdharFile = items.AdharFile,
                            OtherLanguages = items.OtherLanguages,
                            IsApplyforTC = items.IsApplyforTC,
                            IsApplyforAdmission = items.IsApplyforAdmission,
                            Iapprovestudent = Admissionprocess.FirstOrDefault(x => x.DataListItemId == items.IsApprove)?.DataListItemName,
                            IsActive = items.IsActive,
                            IsAdmissionPaid = items.IsAdmissionPaid,
                            IsInsertFromAd = items.IsInsertFromAd,
                            RegNumber = items.ApplicationNumber,
                            Last_Name = items.Last_Name,
                            FatherName = familydetails == null ? "" : familydetails.FatherName,
                            MotherName = familydetails == null ? "" : familydetails.MotherName,
                            FMobile = familydetails == null ? "" : familydetails.FMobile,
                            MMobile = familydetails == null ? "" : familydetails.MMobile,
                            FResidentialAddress = familydetails == null ? "" : familydetails.FResidentialAddress,
                            ParentEmail = items.Parents_Email,
                            Added_Year = items.AddedYear,
                            Added_Date = items.AddedDate,
                            Promotion_Date = items.Promotion_Date,
                            Promotion_Year = items.Promotion_Year,
                            Registration_Date = items.Registration_Date

                        });
                    }

                }
                var AllStudentsList = alldata.ToList();

                if (string.IsNullOrWhiteSpace(SearchTxt))
                    Session["AllDtudentdata"] = alldata;

                //if (!string.IsNullOrEmpty(SearchTxt))
                //{
                //    alldata = AllStudentsList.Where(x => !string.IsNullOrEmpty(x.ApplicationNumber) && x.ApplicationNumber.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.UIN) && x.UIN.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Date) && x.Date.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Class) && x.Class.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Section) && x.Section.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.RTE) && x.RTE.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Medium) && x.Medium.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Caste) && x.Caste.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.DOB) && x.DOB.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.POB) && x.POB.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Nationality) && x.Nationality.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Religion) && x.Religion.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.MotherTongue) && x.MotherTongue.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Category) && x.Category.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.BloodGroup) && x.BloodGroup.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.MedicalHistory) && x.MedicalHistory.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Hobbies) && x.Hobbies.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Sports) && x.Sports.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.OtherDetails) && x.OtherDetails.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.ProfileAvatar) && x.ProfileAvatar.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.MarkForIdentity) && x.MarkForIdentity.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.AdharNo) && x.AdharNo.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.AdharFile) && x.AdharFile.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.OtherLanguages) && x.OtherLanguages.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.RegNumber) && x.RegNumber.ToLower().Contains(SearchTxt.ToLower())

                //    || !string.IsNullOrEmpty(x.Last_Name) && x.Last_Name.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.FatherName) && x.FatherName.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.MotherName) && x.MotherName.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.FMobile) && x.FMobile.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.MMobile) && x.MMobile.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.Added_Year) && x.Added_Year.ToLower().Contains(SearchTxt.ToLower())
                //     || !string.IsNullOrEmpty(x.Added_Date) && x.Added_Date.ToLower().Contains(SearchTxt.ToLower())
                //      || !string.IsNullOrEmpty(x.Registration_Date) && x.Registration_Date.ToLower().Contains(SearchTxt.ToLower())
                //      || !string.IsNullOrEmpty(x.Promotion_Date) && x.Promotion_Date.ToLower().Contains(SearchTxt.ToLower())
                //      || !string.IsNullOrEmpty(x.FResidentialAddress) && x.FResidentialAddress.ToLower().Contains(SearchTxt.ToLower())
                //    || !string.IsNullOrEmpty(x.ParentEmail) && x.ParentEmail.ToLower().Contains(SearchTxt.ToLower())).ToList();





                //}
                ViewBag.Allstudentcount = alldata.Count();
                if (firstItem != null && lastitem != null)
                {
                    int from = Convert.ToInt32(firstItem);
                    int to = Convert.ToInt32(lastitem);
                    var filterdata = alldata.Skip(Math.Max(0, from - 1)).Take(to - (from - 1));
                    alldata = filterdata.ToList();
                }
                ViewBag.Allstudentdetails = alldata;

                return PartialView("AllStudentdetailsview");
            }
            catch (Exception ex)
            {
                //return Content()
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('/Dashboard/Dashboard')</script>");
                //return RedirectToAction("Login", "Account");
            }
        }


        public ActionResult SearchGetgGrid_student(List<AllStudentDetailsViewModel> data, int? firstItem, int? lastitem, string SearchTxt)
        {

            data = data.Where(x => !string.IsNullOrEmpty(x.ApplicationNumber) && x.ApplicationNumber.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.UIN) && x.UIN.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Date) && x.Date.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Class) && x.Class.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Section) && x.Section.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.RTE) && x.RTE.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Medium) && x.Medium.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Caste) && x.Caste.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.DOB) && x.DOB.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.POB) && x.POB.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Nationality) && x.Nationality.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Religion) && x.Religion.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.MotherTongue) && x.MotherTongue.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Category) && x.Category.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.BloodGroup) && x.BloodGroup.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.MedicalHistory) && x.MedicalHistory.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Hobbies) && x.Hobbies.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Sports) && x.Sports.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.OtherDetails) && x.OtherDetails.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.ProfileAvatar) && x.ProfileAvatar.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.MarkForIdentity) && x.MarkForIdentity.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.AdharNo) && x.AdharNo.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.AdharFile) && x.AdharFile.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.OtherLanguages) && x.OtherLanguages.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.RegNumber) && x.RegNumber.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Last_Name) && x.Last_Name.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.FatherName) && x.FatherName.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.MotherName) && x.MotherName.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.FMobile) && x.FMobile.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.MMobile) && x.MMobile.ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.Added_Year) && x.Added_Year.ToLower().Contains(SearchTxt.ToLower())
             || x.Added_Date.ToShortDateString().Contains(SearchTxt.ToLower())
              || !string.IsNullOrEmpty(x.Registration_Date) && x.Registration_Date.ToLower().Contains(SearchTxt.ToLower())
              || !string.IsNullOrEmpty(x.Promotion_Date) && x.Promotion_Date.ToLower().Contains(SearchTxt.ToLower())
              || !string.IsNullOrEmpty(x.FResidentialAddress) && x.FResidentialAddress.ToLower().Contains(SearchTxt.ToLower())
              || !string.IsNullOrEmpty(Convert.ToString(x.StudentId)) && Convert.ToString(x.StudentId).ToLower().Contains(SearchTxt.ToLower())
            || !string.IsNullOrEmpty(x.ParentEmail) && x.ParentEmail.ToLower().Contains(SearchTxt.ToLower())).ToList();

            ViewBag.StartValue = firstItem;

            var ColumnShow = _context.TblUserDynamicConfiguration.Where(x => x.ListType == "1").FirstOrDefault();
            ViewBag.ColumnShow = ColumnShow == null ? "" : Convert.ToString(ColumnShow.ListData);

            ViewBag.Allstudentcount = data.Count();
            if (firstItem != null && lastitem != null)
            {
                int from = Convert.ToInt32(firstItem);
                int to = Convert.ToInt32(lastitem);
                var filterdata = data.Skip(Math.Max(0, from - 1)).Take(to - (from - 1));
                data = filterdata.ToList();
            }
            ViewBag.Allstudentdetails = data;

            return PartialView("AllStudentdetailsview");

        }


        //Delete
        public ActionResult DeleteAdmissionStudent(int Id)
        {
            try
            {
                var stuentdata = _context.Students.FirstOrDefault(x => x.StudentId == Id);
                var studentreg = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == stuentdata.ApplicationNumber);
                if (studentreg != null)
                {
                    stuentdata.IsApprove = 192;
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Data Delete Successfully');location.replace('/Student/ManageStudent')</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult Deletestudentdata(int id)
        {
            try
            {
                var stuentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == id);
                if (stuentdata != null)
                {
                    stuentdata.IsApprove = 192;
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Data Delete Successfully');location.replace('/Student/StudentReport')</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public string SetDynamicColumn(string ListType, string ListData)
        {
            if (ListType != "" && ListData != "")
            {
                TblUserDynamicConfiguration tblUserDynamicConfiguration = new TblUserDynamicConfiguration();
                tblUserDynamicConfiguration.ListType = ListType;
                tblUserDynamicConfiguration.ListData = ListData;
                var data = _context.TblUserDynamicConfiguration.Where(x => x.ListType == ListType).FirstOrDefault();
                if (data != null)
                {
                    data.ListData = ListData;
                    _context.Entry(data).CurrentValues.SetValues(data);
                    _context.SaveChanges();
                }
                else
                {
                    _context.TblUserDynamicConfiguration.Add(tblUserDynamicConfiguration);
                    _context.SaveChanges();
                }
            }
            return null;
        }

        [HttpGet]
        public FileResult ExportToExcel()
        {
            SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
            conn.Close();
            SqlCommand cmd = new SqlCommand("select * from Students", conn);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            dt.TableName = "example";
            dt.AcceptChanges();
            da.Fill(dt);
            conn.Close();
            cmd.Dispose();

            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "sheet1");
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }
        }


        [HttpGet]
        public FileResult ExportStudentdata(int? firstItem, int? lastitem, string SearchTxt)
        {

            var studentdetails = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            List<AllStudentDetailsViewModel> alldata = new List<AllStudentDetailsViewModel>();
            FamilyDetail familydetails = new FamilyDetail();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var Bloodgroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();


            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Category").DataListId.ToString()).ToList();

            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

            var Religious = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();

            var Admissionprocess = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Admission Process").DataListId.ToString()).ToList();

            foreach (var items in studentdetails)
            {
                //var studentappno = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == items.ApplicationNumber && x.IsApprove != 192);
                //if(studentappno != null)
                {
                    familydetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == items.StudentRegisterID);
                }
                //if(studentappno != null && familydetails != null)
                {
                    alldata.Add(new AllStudentDetailsViewModel
                    {
                        StudentId = Convert.ToInt32(items.StudentRegisterID),
                        ApplicationNumber = items.ApplicationNumber,
                        UIN = items.UIN,
                        Date = items.Date,
                        Name = items.Name,
                        Class = Classes.FirstOrDefault(x => x.DataListItemId == items.Class_Id)?.DataListItemName,
                        Section = Section.FirstOrDefault(x => x.DataListItemId == items.Section_Id)?.DataListItemName,
                        Gender = items.Gender,
                        RTE = items.RTE,
                        Medium = items.Medium,
                        Caste = items.Caste,
                        AgeInWords = items.AgeInWords,
                        DOB = items.DOB,
                        POB = items.POB,
                        Nationality = items.Nationality,
                        Religion = Religious.FirstOrDefault(x => x.DataListItemId == items.Religion_Id)?.DataListItemName,
                        MotherTongue = items.MotherTongue,
                        Category = Category.FirstOrDefault(x => x.DataListItemId == items.Category_Id)?.DataListItemName,
                        BloodGroup = Bloodgroup.FirstOrDefault(x => x.DataListItemId == items.BloodGroup_Id)?.DataListItemName,
                        MedicalHistory = items.MedicalHistory,
                        Hobbies = items.Hobbies,
                        Sports = items.Sports,
                        OtherDetails = items.OtherDetails,
                        ProfileAvatar = items.ProfileAvatar,
                        MarkForIdentity = items.MarkForIdentity,
                        AdharNo = items.AdharNo,
                        AdharFile = items.AdharFile,
                        OtherLanguages = items.OtherLanguages,
                        IsApplyforTC = items.IsApplyforTC,
                        IsApplyforAdmission = items.IsApplyforAdmission,
                        Iapprovestudent = Admissionprocess.FirstOrDefault(x => x.DataListItemId == items.IsApprove)?.DataListItemName,
                        IsActive = items.IsActive,
                        IsAdmissionPaid = items.IsAdmissionPaid,
                        IsInsertFromAd = items.IsInsertFromAd,
                        RegNumber = items.ApplicationNumber,
                        Last_Name = items.Last_Name,
                        FatherName = familydetails == null ? "" : familydetails.FatherName,
                        MotherName = familydetails == null ? "" : familydetails.MotherName,
                        FMobile = familydetails == null ? "" : familydetails.FMobile,
                        MMobile = familydetails == null ? "" : familydetails.MMobile,
                        FResidentialAddress = familydetails == null ? "" : familydetails.FResidentialAddress,
                        ParentEmail = items.Parents_Email,
                        Added_Year = items.AddedYear,
                        Added_Date = items.AddedDate,
                        Promotion_Date = items.Promotion_Date,
                        Promotion_Year = items.Promotion_Year,
                        Registration_Date = items.Registration_Date

                    });
                }

            }

            //var AllStudentsList = alldata.ToList();
            //if (!string.IsNullOrEmpty(SearchTxt))
            //{
            //    alldata = AllStudentsList.Where(x => !string.IsNullOrEmpty(x.ApplicationNumber) && x.ApplicationNumber.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.UIN) && x.UIN.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Date) && x.Date.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Class) && x.Class.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Section) && x.Section.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.RTE) && x.RTE.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Medium) && x.Medium.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Caste) && x.Caste.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.DOB) && x.DOB.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.POB) && x.POB.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Nationality) && x.Nationality.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Religion) && x.Religion.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.MotherTongue) && x.MotherTongue.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Category) && x.Category.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.BloodGroup) && x.BloodGroup.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.MedicalHistory) && x.MedicalHistory.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Hobbies) && x.Hobbies.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Sports) && x.Sports.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.OtherDetails) && x.OtherDetails.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.ProfileAvatar) && x.ProfileAvatar.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.MarkForIdentity) && x.MarkForIdentity.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.AdharNo) && x.AdharNo.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.AdharFile) && x.AdharFile.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.OtherLanguages) && x.OtherLanguages.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.RegNumber) && x.RegNumber.ToLower().Contains(SearchTxt.ToLower())

            //    || !string.IsNullOrEmpty(x.Last_Name) && x.Last_Name.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.FatherName) && x.FatherName.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.MotherName) && x.MotherName.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.FMobile) && x.FMobile.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.MMobile) && x.MMobile.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.Added_Year) && x.Added_Year.ToLower().Contains(SearchTxt.ToLower())
            //     || !string.IsNullOrEmpty(x.Added_Date) && x.Added_Date.ToLower().Contains(SearchTxt.ToLower())
            //      || !string.IsNullOrEmpty(x.Registration_Date) && x.Registration_Date.ToLower().Contains(SearchTxt.ToLower())
            //      || !string.IsNullOrEmpty(x.Promotion_Date) && x.Promotion_Date.ToLower().Contains(SearchTxt.ToLower())
            //      || !string.IsNullOrEmpty(x.FResidentialAddress) && x.FResidentialAddress.ToLower().Contains(SearchTxt.ToLower())
            //    || !string.IsNullOrEmpty(x.ParentEmail) && x.ParentEmail.ToLower().Contains(SearchTxt.ToLower())).ToList();

            //}
            ////ViewBag.Allstudentcount = alldata.Count();
            //if (firstItem != null && lastitem != null)
            //{
            //    sbyte from = Convert.ToSByte(firstItem);
            //    sbyte to = Convert.ToSByte(lastitem);
            //    var filterdata = alldata.Skip(Math.Max(0, from - 1)).Take(to - (from - 1));
            //    alldata = filterdata.ToList();
            //}
            //ViewBag.Allstudentdetails = alldata;

            DataTable dt = new DataTable();
            int count = 1;
            var dynamicdata = _context.TblUserDynamicConfiguration.FirstOrDefault(x => x.ListType == "1");
            string validcols = dynamicdata == null ? "" : Convert.ToString(dynamicdata.ListData);
            string[] ColumnsArr = validcols.Split(',');

            foreach (var item in ColumnsArr)
            {
                if (item == "1")
                {
                    dt.Columns.Add("S.No", typeof(System.String));

                }
                if (item == "2")
                {
                    dt.Columns.Add("StudentId", typeof(System.String));

                }
                if (item == "3")
                {
                    dt.Columns.Add("ApplicationNumber", typeof(System.String));

                }
                if (item == "4")
                {
                    dt.Columns.Add("UIN", typeof(System.String));

                }
                if (item == "5")
                {
                    dt.Columns.Add("Date", typeof(System.String));

                }
                if (item == "6")
                {
                    dt.Columns.Add("Name", typeof(System.String));

                }
                if (item == "7")
                {
                    dt.Columns.Add("Class", typeof(System.String));

                }
                if (item == "8")
                {
                    dt.Columns.Add("Section", typeof(System.String));

                }
                if (item == "9")
                {
                    dt.Columns.Add("Gender", typeof(System.String));

                }
                if (item == "10")
                {
                    dt.Columns.Add("LastName", typeof(System.String));

                }
                if (item == "11")
                {
                    dt.Columns.Add("Medium", typeof(System.String));

                }
                if (item == "12")
                {
                    dt.Columns.Add("Caste", typeof(System.String));

                }
                if (item == "13")
                {
                    dt.Columns.Add("Age", typeof(System.String));

                }
                if (item == "14")
                {
                    dt.Columns.Add("DOB", typeof(System.String));

                }
                if (item == "15")
                {
                    dt.Columns.Add("POB", typeof(System.String));

                }
                if (item == "16")
                {
                    dt.Columns.Add("Nationality", typeof(System.String));

                }
                if (item == "17")
                {
                    dt.Columns.Add("Religion", typeof(System.String));

                }
                if (item == "18")
                {
                    dt.Columns.Add("MotherTongue", typeof(System.String));

                }
                if (item == "19")
                {
                    dt.Columns.Add("Category", typeof(System.String));

                }
                if (item == "20")
                {
                    dt.Columns.Add("BloodGroup", typeof(System.String));

                }
                if (item == "21")
                {
                    dt.Columns.Add("MedicalHistory", typeof(System.String));

                }
                if (item == "22")
                {
                    dt.Columns.Add("Hobbies", typeof(System.String));

                }
                if (item == "23")
                {
                    dt.Columns.Add("Sports", typeof(System.String));

                }
                if (item == "24")
                {
                    dt.Columns.Add("OtherDetails", typeof(System.String));

                }
                if (item == "25")
                {
                    dt.Columns.Add("ProfileAvatar", typeof(System.String));

                }
                if (item == "26")
                {
                    dt.Columns.Add("MarkForIdentity", typeof(System.String));

                }
                if (item == "27")
                {
                    dt.Columns.Add("AdharNo", typeof(System.String));

                }
                if (item == "28")
                {
                    dt.Columns.Add("AdharFile", typeof(System.String));

                }
                if (item == "29")
                {
                    dt.Columns.Add("OtherLanguages", typeof(System.String));

                }
                if (item == "30")
                {
                    dt.Columns.Add("IsApplyforTC", typeof(System.String));

                }
                if (item == "31")
                {
                    dt.Columns.Add("IsApplyforAdmission", typeof(System.String));

                }
                if (item == "32")
                {
                    dt.Columns.Add("IsApprove", typeof(System.String));

                }
                if (item == "33")
                {
                    dt.Columns.Add("IsActive", typeof(System.String));

                }
                if (item == "34")
                {
                    dt.Columns.Add("IsAdmissionPaid", typeof(System.String));

                }
                if (item == "35")
                {
                    dt.Columns.Add("IsInsertFromAd", typeof(System.String));

                }
                if (item == "36")
                {
                    dt.Columns.Add("RegNumber", typeof(System.String));

                }
                if (item == "37")
                {
                    dt.Columns.Add("FatherName", typeof(System.String));

                }
                if (item == "38")
                {
                    dt.Columns.Add("MotherName", typeof(System.String));

                }
                if (item == "39")
                {
                    dt.Columns.Add("FatherMobile", typeof(System.String));

                }
                if (item == "40")
                {
                    dt.Columns.Add("MotherMobile", typeof(System.String));

                }
                if (item == "41")
                {
                    dt.Columns.Add("Parent's Email", typeof(System.String));

                }
                if (item == "42")
                {
                    dt.Columns.Add("Address", typeof(System.String));

                }

            }

            foreach (var data in alldata)
            {
                DataRow dataRow = dt.NewRow();

                if (ColumnsArr.Contains("1"))
                {
                    dataRow["S.No"] = count;
                }
                if (ColumnsArr.Contains("2"))
                {
                    dataRow["StudentId"] = data.StudentId;
                }
                if (ColumnsArr.Contains("3"))
                {
                    dataRow["ApplicationNumber"] = data.ApplicationNumber;
                }
                if (ColumnsArr.Contains("4"))
                {
                    dataRow["UIN"] = data.UIN;
                }
                if (ColumnsArr.Contains("5"))
                {
                    dataRow["Date"] = data.Date;
                }
                if (ColumnsArr.Contains("6"))
                {
                    dataRow["Name"] = data.Name;
                }
                if (ColumnsArr.Contains("7"))
                {
                    dataRow["Class"] = data.Class;
                }
                if (ColumnsArr.Contains("8"))
                {
                    dataRow["Section"] = data.Section;
                }
                if (ColumnsArr.Contains("9"))
                {
                    dataRow["Gender"] = data.Gender;
                }
                if (ColumnsArr.Contains("10"))
                {
                    dataRow["LastName"] = data.Last_Name;
                }
                if (ColumnsArr.Contains("11"))
                {
                    dataRow["Medium"] = data.Medium;
                }
                if (ColumnsArr.Contains("12"))
                {
                    dataRow["Caste"] = data.Caste;
                }
                if (ColumnsArr.Contains("13"))
                {
                    dataRow["Age"] = data.AgeInWords;
                }
                if (ColumnsArr.Contains("14"))
                {
                    dataRow["DOB"] = data.DOB;
                }
                if (ColumnsArr.Contains("15"))
                {
                    dataRow["POB"] = data.POB;
                }
                if (ColumnsArr.Contains("16"))
                {
                    dataRow["Nationality"] = data.Nationality;
                }
                if (ColumnsArr.Contains("17"))
                {
                    dataRow["Religion"] = data.Religion;
                }
                if (ColumnsArr.Contains("18"))
                {
                    dataRow["MotherTongue"] = data.MotherTongue;
                }
                if (ColumnsArr.Contains("19"))
                {
                    dataRow["Category"] = data.Category;
                }
                if (ColumnsArr.Contains("20"))
                {
                    dataRow["BloodGroup"] = data.BloodGroup;
                }
                if (ColumnsArr.Contains("21"))
                {
                    dataRow["MedicalHistory"] = data.MedicalHistory;
                }
                if (ColumnsArr.Contains("22"))
                {
                    dataRow["Hobbies"] = data.Hobbies;
                }
                if (ColumnsArr.Contains("23"))
                {
                    dataRow["Sports"] = data.Sports;
                }
                if (ColumnsArr.Contains("24"))
                {
                    dataRow["OtherDetails"] = data.OtherDetails;
                }
                if (ColumnsArr.Contains("25"))
                {
                    dataRow["ProfileAvatar"] = data.ProfileAvatar;
                }
                if (ColumnsArr.Contains("26"))
                {
                    dataRow["MarkForIdentity"] = data.MarkForIdentity;
                }
                if (ColumnsArr.Contains("27"))
                {
                    dataRow["AdharNo"] = data.AdharNo;
                }
                if (ColumnsArr.Contains("28"))
                {
                    dataRow["AdharFile"] = data.AdharFile;
                }
                if (ColumnsArr.Contains("29"))
                {
                    dataRow["OtherLanguages"] = data.OtherLanguages;
                }
                if (ColumnsArr.Contains("30"))
                {
                    dataRow["IsApplyforTC"] = data.IsApplyforTC;
                }
                if (ColumnsArr.Contains("31"))
                {
                    dataRow["IsApplyforAdmission"] = data.IsApplyforAdmission;
                }
                if (ColumnsArr.Contains("32"))
                {
                    dataRow["IsApprove"] = data.Iapprovestudent;
                }
                if (ColumnsArr.Contains("33"))
                {
                    dataRow["IsActive"] = data.IsActive;
                }
                if (ColumnsArr.Contains("34"))
                {
                    dataRow["IsAdmissionPaid"] = data.IsAdmissionPaid;
                }
                if (ColumnsArr.Contains("35"))
                {
                    dataRow["IsInsertFromAd"] = data.IsInsertFromAd;
                }
                if (ColumnsArr.Contains("36"))
                {
                    dataRow["RegNumber"] = data.RegNumber;
                }
                if (ColumnsArr.Contains("37"))
                {
                    dataRow["FatherName"] = data.FatherName;
                }
                if (ColumnsArr.Contains("38"))
                {
                    dataRow["MotherName"] = data.MotherName;
                }
                if (ColumnsArr.Contains("39"))
                {
                    dataRow["FatherMobile"] = data.FMobile;
                }
                if (ColumnsArr.Contains("40"))
                {
                    dataRow["MotherMobile"] = data.MMobile;
                }
                if (ColumnsArr.Contains("41"))
                {
                    dataRow["Parent's Email"] = data.ParentEmail;
                }
                if (ColumnsArr.Contains("42"))
                {
                    dataRow["Address"] = data.FResidentialAddress;
                }
                dt.Rows.Add(dataRow);
                count++;



            }


            using (XLWorkbook wb = new XLWorkbook()) //Install ClosedXml from Nuget for XLWorkbook
            {

                wb.Worksheets.Add(dt, "sheet1");
                using (MemoryStream stream = new MemoryStream()) //using System.IO;  
                {
                    wb.SaveAs(stream);
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                }
            }

        }


        [HttpPost]
        public ActionResult StudentViewList(string SearchTxt, string first, string last)
        {
            //var AllStudentsList = _context.Students.ToList();
            var AllStudentsList = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            List<StudentsRegistration> alldata = new List<StudentsRegistration>();

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();

            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

            foreach (var item in AllStudentsList)
            {
                alldata.Add(new StudentsRegistration
                {
                    StudentRegisterID = Convert.ToInt32(item.StudentRegisterID),
                    UIN = item.UIN,
                    Date = item.Date,
                    Name = item.Name + " " + item.Last_Name,
                    Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                    Section = item.Section == null ? "" : Section.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,
                    Gender = item.Gender,
                    RTE = item.RTE,
                    Medium = item.Medium,
                    Caste = item.Caste,
                    AgeInWords = item.AgeInWords,
                    DOB = item.DOB,
                    POB = item.POB,
                    Nationality = item.Nationality,
                    Religion = item.Religion,
                    MotherTongue = item.MotherTongue,
                    Category = item.Category,
                    BloodGroup = item.BloodGroup,
                    MedicalHistory = item.MedicalHistory,
                    Hobbies = item.Hobbies,
                    Sports = item.Sports,
                    OtherDetails = item.OtherDetails,
                    ProfileAvatar = item.ProfileAvatar,
                    MarkForIdentity = item.MarkForIdentity,
                    AdharNo = item.AdharNo,
                    AdharFile = item.AdharFile,
                    OtherLanguages = item.OtherLanguages,
                    IsApplyforTC = item.IsApplyforTC,
                    IsApplyforAdmission = item.IsApplyforAdmission,
                    //= item.IsApprove,
                    IsActive = item.IsActive,
                    IsAdmissionPaid = item.IsAdmissionPaid,
                    IsInsertFromAd = item.IsInsertFromAd,
                    ApplicationNumber = item.ApplicationNumber
                });
            }

            if (!string.IsNullOrEmpty(SearchTxt))
            {
                alldata = AllStudentsList.Where(x => !string.IsNullOrEmpty(x.ApplicationNumber) && x.ApplicationNumber.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.UIN) && x.UIN.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Date) && x.Date.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Name) && x.Name.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Class) && x.Class.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Section) && x.Section.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Gender) && x.Gender.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.RTE) && x.RTE.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Medium) && x.Medium.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Caste) && x.Caste.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.DOB) && x.DOB.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.POB) && x.POB.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Nationality) && x.Nationality.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Religion) && x.Religion.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.MotherTongue) && x.MotherTongue.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Category) && x.Category.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.BloodGroup) && x.BloodGroup.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.MedicalHistory) && x.MedicalHistory.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Hobbies) && x.Hobbies.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.Sports) && x.Sports.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.OtherDetails) && x.OtherDetails.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.ProfileAvatar) && x.ProfileAvatar.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.MarkForIdentity) && x.MarkForIdentity.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.AdharNo) && x.AdharNo.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.AdharFile) && x.AdharFile.ToLower().Contains(SearchTxt.ToLower())
                || !string.IsNullOrEmpty(x.OtherLanguages) && x.OtherLanguages.ToLower().Contains(SearchTxt.ToLower())).ToList();
                //|| !string.IsNullOrEmpty(x.RegNumber) && x.RegNumber.ToLower().Contains(SearchTxt.ToLower())).ToList();



            }

            if (first != null && last != null)
            {
                sbyte from = Convert.ToSByte(first);
                sbyte to = Convert.ToSByte(last);

                var filteredData = alldata.Skip(Math.Max(0, from - 1)).Take(to - (from - 1));
                alldata = filteredData.ToList();
            }
            ViewBag.StudentList = alldata;

            return PartialView("_StudentViewList");
        }

        public JsonResult GeStudentList(string DDClass, string DDBatch)
        {
            List<StudentDetailVM> studentls = new List<StudentDetailVM>();
            var result = _context.Students.ToList();
            if (!string.IsNullOrEmpty(DDBatch))
            {
                result = result.Where(x => x.BatchName == DDBatch).ToList();
            }

            if (!string.IsNullOrEmpty(DDClass))
            {
                result = result.Where(x => x.Class == DDClass).ToList();
            }

            foreach (var res in result)
            {
                StudentDetailVM studentobj = new StudentDetailVM();

                studentobj.ScholarNumber = res.StudentId.ToString();
                studentobj.StudentName = res.Name;
                studentobj.Category = res.Category;
                studentobj.Course = res.Class;
                studentobj.Semester = res.Class;
                studentobj.StudentId = res.StudentId;
                studentls.Add(studentobj);
            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GeStudentList1(int classlist, int batchlist, int sectionlist, string promote)
        {

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            // var AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var AllBatchs = _context.Tbl_Batches./*Where(x => x.IsActiveForAdmission == true).*/ToList();

            List<Student> studentlist = new List<Student>();
            List<Student> StudentListout = new List<Student>();
            List<StudentDetailVM> studentDetailVMs = new List<StudentDetailVM>();
            var data = new List<Student>();
            var studentpromote = new List<Student>();

            if (sectionlist == 0)
            {
                //data = _context.Students.Where(x => x.Class_Id == classlist && x.Batch_Id == batchlist).ToList(); 
                data = _context.Students.Where(x => x.Class_Id == classlist && x.Batch_Id == batchlist && !x.IsApplyforTC).ToList(); //Confirm for both if and else (filter for TC)
                studentpromote = _context.Students.Where(x => x.Class_Id == classlist).ToList();
            }
            else
            {
                data = _context.Students.Where(x => x.Class_Id == classlist && x.Section_Id == sectionlist && x.Batch_Id == batchlist && !x.IsApplyforTC).ToList();
                studentpromote = _context.Students.Where(x => x.Class_Id == classlist && x.Section_Id == sectionlist).ToList();
            }

            var pagename = "Promotions";
            var createpermission = "Create_permission";

            //foreach (var item in data)
            //{
            //    var promotedata = studentpromote.FirstOrDefault(x => x.Student_Id == item.StudentId);
            //    if(promotedata != null)
            //    {
            //        StudentListout.Add(new Student
            //        {
            //            StudentId = item.StudentId,
            //            Class_Id = item.Class_Id,
            //            Name = item.Name ?? string.Empty,
            //            Last_Name= item.Last_Name ?? string.Empty,
            //            Gender = item.Gender ?? string.Empty,
            //            DOB = item.DOB ?? string.Empty,
            //            Batch_Id = item.Batch_Id,
            //            Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
            //            BatchName = AllBatchs.FirstOrDefault(x => x.DataListItemId == item.Batch_Id)?.DataListItemName

            //        });
            //    }

            //}
            if (promote != "null")
            {
                foreach (var item in data)
                {
                    var promotedata = studentpromote.FirstOrDefault(x => x.StudentId == item.StudentId);
                    if (promotedata != null)
                    {
                        studentDetailVMs.Add(new StudentDetailVM
                        {
                            StudentId = item.StudentId,
                            Class_Id = item.Class_Id,
                            Name = item.Name ?? string.Empty,
                            Last_Name = item.Last_Name ?? string.Empty,
                            Gender = item.Gender ?? string.Empty,
                            DOB = item.DOB ?? string.Empty,
                            Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                            BatchName = AllBatchs.FirstOrDefault(x => x.Batch_Id == item.Batch_Id)?.Batch_Name,
                            CreatePermission = CheckCreatepermission(pagename, createpermission)
                        });
                    }
                }


                var result = new List<tbl_StudentDetail>();
                result = _context.tbl_StudentDetails.ToList();

                result = result.Where(x => x.Class_Id == classlist).ToList();
                //result = result.Where(x => x.Batch_Id == batchlist).ToList();
                return Json(studentDetailVMs, JsonRequestBehavior.AllowGet);
            }
            else
            {
                foreach (var item in data)
                {
                    var promotedata = studentpromote.FirstOrDefault(x => x.StudentId == item.StudentId);
                    if (promotedata != null)
                    {
                        studentDetailVMs.Add(new StudentDetailVM
                        {
                            StudentId = item.StudentId,
                            Class_Id = item.Class_Id,
                            Name = item.Name ?? string.Empty,
                            Last_Name = item.Last_Name ?? string.Empty,
                            Gender = item.Gender ?? string.Empty,
                            DOB = item.DOB ?? string.Empty,
                            Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                            BatchName = AllBatchs.FirstOrDefault(x => x.Batch_Id == item.Batch_Id)?.Batch_Name,
                            CreatePermission = CheckCreatepermission(pagename, createpermission)
                        });
                    }
                }
                //foreach (Student student in data)
                //{
                //    var promotedata = studentpromote.FirstOrDefault(x => x.Student_Id == student.StudentId);

                //    if(promotedata == null)
                //    {
                //        StudentListout.Add(new Student
                //        {
                //            StudentId = student.StudentId,
                //            Class_Id = student.Class_Id,
                //            Name = student.Name ?? string.Empty,
                //            Last_Name = student.Last_Name ?? string.Empty,
                //            Gender = student.Gender ?? string.Empty,
                //            DOB = student.DOB ?? string.Empty,
                //            Batch_Id = student.Batch_Id,
                //            Class = Classes.FirstOrDefault(x => x.DataListItemId == student.Class_Id)?.DataListItemName,
                //            BatchName = AllBatchs.FirstOrDefault(x => x.DataListItemId == student.Batch_Id)?.DataListItemName
                //        });
                //    }
                //}
                return Json(studentDetailVMs, JsonRequestBehavior.AllowGet);
                //return Json(studentDetailVMs, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult ViewStudent(string studentId)
        {
            return View();
        }


        [HttpPost]
        public JsonResult getStudentDetailsByRegNo(string RegNo)
        {
            List<tbl_StudentDetail> res = _context.tbl_StudentDetails
                                         .Where(e => e.ScholarNumber == RegNo)
                                         .ToList();

            if (res != null && Convert.ToString(res[0].Addedon) != "")
            {
                DateTime AddedDate = Convert.ToDateTime(res[0].Addedon);
                DateTime CurrentDate = DateTime.Today;

                DateTime FirstYeartoSecondYear = CurrentDate.AddYears(-1);
                DateTime FirstYeartoAllumini = CurrentDate.AddYears(-2);

                //if today is any month after june 
                if (CurrentDate.Month > 6)
                {
                    //check if its june
                    if (CurrentDate.Month == 7)
                    {
                        if (CurrentDate.Day > 30)
                        {
                            //its 1st year student, so transfer, or display 2nd year 
                            if (AddedDate.Year == FirstYeartoSecondYear.Year)
                            {
                                res[0].Years = "II";
                            }
                            else if (AddedDate.Year == FirstYeartoAllumini.Year)
                            {
                                //Added 2 years back transfer to allumini.
                                //not sure what to set here for now.
                            }
                            else
                            {
                                //any past date its allumini.
                            }
                        }
                    }
                    else
                    {
                        //its 1st year student, so transfer, or display 2nd year 
                        if (AddedDate.Year == FirstYeartoSecondYear.Year)
                        {
                            res[0].Years = "II";
                        }
                        else if (AddedDate.Year == FirstYeartoAllumini.Year)
                        {
                            //Added 2 years back transfer to allumini.
                            //not sure what to set here for now.
                        }
                        else
                        {
                            //any past date its allumni
                        }
                    }
                }
            }
            return Json(res);
        }

        [HttpPost]
        public JsonResult GetAllSkillsetByRegNo(string RegNo)
        {
            List<tbl_skillset> res = _context.tbl_skillsets
                                         .Where(e => e.ScholarNumber == RegNo)
                                         .ToList();
            return Json(res);
        }
        public JsonResult getAllAcademicDetailsByRegNo(string RegNo)
        {
            List<tbl_AcademicDetail> res = _context.tbl_AcademicDetails
                                          .Where(e => e.ScholarNumber == RegNo)
                                          .ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getInternshipDetailsByRegNo(string RegNo)
        {
            List<tbl_SummerInternship> res = _context.tbl_SummerInternships
                                         .Where(e => e.ScholarNumber == RegNo)
                                         .ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDeclarationDetailsByRegNo(string RegNo)
        {
            List<tbl_Declaration> res = _context.tbl_Declarations
                                          .Where(e => e.ScholarNumber == RegNo)
                                          .ToList();
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadSkillSetDetails(string RegNo)
        {
            var skillset = skill.GetByRegNo(RegNo).FirstOrDefault();
            SkillSetViewModel skillSetViewModel = new SkillSetViewModel();
            if (skillset != null)
            {
                skillSetViewModel.SkillsetId = skillset.SkillsetId.ToString();
                skillSetViewModel.Adaptabilitytochange = skillset.Adaptabilitytochange == "Yes" ? true : false;
                skillSetViewModel.Communication = skillset.Communication == "Yes" ? true : false;
                skillSetViewModel.Compering = skillset.Compering == "Yes" ? true : false;
                skillSetViewModel.Contentwriting = skillset.Contentwriting == "Yes" ? true : false;
                skillSetViewModel.CoralDraw = skillset.CoralDraw == "Yes" ? true : false;
                skillSetViewModel.Dancing = skillset.Dancing == "Yes" ? true : false;
                skillSetViewModel.Drawing = skillset.Drawing == "Yes" ? true : false;
                skillSetViewModel.Initiative = skillset.Initiative == "Yes" ? true : false;
                skillSetViewModel.Interpersonalskills = skillset.Interpersonalskills == "Yes" ? true : false;
                skillSetViewModel.Leadership = skillset.Leadership == "Yes" ? true : false;
                skillSetViewModel.Photoshop = skillset.Photoshop == "Yes" ? true : false;
                skillSetViewModel.Singing = skillset.Singing == "Yes" ? true : false;
                skillSetViewModel.Strategicthinking = skillset.Strategicthinking == "Yes" ? true : false;
                skillSetViewModel.Teamwork = skillset.Teamwork == "Yes" ? true : false;
                skillSetViewModel.Timemanagement = skillset.Timemanagement == "Yes" ? true : false;
                skillSetViewModel.Problemsolving = skillset.Problemsolving == "Yes" ? true : false;
            }
            return Json(skillSetViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getInternshipDetailsByRegNoFoeEdit(string RegNo)
        {
            return Json(summr.GetByRegNo(RegNo), JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDeclarationDetailsByRegNoEdit(string RegNo)
        {
            tbl_Declaration res = _context.tbl_Declarations
                                          .FirstOrDefault(e => e.ScholarNumber == RegNo);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateStatus(string RegNo)
        {
            tbl_StudentDetail otablestudent = _context.tbl_StudentDetails.FirstOrDefault(x => x.ScholarNumber == RegNo);
            string name = "";
            //if (Session["role"].ToString() == "admin" || Session["role"].ToString() == "staff")
            //{
            //    name = HttpContext.Current.Session["firstname"].ToString() + " " + HttpContext.Current.Session["lastname"].ToString();
            //}
            //else
            //{
            //    name = "";
            //}
            if (otablestudent != null)
            {
                otablestudent.Spare1 = name;
                otablestudent.Spare2 = DateTime.Today.ToString("dd-MM-yyyy");
                otablestudent.Spare3 = "Confirm";
                _context.tbl_StudentDetails.AddOrUpdate(otablestudent);
                _context.SaveChanges();
                StudentController.CreateStudentCredentails(RegNo, otablestudent.MobileNo, otablestudent.EmailId, otablestudent.StudentId);
                return Json("1", JsonRequestBehavior.AllowGet);
            }
            return Json("0", JsonRequestBehavior.AllowGet);
        }

        public ActionResult Promotions()
        {
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
            ViewBag.AllYears = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "year").DataListId.ToString()).ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            //ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            ViewBag.Sections = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            //ViewBag.Batchlist = _context.Tbl_Batches.ToList();
            //ViewBag.AllBatchs = _context.Tbl_Batches.ToList();
            ViewBag.Batches = new SelectList(_context.Tbl_Batches.ToList().OrderBy(x => x.Batch_Name).ToList(), "id", "BatchName");
            //ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.AcadamicYear = _context.Tbl_Batches.ToList();
            //var Batch = _context.Tbl_Batches.ToList();
            var batches = _context.Tbl_Batches.ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");
            return View();
        }
        public ActionResult PromotionsReport()
        {
            List<PromotionReportList> promotionReportList = new List<PromotionReportList>();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            var semesters = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "semester").DataListId.ToString()).ToList();
            //var semesters = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "semester").DataListId.ToString()).ToList();
            var allStudents = _context.Students;
            ViewBag.Batchlist = _context.Tbl_Batches.ToList();
            var batches = _context.Tbl_Batches./*Where(x => x.IsActiveForAdmission == true).*/ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");
            var allPromotions = _context.Tbl_StudentPromotes;
            int i = 1;
            foreach (var item in Classes)
            {
                int ClassStudents = allStudents.Where(x => x.Class_Id == item.DataListItemId).Count();
                int allPromoted = allPromotions.Where(x => x.ToClass_Id == item.DataListItemId).Count();
                promotionReportList.Add(new PromotionReportList()
                {
                    SNO = i,
                    ClassName = item.DataListItemName,
                    AllStudents = ClassStudents,
                    Promoted = allPromoted,
                    NotPromoted = (ClassStudents - allPromoted) < 0 ? 0 : (ClassStudents - allPromoted),
                    ClassId = item.DataListItemId
                });
                i++;
            }
            return View(promotionReportList);
        }

        public JsonResult AddPromotion(Tbl_StudentPromote tbl_StudentPromote)
        {
            try
            {
                var data = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                var studentadmissiondatalist = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
                var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

                //List<StudentsRegistration> StudentDetails = new List<StudentsRegistration>();


                var student = _context.Tbl_StudentPromotes.FirstOrDefault(x => x.Student_Id == tbl_StudentPromote.Student_Id);
                if (student != null)
                {
                    tbl_StudentPromote.PromoteId = student.PromoteId;
                    tbl_StudentPromote.Firstname = student.Firstname;
                    tbl_StudentPromote.Lastname = student.Lastname;
                    tbl_StudentPromote.Registration_Date = student.Registration_Date;
                    _context.Entry(student).CurrentValues.SetValues(tbl_StudentPromote);
                    _context.SaveChanges();

                }
                else
                {
                    //var studentdata = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                    //var studentadmisiondata = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                    //tbl_StudentPromote.Firstname = studentdata.Name;
                    //tbl_StudentPromote.Lastname = studentdata.Last_Name;
                    //tbl_StudentPromote.FromClass_Id = data.Class_Id;
                    //tbl_StudentPromote.Registration_Date = studentadmisiondata.Registration_Date;
                    //tbl_StudentPromote.FromClass = Classes.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
                    //tbl_StudentPromote.Student_Id = data.StudentId;
                    //_context.Tbl_StudentPromotes.Add(tbl_StudentPromote);
                    //_context.SaveChanges();
                    var studentdata = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                    var studentadmisiondata = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                    tbl_StudentPromote.Firstname = studentdata.Name;
                    tbl_StudentPromote.Lastname = studentdata.Last_Name;
                    tbl_StudentPromote.FromClass_Id = data.Class_Id;
                    tbl_StudentPromote.ToSection = Section.FirstOrDefault(x => x.DataListItemId == data.StudentId)?.DataListItemName;

                    tbl_StudentPromote.Registration_Date = studentadmisiondata.Registration_Date;
                    tbl_StudentPromote.FromClass = Classes.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
                    tbl_StudentPromote.Student_Id = data.StudentId;
                    _context.Tbl_StudentPromotes.Add(tbl_StudentPromote);
                    _context.SaveChanges();
                }




                Student tblstudent = new Student
                {
                    StudentId = data.StudentId,
                    Name = data.Name,
                    UIN = data.UIN,
                    ApplicationNumber = data.ApplicationNumber,
                    Class_Id = tbl_StudentPromote.ToClass_Id,
                    Class = Classes.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.ToClass_Id)?.DataListItemName,
                    Date = data.Date,
                    Section = Section.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.Section_Id)?.DataListItemName,
                    Batch_Id = tbl_StudentPromote.Batch_Id,
                    CurrentYear = tbl_StudentPromote.Batch_Id,
                    BatchName = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == tbl_StudentPromote.Batch_Id).Batch_Name,
                    Gender = data.Gender,
                    RTE = data.RTE,
                    Medium = data.Medium,
                    Caste = data.Caste,
                    AgeInWords = data.AgeInWords,
                    DOB = data.DOB,
                    POB = data.POB,
                    Nationality = data.Nationality,
                    Religion = data.Religion,
                    MotherTongue = data.MotherTongue,
                    Category = data.Category,
                    BloodGroup = data.BloodGroup,
                    MedicalHistory = data.MedicalHistory,
                    Hobbies = data.Hobbies,
                    Sports = data.Sports,
                    OtherDetails = data.OtherDetails,
                    ProfileAvatar = data.ProfileAvatar,
                    MarkForIdentity = data.MarkForIdentity,
                    AdharNo = data.AdharNo,
                    AdharFile = data.AdharFile,
                    OtherLanguages = data.OtherLanguages,
                    IsApplyforTC = data.IsApplyforTC,
                    IsApplyforAdmission = data.IsApplyforAdmission,
                    IsApprove = 217, //what code should be here like 217
                    IsActive = data.IsActive,
                    IsAdmissionPaid = data.IsAdmissionPaid,
                    IsInsertFromAd = data.IsInsertFromAd,
                    RegNumber = data.RegNumber,
                    ParentEmail = data.ParentEmail,
                    City = studentadmissiondatalist.City,
                    State = studentadmissiondatalist.State,
                    Pincode = studentadmissiondatalist.Pincode,
                    BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,
                    Category_Id = studentadmissiondatalist.Category_Id,
                    Last_Name = studentadmissiondatalist.Last_Name
                };
                _context.Entry(data).CurrentValues.SetValues(tblstudent);
                _context.SaveChanges();
                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime date1 = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                string year = Convert.ToDateTime(date1).Year.ToString();

                //var userlist = _context.Tbl_UserManagement.FirstOrDefault(x => x.)

                StudentsRegistration studentsRegistration = new StudentsRegistration
                {
                    StudentRegisterID = studentadmissiondatalist.StudentRegisterID,
                    ApplicationNumber = studentadmissiondatalist.ApplicationNumber,

                    UIN = studentadmissiondatalist.UIN,
                    Date = studentadmissiondatalist.Date,
                    Name = studentadmissiondatalist.Name,
                    Class = studentadmissiondatalist.Class,
                    Section = studentadmissiondatalist.Section,
                    Gender = studentadmissiondatalist.Gender,

                    Medium = studentadmissiondatalist.Medium,
                    Caste = studentadmissiondatalist.Caste,

                    AgeInWords = studentadmissiondatalist.AgeInWords,
                    DOB = studentadmissiondatalist.DOB,

                    POB = studentadmissiondatalist.POB,

                    Nationality = studentadmissiondatalist.Nationality,
                    Religion = studentadmissiondatalist.Religion,

                    MotherTongue = studentadmissiondatalist.MotherTongue,
                    Category = studentadmissiondatalist.Category,

                    BloodGroup = studentadmissiondatalist.BloodGroup,

                    Hobbies = studentadmissiondatalist.Hobbies,
                    Sports = studentadmissiondatalist.Sports,
                    OtherDetails = studentadmissiondatalist.OtherDetails,
                    ProfileAvatar = studentadmissiondatalist.ProfileAvatar,
                    MarkForIdentity = studentadmissiondatalist.MarkForIdentity,
                    AdharNo = studentadmissiondatalist.AdharNo,
                    AdharFile = studentadmissiondatalist.AdharFile,
                    OtherLanguages = studentadmissiondatalist.OtherLanguages,
                    IsApplyforTC = studentadmissiondatalist.IsApplyforTC,
                    IsApplyforAdmission = studentadmissiondatalist.IsApplyforAdmission,
                    IsApprove = studentadmissiondatalist.IsApprove, // what code should be here 217
                    IsActive = studentadmissiondatalist.IsActive,
                    IsAdmissionPaid = studentadmissiondatalist.IsAdmissionPaid,

                    Email = studentadmissiondatalist.Email,

                    Parents_Email = studentadmissiondatalist.Parents_Email,

                    Class_Id = tbl_StudentPromote.ToClass_Id,

                    Class_Name = studentadmissiondatalist.Class_Name,

                    Section_Id = studentadmissiondatalist.Section_Id,



                    Last_Name = studentadmissiondatalist.Last_Name,



                    BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,

                    Religion_Id = studentadmissiondatalist.Religion_Id,

                    Cast_Id = studentadmissiondatalist.Cast_Id,

                    Category_Id = studentadmissiondatalist.Category_Id,



                    Mobile = studentadmissiondatalist.Mobile,

                    AdmissionFeePaid = studentadmissiondatalist.AdmissionFeePaid,

                    City = studentadmissiondatalist.City,

                    State = studentadmissiondatalist.State,

                    Pincode = studentadmissiondatalist.Pincode,

                    AddedYear = year,

                    Registration_Date = studentadmissiondatalist.Registration_Date,
                    IsEmailsent = studentadmissiondatalist.IsEmailsent,
                    Promotion_Date = DateTime.Now.ToString("dd/MM/yyyy"),
                    Promotion_Year = year,
                    UserId = studentadmissiondatalist.UserId

                };
                _context.Entry(studentadmissiondatalist).CurrentValues.SetValues(studentsRegistration);
                _context.SaveChanges();

                SendEmail("" + studentsRegistration.Parents_Email + "", "Application of " + tblstudent.Name + " has been moved to the " + tblstudent.Class + " ", "Your Application (" + tblstudent.ApplicationNumber + ") has been Promoted to " + tblstudent.Class + "  successfully.");


                EmailViewModel emailViewModel = new EmailViewModel();
                emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                emailViewModel.Email_Date = DateTime.Now.ToString();
                emailViewModel.Email_Content = "Promotion to" + tblstudent.Class;

                var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);

                return Json("Student has promoted");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }

        public JsonResult SendPromotion(Tbl_StudentPromote tbl_StudentPromote)
        {

            try
            {
                List<Tbl_StudentPromote> tbl_Promote = new List<Tbl_StudentPromote>();

                //foreach(var item in tbl_Promote)
                //{ }

                var data = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                var studentadmissiondatalist = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                var studentReg = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == data.ApplicationNumber).FirstOrDefault(); //Arvind Added
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
                var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

                //List<StudentsRegistration> StudentDetails = new List<StudentsRegistration>();



                var student = _context.Tbl_StudentPromotes.FirstOrDefault(x => x.Student_Id == tbl_StudentPromote.Student_Id);
                if (student != null)
                {
                    string GetScroll = (data.ScholarNo).ToString();
                    tbl_StudentPromote.ScholarNumber = GetScroll;
                    tbl_StudentPromote.PromoteId = student.PromoteId;
                    tbl_StudentPromote.Firstname = student.Firstname;
                    tbl_StudentPromote.Lastname = student.Lastname;
                    tbl_StudentPromote.Registration_Date = student.Registration_Date;
                    _context.Entry(student).CurrentValues.SetValues(tbl_StudentPromote);
                    _context.SaveChanges();

                }
                else
                {

                    var studentdata = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                    var studentadmisiondata = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                    tbl_StudentPromote.Firstname = studentdata.Name;
                    tbl_StudentPromote.Lastname = studentdata.Last_Name;
                    tbl_StudentPromote.FromClass_Id = data.Class_Id;
                    tbl_StudentPromote.ToSection = Section.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.Section_Id)?.DataListItemName;

                    tbl_StudentPromote.Registration_Date = studentadmisiondata.Registration_Date;
                    tbl_StudentPromote.FromClass = Classes.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
                    tbl_StudentPromote.Student_Id = data.StudentId;
                    _context.Tbl_StudentPromotes.Add(tbl_StudentPromote);
                    _context.SaveChanges();
                }

                #region //Arvind Added


                // Retrieve the entity you want to update
                var familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentReg.StudentRegisterID);

                if (familyDetail != null)
                {
                    // Modify the desired column value
                    familyDetail.ApplicationNumber = data.ApplicationNumber;

                    // Save changes
                    _context.SaveChanges();
                }
                #endregion
                //---xrnik
                string Date = DateTime.Now.ToString("dd/MM/yyyy");
                DateTime date1 = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                string year = Convert.ToDateTime(date1).Year.ToString();
                //---
                Student tblstudent = new Student
                {
                    StudentId = data.StudentId,
                    Name = data.Name,
                    UIN = data.UIN,
                    ApplicationNumber = data.ApplicationNumber,
                    ScholarNo = data.ScholarNo,// Convert.ToUInt32(data.ApplicationNumber),  //---x-rnik
                    Class_Id = tbl_StudentPromote.ToClass_Id,
                    Class = Classes.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.ToClass_Id)?.DataListItemName,
                    Date = year,// data.Date,  //---x-rnik ----- using date column to store promoted year
                    Section = Section.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.Section_Id)?.DataListItemName,

                    Batch_Id = tbl_StudentPromote.Batch_Id,
                    BatchName = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == tbl_StudentPromote.Batch_Id).Batch_Name,
                    Section_Id = tbl_StudentPromote.Section_Id,
                    CurrentYear = data.CurrentYear,
                    Gender = data.Gender,
                    RTE = data.RTE,
                    Medium = studentadmissiondatalist.Medium,//data.Medium ---x-rnik,
                    Caste = data.Caste,
                    AgeInWords = data.AgeInWords,
                    DOB = data.DOB,
                    POB = data.POB,
                    Nationality = data.Nationality,
                    Religion = data.Religion,
                    MotherTongue = data.MotherTongue,
                    Category = data.Category,
                    BloodGroup = data.BloodGroup,
                    MedicalHistory = data.MedicalHistory,
                    Hobbies = data.Hobbies,
                    Sports = data.Sports,
                    OtherDetails = data.OtherDetails,
                    ProfileAvatar = data.ProfileAvatar,
                    MarkForIdentity = data.MarkForIdentity,
                    AdharNo = data.AdharNo,
                    AdharFile = data.AdharFile,
                    OtherLanguages = data.OtherLanguages,
                    IsApplyforTC = data.IsApplyforTC,
                    IsApplyforAdmission = data.IsApplyforAdmission,
                    IsApprove = 217,//data.IsApprove,  //x-rnik---
                    IsActive = data.IsActive,
                    IsAdmissionPaid = data.IsAdmissionPaid,
                    IsInsertFromAd = data.IsInsertFromAd,
                    RegNumber = data.RegNumber,
                    ParentEmail = data.ParentEmail,
                    City = studentadmissiondatalist.City,
                    State = studentadmissiondatalist.State,
                    Pincode = studentadmissiondatalist.Pincode,
                    BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,
                    Category_Id = studentadmissiondatalist.Category_Id,
                    Last_Name = studentadmissiondatalist.Last_Name
                };
                _context.Entry(data).CurrentValues.SetValues(tblstudent);
                _context.SaveChanges();


                //var userlist = _context.Tbl_UserManagement.FirstOrDefault(x => x.)


                StudentsRegistration studentsRegistration = new StudentsRegistration
                {
                    StudentRegisterID = studentadmissiondatalist.StudentRegisterID,
                    ApplicationNumber = studentadmissiondatalist.ApplicationNumber,

                    UIN = studentadmissiondatalist.UIN,
                    Date = studentadmissiondatalist.Date,
                    Name = studentadmissiondatalist.Name,
                    Class = studentadmissiondatalist.Class,
                    Section = studentadmissiondatalist.Section,
                    Gender = studentadmissiondatalist.Gender,
                    Batch_Id = studentadmissiondatalist.Batch_Id,
                    Medium = studentadmissiondatalist.Medium,
                    Caste = studentadmissiondatalist.Caste,

                    AgeInWords = studentadmissiondatalist.AgeInWords,
                    DOB = studentadmissiondatalist.DOB,

                    POB = studentadmissiondatalist.POB,

                    Nationality = studentadmissiondatalist.Nationality,
                    Religion = studentadmissiondatalist.Religion,

                    MotherTongue = studentadmissiondatalist.MotherTongue,
                    Category = studentadmissiondatalist.Category,

                    BloodGroup = studentadmissiondatalist.BloodGroup,

                    Hobbies = studentadmissiondatalist.Hobbies,
                    Sports = studentadmissiondatalist.Sports,
                    OtherDetails = studentadmissiondatalist.OtherDetails,
                    ProfileAvatar = studentadmissiondatalist.ProfileAvatar,
                    MarkForIdentity = studentadmissiondatalist.MarkForIdentity,
                    AdharNo = studentadmissiondatalist.AdharNo,
                    AdharFile = studentadmissiondatalist.AdharFile,
                    OtherLanguages = studentadmissiondatalist.OtherLanguages,
                    IsApplyforTC = studentadmissiondatalist.IsApplyforTC,
                    IsApplyforAdmission = studentadmissiondatalist.IsApplyforAdmission,
                    IsApprove = 191,// studentadmissiondatalist.IsApprove, //--x-rnik
                    IsActive = studentadmissiondatalist.IsActive,
                    IsAdmissionPaid = studentadmissiondatalist.IsAdmissionPaid,

                    Email = studentadmissiondatalist.Email,

                    Parents_Email = studentadmissiondatalist.Parents_Email,

                    //Class_Id = tbl_StudentPromote.ToClass_Id, //--x-rnik
                    Class_Id = studentadmissiondatalist.Class_Id, //--x-rnik

                    Class_Name = studentadmissiondatalist.Class_Name,

                    Section_Id = studentadmissiondatalist.Section_Id,



                    Last_Name = studentadmissiondatalist.Last_Name,



                    BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,

                    Religion_Id = studentadmissiondatalist.Religion_Id,

                    Cast_Id = studentadmissiondatalist.Cast_Id,

                    Category_Id = studentadmissiondatalist.Category_Id,



                    Mobile = studentadmissiondatalist.Mobile,

                    AdmissionFeePaid = studentadmissiondatalist.AdmissionFeePaid,

                    City = studentadmissiondatalist.City,

                    State = studentadmissiondatalist.State,

                    Pincode = studentadmissiondatalist.Pincode,

                    AddedYear = studentadmissiondatalist.AddedYear, //--x-rnik

                    Registration_Date = studentadmissiondatalist.Registration_Date,
                    IsEmailsent = studentadmissiondatalist.IsEmailsent,
                    Promotion_Date = DateTime.Now.ToString("dd/MM/yyyy"),
                    Promotion_Year = year,
                    UserId = studentadmissiondatalist.UserId

                };
                _context.Entry(studentadmissiondatalist).CurrentValues.SetValues(studentsRegistration);
                _context.SaveChanges();


                SendEmail("" + studentsRegistration.Parents_Email + "", "Application of " + tblstudent.Name + " has been moved to the " + tblstudent.Class + " ", "Your Application (" + tblstudent.ApplicationNumber + ") has been Promoted to " + tblstudent.Class + "  successfully.");


                EmailViewModel emailViewModel = new EmailViewModel();
                emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                emailViewModel.Email_Date = DateTime.Now.ToString();
                emailViewModel.Email_Content = "Promotion to" + tblstudent.Class;

                var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);

                return Json("Student has promoted");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }

        public JsonResult SendMultiplePromotion(List<Tbl_StudentPromote> tbl_StudentPromoteList)
        {

            try
            {
                List<Tbl_StudentPromote> tbl_Promote = new List<Tbl_StudentPromote>();

                foreach (var tbl_StudentPromote in tbl_StudentPromoteList)
                {

                    var data = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                    var studentadmissiondatalist = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                    //Arvind Added
                    var studentReg = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == data.ApplicationNumber).FirstOrDefault();
                    //Arvind Added
                    var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                    var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
                    var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

                    //List<StudentsRegistration> StudentDetails = new List<StudentsRegistration>();


                    var student = _context.Tbl_StudentPromotes.FirstOrDefault(x => x.Student_Id == tbl_StudentPromote.Student_Id);
                    if (student != null)
                    {
                        string GetScroll = data.ApplicationNumber;
                        tbl_StudentPromote.ScholarNumber = GetScroll;
                        tbl_StudentPromote.PromoteId = student.PromoteId;
                        tbl_StudentPromote.Firstname = student.Firstname;
                        tbl_StudentPromote.Lastname = student.Lastname;
                        tbl_StudentPromote.Registration_Date = student.Registration_Date;
                        _context.Entry(student).CurrentValues.SetValues(tbl_StudentPromote);
                        _context.SaveChanges();

                    }
                    else
                    {

                        var studentdata = _context.Students.FirstOrDefault(x => x.StudentId == tbl_StudentPromote.Student_Id);
                        var studentadmisiondata = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                        tbl_StudentPromote.Firstname = studentdata.Name;
                        tbl_StudentPromote.Lastname = studentdata.Last_Name;
                        tbl_StudentPromote.FromClass_Id = data.Class_Id;
                        tbl_StudentPromote.ToSection = Section.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.Section_Id)?.DataListItemName;

                        tbl_StudentPromote.Registration_Date = studentadmisiondata.Registration_Date;
                        tbl_StudentPromote.FromClass = Classes.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
                        tbl_StudentPromote.Student_Id = data.StudentId;
                        _context.Tbl_StudentPromotes.Add(tbl_StudentPromote);
                        _context.SaveChanges();
                    }

                    #region Arvind Added a code.. 
                    // Retrieve the entity you want to update
                    //var familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentReg.StudentRegisterID);

                    //if (familyDetail != null)
                    //{
                    //    // Modify the desired column value
                    //    familyDetail.ApplicationNumber = data.ApplicationNumber;

                    //    // Save changes
                    //    _context.SaveChanges();
                    //}
                    #endregion
                    Student tblstudent = new Student
                    {
                        StudentId = data.StudentId,
                        Name = data.Name,
                        UIN = data.UIN,
                        ApplicationNumber = data.ApplicationNumber,
                        //ScholarNo = Convert.ToUInt32(data.ApplicationNumber), //DB 64bits field, model 64bits. then why convert to 32
                        ScholarNo = Convert.ToInt64(data.ScholarNo),
                        Class_Id = tbl_StudentPromote.ToClass_Id,
                        Class = Classes.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.ToClass_Id)?.DataListItemName,
                        Date = data.Date,
                        Section = Section.FirstOrDefault(x => x.DataListItemId == tbl_StudentPromote.Section_Id)?.DataListItemName,
                        Batch_Id = tbl_StudentPromote.Batch_Id,
                        CurrentYear = data.CurrentYear,
                        BatchName = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == tbl_StudentPromote.Batch_Id).Batch_Name,
                        Section_Id = tbl_StudentPromote.Section_Id,
                        Gender = data.Gender,
                        RTE = data.RTE,
                        Medium = data.Medium,
                        Caste = data.Caste,
                        AgeInWords = data.AgeInWords,
                        DOB = data.DOB,
                        POB = data.POB,
                        Nationality = data.Nationality,
                        Religion = data.Religion,
                        MotherTongue = data.MotherTongue,
                        Category = data.Category,
                        BloodGroup = data.BloodGroup,
                        MedicalHistory = data.MedicalHistory,
                        Hobbies = data.Hobbies,
                        Sports = data.Sports,
                        OtherDetails = data.OtherDetails,
                        ProfileAvatar = data.ProfileAvatar,
                        MarkForIdentity = data.MarkForIdentity,
                        AdharNo = data.AdharNo,
                        AdharFile = data.AdharFile,
                        OtherLanguages = data.OtherLanguages,
                        IsApplyforTC = data.IsApplyforTC,
                        IsApplyforAdmission = data.IsApplyforAdmission,
                        IsApprove = 217,
                        IsActive = data.IsActive,
                        IsAdmissionPaid = data.IsAdmissionPaid,
                        IsInsertFromAd = data.IsInsertFromAd,
                        RegNumber = data.RegNumber,
                        ParentEmail = data.ParentEmail,
                        City = studentadmissiondatalist.City,
                        State = studentadmissiondatalist.State,
                        Pincode = studentadmissiondatalist.Pincode,
                        BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,
                        Category_Id = studentadmissiondatalist.Category_Id,
                        Last_Name = studentadmissiondatalist.Last_Name
                    };
                    _context.Entry(data).CurrentValues.SetValues(tblstudent);
                    _context.SaveChanges();
                    string Date = DateTime.Now.ToString("dd/MM/yyyy");
                    DateTime date1 = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    string year = Convert.ToDateTime(date1).Year.ToString();

                    //var userlist = _context.Tbl_UserManagement.FirstOrDefault(x => x.)

                    StudentsRegistration studentsRegistration = new StudentsRegistration
                    {
                        StudentRegisterID = studentadmissiondatalist.StudentRegisterID,
                        ApplicationNumber = studentadmissiondatalist.ApplicationNumber,

                        UIN = studentadmissiondatalist.UIN,
                        Date = studentadmissiondatalist.Date,
                        Name = studentadmissiondatalist.Name,
                        Class = studentadmissiondatalist.Class,
                        Section = studentadmissiondatalist.Section,
                        Gender = studentadmissiondatalist.Gender,
                        Batch_Id = tbl_StudentPromote.Batch_Id,

                        Medium = studentadmissiondatalist.Medium,
                        Caste = studentadmissiondatalist.Caste,

                        AgeInWords = studentadmissiondatalist.AgeInWords,
                        DOB = studentadmissiondatalist.DOB,

                        POB = studentadmissiondatalist.POB,

                        Nationality = studentadmissiondatalist.Nationality,
                        Religion = studentadmissiondatalist.Religion,

                        MotherTongue = studentadmissiondatalist.MotherTongue,
                        Category = studentadmissiondatalist.Category,

                        BloodGroup = studentadmissiondatalist.BloodGroup,

                        Hobbies = studentadmissiondatalist.Hobbies,
                        Sports = studentadmissiondatalist.Sports,
                        OtherDetails = studentadmissiondatalist.OtherDetails,
                        ProfileAvatar = studentadmissiondatalist.ProfileAvatar,
                        MarkForIdentity = studentadmissiondatalist.MarkForIdentity,
                        AdharNo = studentadmissiondatalist.AdharNo,
                        AdharFile = studentadmissiondatalist.AdharFile,
                        OtherLanguages = studentadmissiondatalist.OtherLanguages,
                        IsApplyforTC = studentadmissiondatalist.IsApplyforTC,
                        IsApplyforAdmission = studentadmissiondatalist.IsApplyforAdmission,
                        IsApprove = 191,
                        IsActive = studentadmissiondatalist.IsActive,
                        IsAdmissionPaid = studentadmissiondatalist.IsAdmissionPaid,

                        Email = studentadmissiondatalist.Email,

                        Parents_Email = studentadmissiondatalist.Parents_Email,

                        Class_Id = tbl_StudentPromote.ToClass_Id,

                        Class_Name = studentadmissiondatalist.Class_Name,

                        Section_Id = studentadmissiondatalist.Section_Id,



                        Last_Name = studentadmissiondatalist.Last_Name,



                        BloodGroup_Id = studentadmissiondatalist.BloodGroup_Id,

                        Religion_Id = studentadmissiondatalist.Religion_Id,

                        Cast_Id = studentadmissiondatalist.Cast_Id,

                        Category_Id = studentadmissiondatalist.Category_Id,



                        Mobile = studentadmissiondatalist.Mobile,

                        AdmissionFeePaid = studentadmissiondatalist.AdmissionFeePaid,

                        City = studentadmissiondatalist.City,

                        State = studentadmissiondatalist.State,

                        Pincode = studentadmissiondatalist.Pincode,

                        AddedYear = studentadmissiondatalist.AddedYear,
                        CurrentYear = studentadmissiondatalist.CurrentYear,
                        Registration_Date = studentadmissiondatalist.Registration_Date,
                        IsEmailsent = studentadmissiondatalist.IsEmailsent,
                        Promotion_Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        Promotion_Year = year,
                        UserId = studentadmissiondatalist.UserId

                    };
                    _context.Entry(studentadmissiondatalist).CurrentValues.SetValues(studentsRegistration);
                    _context.SaveChanges();

                    SendEmail("" + studentsRegistration.Parents_Email + "", "Application of " + tblstudent.Name + " has been moved to the " + tblstudent.Class + " ", "Your Application (" + tblstudent.ApplicationNumber + ") has been Promoted to " + tblstudent.Class + "  successfully.");


                    EmailViewModel emailViewModel = new EmailViewModel();
                    emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                    emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                    emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                    emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                    emailViewModel.Email_Date = DateTime.Now.ToString();
                    emailViewModel.Email_Content = "Promotion to" + tblstudent.Class;

                    var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);


                }
                return Json("Students has promoted");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }
        public JsonResult PromoteFromClass(int StudentID)
        {
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            var data = _context.Students.FirstOrDefault(x => x.StudentId == StudentID);
            data.Class = Classes.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public static void CreateStudentCredentails(string RegNo, string mob, string email, long id)
        {
            ApplicationDbContext context = new ApplicationDbContext();
            Random rnd = new Random();
            int rndnumber = rnd.Next(1, 999999);
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            PasswordHasher PasswordHash = new PasswordHasher();
            if (UserManager.FindByEmail(email) == null)
            {
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    PhoneNumber = mob,
                    PasswordHash = PasswordHash.HashPassword("Demo" + rndnumber),
                    UserId = Convert.ToInt32(id)
                };

                IdentityResult result = UserManager.Create(admin);
                string Id = UserManager.FindByEmail(admin.Email).Id;
                UserManager.AddToRole(Id, "Student");
            }
        }

        public ActionResult VewAllStudent()
        {
            var allStudent = _context.Students.ToList();
            ViewBag.allStudent = allStudent;
            ViewBag.totalStudent = _context.Students.Count();
            ViewBag.BatcheNames = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            ViewBag.AllQualifications = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "qualification").DataListId.ToString()).ToList();
            return View();
        }

        public ActionResult FullStudentView(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {

                //var studentreportdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID.ToString() == id);
                //var studentdatalist = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.StudentRegisterID.ToString() == id).ToList();
                var allStudentData = _context.Students.Where(x => x.StudentId.ToString() == id && x.IsApprove != 192).ToList();
                var studentdata = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == id);
                var studentdatalist = _context.StudentsRegistrations.Where(x => x.ApplicationNumber != studentdata.ApplicationNumber).ToList();

                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                var Bloodgroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();
                var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();
                var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Caste").DataListId.ToString()).ToList();
                List<Student> students = new List<Student>();
                foreach (var item in allStudentData)
                {
                    item.Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName;
                    item.BloodGroup = Bloodgroup.FirstOrDefault(x => x.DataListItemId == item.BloodGroup_Id)?.DataListItemName;
                    item.Religion = Religion.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Religion))?.DataListItemName;
                    item.Caste = Caste.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Caste))?.DataListItemName;
                    students.Add(item);
                }

                string imageName = "";
                if (studentdatalist.Count > 0)
                {
                    imageName = studentdatalist[0].ProfileAvatar;
                    imageName = "/WebsiteImages/StudentPhoto/" + imageName;
                }
                ViewBag.ImageName = imageName;
                ViewBag.allStudentData = students;
                ViewBag.allData = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

                ViewBag.allGardianData = _context.GuardianDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

                ViewBag.allAdditionalData = _context.AdditionalInformations.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

                ViewBag.allPastData = _context.PastSchoolingReports.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();
                //       List<StudentAttendance> studentAttendance = new List<StudentAttendance>();
                //         var studentName = studentdatalist.Select(x => x.Name).FirstOrDefault();
                //         var studentAttendanceData = (from st in _context.Tbl_StudentAttendance
                //                                      join batch in _context.Tbl_Batches
                //                                      on st.BatchId equals batch.Batch_Id
                //                                      where st.Student_Name == studentName
                //                                      select new
                //                                      {
                //                                          BatchName = batch.Batch_Name,
                //                                          ClassName = st.Class_Name,
                //                                          Section = st.Section_Name,
                //                                          CreatedDate = st.Created_Date
                //                                      }).ToList();
                //         var _studentAttendance = studentAttendanceData
                //             .GroupBy(x => new { x.BatchName, x.ClassName, x.Section })
                //             .Select(group =>
                //             {
                //                 var totalDays = (group.Max(x => DateTime.TryParse(x.CreatedDate, out var maxDate) ? maxDate : DateTime.MinValue) -
                //                                  group.Min(x => DateTime.TryParse(x.CreatedDate, out var minDate) ? minDate : DateTime.MinValue)).Days;

                //                 // Calculate attendance percentage per group (e.g., number of present days / total days)
                //                 var attendancePercentage = totalDays > 0 ?
                //                                     (group.Count(x => x.CreatedDate != null) / (decimal)totalDays) * 100 : 0;

                //                 return new SchoolManagement.Website.ViewModels.StudentAttendance
                //                 {
                //                     BatchName = group.Key.BatchName,
                //                     ClassName = group.Key.ClassName,
                //                     Section = group.Key.Section,
                //                     TotalDays = totalDays,
                //                     AttendancePercentage = attendancePercentage
                //                 };
                //             })
                //             .OrderByDescending(c => c.BatchName)
                //             .ToList();

                //         ViewBag.studentAttendanceData = studentAttendance;
                //         var groupedData = _studentAttendance
                //.GroupBy(x => x.BatchName)
                //.Select(group => new BatchAttendance
                //{
                //    BatchName = group.Key,
                //    TotalDaysSum = group.Sum(x => x.TotalDays),
                //    AverageAttendancePercentage = group.Average(x => x.AttendancePercentage),
                //    AttendanceDetails = group.Select(x => new AttendanceDetail
                //    {
                //        ClassName = x.ClassName,
                //        Section = x.Section,
                //        TotalDays = x.TotalDays,
                //        AttendancePercentage = x.AttendancePercentage
                //    }).ToList()
                //})
                //.ToList();
                //         ViewBag.GroupedData = groupedData;
                var studentIdLong = Convert.ToInt64(id);
                var rawAttendance = _context.Tbl_StudentAttendance
                    .Where(x => x.StudentRegisterID == studentIdLong)
                    .ToList();
                var batchList = _context.Tbl_Batches.ToList();
                var dataItems = _context.DataListItems.ToList();

                var attendanceList = rawAttendance
                    .GroupBy(x => new { x.BatchId, x.Class_Id, x.Section_Id })
                    .Select(group =>
                    {
                        var batchName = batchList.FirstOrDefault(b => b.Batch_Id == group.Key.BatchId)?.Batch_Name ?? "";
                        var className = dataItems.FirstOrDefault(c => c.DataListItemId == group.Key.Class_Id)?.DataListItemName ?? "";
                        var sectionName = dataItems.FirstOrDefault(s => s.DataListItemId == group.Key.Section_Id)?.DataListItemName ?? "";

                        double fullDays = group.Count(a => a.Mark_FullDayAbsent == "True");
                        double halfDays = group.Count(a => a.Mark_HalfDayAbsent == "True") * 0.5;
                        double otherDays = group.Count(a => a.Others == "True");

                        double totalPresent = fullDays + halfDays + otherDays;
                        double totalRecords = group.Count();

                        decimal percentage = totalRecords > 0
                            ? (decimal)((totalPresent / totalRecords) * 100)
                            : 0;

                        return new StudentAttendance
                        {
                            BatchName = batchName,
                            ClassName = className,
                            Section = sectionName,
                            TotalDays = (int)totalPresent,
                            AttendancePercentage = Math.Round(percentage, 2)
                        };
                    })
                    .ToList();

                ViewBag.studentAttendanceData = attendanceList;

                var result = (from tom in _context.Tbl_TestRecord
                              join tomarks in _context.tbl_TestObtainedMark
                                  on tom.RecordID equals tomarks.RecordIDFK
                              join t in _context.tbl_Tests
                                  on tomarks.TestID equals t.TestID
                              join b in _context.Tbl_Batches
                                  on tom.BatchId equals b.Batch_Id into batchJoin
                              from b in batchJoin.DefaultIfEmpty()
                              join cls in _context.DataListItems
                                  on tom.ClassID equals cls.DataListItemId into classJoin
                              from cls in classJoin.DefaultIfEmpty()
                              join sec in _context.DataListItems
                                  on tom.SectionID equals sec.DataListItemId into secJoin
                              from sec in secJoin.DefaultIfEmpty()
                              join trm in _context.tbl_Term
                                  on t.TermID equals trm.TermID into termJoin
                              from trm in termJoin.DefaultIfEmpty()
                              where tom.StudentID == studentIdLong && t.IsOptional == false
                              group new { tomarks.ObtainedMarks } by new
                              {
                                  BatchName = b.Batch_Name,
                                  ClassName = cls.DataListItemName,
                                  Section = sec.DataListItemName,
                                  TermName = trm.TermName,
                                  TermId = t.TermID
                              } into g
                              orderby g.Key.BatchName descending, g.Key.TermId descending
                              select new StudentMarkBatchWise
                              {
                                  BatchName = g.Key.BatchName,
                                  ClassName = g.Key.ClassName,
                                  Section = g.Key.Section,
                                  TermName = g.Key.TermName,
                                  TermId = g.Key.TermId,
                                  TotalObtainedMarks = g.Sum(x => x.ObtainedMarks).ToString()
                              }).ToList();

                ViewBag.BatchResult = result;
                var gradeMap = new Dictionary<string, int>
                {
                    { "A", 4 }, { "B", 3 }, { "C", 2 }, { "D", 1 }, { "E", 0 }, { "F", 0 }
                };


                var result2 = (from r in _context.tbl_CoScholastic_Result
                               join g in _context.tbl_CoScholasticObtainedGrade on r.Id equals g.ObtainedCoScholasticID
                               join b in _context.Tbl_Batches on g.BatchId equals b.Batch_Id
                               join t in _context.tbl_Term on r.TermID equals t.TermID
                               join c in _context.tbl_CoScholastic on g.CoscholasticID equals c.Id into cs
                               from c in cs.DefaultIfEmpty()
                               join cls in _context.DataListItems on r.ClassID equals cls.DataListItemId into classGroup
                               from cls in classGroup.DefaultIfEmpty()
                               join sec in _context.DataListItems on r.SectionId equals sec.DataListItemId into secGroup
                               from sec in secGroup.DefaultIfEmpty()
                               where r.StudentID == studentIdLong && g.ObtainedGrade != null
                               select new
                               {
                                   r.StudentID,
                                   b.Batch_Name,
                                   t.TermName,
                                   ClassName = cls.DataListItemName,
                                   SectionName = sec.DataListItemName,
                                   CoscholasticName = c.Title,
                                   g.ObtainedGrade
                               })
                .AsEnumerable()
                .GroupBy(x => new { x.Batch_Name, x.TermName, x.ClassName, x.SectionName })
                .Select(grp => new SchoolManagement.Website.ViewModels.CoScholasticReportViewModel
                {
                    BatchName = grp.Key.Batch_Name,
                    TermName = grp.Key.TermName,
                    ClassName = grp.Key.ClassName,
                    SectionName = grp.Key.SectionName,
                    CoscholasticGrades = string.Join(", ", grp
                        .Select(g => $"{g.CoscholasticName} - {g.ObtainedGrade}")),
                    AvgGrade = GetAvgLetterGrade(grp
                        .Where(x => gradeMap.ContainsKey(x.ObtainedGrade))
                        .Select(x => gradeMap[x.ObtainedGrade]))
                })
                .ToList();

                ViewBag.CoScholastic_Result = result2;

            }

            return View();
        }



        private void LoadStudentData(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var allStudentData = _context.Students.Where(x => x.StudentId.ToString() == id && x.IsApprove != 192).ToList();
                var studentdata = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == id);
                var studentdatalist = _context.StudentsRegistrations.Where(x => x.ApplicationNumber != studentdata.ApplicationNumber).ToList();

                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                var Bloodgroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();
                var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();
                var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Caste").DataListId.ToString()).ToList();

                List<Student> students = new List<Student>();
                foreach (var item in allStudentData)
                {
                    item.Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName;
                    item.BloodGroup = Bloodgroup.FirstOrDefault(x => x.DataListItemId == item.BloodGroup_Id)?.DataListItemName;
                    item.Religion = Religion.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Religion))?.DataListItemName;
                    item.Caste = Caste.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Caste))?.DataListItemName;
                    students.Add(item);
                }

                string imageName = "";
                if (studentdatalist.Count > 0)
                {
                    imageName = studentdatalist[0].ProfileAvatar;
                    imageName = "~/WebsiteImages/StudentPhoto/" + imageName;
                }

                ViewBag.ImageName = imageName;
                ViewBag.allStudentData = students;
                ViewBag.allData = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();
                ViewBag.allGardianData = _context.GuardianDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();
                ViewBag.allAdditionalData = _context.AdditionalInformations.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();
                ViewBag.allPastData = _context.PastSchoolingReports.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();
            }
        }

        public ActionResult ViewIdcard(string id)
        {
            //if (!string.IsNullOrEmpty(id))
            //{

            //    var allStudentData = _context.Students.Where(x => x.StudentId.ToString() == id && x.IsApprove != 192).ToList();
            //    var studentdata = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == id);
            //    var studentdatalist = _context.StudentsRegistrations.Where(x => x.ApplicationNumber != studentdata.ApplicationNumber).ToList();

            //    var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            //    var Bloodgroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();
            //    var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();
            //    var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Caste").DataListId.ToString()).ToList();
            //    List<Student> students = new List<Student>();
            //    foreach (var item in allStudentData)
            //    {
            //        item.Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName;
            //        item.BloodGroup = Bloodgroup.FirstOrDefault(x => x.DataListItemId == item.BloodGroup_Id)?.DataListItemName;
            //        item.Religion = Religion.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Religion))?.DataListItemName;
            //        item.Caste = Caste.FirstOrDefault(x => x.DataListItemId == Convert.ToInt32(item.Caste))?.DataListItemName;
            //        students.Add(item);
            //    }

            //    string imageName = "";
            //    if (studentdatalist.Count > 0)
            //    {
            //        imageName = studentdatalist[0].ProfileAvatar;
            //        imageName = "~/WebsiteImages/StudentPhoto/" + imageName;
            //    }
            //    ViewBag.ImageName = imageName;
            //    ViewBag.allStudentData = students;
            //    ViewBag.allData = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

            //    ViewBag.allGardianData = _context.GuardianDetails.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

            //    ViewBag.allAdditionalData = _context.AdditionalInformations.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();

            //    ViewBag.allPastData = _context.PastSchoolingReports.Where(x => x.ApplicationNumber == studentdata.ApplicationNumber).ToList();


            //}
            LoadStudentData(id);
            return View();
        }
      
        public ActionResult ViewIdcard1(string id)
        {
            LoadStudentData(id);
            return View();
        }
        public ActionResult ViewIdcard2(string id)
        {
            LoadStudentData(id);
            return View();
        }

        private static string GetAvgLetterGrade(IEnumerable<int> grades)
        {
            if (!grades.Any()) return null;

            double avg = grades.Average();
            int rounded = (int)Math.Round(avg);

            string letter = null;
            switch (rounded)
            {
                case 4:
                    letter = "A";
                    break;
                case 3:
                    letter = "B";
                    break;
                case 2:
                    letter = "C";
                    break;
                case 1:
                    letter = "D";
                    break;
                case 0:
                    letter = "E";
                    break;
            }
            return letter;
        }


        //update Student Records
        public JsonResult GetStudentByIDForEdit(int studentId)
        {
            Student Student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            return Json(Student, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAdditionalInformationsStudentByIDForEdit(int studentId)
        {
            AdditionalInformation AdditionalInformation = _context.AdditionalInformations.FirstOrDefault(x => x.StudentRefId == studentId);

            return Json(AdditionalInformation, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetFamilyDetailStudentByIDForEdit(int studentId)
        {
            FamilyDetail FamilyDetail = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentId);

            return Json(FamilyDetail, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetGuardianDetailsStudentByIDForEdit(int studentId)
        {
            GuardianDetails GuardianDetails = _context.GuardianDetails.FirstOrDefault(x => x.StudentRefId == studentId);

            return Json(GuardianDetails, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPastSchoolingReportsStudentByIDForEdit(int studentId)
        {
            PastSchoolingReport PastSchoolingReport = _context.PastSchoolingReports.FirstOrDefault(x => x.StudentRefId == studentId);

            return Json(PastSchoolingReport, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentRemoteAccessIDForEdit(int studentId)
        {
            StudentRemoteAccess StudentRemoteAccess = _context.StudentRemoteAccess.FirstOrDefault(x => x.StudentRefId == studentId);

            return Json(StudentRemoteAccess, JsonRequestBehavior.AllowGet);
        }

        //Edit Student
        public ActionResult EditStudentDetail(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel)
        {
            if (studentViewModel.Student.StudentId > 0)
            {
                var student = _context.Students.FirstOrDefault(x => x.StudentId == studentViewModel.Student.StudentId);
                if (student != null)
                {
                    if (uploadFilesViewModel.ProfileAvatar != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), fileName);
                            uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                            studentViewModel.Student.ProfileAvatar = fileName;
                        }
                    }
                    else
                    {
                        studentViewModel.Student.ProfileAvatar = student.ProfileAvatar;
                    }

                    if (uploadFilesViewModel.AdharFile != null)
                    {
                        if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                            uploadFilesViewModel.AdharFile.SaveAs(path);
                            studentViewModel.Student.AdharFile = fileName;
                        }
                    }
                    else
                    {
                        studentViewModel.Student.AdharFile = student.AdharFile;
                    }

                    _context.Entry(student).CurrentValues.SetValues(studentViewModel.Student);
                    _context.SaveChanges();

                    //_context.Students.AddOrUpdate(studentViewModel.Student);
                    //_context.SaveChanges();


                    studentViewModel.AdditionalInformation.StudentRefId = studentViewModel.Student.StudentId;
                    studentViewModel.FamilyDetail.StudentRefId = studentViewModel.Student.StudentId;
                    studentViewModel.GuardianDetails.StudentRefId = studentViewModel.Student.StudentId;
                    studentViewModel.PastSchoolingReport.StudentRefId = studentViewModel.Student.StudentId;

                    studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                    studentViewModel.FamilyDetail.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                    studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                    studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                    //studentViewModel.StudentRemoteAccess.StudentRefId = studentViewModel.Student.StudentId;

                    //_context.AdditionalInformations.AddOrUpdate(studentViewModel.AdditionalInformation);
                    //_context.SaveChanges();


                    //var ExitAddInfo = _context.AdditionalInformations.FirstOrDefault(x => x.StudentRefId == studentViewModel.Student.StudentId);
                    var ExitAddInfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (ExitAddInfo != null)
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.BirthCertificateAvatar = ExitAddInfo.BirthCertificateAvatar;

                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = ExitAddInfo.ThreePassportSizePhotographs;

                        }
                        if (uploadFilesViewModel.IncomeCertificate != null)
                        {
                            if (uploadFilesViewModel.IncomeCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.IncomeCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.IncomeCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.IncomeCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.CastCertificate = ExitAddInfo.CastCertificate;

                        }
                        if (uploadFilesViewModel.CastCertificate != null)
                        {
                            if (uploadFilesViewModel.CastCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CastCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CastCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.CastCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.CastCertificate = ExitAddInfo.CastCertificate;

                        }
                        if (uploadFilesViewModel.FatherAdhar != null)
                        {
                            if (uploadFilesViewModel.FatherAdhar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.FatherAdhar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.FatherAdhar.SaveAs(path);
                                studentViewModel.AdditionalInformation.FatherAdhar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.FatherAdhar = ExitAddInfo.FatherAdhar;

                        }
                        if (uploadFilesViewModel.MotherAdhar != null)
                        {
                            if (uploadFilesViewModel.MotherAdhar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MotherAdhar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MotherAdhar.SaveAs(path);
                                studentViewModel.AdditionalInformation.MotherAdhar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.MotherAdhar = ExitAddInfo.MotherAdhar;

                        }

                        if (uploadFilesViewModel.Ssmid != null)
                        {
                            if (uploadFilesViewModel.Ssmid.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.Ssmid.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.Ssmid.SaveAs(path);
                                studentViewModel.AdditionalInformation.Ssmid = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.Ssmid = ExitAddInfo.Ssmid;

                        }
                        if (uploadFilesViewModel.BankBook != null)
                        {
                            if (uploadFilesViewModel.BankBook.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BankBook.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BankBook.SaveAs(path);
                                studentViewModel.AdditionalInformation.BankBook = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.BankBook = ExitAddInfo.BankBook;

                        }
                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ProgressReport = ExitAddInfo.ProgressReport;

                        }
                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.MigrationCertificate = ExitAddInfo.MigrationCertificate;

                        }

                        studentViewModel.AdditionalInformation.Id = ExitAddInfo.Id;
                        _context.Entry(ExitAddInfo).CurrentValues.SetValues(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }


                    // var ExitFamilyDetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentViewModel.Student.StudentId);
                    var ExitFamilyDetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (ExitFamilyDetails != null)
                    {
                        studentViewModel.FamilyDetail.Id = ExitFamilyDetails.Id;
                        _context.Entry(ExitFamilyDetails).CurrentValues.SetValues(studentViewModel.FamilyDetail);
                        _context.SaveChanges();
                    }


                    //_context.FamilyDetails.AddOrUpdate(studentViewModel.FamilyDetail);
                    //_context.SaveChanges();

                    //var ExitGuardianDetails = _context.GuardianDetails.FirstOrDefault(x => x.StudentRefId == studentViewModel.Student.StudentId);
                    var ExitGuardianDetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (ExitGuardianDetails != null)
                    {
                        studentViewModel.GuardianDetails.Id = ExitGuardianDetails.Id;
                        _context.Entry(ExitGuardianDetails).CurrentValues.SetValues(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }


                    //_context.GuardianDetails.AddOrUpdate(studentViewModel.GuardianDetails);
                    //_context.SaveChanges();

                    var ExitPastSchoolingReports = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    //var ExitPastSchoolingReports = _context.PastSchoolingReports.FirstOrDefault(x => x.StudentRefId == studentViewModel.Student.StudentId);
                    if (ExitPastSchoolingReports != null)
                    {
                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                            else
                            {
                                studentViewModel.PastSchoolingReport.TCAvatar = ExitPastSchoolingReports.TCAvatar;

                            }
                        }
                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.MarksCardAvatar = ExitPastSchoolingReports.MarksCardAvatar;

                        }
                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = ExitPastSchoolingReports.CharacterConductCertificateAvatar;

                        }
                        studentViewModel.PastSchoolingReport.Id = ExitPastSchoolingReports.Id;

                        _context.Entry(ExitPastSchoolingReports).CurrentValues.SetValues(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }

                    //_context.PastSchoolingReports.AddOrUpdate(studentViewModel.PastSchoolingReport);
                    //_context.SaveChanges();

                    //var ExitStudentRemoteAccess = _context.StudentRemoteAccess.FirstOrDefault(x => x.StudentRefId == studentViewModel.Student.StudentId);
                    //if (ExitStudentRemoteAccess != null)
                    //{                    
                    //    //studentViewModel.StudentRemoteAccess.Id = ExitStudentRemoteAccess.Id;
                    //    _context.Entry(ExitStudentRemoteAccess).CurrentValues.SetValues(studentViewModel.StudentRemoteAccess);
                    //    _context.SaveChanges();
                    //}
                    //_context.StudentRemoteAccess.AddOrUpdate(studentViewModel.StudentRemoteAccess);
                    //_context.SaveChanges();
                }
            }
            return RedirectToAction("ManageStudent");
        }

        [HttpGet]
        public JsonResult DeleteStudent(int id)
        {
            try
            {
                var student = _context.Students.FirstOrDefault(x => x.StudentId == id);
                var studentapplicationno = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
                var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
                var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
                var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
                var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);


                if (student != null && familydetails != null && guardiandetails != null && additionalinfo != null && pastschoolingrecord != null)
                {
                    _context.Students.Remove(student);
                    _context.SaveChanges();

                    _context.FamilyDetails.Remove(familydetails);
                    _context.SaveChanges();

                    _context.GuardianDetails.Remove(guardiandetails);
                    _context.SaveChanges();

                    _context.AdditionalInformations.Remove(additionalinfo);
                    _context.SaveChanges();

                    _context.PastSchoolingReports.Remove(pastschoolingrecord);
                    _context.SaveChanges();
                }
                return Json(new { success = true, message = "Student Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("Something Went Wrong");
            }


        }

        public JsonResult GetAllTCAppliedStudents(string batch, string className)
        {
            List<Student> studentList = new List<Student>();
            var students = _context.Students.Where(x => !x.IsDeleted && x.IsApplyforTC);
            if (!string.IsNullOrEmpty(batch))
            {
                students = students.Where(x => x.BatchName == batch);
            }

            if (!string.IsNullOrEmpty(className))
            {
                students = students.Where(x => x.Class == className);
            }

            foreach (Student item in students)
            {
                studentList.Add(new Student { StudentId = item.StudentId, Name = item.Name });
            }

            return Json(studentList, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> AdmissionRegistration(/*string Email, string DOB, string Name*/)
        {
            //if(Email != null || Email != "" && DOB != null || DOB != "" && Name != null || Name != "")
            //{
            //    ViewBag.Email = Email;
            //    ViewBag.Dob = DOB;
            //    ViewBag.Name = Name;
            //    ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            //    ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            //    ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            //    ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            //    ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //    ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            //    ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            //    ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            //    ViewBag.Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //    //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //    //ViewBag.StudentNo = _context.Students.Count() + 101;

            //    ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");
            //    return View();
            //}
            //else
            {

                //ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

                //ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
                //ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
                //ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
                //ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                //ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                //ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
                //ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
                //ViewBag.Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
                //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
                //ViewBag.StudentNo = _context.Students.Count() + 101;

                //ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");
                //return View();

                await BindViewBag();
                return View();
            }


        }

        [HttpPost]
        public async Task<ActionResult> AdmissionRegistration(StudentAdmissionViewModel studentViewModel)
        {
            await BindViewBag();
            if (!ModelState.IsValid)
            {
                return View();
            }
            ViewBag.UIN = "EMIS/" + _context.Students.Count();
            //PaymentViewModels objPaymentViewModels = new PaymentViewModels();
            //objPaymentViewModels.BloodGroup = studentViewModel.BloodGroup;
            //objPaymentViewModels.Category = studentViewModel.Category;
            //objPaymentViewModels.Class = studentViewModel.Class;
            //objPaymentViewModels.DOB = studentViewModel.DOB;
            //objPaymentViewModels.Gender = studentViewModel.Gender;
            //objPaymentViewModels.MotherTongue = studentViewModel.MotherTongue;
            //objPaymentViewModels.Name = studentViewModel.Name;
            //objPaymentViewModels.Nationality = studentViewModel.Nationality;
            //objPaymentViewModels.POB = studentViewModel.POB;
            //objPaymentViewModels.Religion = studentViewModel.Religion;
            //objPaymentViewModels.Nationality = studentViewModel.Nationality;

            try
            {
                if (!string.IsNullOrEmpty(studentViewModel.Name))
                {
                    #region Hide student registration 
                    //Student student = new Student()
                    //{
                    //    BloodGroup = studentViewModel.BloodGroup,
                    //    //ApplicationNumber = studentViewModel.ApplicationNumber,
                    //    Category = studentViewModel.Category,
                    //    Class = studentViewModel.Class,
                    //    //Date = studentViewModel.Date,
                    //    DOB = studentViewModel.DOB,
                    //    Gender = studentViewModel.Gender,
                    //    MotherTongue = studentViewModel.MotherTongue,
                    //    Name = studentViewModel.Name,
                    //    Caste = studentViewModel.Caste,
                    //    Nationality = studentViewModel.Nationality,
                    //    POB = studentViewModel.POB,
                    //    // ProfileAvatar = studentViewModel.ProfileAvatar,
                    //    Religion = studentViewModel.Religion,
                    //    //UIN = studentViewModel.Student.UIN,
                    //    IsApplyforAdmission = true,
                    //    //IsApprove = studentViewModel.IsApproved,
                    //    ApplicationNumber = Guid.NewGuid().ToString(),
                    //    UIN = Guid.NewGuid().ToString(),
                    //    IsActive = true,

                    //};
                    //if (studentViewModel.ProfileAvatar != null)
                    //{
                    //    if (studentViewModel.ProfileAvatar.ContentLength > 0)
                    //    {
                    //        var fileName = Path.GetFileName(studentViewModel.ProfileAvatar.FileName);
                    //        var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), fileName);
                    //        studentViewModel.ProfileAvatar.SaveAs(path);
                    //        student.ProfileAvatar = fileName;
                    //    }
                    //}

                    //var userdetails = _context.Students.Add(student);
                    //await _context.SaveChangesAsync();
                    #endregion
                    var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                    var studentCategory = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "admission process").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == "submitted");
                    StudentsRegistration StudentsRegistration = new StudentsRegistration()
                    {
                        BloodGroup = studentViewModel.BloodGroup,
                        ApplicationNumber = trackId.ToString(),
                        Category = studentViewModel.Category,
                        Class = studentViewModel.Class,
                        Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        DOB = studentViewModel.DOB,
                        Gender = studentViewModel.Gender,
                        MotherTongue = studentViewModel.MotherTongue,
                        Name = studentViewModel.Name,
                        Caste = studentViewModel.Caste,
                        Nationality = studentViewModel.Nationality,
                        POB = studentViewModel.POB,
                        // ProfileAvatar = studentViewModel.ProfileAvatar,
                        Religion = studentViewModel.Religion,
                        IsApplyforAdmission = true,
                        IsApprove = studentCategory.DataListItemId,
                        UIN = Guid.NewGuid().ToString(),
                        IsActive = true,
                        Email = studentViewModel.Email,
                        LastStudiedSchoolName = studentViewModel.LaststudiedSchoolName,

                    };
                    if (studentViewModel.ProfileAvatar != null)
                    {
                        if (studentViewModel.ProfileAvatar.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(studentViewModel.ProfileAvatar.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), fileName);
                            studentViewModel.ProfileAvatar.SaveAs(path);
                            StudentsRegistration.ProfileAvatar = fileName;
                        }
                    }
                    _StudentsRegistration.Insert(StudentsRegistration);
                    _StudentsRegistration.Save();


                    studentViewModel.FamilyDetail.ApplicationNumber = trackId.ToString();
                    _FamilyDetail.Insert(studentViewModel.FamilyDetail);
                    _FamilyDetail.Save();
                    #region User Creation 
                    //var userdetails = _context.StudentsRegistrations.Add(StudentsRegistration);
                    //await _context.SaveChangesAsync();
                    //Tbl_UserManagementViewModel objTbl_UserManagementViewModel = new Tbl_UserManagementViewModel();
                    //objTbl_UserManagementViewModel.Description = "Student registration";
                    //objTbl_UserManagementViewModel.Email = "student@gmail.com";
                    //objTbl_UserManagementViewModel.Password = "Student@123";
                    //objTbl_UserManagementViewModel.UserName = studentViewModel.Name + "1";
                    //objTbl_UserManagementViewModel.UserRole = "Student";
                    //objTbl_UserManagementViewModel.UserId = userdetails.StudentId;
                    //var UserMngID = await CreateUser(objTbl_UserManagementViewModel);
                    //var existingobj = _context.Students.FirstOrDefault(e => e.StudentId == userdetails.StudentId);
                    //Studentupdate studentobj = new Studentupdate()
                    //{
                    //    UserId = UserMngID
                    //};
                    //_context.Entry(existingobj).CurrentValues.SetValues(studentobj);
                    //_context.SaveChanges();
                    #endregion
                }
                // TempData["objPaymentViewModels"] = objPaymentViewModels;
                return RedirectToAction("StudentAdmission", "StudentAdmission");  //by jairam regarding payment include 15/10/2021
                //return RedirectToAction("StudnetPaymentSummary");
            }
            catch (System.Exception ex)
            {
                await BindViewBag();
                ViewBag.Error("Some error occured,Please contact with Administrator");
                return View();
            }

        }


        private async Task BindViewBag()
        {
            ViewBag.BloodGroup = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToListAsync();
            ViewBag.Religion = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToListAsync();
            ViewBag.Group = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToListAsync();
            ViewBag.Class = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToListAsync();
            ViewBag.Caste = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToListAsync();
            ViewBag.Category = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToListAsync();

            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");
        }

        public string CreateUser(Tbl_UserManagementViewModel tbl_UserManagementViewModel)
        {
            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement
            {
                Description = tbl_UserManagementViewModel.Description,
                Email = tbl_UserManagementViewModel.Email,
                Password = tbl_UserManagementViewModel.Password,
                UserName = tbl_UserManagementViewModel.UserName
            };
            var usermanagement = _context.Tbl_UserManagement.Add(tbl_UserManagement);
            _context.SaveChanges();
            if (usermanagement.UserId > 0)
            {
                Random rnd = new Random();
                int rndnumber = rnd.Next(1, 999999);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                PasswordHasher PasswordHash = new PasswordHasher();
                //if (UserManager.FindByEmail(tbl_UserManagementViewModel.Email) == null)
                //{
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = usermanagement.UserName,
                    Email = usermanagement.Email,
                    PasswordHash = PasswordHash.HashPassword(usermanagement.Password),
                    UserId = usermanagement.UserId,
                    PhoneNumber = null,
                    IsEnable = true
                };

                IdentityResult result = UserManager.Create(admin);
                if (result.Succeeded == true)
                {
                    var data = UserManager.FindByName(usermanagement.UserName); // Name based user role assignment
                    if (data != null)
                    {
                        string id = data.Id;
                        UserManager.AddToRole(id, tbl_UserManagementViewModel.UserRole);
                    }
                }

                // }
            }
            return usermanagement.UserId.ToString();
        }


        public ActionResult AdmissionPortal()
        {
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            return View();
        }

        public ActionResult AddAdmissionDetails(StudentsRegistration studentsRegistrations)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.StudentsRegistrations.FirstOrDefault(x => x.Parents_Email == studentsRegistrations.Parents_Email && x.Name == studentsRegistrations.Name && x.DOB == studentsRegistrations.DOB);
                if (data != null)
                {
                    ViewBag.Studentdetails = data;
                    return RedirectToAction("Portal", new { id = data.StudentRegisterID });
                    //return Content("<script language='javascript' type='text/javascript'>alert('Data Already Entered');location.replace('" + url + "')</script>");

                }
                else
                {
                    var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "SUBMITTED").DataListItemId;
                    var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                    studentsRegistrations.ApplicationNumber = trackId.ToString();
                    studentsRegistrations.UIN = Guid.NewGuid().ToString();
                    studentsRegistrations.Registration_Date = DateTime.Now.ToString();
                    string year = Convert.ToDateTime(studentsRegistrations.Registration_Date).Year.ToString();
                    studentsRegistrations.AddedYear = year;
                    studentsRegistrations.IsApprove = Isapprove;
                    _StudentsRegistration.Insert(studentsRegistrations);
                    _StudentsRegistration.Save();

                    return RedirectToAction("Portal", new { id = studentsRegistrations.StudentRegisterID });
                    //return Content("<script language='javascript' type='text/javascript'>alert('Data Added Successfully');location.replace('" + url + "')</script>");

                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }


        }

        public ActionResult Portal()
        {
            //var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == id);
            //ViewBag.Studentdetails = data;
            //ViewBag.Email = data.Parents_Email;
            //ViewBag.Dob = data.DOB;
            //ViewBag.Name = data.Name+" "+data.Last_Name;


            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //ViewBag.StudentNo = _context.Students.Count() + 101;

            Tbl_Batches activeBatch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();
            if (activeBatch != null)
            {
                ViewBag.ActiveBatch = activeBatch.Batch_Name;
                ViewBag.IsActiveForPayments = activeBatch.IsActiveForPayments;
            }
            else
                ViewBag.ActiveBatch = string.Empty;

            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");

            //ViewBag.ClassData = _context.DataListItems.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddStudentDetails(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel, string[] Class_id, string[] Student_name, string[] Confirmation)
        {
            try
            {

                var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "SUBMITTED").DataListItemId;

                if (!string.IsNullOrEmpty(studentViewModel.StudentRegistration.Name))
                {
                    var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                    studentViewModel.StudentRegistration.ApplicationNumber = trackId.ToString();
                    studentViewModel.StudentRegistration.UIN = Guid.NewGuid().ToString();
                    studentViewModel.StudentRegistration.Registration_Date = DateTime.Now.ToString();
                    string year = Convert.ToDateTime(studentViewModel.StudentRegistration.Registration_Date).Year.ToString();
                    studentViewModel.StudentRegistration.AddedYear = year;
                    studentViewModel.StudentRegistration.IsApprove = Isapprove;


                    Tbl_Batches activeBatch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();
                    if (activeBatch != null)
                        studentViewModel.StudentRegistration.Batch_Id = activeBatch.Batch_Id;

                    //profile image
                    if (uploadFilesViewModel.ProfileAvatar != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), filename);
                            uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                            studentViewModel.StudentRegistration.ProfileAvatar = filename;
                        }
                    }

                    //aadhar image
                    if (uploadFilesViewModel.AdharFile != null)
                    {
                        if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), filename);
                            uploadFilesViewModel.AdharFile.SaveAs(path);
                            studentViewModel.StudentRegistration.AdharFile = filename;
                        }
                    }

                    _StudentsRegistration.Insert(studentViewModel.StudentRegistration);
                    _StudentsRegistration.Save();

                    //var data = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber && x.Name == studentViewModel.StudentRegistration.Name && x.Last_Name == studentViewModel.StudentRegistration.Last_Name && x.DOB == studentViewModel.StudentRegistration.DOB && x.Parents_Email == studentViewModel.StudentRegistration.Parents_Email && x.Class_Id == studentViewModel.StudentRegistration.Class_Id);
                    //Family Details
                    {
                        studentViewModel.FamilyDetail.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                        _context.SaveChanges();

                        //if(Confirmation.Length > 0)
                        {
                            foreach (var item in Confirmation)
                            {
                                if (item == "yes")//If the student is present in this school the value is yes 
                                {
                                    int i = 0;
                                    var familydata = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentViewModel.FamilyDetail.StudentRefId);
                                    familydata.Siblings = item;
                                    _context.SaveChanges();

                                    //Add siblings in seperate table
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                    i++;
                                }
                            }
                        }
                    }

                    //Guardian Details
                    {
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }


                    //Additinal Info
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }

                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;

                        _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }

                    //Past Schooling Record
                    {

                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                        }

                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }

                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }

                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                }

                Tbl_Batches curentBatch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();
                bool isRegistrationFeeActive = curentBatch.IsActiveForRegistrationFee;

                if (isRegistrationFeeActive)
                {
                    return RedirectToAction("RegistrationFeepage", "Payment", new { studentRegisterId = studentViewModel.StudentRegistration.StudentRegisterID });
                }

                return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully, " + studentViewModel.StudentRegistration.Name + " " + studentViewModel.StudentRegistration.Last_Name + " Your Application Number is " + studentViewModel.StudentRegistration.ApplicationNumber + " ');location.replace('/Account/Login')</script>");

                //return RedirectToAction("AdmissionPortal");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }


        [HttpPost]
        public ActionResult AddStudentAdmissionDetails(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel, string[] Class_id, string[] Student_name, string[] Confirmation)
        {
            try
            {
                var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "SUBMITTED").DataListItemId;

                if (!string.IsNullOrEmpty(studentViewModel.StudentRegistration.Name))
                {
                    var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                    studentViewModel.StudentRegistration.ApplicationNumber = trackId.ToString();
                    studentViewModel.StudentRegistration.UIN = Guid.NewGuid().ToString();
                    studentViewModel.StudentRegistration.Registration_Date = DateTime.Now.ToString();
                    string year = Convert.ToDateTime(studentViewModel.StudentRegistration.Registration_Date).Year.ToString();
                    studentViewModel.StudentRegistration.AddedYear = year;
                    studentViewModel.StudentRegistration.IsApprove = Isapprove;



                    //profile image
                    if (uploadFilesViewModel.ProfileAvatar != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), filename);
                            uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                            studentViewModel.StudentRegistration.ProfileAvatar = filename;
                        }
                    }

                    //aadhar image
                    if (uploadFilesViewModel.AdharFile != null)
                    {
                        if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                        {
                            var filename = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), filename);
                            uploadFilesViewModel.AdharFile.SaveAs(path);
                            studentViewModel.StudentRegistration.AdharFile = filename;
                        }
                    }

                    _StudentsRegistration.Insert(studentViewModel.StudentRegistration);
                    _StudentsRegistration.Save();

                    //var data = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber && x.Name == studentViewModel.StudentRegistration.Name && x.Last_Name == studentViewModel.StudentRegistration.Last_Name && x.DOB == studentViewModel.StudentRegistration.DOB && x.Parents_Email == studentViewModel.StudentRegistration.Parents_Email && x.Class_Id == studentViewModel.StudentRegistration.Class_Id);
                    //Family Details
                    {
                        studentViewModel.FamilyDetail.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                        _context.SaveChanges();

                        //if(Confirmation.Length > 0)
                        {
                            foreach (var item in Confirmation)
                            {
                                if (item == "yes")//If the student is present in this school the value is yes 
                                {
                                    int i = 0;
                                    //var familydata = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentViewModel.FamilyDetail.StudentRefId);
                                    var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.FamilyDetail.ApplicationNumber);
                                    familydata.Siblings = item;
                                    _context.SaveChanges();

                                    //Add siblings in seperate table
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                    i++;
                                }
                            }
                        }
                    }

                    //Guardian Details
                    {
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }


                    //Additinal Info
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }

                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;

                        _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }

                    //Past Schooling Record
                    {

                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                        }

                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }

                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }

                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully, " + studentViewModel.StudentRegistration.Name + " " + studentViewModel.StudentRegistration.Last_Name + " Your Application Number is " + studentViewModel.StudentRegistration.ApplicationNumber + " ');location.replace('/Student/AddStudent')</script>");

                //return RedirectToAction("AdmissionPortal");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }

        }

        public ActionResult StudentWriteup(int studentId)
        {
            var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
            ViewBag.studentname = data.Name + " " + data.Last_Name;
            ViewBag.Applicationnumber = data.ApplicationNumber;
            return View();
        }

        public JsonResult GetStudentRegistrationDetailsById(int id)
        {
            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var studentdetail = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == id);
            studentdetail.Class_Name = Class.FirstOrDefault(x => x.DataListItemId == studentdetail.Class_Id)?.DataListItemName;
            studentdetail.Section_Name = Section.FirstOrDefault(x => x.DataListItemId == studentdetail.Section_Id)?.DataListItemName;
            studentdetail.BatchName = Batches.FirstOrDefault(x => x.DataListItemId == studentdetail.Batch_Id)?.DataListItemName;
            studentdetail.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetail.BloodGroup_Id)?.DataListItemName;
            studentdetail.Religion = Religion.FirstOrDefault(x => x.DataListItemId == studentdetail.Religion_Id)?.DataListItemName;
            studentdetail.Caste = Caste.FirstOrDefault(x => x.DataListItemId == studentdetail.Cast_Id)?.DataListItemName;
            studentdetail.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;


            var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == id);
            var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.StudentRefId == id);
            var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.StudentRefId == id);

            var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.StudentRefId == id);

            return Json(new
            {
                studentdetail,
                familydetails,
                guardiandetails,
                additionalinfo,
                pastschoolingrecord
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult StudentAtendance()
        {
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            //ViewBag.sectionlist = _context.Tbl_SectionSetup.ToList();
            int classTeacherID = 3;
            var query = from s in _context.Subjects
                        join c in _context.DataListItems on s.Class_Id equals c.DataListItemId
                        where s.StaffId == classTeacherID
                        select c;

            var results = query.ToList();
            if (Session["RoleName"] != null)
            {
                string roleName = Session["RoleName"].ToString();
                // Use the roleName as needed
                if (roleName == "Staff")
                {
                    long staffId = Int64.Parse(Session["StaffID"].ToString());
                    var staff = _context.StafsDetails.Where(x => x.StafId == staffId).ToList();
                    ViewBag.Staff = staff;

                }
                else
                {
                    var staff = _context.StafsDetails.ToList();
                    ViewBag.Staff = staff;
                }
            }
            //ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.sectionlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission == true).*/.ToList();
            ViewBag.AllBatchs = new SelectList(batches, "Batch_Id", "Batch_Name");

            return View();
        }

        [HttpPost]
        public ActionResult SaveAttendanceData(List<Tbl_StudentAttendance> rowData, int classId, int sectionId, string attendanceDate)
        {

            //check any previous attendance by date
            //bool hasMatchingRecord = _context.Tbl_StudentAttendance.Any(c => Convert.ToDateTime(c.Created_Date).Date == Convert.ToDateTime(attendanceDate).Date && c.Class_Id == classId && c.Section_Id == sectionId);
            bool hasMatchingRecord = _context.Tbl_StudentAttendance.Any(c => c.Created_Date == attendanceDate
        && c.Class_Id == classId && c.Section_Id == sectionId);

            if (hasMatchingRecord)
            {
                return Json(new { success = false, errormsg = "Attendance is only taken once" });
            }
            // Save the data into the database table
            foreach (var item in rowData)
            {
                // Create a new entity object and set its properties
                var entity = new Tbl_StudentAttendance
                {
                    Student_Name = item.Student_Name,
                    BatchId = item.BatchId,
                    Class_Name = item.Class_Name,
                    Section_Name = item.Section_Name,
                    Mark_FullDayAbsent = item.Mark_FullDayAbsent,
                    Mark_HalfDayAbsent = item.Mark_HalfDayAbsent,
                    Class_Id = item.Class_Id,
                    Section_Id = item.Section_Id,
                    StudentRegisterID = item.StudentRegisterID,
                    Created_Date = item.Created_Date,
                    Day = item.Day,
                    Others = item.Others,
                    Created_By = item.Created_By
                };

                // Add the entity to the context
                _context.Tbl_StudentAttendance.Add(entity);
            }

            // Save changes to the database
            _context.SaveChanges();
            return Json(new { success = true }); // Return a success response if the data is saved successfully
        }

        public ActionResult GetStaffClass(int staffId)
        {
            // Retrieve class data based on the staff ID
            //var query = (from s in _context.Subjects
            //             join c in _context.DataListItems on s.Class_Id equals c.DataListItemId
            //             where s.StaffId == staffId && s.Class_Teacher == true
            //             select c).Distinct();
            var classlist = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true).ToList().Distinct().Reverse();
            //var list = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true).ToList();

            var query = (from s in classlist
                         join c in _context.DataListItems on s.Class_Id equals c.DataListItemId
                         where s.StaffId == staffId && s.Class_Teacher == true
                         select c).Distinct();



            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassSection(int staffId, int classId)
        {
            // Retrieve class data based on the staff ID
            //////var query = (from s in _context.Subjects
            //////             join c in _context.DataListItems on s.Section_Id equals c.DataListItemId
            //////             where s.StaffId == staffId && s.Class_Id == classId && s.Class_Teacher == true
            //////             select c).Distinct();

            var classlist = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Id == classId && x.Class_Teacher == true).ToList().Distinct().Reverse();


            var query = (from s in classlist
                         join c in _context.DataListItems on s.Section_Id equals c.DataListItemId
                         where s.StaffId == staffId && s.Class_Id == classId && s.Class_Teacher == true
                         select c).Distinct();


            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetClassSectionBatch(int staffId, int classId, int sectionId)
        {
            // Retrieve class data based on the staff ID
            var query = (from s in _context.Subjects
                         join c in _context.Tbl_Batches on s.Batch_Id equals c.Batch_Id
                         where s.StaffId == staffId && s.Class_Id == classId && s.Section_Id == sectionId && s.Class_Teacher == true
                         select c).Distinct();



            var results = query.ToList();


            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddStudentAttendance(Tbl_StudentAttendance tbl_StudentAttendance)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_StudentAttendance.Created_Date = DateTime.Now.ToString("dd/MM/yyyy");
            tbl_StudentAttendance.Created_By = Session["Name"].ToString();
            _context.Tbl_StudentAttendance.Add(tbl_StudentAttendance);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Attendance Added Successfully');location.replace('" + url + "')</script>");
        }


        public JsonResult StudentAttendanceById(int classid, int sectionid, string date, int batchid)
        {
            var classId = _context.DataListItems.Where(x => x.DataListItemId == classid).Select(x => x.DataListItemId).FirstOrDefault();
            var sectionId = _context.DataListItems.Where(x => x.DataListItemId == sectionid).Select(x => x.DataListItemId).FirstOrDefault();
            //var studentlist = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.Section_Id == sectionid).ToList();
            var studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.Batch_Id == batchid && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();

            var stdInfo = new List<Tbl_TestRecords>();
            var list = _context.Students;
            if (studentlist.Count == 0)
            {
                stdInfo = _context.Tbl_TestRecord.Where(x => x.BatchId == batchid && x.ClassID == classId && x.SectionID == sectionId && x.TermID == 1).ToList();
                var studentList = new List<Student>();
                foreach (var item in stdInfo)
                {
                    var student = list.Where(x => x.StudentId == item.StudentID).FirstOrDefault();
                    student.Section = _context.DataListItems.Where(x => x.DataListItemId == item.SectionID).Select(x => x.DataListItemName).FirstOrDefault();
                    student.Class = _context.DataListItems.Where(x => x.DataListItemId == item.ClassID).Select(x => x.DataListItemName).FirstOrDefault();

                    studentList.Add(student);

                }
                return Json(studentList, JsonRequestBehavior.AllowGet);

            }
            else
            {
                foreach (var item in studentlist)
                {
                    item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionid).Select(x => x.DataListItemName).FirstOrDefault();
                    item.Class = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault();
                }
                return Json(studentlist, JsonRequestBehavior.AllowGet);
            }


            //string html = "";
            //for (int i = 1; i < studentlist.Count(); i++;){
            //    html += "<tr>";

            //}


        }

        //view attendance
        public JsonResult StudentAttendanceView(int classId, int sectionId, string fromDate, string toDate, int studentId, int BatchId)
        {
            //var studentlist = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.Section_Id == sectionid).ToList();
            //var studentlist = _context.Tbl_StudentAttendance.Where(x => x.Class_Id == classId && x.Section_Id == sectionId).ToList();
            try
            {
                //parse prams string date into date
                DateTime from_Date = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime to_Date = DateTime.ParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                //var listOfStudents = new List<StudentsRegistration>();
                //var listOfAttendance = new List<Tbl_StudentAttendance>();

                //List of attendances without attendance date filter AND Apply Attendance date filter
                var listOfAttendance = _context.Tbl_StudentAttendance
      .Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.BatchId == BatchId && (studentId == 0 || x.StudentRegisterID == studentId))
      .ToList().Where(x => DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= from_Date && DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= to_Date).OrderBy(x => x.Student_Name).ToList();


                var listOfStudents = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.Batch_Id == BatchId && (studentId == 0 || x.StudentId == studentId)).ToList();


                var attendanceRecords = new List<AttendanceRecord>();

                foreach (var student in listOfStudents)
                {
                    var attendances = listOfAttendance.Where(x => x.StudentRegisterID == student.StudentId).ToList();
                    attendanceRecords.Add(new AttendanceRecord
                    {
                        StudentId = student.StudentId,
                        StudentName = student.Name,
                        Attendance = attendances,
                        AttendancePer = "Some value"
                    });
                }


                //        var attendanceRecords = validStudentList.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.BatchId == BatchId)
                //.GroupBy(r => r.StudentRegisterID)
                //.Select(g => new AttendanceRecord
                //{
                //    StudentId = g.Key,
                //    StudentName = g.First().Student_Name,
                //    Attendance = g.ToList(),
                //    AttendancePer = "Some value"
                //}).OrderBy(x => x.StudentName)
                //.ToList();
                // Calculate attendance percentage

                TimeSpan duration = to_Date - from_Date;
                double totalDays = duration.Days + 1;
                var filteredAttendanceRecords = attendanceRecords
    .Where(record =>
        !_context.Students.Any(student =>
            student.StudentId == record.StudentId && student.IsApplyforTC == true
        )
    )
    .ToList();

                //var attendanceRecords1 = studentlist.Where(x => x.Class_Id == classId && x.Section_Id == sectionId).ToList();
                foreach (var record in filteredAttendanceRecords)
                {

                    double attendedDays = 0;
                    double attendedHalfDays = 0;
                    foreach (var item in record.Attendance)
                    {
                        if (DateTime.ParseExact(item.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= from_Date && DateTime.ParseExact(item.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= to_Date)
                        {
                            if (item.Mark_FullDayAbsent == "True")
                            {
                                attendedDays++;
                            }
                            if (item.Mark_HalfDayAbsent == "True")
                            {
                                attendedHalfDays++;
                            }
                            if (item.Others == "True")
                            {
                                attendedDays++;
                            }
                        }
                    }
                    double totalAttendedDays = attendedDays + (attendedHalfDays / 2);
                    double attendancePercentage = (totalAttendedDays / totalDays) * 100;
                    record.AttendancePer = attendancePercentage.ToString("F2") + "%";
                }
                return Json(filteredAttendanceRecords.OrderBy(x => x.StudentName), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;

            }

            //attendanceRecords.ForEach(record => record.AttendancePer= attendancePercentage.ToString());

        }

        public ActionResult GetStudentForDrpdwn(int classId, int sectionId)
        {
            // Retrieve class data based on the staff ID
            var results = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).Select(x => new
            {
                x.Name,
                x.StudentId
            }).ToList();
            return Json(results.OrderBy(x => x.Name), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllAbsentStudent(int? classId, int? sectionId, string date)
        {
            List<Tbl_StudentAttendance> studentlist;

            if (classId.HasValue && sectionId.HasValue && !string.IsNullOrEmpty(date))
            {
                // Parse the date and apply filters
                DateTime Filtered_Date = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                studentlist = _context.Tbl_StudentAttendance
                    .Where(x => x.Class_Id == classId.Value
                                && x.Section_Id == sectionId.Value
                                && x.Mark_FullDayAbsent == "False"
                                && x.Mark_HalfDayAbsent == "False"
                                && x.Others == "False")
                    .ToList()
                    .Where(x => DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture) == Filtered_Date)
                    .ToList();
            }
            else
            {
                // Return the complete list of absent students
                studentlist = _context.Tbl_StudentAttendance
                    .Where(x => x.Mark_FullDayAbsent == "False"
                                && x.Mark_HalfDayAbsent == "False"
                                && x.Others == "False")
                    .ToList();
            }

            return Json(studentlist, JsonRequestBehavior.AllowGet);
        }

        public class AttendanceRecord
        {
            public int StudentId { get; set; }
            public string StudentName { get; set; }
            public List<Tbl_StudentAttendance> Attendance { get; set; }
            public string AttendancePer { get; set; }
        }
        public ActionResult ViewStudentAttendane()
        {
            //var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            //ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            //ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

            var batch = _context.Tbl_Batches.Distinct().ToList();
            ViewBag.Batch = batch;
            if (Session["RoleName"] != null)
            {
                string roleName = Session["RoleName"].ToString();
                // Use the roleName as needed
                if (roleName == "Staff")
                {
                    long staffId = Int64.Parse(Session["StaffID"].ToString());

                    //var Class = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true)
                    //    .Join(_context.DataListItems,s => s.Class_Id, d => d.DataListItemId,(s, d) => new { DataListItemId = d.DataListItemId, DataListItemName = d.DataListItemName }).Distinct().ToList();
                    //ViewBag.Class = Class;
                    //var Section= _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true)
                    //    .Join(_context.DataListItems, s => s.Section_Id, d => d.DataListItemId, (s, d) => new { DataListItemId = d.DataListItemId, DataListItemName = d.DataListItemName }).Distinct().ToList();
                    //ViewBag.Section = Section;

                    var classIds = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true).Select(x => x.Class_Id).Distinct().ToList();

                    var sectionIds = _context.Subjects.Where(x => x.StaffId == staffId && x.Class_Teacher == true).Select(x => x.Section_Id).Distinct().ToList();

                    var Class = _context.DataListItems.Where(d => classIds.Contains(d.DataListItemId)).ToList();

                    var Section = _context.DataListItems.Where(d => sectionIds.Contains(d.DataListItemId)).ToList();

                    ViewBag.Class = Class;
                    ViewBag.Section = Section;


                }
                else
                {
                    //var staff = _context.StafsDetails.ToList();
                    //ViewBag.Staff = staff;
                    var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                    var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                    ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                    ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

                }
            }
            return View();
        }


        #region Promotion History

        public ActionResult PromotionHistory()
        {
            ViewBag.PromotionHistory = _context.Tbl_StudentPromotes.ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            var data = _context.Tbl_StudentPromotes.ToList();
            List<Tbl_StudentPromote> tbl_StudentPromote = new List<Tbl_StudentPromote>();
            foreach (var item in data)
            {
                tbl_StudentPromote.Add(new Tbl_StudentPromote
                {
                    Student_Id = item.Student_Id,
                    FromClass_Id = item.FromClass_Id,
                    FromClass = Classes.FirstOrDefault(x => x.DataListItemId == item.FromClass_Id)?.DataListItemName,
                    ToClass_Id = item.ToClass_Id,
                    ToClass = Classes.FirstOrDefault(x => x.DataListItemId == item.ToClass_Id)?.DataListItemName,
                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    Registration_Date = item.Registration_Date,
                    AddedDate = item.AddedDate,
                    PromoteId = item.PromoteId
                });
            }
            ViewBag.PaststudentHistory = tbl_StudentPromote;

            //string origin = "Carmel Teresa School, Hagadur Main Road, Whitefield";
            //string destination = "Holy Ghost Church,Banglore";
            //string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&key=AIzaSyA8zy3reM0HthDBo8Wzl7kjLpI_lXipFj8";
            //WebRequest request = WebRequest.Create(url);
            //using (WebResponse response = (HttpWebResponse)request.GetResponse())
            //{
            //    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            //    {
            //        DataSet dsResult = new DataSet();
            //        dsResult.ReadXml(reader);
            //        Duration duration = new Duration();
            //        duration.Text = dsResult.Tables["duration"].Rows[0]["text"].ToString();
            //        Distance distance = new Distance();
            //        distance.Text = dsResult.Tables["distance"].Rows[0]["text"].ToString();

            //    }
            //}

            return View();
        }


        public JsonResult FilterPastStudent(int classid, string Fromdate, string Todate)
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            string[] Fd = Fromdate.Split('/');
            Fromdate = Fd[2] + "-" + Fd[1] + "-" + Fd[0];
            string[] Td = Todate.Split('/');
            Todate = Td[2] + "-" + Td[1] + "-" + Td[0];

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("Sp_FilterPastStudent", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@classid", classid);
            cmd.Parameters.AddWithValue("@Fromdate", Fromdate);
            cmd.Parameters.AddWithValue("@Todate", Todate);
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
            cmd.Dispose();
            //List<Tbl_StudentPromotes> lst = new List<Tbl_StudentPromotes>();
            List<Tbl_StudentPromotesData> lst = new List<Tbl_StudentPromotesData>();
            foreach (DataRow item in dt.Rows)
            {
                lst.Add(new Tbl_StudentPromotesData
                {
                    PromoteId = Convert.ToInt16(item["PromoteId"]),
                    ScholarNumber = Convert.ToString(item["ScholarNumber"]),
                    FromClass = Convert.ToString(item["FromClass"]),
                    ToClass = Convert.ToString(item["ToClass"]),
                    FromClass_Id = Convert.ToInt16(item["FromClass_Id"]),
                    ToClass_Id = Convert.ToInt16(item["ToClass_Id"]),
                    Student_Id = Convert.ToInt16(item["Student_Id"]),
                    Registration_Date = Convert.ToString(item["Registration_Date"]),
                    Firstname = Convert.ToString(item["Firstname"]),
                    Lastname = Convert.ToString(item["Lastname"])
                });
            }
            //var test = DateTime.Compare(Convert.ToDateTime("2022-04-01"), Fromdate);
            //var data = _context.Tbl_StudentPromotes.Where(x => x.FromClass_Id == classid && 
            //Convert.ToDateTime(x.Registration_Date) > Fromdate &&

            //Convert.ToDateTime(x.Registration_Date) < Convert.ToDateTime(Todate)).ToList();
            //var data = _context.Tbl_StudentPromotes.ToList();
            return Json(lst, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FilterStudent()
        {
            ViewBag.PromotionHistory = _context.Tbl_StudentPromotes.ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Sections = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            var data = _context.Tbl_StudentPromotes.ToList();
            List<Tbl_StudentPromote> tbl_StudentPromote = new List<Tbl_StudentPromote>();
            foreach (var item in data)
            {
                tbl_StudentPromote.Add(new Tbl_StudentPromote
                {
                    Student_Id = item.Student_Id,
                    FromClass_Id = item.FromClass_Id,
                    FromClass = Classes.FirstOrDefault(x => x.DataListItemId == item.FromClass_Id)?.DataListItemName,
                    ToClass_Id = item.ToClass_Id,
                    ToClass = Classes.FirstOrDefault(x => x.DataListItemId == item.ToClass_Id)?.DataListItemName,
                    Section_Id = item.Section_Id,
                    ToSection = Section.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,

                    Firstname = item.Firstname,
                    Lastname = item.Lastname,
                    Registration_Date = item.Registration_Date,
                    AddedDate = item.AddedDate,
                    PromoteId = item.PromoteId
                });
            }
            ViewBag.PaststudentHistory = tbl_StudentPromote;

            return Json(tbl_StudentPromote, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult UpdateAdmissionDetails(int id)
        {
            var data = _context.Students.FirstOrDefault(x => x.StudentId == id);
            ViewBag.Studentdetails = data;
            ViewBag.Email = data.ParentEmail;
            ViewBag.Dob = data.DOB;
            ViewBag.Name = data.Name;
            ViewBag.classlist = _context.Tbl_Class.ToList();
            ViewBag.sectionlist = _context.Tbl_SectionSetup.ToList();
            ViewBag.Batchlist = _context.Tbl_Batches.ToList();
            ViewBag.Bloodgrouplist = _context.Tbl_BloodGroup.ToList();
            ViewBag.Religionlist = _context.Tbl_Religion.ToList();
            ViewBag.CastList = _context.Tbl_Caste.ToList();
            ViewBag.CategoryList = _context.Tbl_Category.ToList();


            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //ViewBag.StudentNo = _context.Students.Count() + 101;

            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");

            return View();
        }

        public ActionResult UpdateStudentDetails(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel)
        {
            try
            {
                var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentViewModel.StudentRegistration.StudentRegisterID);
                var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "SUBMITTED").DataListItemId;
                if (!string.IsNullOrEmpty(studentViewModel.StudentRegistration.Name))
                {
                    if (data != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar != null)
                        {
                            if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                            {
                                var filename = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), filename);
                                uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                                studentViewModel.StudentRegistration.ProfileAvatar = filename;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.ProfileAvatar = data.ProfileAvatar;
                        }
                        if (uploadFilesViewModel.AdharFile != null)
                        {
                            if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.AdharFile.SaveAs(path);
                                studentViewModel.StudentRegistration.AdharFile = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.AdharFile = data.AdharFile;
                        }
                        studentViewModel.StudentRegistration.ApplicationNumber = data.ApplicationNumber;
                        studentViewModel.StudentRegistration.UIN = data.UIN;
                        studentViewModel.StudentRegistration.IsApprove = Isapprove;
                        //data.IsApprove = Isapprove;
                        _context.Entry(data).CurrentValues.SetValues(studentViewModel.StudentRegistration);
                        _context.SaveChanges();
                    }
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber);
                    if (familydetails != null)
                    {

                        studentViewModel.FamilyDetail.Id = familydetails.Id;
                        //familydetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.StudentRefId = familydetails.StudentRefId;
                        studentViewModel.FamilyDetail.ApplicationNumber = familydetails.ApplicationNumber;
                        _context.Entry(familydetails).CurrentValues.SetValues(studentViewModel.FamilyDetail);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.FamilyDetail.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                        _context.SaveChanges();
                    }

                    var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber);
                    if (guardiandetails != null)
                    {
                        studentViewModel.GuardianDetails.Id = guardiandetails.Id;
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.Entry(guardiandetails).CurrentValues.SetValues(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }

                    var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber);
                    if (additionalinfo != null)
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.BirthCertificateAvatar = additionalinfo.BirthCertificateAvatar;
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = additionalinfo.ThreePassportSizePhotographs;
                        }

                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ProgressReport = additionalinfo.ProgressReport;
                        }

                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.MigrationCertificate = additionalinfo.MigrationCertificate;
                        }

                        studentViewModel.AdditionalInformation.Id = additionalinfo.Id;
                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        //try
                        //{
                        //    string origin = "Carmel Teresa School, Hagadur Main Road, Whitefield";
                        //    string destination = studentViewModel.FamilyDetail.FResidentialAddress;
                        //    string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&key=AIzaSyA8zy3reM0HthDBo8Wzl7kjLpI_lXipFj8";
                        //    WebRequest request = WebRequest.Create(url);
                        //    using (WebResponse response = (HttpWebResponse)request.GetResponse())
                        //    {
                        //        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        //        {
                        //            DataSet dsResult = new DataSet();
                        //            dsResult.ReadXml(reader);
                        //            Duration duration = new Duration();
                        //            duration.Text = dsResult.Tables["duration"].Rows[0]["text"].ToString();
                        //            Distance distance = new Distance();
                        //            distance.Text = dsResult.Tables["distance"].Rows[0]["text"].ToString();
                        //            var km = distance.Text = dsResult.Tables["distance"].Rows[0]["text"].ToString();
                        //            var removekm = km.Remove(4);
                        //            studentViewModel.AdditionalInformation.DistancefromSchool = float.Parse(removekm);
                        //            //additionalinfo.DistancefromSchool =float.Parse( km.Replace("km","0"));


                        //        }
                        //        //additionalinfo.DistancefromSchool =
                        //    }

                        //} catch (Exception ex)
                        //{

                        //}

                        //var OriginAddress = "Whitefield";
                        //additionalinfo.DistancefromSchool = ;
                        //DirectionsRequest request = new DirectionsRequest();
                        //request.Key = "AIzaSyA8zy3reM0HthDBo8Wzl7kjLpI_lXipFj8";
                        //request.Origin = new Location(OriginAddress);
                        //request.Destination = new Location(studentViewModel.FamilyDetail.FResidentialAddress);
                        //var resonse = GoogleApi.GoogleMaps.Directions.Query(request);
                        //var Kilometer = resonse.Routes.First().Legs.First().Distance;
                        _context.Entry(additionalinfo).CurrentValues.SetValues(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }

                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        //string origin = "Carmel Teresa School, Hagadur Main Road, Whitefield";
                        //string destination = studentViewModel.FamilyDetail.FResidentialAddress;
                        //string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&key=AIzaSyA8zy3reM0HthDBo8Wzl7kjLpI_lXipFj8";
                        //WebRequest request = WebRequest.Create(url);
                        //using (WebResponse response = (HttpWebResponse)request.GetResponse())
                        //{
                        //    using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        //    {
                        //        DataSet dsResult = new DataSet();
                        //        dsResult.ReadXml(reader);
                        //        Duration duration = new Duration();
                        //        duration.Text = dsResult.Tables["duration"].Rows[0]["text"].ToString();
                        //        Distance distance = new Distance();
                        //        distance.Text = dsResult.Tables["distance"].Rows[0]["text"].ToString();
                        //        var km = distance.Text = dsResult.Tables["distance"].Rows[0]["text"].ToString();
                        //        var removekm = km.Remove(4);
                        //        studentViewModel.AdditionalInformation.DistancefromSchool = float.Parse(removekm);
                        //        //additionalinfo.DistancefromSchool =float.Parse( km.Replace("km","0"));


                        //    }
                        //    //additionalinfo.DistancefromSchool =
                        //}
                        _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }

                    var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.StudentRegistration.ApplicationNumber);
                    if (pastschoolingrecord != null)
                    {
                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.TCAvatar = pastschoolingrecord.TCAvatar;
                        }

                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.MarksCardAvatar = pastschoolingrecord.MarksCardAvatar;
                        }

                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = pastschoolingrecord.CharacterConductCertificateAvatar;
                        }

                        studentViewModel.PastSchoolingReport.Id = pastschoolingrecord.Id;
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.Entry(pastschoolingrecord).CurrentValues.SetValues(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.StudentRegistration.ApplicationNumber;
                        _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully, " + data.Name + " Your Application Number is " + data.ApplicationNumber + " ');location.replace('AdmissionPortal')</script>");
                return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('ManageStudent')</script>");

                //return RedirectToAction("AdmissionPortal");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }



        }

        public JsonResult UpdateStudentRegistrationDetailsById(int id)
        {
            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var studentdetail = _context.Students.FirstOrDefault(x => x.StudentId == id);
            studentdetail.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdetail.Class_Id)?.DataListItemName;
            studentdetail.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetail.Section_Id)?.DataListItemName;
            //studentdetail. = Section.FirstOrDefault(x => x.DataListItemId == studentdetail.Section_Id)?.DataListItemName;
            //studentdetail.BatchName = Batches.FirstOrDefault(x => x.DataListItemId == studentdetail.Batch_Id)?.DataListItemName;
            studentdetail.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetail.BloodGroup_Id)?.DataListItemName;
            //studentdetail.Religion = Religion.FirstOrDefault(x => x.DataListItemId == studentdetail)?.DataListItemName;
            //studentdetail.Caste = Caste.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;
            studentdetail.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;


            var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            //additionalinfo.Class_Name = Class.FirstOrDefault(x => x.DataListItemId == additionalinfo.Class_Id)?.DataListItemName;
            //additionalinfo.Section_Name = Section.FirstOrDefault(x => x.DataListItemId == additionalinfo.Section_Id)?.DataListItemName;
            var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);

            return Json(new
            {
                studentdetail,
                familydetails,
                guardiandetails,
                additionalinfo,
                pastschoolingrecord
            }, JsonRequestBehavior.AllowGet);
        }



        #region Rejected Students

        public ActionResult RejectedStudents()
        {

            var data = _context.StudentsRegistrations.Where(x => x.IsApprove == 192).ToList();
            ViewBag.RejectedStudents = data;

            return View();
        }


        #endregion

        #region Update StudentAdmission Process

        public ActionResult UdateStudentAdmissionDetails(int Id, int routingid)
        {
            var data = _context.Students.FirstOrDefault(x => x.StudentId == Id);
            ViewBag.StudentId = data;

            ViewBag.Username = Session["Name"].ToString();

            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            ViewBag.TransportVehicleNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "TransportVehicleNo").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
            //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //ViewBag.StudentNo = _context.Students.Count() + 101;

            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");

            var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == Id);
            studentdetails.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdetails.Class_Id)?.DataListItemName;
            studentdetails.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetails.BloodGroup_Id)?.DataListItemName;
            //studentdetails.Religion = Religion.FirstOrDefault(x => x.DataListItemId == studentdetail)?.DataListItemName;
            //studentdetail.Caste = Caste.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;
            studentdetails.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetails.Category_Id)?.DataListItemName;
            ViewBag.StudentDetails = studentdetails;

            //var TblStudent = _context.Students.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);

            var Familydetails = _context.FamilyDetails.Where(x => x.ApplicationNumber == data.ApplicationNumber);
            ViewBag.FamilyDetails = Familydetails;
            var Guardiandetails = _context.GuardianDetails.Where(x => x.ApplicationNumber == data.ApplicationNumber);
            ViewBag.GuardianDetails = Guardiandetails;
            var AdditionalInfo = _context.AdditionalInformations.Where(x => x.ApplicationNumber == data.ApplicationNumber);
            ViewBag.AdditionalInfo = AdditionalInfo;
            var PastSchoolingrec = _context.PastSchoolingReports.Where(x => x.ApplicationNumber == data.ApplicationNumber);
            ViewBag.PastSchoolingRecord = PastSchoolingrec;
            ViewBag.Routingid = routingid;

            var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission == true).*/.ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");

            return View();
        }

        public ActionResult UdateStudentDetails(int Id, int routingid)
        {
            var data = _context.Students.FirstOrDefault(x => x.StudentId == Id);
            ViewBag.StudentId = data;

            ViewBag.Username = Session["Name"].ToString();

            ViewBag.BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            ViewBag.Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            ViewBag.Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            ViewBag.Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            ViewBag.TransportVehicleNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "TransportVehicleNo").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
            //ViewBag.Category = new SelectList(_context.StudentCategorys.ToList().OrderBy(x => x.CategoryName).ToList(), "CategoryId", "CategoryName");
            //ViewBag.StudentNo = _context.Students.Count() + 101;

            ViewBag.Classes = new SelectList(_context.Classes.ToList().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");

            var studentdetails = _context.Students.FirstOrDefault(x => x.StudentId == Id);
            studentdetails.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdetails.Class_Id)?.DataListItemName;
            studentdetails.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetails.BloodGroup_Id)?.DataListItemName;
            //studentdetails.Religion = Religion.FirstOrDefault(x => x.DataListItemId == studentdetail)?.DataListItemName;
            //studentdetail.Caste = Caste.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;
            studentdetails.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetails.Category_Id)?.DataListItemName;
            ViewBag.StudentDetails = studentdetails;

            var TblStudent = _context.Students.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);

            var Familydetails = _context.FamilyDetails.Where(x => x.ApplicationNumber == TblStudent.ApplicationNumber);
            ViewBag.FamilyDetails = Familydetails;
            var Guardiandetails = _context.GuardianDetails.Where(x => x.ApplicationNumber == TblStudent.ApplicationNumber);
            ViewBag.GuardianDetails = Guardiandetails;
            var AdditionalInfo = _context.AdditionalInformations.Where(x => x.ApplicationNumber == TblStudent.ApplicationNumber);
            ViewBag.AdditionalInfo = AdditionalInfo;
            var PastSchoolingrec = _context.PastSchoolingReports.Where(x => x.ApplicationNumber == TblStudent.ApplicationNumber);
            ViewBag.PastSchoolingRecord = PastSchoolingrec;
            ViewBag.Routingid = routingid;

            var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission == true).*/.ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");

            return View();
        }
        public JsonResult UpdateStudentAdmissionDetailsById(int id)
        {
            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var studentdetail = _context.Students.FirstOrDefault(x => x.StudentId == id);
            // var TblStudent = _context.Students.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            studentdetail.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdetail.Class_Id)?.DataListItemName;
            studentdetail.Section = Section.FirstOrDefault(x => x.DataListItemId == studentdetail.Section_Id)?.DataListItemName;
            //studentdetail.BatchName = Batches.FirstOrDefault(x => x.DataListItemId == studentdetail.Batch_Id)?.DataListItemName;
            studentdetail.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetail.BloodGroup_Id)?.DataListItemName;
            //studentdetail.Religion = Religion.FirstOrDefault(x => x.DataListItemId == studentdetail)?.DataListItemName;
            //studentdetail.Caste = Caste.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;
            studentdetail.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;
            //var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission  == true ||false )*/.ToList();
            //ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");

            var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == id);
            var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.StudentRefId == id);
            var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.StudentRefId == id);
            //additionalinfo.Class_Name = Class.FirstOrDefault(x => x.DataListItemId == additionalinfo.Class_Id)?.DataListItemName;
            //additionalinfo.Section_Name = Section.FirstOrDefault(x => x.DataListItemId == additionalinfo.Section_Id)?.DataListItemName;
            var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.StudentRefId == id);

            List<Tbl_Siblings> Tbl_Siblings = new List<Tbl_Siblings>();
            List<SiblingsViewModel> SiblingsViewModel = new List<SiblingsViewModel>();
            var html = "";

            if (familydetails != null)
            {
                Tbl_Siblings = _context.Tbl_Siblings.Where(x => x.FamilyDetails_Id == familydetails.Id).ToList();

                foreach (var item in Tbl_Siblings)
                {
                    int i = 0;

                    var classname = Class.FirstOrDefault(x => x.DataListItemId == item.Class_id)?.DataListItemName;

                    html += "<div class='row' id='divSibling_" + i + "'>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Sibling:</label>";
                    html += "<input type='text' class='form-control' placeholder='StudentName' name='Student_name' value='" + item.Studentname + "'/>";
                    html += "</div>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Select:</label>";
                    html += "<select class='form-control' name='Confirmation'>";
                    if (item.Confirmation == "yes")
                    {
                        html += "<option value='" + item.Confirmation + "'>Yes</option>";
                    }
                    else if (item.Confirmation == "no")
                    {
                        html += "<option value='" + item.Confirmation + "'>No</option>";
                    }
                    html += "<option value='0'>--select--</option>";
                    html += "<option value='yes'>Yes</option>";
                    html += "<option value='no'>No</option>";
                    html += "</select>";
                    html += "</div>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Class:</label>";
                    html += "<select class='form-control' name='Class_id'>";
                    html += "<option value='" + item.Class_id + "'>" + classname + "</option>";
                    html += "<option value='0'>--Select--</option>";
                    foreach (var data in Class)
                    {
                        html += "<option value='" + data.DataListItemId + "'>" + data.DataListItemName + "</option>";
                    }
                    html += "</select>";
                    html += "</div>";
                    html += "</div>";

                }

            }
            var asdf = html;
            return Json(new
            {
                studentdetail,
                familydetails,
                guardiandetails,
                additionalinfo,
                pastschoolingrecord,
                html
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateStudentDetailsById(int id)
        {
            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            var Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();
            var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
            var Group = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "group").DataListId.ToString()).ToList();
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var studentdetail = _context.Students.FirstOrDefault(x => x.StudentId == id);
            var TblStudent = _context.Students.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            studentdetail.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdetail.Class_Id)?.DataListItemName;
            studentdetail.Section = Section.FirstOrDefault(x => x.DataListItemId == studentdetail.Section_Id)?.DataListItemName;
            studentdetail.BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == studentdetail.BloodGroup_Id)?.DataListItemName;
            studentdetail.Category = Category.FirstOrDefault(x => x.DataListItemId == studentdetail.Category_Id)?.DataListItemName;

            var studentReg = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentdetail.ApplicationNumber).FirstOrDefault();
            var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);
            var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == studentdetail.ApplicationNumber);

            List<Tbl_Siblings> Tbl_Siblings = new List<Tbl_Siblings>();
            List<SiblingsViewModel> SiblingsViewModel = new List<SiblingsViewModel>();
            var html = "";

            if (familydetails != null)
            {
                Tbl_Siblings = _context.Tbl_Siblings.Where(x => x.FamilyDetails_Id == familydetails.Id).ToList();

                foreach (var item in Tbl_Siblings)
                {
                    int i = 0;

                    var classname = Class.FirstOrDefault(x => x.DataListItemId == item.Class_id)?.DataListItemName;

                    html += "<div class='row' id='divSibling_" + i + "'>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Sibling:</label>";
                    html += "<input type='text' class='form-control' placeholder='StudentName' name='Student_name' value='" + item.Studentname + "'/>";
                    html += "</div>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Select:</label>";
                    html += "<select class='form-control' name='Confirmation'>";
                    if (item.Confirmation == "yes")
                    {
                        html += "<option value='" + item.Confirmation + "'>Yes</option>";
                    }
                    else if (item.Confirmation == "no")
                    {
                        html += "<option value='" + item.Confirmation + "'>No</option>";
                    }
                    html += "<option value='0'>--select--</option>";
                    html += "<option value='yes'>Yes</option>";
                    html += "<option value='no'>No</option>";
                    html += "</select>";
                    html += "</div>";
                    html += "<div class='col-sm-4'>";
                    html += "<label>Class:</label>";
                    html += "<select class='form-control' name='Class_id'>";
                    html += "<option value='" + item.Class_id + "'>" + classname + "</option>";
                    html += "<option value='0'>--Select--</option>";
                    foreach (var data in Class)
                    {
                        html += "<option value='" + data.DataListItemId + "'>" + data.DataListItemName + "</option>";
                    }
                    html += "</select>";
                    html += "</div>";
                    html += "</div>";

                }

            }

            var asdf = html;



            return Json(new
            {
                studentdetail,
                studentReg,
                familydetails,
                guardiandetails,
                additionalinfo,
                pastschoolingrecord,
                html
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UpdateStudentsDetails(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel, string[] Class_id, string[] Student_name, string[] Confirmation)
        {

            try
            {
                var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentViewModel.StudentRegistration.StudentRegisterID);
                //var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "APPROVED").DataListItemId;
                if (!string.IsNullOrEmpty(studentViewModel.StudentRegistration.Name))
                {
                    if (data != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar != null)
                        {
                            if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                            {
                                var filename = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto/"), filename);
                                uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                                studentViewModel.StudentRegistration.ProfileAvatar = filename;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.ProfileAvatar = data.ProfileAvatar;
                        }
                        if (uploadFilesViewModel.AdharFile != null)
                        {
                            if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.AdharFile.SaveAs(path);
                                studentViewModel.StudentRegistration.AdharFile = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.AdharFile = data.AdharFile;
                        }
                        studentViewModel.StudentRegistration.ApplicationNumber = data.ApplicationNumber;
                        studentViewModel.StudentRegistration.UIN = data.UIN;
                        studentViewModel.StudentRegistration.IsApprove = data.IsApprove;
                        studentViewModel.StudentRegistration.UserId = data.UserId;
                        studentViewModel.StudentRegistration.AddedYear = data.AddedYear;
                        studentViewModel.StudentRegistration.Registration_Date = data.Registration_Date;
                        studentViewModel.StudentRegistration.IsEmailsent = data.IsEmailsent;
                        //data.IsApprove = Isapprove;
                        _context.Entry(data).CurrentValues.SetValues(studentViewModel.Student);
                        _context.SaveChanges();

                    }
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (familydetails != null)
                    {

                        studentViewModel.FamilyDetail.Id = familydetails.Id;
                        //familydetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.StudentRefId = familydetails.StudentRefId;
                        studentViewModel.FamilyDetail.ApplicationNumber = familydetails.ApplicationNumber;
                        _context.Entry(familydetails).CurrentValues.SetValues(studentViewModel.FamilyDetail);
                        _context.SaveChanges();


                        int i = 0;
                        foreach (var item in Confirmation)
                        {
                            if (item == "yes")//If the student is present in this school the value is yes 
                            {
                                var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                                familydata.Siblings = item;
                                _context.SaveChanges();

                                int classid = Convert.ToInt32(Class_id[i]);
                                string studentnames = Student_name[i];

                                var siblingdata = _context.Tbl_Siblings.FirstOrDefault(x => x.FamilyDetails_Id == familydata.Id && x.Class_id == classid && x.Studentname == studentnames);

                                if (siblingdata != null)
                                {
                                    siblingdata.Class_id = Convert.ToInt32(Class_id[i]);
                                    siblingdata.Studentname = Student_name[i];
                                    siblingdata.Confirmation = item;
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                }

                                //Add siblings in seperate table

                            }
                            i++;
                        }

                    }
                    else
                    {
                        studentViewModel.FamilyDetail.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.FamilyDetail.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                        _context.SaveChanges();

                        {
                            foreach (var item in Confirmation)
                            {
                                if (item == "yes")//If the student is present in this school the value is yes 
                                {
                                    int i = 0;
                                    var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                                    familydata.Siblings = item;
                                    _context.SaveChanges();

                                    //Add siblings in seperate table
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                    i++;
                                }
                            }
                        }

                    }


                    var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (guardiandetails != null)
                    {
                        studentViewModel.GuardianDetails.Id = guardiandetails.Id;
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.Entry(guardiandetails).CurrentValues.SetValues(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.GuardianDetails.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }


                    var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);

                    if (additionalinfo != null)
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.BirthCertificateAvatar = additionalinfo.BirthCertificateAvatar;
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = additionalinfo.ThreePassportSizePhotographs;
                        }

                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ProgressReport = additionalinfo.ProgressReport;
                        }

                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.MigrationCertificate = additionalinfo.MigrationCertificate;
                        }

                        studentViewModel.AdditionalInformation.Id = additionalinfo.Id;
                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);

                        _context.Entry(additionalinfo).CurrentValues.SetValues(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.AdditionalInformation.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }

                    var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.Student.ApplicationNumber);
                    if (pastschoolingrecord != null)
                    {
                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.TCAvatar = pastschoolingrecord.TCAvatar;
                        }

                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.MarksCardAvatar = pastschoolingrecord.MarksCardAvatar;
                        }

                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = pastschoolingrecord.CharacterConductCertificateAvatar;
                        }

                        studentViewModel.PastSchoolingReport.Id = pastschoolingrecord.Id;
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.Entry(pastschoolingrecord).CurrentValues.SetValues(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(studentViewModel.Student.StudentId);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = studentViewModel.Student.ApplicationNumber;
                        _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully, " + data.Name + " Your Application Number is " + data.ApplicationNumber + " ');location.replace('AdmissionPortal')</script>");
                if (studentViewModel.RoutingId == 1)
                {

                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/Student/StudentReport')</script>");
                }
                else if (studentViewModel.RoutingId == 2)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/Student/ManageStudent')</script>");
                }

                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/StudentAdmission/StudentAdmission')</script>");
                }

                //return RedirectToAction("AdmissionPortal");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        public ActionResult UpdateStudentsDetailsInStudent(StudentViewModel studentViewModel, UploadFilesViewModel uploadFilesViewModel, string[] Class_id, string[] Student_name, string[] Confirmation)
        {

            try
            {
                var data = _context.Students.FirstOrDefault(x => x.StudentId == studentViewModel.StudentRegistration.StudentRegisterID);
                var StudentReg = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == data.ApplicationNumber).FirstOrDefault();
                //var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "APPROVED").DataListItemId;
                if (!string.IsNullOrEmpty(studentViewModel.StudentRegistration.Name))
                {
                    if (data != null)
                    {
                        if (uploadFilesViewModel.ProfileAvatar != null)
                        {
                            if (uploadFilesViewModel.ProfileAvatar.ContentLength > 0)
                            {
                                var filename = Path.GetFileName(uploadFilesViewModel.ProfileAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto/"), filename);
                                uploadFilesViewModel.ProfileAvatar.SaveAs(path);
                                studentViewModel.StudentRegistration.ProfileAvatar = filename;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.ProfileAvatar = data.ProfileAvatar;
                        }
                        if (uploadFilesViewModel.AdharFile != null)
                        {
                            if (uploadFilesViewModel.AdharFile.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.AdharFile.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.AdharFile.SaveAs(path);
                                studentViewModel.StudentRegistration.AdharFile = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.StudentRegistration.AdharFile = data.AdharFile;
                        }
                        studentViewModel.StudentRegistration.ApplicationNumber = data.ApplicationNumber;
                        studentViewModel.StudentRegistration.UIN = data.UIN;
                        studentViewModel.StudentRegistration.IsApprove = data.IsApprove;
                        studentViewModel.StudentRegistration.UserId = data.UserId;
                        studentViewModel.StudentRegistration.IsApplyforTC = data.IsApplyforTC;
                        //studentViewModel.StudentRegistration.AddedYear = data.AddedYear;
                        //studentViewModel.StudentRegistration.Registration_Date = data.Registration_Date;
                        //studentViewModel.StudentRegistration.IsEmailsent = data.IsEmailsent;

                        //data.IsApprove = Isapprove;
                        _context.Entry(data).CurrentValues.SetValues(studentViewModel.StudentRegistration);

                        data.ParentEmail = studentViewModel.StudentRegistration.Parents_Email;

                        //update details in student_reg
                        StudentReg.BankAccount = studentViewModel.StudentRegistration.BankAccount;
                        StudentReg.BankACHolder = studentViewModel.StudentRegistration.BankACHolder;
                        StudentReg.BankIFSC = studentViewModel.StudentRegistration.BankIFSC;
                        StudentReg.BankName = studentViewModel.StudentRegistration.BankName;
                        StudentReg.SSSMIdNumber = studentViewModel.StudentRegistration.SSSMIdNumber;
                        StudentReg.PerEduNumber = studentViewModel.StudentRegistration.PerEduNumber;
                        StudentReg.FamilySSSMID = studentViewModel.StudentRegistration.FamilySSSMID;
                        StudentReg.Registration_Date = studentViewModel.StudentRegistration.Registration_Date;
                        StudentReg.Religion_Id = studentViewModel.StudentRegistration.Religion != null ? Int32.Parse(studentViewModel.StudentRegistration.Religion) : 176;
                        //StudentReg.PerEduNumber = studentViewModel.StudentRegistration.PerEduNumber;
                        StudentReg.ApaarId = studentViewModel.StudentRegistration.ApaarId;
                        _context.SaveChanges();

                    }
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                    if (familydetails != null)
                    {

                        studentViewModel.FamilyDetail.Id = familydetails.Id;
                        //familydetails.StudentRefId = Convert.ToInt32(studentViewModel.StudentRegistration.StudentRegisterID);
                        studentViewModel.FamilyDetail.StudentRefId = familydetails.StudentRefId;
                        studentViewModel.FamilyDetail.ApplicationNumber = familydetails.ApplicationNumber;
                        _context.Entry(familydetails).CurrentValues.SetValues(studentViewModel.FamilyDetail);
                        _context.SaveChanges();


                        int i = 0;
                        foreach (var item in Confirmation)
                        {
                            if (item == "yes")//If the student is present in this school the value is yes 
                            {
                                var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.FamilyDetail.ApplicationNumber);
                                familydata.Siblings = item;
                                _context.SaveChanges();

                                int classid = Convert.ToInt32(Class_id[i]);
                                string studentnames = Student_name[i];

                                var siblingdata = _context.Tbl_Siblings.FirstOrDefault(x => x.FamilyDetails_Id == familydata.Id && x.Class_id == classid && x.Studentname == studentnames);

                                if (siblingdata != null)
                                {
                                    siblingdata.Class_id = Convert.ToInt32(Class_id[i]);
                                    siblingdata.Studentname = Student_name[i];
                                    siblingdata.Confirmation = item;
                                    _context.SaveChanges();
                                }
                                else
                                {
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                }

                                //Add siblings in seperate table

                            }
                            i++;
                        }

                    }
                    else
                    {
                        studentViewModel.FamilyDetail.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.FamilyDetail.ApplicationNumber = data.ApplicationNumber;
                        _context.FamilyDetails.Add(studentViewModel.FamilyDetail);
                        _context.SaveChanges();

                        {
                            foreach (var item in Confirmation)
                            {
                                if (item == "yes")//If the student is present in this school the value is yes 
                                {
                                    int i = 0;
                                    var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentViewModel.FamilyDetail.ApplicationNumber);
                                    familydata.Siblings = item;
                                    _context.SaveChanges();

                                    //Add siblings in seperate table
                                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                                    tbl_Siblings.Class_id = Convert.ToInt32(Class_id[i]);
                                    tbl_Siblings.Studentname = Student_name[i];
                                    tbl_Siblings.Student_Id = studentViewModel.FamilyDetail.StudentRefId;
                                    tbl_Siblings.Confirmation = item;
                                    tbl_Siblings.FamilyDetails_Id = familydata.Id;

                                    _context.Tbl_Siblings.Add(tbl_Siblings);
                                    _context.SaveChanges();
                                    i++;
                                }
                            }
                        }

                    }


                    var guardiandetails = _context.GuardianDetails.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                    if (guardiandetails != null)
                    {
                        studentViewModel.GuardianDetails.Id = guardiandetails.Id;
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.GuardianDetails.ApplicationNumber = data.ApplicationNumber;
                        _context.Entry(guardiandetails).CurrentValues.SetValues(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.GuardianDetails.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.GuardianDetails.ApplicationNumber = data.ApplicationNumber;
                        _context.GuardianDetails.Add(studentViewModel.GuardianDetails);
                        _context.SaveChanges();
                    }


                    var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                    if (additionalinfo != null)
                    {
                        if (uploadFilesViewModel.BirthCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.BirthCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.BirthCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.BirthCertificateAvatar.SaveAs(path);
                                studentViewModel.AdditionalInformation.BirthCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.BirthCertificateAvatar = additionalinfo.BirthCertificateAvatar;
                        }
                        if (uploadFilesViewModel.ThreePassportSizePhotographs != null)
                        {
                            if (uploadFilesViewModel.ThreePassportSizePhotographs.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ThreePassportSizePhotographs.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ThreePassportSizePhotographs.SaveAs(path);
                                studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ThreePassportSizePhotographs = additionalinfo.ThreePassportSizePhotographs;
                        }

                        if (uploadFilesViewModel.ProgressReport != null)
                        {
                            if (uploadFilesViewModel.ProgressReport.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.ProgressReport.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.ProgressReport.SaveAs(path);
                                studentViewModel.AdditionalInformation.ProgressReport = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.ProgressReport = additionalinfo.ProgressReport;
                        }

                        if (uploadFilesViewModel.MigrationCertificate != null)
                        {
                            if (uploadFilesViewModel.MigrationCertificate.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MigrationCertificate.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MigrationCertificate.SaveAs(path);
                                studentViewModel.AdditionalInformation.MigrationCertificate = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.AdditionalInformation.MigrationCertificate = additionalinfo.MigrationCertificate;
                        }

                        studentViewModel.AdditionalInformation.Id = additionalinfo.Id;
                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.AdditionalInformation.ApplicationNumber = data.ApplicationNumber;
                        _context.Entry(additionalinfo).CurrentValues.SetValues(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.AdditionalInformation.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.AdditionalInformation.ApplicationNumber = data.ApplicationNumber;
                        _context.AdditionalInformations.Add(studentViewModel.AdditionalInformation);
                        _context.SaveChanges();
                    }

                    var pastschoolingrecord = _context.PastSchoolingReports.FirstOrDefault(x => x.ApplicationNumber == data.ApplicationNumber);
                    if (pastschoolingrecord != null)
                    {
                        if (uploadFilesViewModel.TCAvatar != null)
                        {
                            if (uploadFilesViewModel.TCAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.TCAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.TCAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.TCAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.TCAvatar = pastschoolingrecord.TCAvatar;
                        }

                        if (uploadFilesViewModel.MarksCardAvatar != null)
                        {
                            if (uploadFilesViewModel.MarksCardAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.MarksCardAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.MarksCardAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.MarksCardAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.MarksCardAvatar = pastschoolingrecord.MarksCardAvatar;
                        }

                        if (uploadFilesViewModel.CharacterConductCertificateAvatar != null)
                        {
                            if (uploadFilesViewModel.CharacterConductCertificateAvatar.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(uploadFilesViewModel.CharacterConductCertificateAvatar.FileName);
                                var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentAdhar"), fileName);
                                uploadFilesViewModel.CharacterConductCertificateAvatar.SaveAs(path);
                                studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = fileName;
                            }
                        }
                        else
                        {
                            studentViewModel.PastSchoolingReport.CharacterConductCertificateAvatar = pastschoolingrecord.CharacterConductCertificateAvatar;
                        }

                        studentViewModel.PastSchoolingReport.Id = pastschoolingrecord.Id;
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = data.ApplicationNumber;
                        _context.Entry(pastschoolingrecord).CurrentValues.SetValues(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                    else
                    {
                        studentViewModel.PastSchoolingReport.StudentRefId = Convert.ToInt32(data.StudentId);
                        studentViewModel.PastSchoolingReport.ApplicationNumber = data.ApplicationNumber;
                        _context.PastSchoolingReports.Add(studentViewModel.PastSchoolingReport);
                        _context.SaveChanges();
                    }
                }
                //return Content("<script language='javascript' type='text/javascript'>alert('Details Added Successfully, " + data.Name + " Your Application Number is " + data.ApplicationNumber + " ');location.replace('AdmissionPortal')</script>");
                if (studentViewModel.RoutingId == 1)
                {

                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/Student/StudentReport')</script>");
                }
                else if (studentViewModel.RoutingId == 2)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/Student/ManageStudent')</script>");

                }
                else if (studentViewModel.RoutingId == 3)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/AdmissionFee/ApplyTc')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace('/StudentAdmission/StudentAdmission')</script>");
                }

                //return RedirectToAction("AdmissionPortal");
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           validationErrors.Entry.Entity.ToString(),
                           validationError.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }
        //Siblings Manage
        public ActionResult ManageSiblings()
        {

            List<SiblingsViewModel> siblingsViewModels = new List<SiblingsViewModel>();
            var sibingsdata = _context.Tbl_Siblings.ToList();
            var class_ = _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString();

            var Class = _context.DataListItems.Where(e => e.DataListId == class_).ToList();
            var studentdata = _context.Students.Where(x => x.IsApprove != 192).ToList();

            ViewBag.Classes = Class;

            ViewBag.StudentDatalist = studentdata;

            foreach (var item in sibingsdata)
            {

                var sdata = studentdata.FirstOrDefault(x => x.StudentId == item.Student_Id);
                var familyData = _context.FamilyDetails.Where(w => w.Id == item.FamilyDetails_Id).FirstOrDefault();
                var classData = _context.Classes.Where(w => w.Id == item.Class_id).FirstOrDefault();

                SiblingsViewModel siblingsViewModel = new SiblingsViewModel();
                siblingsViewModel.Siblings_Id = item.Siblings_Id ?? 0;
                siblingsViewModel.Studentname = item.Studentname;
                siblingsViewModel.Fathername = familyData.FatherName;
                siblingsViewModel.Mothername = familyData.MotherName;
                siblingsViewModel.ContactNumber = familyData.FMobile;
                siblingsViewModel.Address = familyData.FResidentialAddress;

                siblingsViewModel.Confirmation = item.Confirmation;
                siblingsViewModel.Classname = Class.FirstOrDefault(x => x.DataListItemId == item.Class_id)?.DataListItemName;
                siblingsViewModel.siblingsstudentname = sdata == null ? "" : sdata.Name + " " + sdata.Last_Name;
                siblingsViewModel.Studentclass = sdata == null ? "" : Class.FirstOrDefault(x => x.DataListItemId == sdata.Class_Id)?.DataListItemName;

                siblingsViewModels.Add(siblingsViewModel);
            }

            var studentSiblingQuery = (from sib in _context.Tbl_Siblings
                                       join FamilyDetail in _context.FamilyDetails on sib.FamilyDetails_Id equals FamilyDetail.Id
                                       join std in _context.Students on sib.Student_Id equals std.StudentId into std_join
                                       from stdhd in std_join.DefaultIfEmpty()
                                       join cls in _context.Classes on sib.Class_id equals cls.Id into sh_join
                                       from clshd in sh_join.DefaultIfEmpty()
                                       where stdhd.Class_Id != null && FamilyDetail.MPermanentAddress != null
                                       select new
                                       {
                                           Student_Id = sib.Student_Id,
                                           FamilyId = FamilyDetail.Id,
                                           studentname = stdhd.Name + " " + stdhd.Last_Name,
                                           studentClassID = stdhd.Class_Id,
                                           Fathername = FamilyDetail.FatherName,
                                           Mothername = FamilyDetail.MotherName,
                                           ContactNumber = FamilyDetail.FMobile,
                                           Address = FamilyDetail.MPermanentAddress,
                                           Confirmation = sib.Confirmation,  // Ensure this is nullable if it can be null
                                           Siblings_Id = sib.Siblings_Id ?? 0,
                                           SiblingStudentname = sib.Studentname + " " + (clshd.ClassName ?? ""),  // Null-coalescing operator
                                           SibClassID = sib.Class_id ?? 0
                                       }).ToList();






            var groupStudentBySiblings = studentSiblingQuery.GroupBy(w => w.FamilyId).Select(s => new SiblingsVM
            {

                Fathername = s.FirstOrDefault().Fathername,
                Mothername = s.FirstOrDefault().Mothername,
                ContactNumber = s.FirstOrDefault().ContactNumber,
                Address = s.FirstOrDefault().Address,
                StudentclassID = s.FirstOrDefault().studentClassID,
                StudentClass = "",
                StudentName = s.FirstOrDefault().studentname,

                SiblingsData = s.Select(q => new SiblingDataVM
                {

                    Siblings_Id = q.Siblings_Id,
                    Confirmation = q.Confirmation,
                    SibClassID = q.SibClassID,
                    siblingsStudentclass = "",
                    SiblingStudentName = q.SiblingStudentname,
                    Student_Id = q.Student_Id ?? 0,
                    SiblingStudentClass = "",
                    //siblingsstudentname = q.siblingsstudentname,

                }).ToList()


            }).ToList();


            //foreach (var i in groupStudentBySiblings)
            //{
            //    i.StudentClass = Class.FirstOrDefault(x => x.DataListItemId == i.StudentclassID)?.DataListItemName;
            //    foreach (var j in i.SiblingsData)
            //    {
            //        j.siblingsStudentclass = Class.FirstOrDefault(x => x.DataListItemId == j.SibClassID)?.DataListItemName;

            //    }
            //}

            foreach (var i in groupStudentBySiblings)
            {
                // Get StudentClass from Class list
                i.StudentClass = Class.FirstOrDefault(x => x.DataListItemId == i.StudentclassID)?.DataListItemName;

                // Add additional entries for StudentName and StudentClass in SiblingsData
                var studentEntry = new SiblingDataVM
                {
                    Siblings_Id = 0, // Or any default value if applicable
                    Confirmation = null, // Or any default value if applicable
                    SibClassID = i.StudentclassID,
                    siblingsStudentclass = i.StudentClass,
                    SiblingStudentName = i.StudentName,
                    Student_Id = 0, // Or any default value if applicable
                    SiblingStudentClass = i.StudentClass
                };

                // Add to SiblingsData list
                i.SiblingsData.Add(studentEntry);

                // Update siblingStudentclass for each entry and order by SibClassID descending
                foreach (var j in i.SiblingsData)
                {
                    j.siblingsStudentclass = Class.FirstOrDefault(x => x.DataListItemId == j.SibClassID)?.DataListItemName;
                }

                // Order the SiblingsData list by SibClassID in descending order
                i.SiblingsData = i.SiblingsData.OrderByDescending(j => j.SibClassID).ToList();
            }



            ViewBag.SiblingsData = siblingsViewModels;
            ViewBag.SiblingsData_ = groupStudentBySiblings;


            return View();
        }

        public ActionResult AddSiblings(SiblingsViewModel siblingsViewModel)
        {

            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                Tbl_Siblings siblingsView = new Tbl_Siblings();
                siblingsView.Student_Id = siblingsViewModel.Student_Id;
                siblingsView.Class_id = siblingsViewModel.Class_id;
                siblingsView.Studentname = siblingsViewModel.Studentname;
                siblingsView.Confirmation = siblingsViewModel.Confirmation;

                var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == siblingsView.Student_Id);
                if (familydetails != null)
                {
                    siblingsView.FamilyDetails_Id = familydetails.Id;
                }

                _context.Tbl_Siblings.Add(siblingsView);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Data Added Successully');location.replace(" + url + ")</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return View();
        }

        public ActionResult UpdateSiblings(SiblingsViewModel siblingsViewModel)
        {

            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var siblingsdata = _context.Tbl_Siblings.FirstOrDefault(x => x.Siblings_Id == siblingsViewModel.Siblings_Id);
                if (siblingsdata != null)
                {
                    Tbl_Siblings tbl_Siblings = new Tbl_Siblings();
                    tbl_Siblings.Siblings_Id = siblingsViewModel.Siblings_Id;
                    tbl_Siblings.Student_Id = siblingsViewModel.Student_Id;
                    tbl_Siblings.Class_id = siblingsViewModel.Class_id;
                    tbl_Siblings.Confirmation = siblingsViewModel.Confirmation;
                    tbl_Siblings.Studentname = siblingsViewModel.Studentname;

                    _context.Entry(siblingsdata).CurrentValues.SetValues(tbl_Siblings);
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successully');location.replace(" + url + ")</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }

            //return View();
        }

        public JsonResult GetsiblingsById(int Id)
        {
            try
            {
                SiblingsViewModel siblingsViewModel = new SiblingsViewModel();
                var siblingsdata = _context.Tbl_Siblings.FirstOrDefault(x => x.Siblings_Id == Id);
                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var studentdata = _context.Students.Where(x => x.IsApprove != 192).ToList();

                if (siblingsdata != null)
                {
                    var studentdetails = studentdata.FirstOrDefault(x => x.StudentId == siblingsdata.Student_Id);
                    siblingsViewModel.Siblings_Id = siblingsdata.Siblings_Id ?? 0;
                    siblingsViewModel.Studentname = siblingsdata.Studentname;
                    siblingsViewModel.Confirmation = siblingsdata.Confirmation;
                    //siblingsViewModel.Classname = Class.FirstOrDefault(x => x.DataListItemId == siblingsdata.Class_id)?.DataListItemName;
                    siblingsViewModel.Classname = siblingsdata.Class_id.ToString();
                    siblingsViewModel.siblingsstudentname = studentdetails.Name + " " + studentdetails.Last_Name;
                    //siblingsViewModel.Studentclass = Class.FirstOrDefault(x => x.DataListItemId == studentdetails.Class_Id)?.DataListItemName;
                    siblingsViewModel.Studentclass = studentdetails.Class_Id.ToString();
                }

                return Json(siblingsViewModel, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //Delete Siblings
        public ActionResult DeleteSiblings(int id)
        {
            //var url = Request.UrlReferrer.AbsoluteUri;
            try
            {
                var data = _context.Tbl_Siblings.FirstOrDefault(x => x.Siblings_Id == id);
                if (data != null)
                {
                    _context.Tbl_Siblings.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Deleted Successully');location.replace('/Student/ManageSiblings')</script>");

                    //return Content("<script language='javascript' type='text/javascript'>alert('Data Deleted Successully');location.replace(/Student/ManageSiblings)</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('No Data in this name');location.replace('/Student/ManageSiblings')</script>");

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        public string SendEmail(string Tomailid, string subject, string bodymessage)
        {
            try
            {

                string sourceMailID = ConfigurationManager.AppSettings["SourceMailID"];
                string sourceMailPassword = ConfigurationManager.AppSettings["SourceMailPassword"];
                string smtpsHost = ConfigurationManager.AppSettings["smtpsHost"];
                int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpsPort"]);
                bool enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                bool useDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(sourceMailID);
                    mail.To.Add(Tomailid);
                    mail.To.Add(sourceMailID);
                    mail.Subject = subject;
                    mail.Body = bodymessage;
                    mail.IsBodyHtml = true;
                    mail.ReplyToList.Add(Tomailid);
                    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                    using (SmtpClient smtp1 = new SmtpClient(smtpsHost.Trim(), port))
                    {
                        smtp1.EnableSsl = enableSSL;
                        smtp1.UseDefaultCredentials = useDefaultCredentials;
                        //smtp1.Timeout = 20000;
                        smtp1.Credentials = new NetworkCredential(sourceMailID.Trim(), sourceMailPassword.Trim());
                        smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp1.Send(mail);
                    }
                }
            }
            catch (SmtpFailedRecipientsException sfrEx)
            {
                // TODO: Handle exception
                // When email could not be delivered to all receipients.
                return "F";
            }
            catch (SmtpException sEx)
            {
                // TODO: Handle exception
                // When SMTP Client cannot complete Send operation.
                return "F";
            }
            catch (Exception ex)
            {
                return "F";
            }
            return "S";
        }


        public ActionResult StudentReport()
        {
            try
            {
                var StudentDetails = _context.Students.Where(x => x.IsApplyforTC == false).Where(x => x.IsApprove == 217).ToList();
                //ViewBag.StudentDetails = StudentDetails;
                //var StudentDetails = _context.Students.ToList();            
                Student student = new Student();
                FamilyDetail familyDetail = new FamilyDetail();

                ViewBag.Batchlist = _context.Tbl_Batches.ToList();

                ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

                ViewBag.Sections = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

                var Batch = _context.Tbl_Batches.ToList();

                var batches = _context.Tbl_Batches./*Where(x => x.IsActiveForAdmission == true).*/ToList();
                ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");

                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

                var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

                ViewBag.Admissionprocess = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

                var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
                var pagename = "Student Report";
                var editpermission = "Edit_Permission";
                var viewpermission = "Update_Permission";
                var DeletePermission = "Delete_Permission";
                List<StudentsRegistration> StudentsRegistration = new List<StudentsRegistration>();
                List<AllStudentDetailsViewModel> alldata = new List<AllStudentDetailsViewModel>();


                foreach (var item in StudentDetails)
                {
                    //long? SCL = 0;

                    //var GettingData = _context.Tbl_StudentPromotes.FirstOrDefault(x => x.Student_Id == item.StudentId);

                    //if(GettingData.Student_Id == 1887)
                    //{
                    //    SCL = Convert.ToInt64(GettingData.ScholarNumber);
                    //}

                    var studentReg = _context.Students.Where(x => x.ApplicationNumber == item.ApplicationNumber).FirstOrDefault();
                    familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentReg.ApplicationNumber);
                    {
                        // SCL = Convert.ToInt64(GettingData.ScholarNumber);

                        alldata.Add(new AllStudentDetailsViewModel
                        {

                            ApplicationNumber = item.ApplicationNumber,
                            Name = item.Name ?? string.Empty,
                            Last_Name = item.Last_Name ?? string.Empty,
                            Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                            Section = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,
                            DOB = item.DOB,
                            StudentRegisterID = item.StudentId,
                            BatchName = StudentCategory.FirstOrDefault(x => x.DataListItemId == item.IsApprove)?.DataListItemName,
                            ParentEmail = item.ParentEmail ?? string.Empty,
                            BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == item.BloodGroup_Id)?.DataListItemName,
                            FatherName = familyDetail == null ? string.Empty : familyDetail.FatherName,
                            MotherName = familyDetail == null ? string.Empty : familyDetail.MotherName,
                            FMobile = familyDetail == null ? string.Empty : familyDetail.FMobile,
                            MMobile = familyDetail == null ? string.Empty : familyDetail.MMobile,
                            Editpermission = CheckEditpermission(pagename, editpermission),
                            ViewPermission = CheckViewpermission(pagename, viewpermission),
                            DeletePermission = CheckDeletepermission(pagename, DeletePermission),
                            ScholarNo = item.ScholarNo


                        });
                    }
                }
                ViewBag.StudentDetails = alldata;
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public JsonResult FileterStudentReport(StudentRegFilterModel objStudentRegFilterModel)
        {
            string month = null;
            string year = null;
            var CurrentBatch = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true && x.IsActiveForAdmission == true).ToList().FirstOrDefault();
            bool isCurrentBatch = objStudentRegFilterModel.BatchId > 0 && CurrentBatch != null && objStudentRegFilterModel.BatchId == CurrentBatch.Batch_Id;
            if (objStudentRegFilterModel.FromDate != null)
            {
                DateTime date = DateTime.ParseExact(objStudentRegFilterModel.FromDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                month = date.Month.ToString();
                year = date.Year.ToString();
            }

            DateTime dt = DateTime.Now;
            var year1 = dt.Year.ToString();
            //var StudentDetails = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Class_Id == objStudentRegMasterModel.Class_Id).ToList();
            //string month = null;
            //string year = null;
            //if (objStudentRegMasterModel.FromDate != null)
            //{
            //    DateTime date = DateTime.ParseExact(objStudentRegMasterModel.FromDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            //    month = date.Month.ToString();
            //    year = date.Year.ToString();
            //}

            //DateTime dt = DateTime.Now;
            //var year1 = dt.Year.ToString();


            //var isapprove = Convert.ToInt32(objStudentRegMasterModel.AdmissionProcess_Id);
            //var classid = Convert.ToInt32(objStudentRegMasterModel.Class);
            //var sectionid = Convert.ToInt32(objStudentRegMasterModel.Section);
            //var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();

            List<Student> StudentDetails = new List<Student>();
            List<AllStudentDetailsViewModel> alldata = new List<AllStudentDetailsViewModel>();
            FamilyDetail familyDetail = new FamilyDetail();

            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var isapprove = Convert.ToInt32(objStudentRegFilterModel.AdmissionProcessId);
            var classid = Convert.ToInt32(objStudentRegFilterModel.ClassId);

            if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.SectionId > 0 && objStudentRegFilterModel.BatchId > 0 && objStudentRegFilterModel.AdmissionProcessId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.Section_Id == objStudentRegFilterModel.SectionId && x.Batch_Id == objStudentRegFilterModel.BatchId && x.IsApprove == objStudentRegFilterModel.AdmissionProcessId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();

                //StudentDetails = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.IsApprove == isapprove && x.Promotion_Year != year1).ToList();

            }
            else if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.SectionId > 0 && objStudentRegFilterModel.BatchId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.Section_Id == objStudentRegFilterModel.SectionId && x.Batch_Id == objStudentRegFilterModel.BatchId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.SectionId > 0 && objStudentRegFilterModel.AdmissionProcessId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.Section_Id == objStudentRegFilterModel.SectionId && x.IsApprove == objStudentRegFilterModel.AdmissionProcessId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.SectionId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.Section_Id == objStudentRegFilterModel.SectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.BatchId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.Batch_Id == objStudentRegFilterModel.BatchId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.ClassId > 0 && objStudentRegFilterModel.AdmissionProcessId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.IsApprove == objStudentRegFilterModel.AdmissionProcessId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.ClassId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Class_Id == objStudentRegFilterModel.ClassId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.SectionId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Section_Id == objStudentRegFilterModel.SectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.BatchId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.Batch_Id == objStudentRegFilterModel.BatchId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }
            else if (objStudentRegFilterModel.AdmissionProcessId > 0)
            {
                StudentDetails = _context.Students.Where(x => x.IsApprove == objStudentRegFilterModel.AdmissionProcessId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }

            else
            {
                StudentDetails = _context.Students.Where(x => x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
            }

            var pagename = "Student Report";
            var editpermission = "Edit_Permission";
            var viewpermission = "Update_Permission";
            var deletePermission = "Delete_Permission";
            List<StudentsRegistration> StudentsRegistration = new List<StudentsRegistration>();
            if (isCurrentBatch || objStudentRegFilterModel.BatchId == 0)
            {
                foreach (var item in StudentDetails)
                {
                    var studentReg = _context.Students.Where(x => x.ApplicationNumber == item.ApplicationNumber).FirstOrDefault();
                    familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentReg.ApplicationNumber);
                    {
                        alldata.Add(new AllStudentDetailsViewModel
                        {
                            ApplicationNumber = item.ApplicationNumber,
                            Name = item.Name ?? string.Empty,
                            Last_Name = item.Last_Name ?? string.Empty,
                            Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                            Section = Section.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,
                            DOB = item.DOB ?? string.Empty,
                            StudentRegisterID = item.StudentId,
                            BatchName = StudentCategory.FirstOrDefault(x => x.DataListItemId == item.IsApprove)?.DataListItemName,
                            ParentEmail = item.ParentEmail ?? string.Empty,
                            BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == item.BloodGroup_Id)?.DataListItemName,
                            FatherName = familyDetail == null ? string.Empty : familyDetail.FatherName,
                            MotherName = familyDetail == null ? string.Empty : familyDetail.MotherName,
                            FMobile = familyDetail == null ? string.Empty : familyDetail.FMobile,
                            MMobile = familyDetail == null ? string.Empty : familyDetail.MMobile,
                            Editpermission = CheckEditpermission(pagename, editpermission),
                            ViewPermission = CheckViewpermission(pagename, viewpermission),
                            DeletePermission = CheckDeletepermission(pagename, deletePermission),
                            ScholarNo = item.ScholarNo
                            //Section = Section.FirstOrDefault(x=>x.DataListItemId == item.Section_Id)?.DataListItemName

                        });
                    }
                }
            }
            else
            {
                alldata = (from tr in _context.Tbl_TestRecord
                           join st in _context.Students on tr.StudentID equals st.StudentId
                           join cls in _context.DataListItems on tr.ClassID equals cls.DataListItemId
                           join sec in _context.DataListItems on tr.SectionID equals sec.DataListItemId
                           where tr.BatchId == objStudentRegFilterModel.BatchId && tr.ClassID == objStudentRegFilterModel.ClassId && tr.SectionID == objStudentRegFilterModel.SectionId
                           select new AllStudentDetailsViewModel
                           {
                               ApplicationNumber = st.ApplicationNumber,
                               Name = st.Name ?? string.Empty,
                               Last_Name = st.Last_Name ?? string.Empty,
                               Class = cls.DataListItemName ?? string.Empty,
                               Section = sec.DataListItemName ?? string.Empty,
                               DOB = st.DOB ?? string.Empty,
                               StudentRegisterID = st.StudentId,
                               BatchName = _context.Tbl_Batches.Where(x => x.Batch_Id == tr.BatchId).Select(x => x.Batch_Name).FirstOrDefault(),
                               ParentEmail = st.ParentEmail ?? string.Empty,
                               BloodGroup = _context.DataListItems.Where(x => x.DataListItemId == st.Batch_Id).Select(x => x.DataListItemName).FirstOrDefault(), //BloodGroup.FirstOrDefault(x => x.DataListItemId == st.BloodGroup_Id)?.DataListItemName,
                                                                                                                                                                 //FatherName = familyDetail == null ? string.Empty : familyDetail.FatherName,
                                                                                                                                                                 //MotherName = familyDetail == null ? string.Empty : familyDetail.MotherName,
                                                                                                                                                                 //FMobile = familyDetail == null ? string.Empty : familyDetail.FMobile,
                                                                                                                                                                 //MMobile = familyDetail == null ? string.Empty : familyDetail.MMobile,
                                                                                                                                                                 //Editpermission = CheckEditpermission(pagename, editpermission),
                                                                                                                                                                 //ViewPermission = CheckViewpermission(pagename, viewpermission),
                                                                                                                                                                 //DeletePermission = CheckDeletepermission(pagename, deletePermission),
                               ScholarNo = st.ScholarNo
                           }).Distinct().OrderBy(x => x.Name).ToList();
                foreach (var student in alldata)
                {
                    var family = _context.FamilyDetails.FirstOrDefault(f => f.ApplicationNumber == student.ApplicationNumber);
                    student.FatherName = family?.FatherName ?? string.Empty;
                    student.MotherName = family?.MotherName ?? string.Empty;
                    student.FMobile = family?.FMobile ?? string.Empty;
                    student.MMobile = family?.MMobile ?? string.Empty;

                    student.Editpermission = CheckEditpermission(pagename, editpermission);
                    student.ViewPermission = CheckViewpermission(pagename, viewpermission);
                    student.DeletePermission = CheckDeletepermission(pagename, deletePermission);
                }
            }
            //}

            return Json(alldata, JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        public ActionResult ExportStudentToExcelReport(int Classid, int Sectionid, int BatchId)
        {

            var data = _context.StudentsRegistrations.Where(x => x.Class_Id == Classid && x.Section_Id == Sectionid).ToList();
            var currentatch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true && x.IsActiveForPayments == true).OrderByDescending(x => x.Batch_Id).Select(x => x.Batch_Id).FirstOrDefault();
            bool isCurrentBatch = BatchId > 0 && currentatch != null && BatchId == currentatch;

            if (data.Count > 0)
            {

                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    SqlCommand cmd;
                    string query;
                    query = @"EXEC usp_ExportStudentData  @ClassId,  @SectionId,  @BatchId";


                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ClassId", Classid);
                    cmd.Parameters.AddWithValue("@SectionId", Sectionid);
                    cmd.Parameters.AddWithValue("@BatchId", BatchId);
                    cmd.CommandTimeout = 300;
                    conn.Open();
                    SqlDataAdapter da = new SqlDataAdapter();
                    da.SelectCommand = cmd;
                    DataTable dt = new DataTable();
                    dt.TableName = "example";
                    //dt.AcceptChanges();
                    da.Fill(dt);
                    conn.Close();
                    cmd.Dispose();
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt, "sheet1");
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                        }
                    }
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No Data Present In the Date');location.replace('ManageStudent')</script>");
            }


        }

        public ActionResult ExistingStudentAdmissionProcess()
        {

            DateTime dt = DateTime.Now;
            var year1 = dt.Year.ToString();



            List<StudentPromoteVM> studentPromoteVMs = new List<StudentPromoteVM>();

            var StudentDetails = _context.StudentsRegistrations.Where(x => x.Promotion_Year == year1 && x.IsApprove != 192).ToList();




            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

            ViewBag.Admissionprocess = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();


            List<StudentsRegistration> StudentsRegistration = new List<StudentsRegistration>();
            foreach (var item in StudentDetails)
            {
                StudentsRegistration.Add(new StudentsRegistration
                {
                    ApplicationNumber = item.ApplicationNumber,
                    Name = item.Name,
                    Last_Name = item.Last_Name,
                    Class = Class.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                    DOB = item.DOB,
                    StudentRegisterID = item.StudentRegisterID,
                    BatchName = StudentCategory.FirstOrDefault(x => x.DataListItemId == item.IsApprove)?.DataListItemName

                });
            }
            ViewBag.StudentList = StudentsRegistration;
            return View();
        }


        public JsonResult UpdatedExistingStudentAdmissionStatus(StudentAdmissionModel objStudentAdmissionModel)
        {
            try
            {
                int studentid = Convert.ToInt32(objStudentAdmissionModel.StudentId);


                var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
                int Category_Id = Convert.ToInt32(AllCategorys.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.Category)?.DataListItemId);
                int Class_ID = Convert.ToInt32(Classes.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.Course)?.DataListItemId);
                //int BloodGroup_ID = Convert.ToInt32(BloodGroup.FirstOrDefault(x => x.DataListItemName ))
                var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

                Tbl_DataListItem objTbl_DataListItem;

                if (objStudentAdmissionModel.AdmissionStatusID == "217")
                {
                    int admissionprocesid = Convert.ToInt32(objStudentAdmissionModel.AdmissionStatusID);
                    objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemId == admissionprocesid);
                }
                else
                {
                    objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.AdmissionStatusID);
                }


                Tbl_DataListItem objTbl_DataListItemPreviews = StudentCategory.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.isapprove);

                #region StudentRegistration Update


                StudentsRegistration objstudentsRegistrationUpdate = new StudentsRegistration();
                var StudentData = _context.StudentsRegistrations.Where(e => e.StudentRegisterID == studentid);
                objstudentsRegistrationUpdate = StudentData.FirstOrDefault(x => x.StudentRegisterID == studentid);


                #endregion


                if (objStudentAdmissionModel.AdmissionStatusID == "ADMITTED" || objStudentAdmissionModel.AdmissionStatusID == "217")
                {
                    #region status updated 
                    StudentAdmissionStatusUpdate objStudentAdmissionStatusUpdate = new StudentAdmissionStatusUpdate
                    {
                        IsApprove = objTbl_DataListItem.DataListItemId
                    };
                    var existingobj = _context.StudentsRegistrations.FirstOrDefault(e => e.StudentRegisterID == studentid);
                    _context.Entry(existingobj).CurrentValues.SetValues(objStudentAdmissionStatusUpdate);
                    _context.SaveChanges();
                    #endregion
                    SendEmail(/*"zingerarul@gmail.com"*/"" + objstudentsRegistrationUpdate.Parents_Email + "", "Application of " + objstudentsRegistrationUpdate.Name + " has been moved to the " + objTbl_DataListItem.DataListItemName + " Status", "Your Application (" + objstudentsRegistrationUpdate.ApplicationNumber + ") has been moved to " + objTbl_DataListItem.DataListItemName + " status successfully.");

                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //---x-rnik---
                    string appNum = _context.StudentsRegistrations.Where(y => y.StudentRegisterID == studentid).FirstOrDefault().ApplicationNumber;

                    StudentAdmissionStatusUpdate objStudentStatusUpdate = new StudentAdmissionStatusUpdate
                    {
                        IsApprove = Convert.ToInt32(objStudentAdmissionModel.AdmissionStatusID)
                    };
                    var existingobj = _context.Students.Where(e => e.ApplicationNumber == appNum).FirstOrDefault();
                    _context.Entry(existingobj).CurrentValues.SetValues(objStudentStatusUpdate);
                    _context.SaveChanges();
                    //---
                }


            }
            catch (Exception ex)
            {

            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditStudentAttendance()
        {
            if (Session["RoleName"] != null)
            {
                string roleName = Session["RoleName"].ToString();
                // Use the roleName as needed
                if (roleName == "Staff")
                {
                    long staffId = Int64.Parse(Session["StaffID"].ToString());
                    var staff = _context.StafsDetails.Where(x => x.StafId == staffId).ToList();
                    ViewBag.Staff = staff;

                }
                else
                {
                    var staff = _context.StafsDetails.ToList();
                    ViewBag.Staff = staff;
                }
            }
            return View();
        }

        #region GetStudentAttendanceByDateold 
        public JsonResult GetStudentAttendanceByDate(int classid, int sectionid, string date)
        {
            var allStudentlist = _context.Students.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();

            foreach (var item in allStudentlist)
            {
                item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionid).Select(x => x.DataListItemName).FirstOrDefault();
                item.Class = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault();
            }

            var studentlist = _context.Tbl_StudentAttendance.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Created_Date == date).OrderBy(x => x.Student_Name).ToList();

            foreach (var item in studentlist)
            {
                item.Student_Name = _context.Students.Where(x => x.StudentId == item.StudentRegisterID && x.Class_Id == classid && x.Section_Id == sectionid).Select(x => x.Name).FirstOrDefault();
                item.Section_Name = _context.DataListItems.Where(x => x.DataListItemId == item.Section_Id).Select(x => x.DataListItemName).FirstOrDefault();
                item.Class_Name = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault();
            }
            var attendanceList = new List<Tbl_StudentAttendance>();
            if (studentlist.Count > 0)
            {

                foreach (var student in allStudentlist)
                {
                    var attendanceRecord = studentlist.FirstOrDefault(s => s.StudentRegisterID == student.StudentId);

                    if (attendanceRecord == null)
                    {
                        attendanceRecord = new Tbl_StudentAttendance
                        {
                            Class_Id = classid,
                            Section_Id = sectionid,
                            Class_Name = student.Class,
                            Section_Name = student.Section,
                            StudentRegisterID = student.StudentId,
                            Student_Name = student.Name,
                            Created_Date = date,
                            Mark_FullDayAbsent = attendanceRecord?.Mark_FullDayAbsent,
                            Mark_HalfDayAbsent = attendanceRecord?.Mark_HalfDayAbsent,
                            Others = attendanceRecord?.Others,
                        };
                    }

                    attendanceList.Add(attendanceRecord);
                }
                return Json(attendanceList, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(studentlist, JsonRequestBehavior.AllowGet);

            }
        }
        #endregion
        [HttpPost]
        public ActionResult UpdateStudentAttendanceData(List<Tbl_StudentAttendance> rowData, int classId, int sectionId, string attendanceDate)
        {

            //check any previous attendance by date
            bool hasMatchingRecord = _context.Tbl_StudentAttendance.Any(c => c.Created_Date == attendanceDate && c.Class_Id == classId && c.Section_Id == sectionId);

            if (!hasMatchingRecord)
            {
                return Json(new { success = false, errormsg = "Please select proper class, section, and date" });
            }
            // Save the data into the database table
            foreach (var item in rowData)
            {
                var rowItem = _context.Tbl_StudentAttendance
        .FirstOrDefault(x => x.StudentRegisterID == item.StudentRegisterID && x.Created_Date == attendanceDate && x.Class_Id == item.Class_Id && x.Section_Id == item.Section_Id);
                var ClassName = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == item.Class_Id).DataListItemName;
                var sectioName = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == item.Section_Id).DataListItemName;
                var studentFullName = _context.Students.Where(x => x.StudentId == item.StudentRegisterID).Select(x => x.Name + " " + x.Last_Name).FirstOrDefault();

                if (rowItem == null)
                {
                    // Create a new entity object and set its properties
                    var newAttendanceRecord = new Tbl_StudentAttendance
                    {
                        StudentRegisterID = item.StudentRegisterID,
                        Created_Date = attendanceDate,
                        Class_Id = item.Class_Id,
                        Section_Id = item.Section_Id,
                        Mark_FullDayAbsent = item.Mark_FullDayAbsent,
                        Mark_HalfDayAbsent = item.Mark_HalfDayAbsent,
                        Others = item.Others,
                        Student_Name = studentFullName,
                        Class_Name = ClassName,
                        Section_Name = sectioName,
                        Day = item.Day,
                        Created_By = item.Created_By,
                        BatchId = item.BatchId

                        // Set other properties as needed
                    };

                    // Add the new attendance record to the context
                    _context.Tbl_StudentAttendance.Add(newAttendanceRecord);
                    _context.SaveChanges();
                }
                else
                {
                    // Create a new entity object and set its properties
                    rowItem.Mark_FullDayAbsent = item.Mark_FullDayAbsent;
                    rowItem.Mark_HalfDayAbsent = item.Mark_HalfDayAbsent;
                    rowItem.Others = item.Others;
                    rowItem.Student_Name = studentFullName;
                    rowItem.Class_Name = ClassName;
                    rowItem.Section_Id = sectionId;
                    // Save changes to the database
                    _context.SaveChanges();
                }
            }


            return Json(new { success = true }); // Return a success response if the data is saved successfully
        }

        [HttpPost]
        public ActionResult DeleteStudentAttendence(List<Tbl_StudentAttendance> rowData, int classId, int sectionId, string attendanceDate)
        {

            //check any previous attendance by date
            bool hasMatchingRecord = _context.Tbl_StudentAttendance.Any(c => c.Created_Date == attendanceDate && c.Class_Id == classId && c.Section_Id == sectionId);

            if (!hasMatchingRecord)
            {
                return Json(new { success = false, errormsg = "Please select proper class, section, and date" });
            }
            // Save the data into the database table
            foreach (var item in rowData)
            {
                var rowItem = _context.Tbl_StudentAttendance
                                .FirstOrDefault(x => x.StudentRegisterID == item.StudentRegisterID && x.Created_Date == attendanceDate && x.Class_Id == item.Class_Id && x.Section_Id == item.Section_Id);
                if (rowItem != null)
                {
                    _context.Tbl_StudentAttendance.Remove(rowItem);
                    _context.SaveChanges();
                }
                else
                {
                    continue;
                }
            }


            return Json(new { success = true }); // Return a success response if the data is saved successfully
        }
        public JsonResult FilterExistingStudents(StudentRegMasterModel objStudentRegMasterModel)
        {
            string year = "";
            //string html = "";

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var classid = Convert.ToInt32(objStudentRegMasterModel.Class);
            var isapprove = Convert.ToInt32(objStudentRegMasterModel.AdmissionProcess_Id);

            if (objStudentRegMasterModel.FromDate != null)
            {
                DateTime date = DateTime.ParseExact(objStudentRegMasterModel.FromDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);

                year = Convert.ToDateTime(date).Year.ToString();
            }


            List<AdmissionStudentDetailVM> studentls = new List<AdmissionStudentDetailVM>();

            List<StudentsRegistration> result = new List<StudentsRegistration>();
            List<StudentApproveStatusModel> studentApproveStatus = new List<StudentApproveStatusModel>();

            List<Student> lstStudent = new List<Student>();//x-rnik---

            if (objStudentRegMasterModel.AdmissionProcess_Id != 0 && objStudentRegMasterModel.FromDate != null && classid != 0)
            {
                result = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.Promotion_Year == year && x.IsApprove == isapprove).ToList();
                lstStudent = _context.Students.Where(x => x.Class_Id == classid && x.Date == year && x.IsApprove == isapprove).ToList();//x-rnik---
            }
            else if (objStudentRegMasterModel.FromDate != null && objStudentRegMasterModel.AdmissionProcess_Id != 0)
            {
                result = _context.StudentsRegistrations.Where(x => x.Promotion_Year == year && x.IsApprove == isapprove).ToList();
                lstStudent = _context.Students.Where(x => x.Date == year && x.IsApprove == isapprove).ToList();//x-rnik---
            }
            else if (objStudentRegMasterModel.FromDate != null && classid != 0)
            {
                result = _context.StudentsRegistrations.Where(x => x.Promotion_Year == year && x.Class_Id == classid).ToList();
                lstStudent = _context.Students.Where(x => x.Date == year && x.Class_Id == classid).ToList();//x-rnik---
            }
            else
            {
                result = _context.StudentsRegistrations.Where(x => x.Promotion_Year == year).ToList();
                lstStudent = _context.Students.Where(x => x.Date == year).ToList();  //x-rnik---
            }
            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();

            ////var isApprove = _context.Students.Where(x=>x.studentid)

            //foreach (var res in result)
            //{
            //    AdmissionStudentDetailVM studentobj = new AdmissionStudentDetailVM();
            //    studentobj.ScholarNumber = res.StudentRegisterID.ToString();
            //    studentobj.StudentName = res.Name;
            //    studentobj.LastName = res.Last_Name;
            //    studentobj.DOB = res.DOB;
            //    studentobj.Class_Id = res.Class_Id;
            //    studentobj.AdmissionNumber = res.ApplicationNumber;
            //    studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
            //    studentobj.StudentId = res.StudentRegisterID;
            //    Tbl_DataListItem objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemId == res.IsApprove);
            //    studentobj.isapprove = objTbl_DataListItem.DataListItemName.ToString();
            //    studentls.Add(studentobj);
            //}

            List<StudentsRegistration> srData = new List<StudentsRegistration>();
            srData = _context.StudentsRegistrations.Where(x => x.Promotion_Year == year).ToList();
            foreach (var res in lstStudent)
            {
                AdmissionStudentDetailVM studentobj = new AdmissionStudentDetailVM();
                studentobj.ScholarNumber = (srData.Where(x => x.ScholarNo == res.ScholarNo).FirstOrDefault().StudentRegisterID).ToString();//res..ToString();
                studentobj.StudentName = res.Name;
                studentobj.LastName = res.Last_Name;
                studentobj.DOB = res.DOB;
                studentobj.Class_Id = res.Class_Id;
                studentobj.AdmissionNumber = res.ApplicationNumber;
                studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.StudentId = srData.Where(x => x.ApplicationNumber == res.ApplicationNumber).FirstOrDefault().StudentRegisterID;//res.StudentRegisterID;
                Tbl_DataListItem objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemId == res.IsApprove);
                studentobj.isapprove = objTbl_DataListItem.DataListItemName.ToString();
                studentls.Add(studentobj);
            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }

        //check tappermission
        //edit permission
        public string CheckEditpermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Edit_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Edit_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //create permission
        public string CheckCreatepermission(string pagename, string permission)
        {
            try
            {

                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Create_permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Create_permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        //Delete Permission
        public string CheckDeletepermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Delete_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Delete_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //View Permission

        public string CheckViewpermission(string pagename, string permission)
        {
            try
            {
                var result = "false";
                var data = Session["RolepermissionNew"] as List<Tbl_RolePermissionNew>;
                if (data != null && data.Count > 0)
                {
                    if (permission == "Update_Permission")
                    {
                        var tappermission = data.FirstOrDefault(x => x.Submenu_Name == pagename && x.Update_Permission == true);
                        if (tappermission != null)
                        {
                            result = "true";
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //Student Remark

        public ActionResult GetStaffClassById(int staffId)
        {
            // Retrieve class data based on the staff ID
            var query = (from s in _context.Subjects
                         join c in _context.DataListItems on s.Class_Id equals c.DataListItemId
                         where s.StaffId == staffId
                         select c).Distinct();


            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StudentRemark()
        {
            try
            {
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                ViewBag.ClassList = Classes;
                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                ViewBag.SectionList = Section;
                if (Session["RoleName"] != null)
                {
                    string roleName = Session["RoleName"].ToString();
                    // Use the roleName as needed
                    if (roleName == "Staff")
                    {
                        long staffId = Int64.Parse(Session["StaffID"].ToString());
                        var staff = _context.StafsDetails.Where(x => x.StafId == staffId).ToList();
                        ViewBag.Staff = staff;

                    }
                    else
                    {
                        var staff = _context.StafsDetails.OrderBy(x => x.Name).ToList();
                        ViewBag.Staff = staff;
                        var BatchList = _context.Tbl_Batches.Select(x => new Data.Models.BatchListDTO
                        {
                            Batch_Id = x.Batch_Id,
                            Batch_Name = x.Batch_Name
                        }).OrderBy(x => x.Batch_Name).ToList();
                        ViewBag.BatchList = BatchList;
                    }
                }
                var Terms = _context.tbl_Term.ToList();
                ViewBag.Terms = Terms;
                var batch = _context.Tbl_Batches.Distinct().ToList();
                ViewBag.Batch = batch;

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        public ActionResult GetClassSectionById(int staffId, int classId)
        {
            // Retrieve class data based on the staff ID
            var query = (from s in _context.Subjects
                         join c in _context.DataListItems on s.Section_Id equals c.DataListItemId
                         where s.StaffId == staffId && s.Class_Id == classId
                         select c).Distinct();



            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TermsData()
        {
            var Terms = _context.tbl_Term.ToList();

            return Json(Terms, JsonRequestBehavior.AllowGet);
        }

        public JsonResult StudentByClassSection(int classId, int sectionId, int testId, int termId, int staffId, int batchId)
        {
            try
            {
                var students = _context.Students
                    .Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false)
                    .OrderBy(x => x.Name)
                    .Select(s => new { StudentId = s.StudentId, StudentName = s.Name })
                    .ToList();

                return Json(students, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult InsertUpdateObtainedMarks(List<Tbl_StudentRemark> rowData, int staffId)
        {
            // var existingRecord = _context.Tbl_TestRecord.FirstOrDefault(tr => tr.TermID == item.TermID && tr.StudentID == item.StudentID && tr.ClassID == item.ClassID && tr.SectionID == item.SectionId && tr.BoardID == getBoardID);

            foreach (var item in rowData)
            {
                // var existingRecord = _context.StudentRemark.FirstOrDefault(e => e.StudentId == item.StudentId && e.Class_Id == item.Class_Id && e.Section_Id == item.Section_Id);
                var newRemark = new Tbl_StudentRemark
                {
                    StudentRemarkId = item.StudentRemarkId,
                    Remark = item.Remark,
                    Reward = item.Reward,
                    Awards = item.Awards,
                    Punishment = item.Punishment,
                    Class_Id = item.Class_Id,
                    Batch_Id = item.Batch_Id,
                    Section_Id = item.Section_Id,
                    StudentId = item.StudentId,
                    Term_Id = item.Term_Id
                };
                _context.StudentRemark.Add(newRemark);
            }
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (DbEntityEntry entry in ex.Entries)
                {
                    if (refreshMode == RefreshMode.ClientWins)
                        entry.OriginalValues.SetValues(entry.GetDatabaseValues());
                    else
                        entry.Reload();
                }
                _context.SaveChanges();
            }
            return Json(new { success = true });

        }





        //updated code 




    }

}
public class Tbl_StudentPromotesData
{
    public int PromoteId { get; set; }
    public string ScholarNumber { get; set; }
    public string FromClass { get; set; }
    public string ToClass { get; set; }
    public int FromClass_Id { get; set; }
    public int ToClass_Id { get; set; }
    public int Student_Id { get; set; }
    public string Registration_Date { get; set; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
}
public class Studentupdate
{
    public string UserId { get; set; }
}

public class Studentfamilyupdate
{
    public int StudentRefId { get; set; }
}

public class Tbl_StudentRemark
{
    [Key]
    public int StudentRemarkId { get; set; }
    public string Reward { get; set; }
    public string Awards { get; set; }
    public string Punishment { get; set; }
    public int Class_Id { get; set; }
    public int Section_Id { get; set; }
    public int Batch_Id { get; set; }
    public int Term_Id { get; set; }
    public string Remark { get; set; }
    public int StudentId { get; set; }
}





