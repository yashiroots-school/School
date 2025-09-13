using DataAccess.ViewModels;
using SchoolManagement.Data;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace SchoolManagement.Website.Controllers
{
    public class StafController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Staf
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddStaf()
        {
            var url = Request.Url.AbsoluteUri;
            //if (url.Contains("UpdateId"))
            //{
            //    ViewBag.IsEdit = true;
            //}
            //else
            //{
            //    ViewBag.IsEdit = false;
            //}
            ViewBag.EmpId = "EMIS/" + db.StafsDetails.Count();
            ViewBag.BloodGroup = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "BloodGroup").DataListId.ToString()).ToList();

            ViewBag.Medium = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Medium").DataListId.ToString()).ToList();
            ViewBag.Religion = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();
            ViewBag.Group = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Group").DataListId.ToString()).ToList();
     
            ViewBag.Caste = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Caste").DataListId.ToString()).ToList();
            ViewBag.Category = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Category").DataListId.ToString()).ToList();
            ViewBag.AllQualifications = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "qualification").DataListId.ToString()).ToList();
            ViewBag.StaffCategory = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "StaffCategory").DataListId.ToString()).ToList();
            ViewBag.AccountType = db.Tbl_AccountType.ToList();

            ViewBag.CategoryList = db.Tbl_StaffCategory.ToList();

            return View();
        }
        public List<StafsDetails> GetAllStaffs()
        {
            List<StafsDetails> staffList = new List<StafsDetails>();
            System.Data.Entity.DbSet<StafsDetails> staff = db.StafsDetails;
            foreach (StafsDetails item in staff)
            {
                staffList.Add(item);
            }
            return staffList;
        }

        [HttpPost]
        public ActionResult AddStaf(StafsDetails staffdetails, HttpPostedFileBase File, HttpPostedFileBase RelievingLetter, HttpPostedFileBase PerformanceLetter, HttpPostedFileBase AdharFile, HttpPostedFileBase PanFile,HttpPostedFileBase StaffSignatureFile) //
        {
           var url = Request.UrlReferrer.AbsoluteUri;
            string trackId = DateTime.Now.ToString("yyyyddMMhhmmss");
            try
            {
                if (File != null && File.ContentLength > 0)
                {
                    var extension = Path.GetExtension(File.FileName);
                    var fileName = "Profile_" + trackId + extension;
                    var directory = Server.MapPath("~/WebsiteImages/MemberImage");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    File.SaveAs(path);
                    staffdetails.File = fileName;
                }


                if (RelievingLetter != null && RelievingLetter.ContentLength > 0)
                {
                    var extension = Path.GetExtension(RelievingLetter.FileName);
                    var fileName = "Relieving_" + trackId + extension;
                    var directory = Server.MapPath("~/WebsiteImages/RelevingLetter");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    RelievingLetter.SaveAs(path);
                    staffdetails.RelievingLetter = fileName;
                }

                if (PerformanceLetter != null && PerformanceLetter.ContentLength > 0)
                {
                    var extension = Path.GetExtension(PerformanceLetter.FileName);
                    var fileName = "Performance_" + trackId + extension;
                    var directory = Server.MapPath("~/WebsiteImages/PerforLetter");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    PerformanceLetter.SaveAs(path);
                    staffdetails.PerformanceLetter = fileName;
                }

                if (AdharFile != null && AdharFile.ContentLength > 0)
                {
                    var extension = Path.GetExtension(AdharFile.FileName);
                    var fileName = "Aadhar_" + trackId + extension;
                    var directory = Server.MapPath("~/WebsiteImages/StaffAdhar");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    AdharFile.SaveAs(path);
                    staffdetails.AdharFile = fileName;
                }

                if (StaffSignatureFile != null && StaffSignatureFile.ContentLength > 0)
                {
                    var extension = Path.GetExtension(StaffSignatureFile.FileName);
                    var fileName = "Signature_" + trackId + extension;
                    var directory = Server.MapPath("~/WebsiteImages/Staffsignature");
                    if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                    var path = Path.Combine(directory, fileName);
                    StaffSignatureFile.SaveAs(path);
                    staffdetails.StaffSignatureFile = fileName;
                }
                if (StaffSignatureFile != null)
                {
                    if (StaffSignatureFile.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(StaffSignatureFile.FileName);
                        var path = Path.Combine(Server.MapPath("~/WebsiteImages/Staffsignature"), fileName);
                        StaffSignatureFile.SaveAs(path);
                        staffdetails.StaffSignatureFile = fileName;
                    }
                }
                //staffdetails.UserId = _context.StafsDetails.FirstOrDefault(x => x.UserId == currentUser.UserId.ToString()); //Session["UserId"].ToString();
                db.StafsDetails.Add(staffdetails);
                db.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Some error occure');location.replace('" + url + "')</script>");
            }
          
            return Content("<script language='javascript' type='text/javascript'>alert('Record Save Successfully!');location.replace('" + url + "')</script>");
        }

        /// <summary>
        /// Staff Detail by Staff Id
        /// </summary>
        /// <returns></returns>
        public ActionResult StafsDetails(int? StafId)
        {
            return View();
        }
    
        public StaffViewModel GetCompleteDetails(int StafId)
        {
            StaffViewModel StaffViewModel = new StaffViewModel();
            System.Data.Entity.DbSet<StafsDetails> data = db.StafsDetails;

            return null;
        }
        

        public ActionResult ManageStaff()
        {
            var allStaff = db.StafsDetails.ToList();
            
            ViewBag.totalEmp = db.StafsDetails.Count();
            //ViewBag.EmpId = new SelectList(db.StafsDetails.ToList().OrderBy(x => x.EmpId).ToList(), "StafId", "EmpId");
            ViewBag.EmpId = new SelectList(db.StafsDetails.ToList().Select(x => new { x.EmpId, x.StafId }).ToList());
            ViewBag.AllQualifications = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "qualification").DataListId.ToString()).ToList();
            return View();
        }
        public JsonResult GetAllPersionsEmpId()
        {
            var allRecords = db.StafsDetails.Select(x => new { x.EmpId, x.StafId })
                               .ToList();
            return Json(allRecords, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStaffDetailList()
        {
            //var allStaff = db.StafsDetails.ToList();
            var allStaff = from s in _context.StafsDetails
                           join d in _context.DataListItems
                               on s.StaffCategory equals d.DataListItemId into gj
                           from d in gj.DefaultIfEmpty()   // LEFT JOIN
                           select new
                           {
                               s,   // all columns from StafsDetails
                               StaffCategoryName = d != null ? d.DataListItemName : null
                           };
            return Json(allStaff, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetAllStaffDetails()
        {
            int allStaffs = 0;
            List<StafsDetails> studentList = new List<StafsDetails>();
            List<StafsDetails> allStaff = GetAllStaffs();
            if (allStaff != null)
            {
                studentList = allStaff;
                allStaffs = studentList.Count();

            }
            ViewBag.allStudents = allStaffs;
            return View(studentList);
        }

        public ActionResult GetAllStaffDetails1()
        {
            int allStaffs = 0;
            List<StafsDetails> studentList = new List<StafsDetails>();
            List<StafsDetails> allStaff = GetAllStaffs();
            if (allStaff != null)
            {
                studentList = allStaff;
                allStaffs = studentList.Count();

            }
            ViewBag.allStudents = allStaffs;
            return View(studentList);
        }

        public ActionResult ViewStaff()
        {
            var allStaff = db.StafsDetails.ToList();
            string imageName = "";
            if (allStaff.Count > 0)
            {
                imageName = allStaff[0].File;
                imageName = "/WebsiteImages/MemberImage/" + imageName;

            }
            ViewBag.ImageName = imageName;
           
            ViewBag.allStaff = allStaff;
            ViewBag.totalEmp = db.StafsDetails.Count();
           
            var allEmpId = db.StafsDetails.ToList();
            ViewBag.allEmpId = allEmpId;
            //ViewBag.EmpId = new SelectList(db.StafsDetails.ToList().OrderBy(x => x.EmpId.ToLower() == "EmpId").ToList());

            ViewBag.AllQualifications = db.DataListItems.Where(e => e.DataListId == db.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "qualification").DataListId.ToString()).ToList();
            return View();
        }

        public ActionResult FullStaffView(string id)
        {
            var allStafsDetails = db.StafsDetails.Where(x => x.StafId.ToString() == id).ToList();
            string imageName = "";
            if (allStafsDetails.Count > 0)
            {
                imageName = allStafsDetails[0].File;
                imageName = "/WebsiteImages/MemberImage/" + imageName;

            }
            ViewBag.ImageName = imageName;
            StafsDetails data = db.StafsDetails.FirstOrDefault(x => x.StafId.ToString() == id);
            ViewBag.allData = db.StafsDetails.Where(x => x.StafId == data.StafId).ToList();
            return View(data);
        }

        [HttpPost]
        public ActionResult EditStaffDetail(StafsDetails staffdetails, HttpPostedFileBase File, HttpPostedFileBase RelievingLetter, HttpPostedFileBase PerformanceLetter, HttpPostedFileBase AdharFile, HttpPostedFileBase PanFile, HttpPostedFileBase StaffSignatureFile)
        {
            if (staffdetails.StafId>0)
            {
         
                var existingobj = db.StafsDetails.FirstOrDefault(e => e.StafId == staffdetails.StafId);
                if (existingobj != null)
                {
                    if (File != null)
                    {
                        if (File.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(File.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/MemberImage"), fileName);
                            File.SaveAs(path);
                            staffdetails.File = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.File = existingobj.File;
                    }

                    if (RelievingLetter != null)
                    {
                        if (RelievingLetter.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(RelievingLetter.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/RelevingLetter"), fileName);
                            RelievingLetter.SaveAs(path);
                            staffdetails.RelievingLetter = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.RelievingLetter = existingobj.RelievingLetter;
                    }

                    if (PerformanceLetter != null)
                    {
                        if (PerformanceLetter.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(PerformanceLetter.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/PerforLetter"), fileName);
                            PerformanceLetter.SaveAs(path);
                            staffdetails.PerformanceLetter = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.PerformanceLetter = existingobj.PerformanceLetter;
                    }
                    if (AdharFile != null)
                    {
                        if (AdharFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(AdharFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StaffAdhar"), fileName);
                            AdharFile.SaveAs(path);
                            staffdetails.AdharFile = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.AdharFile = existingobj.AdharFile;
                    }

                    if (PanFile != null)
                    {
                        if (PanFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(PanFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/StaffPanDoc"), fileName);
                            PanFile.SaveAs(path);
                            staffdetails.PanFile = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.PanFile = existingobj.PanFile;
                    }

                    if (StaffSignatureFile != null)
                    {
                        if (StaffSignatureFile.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(StaffSignatureFile.FileName);
                            var path = Path.Combine(Server.MapPath("~/WebsiteImages/Staffsignature"), fileName);
                            StaffSignatureFile.SaveAs(path);
                            staffdetails.StaffSignatureFile = fileName;
                        }
                    }
                    else
                    {
                        staffdetails.StaffSignatureFile = existingobj.StaffSignatureFile;
                    }
                    staffdetails.UserId = existingobj.UserId;
                    db.Entry(existingobj).CurrentValues.SetValues(staffdetails);
                    db.SaveChangesAsync();
                }            
            }
            
            //return RedirectToAction("ManageStaff");
            return Content("<script language='javascript' type='text/javascript'>alert('Record Updated Successfully');location.replace('/Staf/ManageStaff')</script>");
        }

        [HttpGet]
        public JsonResult DeleteStaff(int id)
        {
            var staff = db.StafsDetails.FirstOrDefault(x => x.StafId == id);
            if (staff != null)
            {
                db.StafsDetails.Remove(staff);
                db.SaveChanges();
            }
            return Json("Staff Delete Sccessfully", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetstafById(int StafId)
        {
            try
            {
                var gid = db.StafsDetails.FirstOrDefault(e => e.StafId == StafId);
                if (gid != null)
                {
                    return Json(gid, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }
        public JsonResult GetStaffList1(string empid)
        {
            var result = new List<StafsDetails>();
            result = db.StafsDetails.ToList();
            if (!string.IsNullOrEmpty(empid))
            {
                result = result.Where(x => x.EmpId == empid).ToList();
            }
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult StaffAttendance()
        {
            ViewBag.classlist = _context.Tbl_Class.ToList();
            ViewBag.sectionlist = _context.Tbl_SectionSetup.ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Staffdetails = _context.StafsDetails.ToList();
            return View();
        }

        public JsonResult StaffAttendanceList()
        {
            var data = _context.StafsDetails.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddStaffAttendence(StaffAttendenceViewmodel staffAttendenceViewmodel)
        {
            try
            {
                var staffid = string.Join(",", staffAttendenceViewmodel.Staff_Id);
                string Staff_Name = string.Join(",", staffAttendenceViewmodel.Staff_Name);
                string fulldayabsent = string.Join(",", staffAttendenceViewmodel.FullDay_Absent);
                string halfdayabsent = string.Join(",", staffAttendenceViewmodel.HalfDay_Absent);
                Tbl_StaffAttendance tbl_StaffAttendance = new Tbl_StaffAttendance();

                foreach (string item in staffAttendenceViewmodel.Staff_Name)
                {
                    tbl_StaffAttendance.Staff_Id = Convert.ToInt32(staffAttendenceViewmodel.Staff_Id);
                    tbl_StaffAttendance.Staff_Name = staffAttendenceViewmodel.Staff_Name.ToString();
                    tbl_StaffAttendance.Mark_FullDayAbsent = staffAttendenceViewmodel.FullDay_Absent.ToString();
                    tbl_StaffAttendance.Mark_HalfDayAbsent = staffAttendenceViewmodel.HalfDay_Absent.ToString();
                    _context.Tbl_StaffAttendance.Add(tbl_StaffAttendance);
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                ex.ToString();
            }
            return Json("");

        }

        public ActionResult ViewStaffAttendence()
        {
            var StaffAttendence = _context.Tbl_StaffAttendance.Where(x => x.Attendence_Month == "3").ToList();
            ViewBag.StaffAttendence = StaffAttendence;

            ViewBag.Staff_List = _context.StafsDetails.ToList();

            var staffdetails = _context.StafsDetails.ToList();
            ViewBag.staffdetails = _context.StafsDetails.ToList();
            ViewBag.TotalCount = _context.StafsDetails.Count();
            string str = DateTime.Now.ToString("dd/MM/yyyy");
            DateTime dateTime = DateTime.ParseExact(str, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            int year = dateTime.Year;
            int month = dateTime.Month;
            var nofdays = DateTime.DaysInMonth(year, month);
            ViewBag.Daycount = nofdays;
            //ViewBag.Daycount = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id);
            List<viewstaffattendanceviewmodel> viewstaffattendanceviewmodel = new List<viewstaffattendanceviewmodel>();
            foreach (var item in staffdetails)
            {
                int staffatend = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id == item.StafId && x.Mark_FullDayPresent == "p").Count();
                float Noofworkingdays = staffatend * 100;
                float Totalattendence = Noofworkingdays / 30;
                if(Totalattendence != 0)
                {
                    viewstaffattendanceviewmodel.Add(new viewstaffattendanceviewmodel
                    {
                        Staff_Id = item.StafId,
                        Staff_Name = item.Name,
                        Total = Totalattendence
                    });
                }
               

            }
            ViewBag.data = viewstaffattendanceviewmodel;

            List<Tbl_StaffAttendance> TblStaffAttendence = new List<Tbl_StaffAttendance>();


            




            return View();
        }


        //Edit Attendance

        //Edit Staff Attendance
        public ActionResult EditStaffAttendance()
        {

            var stafflist = _context.StafsDetails.ToList();
            ViewBag.Staff_List = stafflist;
            ViewBag.TotalCount = stafflist.Count();
            //ViewBag.Rolename = Session["rolename"].ToString();

            return View();
        }

        public JsonResult EditStaffAttendence(int stafid, string fromdate, string todate)
        {
            DateTime fromdate1 = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            DateTime todate1 = DateTime.ParseExact(todate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string month = fromdate1.Month.ToString();
            string year = fromdate1.Year.ToString();
            int fromday = fromdate1.Day;
            int today = todate1.Day;
            int year1 = fromdate1.Year;
            int month1 = fromdate1.Month;
            int day = DateTime.DaysInMonth(year1, month1);

            var staffid = stafid;
            int employeid = Convert.ToInt32(stafid);
            //List<StafsDetails> staffdetails = new List<StafsDetails>();


            var staffdata = _context.StafsDetails.FirstOrDefault(x => x.StafId == employeid);
            var data = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id == staffid && x.Attendence_Month == month && x.Attendence_Year == year).ToList();
            //data = data.Where(x => x.Attendence_Day >= fromday && x.Attendence_Day <= today).ToList();
            var fullpresentday = data.Where(x => x.Staff_Id == staffid && x.Mark_FullDayPresent == "A").Count();
            var halpresentday = data.Where(x => x.Staff_Id == staffid && x.Mark_FullDayPresent == "½P").Count();
            double halfdayabasent = (double)halpresentday / 2;
            double totalabsent = fullpresentday + halfdayabasent;
            double totalpresent = today - totalabsent;
            double Noofworkingdays = totalpresent * 100;
            double Totalattendence = Math.Round(Noofworkingdays / today);



            var html = "";

            html += "<thead>";
            html += "<tr>";
            html += "<th style='background-color:#41BDE2'>StaffId</th>";
            html += "<th style='background-color:#41BDE2'>Name</th>";


            for (int j = 1; j <= day; j++)
            {
                html += "<th style='background-color:#41BDE2'>" + j + "</th>";

            }

            html += "<th style='background-color:#41BDE2'>Total</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody id='tbody'>";
            html += "<tr>";
            html += "<td>" + staffdata.StafId + "</td>";
            html += "<td>" + staffdata.Name + "</td>";

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Mark_FullDayPresent == "P")
                {
                    html += "<td><input type='text' value='" + data[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/></td>";
                }
                else if (data[i].Mark_FullDayPresent == "A")
                {
                    html += "<td> <input type='text' value='" + data[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/> </td>";
                }
                else
                {
                    html += "<td><input type='text' value='" + data[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/></td>";
                }
            }

            html += "<td>" + Totalattendence + "%</td>";
            html += "</tr>";
            html += "</tbody>";
            //ViewBag.data = html;


            return Json(html, JsonRequestBehavior.AllowGet);

        }


        public JsonResult EditFullStaffaAttendance(string fromdate, string todate)
        {

            DateTime fromdate1 = DateTime.ParseExact(fromdate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            DateTime todate1 = DateTime.ParseExact(todate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string month = todate1.Month.ToString();
            string year = todate1.Year.ToString();
            int year1 = todate1.Year;
            int month1 = todate1.Month;
            int fromday = fromdate1.Day;
            int To_Day = todate1.Day;
            int day = DateTime.DaysInMonth(year1, month1);
            var stafflist = _context.StafsDetails.ToList();

            var stafattendance = _context.Tbl_StaffAttendance.ToList();
            string html = "";
            html += "<thead>";
            html += "<tr>";
            //html += "<th style='background-color:#41BDE2'>S.No</th>";
            html += "<th style='background-color:#41BDE2'>StaffId</th>";
            html += "<th style='background-color:#41BDE2'>Name</th>";


            for (int j = 1; j <= day; j++)
            {
                html += "<th style='background-color:#41BDE2'>" + j + "</th>";

            }

            html += "<th style='background-color:#41BDE2'>Total</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody id='tbody'>";
            int a = 1;
            foreach (var item in stafflist)
            {
                if (item.EmployeeCode != null)
                {
                    int employcode = Convert.ToInt32(item.EmployeeCode);
                    var PerdayAtte = stafattendance.Where(x => x.Staff_Id == employcode && x.Attendence_Month == month && x.Attendence_Year == year).ToList();
                    //PerdayAtte = PerdayAtte.Where(x => x.Attendence_Day >= fromday && x.Attendence_Day <= To_Day).ToList();

                    if (PerdayAtte.Count > 0)
                    {

                        var fulldaypresent = PerdayAtte.Where(x => x.Staff_Id == employcode && x.Mark_FullDayPresent == "A").Count();
                        var halpresentday = PerdayAtte.Where(x => x.Staff_Id == employcode && x.Mark_FullDayPresent == "½P").Count();

                        double halfdayabasent = (double)halpresentday / 2;
                        double totalabsent = fulldaypresent + halfdayabasent;
                        double totalpresent = To_Day - totalabsent;
                        double Noofworkingdays = totalpresent * 100;
                        double Totalattendence = Math.Round(Noofworkingdays / To_Day);

                        html += "<tr>";
                        //html += "<td>" + a + "</td>";
                        html += "<td>" + item.EmployeeCode + "</td>";
                        html += "<td>" + item.Name + "</td>";
                        for (int i = 0; i < PerdayAtte.Count; i++)
                        {
                            if (PerdayAtte[i].Mark_FullDayPresent == "P")
                            {

                                html += "<td><input type='text' value='" + PerdayAtte[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/></td>";
                            }
                            else if (PerdayAtte[i].Mark_FullDayPresent == "A")
                            {
                                html += "<td><input type='text' value='" + PerdayAtte[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/></td>";
                            }
                            else
                            {
                                html += "<td><input type='text' value='" + PerdayAtte[i].Mark_FullDayPresent + "' style='width:40px;' class='markattendance'/></td>";
                            }
                        }

                        html += "<td>" + Totalattendence + "%</td>";
                        html += "</tr>";

                    }

                }
                a++;

            }
            html += "</tbody>";
            return Json(html, JsonRequestBehavior.AllowGet);
        }



        ////update attendance
        public ActionResult UpdateStaffAttendance(StaffAttendenceViewmodel UpdateStafattence)
        {

            try
            {
                if (UpdateStafattence.Fromdate != null && UpdateStafattence.Todate != null)
                {

                    DateTime fromdate = Convert.ToDateTime(UpdateStafattence.Fromdate);
                    DateTime todate = Convert.ToDateTime(UpdateStafattence.Todate);
                    foreach (var item in UpdateStafattence.Staffid)
                    {
                        int staffids = Convert.ToInt32(item);


                        var Attendancedata = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id == staffids && x.Attendence_Month == fromdate.Month.ToString()).ToList();
                        int i = 0;

                        foreach (var items in Attendancedata)
                        {
                            var perdayatt = Attendancedata.FirstOrDefault(x => x.Staff_Id == items.Staff_Id && x.Attendence_Month == items.Attendence_Month && x.StaffAtte_Id == items.StaffAtte_Id);

                            perdayatt.Mark_FullDayPresent = UpdateStafattence.MarkAttendance[i];
                            _context.SaveChanges();
                            i++;
                        }
                    }
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successfully');location.replace('/Staf/EditStaffAttendance')</script>");


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View();
        }

        public ActionResult AddPFCalculation(Account_SummaryViewmodel tbl_AccountSummary)
        {
            DateTime date = DateTime.ParseExact(tbl_AccountSummary.Added_Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var month = date.Month.ToString();
            var year = date.Year.ToString();
            var day = date.Date.ToString();
            double Pension = 0;
            int TotalPensionAmount = 0;
            double PensionAMount = 0;


            tbl_AccountSummary.Added_Day = day;
            tbl_AccountSummary.Added_Month = month;
            tbl_AccountSummary.Added_Year = year;
            var stafid = Convert.ToString(tbl_AccountSummary.Staff_Id);
            var stafdata = _context.StafsDetails.FirstOrDefault(x => x.StafId.ToString() == stafid);
            double Baiscandda = tbl_AccountSummary.Basic_Salary + tbl_AccountSummary.DA;

            var Pension_Percentage = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicPay_Id == 6);

            if (tbl_AccountSummary.Gross > 8000)
            {
                Pension = Baiscandda / 100 * Pension_Percentage.Basic_Amount;//Pension Amount
                PensionAMount = Math.Round(Pension);
                TotalPensionAmount = Convert.ToInt32(PensionAMount);
            }
            else
            {
                TotalPensionAmount = 0;
            }


            Tbl_AccountSummary tbl_Account = new Tbl_AccountSummary();
            tbl_Account.Added_Day = day;
            tbl_Account.Added_Month = month;
            tbl_Account.Added_Year = year;
            tbl_Account.Staff_Id = tbl_AccountSummary.Staff_Id;
            tbl_Account.Staff_Name = tbl_AccountSummary.Staff_Name;
            tbl_Account.NetPay = tbl_AccountSummary.NetPay;
            tbl_Account.PF = tbl_AccountSummary.PF;
            tbl_Account.Basic_Salary = tbl_AccountSummary.Basic_Salary;
            tbl_Account.DA = tbl_AccountSummary.DA;
            tbl_Account.Professional_Tax = tbl_AccountSummary.Professional_Tax;
            tbl_Account.Employee_Contribution = tbl_AccountSummary.Employee_Contribution;
            tbl_Account.Employer_Contribution = tbl_AccountSummary.Employer_Contribution;
            tbl_Account.Net_Pay = tbl_AccountSummary.Net_Pay;
            tbl_Account.ESI = tbl_AccountSummary.ESI;
            tbl_Account.Gross = tbl_AccountSummary.Gross;
            tbl_Account.Total_Salary = tbl_AccountSummary.Total_Salary;
            tbl_Account.LOP = tbl_AccountSummary.LOP;
            tbl_Account.CCA = tbl_AccountSummary.CCA;
            tbl_Account.HRA = tbl_AccountSummary.HRA;
            tbl_Account.OtherALlowance = tbl_AccountSummary.OtherALlowance;
            tbl_Account.NoOfdayspresent = tbl_AccountSummary.NoOfdayspresent;
            tbl_Account.TotalPercentage = tbl_AccountSummary.TotalPercentage;
            tbl_Account.Deduction_Amt = tbl_AccountSummary.Deduction_Amt;
            tbl_Account.Arrear_Amt = tbl_AccountSummary.Arrear_Amt;
            tbl_Account.Arrear = tbl_AccountSummary.Arrear;

            var data = _context.Tbl_AccountSummary.FirstOrDefault(x => x.Staff_Id == tbl_AccountSummary.Staff_Id && x.Added_Month == month);
            if (data != null)
            {
                tbl_Account.Staff_Id = data.Staff_Id;
                tbl_Account.Summary_Id = data.Summary_Id;

                _context.Entry(data).CurrentValues.SetValues(tbl_Account);
                _context.SaveChanges();
            }
            else
            {
                _context.Tbl_AccountSummary.Add(tbl_Account);
                _context.SaveChanges();
            }

            //For Letter To Bank
            var Salarystatement = _context.Tbl_SalaryStatement.FirstOrDefault(x => x.Employee_Code == tbl_AccountSummary.Staff_Id && x.Salarystatement_Month == month);
            Tbl_SalaryStatement tbl_SalaryStatement = new Tbl_SalaryStatement();

            if (Salarystatement == null)
            {
                tbl_SalaryStatement.Employers_Designation = stafdata.Employee_Designation;
                tbl_SalaryStatement.Employee_Name = stafdata.Name;
                var employeecode = stafdata.StafId;
                tbl_SalaryStatement.Employee_Code = Convert.ToInt32(employeecode);
                tbl_SalaryStatement.Employee_AccountNo = stafdata.Account_No;
                tbl_SalaryStatement.Total_Salary = Convert.ToString(tbl_AccountSummary.Net_Pay);
                tbl_SalaryStatement.AccountDetails_Id = stafdata.Employee_AccountId;
                tbl_SalaryStatement.Salarystatement_Month = month;
                tbl_SalaryStatement.Salarystatement_year = year;
                tbl_SalaryStatement.SalaryStatement_Date = tbl_AccountSummary.Added_Date;
                tbl_SalaryStatement.StaffCategory_Id = stafdata.Category_Id;
                _context.Tbl_SalaryStatement.Add(tbl_SalaryStatement);
                _context.SaveChanges();

            }
            else
            {
                tbl_SalaryStatement.SalaryStatement_Id = Salarystatement.SalaryStatement_Id;
                tbl_SalaryStatement.Employers_Designation = Salarystatement.Employers_Designation;
                tbl_SalaryStatement.Employee_Name = Salarystatement.Employee_Name;
                var employeecode = Salarystatement.Employee_Code;
                tbl_SalaryStatement.Employee_Code = Convert.ToInt32(employeecode);
                tbl_SalaryStatement.Employee_AccountNo = Salarystatement.Employee_AccountNo;
                tbl_SalaryStatement.Total_Salary = Convert.ToString(tbl_AccountSummary.Net_Pay);
                tbl_SalaryStatement.AccountDetails_Id = Salarystatement.AccountDetails_Id;
                tbl_SalaryStatement.Salarystatement_Month = month;
                tbl_SalaryStatement.Salarystatement_year = year;
                tbl_SalaryStatement.SalaryStatement_Date = tbl_AccountSummary.Added_Date;
                tbl_SalaryStatement.StaffCategory_Id = Salarystatement.StaffCategory_Id;
                _context.Entry(Salarystatement).CurrentValues.SetValues(tbl_SalaryStatement);
                _context.SaveChanges();
            }


            ////For EpF and ESI Statement

            var Epf_Statement = _context.Tbl_EPFStatement.FirstOrDefault(x => x.Employee_Code == tbl_AccountSummary.Staff_Id.ToString() && x.Added_Month == month);

            Tbl_EPFStatement tbl_EPFStatement = new Tbl_EPFStatement();

            if (Epf_Statement == null)
            {
                tbl_EPFStatement.Employee_Code = stafdata.StafId.ToString();
                tbl_EPFStatement.Employee_Name = stafdata.Name;
                tbl_EPFStatement.Gross_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.Epf_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.EPs_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.EDLIWages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.Employe_Contribution = Convert.ToInt32(tbl_AccountSummary.PF);
                tbl_EPFStatement.EPS_Pension = TotalPensionAmount;
                tbl_EPFStatement.Employer_Contribution = Convert.ToInt32(tbl_AccountSummary.Employer_Contribution);
                tbl_EPFStatement.NCP_Days = 0;
                tbl_EPFStatement.Refund_Advances = 0;
                tbl_EPFStatement.Added_Date = tbl_AccountSummary.Added_Date;
                tbl_EPFStatement.Added_Day = Convert.ToString(day);
                tbl_EPFStatement.Added_Month = month;
                tbl_EPFStatement.Added_Year = year;
                tbl_EPFStatement.StaffCategory_Id = stafdata.Category_Id;
                _context.Tbl_EPFStatement.Add(tbl_EPFStatement);
                _context.SaveChanges();

            }
            else
            {
                tbl_EPFStatement.EPFstatement_Id = Epf_Statement.EPFstatement_Id;
                tbl_EPFStatement.Employee_Code = Epf_Statement.Employee_Code;
                tbl_EPFStatement.Employee_Name = Epf_Statement.Employee_Name;
                tbl_EPFStatement.Gross_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.Epf_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.EPs_Wages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.EDLIWages = Convert.ToInt32(Baiscandda);
                tbl_EPFStatement.Employe_Contribution = Convert.ToInt32(tbl_AccountSummary.PF);
                tbl_EPFStatement.EPS_Pension = TotalPensionAmount;
                tbl_EPFStatement.Employer_Contribution = Convert.ToInt32(tbl_AccountSummary.Employer_Contribution);
                tbl_EPFStatement.NCP_Days = 0;
                tbl_EPFStatement.Refund_Advances = 0;
                tbl_EPFStatement.Added_Date = tbl_AccountSummary.Added_Date;
                tbl_EPFStatement.Added_Day = Convert.ToString(day);
                tbl_EPFStatement.Added_Month = month;
                tbl_EPFStatement.Added_Year = year;
                tbl_EPFStatement.StaffCategory_Id = Epf_Statement.StaffCategory_Id;
                _context.Entry(Epf_Statement).CurrentValues.SetValues(tbl_EPFStatement);
                _context.SaveChanges();
            }


            //For Deductions

            var deductiondata = _context.Tbl_Deductions.FirstOrDefault(x => x.Staff_Id == tbl_AccountSummary.Staff_Id && x.Added_Year == year && x.Added_Month == month);
            if (deductiondata != null)
            {
                Tbl_Deductions tbldedu = new Tbl_Deductions();
                tbldedu.Deduction_Amt = tbl_AccountSummary.Deduction_Amt;
                tbldedu.Net_Pay = tbl_AccountSummary.NetPay;
                tbldedu.Staff_Id = tbl_AccountSummary.Staff_Id;
                tbldedu.Staff_Name = stafdata.Name;
                tbldedu.Added_Day = day;
                tbldedu.Added_Month = month;
                tbldedu.Added_Year = year;
                tbldedu.Deductions_Id = deductiondata.Deductions_Id;

                _context.Entry(deductiondata).CurrentValues.SetValues(tbldedu);
                _context.SaveChanges();

            }
            else
            {
                if (tbl_AccountSummary.Deduction_Amt > 0)
                {
                    Tbl_Deductions tbl_Deductions = new Tbl_Deductions();
                    tbl_Deductions.Deduction_Amt = tbl_AccountSummary.Deduction_Amt;
                    tbl_Deductions.Net_Pay = tbl_AccountSummary.NetPay;
                    tbl_Deductions.Staff_Id = tbl_AccountSummary.Staff_Id;
                    tbl_Deductions.Staff_Name = stafdata.Name;
                    tbl_Deductions.Added_Day = day;
                    tbl_Deductions.Added_Month = month;
                    tbl_Deductions.Added_Year = year;

                    _context.Tbl_Deductions.Add(tbl_Deductions);
                    _context.SaveChanges();

                }
            }


            //For Arrear
            var arreardata = _context.Tbl_Arrear.FirstOrDefault(x => x.Staff_Id == tbl_AccountSummary.Staff_Id && x.Added_Month == month && x.Added_Year == year);
            if (arreardata != null)
            {
                Tbl_Arrear tbl_Arrear = new Tbl_Arrear();
                tbl_Arrear.Arrear = tbl_AccountSummary.Arrear;
                tbl_Arrear.Arrear_Amt = tbl_AccountSummary.Arrear_Amt;
                tbl_Arrear.Staff_Id = tbl_AccountSummary.Staff_Id;
                tbl_Arrear.Staff_Name = stafdata.Name;
                tbl_Arrear.Added_Month = month;
                tbl_Arrear.Added_Year = year;
                tbl_Arrear.Added_Day = day;
                tbl_Arrear.Arrear_Id = arreardata.Arrear_Id;

                _context.Entry(arreardata).CurrentValues.SetValues(tbl_Arrear);
                _context.SaveChanges();

            }
            else
            {

                if (tbl_AccountSummary.Arrear_Amt > 0 && tbl_AccountSummary.Arrear == "Yes")
                {
                    Tbl_Arrear tblarr = new Tbl_Arrear();
                    tblarr.Arrear = tbl_AccountSummary.Arrear;
                    tblarr.Arrear_Amt = tbl_AccountSummary.Arrear_Amt;
                    tblarr.Staff_Id = tbl_AccountSummary.Staff_Id;
                    tblarr.Staff_Name = stafdata.Name;
                    tblarr.Added_Month = month;
                    tblarr.Added_Year = year;
                    tblarr.Added_Day = day;

                    _context.Tbl_Arrear.Add(tblarr);
                    _context.SaveChanges();
                }



            }


            //For PastArrear
            DateTime pastarreardate = Convert.ToDateTime(tbl_AccountSummary.Pastarreardate);
            var pastarreardata = _context.Tbl_Arrear.FirstOrDefault(x => x.Staff_Id == tbl_AccountSummary.Staff_Id && x.Added_Month == pastarreardate.Month.ToString() && x.Added_Year == pastarreardate.Year.ToString());
            if (pastarreardata != null)
            {
                pastarreardata.Arrear_Amt = tbl_AccountSummary.Pastarrearamt;
                pastarreardata.Deduction_Amt = tbl_AccountSummary.Pastdeductionamt;
                pastarreardata.Added_Date = pastarreardate.Date.ToString();
                pastarreardata.Added_Day = pastarreardate.Day.ToString();
                pastarreardata.Added_Month = pastarreardate.Month.ToString();
                pastarreardata.Added_Year = pastarreardate.Year.ToString();

                if (tbl_AccountSummary.Pastarrearamt == 0 && tbl_AccountSummary.Pastdeductionamt == 0)
                {
                    pastarreardata.Arrear = "No";
                }

                _context.SaveChanges();

            }

            return Content("<script language='javascript' type='text/javascript'>location.replace('/Account/PFCalculation')</script>");


            //return View();
        }

        //Check Arrearamt
        public JsonResult CheckArrear(string date, int stafid)
        {
            int ArreareAmt = 0;
            try
            {
                if (date != null && date != "")
                {

                    DateTime dateTime = Convert.ToDateTime(date);

                    var checkarrear = _context.Tbl_Arrear.FirstOrDefault(x => x.Staff_Id == stafid && x.Added_Month == dateTime.Month.ToString() && x.Added_Year == dateTime.Year.ToString());

                    if (checkarrear != null)
                    {
                        if (checkarrear.Deduction_Amt == 0)
                        {

                            ArreareAmt = checkarrear.Arrear_Amt;
                        }
                        else
                        {
                            ArreareAmt = checkarrear.Deduction_Amt;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Json(ArreareAmt, JsonRequestBehavior.AllowGet);
        }
    }

    public class StaffAttendence
    {
        public string Present { get; set; }
    }
    //// Staff Remark
    //public ActionResult StaffRemark()
    //{
    //    if (Session["RoleName"] != null)
    //    {
    //        string roleName = Session["RoleName"].ToString();
    //        // Use the roleName as needed
    //        if (roleName == "Staff")
    //        {
    //            long staffId = Int64.Parse(Session["StaffID"].ToString());
    //            var staff = _context.StafsDetails.Where(x => x.StafId == staffId).ToList();
    //            ViewBag.Staff = staff;

    //        }
    //        else
    //        {
    //            var staff = _context.StafsDetails.OrderBy(x => x.Name).ToList();
    //            ViewBag.Staff = staff;
    //            var BatchList = _context.Tbl_Batches.Select(x => new Data.Models.BatchListDTO
    //            {
    //                Batch_Id = x.Batch_Id,
    //                Batch_Name = x.Batch_Name
    //            }).OrderBy(x => x.Batch_Name).ToList();
    //            ViewBag.BatchList = BatchList;
    //        }
    //    }
    //    var batch = _context.Tbl_Batches.Distinct().ToList();
    //    ViewBag.Batch = batch;
    //    return View();
    //}
}