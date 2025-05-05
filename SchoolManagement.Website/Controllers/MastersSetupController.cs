using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class MastersSetupController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        
        // GET: MastersSetup
        public ActionResult Index()
        {
            return View();          
        }
        //Add Class & Section
        public ActionResult AddClassSection()
        {
            var ClassAndSection = _context.ClassAndSection.ToList();
            ViewBag.ClassAndSection = ClassAndSection;
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();           
            return View();
                      
        }
        [HttpPost]
        public ActionResult AddClassSection(string Class, string Section, string OtherSection)
        {
           
            ClassAndSection ClassAndSection = new ClassAndSection()
            {
                Class = Class,
                Section = Section,
                OtherSection = OtherSection

            };
            _context.ClassAndSection.Add(ClassAndSection);
            _context.SaveChanges();
            string url = this.Request.UrlReferrer.AbsoluteUri;
            return Content("<script language='javascript' type='text/javascript'>alert('Record Save Successfully!');location.replace('" + url + "')</script>");
                     
        }
        public JsonResult GetClassSectionById(int id)
        {
            var data = _context.ClassAndSection.FirstOrDefault(x => x.Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateClassSection(ClassAndSection ClassAndSection)
        {
            var existingobj = _context.ClassAndSection.FirstOrDefault(e => e.Id == ClassAndSection.Id);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(ClassAndSection);
                _context.SaveChanges();
            }
            return RedirectToAction("AddClassSection");
        }
        public JsonResult DeleteClassSectionItem(int id)
        {
            var genralobj = _context.ClassAndSection.FirstOrDefault(e => e.Id == id);
            if (genralobj != null)
            {
                _context.ClassAndSection.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        //Add Subject
        public ActionResult AddSubjectSetup()
        {
            var AddSubjectSetup = _context.Subjects.ToList();
            ViewBag.AddSubjectSetup = AddSubjectSetup;

            var allStaf = _context.StafsDetails.ToList();
            ViewBag.allStaf = allStaf;

            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            ViewBag.Subject = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Group").DataListId.ToString()).ToList();

            return View();

        }
        [HttpPost]
        public ActionResult AddSubjectSetup(string Class, string Teacher, string Subject)
        {

            Subjects Subjects = new Subjects()
            {
                Class = Class,
                Teacher = Teacher,
                Subject = Subject

            };
            _context.Subjects.Add(Subjects);
            _context.SaveChanges();
            string url = this.Request.UrlReferrer.AbsoluteUri;
            return Content("<script language='javascript' type='text/javascript'>alert('Record Save Successfully!');location.replace('" + url + "')</script>");

        }
        public JsonResult GetSubjectById(int id)
        {
            var data = _context.Subjects.FirstOrDefault(x => x.Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSubject(Subjects Subjects)
        {
            var existingobj = _context.Subjects.FirstOrDefault(e => e.Id == Subjects.Id);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(Subjects);
                _context.SaveChanges();
            }
            return RedirectToAction("AddSubjectSetup");
        }
        public JsonResult DeleteSubject(int id)
        {
            var genralobj = _context.Subjects.FirstOrDefault(e => e.Id == id);
            if (genralobj != null)
            {
                _context.Subjects.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        //Add Department
        public ActionResult AddDepartmentSetup()
        {
            var Departments = _context.tbl_Departments.ToList();
            ViewBag.Departments = Departments;
            return View();

        }
        [HttpPost]
        public ActionResult AddDepartmentSetup(tbl_Department Departments)
        {

            //tbl_Department Depertment = new tbl_Department()
            //{
            //   DepartmentName  = Departments.Depertment
            //};

            _context.tbl_Departments.Add(Departments);
            _context.SaveChanges();
            string url = this.Request.UrlReferrer.AbsolutePath;
            return Content("<script language='javascript' type='text/javascript'>alert('Data Save Successfully!');location.replace('" + url + "')</script>");

          
        }
        public JsonResult GetDepartmentById(int id)
        {
            var data = _context.tbl_Departments.FirstOrDefault(x => x.DepartmentId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDepartment(tbl_Department tbl_Departments)
        {
            var existingobj = _context.tbl_Departments.FirstOrDefault(e => e.DepartmentId == tbl_Departments.DepartmentId);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(tbl_Departments);
                _context.SaveChanges();
            }
            return RedirectToAction("AddDepartmentSetup");
        }
        public JsonResult DeleteDepartment(int id)
        {
            var genralobj = _context.tbl_Departments.FirstOrDefault(e => e.DepartmentId == id);
            if (genralobj != null)
            {
                _context.tbl_Departments.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        //Add Classroom setup
        public ActionResult ClassRoomSetup1()
        {
            var Classrooms = _context.Classrooms.ToList();
            ViewBag.Classrooms = Classrooms;       
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.RoomNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "RoomNo").DataListId.ToString()).ToList();
            ViewBag.RoomType = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "RoomType").DataListId.ToString()).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult ClassroomSetup(string Remarks, string Location, string className, string RoomNo, string RoomType, string Seatingcapacity)
        {
            Classrooms Classrooms = new Classrooms()
            {
                className = className,
                RoomNo = RoomNo,
                RoomType = RoomType,
                Seatingcapacity = Seatingcapacity,
                Location = Location,
                Remarks = Remarks

            };
            _context.Classrooms.Add(Classrooms);
            _context.SaveChanges();
            string url = this.Request.UrlReferrer.AbsoluteUri;
            return Content("<script language='javascript' type='text/javascript'>alert('Record Save Successfully!');location.replace('" + url + "')</script>");

        }
        public JsonResult GetClassroomSetupById(int id)
        {
            var data = _context.Classrooms.FirstOrDefault(x => x.Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateClassroomSetup(Classrooms Classrooms)
        {
            var existingobj = _context.Classrooms.FirstOrDefault(e => e.Id == Classrooms.Id);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(Classrooms);
                _context.SaveChanges();
            }
            return RedirectToAction("ClassRoomSetup1");
        }
        public JsonResult DeleteClassroomSetup(int id)
        {
            var genralobj = _context.Classrooms.FirstOrDefault(e => e.Id == id);
            if (genralobj != null)
            {
                _context.Classrooms.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        //Add Set Exam
        public ActionResult SetExamTypes()
        {
            var ExamTypes = _context.ExamTypes.ToList();
            ViewBag.ExamTypes = ExamTypes;
           
            return View();
        }
        [HttpPost]
        public ActionResult SetExamTypes(string ExamType)
        {
            ExamTypes ExamTypes = new ExamTypes()
            {
                ExamType = ExamType
            
            };
            _context.ExamTypes.Add(ExamTypes);
            _context.SaveChanges();
            string url = this.Request.UrlReferrer.AbsoluteUri;
            return Content("<script language='javascript' type='text/javascript'>alert('Record Save Successfully!');location.replace('" + url + "')</script>");

        }
        public JsonResult GetSetExamTypesById(int id)
        {
            var data = _context.ExamTypes.FirstOrDefault(x => x.Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateSetExamTypes(ExamTypes ExamTypes)
        {
            var existingobj = _context.ExamTypes.FirstOrDefault(e => e.Id == ExamTypes.Id);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(ExamTypes);
                _context.SaveChanges();
            }
            return RedirectToAction("SetExamTypes");
        }
        public JsonResult DeleteSetExamTypes(int id)
        {
            var genralobj = _context.ExamTypes.FirstOrDefault(e => e.Id == id);
            if (genralobj != null)
            {
                _context.ExamTypes.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }
    } 
}