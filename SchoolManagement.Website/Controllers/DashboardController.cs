using System;
using System.Data.Entity;
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
        public JsonResult GetStudentDetails(string Class, string Section)
        {
            // Format the current date as a string to match the database's Created_Date format (dd/MM/yyyy).
            string today = DateTime.Now.ToString("dd/MM/yyyy");

            // Get the current active batch
            var currentBatch = _context.Tbl_Batches
                .FirstOrDefault(x => x.IsActiveForPayments && x.IsActiveForAdmission);

            if (currentBatch == null)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }

            // LINQ query to join Students, Class, Section, and today's Attendance
            var studentsQuery = from s in _context.Students

                                    // Left Join Class details
                                join cls in _context.DataListItems
                                    on s.Class_Id equals cls.DataListItemId
                                    into clsJoin
                                from cls in clsJoin.DefaultIfEmpty()

                                    // Left Join Section details
                                join sec in _context.DataListItems
                                    on s.Section_Id equals sec.DataListItemId
                                    into secJoin
                                from sec in secJoin.DefaultIfEmpty()

                                    // Left Join Student Attendance for today only.
                                    // FIX for CS1941: Explicitly cast the join keys (assuming they should be int)
                                    // to ensure type consistency in the composite key.
                                join sa in _context.Tbl_StudentAttendance
                                    .Where(a => a.Created_Date == today)
                                    on new { StudentId = (int)s.StudentId, BatchId = (int)s.Batch_Id }
                                    equals new { StudentId = (int)sa.StudentRegisterID, BatchId = (int)sa.BatchId }
                                    into saJoin
                                from sa in saJoin.DefaultIfEmpty()

                                where s.IsApplyforTC == false
                                    && s.IsApprove == 217
                                    && s.Batch_Id == currentBatch.Batch_Id
                                select new
                                {
                                    s.StudentId,
                                    s.Name,
                                    s.Last_Name,
                                    s.ApplicationNumber,
                                    s.Class_Id,
                                    s.Section_Id,
                                    s.Batch_Id,
                                    Class = cls != null ? cls.DataListItemName : "",
                                    Section = sec != null ? sec.DataListItemName : "",
                                    // Determine attendance status based on the joined record
                                    Attendance = sa != null
                                        ? sa.Mark_FullDayAbsent == "true" ? "Present"
                                        : sa.Mark_HalfDayAbsent == "True" ? "Half-Day"
                                        : sa.Others == "True" ? "Other"
                                        : "Absent" // Default for a present record that isn't explicitly marked
                                        : "-" // Default if no attendance record exists for today
                                };

            // Apply Class filter if provided
            if (!string.IsNullOrEmpty(Class) && Class != "0")
            {
                if (int.TryParse(Class, out int classId))
                {
                    studentsQuery = studentsQuery.Where(x => x.Class_Id == classId);
                }
            }

            // Apply Section filter if provided
            if (!string.IsNullOrEmpty(Section) && Section != "0")
            {
                if (int.TryParse(Section, out int sectionId))
                {
                    studentsQuery = studentsQuery.Where(x => x.Section_Id == sectionId);
                }
            }

            // Execute the query and materialize the results into a List.
            var students = studentsQuery.ToList();

            // FIX for CS0019: If 'students.Count' (property) is still failing as a 'method group', 
            // we use the 'students.Count()' (method) extension instead.
            if (students.Count() > 0)
            {
                return Json(students, JsonRequestBehavior.AllowGet);
            }

            return Json("Fail", JsonRequestBehavior.AllowGet);
        }


      

    }
}