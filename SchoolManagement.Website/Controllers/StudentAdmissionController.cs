using EmployeeManagement.Repository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.Models.DataAccess;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
namespace SchoolManagement.Website.Controllers
{
    public class StudentAdmissionController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationDbContext _contextstudent = new ApplicationDbContext();
        // GET: StudentAdmission
        private IRepository<StudentRegistrationHistory> _StudentRegistrationHistory = null;
        private IRepository<StudentRegNumberMaster> _StudentRegNumberMaster = null;
        private IRepository<Student> _Student = null;
        public StudentAdmissionController()
        {
            _StudentRegistrationHistory = new Repository<StudentRegistrationHistory>();
            _StudentRegNumberMaster = new Repository<StudentRegNumberMaster>();
            _Student = new Repository<Student>();
        }

        public async Task<ActionResult> StudentAdmission()
        {
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
            ViewBag.AllYears = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "year").DataListId.ToString()).ToList();
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();
            var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Admissionprocess = StudentCategory;
            var appProcessList = new SelectList(StudentCategory.ToList(), "DataListItemId", "DataListItemName");
            ViewBag.AppProcessList = appProcessList;
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();


            DateTime dt = DateTime.Now;
            var year1 = dt.Year.ToString();

            var batches = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");

            List<AdmissionStudentDetailVM> studentls = new List<AdmissionStudentDetailVM>();
            var result = await _context.StudentsRegistrations.Where(e => e.IsApprove == 189 && e.Promotion_Year != year1).OrderByDescending(x => x.StudentRegisterID).ToListAsync(); //OrderByDescending(x => x.StudentRegisterID)

            var ClassList = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString());

            //var pagename = "Student Admission Process";
            var pagename = "Student Admission";
            var editpermission = "Edit_Permission";



            foreach (var res in result)
            {
                AdmissionStudentDetailVM studentobj = new AdmissionStudentDetailVM();
                studentobj.ScholarNumber = res.ScholarNo.ToString();
                studentobj.StudentName = res.Name;
                studentobj.Category_Id = res.Category_Id;
                studentobj.Category = AllCategorys.FirstOrDefault(x => x.DataListItemId == res.Category_Id)?.DataListItemName;
                studentobj.Class_Id = res.Class_Id;
                studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.AdmissionFeePaid = res.AdmissionFeePaid;
                studentobj.AdmissionNumber = res.ApplicationNumber;
                studentobj.LastName = res.Last_Name;
                int Classid = 0;
                int.TryParse(res.Class, out Classid);
                string ReturnClassName = "";
                if (Classid != 0)
                {
                    var resutlclass = _context.DataListItems.Where(e => e.DataListItemId == Classid);
                    foreach (var cl in resutlclass)
                    {
                        ReturnClassName = cl.DataListItemName;
                    }
                }
                else
                {
                    ReturnClassName = res.Class;
                }
                studentobj.Course = ReturnClassName;
                studentobj.Semester = ReturnClassName;
                studentobj.StudentId = res.StudentRegisterID;
                Tbl_DataListItem objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemId == res.IsApprove);
                studentobj.isapprove = objTbl_DataListItem.DataListItemName.ToString();
                var per = CheckEditpermission(pagename, editpermission);
                studentobj.Editpermission = per;
                studentls.Add(studentobj);

            }
            List<Tbl_DataListItem> tbl_DataListItems = new List<Tbl_DataListItem>();

            ViewBag.StudentList = studentls;
            return View();
        }

        //[HttpPost]
        public JsonResult GeStudentList(StudentRegMasterModel objStudentRegMasterModel)
        {
            List<AdmissionStudentDetailVM> studentls = new List<AdmissionStudentDetailVM>();
            int year = 0;
            int.TryParse(objStudentRegMasterModel.FromDate, out year);
            
            var url = Request.UrlReferrer.AbsoluteUri;
            List<StudentsRegistration> result = new List<StudentsRegistration>();
            //var result = _context.StudentsRegistrations.Where(e => e.IsApprove != 191).ToList();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var classid = Convert.ToInt32( objStudentRegMasterModel.Class);
            var isapprove = Convert.ToInt32(objStudentRegMasterModel.AdmissionProcess_Id);
            //var Batchid = Convert.ToInt32( objStudentRegMasterModel.BatchName);

            //if(objStudentRegMasterModel.FromDate != null)
            //{
            //    DateTime date = DateTime.ParseExact(objStudentRegMasterModel.FromDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            //    year = Convert.ToDateTime(date).Year.ToString();
            //}

            

            DateTime dt = DateTime.Now;
            var year1 = dt.Year.ToString();
            

            if (objStudentRegMasterModel.AdmissionProcess_Id != 0 && year != 0)
            {
                result = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.Batch_Id == year && x.IsApprove == isapprove && x.Promotion_Year != year1).ToList();
            }
            else if (objStudentRegMasterModel.AdmissionProcess_Id != 0)
            {
                result = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.IsApprove == isapprove && x.Promotion_Year != year1).ToList();
            }
            else
            {
                result = _context.StudentsRegistrations.Where(x => x.Class_Id == classid && x.IsApprove != 192 && x.Promotion_Year != year1).ToList();
            }

            if (year != 0)
                result = result.Where(x => x.Batch_Id == year && x.IsApprove != 192).ToList();

            var pagename = "Student Admission";
            var editpermission = "Edit_Permission";

            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();
            foreach (var res in result)
            {
                AdmissionStudentDetailVM studentobj = new AdmissionStudentDetailVM();

                studentobj.ScholarNumber = res.StudentRegisterID.ToString();
                studentobj.StudentName = res.Name;
                studentobj.LastName = res.Last_Name;
                studentobj.Category_Id = res.Category_Id;
                studentobj.Category = AllCategorys.FirstOrDefault(x => x.DataListItemId == res.Category_Id)?.DataListItemName;
                studentobj.Class_Id = res.Class_Id;
                studentobj.AdmissionNumber = res.ApplicationNumber;
                studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                int Classid = 0;
                int.TryParse(res.Class, out Classid);
                string ReturnClassName = "";
                if (Classid != 0)
                {
                    var resutlclass = _context.DataListItems.Where(e => e.DataListItemId == Classid);
                    foreach (var cl in resutlclass)
                    {
                        ReturnClassName = cl.DataListItemName;
                    }
                }
                else
                {
                    ReturnClassName = res.Class;
                }
                studentobj.Course = ReturnClassName;
                studentobj.Semester = ReturnClassName;
                studentobj.StudentId = res.StudentRegisterID;
                studentobj.StudCate = new List<listitem>();
                foreach (var item in StudentCategory)
                {
                    listitem list = new listitem();
                    list.DataListItemId = item.DataListItemId.ToString();
                    list.DataListItemName = item.DataListItemName;
                    studentobj.StudCate.Add(list);
                }
                Tbl_DataListItem objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemId == res.IsApprove);
                studentobj.isapprove = objTbl_DataListItem.DataListItemName.ToString();
                var appProcessList = new SelectList(studentobj.StudCate.ToList(), "DataListItemId", "DataListItemName");
                ViewBag.AppProcessList = appProcessList;
                var per = CheckEditpermission(pagename, editpermission);
                //studentls.
                studentobj.Editpermission = per;
                studentls.Add(studentobj);

                //for (int i = 0; i < studentls.Count(); i++)
                //{
                //    html += "<tr>";
                //    html += "<td>" + studentls[i].StudentId + "</td>";
                //    html += "<td>" + studentls[i].StudentName + "</td>";
                //    html += "<td>" + studentls[i].Category + "</td>";
                //    html += "<td>" + studentls[i].Class + "</td>";
                //    html += "<td>" + studentls[i].isapprove + "</td>";
                //    //html += "<td>" + studentls[i].isapprove + "</td>";
                //    html += "<td> <select> for(int i = 0;)</select> </td>";
                //    html += "<td><a href='/Student/UdateStudentAdmissionDetails?Id=" + studentls[i].StudentId + "'>Edit</a></td>";
                //    html += "</tr>";
                //}
                //ViewBag.Html = html.Count();
            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProcessList()
        {
            var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();
            return Json(StudentCategory, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> UpdatedStudentAdmissionStatus(StudentAdmissionModel objStudentAdmissionModel)
        {
            try
            {
                if(objStudentAdmissionModel.Course == null)
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    int studentid = Convert.ToInt32(objStudentAdmissionModel.StudentId);


                    var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
                    var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                    var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
                    int Category_Id = Convert.ToInt32(AllCategorys.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.Category)?.DataListItemId);
                    int Class_ID = Convert.ToInt32(Classes.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.Course)?.DataListItemId);
                    //int BloodGroup_ID = Convert.ToInt32(BloodGroup.FirstOrDefault(x => x.DataListItemName ))
                    var StudentCategory = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "admission process").DataListId.ToString()).ToList();
                    var AllCaste= _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "caste").DataListId.ToString()).ToList();
                    var AllReligion= _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "religion").DataListId.ToString()).ToList();
                   var AllMedium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();


                    Tbl_DataListItem objTbl_DataListItem = StudentCategory.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.AdmissionStatusID);

                    Tbl_DataListItem objTbl_DataListItemPreviews = StudentCategory.FirstOrDefault(x => x.DataListItemName == objStudentAdmissionModel.isapprove);

                    #region History insert
                    StudentsRegistration objstudentsRegistration = new StudentsRegistration();


                    var result = _context.StudentsRegistrations.Where(e => e.StudentRegisterID == studentid);
                    objstudentsRegistration = result.FirstOrDefault(x => x.StudentRegisterID == studentid);

                    StudentRegistrationHistory StudentRegistrationHistory = new StudentRegistrationHistory()
                    {
                        BloodGroup = objstudentsRegistration.BloodGroup,
                        ApplicationNumber = objstudentsRegistration.ApplicationNumber.ToString(),
                        StudentRegisterID = studentid,
                        Category_Id = objstudentsRegistration.Category_Id,
                        Class_Id = Class_ID,
                        Category = objstudentsRegistration.Category,
                        Class = objstudentsRegistration.Class,
                        Date = DateTime.Now.ToString("dd/MM/yyyy"),
                        DOB = objstudentsRegistration.DOB,
                        Gender = objstudentsRegistration.Gender,
                        MotherTongue = objstudentsRegistration.MotherTongue,
                        Name = objstudentsRegistration.Name,
                        Caste = objstudentsRegistration.Caste,
                        Nationality = objstudentsRegistration.Nationality,
                        POB = objstudentsRegistration.POB,
                        // ProfileAvatar = studentViewModel.ProfileAvatar,
                        Religion = objstudentsRegistration.Religion,
                        IsApplyforAdmission = true,
                        IsApprove = objTbl_DataListItem.DataListItemId,
                        IsApprovePreview = objTbl_DataListItemPreviews.DataListItemId,
                        UIN = Guid.NewGuid().ToString(),
                        IsActive = true,
                        Parents_Email = objstudentsRegistration.Parents_Email,
                        
                    };
                    _StudentRegistrationHistory.Insert(StudentRegistrationHistory);
                    _StudentRegistrationHistory.Save();
                    #endregion
                    if (objStudentAdmissionModel.AdmissionStatusID == "ADMITTED")
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
                        
                        #region Student details insert 
                        var StudentDetails = _context.Students.FirstOrDefault(e => e.ApplicationNumber == objstudentsRegistration.ApplicationNumber);
                        if (StudentDetails != null)
                        {
                            StudentStatusUpdate objStudentStatusUpdate = new StudentStatusUpdate
                            {
                                IsActive = true
                            };
                            var existingstudent = _context.Students.FirstOrDefault(e => e.ApplicationNumber == objstudentsRegistration.ApplicationNumber);
                            _context.Entry(existingstudent).CurrentValues.SetValues(objStudentStatusUpdate);
                            _context.SaveChanges();
                        }
                        else
                        {
                            Student student = new Student()
                            {
                                BloodGroup = BloodGroup.FirstOrDefault(x => x.DataListItemId == objstudentsRegistration.BloodGroup_Id)?.DataListItemName,
                                BloodGroup_Id = objstudentsRegistration.BloodGroup_Id,
                                //ApplicationNumber = studentViewModel.ApplicationNumber,
                                Category_Id = objstudentsRegistration.Category_Id,
                                Class_Id = Class_ID,
                                AdharNo=objstudentsRegistration.AdharNo,
                                Batch_Id = objstudentsRegistration.Batch_Id,
                                CurrentYear = objstudentsRegistration.Batch_Id,
                                Category = AllCategorys.FirstOrDefault(x => x.DataListItemId == objstudentsRegistration.Category_Id)?.DataListItemName,
                                Class = Classes.FirstOrDefault(x => x.DataListItemId == objstudentsRegistration.Class_Id)?.DataListItemName,
                                //Date = studentViewModel.Date,
                                DOB = objstudentsRegistration.DOB,
                                Gender = objstudentsRegistration.Gender,
                                MotherTongue = objstudentsRegistration.MotherTongue,
                                Name = objstudentsRegistration.Name,
                                Last_Name=objstudentsRegistration.Last_Name,
                                Caste = objstudentsRegistration.Cast_Id.ToString(),
                                Medium= objstudentsRegistration.Medium,
                                Nationality = objstudentsRegistration.Nationality,
                                POB = objstudentsRegistration.POB,
                                // ProfileAvatar = studentViewModel.ProfileAvatar,
                                Religion = objstudentsRegistration.Religion_Id.ToString(),
                                //UIN = studentViewModel.Student.UIN,
                                IsApplyforAdmission = true,
                                IsApprove = objstudentsRegistration.IsApprove,
                                Transport = objstudentsRegistration.Transport,
                                Transport_Options = objstudentsRegistration.Transport_Options,
                                Mobile = objstudentsRegistration.Mobile,
                                City = objstudentsRegistration.City,
                                State = objstudentsRegistration.State,
                                Pincode = objstudentsRegistration.Pincode,
                                ParentEmail = objstudentsRegistration.Parents_Email,
                                ProfileAvatar = objstudentsRegistration.ProfileAvatar,
                                ApplicationNumber = objstudentsRegistration.ApplicationNumber.ToString(),
                                UIN = Guid.NewGuid().ToString(),
                                IsActive = true,
                                Section_Id=objstudentsRegistration.Section_Id,
                                ScholarNo=objstudentsRegistration.ScholarNo,
                                Hobbies=objstudentsRegistration.Hobbies,
                                OtherLanguages=objstudentsRegistration.OtherLanguages
                            };
                            
                            var userdetails = _context.Students.Add(student);
                            await _context.SaveChangesAsync();
                            int newstudentId = student.StudentId;
                            var familyDetail = _context.FamilyDetails.FirstOrDefault(fd => fd.ApplicationNumber == objstudentsRegistration.ApplicationNumber);
                            if (familyDetail != null)
                            {
                                familyDetail.StudentRefId = newstudentId; // Assuming StudentRefId is the correct field
                                _context.Entry(familyDetail).State = EntityState.Modified;
                                _context.SaveChanges(); // Use await if you have SaveChangesAsync support
                            }
                            var Additional = _context.AdditionalInformations.FirstOrDefault(fd => fd.StudentRefId == studentid);
                            if (Additional != null)
                            {
                                Additional.StudentRefId = newstudentId; // Assuming StudentRefId is the correct field
                                _context.Entry(Additional).State = EntityState.Modified;
                                _context.SaveChanges(); // Use await if you have SaveChangesAsync support
                            }
                            var Guardian = _context.GuardianDetails.FirstOrDefault(fd => fd.StudentRefId == studentid);
                            if (Guardian != null)
                            {
                                Guardian.StudentRefId = newstudentId; // Assuming StudentRefId is the correct field
                                _context.Entry(Guardian).State = EntityState.Modified;
                                _context.SaveChanges(); // Use await if you have SaveChangesAsync support
                            }
                            var PastSchool = _context.PastSchoolingReports.FirstOrDefault(fd => fd.StudentRefId == studentid);
                            if (PastSchool != null)
                            {
                                PastSchool.StudentRefId = newstudentId; // Assuming StudentRefId is the correct field
                                _context.Entry(PastSchool).State = EntityState.Modified;
                                _context.SaveChanges(); // Use await if you have SaveChangesAsync support
                            }

                            //if (objStudentAdmissionModel.isapprove == "ADMITTED")
                            //{
                            //    var StudentDetails = _context.Students.FirstOrDefault(e => e.ApplicationNumber == objstudentsRegistration.ApplicationNumber);
                            //    if (StudentDetails != null)
                            //    {
                            //        StudentStatusUpdate objStudentStatusUpdate = new StudentStatusUpdate
                            //        {
                            //            IsActive = false
                            //        };
                            //        var existingstudent = _context.Students.FirstOrDefault(e => e.ApplicationNumber == objstudentsRegistration.ApplicationNumber);
                            //        _context.Entry(existingstudent).CurrentValues.SetValues(objStudentStatusUpdate);
                            //        _context.SaveChanges();
                            //    }
                            //}
                        }
                        #endregion
                        #region ENCRIPT
                        const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                        StringBuilder stringBuilder = new StringBuilder();
                        Random rnd = new Random();
                        int length = 8;
                        while (0 < length--)
                        {
                            stringBuilder.Append(valid[rnd.Next(valid.Length)]);
                        }
                        var password = stringBuilder.ToString();

                        #endregion

                        #region User Creation


                        Tbl_UserManagementViewModel objTbl_UserManagementViewModel = new Tbl_UserManagementViewModel();
                        objTbl_UserManagementViewModel.Description = "Student registration";
                        objTbl_UserManagementViewModel.Email = objstudentsRegistration.Parents_Email;
                        //objTbl_UserManagementViewModel.Password = ToReturn; 
                        objTbl_UserManagementViewModel.Password = password;
                        objTbl_UserManagementViewModel.UserName = objstudentsRegistration.Name + "1";
                        objTbl_UserManagementViewModel.UserRole = "Student";
                        objTbl_UserManagementViewModel.UserId = Convert.ToInt32(objstudentsRegistration.StudentRegisterID);
                        objTbl_UserManagementViewModel.ApplicationNumber = objstudentsRegistration.ApplicationNumber;

                        var studentname = (objstudentsRegistration.Name + objstudentsRegistration.Last_Name);
                        var stdtname = studentname.Replace(" ", string.Empty);

                        //objTbl_UserManagementViewModel.StudentName = studentname;


                        var schoolemail = _context.TblCreateSchool.FirstOrDefault();
                        if (schoolemail == null)
                        {
                            throw new Exception("Please enter school details!.");
                        }

                        objTbl_UserManagementViewModel.StudentName = stdtname;
                        var UserMngID = CreateUser(objTbl_UserManagementViewModel);

                        var existingobj1 = _context.StudentsRegistrations.FirstOrDefault(e => e.StudentRegisterID == objstudentsRegistration.StudentRegisterID);
                        Studentupdate studentobj = new Studentupdate()
                        {
                            UserId = UserMngID
                        };
                        _context.Entry(existingobj1).CurrentValues.SetValues(studentobj);
                        _context.SaveChanges();

                        
                        //var loginpagelink = "https://www.carmelteresaschool.in/";

                        var str = SendEmail("" + objstudentsRegistration.Parents_Email + "", "Application of " + objstudentsRegistration.Name + " has been moved to the " + objStudentAdmissionModel.AdmissionStatusID + " Status", "Your Application (" + objstudentsRegistration.ApplicationNumber + ") has been moved to " + objStudentAdmissionModel.AdmissionStatusID + " status successfully. Your Uername is " + objTbl_UserManagementViewModel.UserName + " and password is " + objTbl_UserManagementViewModel.Password + " You can login through this link "+ schoolemail.Website +" " + schoolemail.School_Name + " will review and let you know of next steps soon. Please contact school directly if there is a delay.");

                        if (str == "S")
                        {
                            EmailViewModel emailViewModel = new EmailViewModel();
                            emailViewModel.Student_id = Convert.ToInt32(objstudentsRegistration.StudentRegisterID);
                            emailViewModel.ApplicationNumber = objstudentsRegistration.ApplicationNumber;
                            emailViewModel.Name = objstudentsRegistration.Name + " " + objstudentsRegistration.Last_Name;
                            emailViewModel.Parent_Email = objstudentsRegistration.Parents_Email;
                            emailViewModel.Email_Date = DateTime.Now.ToString();
                            emailViewModel.Email_Content = objStudentAdmissionModel.AdmissionStatusID;

                            var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                        }

                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        #region Reset student status
                      
                        #endregion

                        #region Student Status modify
                        StudentAdmissionStatusUpdate objStudentAdmissionStatusUpdate = new StudentAdmissionStatusUpdate
                        {
                            IsApprove = objTbl_DataListItem.DataListItemId
                        };
                        var existingobj = _context.StudentsRegistrations.FirstOrDefault(e => e.StudentRegisterID == studentid);
                        _context.Entry(existingobj).CurrentValues.SetValues(objStudentAdmissionStatusUpdate);
                        _context.SaveChanges();

                      
                        #endregion
                        if (objStudentAdmissionModel.AdmissionStatusID == "APPROVED")
                        {
                            var schoolemail = _context.TblCreateSchool.FirstOrDefault();
                            if (schoolemail == null)
                            {
                                throw new Exception("Please enter school details!.");
                            }
                            //var feeplans = _context.FeePlans.FirstOrDefault(x => x.FeeType_Id == 243 && x.ClassId == Class_ID);
                            //var feevalue = "";
                            //if(feeplans != null)
                            //{
                            //    feevalue = feeplans.FeeValue.ToString();
                            //}
                            //need to change the link when publish into server
                            //var emaillink = "http://svd.orootsfoundations.org/Payment/AdmissionFeepage?studentId=" + objstudentsRegistration.StudentRegisterID + "";
                            // var emaillink = "https://www.carmelteresaschool.in/Payment/AdmissionFeepage?studentId=" + objstudentsRegistration.StudentRegisterID+"";
                            var emaillink = schoolemail.Website + "/Payment/AdmissionFeepage?studentId=" + objstudentsRegistration.StudentRegisterID + "";

                            var str = SendEmail("" + objstudentsRegistration.Parents_Email + "", "Application of " + objstudentsRegistration.Name + " has been moved to the " + objStudentAdmissionModel.AdmissionStatusID + " Status", "Your Application (" + objstudentsRegistration.ApplicationNumber + ") has been moved to " + objStudentAdmissionModel.AdmissionStatusID + " status successfully. Please pay your Admission Fee using this link " + emaillink + " " + schoolemail.School_Name + "   will review and let you know of next steps soon. Please contact school directly if there is a delay.");

                            if (str == "S")
                            {
                                EmailViewModel emailViewModel = new EmailViewModel();
                                emailViewModel.Student_id = Convert.ToInt32(objstudentsRegistration.StudentRegisterID);
                                emailViewModel.ApplicationNumber = objstudentsRegistration.ApplicationNumber;
                                emailViewModel.Name = objstudentsRegistration.Name + " " + objstudentsRegistration.Last_Name;
                                emailViewModel.Parent_Email = objstudentsRegistration.Parents_Email;
                                emailViewModel.Email_Date = DateTime.Now.ToString();
                                emailViewModel.Email_Content = objStudentAdmissionModel.AdmissionStatusID;

                                var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                            }

                        }
                        
                        else
                        {
                          var str =  SendEmail("" + objstudentsRegistration.Parents_Email + "", "Application of " + objstudentsRegistration.Name + " has been moved to the " + objStudentAdmissionModel.AdmissionStatusID + " Status", "Your Application (" + objstudentsRegistration.ApplicationNumber + ") has been moved to " + objStudentAdmissionModel.AdmissionStatusID + " status successfully. Carmel Teresa will review and let you know of next steps soon. Please contact school directly if there is a delay.");

                            if(str == "S")
                            {
                                EmailViewModel emailViewModel = new EmailViewModel();
                                emailViewModel.Student_id = Convert.ToInt32(objstudentsRegistration.StudentRegisterID);
                                emailViewModel.ApplicationNumber = objstudentsRegistration.ApplicationNumber;
                                emailViewModel.Name = objstudentsRegistration.Name + " " + objstudentsRegistration.Last_Name;
                                emailViewModel.Parent_Email = objstudentsRegistration.Parents_Email;
                                emailViewModel.Email_Date = DateTime.Now.ToString();
                                emailViewModel.Email_Content = objStudentAdmissionModel.AdmissionStatusID;

                                var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                            }
                        }

                        // SendEmail("jai35ram@gmail.com", "Admission", "Status");
                        #endregion
                        return Json("Success", JsonRequestBehavior.AllowGet);
                    }
               
                }

            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbex)
            {
                Exception raise = dbex;
                foreach(var error in dbex.EntityValidationErrors)
                {
                    foreach(var validationerror in error.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                           error.Entry.Entity.ToString(),
                           validationerror.ErrorMessage);
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;

            }
        }



        //Create userlogin function
        public string CreateUserlogin(int studentid)
        {

            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentid);

            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder stringBuilder = new StringBuilder();
            Random rnd = new Random();
            int length = 8;
            while (0 < length--)
            {
                stringBuilder.Append(valid[rnd.Next(valid.Length)]);
            }
            var password = stringBuilder.ToString();


            Tbl_UserManagementViewModel objTbl_UserManagementViewModel = new Tbl_UserManagementViewModel();
            objTbl_UserManagementViewModel.Description = "Student registration";
            objTbl_UserManagementViewModel.Email = studentdata.Parents_Email;
            //objTbl_UserManagementViewModel.Password = ToReturn; 
            objTbl_UserManagementViewModel.Password = password;
            objTbl_UserManagementViewModel.UserName = studentdata.Name + "1";
            objTbl_UserManagementViewModel.UserRole = "Student";
            objTbl_UserManagementViewModel.UserId = Convert.ToInt32(studentdata.StudentRegisterID);
            objTbl_UserManagementViewModel.ApplicationNumber = studentdata.ApplicationNumber;

            var studentname = (studentdata.Name + studentdata.Last_Name);
            var stdtname = studentname.Replace(" ", string.Empty);

            //objTbl_UserManagementViewModel.StudentName = studentname;



            objTbl_UserManagementViewModel.StudentName = stdtname;
            var UserMngID = CreateUser(objTbl_UserManagementViewModel);

            var userid = UserMngID;


            studentdata.UserId = userid;
            studentdata.IsApprove = 217;
            _context.SaveChanges();


            return userid;

        }


        public string CreateUser(Tbl_UserManagementViewModel tbl_UserManagementViewModel)
        {
            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement
            {
                Description = tbl_UserManagementViewModel.Description,
                Email = tbl_UserManagementViewModel.Email,
                Password = tbl_UserManagementViewModel.Password,
                //UserName = tbl_UserManagementViewModel.ApplicationNumber
                UserName = tbl_UserManagementViewModel.StudentName
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
                return Content("<script language='javascript' type='text/javascript'>alert('Data Delete Successfully');location.replace('/StudentAdmission/StudentAdmission')</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult StudentRegMaster()
        {

            ViewBag.Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var classlist = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            List<StudentRegMasterModel> studentls = new List<StudentRegMasterModel>();
            var result = _context.StudentRegNumberMaster.OrderByDescending(x => x.CreatedOn);
            foreach (var res in result)
            {
                StudentRegMasterModel studentobj = new StudentRegMasterModel();

                //studentobj.BatchName = res.BatchName.ToString();
                studentobj.Class_Id = res.Class_Id;
                studentobj.Class = classlist.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.RegPrefix = res.RegPrefix;
                studentobj.RegLength = res.RegLength;
                studentobj.RegNumberStartWith = res.RegNumberStartWith;
                studentls.Add(studentobj);
            }
            ViewBag.StudentReg = studentls;
            return View();
        }

        [HttpPost]
        public JsonResult ConfigureStudentRegMaster(StudentRegMasterModel objStudentRegMasterModel)
        {


            StudentRegNumberMaster objStudentRegNumberMaster = new StudentRegNumberMaster
            {
                //BatchName = objStudentRegMasterModel.BatchName,
                Class_Id =Convert.ToInt32( objStudentRegMasterModel.Class),
                //Class = objStudentRegMasterModel.Class,
                RegPrefix = objStudentRegMasterModel.RegPrefix,
                RegLength = objStudentRegMasterModel.RegLength,
                RegNumberStartWith = objStudentRegMasterModel.RegNumberStartWith,
                RegLastNumber = objStudentRegMasterModel.RegNumberStartWith,
                CreatedOn = DateTime.Today,
            };

            _StudentRegNumberMaster.Insert(objStudentRegNumberMaster);
            _StudentRegNumberMaster.Save();
            return Json("Ok");
        }

        public ActionResult StudentRegistrationGeneration()
        {

            ViewBag.Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var classlist = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            List<StudentRegMasterModel> studentls = new List<StudentRegMasterModel>();
            var result = _context.StudentRegNumberMaster.OrderByDescending(x => x.CreatedOn);
            foreach (var res in result)
            {
                StudentRegMasterModel studentobj = new StudentRegMasterModel();

                //studentobj.BatchName = res.BatchName.ToString();
                studentobj.Class_Id = res.Class_Id;
                studentobj.Class = classlist.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.RegPrefix = res.RegPrefix;
                studentobj.RegLength = res.RegLength;
                studentobj.RegNumberStartWith = res.RegNumberStartWith;
                string StartNumber = string.Empty;
                StartNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), res.RegNumberStartWith.ToString());
                string lastNumber = string.Empty;
                lastNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), res.RegLastNumber.ToString());
                studentobj.LastRegNumber = lastNumber;
                string Status = string.Empty;
                if (res.RegStatus != null && res.RegStatus != "")
                {
                    if (res.RegStatus == "N")
                    {
                        Status = "Not Generated";
                    }
                    else if (res.RegStatus == "Y")
                    {
                        Status = "Generated";
                    }

                }
                else
                {
                    Status = "Not Generated";
                }
                studentobj.RegStatus = Status;
                studentobj.StartRegNumber = StartNumber;
                studentls.Add(studentobj);
            }
            ViewBag.StudentReg = studentls;
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> StudentRegistrationGeneration(StudentRegMasterModel objStudentRegMasterModel)
        {
            var result = await _context.StudentRegNumberMaster.Where(x => x.Class_Id == objStudentRegMasterModel.Class_Id).ToListAsync(); //OrderByDescending(x => x.StudentRegisterID)
            var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            List<StudentRegMasterModel> studentls = new List<StudentRegMasterModel>();
            foreach (var res in result)
            {
                StudentRegMasterModel studentobj = new StudentRegMasterModel();

                //studentobj.BatchName = res.BatchName.ToString();
                studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.RegPrefix = res.RegPrefix;
                studentobj.RegLength = res.RegLength;
                studentobj.RegNumberStartWith = res.RegNumberStartWith;
                string Status = string.Empty;
                if (res.RegStatus != null && res.RegStatus != "")
                {
                    if (res.RegStatus == "N")
                    {
                        Status = "Not Generated";
                    }
                    else if (res.RegStatus == "Y")
                    {
                        Status = "Generated";
                    }

                }
                else
                {
                    Status = "Not Generated";
                }
                studentobj.RegStatus = Status;
                string StartNumber = string.Empty;
                StartNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), res.RegNumberStartWith.ToString());
                string lastNumber = string.Empty;
                lastNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), res.RegLastNumber.ToString());
                studentobj.LastRegNumber = lastNumber;
                studentobj.StartRegNumber = StartNumber;
                studentls.Add(studentobj);
            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }
        public string GetStartNumber(string perfix, string length, string startwith)
        {
            string startNumber = string.Empty;
            int len = Convert.ToInt32(length);
            int startlen = startwith.Length;
            string append = string.Empty;
            for (int i = startlen; i < len; i++)
            {
                append += "0";
            }
            startNumber = perfix + append + startwith;
            return startNumber;
        }

        [HttpPost]
        public async Task<JsonResult> StudentRegistrationProcess(StudentRegMasterModel objStudentRegMasterModel)
        {
            try
            {
                var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var classid = Classes.FirstOrDefault(x => x.DataListItemName == objStudentRegMasterModel.Class)?.DataListItemId;
                var result = await _context.StudentRegNumberMaster.Where(x => x.Class_Id == classid).ToListAsync(); //OrderByDescending(x => x.StudentRegisterID)
                List<StudentRegMasterModel> studentls = new List<StudentRegMasterModel>();
                foreach (var res in result)
                {
                    StudentRegMasterModel studentobj = new StudentRegMasterModel();

                    //studentobj.BatchName = res.BatchName.ToString();
                    studentobj.Class = Classes.FirstOrDefault(x => x.DataListItemId == classid)?.DataListItemName;
                    studentobj.RegPrefix = res.RegPrefix;
                    studentobj.RegLength = res.RegLength;
                    studentobj.RegNumberStartWith = res.RegNumberStartWith;
                    string StartNumber = string.Empty;
                    StartNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), res.RegNumberStartWith.ToString());
                    studentobj.StartRegNumber = StartNumber;
                    studentls.Add(studentobj);

                    //var studentlist = _context.Students.Where(x=> x.Class_Id == classid).OrderBy(x => x.Name);
                    var studentlist = _context.Students.Where(x => x.Class_Id == classid).OrderBy(x => x.Name).ToList();
                    int startRegNumber = res.RegLastNumber;
                    foreach (var stud in studentlist)
                    {
                        int studentid = stud.StudentId;
                        StartNumber = GetStartNumber(res.RegPrefix, res.RegLength.ToString(), startRegNumber.ToString());

                        studentupdate objstudentupdate = new studentupdate
                        {
                            RegNumber = StartNumber,
                        };
                        // var existingobj = _context.Students.FirstOrDefault(e => e.StudentId == studentid);
                        //_context.Entry(existingobj).CurrentValues.SetValues(objstudentupdate);
                        //_context.SaveChanges();

                        var StudentDetails = _contextstudent.Students.FirstOrDefault(e => e.StudentId == studentid);
                        if (StudentDetails != null)
                        {
                            var existingstudent = _contextstudent.Students.FirstOrDefault(e => e.StudentId == studentid);
                            _contextstudent.Entry(existingstudent).CurrentValues.SetValues(objstudentupdate);
                            _contextstudent.SaveChanges();
                        }

                        startRegNumber++;
                    }

                    studentRegNumberUpdate objstudentRegNumberUpdate = new studentRegNumberUpdate
                    {
                        RegLastNumber = startRegNumber,
                        RegStatus = "Y",
                    };
                    var Regobj = _context.StudentRegNumberMaster.FirstOrDefault(e => e.Class_Id == classid);
                    _context.Entry(Regobj).CurrentValues.SetValues(objstudentRegNumberUpdate);
                    _context.SaveChanges();

                }
            }
            catch (Exception ex)
            {

            }
            return Json("Success", JsonRequestBehavior.AllowGet);
        }

        public string SendEmail(string Tomailid, string subject, string bodymessage)
        {
            try
            {

                //string sourceMailID = ConfigurationManager.AppSettings["SourceMailID"];
                //string sourceMailPassword = ConfigurationManager.AppSettings["SourceMailPassword"];
                //string smtpsHost = ConfigurationManager.AppSettings["smtpsHost"];
                //int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpsPort"]);
                //bool enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                //bool useDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(sourceMailID);
                //    mail.To.Add(Tomailid);
                //    mail.To.Add(sourceMailID);
                //    mail.Subject = subject;
                //    mail.Body = bodymessage;
                //    mail.IsBodyHtml = true;
                //    mail.ReplyToList.Add(Tomailid);
                //    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                //    using (SmtpClient smtp1 = new SmtpClient(smtpsHost.Trim(), port))
                //    {
                //        smtp1.EnableSsl = enableSSL;
                //        smtp1.UseDefaultCredentials = useDefaultCredentials;
                //        //smtp1.Timeout = 20000;
                //        smtp1.Credentials = new NetworkCredential(sourceMailID.Trim(), sourceMailPassword.Trim());
                //        smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        smtp1.Send(mail);
                //    }
                //}

                var schoolemail = _context.TblCreateSchool.FirstOrDefault();
                if (schoolemail == null)
                {
                    throw new Exception("Please enter school details!.");
                }
                var sourceMailID = new MailAddress(schoolemail.Email);
                string sourceMailPassword = schoolemail.Password;
                if (sourceMailID == null || sourceMailPassword == null)
                {
                    throw new Exception("Please enter the Email details!.");
                }




                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sourceMailID.Address, sourceMailPassword)
                };

                var message = new MailMessage(sourceMailID.Address, Tomailid)
                {
                    Subject = subject,
                    Body = bodymessage,
                };
               
                
                using (message)
                {
                    smtp.Send(message);
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

       

        public string SendSMS(string MobileNo, string Message)
        {
            string Return = string.Empty;
            try
            {

                string rdvalue1 = Message;
                string Smsuserid = ConfigurationManager.AppSettings["SmsUserid"];
                string SmsPassword = ConfigurationManager.AppSettings["SmsPassword"];
                string SmsSenderID = ConfigurationManager.AppSettings["SmsSenderid"];
                string mobno = MobileNo;
                try
                {
                    string YOUR_URL = string.Empty;
                    string response = new System.Net.WebClient().DownloadString(YOUR_URL);
                }
                catch (Exception ex)
                {

                }
                return "S";
            }
            catch (Exception ex)
            {

            }
            return "F";
        }


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

    }
}