using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Migrations;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using Tbl_DataListItem = SchoolManagement.Data.Models.Tbl_DataListItem;

namespace SchoolManagement.Website.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Dashboard
        #region old
        // public ActionResult Dashboard()
        //{
        //     if (Session["rolename"] == null || Session["ScolarNo"] == null || Session["StudentId"] == null)
        //     {
        //         return RedirectToAction("Login", "Account"); // Redirect to login if session variables are missing
        //     }
        //     if (Session["rolename"].ToString() == "Student")
        //     {
        //         string applicationNo = Session["ScolarNo"].ToString();
        //         string studentRegId = Session["StudentId"].ToString();

        //         // Validate application number and student ID
        //         if (!string.IsNullOrEmpty(applicationNo) && !string.IsNullOrEmpty(studentRegId))
        //         {
        //             int studentId;
        //             if (int.TryParse(studentRegId, out studentId))
        //             {
        //                 // Fetch student registration data
        //                 var studentData = _context.StudentsRegistrations
        //                     .FirstOrDefault(x => x.StudentRegisterID == studentId && x.ApplicationNumber == applicationNo);

        //                 if (studentData != null)
        //                 {
        //                     // Fetch additional information if available
        //                     var additionalInfo = _context.AdditionalInformations
        //                         .FirstOrDefault(x => x.ApplicationNumber == applicationNo && x.DistancefromSchool > 0);

        //                     // Set ViewBag property if additional information exists
        //                     if (additionalInfo != null)
        //                     {
        //                         ViewBag.Additionalinfo = additionalInfo.TransportFacility;
        //                     }
        //                 }

        //                 return View();
        //             }
        //         }

        //         // Redirect to login if application number or student ID is invalid
        //         return RedirectToAction("Login", "Account");
        //     }
        //     var CurrentBatch = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true && x.IsActiveForAdmission == true).ToList().FirstOrDefault();
        //     ViewBag.StudentNames = null;
        //     ViewBag.Classes = null;
        //     ViewBag.Section = null;
        //     ViewBag.TeacherDetails = null;
        //     var classListId = _context.DataLists.Where(c => c.DataListName.ToLower() == "class").Select(c => c.DataListId.ToString()).FirstOrDefault();
        //     var sectionListId = _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "section").DataListId.ToString();
        //     if (Session["rolename"] != null && Session["rolename"].ToString()== "Administrator")
        //     {
        //         var noticesFromDb = _context.tbl_Notice
        //        .OrderByDescending(x => x.CurrentDate)
        //        .ToList();

        //         var notices = noticesFromDb
        //             .Select(x => new NoticeViewModel
        //             {
        //                 NoticeName = x.NoticeName,
        //                 NoticeDate = x.CurrentDate,
        //                 DaysAgo = (DateTime.Now - x.CurrentDate).Days
        //             })
        //             .ToList();
        //         ViewBag.Notice = notices;
        //         //var CurrentBatch = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true && x.IsActiveForAdmission == true).ToList().FirstOrDefault();
        //         ViewBag.StudentCount = 0;
        //         var StCount = _context.Students.Where(x => x.IsApplyforTC == false).Where(x => x.IsApprove == 217 && x.Batch_Id==CurrentBatch.Batch_Id).ToList().Count();
        //         ViewBag.StudentCount = StCount;
        //         ViewBag.TeacherCount = 0;
        //         var TeacherCount = _context.StafsDetails.Where(x => x.IsDeleted == false).ToList().Count();
        //         ViewBag.TeacherCount = TeacherCount;
        //         ViewBag.TcCount = 0;
        //         ViewBag.NewAdmissionCount = 0;

        //         var birthdayStaffs = _context.Database.SqlQuery<StaffBirthdayViewModel>("EXEC GetTodaysBirthdayStaffs1").ToList();

        //         ViewBag.BirthdayStaffs = birthdayStaffs;

        //         var birthdayStudents = _context.Database.SqlQuery<StudentsBirthdayViewModel>("EXEC GetTodaysBirthdayStudents1").ToList();

        //         ViewBag.birthdayStudents = birthdayStudents;

        //         if (CurrentBatch != null)
        //         {
        //             var TcCount = _context.Tbl_StudentTcDetails.Where(x => x.BatchId == CurrentBatch.Batch_Id).ToList().Count();
        //             ViewBag.TcCount = TcCount;

        //             var NewAdmission = _context.Students.Where(x => x.IsApplyforTC == false && x.CurrentYear == CurrentBatch.Batch_Id).ToList().Count();
        //             ViewBag.NewAdmissionCount = NewAdmission;
        //             var studentlist = (from a in _context.Students
        //                                join fp in _context.FeePlans on new { a = a.Class_Id, a.Medium } equals new { a = fp.ClassId, fp.Medium }
        //                                where a.IsApprove == 217
        //                                select a).DistinctBy(a => a.StudentId).ToList();

        //             var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
        //             var Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

        //             studentlist.ForEach(fe =>
        //             {

        //                 var className = Classes.Where(w => w.DataListItemId == fe.Class_Id)
        //                 .Select(s => s.DataListItemName).FirstOrDefault();
        //                 var SectionName = Section.Where(w => w.DataListItemId == fe.Section_Id)
        //                .Select(s => s.DataListItemName).FirstOrDefault();

        //                 fe.Name = (fe.Name ?? string.Empty) + " "
        //                               + (fe.Last_Name ?? string.Empty) + "-"
        //                               + (className ?? string.Empty) + "-"
        //                               + (SectionName ?? string.Empty);
        //             });
        //             ViewBag.StudentNames = studentlist;
        //             ViewBag.Classes = Classes;
        //             ViewBag.Section = Section;
        //             ViewBag.TeacherDetails = _context.StafsDetails.Where(x=>x.IsActive==true).OrderBy(x => x.Name).ToList();
        //         }
        //         else
        //         {

        //         }
        //     }
        //     if (Session["rolename"] != null && Session["rolename"].ToString() == "Staff")
        //     {
        //         var noticesFromDb = _context.tbl_Notice
        //                        .OrderByDescending(x => x.CurrentDate)
        //                        .ToList();

        //         var notices = noticesFromDb
        //             .Select(x => new NoticeViewModel
        //             {
        //                 NoticeName = x.NoticeName,
        //                 NoticeDate = x.CurrentDate,
        //                 DaysAgo = (DateTime.Now - x.CurrentDate).Days
        //             })
        //             .ToList();
        //         int StaffID = Convert.ToInt32(Session["StaffID"]);
        //         var result = _context.Subjects.Where(s => s.Batch_Id == CurrentBatch.Batch_Id && s.Class_Teacher == true && s.StaffId == StaffID).Select(s => new { s.Class_Id, s.Section_Id }).Distinct().ToList();
        //         var classIds = result.Select(r => r.Class_Id).ToList();
        //         var sectionIds = result.Select(r => r.Section_Id).ToList();

        //         var classes = _context.DataListItems.Where(x => x.DataListId == classListId && classIds.Contains(x.DataListItemId)).ToList();
        //         var sections = _context.DataListItems.Where(x => x.DataListId == sectionListId && sectionIds.Contains(x.DataListItemId)).ToList();
        //         ViewBag.Classes = classes;
        //         ViewBag.Section = sections;
        //         ViewBag.TeacherDetails = _context.StafsDetails.Where(x => x.IsDeleted == false && x.IsActive == true && x.StafId==StaffID).OrderBy(x => x.Name).ToList();

        //     }
        //  return View();
        // }
        #endregion
        #region New
        public ActionResult Dashboard()
        {
            if (Session["rolename"] == null || Session["ScolarNo"] == null || Session["StudentId"] == null)
                return RedirectToAction("Login", "Account");

            var role = Session["rolename"].ToString();
            ViewBag.StudentNames = null;
            ViewBag.Classes = null;
            ViewBag.Section = null;
            ViewBag.TeacherDetails = null;
            ViewBag.Notice = null;
            ViewBag.BirthdayStaffs = null;
            ViewBag.BirthdayStudents = null;

            var currentBatch = _context.Tbl_Batches
                .FirstOrDefault(x => x.IsActiveForPayments && x.IsActiveForAdmission);

            if (role == "Student")
            {
                string appNo = Session["ScolarNo"].ToString();
                string studentIdStr = Session["StudentId"].ToString();

                if (int.TryParse(studentIdStr, out int studentId))
                {
                    var studentData = _context.StudentsRegistrations
                        .FirstOrDefault(x => x.StudentRegisterID == studentId && x.ApplicationNumber == appNo);

                    if (studentData != null)
                    {
                        var additionalInfo = _context.AdditionalInformations
                            .FirstOrDefault(x => x.ApplicationNumber == appNo && x.DistancefromSchool > 0);

                        if (additionalInfo != null)
                            ViewBag.AdditionalInfo = additionalInfo.TransportFacility;
                    }

                    return View();
                }

                return RedirectToAction("Login", "Account");
            }

            // StaffID if role is Staff
            int? staffID = role == "Staff" ? (int?)Convert.ToInt32(Session["StaffID"]) : null;

            // Common for Admin & Staff
            ViewBag.Notice = GetNotices();

            if (currentBatch != null)
            {
                var counts = GetDashboardCounts(currentBatch.Batch_Id);
                ViewBag.StudentCount = counts.StudentCount;
                ViewBag.TeacherCount = counts.StaffCount;
                ViewBag.TcCount = counts.TcCount;
                ViewBag.NewAdmissionCount = counts.NewAdmissionCount;

                ViewBag.TeacherDetails = _context.StafsDetails
                    .Where(x => x.IsActive==true && (!staffID.HasValue || x.StafId == staffID))
                    .OrderBy(x => x.Name)
                    .ToList();

                var classSection = GetClassesAndSections(currentBatch.Batch_Id, staffID);
                ViewBag.Classes = classSection.Classes;
                ViewBag.Section = classSection.Sections;

                //if (role == "Administrator")
                //{
                //    ViewBag.BirthdayStaffs = _context.Database.SqlQuery<StaffBirthdayViewModel>("EXEC GetTodaysBirthdayStaffs1").ToList();
                //    ViewBag.BirthdayStudents = _context.Database.SqlQuery<StudentsBirthdayViewModel>("EXEC GetTodaysBirthdayStudents1").ToList();
                //}
            }

            return View();
        }
        private DashboardCountsViewModel GetDashboardCounts(int batchId)
        {
            return new DashboardCountsViewModel
            {
                StudentCount = _context.Students.Count(x => x.IsApplyforTC == false && x.IsApprove == 217 && x.Batch_Id == batchId),
                StaffCount = _context.StafsDetails.Count(x => x.IsActive ==true),
                TcCount = _context.Tbl_StudentTcDetails.Count(x => x.BatchId == batchId),
                NewAdmissionCount = _context.Students.Count(x => x.IsApplyforTC == false && x.CurrentYear == batchId)
            };
        }
        private List<NoticeViewModel> GetNotices()
        {
            // Fetch data first
            var noticesFromDb = _context.tbl_Notice
                .OrderByDescending(x => x.CurrentDate)
                .ToList(); // fetch into memory

            // Calculate DaysAgo in memory
            var notices = noticesFromDb
                .Select(x => new NoticeViewModel
                {
                    NoticeName = x.NoticeName,
                    NoticeDate = x.CurrentDate,
                    DaysAgo = (DateTime.Now - x.CurrentDate).Days
                })
                .ToList();

            return notices;
        }
        private (List<Tbl_DataListItem> Classes, List<Tbl_DataListItem> Sections) GetClassesAndSections(int batchId, int? staffId = null)
        {
            // Fetch DataList IDs from DataLists table
            var classListId = _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class")?.DataListId.ToString();
            var sectionListId = _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "section")?.DataListId.ToString();

            List<int> classIds = null;
            List<int> sectionIds = null;

            if (staffId.HasValue)
            {
                var result = _context.Subjects
                    .Where(s => s.Batch_Id == batchId && s.Class_Teacher && s.StaffId == staffId.Value)
                    .Select(s => new { s.Class_Id, s.Section_Id })
                    .Distinct()
                    .ToList();

                classIds = result.Select(r => r.Class_Id).ToList();
                sectionIds = result.Select(r => r.Section_Id).ToList();
            }

            // Fetch all DataListItems into memory
            var allClasses = !string.IsNullOrEmpty(classListId)
                ? _context.DataListItems.Where(x => x.DataListId == classListId).ToList()
                : new List<Tbl_DataListItem>();

            var allSections = !string.IsNullOrEmpty(sectionListId)
                ? _context.DataListItems.Where(x => x.DataListId == sectionListId).ToList()
                : new List<Tbl_DataListItem>();

            // Filter in memory using DataListItemId (int)
            var classes = classIds != null
                ? allClasses.Where(x => classIds.Contains(x.DataListItemId)).ToList()
                : allClasses;

            var sections = sectionIds != null
                ? allSections.Where(x => sectionIds.Contains(x.DataListItemId)).ToList()
                : allSections;

            return (classes, sections);
        }
        #endregion
        public ActionResult AddCalendarEvent()
        {
            var events = _context.tbl_CalendarEvents.ToList();
            return View(events);
        }
        public ActionResult AddNotice()
        {
            var notices = _context.tbl_Notice.OrderByDescending(x => x.ID).ToList();
            return View(notices);
        }
        // POST: AddNotice via AJAX
        [HttpPost]
        public JsonResult AddNotice(string NoticeName, DateTime NoticeDate)
        {
            if (string.IsNullOrWhiteSpace(NoticeName))
                return Json(new { success = false, message = "Please enter event name." });

            var notice = new tbl_Notice
            {
                NoticeName = NoticeName,
                NoticeDate = NoticeDate,
                CurrentDate = DateTime.Now
            };

            _context.tbl_Notice.Add(notice);
            _context.SaveChanges();

            var newNotice = new
            {
                notice.ID,
                notice.NoticeName,
                NoticeDate = notice.NoticeDate.ToString("dd-MM-yyyy"),
                NoticeTime = notice.CurrentDate.ToString("hh:mm tt")
            };

            return Json(new { success = true, notice = newNotice });
        }
        // POST: Delete notice via AJAX
        [HttpPost]
        public JsonResult DeleteNotice(int id)
        {
            var notice = _context.tbl_Notice.FirstOrDefault(x => x.ID == id);
            if (notice != null)
            {
                _context.tbl_Notice.Remove(notice);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false, message = "Record not found" });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCalendarEvent(tbl_CalendarEvents model)
        {
            if (ModelState.IsValid)
            {
                _context.tbl_CalendarEvents.Add(model);
                _context.SaveChanges();
                return RedirectToAction("AddCalendarEvent");
            }

            var events = _context.tbl_CalendarEvents.ToList();
            return View(events);
        }
        public ActionResult DeleteEvent(int id)
        {
            var data = _context.tbl_CalendarEvents.Find(id);
            if (data != null)
            {
                _context.tbl_CalendarEvents.Remove(data);
                _context.SaveChanges();
            }
            return RedirectToAction("AddCalendarEvent");
        }
        public JsonResult GetEventsByDate(string date)
        {
            try
            {
                if (!DateTime.TryParse(date, out DateTime selectedDate))
                {
                    return Json(new { success = false, message = "Invalid date" }, JsonRequestBehavior.AllowGet);
                }

                // Pehle DB se saara data load karo memory me
                var eventsFromDb = _context.tbl_CalendarEvents
                    .OrderBy(x => x.EventTime)
                    .ToList()  // ✅ Load into memory
                    .Where(x => x.EventDate.Date == selectedDate.Date) // ✅ Compare in memory
                    .ToList();

                // Format TimeSpan
                var events = eventsFromDb.Select(x => new
                {
                    EventTime = TimeSpanToString(x.EventTime),
                    EventName = x.EventName
                }).ToList();

                return Json(events, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        private string TimeSpanToString(TimeSpan time)
        {
            DateTime dt = DateTime.Today.Add(time);
            return dt.ToString("hh:mm tt"); // Example: 07:45 PM
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

        public class DashboardCountsViewModel
        {
            public int StudentCount { get; set; }
            public int StaffCount { get; set; }
            public int TcCount { get; set; }
            public int NewAdmissionCount { get; set; }
        }
        public class DashboardViewModel
        {
            public List<NoticeViewModel> Notices { get; set; }
            public DashboardCountsViewModel Counts { get; set; }
            public List<Tbl_DataListItem> Classes { get; set; }
            public List<Tbl_DataListItem> Sections { get; set; }
            public List<Data.Models.StafsDetails> TeacherDetails { get; set; }
            public List<Data.Models.Student> StudentNames { get; set; }
            public List<StaffBirthdayViewModel> BirthdayStaffs { get; set; }
            public List<StudentsBirthdayViewModel> BirthdayStudents { get; set; }
            public string AdditionalInfo { get; set; } // For Student transport info
        }
    }
}