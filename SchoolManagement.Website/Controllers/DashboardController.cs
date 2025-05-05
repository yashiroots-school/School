using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Migrations;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;

namespace SchoolManagement.Website.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Dashboard
       public ActionResult Dashboard()
       {
            if (Session["rolename"] == null || Session["ScolarNo"] == null || Session["StudentId"] == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if session variables are missing
            }
            if (Session["rolename"].ToString() == "Student")
            {
                string applicationNo = Session["ScolarNo"].ToString();
                string studentRegId = Session["StudentId"].ToString();

                // Validate application number and student ID
                if (!string.IsNullOrEmpty(applicationNo) && !string.IsNullOrEmpty(studentRegId))
                {
                    int studentId;
                    if (int.TryParse(studentRegId, out studentId))
                    {
                        // Fetch student registration data
                        var studentData = _context.StudentsRegistrations
                            .FirstOrDefault(x => x.StudentRegisterID == studentId && x.ApplicationNumber == applicationNo);

                        if (studentData != null)
                        {
                            // Fetch additional information if available
                            var additionalInfo = _context.AdditionalInformations
                                .FirstOrDefault(x => x.ApplicationNumber == applicationNo && x.DistancefromSchool > 0);

                            // Set ViewBag property if additional information exists
                            if (additionalInfo != null)
                            {
                                ViewBag.Additionalinfo = additionalInfo.TransportFacility;
                            }
                        }

                        return View();
                    }
                }

                // Redirect to login if application number or student ID is invalid
                return RedirectToAction("Login", "Account");
            }
            var CurrentBatch = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true && x.IsActiveForAdmission == true).ToList().FirstOrDefault();
            ViewBag.StudentCount = 0;
            var StCount = _context.Students.Where(x => x.IsApplyforTC == false).Where(x => x.IsApprove == 217).ToList().Count();
            ViewBag.StudentCount = StCount;
            ViewBag.TeacherCount = 0;
            var TeacherCount = _context.StafsDetails.Where(x => x.IsDeleted == false).ToList().Count();
            ViewBag.TeacherCount = TeacherCount;
            ViewBag.TcCount = 0;
            ViewBag.NewAdmissionCount = 0;
            ViewBag.StudentNames = null;
            ViewBag.Classes = null;
            ViewBag.Section = null;
            ViewBag.TeacherDetails = null;
            if (CurrentBatch != null)
            {
                var TcCount = _context.Tbl_StudentTcDetails.Where(x => x.BatchId == CurrentBatch.Batch_Id).ToList().Count();
                ViewBag.TcCount = TcCount;
               
                var NewAdmission = _context.Students.Where(x => x.IsApplyforTC == false && x.CurrentYear == CurrentBatch.Batch_Id).ToList().Count();
                ViewBag.NewAdmissionCount = NewAdmission;
                var studentlist = (from a in _context.Students
                                   join fp in _context.FeePlans on new { a = a.Class_Id, a.Medium } equals new { a = fp.ClassId, fp.Medium }
                                   where a.IsApprove == 217
                                   select a).DistinctBy(a => a.StudentId).ToList();

                var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

                studentlist.ForEach(fe =>
                {

                    var className = Classes.Where(w => w.DataListItemId == fe.Class_Id)
                    .Select(s => s.DataListItemName).FirstOrDefault();
                    var SectionName = Section.Where(w => w.DataListItemId == fe.Section_Id)
                   .Select(s => s.DataListItemName).FirstOrDefault();

                    fe.Name = (fe.Name ?? string.Empty) + " "
                                  + (fe.Last_Name ?? string.Empty) + "-"
                                  + (className ?? string.Empty) + "-"
                                  + (SectionName ?? string.Empty);
                });
                ViewBag.StudentNames = studentlist;
                ViewBag.Classes = Classes;
                ViewBag.Section = Section;
                ViewBag.TeacherDetails = _context.StafsDetails.Where(x => x.IsDeleted == false).OrderBy(x => x.Name).ToList();
            }
            else
            { 
            
            }
            return View();

            //if (Session["rolename"].ToString() == "Student")
            //{
            //    var Applicationno = Session["ScolarNo"].ToString();
            //    var studentregid = Session["StudentId"];
            //    if (Applicationno != "")
            //    {
            //        int studentid = Convert.ToInt32(studentregid);
            //        var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentid && x.ApplicationNumber == Applicationno);



            //        var additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.StudentRefId == studentid && x.DistancefromSchool > 0);
            //        if (additionalinfo != null && data != null)
            //        {

            //            ViewBag.Additionalinfo = additionalinfo.TransportFacility;
            //        }

            //        return View();
            //    }
            //    else
            //    {
            //        return Content("<script language='javascript' type='text/javascript'>location.replace('/Account/Login')</script>");
            //    }
            //}
            //else
            //{
            //    return View();
            //}

        }
        public JsonResult GetStudentDetails(string Class,string Section)
        {

            //var studentsQuery = _context.Students
            //                .GroupJoin(_context.Tbl_StudentAttendance,
            //                           student => student.StudentId,
            //                           attendance => attendance.StudentRegisterID,
            //                           (student, attendanceGroup) => new { student, attendanceGroup })
            //                .Where(x => x.student.IsActive)  // Ensure the student is active
            //                .AsQueryable();
            var studentsQuery=_context.Students.ToList().Where(x=>x.IsActive==true);

            if (!string.IsNullOrEmpty(Class)&& Class!="0")
            {
                studentsQuery = studentsQuery.Where(x => x.Class_Id==Convert.ToInt32(Class));
            }

            // Apply filtering by Section if provided
            if (!string.IsNullOrEmpty(Section) && Section != "0")
            {
                studentsQuery = studentsQuery.Where(x => x.Section_Id == Convert.ToInt32(Section));
            }
            var students = studentsQuery
                 .AsEnumerable()  // Make sure we load the data in memory before projecting
                 .Select(x => new
                 {
                    x.StudentId,
                     x.Name,
                     Last_Name = x.Last_Name,
                     Class = x.Class,  // Assuming class name is in 'Name' field
                     Section = x.Section??"",
                     //City = x.attendanceGroup.Count().ToString() ?? "0" // Null check for profile avatar
                 })
                 .ToList();
            if (students.Count > 0)
                return Json(students, JsonRequestBehavior.AllowGet);

            return Json("Fail", JsonRequestBehavior.AllowGet);
        }

    }
}