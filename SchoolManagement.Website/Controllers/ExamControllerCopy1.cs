using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoogleApi.Entities.Places.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PuppeteerSharp;
using PuppeteerSharp.Media;


using DinkToPdf;
using DinkToPdf.Contracts;
using SchoolManagement.Website.Helpers;

using log4net;
//using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json.Linq;
//using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.Models.DataAccess;
using SchoolManagement.Website.ViewModels;

namespace SchoolManagement.Website.Controllers
{

    public class ExamControllerCopy1 : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public decimal Pre1AllTotal { get; private set; }
        public decimal SelectionAllTotal { get; private set; }
        public decimal PromotionAllTotal { get; private set; }
        public decimal Pre2AllTotal { get; private set; }
        public static int Batch_Id = 0;

        //GradingCriteria

        public ActionResult ExamGrading()
        {
            List<GradingCriteria> gradingCriteriaList = new List<GradingCriteria>();
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            var getBoardName = _context.schoolBoards.Where(x => x.BoardID == getBoardID).FirstOrDefault();

            //gradingCriteriaList = _context.gringCriteria.Where(x => x.BoardID == getBoardID
            var gradingCriteria = (from e in _context.gradingCriteria
                                   join d in _context.tbl_Term on e.TermID equals d.TermID
                                   // join te in _context.tbl_Tests on e.TestID equals te.TestID
                                   join c in _context.Tbl_Batches on e.BatchID equals c.Batch_Id
                                   join f in _context.DataListItems on e.ClassID equals f.DataListItemId
                                   select new GradingCriteriaModel
                                   {
                                       CriteriaID = e.CriteriaID,
                                       //  TestID = te.TestID,
                                       BatchID = c.Batch_Id,
                                       ClassID = f.DataListItemId,
                                       BatchName = c.Batch_Name,
                                       ClassName = f.DataListItemName,
                                       TermName = d.TermName,
                                       //  TestName= te.TestName,
                                       //MinimumPercentage=Convert.ToDecimal(e.MinimumPercentage),
                                       //MaximumPercentage= Convert.ToDecimal(e.MaximumPercentage),
                                       MinimumPercentage = (e.MinimumPercentage),
                                       MaximumPercentage = (e.MaximumPercentage),
                                       Grade = e.Grade,
                                       GradeDescription = e.GradeDescription,
                                       TermID = d.TermID
                                       //StartDate = e.StartDate,
                                       //           EndDate = e.ToDate
                                   }).ToList();
            // gradingCriteriaList=
            //  ViewBag.GradingCriteriaList = gradingCriteria;
            ViewBag.GradingCriteriaLists = gradingCriteria.ToList<GradingCriteriaModel>();
            // ViewBag.GradingCriteriaList = gradingCriteriaList;
            object Tests = null; object Batch = null; object Classes = null;
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;
            if (Session["TestAssignData"] == null)
            {
                //Term= _context.tbl_Tests.ToList();
                Batch = _context.Tbl_Batches.ToList();//_context.DataListItems.Where(e => e.DataListId == "9" && e.Status == "Active").ToList();
                Classes = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
                //  Tests = _context.tbl_Tests.ToList();
            }
            else
            {
                var ViewDt = Session["TestAssignData"] as TblTestAssignDate;
                //  Tests = _context.tbl_Tests.Where(x => x.TestID == ViewDt.TestID).ToList();
                Batch = _context.Tbl_Batches.Where(e => e.Batch_Id == ViewDt.BatchID).ToList();
                Classes = _context.DataListItems.Where(e => e.DataListId == "5" && e.DataListItemId == ViewDt.ClassID).ToList();
            }
            //var innerJoin = from e in _context.TblTestAssignDate
            //                join d in _context.tbl_Term on e.TestID equals d.TermID
            //                join c in _context.Tbl_Batches on e.BatchID equals c.Batch_Id
            //                join f in _context.DataListItems on e.ClassID equals f.DataListItemId
            //                select new PL_TestAssignData
            //                {
            //                    TestAssignId = e.TestAssignID,
            //                    TestName = d.TermName,
            //                    BatchName = c.Batch_Name,
            //                    ClassName = f.DataListItemName,
            //                    StartDate = e.StartDate,
            //                    EndDate = e.ToDate
            //                };

            //ViewBag.TestAssignList = innerJoin.ToList<PL_TestAssignData>();
            ViewBag.ClassList = Classes; ViewBag.TestList = Tests; ViewBag.SectionList = Batch;
            ViewBag.Terms = Terms;
            if (Session["Msg"] != null)
            {
                ViewBag.Msg = Session["Msg"];
            }
            else
            {
                Session["Msg"] = null;
                ViewBag.Msg = Session["Msg"];
            }
            Session.Remove("Msg");
            return View();
        }
        // POST: GradingCriteria/Create
        [HttpPost]
        public ActionResult Create(GradingCriteria model)
        // public ActionResult Create(GradingCriteria model)
        {
            try
            {
                // Add the new grading criteria to the list
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                model.BoardID = getBoardID;
                _context.gradingCriteria.Add(model);
                _context.SaveChanges();
                // Redirect to the Index action to display the updated list
                return RedirectToAction("ExamGrading");
            }
            catch (Exception)
            {

                return RedirectToAction("ExamGrading");
            }



            // If the model state is not valid, return the Create view with the current model

        }

        public JsonResult GetGradingById(int id)
        {
            try
            {
                var data = _context.gradingCriteria.FirstOrDefault(x => x.CriteriaID == id);
                GradingCriteria grade = new GradingCriteria()
                {
                    TermID = data.TermID,
                    BatchID = data.BatchID,
                    // TestID=data.TestID,
                    ClassID = data.ClassID,
                    MaximumPercentage = data.MaximumPercentage,
                    MinimumPercentage = data.MinimumPercentage,
                    Grade = data.Grade,
                    GradeDescription = data.GradeDescription,
                    CriteriaID = id//---x-rnik--

                    //  BoardId = data.BoardId,

                };
                var Term = _context.tbl_Term.Where(e => e.TermID == data.TermID).ToList();
                //  var Tests= _context.tbl_Tests.Where(x => x.TestID == data.TestID).ToList();
                var Batch = _context.Tbl_Batches.Where(e => e.Batch_Id == data.BatchID).ToList();
                var Classes = _context.DataListItems.Where(e => e.DataListId == "5" && e.DataListItemId == data.ClassID).ToList();
                ViewBag.ClassList = Classes;
                // ViewBag.TestList = Tests;
                ViewBag.SectionList = Batch;
                ViewBag.Terms = Term;
                //if (data != null)
                //{
                //    return Json(data, JsonRequestBehavior.AllowGet);
                //}
                //else
                //{
                //    return Json(null, JsonRequestBehavior.AllowGet);
                //}
                //if (grade != null)
                //{
                return Json(grade, JsonRequestBehavior.AllowGet);
                // }
                //else
                //{
                //    return Json(null, JsonRequestBehavior.AllowGet);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateGrading(GradingCriteria gradingCriteria, HttpPostedFileBase Upload_Image)
        {
            try
            {
                var data = _context.gradingCriteria.FirstOrDefault(x => x.CriteriaID == gradingCriteria.CriteriaID);
                _context.Entry(data).CurrentValues.SetValues(gradingCriteria);
                _context.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamGrading');</script>");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteGrading(int Id)
        {
            try
            {
                var data = _context.gradingCriteria.FirstOrDefault(x => x.CriteriaID == Id);
                if (data != null)
                {
                    _context.gradingCriteria.Remove(data);
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamGrading');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Term
        public ActionResult ExamTerm()
        {

            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            //var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            //ViewBag.ClassList = Classes;
            List<Tbl_Term> termList = _context.tbl_Term.Where(x => x.BoardId == getBoardID).ToList();
            List<TermsM> TermsList = new List<TermsM>();
            foreach (var item in termList)
            {
                TermsM termsMnew = new TermsM()
                {
                    TermID = item.TermID,
                    BatchId = item.BatchId,
                    BoardId = item.BoardId,
                    TermName = item.TermName,
                    CreatedAt = item.CreatedAt,
                    UpdatedAt = item.UpdatedAt,
                    StartDate = item.StartDate?.ToString("dd/MM/yyyy"),
                    EndDate = item.EndDate?.ToString("dd/MM/yyyy")
                };
                TermsList.Add(termsMnew);
            }
            ViewBag.TermList = TermsList;

            return View();
        }

        // POST: TermManagement/Create
        [HttpPost]
        public ActionResult TermCreate(Tbl_Term model)
        {
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                model.BoardId = getBoardID;
                model.BatchId = 20;
                // Add the new term to the list
                _context.tbl_Term.Add(model);
                _context.SaveChanges();
                // Redirect to the TermManagement action to display the updated list
                return RedirectToAction("ExamTerm");
            }
            catch
            {
                return RedirectToAction("ExamTerm");

            }

        }

        public JsonResult GetTermById(int id)
        {
            try
            {
                var data = _context.tbl_Term.FirstOrDefault(x => x.TermID == id);
                if (data != null)
                {


                    TermsM termsMnew = new TermsM()
                    {
                        TermID = data.TermID,
                        BatchId = data.BatchId,
                        BoardId = data.BoardId,
                        TermName = data.TermName,
                        CreatedAt = data.CreatedAt,
                        UpdatedAt = data.UpdatedAt,
                        StartDate = data.StartDate?.ToString("dd/MM/yyyy"),
                        EndDate = data.EndDate?.ToString("dd/MM/yyyy")
                    };

                    return Json(termsMnew, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public ActionResult UpdateTerm(Tbl_Term term)
        //{
        //    try
        //    {
        //        var data = _context.tbl_Term.FirstOrDefault(x => x.TermID == term.TermID);
        //        if (data != null)
        //        {
        //            data.TermName = term.TermName;
        //            data.StartDate = term.StartDate;
        //            data.EndDate = term.EndDate;
        //            data.ClassId = term.ClassId;
        //            _context.SaveChanges();
        //        }

        //        return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamTerm');</script>");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public ActionResult DeleteTerm(int id)
        {
            try
            {
                var data = _context.tbl_Term.FirstOrDefault(x => x.TermID == id);
                if (data != null)
                {
                    _context.tbl_Term.Remove(data);
                    _context.SaveChanges();
                }

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamTerm');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Test

        public ActionResult ExamTest()
        {
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            var getBoardName = _context.schoolBoards.Where(x => x.BoardID == getBoardID).FirstOrDefault();

            var testList = _context.tbl_Tests.Where(x => x.BoardID == getBoardID).ToList();
            List<TestsTable> testsTables = new List<TestsTable>();
            foreach (var item in testList)
            {
                TestsTable testsTable = new TestsTable()
                {
                    TestID = item.TestID,
                    ClassID = item.ClassID,
                    ClassName = _context.DataListItems.Where(x => x.DataListItemId == item.ClassID).Select(x => x.DataListItemName).FirstOrDefault(),
                    BoardID = item.BoardID,
                    TestName = item.TestName,
                    TestType = item.TestType,
                    SubjectID = item.SubjectID,
                    TermID = item.TermID,
                    MaximumMarks = item.MaximumMarks,
                    Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectID).Select(x => x.Subject_Name).FirstOrDefault(),
                    Term = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault()
                };
                testsTables.Add(testsTable);
            }
            var Term = _context.tbl_Term.ToList();
            //ViewBag.Subject = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "subject").DataListId.ToString()).ToList();
            // ViewBag.Subject = _context.Tbl_SubjectsSetup.ToList();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.TestList = testsTables;
            ViewBag.Term = Term;
            ViewBag.ClassList = Classes;
            return View();
        }

        [HttpPost]
        public ActionResult CreateTest(Tbl_Tests model)
        {
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                model.BoardID = getBoardID;
                _context.tbl_Tests.Add(model);
                _context.SaveChanges();
                return RedirectToAction("ExamTest");
            }
            catch
            {
                return RedirectToAction("ExamTest");
            }

        }

        public JsonResult GetTestById(int id)
        {
            try
            {
                var data = _context.tbl_Tests.FirstOrDefault(x => x.TestID == id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult UpdateTest(Tbl_Tests test)
        {
            try
            {
                var data = _context.tbl_Tests.FirstOrDefault(x => x.TestID == test.TestID);
                if (data != null)
                {
                    data.ClassID = test.ClassID;
                    data.TestName = test.TestName;
                    data.TestType = test.TestType;
                    data.MaximumMarks = test.MaximumMarks;
                    data.TermID = test.TermID;
                    data.SubjectID = test.SubjectID;
                    data.IsOptional = test.IsOptional;
                    _context.SaveChanges();
                }

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamTest');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult DeleteTest(int id)
        {
            try
            {
                var data = _context.tbl_Tests.FirstOrDefault(x => x.TestID == id);
                if (data != null)
                {
                    _context.tbl_Tests.Remove(data);
                    _context.SaveChanges();
                }

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/ExamTest');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult SubjectByClassId(int classId)
        {
            var Tests = _context.Subjects.Where(x => x.Class_Id == classId).ToList();
            var subjectNames = _context.Subjects
               .Where(cs => cs.Class_Id == classId)
               .Join(_context.DataListItems,
                     cs => cs.Subject_ID,
                     sub => sub.DataListItemId,
                     (cs, sub) => new { sub.DataListItemId, sub.DataListItemName })
               .ToList();
            return Json(subjectNames, JsonRequestBehavior.AllowGet);
        }

        //ObtainedMarks
        public ActionResult ExamObtainedMarks()
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

        public ActionResult GetStaffClass(int staffId)
        {
            // Retrieve class data based on the staff ID
            var query = (from s in _context.Subjects
                         join c in _context.DataListItems on s.Class_Id equals c.DataListItemId
                         where s.StaffId == staffId
                         select c).Distinct();


            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetClassSection(int staffId, int classId)
        {
            // Retrieve class data based on the staff ID
            var query = (from s in _context.Subjects
                         join c in _context.DataListItems on s.Section_Id equals c.DataListItemId
                         where s.StaffId == staffId && s.Class_Id == classId
                         select c).Distinct();



            var results = query.ToList();

            return Json(results, JsonRequestBehavior.AllowGet);
        }
        public ActionResult GetSubjectByClass(int classId)
        {

            var subjectInfo = (from cs in _context.tbl_ClassSubject
                               join s in _context.Tbl_SubjectsSetup on cs.SubjectId equals s.Subject_ID
                               where cs.ClassId == classId
                               select new
                               {
                                   SubjectId = s.Subject_ID,
                                   SubjectName = s != null ? (cs.IsElective == true ? "(Elective)" + s.Subject_Name : s.Subject_Name) : null
                               }).ToList();


            return Json(subjectInfo, JsonRequestBehavior.AllowGet);
        }
        public JsonResult TestByClassId(int classId, int termId, int staffId, int sectionId)
        {

            //var staffsubjectids = _context.Subjects.Where(x => x.StaffId == 22).Select(x => x.Subject_ID).ToList();
            //var tests = _context.tbl_Tests.Where(x => x.SubjectID in staffsubjectids).tolist();
            bool IsClassTeacher = _context.Subjects.Any(x => x.Class_Id == classId && x.StaffId == staffId && x.Section_Id == sectionId && x.Class_Teacher == true);
            if (IsClassTeacher)
            {
                var Tests = _context.tbl_Tests.Where(x => x.ClassID == classId && x.TermID == termId).ToList();
                foreach (var item in Tests)
                {
                    var termName = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault();
                    item.TestName = item.TestName + "(" + item.TestType + ", " + termName + ")";
                }
                return Json(Tests, JsonRequestBehavior.AllowGet);

            }
            else
            {
                var staffsubjectids = _context.Subjects
                            .Where(x => x.StaffId == staffId)
                            .Select(x => x.Subject_ID)
                            .ToList();

                var tests = _context.tbl_Tests
                                    .Where(x => staffsubjectids.Contains((int)x.SubjectID) && x.TermID == termId && x.ClassID == classId)
                                    .ToList();

                var result = _context.Subjects
                         .Where(x => x.StaffId == staffId)
                         .SelectMany(subject => _context.tbl_Tests.Where(test => test.SubjectID == subject.Subject_ID && test.TermID == termId && test.ClassID == classId))
                         .Distinct()
                         .ToList();
                foreach (var item in result)
                {
                    var termName = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault();
                    item.TestName = item.TestName + "(" + item.TestType + ", " + termName + ")";
                }

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult TermsData()
        {
            var Terms = _context.tbl_Term.ToList();

            return Json(Terms, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStaffSubjects(int classId, int staffId)
        {
            var result = _context.Subjects
                                 .Where(x => x.StaffId == staffId && x.Class_Id == classId).Select(x => x.Subject_ID)
                                 .ToList();
            var Subjects = _context.Tbl_SubjectsSetup
                              .Where(x => result.Contains(x.Subject_ID))
                              .ToList();
            return Json(Subjects, JsonRequestBehavior.AllowGet);
        }
        public JsonResult StudentByClassSection(int classId, int sectionId, int testId, int termId, int staffId, int batchId)
        {
            List<ListStudent> listStudents = new List<ListStudent>();
            List<Tbl_Tests> Tests;

            try
            {

                bool IsClassTeacher = _context.Subjects.Any(x => x.Class_Id == classId && x.Batch_Id == batchId && x.StaffId == staffId && x.Section_Id == sectionId && x.Class_Teacher == true);
                if (IsClassTeacher)
                {
                    Tests = _context.tbl_Tests.Where(x => x.ClassID == classId && x.TermID == termId).ToList();
                    foreach (var item in Tests)
                    {
                        var termName = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault();
                        item.TestName = item.TestName + "(" + item.TestType + ", " + termName + ")";
                    }
                    Tbl_Tests remark = new Tbl_Tests()
                    {
                        TestID = 0,
                    };
                    Tests.Add(remark);
                }
                else
                {
                    var staffsubjectids = _context.Subjects
                                .Where(x => (x.StaffId == staffId && x.Class_Id == classId) || x.Batch_Id == batchId)
                                .Select(x => x.Subject_ID)
                                .ToList();

                    var tests = _context.tbl_Tests
                                        .Where(x => staffsubjectids.Contains((int)x.SubjectID) && x.TermID == termId && x.ClassID == classId)
                                        .ToList();

                    var subjects = _context.Subjects
                             .Where(x => x.StaffId == staffId && x.Class_Id == classId && x.Section_Id == sectionId).ToList();
                    Tests = subjects.Where(x => x.Batch_Id == batchId).SelectMany(subject => _context.tbl_Tests.Where(test => test.SubjectID == subject.Subject_ID
     && test.TermID == termId && test.ClassID == classId))
                             .Distinct()
                             .ToList();
                    foreach (var item in Tests)
                    {
                        var termName = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault();
                        item.TestName = item.TestName + "(" + item.TestType + ", " + termName + ")";
                    }


                }
                List<Data.Models.Student> studentlist = new List<Data.Models.Student>();

                var SubjectIds = Tests.Select(x => x.SubjectID).ToList();
                var electiveSubjectId = _context.tbl_ClassSubject
                              .Where(x => SubjectIds.Contains(x.SubjectId)
                              && x.ClassId == classId && x.IsElective == true)
                              .ToList();
                //  var isElective = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == electiveSubjectId.SubjectID).FirstOrDefault();
                //            if (isElective.IsElective == true)
                //            {
                //                studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId).OrderBy(x => x.Name).ToList();
                //                var studentIds = _context.tbl_Student_ElectiveRecord.Where(x=>x.ElectiveSubjectId==isElective.Subject_ID).ToList();
                //                studentlist = studentlist
                //.Where(student => studentIds.Any(studentId => studentId.StudentId == student.StudentId))
                //.ToList();

                //            }
                //            else
                //            {
                //                studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId).OrderBy(x => x.Name).ToList();
                //            }
                var Freezeitem = _context.Tbl_FreezeData.Where(x => x.ClassId == classId && x.SectionId == sectionId
                                               && x.TermId == termId && x.BatchId == batchId).FirstOrDefault();


                studentlist = _context.Students.Where(x => x.Class_Id == classId
            && x.Section_Id == sectionId && x.Batch_Id == batchId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
                var stdInfo = new List<Tbl_TestRecords>();
                List<long> fk = new List<long>();
                if (studentlist.Count == 0)
                {
                    stdInfo = _context.Tbl_TestRecord.Where(x => x.BatchId == batchId && x.ClassID == classId
            && x.SectionID == sectionId && x.TermID == termId).ToList();

                    foreach (var item in stdInfo)
                    {
                        var studentElectiveSubjectIds = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == item.StudentID).Select(x => x.ElectiveSubjectId).ToList();
                        var IsAlreadyExist1 = _context.Tbl_TestRecord.Where(x => x.ClassID == classId && x.SectionID == sectionId && x.TermID == termId).ToList();
                        var RecordFKID = IsAlreadyExist1.Where(x1 => x1.StudentID == item.StudentID).Select(x2 => x2.RecordID).FirstOrDefault();
                        fk.Add(RecordFKID);
                        var studentObtainedData = _context.tbl_TestObtainedMark.Where(x => x.RecordIDFK == RecordFKID).ToList();

                        List<StudentTestObtMarks> studentTestObtMarksList = new List<StudentTestObtMarks>();

                        foreach (var data in Tests)
                        {
                            if (data.TestID == 0)
                            {
                                var RemarkData = _context.tbl_Remark.Where(x => x.StudentId == item.StudentID && x.TermId == termId && x.BatchId == batchId).Select(x => x.Remark).FirstOrDefault();
                                StudentTestObtMarks studentTestObtMarks = new StudentTestObtMarks()
                                {
                                    TestID = 0,
                                    TestName = "Teacher Remark",
                                    ObtainedMarks = 0,
                                    MaximumdMarks = 0,
                                    Remark = RemarkData ?? ""
                                };
                                studentTestObtMarksList.Add(studentTestObtMarks);
                            }
                            else
                            {
                                StudentTestObtMarks studentTestObtMarksData = new StudentTestObtMarks()
                                {
                                    TestID = data.TestID,
                                    ObtainedMarks = studentObtainedData?.FirstOrDefault(x => x.TestID == data.TestID)?.ObtainedMarks ?? 0,
                                    MaximumdMarks = data.MaximumMarks,
                                    TestName = _context.tbl_Tests.Where(x => x.TestID == data.TestID).Select(x => x.TestName).FirstOrDefault(),
                                    IsElective = electiveSubjectId.Any(x => x.SubjectId == data.SubjectID) && !studentElectiveSubjectIds.Contains(data.SubjectID),
                                    IsOptional = (bool)_context.tbl_Tests.Where(x => x.TestID == data.TestID).Select(x => x.IsOptional).FirstOrDefault()
                                };
                                studentTestObtMarksList.Add(studentTestObtMarksData);
                            }
                        }
                        ListStudent listStudent = new ListStudent()
                        {
                            StudentId = item.StudentID,
                            StudentName = _context.Students.Where(x => x.StudentId == item.StudentID).Select(x => x.Name).FirstOrDefault(),
                            BatchId = Convert.ToInt32(item.BatchId),
                            IsFreeze = Freezeitem != null ? Freezeitem.IsFreeze : false

                        };
                        listStudent.studentTestObtMarks = studentTestObtMarksList;

                        listStudents.Add(listStudent);

                    }

                }
                else
                {
                    foreach (var item in studentlist)
                    {
                        var studentElectiveSubjectIds = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == item.StudentId).Select(x => x.ElectiveSubjectId).ToList();
                        var IsAlreadyExist1 = _context.Tbl_TestRecord.Where(x => x.ClassID == classId && x.SectionID == sectionId && x.TermID == termId && x.BatchId == batchId).ToList();
                        var RecordFKID = IsAlreadyExist1.Where(x1 => x1.StudentID == item.StudentId).Select(x2 => x2.RecordID).FirstOrDefault();
                        fk.Add(RecordFKID);
                        var studentObtainedData = _context.tbl_TestObtainedMark.Where(x => x.RecordIDFK == RecordFKID).ToList();

                        List<StudentTestObtMarks> studentTestObtMarksList = new List<StudentTestObtMarks>();

                        foreach (var data in Tests)
                        {
                            if (data.TestID == 0)
                            {
                                var RemarkData = _context.tbl_Remark.Where(x => x.StudentId == item.StudentId && x.TermId == termId && x.BatchId == batchId).Select(x => x.Remark).FirstOrDefault();
                                StudentTestObtMarks studentTestObtMarks = new StudentTestObtMarks()
                                {
                                    TestID = 0,
                                    TestName = "Teacher Remark",
                                    ObtainedMarks = 0,
                                    MaximumdMarks = 0,
                                    Remark = RemarkData ?? ""

                                };
                                studentTestObtMarksList.Add(studentTestObtMarks);
                            }
                            else
                            {
                                StudentTestObtMarks studentTestObtMarksData = new StudentTestObtMarks()
                                {
                                    TestID = data.TestID,
                                    ObtainedMarks = studentObtainedData?.FirstOrDefault(x => x.TestID == data.TestID)?.ObtainedMarks ?? 0,
                                    MaximumdMarks = data.MaximumMarks,
                                    TestName = _context.tbl_Tests.Where(x => x.TestID == data.TestID).Select(x => x.TestName).FirstOrDefault(),
                                    IsElective = electiveSubjectId.Any(x => x.SubjectId == data.SubjectID) && !studentElectiveSubjectIds.Contains(data.SubjectID),
                                    IsOptional = (bool)_context.tbl_Tests.Where(x => x.TestID == data.TestID).Select(x => x.IsOptional).FirstOrDefault()
                                };
                                studentTestObtMarksList.Add(studentTestObtMarksData);
                            }
                        }
                        ListStudent listStudent = new ListStudent()
                        {
                            StudentId = item.StudentId,
                            StudentName = item.Name + ' ' + item.Last_Name,
                            BatchId = item.Batch_Id,
                            IsFreeze = Freezeitem != null ? Freezeitem.IsFreeze : false

                        };
                        listStudent.studentTestObtMarks = studentTestObtMarksList;

                        listStudents.Add(listStudent);

                    }
                }


                var result = new
                {
                    IsUpdate = false,
                    data = listStudents.OrderBy(x => x.StudentName),
                    HeaderData = Tests
                };
                string commaSeparatedString = string.Join(",", fk);
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult GetStudentByClassSection(int classId, int sectionId, int termId, int Batchid)
        {
            List<ListStudent> listStudents = new List<ListStudent>();
            try
            {
                //var studentlist = _context.Students.Where(x => ((x.Class_Id == classId && x.Section_Id == sectionId) || x.Batch_Id == Batchid) && x.IsApplyforTC == false).Distinct().OrderBy(x => x.Name).ToList();
                IQueryable<SchoolManagement.Website.Models.Tbl_TestRecords> query = _context.Tbl_TestRecord;


                if (classId != 0 && sectionId != 0)
                {
                    query = query.Where(x => x.ClassID == classId && x.SectionID == sectionId);
                }

                if (Batchid != 0)
                {
                    query = query.Where(x => x.BatchId == Batchid);
                }
                var studentlist = new List<Data.Models.Student>();
                if (termId != 10)
                {
                    studentlist = (from testRecord in query
                                   join student in _context.Students
                                   on testRecord.StudentID equals student.StudentId
                                   where testRecord.TermID == termId && student.IsApplyforTC == false
                                   select student)
                               .Distinct()
                               .ToList();

                }
                else
                {
                    studentlist = (from testRecord in query
                                   join student in _context.Students
                                   on testRecord.StudentID equals student.StudentId
                                   where student.IsApplyforTC == false
                                   select student)
                                  .Distinct()
                                  .ToList();

                }


                foreach (var item in studentlist)
                {

                    item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                    ListStudent listStudent = new ListStudent()
                    {
                        StudentId = item.StudentId,
                        ClassName = _context.DataListItems.Where(x => x.DataListItemId == classId).Select(x => x.DataListItemName).FirstOrDefault(),
                        SectionName = item.Section,
                        StudentName = item.Name,
                        //BatchName = item.BatchName,  //---x-rnik--
                        BatchName = _context.Tbl_Batches.Where(x => x.Batch_Id == Batchid).FirstOrDefault().Batch_Name,
                        IsHold = _context.Tbl_HoldDetail.Where(x => x.StudentId == item.StudentId && x.BatchId == Batchid && x.TermId == termId && x.ClassId == classId).Select(x => x.IsHold).FirstOrDefault(),


                        ObtainedMarks = 0
                    };
                    listStudents.Add(listStudent);
                }
                return Json(listStudents.OrderBy(x => x.StudentName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult InsertUpdateObtainedMarks(List<StudentObtainedMarkModel> rowData, int staffId)
        {




            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            // Update existing records in Tbl_TestRecord
            foreach (var item in rowData)
            {
                var existingRecord = _context.Tbl_TestRecord.FirstOrDefault(tr => tr.TermID == item.TermID && tr.StudentID == item.StudentID && tr.ClassID == item.ClassID && tr.SectionID == item.SectionId && tr.BoardID == getBoardID && tr.BatchId == item.BatchId);
                if (existingRecord != null)
                {
                    bool IsClassTeacher = _context.Subjects.Any(x => x.Class_Id == item.ClassID && x.StaffId == staffId && x.Section_Id == item.SectionId && x.Class_Teacher == true);
                    if (IsClassTeacher)
                    {
                        var existingRemark = _context.tbl_Remark.FirstOrDefault(r => r.TermId == item.TermID && r.StudentId == item.StudentID && r.BoardId == getBoardID && r.BatchId == item.BatchId);

                        if (existingRemark != null)
                        {
                            existingRemark.Remark = item.Remark;
                        }
                        else
                        {
                            // Create a new Remark entity and add it to the context
                            var newRemark = new Tbl_Remark
                            {
                                TermId = item.TermID,
                                StudentId = item.StudentID,
                                BoardId = getBoardID,
                                Remark = item.Remark,
                                BatchId = item.BatchId
                            };

                            _context.tbl_Remark.Add(newRemark);
                        }

                        // Save changes to the context
                        _context.SaveChanges();
                        existingRecord.RankInClass = item.RankInClass;
                    }
                    // Update the associated CoScholasticData
                    foreach (var Dt in item.ObtainedMarkData)
                    {
                        var existingData = _context.tbl_TestObtainedMark.FirstOrDefault(data => data.RecordIDFK == existingRecord.RecordID && data.TestID == Dt.TestID);
                        if (existingData != null)
                        {
                            // Update existing data
                            //existingData.ObtainedCoScholasticID = Dt.ObtainedCoScholasticID;
                            existingData.ObtainedMarks = Dt.ObtainedMarks;
                        }
                        else
                        {
                            // Add new CoScholasticData for the existing record
                            Dt.RecordIDFK = existingRecord.RecordID;
                            _context.tbl_TestObtainedMark.Add(Dt);
                        }
                    }
                    existingRecord.BatchId = item.BatchId;

                    _context.SaveChanges();
                    //return Json(new { success = true, errormsg="Data Updated .." });

                }
                else
                {
                    item.BoardID = getBoardID;
                    bool IsClassTeacher = _context.Subjects.Any(x => x.Class_Id == item.ClassID && x.StaffId == staffId && x.Section_Id == item.SectionId && x.Class_Teacher == true);
                    if (IsClassTeacher)
                    {
                        var existingRemark = _context.tbl_Remark.FirstOrDefault(r => r.TermId == item.TermID && r.StudentId == item.StudentID && r.BoardId == getBoardID);

                        if (existingRemark != null)
                        {
                            existingRemark.Remark = item.Remark;
                        }
                        else
                        {
                            // Create a new Remark entity and add it to the context
                            Tbl_Remark tbl_Remark = new Tbl_Remark()
                            {
                                TermId = item.TermID,
                                BoardId = getBoardID,
                                StudentId = item.StudentID,
                                Remark = item.Remark
                            };
                            _context.tbl_Remark.Add(tbl_Remark);
                        }

                        // Save changes to the context
                        _context.SaveChanges();

                    }
                    // This item doesn't have a corresponding record in Tbl_TestRecord, so add it as a new record
                    Tbl_TestRecords testRecords = new Tbl_TestRecords()
                    {
                        BoardID = getBoardID,
                        TermID = item.TermID,
                        StudentID = item.StudentID,
                        ClassID = item.ClassID,
                        SectionID = item.SectionId,
                        BatchId = item.BatchId,
                        RankInClass = item.RankInClass
                    };
                    _context.Tbl_TestRecord.Add(testRecords);
                    _context.SaveChanges();
                    long latestId = testRecords.RecordID;
                    foreach (var Dt in item.ObtainedMarkData)
                    {
                        Dt.RecordIDFK = latestId;
                        _context.tbl_TestObtainedMark.Add(Dt);
                    }
                }
            }


            // Save changes to the database
            _context.SaveChanges();
            return Json(new { success = true }); // Return a success response if the data is saved successfully

            //if (hasMatchingRecords)
            //{
            //    // Update existing records in Tbl_TestRecord
            //    foreach (var item in rowData)
            //    {
            //        var existingRecord = _context.Tbl_TestRecord.FirstOrDefault(tr => tr.TestID == testId && tr.StudentID == item.StudentID);
            //        if (existingRecord != null)
            //        {
            //            // Update properties of the existing record with the values from 'item'
            //            existingRecord.ObtainedMarks = item.ObtainedMarks;
            //            // Update other properties as needed
            //        }
            //        else
            //        {
            //            // This item doesn't have a corresponding record in Tbl_TestRecord, so add it as a new record
            //            _context.Tbl_TestRecord.Add(item);
            //        }
            //    }
            //}
            //else
            //{
            //    // No matching records found in Tbl_TestRecord, so add all items as new records
            //    foreach (var item in rowData)
            //    {
            //        _context.Tbl_TestRecord.Add(item);
            //    }
            //}

            //// Save changes to the database
            //_context.SaveChanges();
            //return Json(new { success = true }); // Return a success response if the data is saved successfully
        }
        //Report Card
        public ActionResult ReportCard()
        {
            try
            {
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                ViewBag.ClassList = Classes;
                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                ViewBag.SectionList = Section;
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

        public class FileViewModel
        {
            public byte[] Content { get; set; }
            public string Extension { get; set; }
            public string FileName { get; set; }
        }
        #region Old
        public class PdfRequestModel
        {
            public string Htmlcontent { get; set; }
        }
        // Define a simple model to match the incoming JSON


        [HttpPost]
        public ActionResult ConvertToPdf(PdfRequestModel request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Htmlcontent))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "HTML content is missing.");
            }

            try
            {
                // Convert HTML to PDF (using your method)
                var fileContent = ConvertHtmlToPdfNew(request.Htmlcontent); // byte[]

                string fileName = "output.pdf";
                string path = Server.MapPath("~/Rotativa/" + fileName);

                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                System.IO.File.WriteAllBytes(path, fileContent);

                return Json(fileName); // return the file name to download
            }
            catch (Exception ex)
            {
                // Log error if needed
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "PDF generation failed.");
            }
        }

        //[HttpPost]
        //public string ConvertToPdf(string Htmlcontent)
        ////public FileStreamResult covertopdf(file fo)
        //{
        //    var result = ExecuteAction(() =>
        //    {
        //        var fileViewmodel = new FileViewModel
        //        {
        //            Content = ConvertHtmlToPdfNew(Htmlcontent),
        //            //Content= ConvertHtmlToPdf(fo.cont),
        //            Extension = "application/pdf",
        //            FileName = "Policy Information.pdf"
        //        };
        //        return fileViewmodel;
        //    }, "covertopdf");
        //    // return result;
        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    // Content is the file 
        //    var fileName = "output.pdf";//@"C:\Users\banso\Pranay\KcK\KcK\SchoolManagement.Website\Rotativa\output.pdf";
        //    var path = Server.MapPath("~/Rotativa/") + fileName;//@"C:\Users\banso\Pranay\KcK\KcK\SchoolManagement.Website\Rotativa\output.pdf";

        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }


        //    var stream = new MemoryStream(result.Content);
        //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        Byte[] title = stream.ToArray();
        //        fileStream.Write(title, 0, title.Length);
        //    }
        //    return fileName;
        //}




        #endregion
        //[HttpPost]
        //public string covertopdf(string Htmlcontent)
        ////public FileStreamResult covertopdf(file fo)
        //{
        //    var result = ExecuteAction(() =>
        //    {
        //        var fileViewmodel = new FileViewModel
        //        {
        //            Content = ConvertHtmlToPdf(Htmlcontent),
        //            //Content= ConvertHtmlToPdf(fo.cont),
        //            Extension = "application/pdf",
        //            FileName = "Policy Information.pdf"
        //        };
        //        return fileViewmodel;
        //    }, "covertopdf");
        //    // return result;
        //    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
        //    // Content is the file 
        //    var fileName = "output.pdf";
        //    var path = Server.MapPath("~/Rotativa/") + fileName;//@"C:
        //    if (System.IO.File.Exists(path))
        //    {
        //        System.IO.File.Delete(path);
        //    }

        //    var stream = new MemoryStream(result.Content);
        //    using (FileStream fileStream = new FileStream(path, FileMode.Create))
        //    {
        //        Byte[] title = stream.ToArray();
        //        fileStream.Write(title, 0, title.Length);
        //    }
        //    return fileName;
        //}
        public T ExecuteAction<T>(Func<T> action, string method)
        {
            try
            {
                return action.Invoke();
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }
        protected byte[] ConvertHtmlToPdfNew(string html, string header = null, string footer = null, bool isPageNumberInFooter = false)
        {
            using (MemoryStream outputMemoryStream = new MemoryStream())
            {
                // Create document
                using (Document document = new Document())
                {
                    // Create PDFWriter
                    PdfWriter writer = PdfWriter.GetInstance(document, outputMemoryStream);

                    // Open the document
                    document.Open();

                    // Add header if provided
                    if (!string.IsNullOrEmpty(header))
                    {
                        Paragraph headerParagraph = new Paragraph(header);
                        document.Add(headerParagraph);
                    }

                    // Convert HTML to PDF
                    using (StringReader htmlReader = new StringReader(html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, htmlReader);
                    }

                    // Add footer if provided
                    if (!string.IsNullOrEmpty(footer))
                    {
                        StringBuilder footerBuilder = new StringBuilder();
                        for (int i = 1; i <= writer.PageNumber; i++)
                        {
                            if (isPageNumberInFooter)
                            {
                                footerBuilder.Append(footer.Replace("PageNumber", "Page " + i.ToString() + " of " + writer.PageNumber.ToString()));
                            }
                            else
                            {
                                footerBuilder.Append(footer);
                            }
                            footerBuilder.Append("\n");
                        }
                        Paragraph footerParagraph = new Paragraph(footerBuilder.ToString());
                        document.Add(footerParagraph);
                    }

                    // Close the document
                    document.Close();
                }

                // Get PDF as byte array
                byte[] pdfBytes = outputMemoryStream.ToArray();
                return pdfBytes;
            }
        }
        protected byte[] ConvertHtmlToPdf(string html, string header = null, string footer = null, bool isPageNumberInFooter = false)
        {
            // Create ABCpdf Doc object
            var doc = new WebSupergoo.ABCpdf11.Doc();
            if (header == null && footer == null)
                doc.Rect.Inset(20, 10);
            else
                doc.Rect.String = "0 70 600 760"; /*padding from left, padding from bottom, width from left, height from bottom*/
            // Add html to Doc   
            //html = "<html><head></head><body></body></html>";
            int theId = doc.AddImageHtml(html);

            // Loop through document to create multi-page PDF
            while (true)
            {
                if (!doc.Chainable(theId))
                    break;
                doc.Page = doc.AddPage();
                theId = doc.AddImageToChain(theId);
            }
            var count = doc.PageCount;

            /*****************Footer area******************/
            if (footer != null)
            {
                var newfooter = "";
                doc.Rect.String = "40 20 580 50";
                for (int i = 1; i <= count; i++)
                {

                    doc.PageNumber = i;
                    if (isPageNumberInFooter)
                    {
                        newfooter = footer.Replace("PageNumber", "Page " + i.ToString() + " of " + count.ToString());
                        int id = doc.AddImageHtml(newfooter);

                        while (true)
                        {
                            if (!doc.Chainable(id))
                                break;
                            id = doc.AddImageToChain(id);
                        }
                    }
                    else
                        doc.AddText(footer);
                }
            }
            /*****************Footer area******************/


            // Flatten the PDF
            for (int i = 1; i <= doc.PageCount; i++)
            {
                doc.PageNumber = i;
                doc.Flatten();
            }

            var pdf = doc.GetData();
            doc.Clear();
            // Get PDF as byte array. Couls also use .Save() to save to disk
            return pdf;
        }
        //protected byte[] ConvertHtmlToPdf(string html, string header = null, string footer = null, bool isPageNumberInFooter = false)
        //{
        //    // Create ABCpdf Doc object
        //    var doc = new WebSupergoo.ABCpdf11.Doc();
        //    if (header == null && footer == null)
        //        doc.Rect.Inset(20, 10);
        //    else
        //        doc.Rect.String = "0 70 600 760"; /*padding from left, padding from bottom, width from left, height from bottom*/
        //                                          // Add html to Doc   
        //                                          //html = "<html><head></head><body></body></html>";
        //    int theId = doc.AddImageHtml(html);

        //    // Loop through document to create multi-page PDF
        //    while (true)
        //    {
        //        if (!doc.Chainable(theId))
        //            break;
        //        doc.Page = doc.AddPage();
        //        theId = doc.AddImageToChain(theId);
        //    }
        //    var count = doc.PageCount;

        //    /*****************Footer area******************/
        //    if (footer != null)
        //    {
        //        var newfooter = "";
        //        doc.Rect.String = "40 20 580 50";
        //        for (int i = 1; i <= count; i++)
        //        {

        //            doc.PageNumber = i;
        //            if (isPageNumberInFooter)
        //            {
        //                newfooter = footer.Replace("PageNumber", "Page " + i.ToString() + " of " + count.ToString());
        //                int id = doc.AddImageHtml(newfooter);

        //                while (true)
        //                {
        //                    if (!doc.Chainable(id))
        //                        break;
        //                    id = doc.AddImageToChain(id);
        //                }
        //            }
        //            else
        //                doc.AddText(footer);
        //        }
        //    }
        //    /*****************Footer area******************/


        //    // Flatten the PDF
        //    for (int i = 1; i <= doc.PageCount; i++)
        //    {
        //        doc.PageNumber = i;
        //        doc.Flatten();
        //    }

        //    var pdf = doc.GetData();
        //    doc.Clear();
        //    // Get PDF as byte array. Couls also use .Save() to save to disk
        //    return pdf;
        //}

        //public ActionResult PrintReportCard()
        //{
        //    try
        //    {

        //        return View();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        public ActionResult PrintReportCard(string id, int batchId)
        {
            try
            {
                var school = _context.TblCreateSchool.FirstOrDefault();
                var filename = System.IO.Path.GetFileName(school.Upload_Image);
                var base64Image = ConvertImageToBase64(Server.MapPath("~/WebsiteImages/SchoolImage/" + filename));
                Batch_Id = batchId;
                ViewBag.School_logo = base64Image;
                ViewBag.SchoolNewName = school.School_Name;
                ViewBag.current_Year = _context.Tbl_Batches.Where(x => x.Batch_Id == batchId).Select(x => x.Batch_Name).FirstOrDefault().Split('-')[0];
                ViewBag.newAddress = school.Address;

                return View(new StudentReportData());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public ActionResult PrintReportCardCBSEBoard(string id, int batchId)
        {
            try
            {
                var school = _context.TblCreateSchool.FirstOrDefault();
                var filename = System.IO.Path.GetFileName(school.Upload_Image);
                var base64Image = ConvertImageToBase64(Server.MapPath("~/WebsiteImages/SchoolImage/" + filename));
                Batch_Id = batchId;
                ViewBag.School_logo = base64Image;
                ViewBag.SchoolNewName = school.School_Name;
                ViewBag.current_Year = _context.Tbl_Batches.Where(x => x.Batch_Id == batchId).Select(x => x.Batch_Name).FirstOrDefault();
                ViewBag.newAddress = school.Address;

                return View(new StudentReportData());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string ConvertImageToBase64(string imagePath)
        {
            byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageArray);
            return base64Image;
        }
        public JsonResult PrintReportCardData(int studentId, int termId, int batchId, string _Name = "1") //, int classId
        {
            try
            {
                Batch_Id = batchId;
                var stdInfo = new Tbl_TestRecords();
                if (Batch_Id > 0)
                {
                    stdInfo = _context.Tbl_TestRecord.Where(x => x.BatchId == Batch_Id && x.StudentID == studentId).FirstOrDefault();
                }
                var studentInfo = _context.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                var studentRegister = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var familyDetail = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                if (_Name == "2")
                {
                    familyDetail = _context.FamilyDetails.Where(x => x.StudentRefId == studentId).FirstOrDefault();
                }
                var staffId = _context.Subjects.Where(x => x.Class_Id == stdInfo.ClassID && x.Batch_Id == Batch_Id && x.Section_Id == stdInfo.SectionID && x.Class_Teacher == true).Select(x => x.StaffId).FirstOrDefault();


                List<Tbl_StudentAttendance> ActualAttendance = new List<Tbl_StudentAttendance>();
                if (termId != 10)
                {
                    var batch = _context.Tbl_Batches.Where(x => x.Batch_Id == Batch_Id).FirstOrDefault();
                    //  var batchItems = _context.DataListItems.Where(x => x.DataListId == "9" && x.DataListItemName== batch.Batch_Name).FirstOrDefault();
                    var attendanceDate = _context.TblTestAssignDate.Where(x => x.TestID == termId && x.BatchID == batch.Batch_Id && x.ClassID == stdInfo.ClassID).FirstOrDefault();
                    var StartDate = DateTime.Now; var ToDate = DateTime.Now;
                    if (attendanceDate == null)
                    {
                        StartDate = DateTime.Now;
                        ToDate = DateTime.Now;
                    }
                    else
                    {
                        StartDate = Convert.ToDateTime(attendanceDate.StartDate);
                        ToDate = Convert.ToDateTime(attendanceDate.ToDate);
                    }
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID &&
                x.Class_Id == stdInfo.ClassID && x.Section_Id == stdInfo.SectionID).ToList().Where(x =>
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= StartDate.Date &&
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= ToDate.Date).ToList();

                }
                else
                {
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID && x.Class_Id == stdInfo.ClassID && x.Section_Id == stdInfo.SectionID).ToList();

                }

                double attendedDays = 0;
                double attendedHalfDays = 0;
                foreach (var item in ActualAttendance)
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
                //m double totalAttendedDays = attendedDays + (attendedHalfDays / 2);

                int totalAttendedDays = Convert.ToInt32(attendedDays + (attendedHalfDays / 2));

                StudentReportData studentReportData = new StudentReportData()
                {
                    studentName = studentInfo.Name,
                    LastName = studentInfo.Last_Name,
                    fatherName = familyDetail.FatherName,
                    motherName = familyDetail.MotherName,
                    scholarNo = studentInfo.ScholarNo.ToString(),
                    rollNo = studentInfo.RollNo.ToString(),
                    className = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID).Select(x => x.DataListItemName).FirstOrDefault(),
                    sectionName = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.SectionID).Select(x => x.DataListItemName).FirstOrDefault(),
                    dateOfBirth = studentInfo.DOB,
                    academicYear = _context.Tbl_Batches.Where(x => x.Batch_Id == stdInfo.BatchId).Select(x => x.Batch_Name).FirstOrDefault(),
                    studentID = studentInfo.StudentId,
                    attandence = totalAttendedDays + "/" + ActualAttendance.Count(),
                    promotedClass = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID + 1).Select(x => x.DataListItemName).FirstOrDefault(),
                    staffSignatureLink = _context.StafsDetails.Where(x => x.StafId == staffId).Select(x => x.StaffSignatureFile).FirstOrDefault(),
                    Remark = _context.tbl_Remark.Where(x => x.StudentId == stdInfo.StudentID && x.BatchId == Batch_Id && (x.TermId == termId || (termId == 10 && x.TermId == 4) || (termId == 7 && x.TermId == 8))).Select(x => x.Remark).FirstOrDefault(),
                    classID = stdInfo.ClassID,
                    Rank = _context.Tbl_TestRecord.Where(x => x.StudentID == stdInfo.StudentID && x.BatchId == Batch_Id && x.TermID == termId && x.ClassID == studentInfo.Class_Id).Select(x => x.RankInClass).FirstOrDefault().ToString()
                };
                var AllSubject = (from subj in _context.tbl_ClassSubject
                                  join test in _context.tbl_Tests
                                  on subj.SubjectId equals test.SubjectID
                                  where test.ClassID == stdInfo.ClassID && subj.ClassId == stdInfo.ClassID && (termId == 10 || test.TermID == termId) && test.IsOptional == false
                                  select subj).Distinct().ToList();


                var electiveSubjectId = AllSubject.Where(x => x.IsElective == true).ToList();
                if (electiveSubjectId.Count > 0)
                {
                    var subjectsToRemove = new List<long>();
                    foreach (var item in electiveSubjectId)
                    {
                        var isAssignedSubject = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == stdInfo.StudentID && x.ElectiveSubjectId == item.SubjectId).FirstOrDefault();
                        if (isAssignedSubject == null)
                        {
                            subjectsToRemove.Add(item.SubjectId);
                        }

                    }
                    AllSubject.RemoveAll(subj => subjectsToRemove.Contains(subj.SubjectId));

                }
                var Tests = _context.tbl_Tests.ToList();


                Tbl_TestRecords obtainedTheoryMarksT1 = null;
                Tbl_TestRecords obtainedPracticalMarksT1 = null;
                Tbl_TestRecords obtainedTheoryMarksT2 = null;
                Tbl_TestRecords obtainedPracticalMarksT2 = null;
                Tbl_TestRecords obtainedUT1Marks = null;
                Tbl_TestRecords obtainedSelectionTheoryMarks = null;
                Tbl_TestRecords obtainedSelectionPracticalMarks = null;
                Tbl_TestRecords obtainedPromotionTheoryMarks = null;
                Tbl_TestRecords obtainedPromotionPracticalMarks = null;

                Tbl_TestRecords obtainedUT2Marks = null;
                decimal theroyMaxMark = 1; decimal practicalMaxMark = 1; decimal theroyTotalMark = 0;
                decimal practicalTotalMark = 0; decimal UT1MaxMark = 10; decimal UT2MaxMark = 1;
                decimal Term1TheoryMaxMark = 0; decimal Term1PracticalMaxMark = 0; decimal Term2TheoryMaxMark = 0;
                decimal Term2PracticalMaxMark = 0; decimal OptionalUT1MaxMark = 1; decimal OptionalUT2MaxMark = 1; decimal TotalObtainedMarks = 0;
                decimal Tem1total = 0; decimal OptionalSelectioMaxMark = 0; decimal SelectionMaxMark = 1;
                decimal OptionalPromotionaMaxMark = 0; decimal PromotionalMaxMark = 1;
                //Add Pre-1 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre1 = null; Tbl_TestRecords obtainedPracticalMarksPre1 = null;
                decimal Pre1TheoryMaxMark = 0; decimal Pre1PracticalMaxMark = 0;

                Tbl_TestRecords obtainedTheoryMarksPromotion = null; Tbl_TestRecords obtainedPracticalMarksPromotion = null;
                decimal SelectionTheoryMaxMark = 0; decimal SelectionPracticalMaxMark = 0;
                Tbl_TestRecords obtainedTheoryMarksSelection = null; Tbl_TestRecords obtainedPracticalMarksSelection = null;
                decimal PromotionTheoryMaxMark = 0; decimal PromotionPracticalMaxMark = 0;

                //Add Pre-2 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre2 = null; Tbl_TestRecords obtainedPracticalMarksPre2 = null;
                decimal Pre2TheoryMaxMark = 0; decimal Pre2PracticalMaxMark = 0;
                //changes by Atul Kumar
                var terms = new List<Tbl_Term>();
                if (termId == 10)
                {
                    terms = _context.tbl_Term.ToList();
                }
                else
                {
                    terms = _context.tbl_Term.Where(x => x.TermID == termId).ToList();
                }


                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                var grades = _context.gradingCriteria.Where(x => x.BoardID == getBoardID).ToList();
                var count = 1;
                List<SubjectData> subjectDatas = new List<SubjectData>();
                foreach (var item in AllSubject)
                {
                    SubjectData subjectData = new SubjectData();
                    foreach (var termItem in terms)
                    {
                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID).ToList();

                        if (test.Count > 0)
                        {
                            foreach (var testItem in test)
                            {
                                if (termItem.TermID == 1)//UT1
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 1 && cog.TestID == testItem.TestID
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();
                                    //&& cr.BatchId == stdInfo.BatchId
                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                    };
                                    OptionalUT1MaxMark = testItem.MaximumMarks;
                                    obtainedUT1Marks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    UT1MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += (obtainedUT1Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0);
                                }
                                if (_Name == "2")
                                {
                                    if (termItem.TermID == 3)//UT2
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        OptionalUT2MaxMark = testItem.MaximumMarks;

                                        obtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        UT2MaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedUT2Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0);

                                    }
                                    if (termItem.TermID == 2)//Term1
                                    {

                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term1TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);
                                            //Tem1total+=(obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term1PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);
                                            /*                    Tem1total+= (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);*/
                                        }

                                    }
                                    if (termItem.TermID == 4)//Term2
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term2TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term2PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0);


                                        }
                                    }
                                }
                                else
                                {
                                    if (termItem.TermID == 2)//UT2
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        OptionalUT2MaxMark = testItem.MaximumMarks;

                                        obtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        UT2MaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedUT2Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0);

                                    }
                                    if (termItem.TermID == 3)//Term1
                                    {

                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term1TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);
                                            //Tem1total+=(obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term1PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);
                                            /*                    Tem1total+= (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);*/
                                        }

                                    }
                                    if (termItem.TermID == 4)//Term2
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term2TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Term2PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0);


                                        }
                                    }

                                }

                                if (termItem.TermID == 6)//Preboard
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedPromotionTheoryMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        PromotionTheoryMaxMark = testItem.MaximumMarks;
                                        theroyMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPromotionTheoryMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0);
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;obtainedPromotionTheoryMarks
                                        obtainedPromotionPracticalMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        PromotionPracticalMaxMark = testItem.MaximumMarks;
                                        practicalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPromotionPracticalMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0);
                                    }
                                }
                                if (termItem.TermID == 5)//Preboard
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 5 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedSelectionTheoryMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        SelectionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSelectionTheoryMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0);
                                        theroyMaxMark = testItem.MaximumMarks;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 5 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedSelectionPracticalMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        SelectionMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSelectionPracticalMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0);
                                        practicalMaxMark = testItem.MaximumMarks;
                                    }
                                }
                                if (termId != 10)
                                {
                                    if (termItem.TermID == 7)//PreBoard1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre1TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksPre1?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre1PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0);


                                        }
                                    }
                                    if (termItem.TermID == 8)//PreBoard2
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre2TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksPre2?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre2PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0);


                                        }
                                    }

                                }

                            }

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();

                            subjectData.MarksUT1 = obtainedUT1Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT1 = UT1MaxMark;

                            subjectData.MarksUT1Grade = GetGradebyTermBatch(PercentageCal(obtainedUT1Marks?.ObtainedMarks ?? 0, OptionalUT1MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            subjectData.MaxMarksUT1 = UT1MaxMark;

                            subjectData.MarksUT1Grade = GetGradebyTermBatch(PercentageCal(obtainedUT1Marks?.ObtainedMarks ?? 0, OptionalUT1MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarks = obtainedTheoryMarksT1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarks = obtainedPracticalMarksT1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksTerm1Practical = Term1PracticalMaxMark;
                            subjectData.MaxMarksTerm1Theory = Term1TheoryMaxMark;
                            subjectData.MaxMarksTerm2Practical = Term2PracticalMaxMark;
                            subjectData.MaxMarksTerm2Theory = Term2TheoryMaxMark;
                            //m
                            subjectData.TotalObtainedMarks = ((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0));

                            ////m
                            //                            subjectData.TotalObtainedMarks = ((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0))+
                            //                                ((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));
                            var divisor = (Term1PracticalMaxMark + Term1TheoryMaxMark) == 0 ? 1 : (Term1PracticalMaxMark + Term1TheoryMaxMark);
                            subjectData.GradeUT1 = GetGradebyTermBatch(((((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT1?.ObtainedMarks ?? 0)) / (divisor)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            //subjectData.GradeSelection = GetGrade(((((obtainedSelectionMarks?.ObtainedMarks ?? 0) ) / (divisor)) * 100), Convert.ToInt32(stdInfo.ClassID));
                            subjectData.MarksUT2 = obtainedUT2Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT2 = UT2MaxMark;
                            subjectData.MarksUT2Grade = GetGradebyTermBatch(PercentageCal(obtainedUT2Marks?.ObtainedMarks ?? -1, OptionalUT2MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            subjectData.TotalMarks =
                             ((obtainedUT1Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0)) +
                             ((obtainedUT2Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0));

                            //                        subjectData.TheoryMarksUT2 = obtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            //                        subjectData.PracticalMarksUT2 = obtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            //                        subjectData.TotalObtainedMarksUT2 =
                            //((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) +
                            //((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));

                            //megha comment

                            subjectData.TheoryMarksUT2 = obtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksUT2 = obtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarksUT2 =
    ((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) +
    ((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));
                            var divisor1 = (Term2PracticalMaxMark + Term2TheoryMaxMark) == 0 ? 1 : (Term2PracticalMaxMark + Term2TheoryMaxMark);
                            //subjectData.GradeUT2 = GetGrade((((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) / (divisor1)) * 100);
                            subjectData.GradeUT2 = GetGradebyTermBatch(((((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) / (divisor1)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            // end comment


                            //subjectData.MarksSelection = obtainedSelectionMarks?.ObtainedMarks ?? -2;
                            //subjectData.MaxMarksSelection = SelectionMaxMark;

                            //var divisorSelection = (SelectionMaxMark) == 0 ? 1 : (SelectionMaxMark);
                            //subjectData.GradeSelection = GetGrade(((((obtainedSelectionMarks?.ObtainedMarks ?? 0)) / (divisorSelection)) * 100), Convert.ToInt32(stdInfo.ClassID));

                            subjectData.TotalMarksBothUTs = TotalObtainedMarks;
                            subjectData.FinalGrade = GetGradebyTermBatch(((TotalObtainedMarks / 240) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksSelection = obtainedSelectionTheoryMarks?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksSelection = obtainedSelectionPracticalMarks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksSelectionPractical = SelectionPracticalMaxMark;
                            subjectData.MaxMarksSelectionTheory = SelectionTheoryMaxMark;
                            subjectData.TotalObtainedMarksSelection = ((obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0)) + ((obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0));

                            var divisorSelection = (SelectionPracticalMaxMark + SelectionTheoryMaxMark) == 0 ? 1 : (SelectionPracticalMaxMark + SelectionTheoryMaxMark);
                            subjectData.GradeSelection = GetGradebyTermBatch(((((obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0) + (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0)) / (divisorSelection)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksPromotion = obtainedPromotionTheoryMarks?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPromotion = obtainedPromotionPracticalMarks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPromotionPractical = PromotionPracticalMaxMark;
                            subjectData.MaxMarksPromotionTheory = PromotionTheoryMaxMark;
                            subjectData.TotalObtainedMarksPromotion = ((obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0)) + ((obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0));
                            var divisorPromotion = (PromotionTheoryMaxMark + PromotionTheoryMaxMark) == 0 ? 1 : (PromotionPracticalMaxMark + PromotionTheoryMaxMark);
                            subjectData.GradePromotion = GetGradebyTermBatch(((((obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0) + (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0)) / (divisorPromotion)) * 100), Convert.ToInt32(stdInfo.ClassID), Convert.ToInt32(termId), batchId);


                            //var subjects = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID.ToString() == "45").ToList();
                            ///  foreach (var subid in subjects)
                            // {







                            //// }
                            //subjectData.TotalMarksBothUTs = subjectData.TotalMarks;
                            //subjectData.FinalGrade = GetGrade((subjectData.TotalMarks / 240) * 100);

                            // Calculate Pre-1,Pre-2 Marks By Using Atul Kumar
                            subjectData.TheoryMarksPre1 = obtainedTheoryMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre1 = obtainedPracticalMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre1Practical = Pre1PracticalMaxMark;
                            subjectData.MaxMarksPre1Theory = Pre1TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre1 = ((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0));

                            var divisorPre = (Pre1PracticalMaxMark + Pre1TheoryMaxMark) == 0 ? 1 : (Pre1PracticalMaxMark + Pre1TheoryMaxMark);
                            subjectData.GradePre1 = GetGradebyTermBatch(((((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0)) / (divisorPre)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksPre2 = obtainedTheoryMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre2 = obtainedPracticalMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre2Practical = Pre2PracticalMaxMark;
                            subjectData.MaxMarksPre2Theory = Pre2TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre2 = ((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre2?.ObtainedMarks ?? 0)); /*megha((obtainedPracticalMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0));*/

                            var divisorPre2 = (Pre2PracticalMaxMark + Pre2TheoryMaxMark) == 0 ? 1 : (Pre2PracticalMaxMark + Pre2TheoryMaxMark);
                            subjectData.GradePre2 = GetGradebyTermBatch(((((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0)) / (divisorPre2)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            if (stdInfo.ClassID.ToString() == "207" || stdInfo.ClassID.ToString() == "208" || stdInfo.ClassID.ToString() == "209")
                            {
                                // foreach ( var subid in item.SubjectId.ToString())
                                // {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == item.SubjectId.ToString() && x.ClassID == stdInfo.ClassID).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorviva = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorviva += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }
                                var per = (Convert.ToDecimal(TotalObtainedMarks / finaldivisorviva) * 100);
                                decimal a = (Math.Round(Convert.ToDecimal(per), 1));
                                //  subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);
                                subjectData.FinalGrade = GetGradebyTermBatch(Convert.ToDecimal(a), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                                //subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);


                                //  }

                            }


                            if (item.SubjectId.ToString() == "45")
                            {
                                //  if (subid.Subject_ID.ToString() == "45")
                                //  {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "45" && x.ClassID == stdInfo.ClassID).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorviva = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorviva += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }


                                subjectData.FinalGrade = GetGradebyTermBatch((Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            }

                            // }
                            //megha


                            if (item.SubjectId.ToString() == "20")

                            {
                                var divisovias = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "20" && x.ClassID == stdInfo.ClassID).ToList();
                                var divisorfinalut1 = divisovias.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovias.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovias.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovias.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovias.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovias.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorvivas = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorvivas += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }

                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }



                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }

                                var per = (Convert.ToDecimal(TotalObtainedMarks / finaldivisorvivas) * 100);
                                decimal a = (Math.Round(Convert.ToDecimal(per), 1));
                                //  subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);
                                subjectData.FinalGrade = GetGradebyTermBatch(Convert.ToDecimal(a), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            }



                        }
                        //megha
                        else
                        {

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault(); ;
                            subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            subjectData.MaxMarksUT1 = UT1MaxMark;
                            //subjectData.MarksSelection = subjectData.MarksSelection == -1 ? -1 : subjectData.MarksSelection;
                            //subjectData.MaxMarksSelection = SelectionMaxMark;
                            subjectData.MarksUT2 = subjectData.MarksUT2 == -1 ? -1 : subjectData.MarksUT2;
                            subjectData.MaxMarksUT2 = UT2MaxMark;
                            subjectData.MarksUT1Grade = subjectData.MarksUT1Grade == "D" ? "D" : subjectData.MarksUT1Grade;
                            subjectData.MarksUT2Grade = subjectData.MarksUT2Grade == "D" ? "D" : subjectData.MarksUT2Grade;
                            subjectData.TotalMarks = subjectData.TotalMarks == -1 ? -1 : subjectData.TotalMarks;
                            subjectData.TheoryMarks = subjectData.TheoryMarks == -1 ? -1 : subjectData.TheoryMarks;
                            subjectData.PracticalMarks = subjectData.PracticalMarks == -1 ? -1 : subjectData.PracticalMarks;
                            subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == -1 ? -1 : subjectData.TotalObtainedMarks;
                            subjectData.GradeUT1 = subjectData.GradeUT1 == "D" ? "D" : subjectData.GradeUT1;
                            subjectData.TheoryMarksUT2 = subjectData.TheoryMarksUT2 == -1 ? -1 : subjectData.TheoryMarksUT2;
                            subjectData.PracticalMarksUT2 = subjectData.PracticalMarksUT2 == -1 ? -1 : subjectData.PracticalMarksUT2;
                            subjectData.TotalObtainedMarksUT2 = subjectData.TotalObtainedMarksUT2 == -1 ? -1 : subjectData.TotalObtainedMarksUT2;
                            subjectData.GradeUT2 = subjectData.GradeUT2 == "D" ? "D" : subjectData.GradeUT2;
                            subjectData.TotalMarksBothUTs = subjectData.TotalMarksBothUTs == -1 ? -1 : subjectData.TotalMarksBothUTs;
                            subjectData.FinalGrade = subjectData.FinalGrade == "D" ? "D" : subjectData.FinalGrade;
                            //Pre1,2 Add By Atul Kumar
                            subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                            subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                            subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;
                            subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                            subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                            subjectData.TheoryMarksSelection = subjectData.TheoryMarksSelection == -1 ? -1 : subjectData.TheoryMarksSelection;
                            subjectData.PracticalMarksSelection = subjectData.PracticalMarksSelection == -1 ? -1 : subjectData.PracticalMarksSelection;
                            subjectData.GradeSelection = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;

                            subjectData.TheoryMarksPromotion = subjectData.TheoryMarksPromotion == -1 ? -1 : subjectData.TheoryMarksPromotion;
                            subjectData.PracticalMarksPromotion = subjectData.PracticalMarksPromotion == -1 ? -1 : subjectData.PracticalMarksPromotion;
                            subjectData.GradePromotion = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;
                            //subjectData.GradeUT2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;

                        }
                    }

                    TotalObtainedMarks = 0; subjectDatas.Add(subjectData); obtainedTheoryMarksT1 = null;
                    obtainedPracticalMarksT1 = null; obtainedTheoryMarksT2 = null; obtainedPracticalMarksT2 = null;
                    obtainedUT1Marks = null; obtainedUT2Marks = null; theroyMaxMark = 1;
                    practicalMaxMark = 1; theroyTotalMark = 0; practicalTotalMark = 0;
                    UT1MaxMark = 1; UT2MaxMark = 1; Term1TheoryMaxMark = 0;
                    Term1PracticalMaxMark = 0; Term2TheoryMaxMark = 0; Term2PracticalMaxMark = 0;
                    OptionalUT1MaxMark = 1; OptionalUT2MaxMark = 1;
                    obtainedTheoryMarksPre1 = null; obtainedPracticalMarksPre1 = null; obtainedTheoryMarksPre2 = null;
                    obtainedPracticalMarksPre2 = null; Pre1TheoryMaxMark = 0; Pre1PracticalMaxMark = 0;
                    Pre2TheoryMaxMark = 0; Pre2PracticalMaxMark = 0; obtainedTheoryMarksPromotion = null;
                    obtainedPracticalMarksPromotion = null; PromotionTheoryMaxMark = 0; PromotionPracticalMaxMark = 0;
                    obtainedTheoryMarksSelection = null;
                    obtainedPracticalMarksSelection = null; SelectionTheoryMaxMark = 0; SelectionPracticalMaxMark = 0;
                }

                //for optional subject
                Tbl_TestRecords NewobtainedTheoryMarksT1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksT2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT2 = null;
                Tbl_TestRecords NewobtainedUT1Marks = null;
                //Tbl_TestRecords NewobtainedSelectionMarks = null;
                Tbl_TestRecords NewobtainedUT2Marks = null;

                Tbl_TestRecords NewobtainedTheoryMarksPre1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksPre2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre2 = null;
                Tbl_TestRecords NewobtainedTheoryMarksSelection = null;
                Tbl_TestRecords NewobtainedPracticalMarksSelection = null;
                Tbl_TestRecords NewobtainedTheoryMarksPromotion = null;
                Tbl_TestRecords NewobtainedPracticalMarksPromotion = null;


                decimal NewtheroyMaxMark = 1;
                decimal NewpracticalMaxMark = 1;
                decimal NewtheroyTotalMark = 0;
                decimal NewpracticalTotalMark = 0;
                decimal NewUT1MaxMark = 1;
                //decimal NewSelectionMaxMark = 1;
                decimal NewUT2MaxMark = 1;
                decimal NewTerm1TheoryMaxMark = 1;
                decimal NewTerm1PracticalMaxMark = 1;
                decimal NewTerm2TheoryMaxMark = 1;
                decimal NewTerm2PracticalMaxMark = 1;
                decimal NewOptionalUT1MaxMark = 1;
                decimal NewOptionalUT2MaxMark = 1;
                decimal NewOptionalSelectionMaxMark = 1;
                decimal NewTotalObtainedMarks = 0;

                decimal NewPre1TheoryMaxMark = 1;
                decimal NewPre1PracticalMaxMark = 1;

                decimal NewSelectionTheoryMaxMark = 1;
                decimal NewSelectionPracticalMaxMark = 1;
                decimal NewPromotionTheoryMaxMark = 1;
                decimal NewPromotionPracticalMaxMark = 1;
                decimal NewPre2TheoryMaxMark = 1;
                decimal NewPre2PracticalMaxMark = 1;

                var AllOptionalSubject = (from subj in _context.tbl_ClassSubject
                                          join test in _context.tbl_Tests
                                          on subj.SubjectId equals test.SubjectID
                                          where test.ClassID == stdInfo.ClassID && subj.ClassId == stdInfo.ClassID && (termId == 10 || test.TermID == termId) && test.IsOptional == true
                                          select subj).Distinct().ToList();

                List<OptionalSubjectData> optionalsubjectDatas = new List<OptionalSubjectData>();
                foreach (var item in AllOptionalSubject)
                {
                    OptionalSubjectData subjectData = new OptionalSubjectData();
                    foreach (var termItem in terms)
                    {

                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID).ToList();

                        if (test.Count > 0)
                        {
                            foreach (var testItem in test)
                            {
                                if (termItem.TermID == 1)//UT1
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.TermID == 1 && cog.TestID == testItem.TestID
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();
                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                    };

                                    NewobtainedUT1Marks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    NewUT1MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += NewobtainedUT1Marks?.ObtainedMarks ?? 0;
                                }

                                if (_Name == "2")
                                {
                                    if (termItem.TermID == 3)//UT2
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewUT2MaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedUT2Marks?.ObtainedMarks ?? 0;

                                    }
                                    if (termItem.TermID == 2)//Term1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewTerm1TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksT1?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewTerm1PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksT1?.ObtainedMarks ?? 0;
                                        }

                                    }
                                }
                                else
                                {
                                    if (termItem.TermID == 2)//UT2
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewUT2MaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedUT2Marks?.ObtainedMarks ?? 0;

                                    }
                                    if (termItem.TermID == 3)//Term1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewTerm1TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksT1?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                                 join cog in _context.tbl_TestObtainedMark
                                                                 on cr.RecordID equals cog.RecordIDFK
                                                                 where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID
                                                                 select new
                                                                 {
                                                                     TestID = cog.TestID,
                                                                     ObtainedMarks = cog.ObtainedMarks
                                                                 }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewTerm1PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksT1?.ObtainedMarks ?? 0;
                                        }

                                    }

                                }
                                if (termItem.TermID == 4)//Term2
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewTerm2TheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksT2?.ObtainedMarks ?? 0;

                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewTerm2PracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksT2?.ObtainedMarks ?? 0;

                                    }
                                }
                                if (termItem.TermID == 5)
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 5 && cog.TestID == testItem.TestID
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksSelection = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewSelectionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksSelection?.ObtainedMarks ?? 0;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksSelection = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewSelectionPracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksSelection?.ObtainedMarks ?? 0;
                                    }
                                }
                                if (termItem.TermID == 6)
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksPromotion = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewPromotionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? 0;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksPromotion = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewPromotionPracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? 0;
                                    }
                                }
                                // Pre-1,2 Add By Atul kumar
                                if (termId != 10)
                                {
                                    if (termItem.TermID == 7)//Pre1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewPre1TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksPre1?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewPre1PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksPre1?.ObtainedMarks ?? 0;
                                        }

                                    }
                                    if (termItem.TermID == 8)//Pre8
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewPre2TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksPre2?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewPre2PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksPre2?.ObtainedMarks ?? 0;
                                        }

                                    }

                                }

                            }


                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();
                            subjectData.MarksUT1 = NewobtainedUT1Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT1 = NewUT1MaxMark;
                            subjectData.MarksUT1Grade = GetOptionMarkGrade(NewobtainedUT1Marks?.ObtainedMarks ?? -2);
                            subjectData.TheoryMarks = NewobtainedTheoryMarksT1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarks = NewobtainedPracticalMarksT1?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarks =
    ((NewobtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksT1?.ObtainedMarks ?? -1)) +
    ((NewobtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksT1?.ObtainedMarks ?? -1));

                            subjectData.GradeUT1 = GetOptionMarkGrade((NewobtainedTheoryMarksT1?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksT1?.ObtainedMarks ?? 0));
                            subjectData.MarksUT2 = NewobtainedUT2Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT2 = NewUT2MaxMark;
                            subjectData.MarksUT2Grade = GetOptionMarkGrade(NewobtainedUT2Marks?.ObtainedMarks ?? -2);
                            subjectData.TotalMarks =
      ((NewobtainedUT1Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedUT1Marks?.ObtainedMarks ?? -1)) +
      ((NewobtainedUT2Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedUT2Marks?.ObtainedMarks ?? -1));

                            subjectData.TheoryMarksUT2 = NewobtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksUT2 = NewobtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarksUT2 =
      ((NewobtainedTheoryMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksT2?.ObtainedMarks ?? -1)) +
      ((NewobtainedPracticalMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksT2?.ObtainedMarks ?? -1));

                            subjectData.GradeUT2 = GetOptionMarkGrade((NewobtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksT2?.ObtainedMarks ?? 0));


                            if (termId != 10)
                            {
                                //Pre-1,2 Add By Atul Kumar
                                subjectData.TheoryMarksPre1 = NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre1 = NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre1 =
          ((NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -1));

                                subjectData.GradePre1 = GetOptionMarkGrade((NewobtainedTheoryMarksPre1?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPre1?.ObtainedMarks ?? 0));

                                subjectData.TheoryMarksPre2 = NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre2 = NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre2 =
          ((NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -1));

                                subjectData.GradePre2 = GetOptionMarkGrade((NewobtainedTheoryMarksPre2?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPre2?.ObtainedMarks ?? 0));


                                subjectData.TheoryMarksSelection = NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksSelection = NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarkSelection =
          ((NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -1));

                                subjectData.GradeSelection = GetOptionMarkGrade((NewobtainedTheoryMarksSelection?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksSelection?.ObtainedMarks ?? 0));

                                subjectData.TheoryMarksPromotion = NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPromotion = NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarkPromotion =
          ((NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -1));

                                subjectData.GradePromotion = GetOptionMarkGrade((NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? 0));






                            }

                        }
                        else
                        {

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();
                            subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            subjectData.MaxMarksUT1 = subjectData.MaxMarksUT1 == -1 ? -1 : subjectData.MaxMarksUT1;
                            subjectData.MarksUT2 = subjectData.MarksUT2 == -1 ? -1 : subjectData.MarksUT2;
                            subjectData.MaxMarksUT2 = subjectData.MaxMarksUT2 == -1 ? -1 : subjectData.MaxMarksUT2;
                            subjectData.MarksUT1Grade = subjectData.MarksUT1Grade == "D" ? "D" : subjectData.MarksUT1Grade;
                            subjectData.MarksUT2Grade = subjectData.MarksUT2Grade == "D" ? "D" : subjectData.MarksUT2Grade;
                            subjectData.TotalMarks = subjectData.TotalMarks == -1 ? -1 : subjectData.TotalMarks;
                            subjectData.TheoryMarks = subjectData.TheoryMarks == -1 ? -1 : subjectData.TheoryMarks;
                            subjectData.PracticalMarks = subjectData.PracticalMarks == -1 ? -1 : subjectData.PracticalMarks;
                            subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == -1 ? -1 : subjectData.TotalObtainedMarks;
                            subjectData.GradeUT1 = subjectData.GradeUT1 == "D" ? "D" : subjectData.GradeUT1;
                            subjectData.TheoryMarksUT2 = subjectData.TheoryMarksUT2 == -1 ? -1 : subjectData.TheoryMarksUT2;
                            subjectData.PracticalMarksUT2 = subjectData.PracticalMarksUT2 == -1 ? -1 : subjectData.PracticalMarksUT2;
                            subjectData.TotalObtainedMarksUT2 = subjectData.TotalObtainedMarksUT2 == -1 ? -1 : subjectData.TotalObtainedMarksUT2;
                            subjectData.GradeUT2 = subjectData.GradeUT2 == "D" ? "D" : subjectData.GradeUT2;

                            if (termId != 10)
                            {
                                //Pre-1,2 Add By Atul Kumar
                                subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                                subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                                subjectData.TotalObtainedMarksPre1 = subjectData.TotalObtainedMarksPre1 == -1 ? -1 : subjectData.TotalObtainedMarksPre1;
                                subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;

                                subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                                subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                                subjectData.TotalObtainedMarksPre2 = subjectData.TotalObtainedMarksPre2 == -1 ? -1 : subjectData.TotalObtainedMarksPre2;
                                subjectData.GradePre2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;

                                subjectData.TheoryMarksSelection = subjectData.TheoryMarksSelection == -1 ? -1 : subjectData.TheoryMarksSelection;
                                subjectData.PracticalMarksSelection = subjectData.PracticalMarksSelection == -1 ? -1 : subjectData.PracticalMarksSelection;
                                subjectData.TotalObtainedMarkSelection = subjectData.TotalObtainedMarkSelection == -1 ? -1 : subjectData.TotalObtainedMarkSelection;
                                subjectData.GradeSelection = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;

                                subjectData.TheoryMarksPromotion = subjectData.TheoryMarksPromotion == -1 ? -1 : subjectData.TheoryMarksPromotion;
                                subjectData.PracticalMarksPromotion = subjectData.PracticalMarksPromotion == -1 ? -1 : subjectData.PracticalMarksPromotion;
                                subjectData.TotalObtainedMarkPromotion = subjectData.TotalObtainedMarkPromotion == -1 ? -1 : subjectData.TotalObtainedMarkPromotion;
                                subjectData.GradePromotion = subjectData.GradePromotion == "D" ? "D" : subjectData.GradePromotion;

                            }

                        }


                    }
                    TotalObtainedMarks = 0;
                    optionalsubjectDatas.Add(subjectData);
                    NewobtainedTheoryMarksT1 = null;
                    NewobtainedPracticalMarksT1 = null;
                    NewobtainedTheoryMarksT2 = null;
                    NewobtainedPracticalMarksT2 = null;
                    NewobtainedUT1Marks = null;
                    NewobtainedUT2Marks = null;
                    NewobtainedTheoryMarksPre1 = null;
                    //NewobtainedSelectionMarks = null;
                    NewobtainedPracticalMarksPre1 = null;
                    NewobtainedTheoryMarksPre2 = null;
                    NewobtainedPracticalMarksPre2 = null;

                    NewobtainedTheoryMarksSelection = null;
                    NewobtainedPracticalMarksSelection = null;
                    NewobtainedTheoryMarksPromotion = null;
                    NewobtainedPracticalMarksPromotion = null;
                }
                decimal UT1Total = 0; decimal UT1MaxTotal = 0; decimal UT2Total = 0; decimal UT2MaxTotal = 0;
                decimal Term1TheoryMaxTotal = 0; decimal Term1PracticalMaxTotal = 0; decimal Term2TheoryMaxTotal = 0;
                decimal Term2PracticalMaxTotal = 0; decimal UTAllTotal = 0; decimal TheoryTotalT1 = 0;
                decimal PracticalTotalT1 = 0; decimal T1AllTotal = 0; decimal TheoryTotalT2 = 0;
                decimal PracticalTotalT2 = 0; decimal T2AllTotal = 0; decimal OverallAllTotal = 0;

                decimal Pre1TheoryMaxTotal = 0; decimal Pre1PracticalMaxTotal = 0; decimal Pre2TheoryMaxTotal = 0;
                decimal Pre2PracticalMaxTotal = 0; decimal PreAllTotal = 0;
                decimal SelectionTheoryMaxTotal = 0; decimal SelectionPracticalMaxTotal = 0; decimal SelectioAllTotal = 0;
                decimal PromotionTheoryMaxTotal = 0; decimal PromotionPracticalMaxTotal = 0; decimal PromotioAllTotal = 0;
                decimal PromotionTheoryTotal = 0; decimal PromotionPracticalTotal = 0; decimal SelectionTheoryTotal = 0; decimal SelectionPracticalTotal = 0;

                foreach (var item in subjectDatas)
                {

                    UT1Total += (item.MarksUT1 == -1 || item.MarksUT1 == -2) ? 0 : item.MarksUT1;
                    UT1MaxTotal += (item.MaxMarksUT1 == -1 || item.MaxMarksUT1 == -2 /*|| item.Subject == "English Language   " || item.Subject == "English Dictation  " || item.Subject == "English Writing" || item.Subject == "Hindi Lang " || item.Subject == "Hindi Dictation " || item.Subject == "Hindi Writing" || item.Subject == "Math Written"*/) ? 0 : item.MaxMarksUT1;
                    UT2MaxTotal += (item.MaxMarksUT2 == -1 || item.MaxMarksUT2 == -2) ? 0 : item.MaxMarksUT2;
                    Term1TheoryMaxTotal += (item.MaxMarksTerm1Theory == -1 || item.MaxMarksTerm1Theory == -2) ? 0 : item.MaxMarksTerm1Theory;
                    Term1PracticalMaxTotal += (item.MaxMarksTerm1Practical == -1 || item.MaxMarksTerm1Practical == -2) ? 0 : item.MaxMarksTerm1Practical;
                    Term2TheoryMaxTotal += (item.MaxMarksTerm2Theory == -1 || item.MaxMarksTerm2Theory == -2) ? 0 : item.MaxMarksTerm2Theory;
                    Term2PracticalMaxTotal += (item.MaxMarksTerm2Practical == -1 || item.MaxMarksTerm2Practical == -2) ? 0 : item.MaxMarksTerm2Practical;
                    UT2Total += (item.MarksUT2 == -1 || item.MarksUT2 == -2) ? 0 : item.MarksUT2;
                    UTAllTotal += (item.TotalMarks == -1 || item.TotalMarks == -2) ? 0 : item.TotalMarks;
                    TheoryTotalT1 += (item.TheoryMarks == -1 || item.TheoryMarks == -2) ? 0 : item.TheoryMarks;
                    PracticalTotalT1 += (item.PracticalMarks == -1 || item.PracticalMarks == -2) ? 0 : item.PracticalMarks;
                    T1AllTotal += (item.TotalObtainedMarks == -1 || item.TotalObtainedMarks == -2) ? 0 : item.TotalObtainedMarks;
                    TheoryTotalT2 += (item.TheoryMarksUT2 == -1 || item.TheoryMarksUT2 == -2) ? 0 : item.TheoryMarksUT2;
                    PracticalTotalT2 += (item.PracticalMarksUT2 == -1 || item.PracticalMarksUT2 == -2) ? 0 : item.PracticalMarksUT2;
                    T2AllTotal += (item.TotalObtainedMarksUT2 == -1 || item.TotalObtainedMarksUT2 == -2) ? 0 : item.TotalObtainedMarksUT2;
                    OverallAllTotal += (item.TotalMarksBothUTs == -1 || item.TotalMarksBothUTs == -2) ? 0 : item.TotalMarksBothUTs;


                    if (termId != 10)
                    {
                        Pre1TheoryMaxTotal += (item.MaxMarksPre1Theory == -1 || item.MaxMarksPre1Theory == -2) ? 0 : item.MaxMarksPre1Theory;
                        Pre1PracticalMaxTotal += (item.PracticalMarksPre1 == -1 || item.PracticalMarksPre1 == -2) ? 0 : item.PracticalMarksPre1;
                        Pre1AllTotal += (item.TotalObtainedMarksPre1 == -1 || item.TotalObtainedMarksPre1 == -2) ? 0 : item.TotalObtainedMarksPre1;


                        Pre2TheoryMaxTotal += (item.MaxMarksPre2Theory == -1 || item.MaxMarksPre2Theory == -2) ? 0 : item.MaxMarksPre2Theory;
                        Pre2PracticalMaxTotal += (item.MaxMarksPre2Practical == -1 || item.MaxMarksPre2Practical == -2) ? 0 : item.MaxMarksPre2Practical;
                        Pre2AllTotal += (item.TotalObtainedMarksPre2 == -1 || item.TotalObtainedMarksPre2 == -2) ? 0 : item.TotalObtainedMarksPre2;

                        SelectionTheoryTotal += (item.TheoryMarksSelection == -1 || item.TheoryMarksSelection == -2) ? 0 : item.TheoryMarksSelection;
                        PromotionPracticalTotal += (item.PracticalMarksSelection == -1 || item.PracticalMarksSelection == -2) ? 0 : item.PracticalMarksSelection;
                        SelectionTheoryMaxTotal += (item.MaxMarksSelectionTheory == -1 || item.MaxMarksSelectionTheory == -2) ? 0 : item.MaxMarksSelectionTheory;
                        SelectionPracticalMaxTotal += (item.PracticalMarksSelection == -1 || item.PracticalMarksSelection == -2) ? 0 : item.PracticalMarksSelection;
                        SelectionAllTotal += (item.TotalObtainedMarksSelection == -1 || item.TotalObtainedMarksSelection == -2) ? 0 : item.TotalObtainedMarksSelection;

                        PromotionTheoryTotal += (item.TheoryMarksPromotion == -1 || item.TheoryMarksPromotion == -2) ? 0 : item.TheoryMarksPromotion;
                        PromotionPracticalTotal += (item.PracticalMarksPromotion == -1 || item.PracticalMarksPromotion == -2) ? 0 : item.PracticalMarksPromotion;
                        PromotionTheoryMaxTotal += (item.MaxMarksPromotionTheory == -1 || item.MaxMarksPromotionTheory == -2) ? 0 : item.MaxMarksPromotionTheory;
                        PromotionPracticalMaxTotal += (item.MaxMarksPromotionPractical == -1 || item.MaxMarksPromotionPractical == -2) ? 0 : item.MaxMarksPromotionPractical;
                        PromotionAllTotal += (item.TotalObtainedMarksPromotion == -1 || item.TotalObtainedMarksPromotion == -2) ? 0 : item.TotalObtainedMarksPromotion;

                        //PromotionAllTotal += (item.TotalMarks == -1 || item.TotalMarks == -2) ? 0 : item.TotalMarks;

                    }
                    //2  
                }
                var divisor01 = (Term1TheoryMaxTotal + Term1PracticalMaxTotal) == 0 ? 1 : (Term1TheoryMaxTotal + Term1PracticalMaxTotal);
                var divisor02 = (Term2TheoryMaxTotal + Term2PracticalMaxTotal) == 0 ? 1 : (Term2TheoryMaxTotal + Term2PracticalMaxTotal);

                var divisorPre01 = (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal) == 0 ? 1 : (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal);
                var divisorPre02 = (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal) == 0 ? 1 : (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal);
                var divisor0Selection = (SelectionTheoryMaxTotal + SelectionPracticalMaxTotal) == 0 ? 1 : (SelectionTheoryMaxTotal + SelectionPracticalMaxTotal);
                var divisor0Promotion = (PromotionTheoryMaxTotal + PromotionPracticalMaxTotal) == 0 ? 1 : (PromotionTheoryMaxTotal + PromotionPracticalMaxTotal);
                var T1Gradee = GetGradebyTermBatch(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                TotalResult totalResult = new TotalResult()
                {
                    UT1Total = UT1Total,
                    UT1MaxTotal = UT1MaxTotal,
                    UT2Total = UT2Total,
                    UT2MaxTotal = UT2MaxTotal,
                    UT1TotalGrade = GetGradebyTermBatch(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UT2TotalGrade = GetGradebyTermBatch(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UTAllTotal = UTAllTotal,
                    TheoryTotalT1 = TheoryTotalT1,
                    PracticalTotalT1 = PracticalTotalT1,
                    T1AllTotal = T1AllTotal,
                    T1Grade = GetGradebyTermBatch(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    TheoryTotalT2 = TheoryTotalT2,
                    PracticalTotalT2 = PracticalTotalT2,
                    T2AllTotal = T2AllTotal,
                    T2Grade = GetGradebyTermBatch(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    OverallAllTotal = OverallAllTotal,
                    OverallGrade = GetGradebyTermBatch(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)), Convert.ToInt32(stdInfo.ClassID), termId, batchId),

                    Term1TheoryMaxTotal = Term1TheoryMaxTotal,
                    //SelectionMaxTotal = SelectionMaxTotal,
                    Term1PracticalMaxTotal = Term1PracticalMaxTotal,
                    Term2TheoryMaxTotal = Term2TheoryMaxTotal,
                    Term2PracticalMaxTotal = Term2PracticalMaxTotal


                };

                var totals =
                      (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal + SelectionTheoryMaxTotal + SelectionPracticalMaxTotal + PromotionTheoryMaxTotal + PromotionPracticalMaxTotal);
                //totalResult.  OverallGrade = GetGrade(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)), Convert.ToInt32(studentInfo.Class_Id));


                TotalResultpercentage totalResultpercentage = new TotalResultpercentage()
                {
                    UT1Total = PercentageCal(UT1Total, UT1MaxTotal),
                    UT1TotalGrade = GetGradebyTermBatch(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),

                    UT2Total = PercentageCal(UT2Total, UT2MaxTotal),
                    UT2TotalGrade = GetGradebyTermBatch(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UTAllTotal = PercentageCal(UTAllTotal, UT1MaxTotal + UT2MaxTotal),
                    TheoryTotalT1 = PercentageCal(TheoryTotalT1, Term1TheoryMaxTotal),
                    PracticalTotalT1 = PercentageCal(PracticalTotalT1, Term1PracticalMaxTotal == 0 ? 1 : Term1PracticalMaxTotal),
                    T1AllTotal = PercentageCal(T1AllTotal, divisor01),
                    T1Grade = GetGradebyTermBatch(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    TheoryTotalT2 = PercentageCal(TheoryTotalT2, Term2TheoryMaxTotal),
                    PracticalTotalT2 = PercentageCal(PracticalTotalT2, Term2PracticalMaxTotal == 0 ? 1 : Term2PracticalMaxTotal),
                    T2AllTotal = PercentageCal(T2AllTotal, divisor02),
                    T2Grade = GetGradebyTermBatch(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    OverallAllTotal = Math.Round(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal)), 1),
                    OverallGrade = GetGradebyTermBatch(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal + SelectionPracticalMaxTotal + SelectionTheoryMaxTotal + PromotionPracticalMaxTotal + PromotionTheoryMaxTotal)), Convert.ToInt32(stdInfo.ClassID), termId, batchId)


                };
                var persoverall = PercentageCal(Convert.ToDecimal(totalResult.OverallAllTotal), Convert.ToDecimal(totals));
                var perst = Math.Round(Convert.ToDecimal(persoverall), 1);
                totalResult.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(perst), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                var pers = Math.Round(Convert.ToDecimal(totalResultpercentage.OverallAllTotal), 1);
                totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(pers), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                if (stdInfo.ClassID.ToString() == "205")
                {
                    var overallpers = PercentageCal(totalResult.OverallAllTotal, 2630);
                    var perstoverall = Math.Round(Convert.ToDecimal(overallpers), 1);
                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(perstoverall), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = Math.Round(Convert.ToDecimal(totalResultpercentage.OverallAllTotal), 1);
                    totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                }


                if (stdInfo.ClassID.ToString() == "208")//kg
                {

                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 880), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                }

                if (stdInfo.ClassID.ToString() == "209")
                {
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 980), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                }



                if (stdInfo.ClassID.ToString() == "207")
                {

                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 680), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                }

                //if (totalResultpercentage.OverallGrade == "0 " || totalResultpercentage.OverallGrade == null)
                //{
                //    totalResultpercentage.OverallGrade = "D";
                //}
                var validGrade = false;
                if (termId != 10)
                {
                    totalResult.Pre1Grade = GetGradebyTermBatch(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.Pre1AllTotal = Pre1AllTotal;
                    totalResult.Pre2AllTotal = Pre2AllTotal;
                    totalResult.Pre2Grade = GetGradebyTermBatch(PercentageCal(Pre2AllTotal, divisorPre02), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.Pre1TheoryMaxTotal = Pre1AllTotal;
                    totalResult.Pre1PracticalMaxTotal = Pre1PracticalMaxTotal;
                    totalResult.Pre2TheoryMaxTotal = Pre2AllTotal;
                    totalResult.Pre2PracticalMaxTotal = Pre2PracticalMaxTotal;


                    totalResult.SelectionGrade = GetGradebyTermBatch(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.SelectionAllTotal = SelectionAllTotal;
                    totalResult.SelectionTheoryMaxTotal = SelectionAllTotal;
                    totalResult.SelectionPracticalMaxTotal = SelectionPracticalMaxTotal;


                    totalResult.PromotionGrade = GetGradebyTermBatch(PercentageCal(PromotionAllTotal, divisor0Promotion), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.PromotionAllTotal = PromotionAllTotal;
                    totalResult.PromotionTheoryMaxTotal = PromotionTheoryTotal;
                    totalResult.PromotionPracticalMaxTotal = PromotionPracticalTotal;
                    //totalResult.SelectionGrade = GetGrade(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID));
                    //totalResult.SelectionAlltotal = SelectionAllTotal;

                    totalResultpercentage.TheoryTotalPre1 = PercentageCal(Pre1AllTotal, Pre1TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre1 = PercentageCal(Pre1PracticalMaxMark, Pre1PracticalMaxTotal == 0 ? 1 : Pre1PracticalMaxTotal);
                    totalResultpercentage.Pre1AllTotal = PercentageCal(Pre1AllTotal, divisorPre01);
                    totalResultpercentage.Pre1Grade = GetGradebyTermBatch(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalPre2 = PercentageCal(Pre2AllTotal, Pre2TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre2 = PercentageCal(Pre2PracticalMaxMark, Pre2PracticalMaxTotal == 0 ? 1 : Pre2PracticalMaxTotal);
                    totalResultpercentage.Pre2AllTotal = PercentageCal(Pre2AllTotal, divisorPre01);
                    totalResultpercentage.Pre2Grade = GetGradebyTermBatch(PercentageCal(Pre2AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalSelection = PercentageCal(SelectionTheoryTotal, SelectionTheoryMaxTotal);
                    totalResultpercentage.PracticalTotalSelection = PercentageCal(SelectionPracticalTotal, SelectionPracticalMaxTotal == 0 ? 1 : SelectionPracticalMaxTotal);
                    totalResultpercentage.SelectionAllTotal = PercentageCal(SelectionAllTotal, divisor0Selection);
                    totalResultpercentage.SelectionGrade = GetGradebyTermBatch(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalPromotion = PercentageCal(PromotionTheoryTotal, PromotionTheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPromotion = PercentageCal(PromotionPracticalTotal, PromotionPracticalMaxTotal == 0 ? 1 : PromotionPracticalMaxTotal);
                    totalResultpercentage.PromotionAllTotal = PercentageCal(PromotionAllTotal, divisor0Promotion);
                    totalResultpercentage.PromotionGrade = GetGradebyTermBatch(PercentageCal(PromotionAllTotal, divisor0Promotion), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    if (termId == 7)
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarksPre1 <= 32);
                    }
                    else if (termId == 8)
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarksPre2 <= 32);
                    }
                    else
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarks <= 32);
                    }
                }

                if (termId == 7 || termId == 8 || termId == 10)
                {
                    if (validGrade)
                    {
                        totalResult.T1Grade = "";
                        totalResult.T2Grade = "";
                        totalResult.UT1TotalGrade = "";
                        totalResult.UT2TotalGrade = "";
                        totalResult.Pre1Grade = "";
                        totalResult.Pre2Grade = "";
                        totalResult.OverallGrade = "";
                        totalResultpercentage.T1Grade = "";
                        totalResultpercentage.T2Grade = "";
                        totalResultpercentage.UT1TotalGrade = "";
                        totalResultpercentage.UT2TotalGrade = "";
                        totalResultpercentage.Pre1Grade = "";
                        totalResultpercentage.Pre2Grade = "";
                        totalResultpercentage.OverallGrade = "";

                    }
                    else
                    {
                        totalResult.T1Grade = totalResult.T1Grade == "D" ? "" : totalResult.T1Grade;
                        totalResult.T2Grade = totalResult.T2Grade == "D" ? "" : totalResult.T2Grade;
                        totalResult.UT1TotalGrade = totalResult.UT1TotalGrade == "D" ? "" : totalResult.UT1TotalGrade;
                        totalResult.UT2TotalGrade = totalResult.UT2TotalGrade == "D" ? "" : totalResult.UT2TotalGrade;
                        totalResultpercentage.T1Grade = totalResultpercentage.T1Grade == "D" ? "" : totalResultpercentage.T1Grade;
                        totalResultpercentage.T2Grade = totalResultpercentage.T2Grade == "D" ? "" : totalResultpercentage.T2Grade;
                        totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT1TotalGrade == "D" ? "" : totalResultpercentage.UT1TotalGrade;
                        totalResultpercentage.UT2TotalGrade = totalResultpercentage.UT2TotalGrade == "D" ? "" : totalResultpercentage.UT2TotalGrade;

                        ////m
                        //totalResultpercentage.fin = totalResultpercentage.OverallGrade == "0" ? "D" : totalResultpercentage.OverallGrade;
                        totalResultpercentage.OverallGrade = totalResultpercentage.OverallGrade == "" ? "D" : totalResultpercentage.OverallGrade;
                        //m end

                        totalResult.Pre1Grade = totalResult.Pre1Grade == "D" ? "" : totalResult.Pre1Grade;
                        totalResult.Pre2Grade = totalResult.Pre2Grade == "D" ? "" : totalResult.Pre2Grade;
                        totalResultpercentage.Pre1Grade = totalResultpercentage.Pre1Grade == "D" ? "" : totalResultpercentage.Pre1Grade;
                        totalResultpercentage.Pre2Grade = totalResultpercentage.Pre2Grade == "D" ? "" : totalResultpercentage.Pre2Grade;
                        totalResult.SelectionGrade = totalResult.SelectionGrade == "D" ? "" : totalResult.SelectionGrade;
                        totalResultpercentage.SelectionGrade = totalResultpercentage.SelectionGrade == "D" ? "" : totalResultpercentage.SelectionGrade;
                        totalResult.PromotionGrade = totalResult.PromotionGrade == "D" ? "" : totalResult.PromotionGrade;
                        totalResultpercentage.PromotionGrade = totalResultpercentage.PromotionGrade == "D" ? "" : totalResultpercentage.PromotionGrade;
                    }
                }
                else
                {

                    //if (!validGrade)
                    //{
                    //    totalResult.T1Grade = "";
                    //    totalResult.T2Grade = "";
                    //    totalResult.UT1TotalGrade = "";
                    //    totalResult.UT2TotalGrade = "";
                    //    totalResult.Pre1Grade = "";
                    //    totalResult.Pre2Grade = "";
                    //    totalResult.OverallGrade = "";
                    //    totalResultpercentage.T1Grade = "";
                    //    totalResultpercentage.T2Grade = "";
                    //    totalResultpercentage.UT1TotalGrade = "";
                    //    totalResultpercentage.UT2TotalGrade = "";
                    //    totalResultpercentage.Pre1Grade = "";
                    //    totalResultpercentage.Pre2Grade = "";
                    //    totalResultpercentage.OverallGrade = "";

                    //}
                    //else
                    //{
                    //    totalResult.T1Grade = totalResult.T1Grade == "D" ? "" : totalResult.T1Grade;
                    //    totalResult.T2Grade = totalResult.T2Grade == "D" ? "" : totalResult.T2Grade;
                    //    totalResult.UT1TotalGrade = totalResult.UT1TotalGrade == "D" ? "" : totalResult.UT1TotalGrade;
                    //    totalResult.UT2TotalGrade = totalResult.UT2TotalGrade == "D" ? "" : totalResult.UT2TotalGrade;
                    //    totalResultpercentage.T1Grade = totalResultpercentage.T1Grade == "D" ? "" : totalResultpercentage.T1Grade;
                    //    totalResultpercentage.T2Grade = totalResultpercentage.T2Grade == "D" ? "" : totalResultpercentage.T2Grade;
                    //    totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT1TotalGrade == "D" ? "" : totalResultpercentage.UT1TotalGrade;
                    //    totalResultpercentage.UT2TotalGrade = totalResultpercentage.UT2TotalGrade == "D" ? "" : totalResultpercentage.UT2TotalGrade;

                    //    totalResult.Pre1Grade = totalResult.Pre1Grade == "D" ? "" : totalResult.Pre1Grade;
                    //    totalResult.Pre2Grade = totalResult.Pre2Grade == "D" ? "" : totalResult.Pre2Grade;
                    //    totalResultpercentage.Pre1Grade = totalResultpercentage.Pre1Grade == "D" ? "" : totalResultpercentage.Pre1Grade;
                    //    totalResultpercentage.Pre2Grade = totalResultpercentage.Pre2Grade == "D" ? "" : totalResultpercentage.Pre2Grade;
                    //}
                }


                //Step 1 Working
                foreach (var subjectData in subjectDatas)
                {
                    // Check if any of the properties MarksUT1, MarksUT2, TotalMarks, TheoryMarks, PracticalMarks, 
                    // TotalObtainedMarks, TheoryMarksUT2, PracticalMarksUT2, or TotalMarksBothUTs is equal to 0
                    if (subjectData.MarksUT1 == 0 && subjectData.MarksUT2 == 0 && subjectData.TotalMarks == 0 &&
                        subjectData.TheoryMarks == 0 && subjectData.PracticalMarks == 0 && subjectData.TotalObtainedMarks == 0 &&
                        subjectData.TheoryMarksUT2 == 0 && subjectData.PracticalMarksUT2 == 0 && subjectData.TotalMarksBothUTs == 0 &&
                        subjectData.TheoryMarksPre1 == 0 && subjectData.PracticalMarksPre1 == 0 && subjectData.TheoryMarksPre2 == 0 &&
                        subjectData.PracticalMarksPre2 == 0 && subjectData.TotalObtainedMarksPre1 == 0 && subjectData.TotalObtainedMarksPre2 == 0 &&
                        subjectData.TheoryMarksSelection == 0 && subjectData.PracticalMarksSelection == 0 && subjectData.TheoryMarksPromotion == 0 && subjectData.PracticalMarksPromotion == 0)
                    {
                        // Set the GradeUT1, GradeUT2, and FinalGrade properties to "AB" (Absent)
                        subjectData.GradeUT1 = "D";
                        subjectData.GradeUT2 = "D";
                        subjectData.FinalGrade = "D";
                        subjectData.GradePre1 = "D";
                        subjectData.GradePre2 = "D";
                        //m
                        // subjectData.FinalGrade = "D";

                    }
                }


                // Check if any of the properties UT1Total, UT2Total, UTAllTotal, TheoryTotalT1, PracticalTotalT1,
                // T1AllTotal, TheoryTotalT2, PracticalTotalT2, T2AllTotal, or OverallAllTotal is equal to 0
                if (totalResult.UT1Total == 0 && totalResult.UT2Total == 0 && totalResult.UTAllTotal == 0 &&
                    totalResult.TheoryTotalT1 == 0 && totalResult.PracticalTotalT1 == 0 && totalResult.T1AllTotal == 0 &&
                    totalResult.TheoryTotalT2 == 0 && totalResult.PracticalTotalT2 == 0 && totalResult.T2AllTotal == 0 &&
                    totalResult.OverallAllTotal == 0)
                {
                    // Set the T1Grade, T2Grade, and OverallGrade properties to "AB" (Absent)
                    totalResult.T1Grade = "D";
                    totalResult.T2Grade = "D";
                    totalResult.OverallGrade = "D";

                }

                if (termId == 7)
                {
                    if (totalResult.Pre1TheoryMaxTotal == 0 && totalResult.Pre1AllTotal == 0)
                    {
                        totalResult.Pre1Grade = "D";
                    }
                }
                if (termId == 7)
                {
                    if (totalResult.Pre2TheoryMaxTotal == 0 && totalResult.Pre2AllTotal == 0)
                    {
                        totalResult.Pre2Grade = "D";
                    }
                }
                List<CoscholasticAreaData> coscholasticAreaDatas = new List<CoscholasticAreaData>();

                var classCoscholastic = _context.tbl_CoScholasticClass.Where(x => x.ClassID == stdInfo.ClassID).Select(x => x.CoscholasticID).ToList();
                var CoscholasticMatchingRecords = _context.tbl_CoScholastic
                .Where(record => classCoscholastic.Contains(record.Id))
                .ToList();


                var resultTermUT1 = (from cr in _context.tbl_CoScholastic_Result
                                     join cog in _context.tbl_CoScholasticObtainedGrade
                                     on cr.Id equals cog.ObtainedCoScholasticID
                                     join c in _context.tbl_CoScholastic
                                     on cog.CoscholasticID equals c.Id
                                     where cr.StudentID == studentId && cr.TermID == 1 && cog.BatchId == Batch_Id
                                     select new
                                     {
                                         CoscholasticID = c.Id,
                                         Title = c.Title,
                                         ObtainedGrade = cog.ObtainedGrade
                                     }).ToList();
                var resultTermUT_1 = (from c in CoscholasticMatchingRecords
                                      join cr in _context.tbl_CoScholastic_Result
                                      on c.Id equals cr.CoScholasticID into crGroup
                                      from cr in crGroup.DefaultIfEmpty()
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                      from cog in cogGroup.DefaultIfEmpty()
                                      where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 1 && cog.BatchId == Batch_Id)
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                      }).ToList();


                var resultTermUT2 = (from cr in _context.tbl_CoScholastic_Result
                                     join cog in _context.tbl_CoScholasticObtainedGrade
                                     on cr.Id equals cog.ObtainedCoScholasticID
                                     join c in _context.tbl_CoScholastic
                                     on cog.CoscholasticID equals c.Id
                                     where cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == Batch_Id
                                     select new
                                     {
                                         CoscholasticID = c.Id,
                                         Title = c.Title,
                                         ObtainedGrade = cog.ObtainedGrade
                                     }).ToList();
                var resultTermUT_2 = (from c in CoscholasticMatchingRecords
                                      join cr in _context.tbl_CoScholastic_Result
                                      on c.Id equals cr.CoScholasticID into crGroup
                                      from cr in crGroup.DefaultIfEmpty()
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                      from cog in cogGroup.DefaultIfEmpty()
                                      where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == Batch_Id)
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                      }).ToList();


                var resultTerm0 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == Batch_Id
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                var resultTerm1 = (from c in CoscholasticMatchingRecords
                                   join cr in _context.tbl_CoScholastic_Result
                                   on c.Id equals cr.CoScholasticID into crGroup
                                   from cr in crGroup.DefaultIfEmpty()
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                   from cog in cogGroup.DefaultIfEmpty()
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == Batch_Id)
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                   }).ToList();

                var resultTerm3 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 4 && cog.BatchId == Batch_Id
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                var resultTerm2 = (from c in CoscholasticMatchingRecords
                                   join cr in _context.tbl_CoScholastic_Result
                                   on c.Id equals cr.CoScholasticID into crGroup
                                   from cr in crGroup.DefaultIfEmpty()
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                   from cog in cogGroup.DefaultIfEmpty()
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 4 && cog.BatchId == Batch_Id)
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                   }).ToList();
                var resultTerm9 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == Batch_Id
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();

                if (_Name == "2")
                {
                    resultTermUT2 = (from cr in _context.tbl_CoScholastic_Result
                                     join cog in _context.tbl_CoScholasticObtainedGrade
                                     on cr.Id equals cog.ObtainedCoScholasticID
                                     join c in _context.tbl_CoScholastic
                                     on cog.CoscholasticID equals c.Id
                                     where cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == Batch_Id
                                     select new
                                     {
                                         CoscholasticID = c.Id,
                                         Title = c.Title,
                                         ObtainedGrade = cog.ObtainedGrade
                                     }).ToList();
                    resultTermUT_2 = (from c in CoscholasticMatchingRecords
                                      join cr in _context.tbl_CoScholastic_Result
                                      on c.Id equals cr.CoScholasticID into crGroup
                                      from cr in crGroup.DefaultIfEmpty()
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                      from cog in cogGroup.DefaultIfEmpty()
                                      where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == Batch_Id)
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                      }).ToList();


                    resultTerm0 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == Batch_Id
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                    resultTerm1 = (from c in CoscholasticMatchingRecords
                                   join cr in _context.tbl_CoScholastic_Result
                                   on c.Id equals cr.CoScholasticID into crGroup
                                   from cr in crGroup.DefaultIfEmpty()
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                   from cog in cogGroup.DefaultIfEmpty()
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == Batch_Id)
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                   }).ToList();
                    resultTerm9 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == Batch_Id
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                }

                List<CoscholasticResultModel> coscholasticResultModelsList1 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTerm9.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsList1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTerm9.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsList1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }


                List<CoscholasticResultModel> coscholasticResultModelsListUT1 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTermUT1.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsListUT1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTermUT1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsListUT1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }

                List<CoscholasticResultModel> coscholasticResultModelsListUT2 = new List<CoscholasticResultModel>();
                //foreach (var item in CoscholasticMatchingRecords)
                //{
                //    if (resultTermUT_1.Any(x => x.CoscholasticID == item.Id))
                //    {
                //        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                //        {
                //            CoscholasticID = item.Id,
                //            Title = item.Title,
                //            ObtainedGrade = resultTermUT_1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                //        });
                //    }
                //    else
                //    {
                //        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                //        {
                //            CoscholasticID = item.Id,
                //            Title = item.Title,
                //            ObtainedGrade = null
                //        });
                //    }
                //}
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTermUT2.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTermUT2.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }


                var resultTerm10 = (from cr in _context.tbl_CoScholastic_Result
                                    join cog in _context.tbl_CoScholasticObtainedGrade
                                    on cr.Id equals cog.ObtainedCoScholasticID
                                    join c in _context.tbl_CoScholastic
                                    on cog.CoscholasticID equals c.Id
                                    where cr.StudentID == studentId && cr.TermID == 4
                                    select new
                                    {
                                        CoscholasticID = c.Id,
                                        Title = c.Title,
                                        ObtainedGrade = cog.ObtainedGrade
                                    }).ToList();
                List<CoscholasticResultModel> coscholasticResultModelsList2 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTerm10.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsList2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTerm10.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsList2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }

                List<CoscholasticResultModel> coscholasticResultModelsList3 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList4 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList5 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList6 = new List<CoscholasticResultModel>();
                if (termId != 10)
                {
                    var resultPre1 = (from cr in _context.tbl_CoScholastic_Result
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr.Id equals cog.ObtainedCoScholasticID
                                      join c in _context.tbl_CoScholastic
                                      on cog.CoscholasticID equals c.Id
                                      where cr.StudentID == studentId && cr.TermID == 7
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog.ObtainedGrade
                                      }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultPre1.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList3.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultPre1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList3.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                    var resultPre2 = (from cr in _context.tbl_CoScholastic_Result
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr.Id equals cog.ObtainedCoScholasticID
                                      join c in _context.tbl_CoScholastic
                                      on cog.CoscholasticID equals c.Id
                                      where cr.StudentID == studentId && cr.TermID == 8
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog.ObtainedGrade
                                      }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultPre2.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList4.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultPre2.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList4.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                    var resultpromotion = (from cr in _context.tbl_CoScholastic_Result
                                           join cog in _context.tbl_CoScholasticObtainedGrade
                                           on cr.Id equals cog.ObtainedCoScholasticID
                                           join c in _context.tbl_CoScholastic
                                           on cog.CoscholasticID equals c.Id
                                           where cr.StudentID == studentId && cr.TermID == 6
                                           select new
                                           {
                                               CoscholasticID = c.Id,
                                               Title = c.Title,
                                               ObtainedGrade = cog.ObtainedGrade
                                           }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultpromotion.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList6.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultpromotion.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }

                    var resultSelection = (from cr in _context.tbl_CoScholastic_Result
                                           join cog in _context.tbl_CoScholasticObtainedGrade
                                           on cr.Id equals cog.ObtainedCoScholasticID
                                           join c in _context.tbl_CoScholastic
                                           on cog.CoscholasticID equals c.Id
                                           where cr.StudentID == studentId && cr.TermID == 5
                                           select new
                                           {
                                               CoscholasticID = c.Id,
                                               Title = c.Title,
                                               ObtainedGrade = cog.ObtainedGrade
                                           }).ToList();
                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultSelection.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultSelection.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                }

                //if (termId != 10)
                //{

                //}
                var combinedResult1 = (from coscholasticId in classCoscholastic
                                       join term1 in coscholasticResultModelsList1
                                       on coscholasticId equals term1.CoscholasticID into term1Group
                                       join term2 in coscholasticResultModelsList2
                                       on coscholasticId equals term2.CoscholasticID into term2Group
                                       join Pre1 in coscholasticResultModelsList3
                                      on coscholasticId equals Pre1.CoscholasticID into Pre1Group
                                       join Pre2 in coscholasticResultModelsList4
                                      on coscholasticId equals Pre2.CoscholasticID into Pre2Group
                                       join UT1 in coscholasticResultModelsListUT1
                                       on coscholasticId equals UT1.CoscholasticID into UT1Group
                                       join UT2 in coscholasticResultModelsListUT2
                                      on coscholasticId equals UT2.CoscholasticID into UT2Group
                                       join Selection in coscholasticResultModelsList5
                                       on coscholasticId equals Selection.CoscholasticID into selectionGroup
                                       join Promotion in coscholasticResultModelsList6
                                       on coscholasticId equals Promotion.CoscholasticID into PrmotionGroup
                                       from UT1 in UT1Group.DefaultIfEmpty()
                                       from UT2 in UT2Group.DefaultIfEmpty()
                                       from term1 in term1Group.DefaultIfEmpty()
                                       from term2 in term2Group.DefaultIfEmpty()
                                       from Pre1 in Pre1Group.DefaultIfEmpty()
                                       from Selection in selectionGroup.DefaultIfEmpty()
                                       from Promotion in PrmotionGroup.DefaultIfEmpty()
                                       from Pre2 in Pre2Group.DefaultIfEmpty()
                                       select new
                                       {
                                           Title = UT1?.Title ?? UT2?.Title ?? term1?.Title ?? term2?.Title ?? Pre1?.Title ?? Pre2?.Title ?? Selection.Title ?? Promotion.Title,
                                           GradeTerm1 = term1?.ObtainedGrade,
                                           GradeTerm2 = term2?.ObtainedGrade,
                                           GradePre1 = Pre1?.ObtainedGrade,
                                           GradeSelection = Selection?.ObtainedGrade,
                                           GradePromotion = Promotion?.ObtainedGrade,
                                           GradePre2 = Pre2?.ObtainedGrade,
                                           GradeUT1 = UT1?.ObtainedGrade,
                                           GradeUT2 = UT2?.ObtainedGrade
                                       }).ToList();

                var combinedResult = (from coscholasticId in classCoscholastic
                                      join ut1 in resultTermUT_1
                                     on coscholasticId equals ut1.CoscholasticID into ut1Group
                                      from ut1 in ut1Group.DefaultIfEmpty()
                                      join ut2 in resultTermUT_2
                                     on coscholasticId equals ut2.CoscholasticID into ut2Group
                                      from ut2 in ut2Group.DefaultIfEmpty()
                                      join term1 in resultTerm1
                                      on coscholasticId equals term1.CoscholasticID into term1Group
                                      from term1 in term1Group.DefaultIfEmpty()
                                      join term2 in resultTerm2
                                      on coscholasticId equals term2.CoscholasticID into term2Group
                                      from term2 in term2Group.DefaultIfEmpty()
                                      select new
                                      {
                                          Title = term1?.Title ?? term2?.Title,
                                          GradeTerm1 = term1?.ObtainedGrade,
                                          GradeTerm2 = term2?.ObtainedGrade
                                      }).ToList();


                // Group the combined result based on CoscholasticID count
                // Group the combined result based on CoscholasticID count
                var groupedResult = combinedResult1.GroupBy(item => item.Title)
                                                  .Select(group => new
                                                  {
                                                      Title = group.Key,
                                                      GradeTerm1 = group.FirstOrDefault(item => item.GradeTerm1 != null)?.GradeTerm1,
                                                      GradeTerm2 = group.FirstOrDefault(item => item.GradeTerm2 != null)?.GradeTerm2,
                                                      GradePre1 = group.FirstOrDefault(item => item.GradePre1 != null)?.GradePre1,
                                                      GradePre2 = group.FirstOrDefault(item => item.GradePre2 != null)?.GradePre2,
                                                      GradeUT1 = group.FirstOrDefault(item => item.GradeUT1 != null)?.GradeUT1,
                                                      GradeUT2 = group.FirstOrDefault(item => item.GradeUT2 != null)?.GradeUT2,
                                                      GradeSelection = group.FirstOrDefault(item => item.GradeSelection != null)?.GradeSelection,
                                                      GradePromotion = group.FirstOrDefault(item => item.GradePromotion != null)?.GradePromotion
                                                  })
                                                  .ToList();


                foreach (var item in groupedResult)
                {
                    CoscholasticAreaData coscholasticAreaData = new CoscholasticAreaData()
                    {
                        Name = item.Title,
                        GradeTerm1 = item.GradeTerm1 ?? "-",
                        GradeTerm2 = item.GradeTerm2 ?? "-",
                        GradePre1 = item.GradePre1 ?? "-",
                        GradeSelection = item.GradeSelection ?? "-",
                        GradePromotion = item.GradePromotion ?? "-",
                        GradePre2 = item.GradePre2 ?? "-",
                        GradeUT1 = item.GradeUT1 ?? "-",
                        GradeUT2 = item.GradeUT2 ?? "-"
                    };
                    coscholasticAreaDatas.Add(coscholasticAreaData);
                }
                var gradinglist = _context.gradingCriteria.Where(x => x.TermID == termId && x.BatchID == Batch_Id && x.ClassID == stdInfo.ClassID).ToList();
                if (termId == 10)
                {
                    gradinglist = _context.gradingCriteria.Where(x => x.TermID == 4 && x.BatchID == Batch_Id && x.ClassID == stdInfo.ClassID).ToList();
                }

                studentReportData.gradingCriteria = gradinglist;

                studentReportData.coscholasticAreaDatas = coscholasticAreaDatas;
                studentReportData.totalResult = totalResult;
                studentReportData.totalResultPercentage = totalResultpercentage;
                studentReportData.subjectDatas = subjectDatas;
                studentReportData.optionalSubjectDatas = optionalsubjectDatas;
                studentReportData.totalResult.T1Grade = studentReportData.totalResult.T1Grade;
                studentReportData.totalResultPercentage.T1Grade = studentReportData.totalResultPercentage.T1Grade;
                string termName = _context.tbl_Term.Where(x => x.TermID == termId).Select(t => t.TermName).FirstOrDefault();
                int DgradeCountUT1 = subjectDatas.Count(x => x.MarksUT1Grade == "D");
                int DgradeCountUT2 = subjectDatas.Count(x => x.MarksUT2Grade == "D");
                int DgradeCountTerm1 = subjectDatas.Count(x => x.GradeUT1 == "D");
                int DgradeCountTerm2 = subjectDatas.Count(x => x.GradeUT2 == "D");
                int DgradeCountPre1 = subjectDatas.Count(x => x.GradePre1 == "D");
                int DgradeCountSelection = subjectDatas.Count(x => x.GradeSelection == "D");
                int DgradeCountPromotion = subjectDatas.Count(x => x.GradePromotion == "D");
                int DgradeCountPre2 = subjectDatas.Count(x => x.GradePre2 == "D");
                int DgradeCountFinal = subjectDatas.Count(x => x.FinalGrade == "D");
                string result;
                switch (termId)
                {
                    case 1:
                        result = DgradeCountUT1 > 0 ? "" : "Pass";
                        break;
                    case 2:
                        result = DgradeCountUT2 > 0 ? "" : "Pass";
                        break;
                    case 3:
                        result = DgradeCountTerm1 > 0 ? "" : "Pass";
                        break;
                    case 4:
                        result = DgradeCountTerm2 > 0 ? "" : "Pass";
                        break;
                    case 6:
                        result = DgradeCountPromotion > 0 ? "" : "Pass";
                        break;
                    case 5:
                        result = DgradeCountSelection > 0 ? "" : "Pass";
                        break;
                    case 7:
                        result = DgradeCountPre1 > 0 ? "" : "Pass";
                        break;
                    case 8:
                        result = DgradeCountPre2 > 0 ? "" : "Pass";
                        break;
                    case 10:
                        result = DgradeCountFinal > 0 ? "" : "Pass";
                        break;
                    default:
                        result = "Invalid grade";
                        break;
                }



                studentReportData.Result = result;
                return Json(studentReportData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult PrintReportCardDataForCBSEResult(int studentId, int termId, int batchId) //, int classId
        {
            try
            {
                Batch_Id = batchId;
                var stdInfo = new Tbl_TestRecords();
                if (Batch_Id > 0)
                {
                    stdInfo = _context.Tbl_TestRecord.Where(x => x.BatchId == Batch_Id && x.StudentID == studentId).FirstOrDefault();
                }
                var studentInfo = _context.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                var studentRegister = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var familyDetail = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var staffId = _context.Subjects.Where(x => x.Class_Id == stdInfo.ClassID && x.Batch_Id == Batch_Id && x.Section_Id == stdInfo.SectionID && x.Class_Teacher == true).Select(x => x.StaffId).FirstOrDefault();


                List<Tbl_StudentAttendance> ActualAttendance = new List<Tbl_StudentAttendance>();
                if (termId != 10)
                {
                    var batch = _context.Tbl_Batches.Where(x => x.Batch_Id == Batch_Id).FirstOrDefault();
                    //  var batchItems = _context.DataListItems.Where(x => x.DataListId == "9" && x.DataListItemName== batch.Batch_Name).FirstOrDefault();
                    var attendanceDate = _context.TblTestAssignDate.Where(x => x.TestID == termId && x.BatchID == batch.Batch_Id && x.ClassID == stdInfo.ClassID).FirstOrDefault();
                    var StartDate = DateTime.Now; var ToDate = DateTime.Now;
                    if (attendanceDate == null)
                    {
                        StartDate = DateTime.Now;
                        ToDate = DateTime.Now;
                    }
                    else
                    {
                        StartDate = Convert.ToDateTime(attendanceDate.StartDate);
                        ToDate = Convert.ToDateTime(attendanceDate.ToDate);
                    }
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID && x.BatchId == batchId &&
                x.Class_Id == stdInfo.ClassID && x.Section_Id == stdInfo.SectionID).ToList().Where(x =>
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= StartDate.Date &&
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= ToDate.Date).ToList();

                }
                else
                {
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID && x.Class_Id == stdInfo.ClassID && x.Section_Id == stdInfo.SectionID && x.BatchId == batchId).ToList();

                }

                double attendedDays = 0;
                double attendedHalfDays = 0;
                foreach (var item in ActualAttendance)
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
                //m double totalAttendedDays = attendedDays + (attendedHalfDays / 2);

                int totalAttendedDays = Convert.ToInt32(attendedDays + (attendedHalfDays / 2));

                StudentReportData studentReportData = new StudentReportData()
                {
                    studentName = studentInfo.Name + studentInfo.Last_Name,
                    fatherName = familyDetail.FatherName,
                    motherName = familyDetail.MotherName,
                    scholarNo = studentInfo.ScholarNo.ToString(),
                    rollNo = studentInfo.RollNo.ToString(),
                    className = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID).Select(x => x.DataListItemName).FirstOrDefault(),
                    sectionName = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.SectionID).Select(x => x.DataListItemName).FirstOrDefault(),
                    dateOfBirth = studentInfo.DOB,
                    academicYear = _context.Tbl_Batches.Where(x => x.Batch_Id == stdInfo.BatchId).Select(x => x.Batch_Name).FirstOrDefault(),
                    studentID = studentInfo.StudentId,
                    ProfileAvatar = studentInfo.ProfileAvatar ?? "",
                    attandence = totalAttendedDays + "/" + ActualAttendance.Count(),
                    promotedClass = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID + 1).Select(x => x.DataListItemName).FirstOrDefault(),
                    staffSignatureLink = _context.StafsDetails.Where(x => x.StafId == staffId).Select(x => x.StaffSignatureFile).FirstOrDefault(),
                    Remark = _context.tbl_Remark.Where(x => x.StudentId == stdInfo.StudentID && x.BatchId == Batch_Id && (x.TermId == termId || (termId == 10 && x.TermId == 4) || (termId == 7 && x.TermId == 8))).Select(x => x.Remark).FirstOrDefault(),
                    classID = stdInfo.ClassID
                };
                var AllSubject = (from subj in _context.tbl_ClassSubject
                                  join test in _context.tbl_Tests
                                  on subj.SubjectId equals test.SubjectID
                                  where test.ClassID == stdInfo.ClassID && subj.ClassId == stdInfo.ClassID && (termId == 10 || test.TermID == termId) && test.IsOptional == false
                                  select subj).Distinct().ToList();


                var electiveSubjectId = AllSubject.Where(x => x.IsElective == true).ToList();
                if (electiveSubjectId.Count > 0)
                {
                    var subjectsToRemove = new List<long>();
                    foreach (var item in electiveSubjectId)
                    {
                        var isAssignedSubject = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == stdInfo.StudentID && x.ElectiveSubjectId == item.SubjectId).FirstOrDefault();
                        if (isAssignedSubject == null)
                        {
                            subjectsToRemove.Add(item.SubjectId);
                        }

                    }
                    AllSubject.RemoveAll(subj => subjectsToRemove.Contains(subj.SubjectId));

                }
                var Tests = _context.tbl_Tests.ToList();


                Tbl_TestRecords obtainedTheoryMarksT1 = null;
                Tbl_TestRecords obtainedPracticalMarksT1 = null;
                Tbl_TestRecords obtainedProjectMarksT1 = null;
                Tbl_TestRecords obtainedSubjectEnrichMarksT1 = null;
                Tbl_TestRecords obtainedTheoryMarksT2 = null;
                Tbl_TestRecords obtainedPracticalMarksT2 = null;
                Tbl_TestRecords obtainedUT1Marks = null;
                Tbl_TestRecords obtainedSelectionTheoryMarks = null;
                Tbl_TestRecords obtainedSelectionPracticalMarks = null;
                Tbl_TestRecords obtainedPromotionTheoryMarks = null;
                Tbl_TestRecords obtainedPromotionPracticalMarks = null;
                Tbl_TestRecords obtainedProjectMarksT2 = null;
                Tbl_TestRecords obtainedSubjectEnrichMarksT2 = null;

                Tbl_TestRecords obtainedUT2Marks = null;
                decimal theroyMaxMark = 0; decimal practicalMaxMark = 0; decimal theroyTotalMark = 0;
                decimal practicalTotalMark = 0; decimal UT1MaxMark = 0; decimal UT2MaxMark = 0;
                decimal Term1TheoryMaxMark = 0; decimal Term1PracticalMaxMark = 0; decimal Term2TheoryMaxMark = 0;
                decimal Term2PracticalMaxMark = 0; decimal OptionalUT1MaxMark = 1; decimal OptionalUT2MaxMark = 0; decimal TotalObtainedMarks = 0;
                decimal Tem1total = 0; decimal OptionalSelectioMaxMark = 0; decimal SelectionMaxMark = 1;
                decimal Term1ProjectMaxMark = 0; decimal Term1SubjectEnrichMaxMark = 0;
                decimal Term2ProjectMaxMark = 0; decimal Term2SubjectEnrichMaxMark = 0;

                decimal projectMaxMark = 1; decimal subjectEnrichMaxMark = 0; decimal projectTotalMark = 0;
                decimal subjectEnrichTotalMark = 0;
                decimal OptionalPromotionaMaxMark = 0; decimal PromotionalMaxMark = 0;
                //Add Pre-1 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre1 = null; Tbl_TestRecords obtainedPracticalMarksPre1 = null;
                decimal Pre1TheoryMaxMark = 0; decimal Pre1PracticalMaxMark = 0;

                Tbl_TestRecords obtainedTheoryMarksPromotion = null; Tbl_TestRecords obtainedPracticalMarksPromotion = null;
                decimal SelectionTheoryMaxMark = 0; decimal SelectionPracticalMaxMark = 0;
                Tbl_TestRecords obtainedTheoryMarksSelection = null; Tbl_TestRecords obtainedPracticalMarksSelection = null;
                decimal PromotionTheoryMaxMark = 0; decimal PromotionPracticalMaxMark = 0;

                //Add Pre-2 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre2 = null; Tbl_TestRecords obtainedPracticalMarksPre2 = null;
                decimal Pre2TheoryMaxMark = 0; decimal Pre2PracticalMaxMark = 0;
                //changes by Atul Kumar
                var terms = new List<Tbl_Term>();
                if (termId == 10)
                {
                    terms = _context.tbl_Term.ToList();
                }
                else
                {
                    terms = _context.tbl_Term.Where(x => x.TermID == termId).ToList();
                }


                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                var grades = _context.gradingCriteria.Where(x => x.BoardID == getBoardID).ToList();
                var count = 1;
                List<SubjectData> subjectDatas = new List<SubjectData>();
                foreach (var item in AllSubject)
                {
                    SubjectData subjectData = new SubjectData();
                    foreach (var termItem in terms)
                    {
                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID).ToList();

                        if (test.Count > 0)
                        {
                            foreach (var testItem in test)
                            {
                                if (termItem.TermID == 1)//UT1
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 1 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();

                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                    };
                                    OptionalUT1MaxMark = testItem.MaximumMarks;
                                    obtainedUT1Marks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    UT1MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += (obtainedUT1Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0);
                                }
                                if (termItem.TermID == 3)//UT2
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();
                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                    };
                                    OptionalUT2MaxMark = testItem.MaximumMarks;

                                    obtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    UT2MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += (obtainedUT2Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0);

                                }
                                if (termItem.TermID == 2)//Term1
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term1TheoryMaxMark = testItem.MaximumMarks;
                                        theroyMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);
                                        //Tem1total+=(obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);

                                    }
                                    if (testItem.TestType == "Project")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedProjectMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term1ProjectMaxMark = testItem.MaximumMarks;
                                        projectMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedProjectMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedProjectMarksT1?.ObtainedMarks ?? 0);
                                        //Tem1total+=(obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);

                                    }
                                    if (testItem.TestType == "SubjectEnrich")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedSubjectEnrichMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term1SubjectEnrichMaxMark = testItem.MaximumMarks;
                                        subjectEnrichMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSubjectEnrichMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSubjectEnrichMarksT1?.ObtainedMarks ?? 0);
                                        //Tem1total+=(obtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0);

                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term1PracticalMaxMark = testItem.MaximumMarks;
                                        practicalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);
                                        /*                    Tem1total+= (obtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0);*/
                                    }
                                }
                                if (termItem.TermID == 4)//Term2
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedTheoryMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term2TheoryMaxMark = testItem.MaximumMarks;
                                        theroyMaxMark = testItem.MaximumMarks;

                                        TotalObtainedMarks += (obtainedTheoryMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0);

                                    }
                                    if (testItem.TestType == "Project")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedProjectMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term2ProjectMaxMark = testItem.MaximumMarks;
                                        projectMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedProjectMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedProjectMarksT2?.ObtainedMarks ?? 0);


                                    }
                                    if (testItem.TestType == "SubjectEnrich")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedSubjectEnrichMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term2SubjectEnrichMaxMark = testItem.MaximumMarks;
                                        subjectEnrichMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSubjectEnrichMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSubjectEnrichMarksT2?.ObtainedMarks ?? 0);
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? -1
                                        };
                                        obtainedPracticalMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        Term2PracticalMaxMark = testItem.MaximumMarks;
                                        practicalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPracticalMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0);


                                    }
                                }
                                if (termItem.TermID == 6)//Preboard
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 6 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedPromotionTheoryMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        PromotionTheoryMaxMark = testItem.MaximumMarks;
                                        theroyMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPromotionTheoryMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0);
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 6 && cog.TestID == testItem.TestID
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;obtainedPromotionTheoryMarks
                                        obtainedPromotionPracticalMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        PromotionPracticalMaxMark = testItem.MaximumMarks;
                                        practicalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedPromotionPracticalMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0);
                                    }
                                }
                                if (termItem.TermID == 5)//Preboard
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 5 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedSelectionTheoryMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        SelectionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSelectionTheoryMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0);
                                        theroyMaxMark = testItem.MaximumMarks;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentelectionMark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.ClassID == stdInfo.ClassID && cr.SectionID == stdInfo.SectionID && cr.TermID == 5 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();

                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentelectionMark?.ObtainedMarks ?? -1
                                        };
                                        //OptionalSelectioMaxMark = testItem.MaximumMarks;
                                        obtainedSelectionPracticalMarks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        SelectionMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += (obtainedSelectionPracticalMarks?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0);
                                        practicalMaxMark = testItem.MaximumMarks;
                                    }
                                }
                                if (termId != 10)
                                {
                                    if (termItem.TermID == 7)//PreBoard1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre1TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksPre1?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre1PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0);


                                        }
                                    }
                                    if (termItem.TermID == 8)//PreBoard2
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedTheoryMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre2TheoryMaxMark = testItem.MaximumMarks;
                                            theroyMaxMark = testItem.MaximumMarks;

                                            TotalObtainedMarks += (obtainedTheoryMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedTheoryMarksPre2?.ObtainedMarks ?? 0);

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? -1
                                            };
                                            obtainedPracticalMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            Pre2PracticalMaxMark = testItem.MaximumMarks;
                                            practicalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += (obtainedPracticalMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0);


                                        }
                                    }

                                }

                            }

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();

                            subjectData.MarksUT1 = obtainedUT1Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT1 = UT1MaxMark;

                            subjectData.MarksUT1Grade = GetGradebyTermBatch(PercentageCal(obtainedUT1Marks?.ObtainedMarks ?? 0, OptionalUT1MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            subjectData.MaxMarksUT1 = UT1MaxMark;

                            subjectData.MarksUT1Grade = GetGradebyTermBatch(PercentageCal(obtainedUT1Marks?.ObtainedMarks ?? 0, OptionalUT1MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarks = obtainedTheoryMarksT1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarks = obtainedPracticalMarksT1?.ObtainedMarks ?? -2;
                            subjectData.ProjectMarks = obtainedProjectMarksT1?.ObtainedMarks ?? -2;
                            subjectData.SubjectEnrichMarks = obtainedSubjectEnrichMarksT1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksTerm1Practical = Term1PracticalMaxMark;
                            subjectData.MaxMarksTerm1Theory = Term1TheoryMaxMark;
                            subjectData.MaxMarksTerm1Project = Term1ProjectMaxMark;
                            subjectData.MaxMarksTerm1SubjectEnrich = Term1SubjectEnrichMaxMark;
                            subjectData.MaxMarksTerm2Practical = Term2PracticalMaxMark;
                            subjectData.MaxMarksTerm2Theory = Term2TheoryMaxMark;
                            subjectData.MaxMarksTerm2Project = Term2ProjectMaxMark;
                            subjectData.MaxMarksTerm2SubjectEnrich = Term2SubjectEnrichMaxMark;
                            //m
                            subjectData.TotalObtainedMarks = ((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0)) + ((obtainedProjectMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedProjectMarksT1?.ObtainedMarks ?? 0)) + ((obtainedSubjectEnrichMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSubjectEnrichMarksT1?.ObtainedMarks ?? 0)) + ((obtainedUT1Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0));

                            ////m
                            //                            subjectData.TotalObtainedMarks = ((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0))+
                            //                                ((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));
                            var divisor = (Term1PracticalMaxMark + Term1TheoryMaxMark + Term1ProjectMaxMark + Term1SubjectEnrichMaxMark) == 0 ? 1 : (Term1PracticalMaxMark + Term1TheoryMaxMark + Term1ProjectMaxMark + Term1SubjectEnrichMaxMark);
                            subjectData.GradeUT1 = GetGradebyTermBatch(((((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT1?.ObtainedMarks ?? 0)) / (divisor)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            //subjectData.GradeSelection = GetGrade(((((obtainedSelectionMarks?.ObtainedMarks ?? 0) ) / (divisor)) * 100), Convert.ToInt32(stdInfo.ClassID));
                            subjectData.MarksUT2 = obtainedUT2Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT2 = UT2MaxMark;
                            subjectData.MarksUT2Grade = GetGradebyTermBatch(PercentageCal(obtainedUT2Marks?.ObtainedMarks ?? -1, OptionalUT2MaxMark), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            subjectData.TotalMarks =
                             ((obtainedUT1Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0)) +
                             ((obtainedUT2Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0));

                            //                        subjectData.TheoryMarksUT2 = obtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            //                        subjectData.PracticalMarksUT2 = obtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            //                        subjectData.TotalObtainedMarksUT2 =
                            //((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) +
                            //((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));

                            //megha comment

                            subjectData.TheoryMarksUT2 = obtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksUT2 = obtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            subjectData.ProjectMarksUT2 = obtainedProjectMarksT2?.ObtainedMarks ?? -2;
                            subjectData.SubjectEnrichMarksUT2 = obtainedSubjectEnrichMarksT2?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarksUT2 = ((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) + ((obtainedProjectMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedProjectMarksT2?.ObtainedMarks ?? 0)) + ((obtainedSubjectEnrichMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSubjectEnrichMarksT2?.ObtainedMarks ?? 0)) + ((obtainedUT2Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0));
                            var divisor1 = (Term2PracticalMaxMark + Term2TheoryMaxMark + Term2ProjectMaxMark + Term2SubjectEnrichMaxMark) == 0 ? 1 : (Term2PracticalMaxMark + Term2TheoryMaxMark + Term2ProjectMaxMark + Term2SubjectEnrichMaxMark);
                            //subjectData.GradeUT2 = GetGrade((((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) / (divisor1)) * 100);
                            subjectData.GradeUT2 = GetGradebyTermBatch(((((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) / (divisor1)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                            // end comment


                            //subjectData.MarksSelection = obtainedSelectionMarks?.ObtainedMarks ?? -2;
                            //subjectData.MaxMarksSelection = SelectionMaxMark;

                            //var divisorSelection = (SelectionMaxMark) == 0 ? 1 : (SelectionMaxMark);
                            //subjectData.GradeSelection = GetGrade(((((obtainedSelectionMarks?.ObtainedMarks ?? 0)) / (divisorSelection)) * 100), Convert.ToInt32(stdInfo.ClassID));

                            subjectData.TotalMarksBothUTs = TotalObtainedMarks;
                            subjectData.FinalGrade = GetGradebyTermBatch(((TotalObtainedMarks / 240) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksSelection = obtainedSelectionTheoryMarks?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksSelection = obtainedSelectionPracticalMarks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksSelectionPractical = SelectionPracticalMaxMark;
                            subjectData.MaxMarksSelectionTheory = SelectionTheoryMaxMark;
                            subjectData.TotalObtainedMarksSelection = ((obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0)) + ((obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0));

                            var divisorSelection = (SelectionPracticalMaxMark + SelectionTheoryMaxMark) == 0 ? 1 : (SelectionPracticalMaxMark + SelectionTheoryMaxMark);
                            subjectData.GradeSelection = GetGradebyTermBatch(((((obtainedSelectionTheoryMarks?.ObtainedMarks ?? 0) + (obtainedSelectionPracticalMarks?.ObtainedMarks ?? 0)) / (divisorSelection)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksPromotion = obtainedPromotionTheoryMarks?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPromotion = obtainedPromotionPracticalMarks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPromotionPractical = PromotionPracticalMaxMark;
                            subjectData.MaxMarksPromotionTheory = PromotionTheoryMaxMark;
                            subjectData.TotalObtainedMarksPromotion = ((obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0)) + ((obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0));
                            var divisorPromotion = (PromotionTheoryMaxMark + PromotionTheoryMaxMark) == 0 ? 1 : (PromotionPracticalMaxMark + PromotionTheoryMaxMark);
                            subjectData.GradePromotion = GetGradebyTermBatch(((((obtainedPromotionTheoryMarks?.ObtainedMarks ?? 0) + (obtainedPromotionPracticalMarks?.ObtainedMarks ?? 0)) / (divisorPromotion)) * 100), Convert.ToInt32(stdInfo.ClassID), Convert.ToInt32(termId), batchId);


                            //var subjects = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID.ToString() == "45").ToList();
                            ///  foreach (var subid in subjects)
                            // {







                            //// }
                            //subjectData.TotalMarksBothUTs = subjectData.TotalMarks;
                            //subjectData.FinalGrade = GetGrade((subjectData.TotalMarks / 240) * 100);

                            // Calculate Pre-1,Pre-2 Marks By Using Atul Kumar
                            subjectData.TheoryMarksPre1 = obtainedTheoryMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre1 = obtainedPracticalMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre1Practical = Pre1PracticalMaxMark;
                            subjectData.MaxMarksPre1Theory = Pre1TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre1 = ((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0));

                            var divisorPre = (Pre1PracticalMaxMark + Pre1TheoryMaxMark) == 0 ? 1 : (Pre1PracticalMaxMark + Pre1TheoryMaxMark);
                            subjectData.GradePre1 = GetGradebyTermBatch(((((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0)) / (divisorPre)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            subjectData.TheoryMarksPre2 = obtainedTheoryMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre2 = obtainedPracticalMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre2Practical = Pre2PracticalMaxMark;
                            subjectData.MaxMarksPre2Theory = Pre2TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre2 = ((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre2?.ObtainedMarks ?? 0)); /*megha((obtainedPracticalMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0));*/

                            var divisorPre2 = (Pre2PracticalMaxMark + Pre2TheoryMaxMark) == 0 ? 1 : (Pre2PracticalMaxMark + Pre2TheoryMaxMark);
                            subjectData.GradePre2 = GetGradebyTermBatch(((((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0)) / (divisorPre2)) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            if (stdInfo.ClassID.ToString() == "207" || stdInfo.ClassID.ToString() == "208" || stdInfo.ClassID.ToString() == "209")
                            {
                                // foreach ( var subid in item.SubjectId.ToString())
                                // {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == item.SubjectId.ToString() && x.ClassID == stdInfo.ClassID).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm1Project = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Project").FirstOrDefault();
                                var divisorfinalterm1SubjectEnrich = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "SubjectEnrich").FirstOrDefault();
                                var divisorfinalterm2Project = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Project").FirstOrDefault();
                                var divisorfinalterm2SubjectEnrich = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "SubjectEnrich").FirstOrDefault();


                                decimal finaldivisorviva = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorviva += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1Project != null)
                                {
                                    if (divisorfinalterm1Project.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1Project.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1SubjectEnrich != null)
                                {
                                    if (divisorfinalterm1SubjectEnrich.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1SubjectEnrich.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2Project != null)
                                {
                                    if (divisorfinalterm2Project.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2Project.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2SubjectEnrich != null)
                                {
                                    if (divisorfinalterm2SubjectEnrich.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2SubjectEnrich.MaximumMarks);
                                    }
                                }
                                var per = (Convert.ToDecimal(TotalObtainedMarks / finaldivisorviva) * 100);
                                decimal a = (Math.Round(Convert.ToDecimal(per), 1));
                                //  subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);
                                subjectData.FinalGrade = GetGradebyTermBatch(Convert.ToDecimal(a), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                                //subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);


                                //  }

                            }


                            if (item.SubjectId.ToString() == "45")
                            {
                                //  if (subid.Subject_ID.ToString() == "45")
                                //  {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "45" && x.ClassID == stdInfo.ClassID).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2Project = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Project").FirstOrDefault();
                                var divisorfinalterm2SubjectEnrich = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "SubjectEnrich").FirstOrDefault();
                                var divisorfinalterm1Project = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Project").FirstOrDefault();
                                var divisorfinalterm1SubjectEnrich = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "SubjectEnrich").FirstOrDefault();

                                decimal finaldivisorviva = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorviva += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2Project != null)
                                {
                                    if (divisorfinalterm2Project.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2Project.MaximumMarks);
                                    }
                                }
                                if (divisorfinalterm2SubjectEnrich != null)
                                {
                                    if (divisorfinalterm2SubjectEnrich.MaximumMarks != null)
                                    {
                                        finaldivisorviva += Convert.ToDecimal(divisorfinalterm2SubjectEnrich.MaximumMarks);
                                    }
                                }


                                subjectData.FinalGrade = GetGradebyTermBatch((Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                            }

                            // }
                            //megha


                            if (item.SubjectId.ToString() == "20")

                            {
                                var divisovias = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "20" && x.ClassID == stdInfo.ClassID).ToList();
                                var divisorfinalut1 = divisovias.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovias.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovias.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovias.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovias.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovias.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorvivas = 0;
                                if (divisorfinalut1 != null)
                                {
                                    if (divisorfinalut1.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalut1.MaximumMarks);
                                    }
                                }
                                if (divisorfinalut2 != null)
                                {
                                    finaldivisorvivas += Convert.ToDecimal(divisorfinalut2.MaximumMarks);
                                }
                                if (divisorfinalterm1theory != null)
                                {
                                    if (divisorfinalterm1theory.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks);
                                    }
                                }

                                if (divisorfinalterm1pract != null)
                                {
                                    if (divisorfinalterm1pract.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks);
                                    }
                                }



                                if (divisorfinalterm2theory != null)
                                {
                                    if (divisorfinalterm2theory.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks);
                                    }
                                }


                                if (divisorfinalterm2pract != null)
                                {
                                    if (divisorfinalterm2pract.MaximumMarks != null)
                                    {
                                        finaldivisorvivas += Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                    }
                                }

                                var per = (Convert.ToDecimal(TotalObtainedMarks / finaldivisorvivas) * 100);
                                decimal a = (Math.Round(Convert.ToDecimal(per), 1));
                                //  subjectData.FinalGrade = GetGrade(Convert.ToInt32(TotalObtainedMarks / finaldivisorviva) * 100);
                                subjectData.FinalGrade = GetGradebyTermBatch(Convert.ToDecimal(a), Convert.ToInt32(stdInfo.ClassID), termId, batchId);


                            }



                        }
                        //megha
                        else
                        {

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault(); ;
                            subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            subjectData.MaxMarksUT1 = UT1MaxMark;
                            //subjectData.MarksSelection = subjectData.MarksSelection == -1 ? -1 : subjectData.MarksSelection;
                            //subjectData.MaxMarksSelection = SelectionMaxMark;
                            subjectData.MarksUT2 = subjectData.MarksUT2 == -1 ? -1 : subjectData.MarksUT2;
                            subjectData.MaxMarksUT2 = UT2MaxMark;
                            subjectData.MarksUT1Grade = subjectData.MarksUT1Grade == "D" ? "D" : subjectData.MarksUT1Grade;
                            subjectData.MarksUT2Grade = subjectData.MarksUT2Grade == "D" ? "D" : subjectData.MarksUT2Grade;
                            subjectData.TotalMarks = subjectData.TotalMarks == -1 ? -1 : subjectData.TotalMarks;
                            subjectData.TheoryMarks = subjectData.TheoryMarks == -1 ? -1 : subjectData.TheoryMarks;
                            subjectData.PracticalMarks = subjectData.PracticalMarks == -1 ? -1 : subjectData.PracticalMarks;
                            subjectData.ProjectMarks = subjectData.ProjectMarks == -1 ? -1 : subjectData.ProjectMarks;
                            subjectData.SubjectEnrichMarks = subjectData.SubjectEnrichMarks == -1 ? -1 : subjectData.SubjectEnrichMarks;
                            subjectData.ProjectMarksUT2 = subjectData.ProjectMarksUT2 == -1 ? -1 : subjectData.ProjectMarksUT2;
                            subjectData.SubjectEnrichMarksUT2 = subjectData.SubjectEnrichMarksUT2 == -1 ? -1 : subjectData.SubjectEnrichMarksUT2;
                            subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == -1 ? -1 : subjectData.TotalObtainedMarks;
                            subjectData.GradeUT1 = subjectData.GradeUT1 == "D" ? "D" : subjectData.GradeUT1;
                            subjectData.TheoryMarksUT2 = subjectData.TheoryMarksUT2 == -1 ? -1 : subjectData.TheoryMarksUT2;
                            subjectData.PracticalMarksUT2 = subjectData.PracticalMarksUT2 == -1 ? -1 : subjectData.PracticalMarksUT2;
                            subjectData.TotalObtainedMarksUT2 = subjectData.TotalObtainedMarksUT2 == -1 ? -1 : subjectData.TotalObtainedMarksUT2;
                            subjectData.GradeUT2 = subjectData.GradeUT2 == "D" ? "D" : subjectData.GradeUT2;
                            subjectData.TotalMarksBothUTs = subjectData.TotalMarksBothUTs == -1 ? -1 : subjectData.TotalMarksBothUTs;
                            subjectData.FinalGrade = subjectData.FinalGrade == "D" ? "D" : subjectData.FinalGrade;
                            //Pre1,2 Add By Atul Kumar
                            subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                            subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                            subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;
                            subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                            subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                            subjectData.TheoryMarksSelection = subjectData.TheoryMarksSelection == -1 ? -1 : subjectData.TheoryMarksSelection;
                            subjectData.PracticalMarksSelection = subjectData.PracticalMarksSelection == -1 ? -1 : subjectData.PracticalMarksSelection;
                            subjectData.GradeSelection = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;

                            subjectData.TheoryMarksPromotion = subjectData.TheoryMarksPromotion == -1 ? -1 : subjectData.TheoryMarksPromotion;
                            subjectData.PracticalMarksPromotion = subjectData.PracticalMarksPromotion == -1 ? -1 : subjectData.PracticalMarksPromotion;
                            subjectData.GradePromotion = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;
                            //subjectData.GradeUT2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;

                        }
                    }

                    TotalObtainedMarks = 0; subjectDatas.Add(subjectData); obtainedTheoryMarksT1 = null;
                    obtainedPracticalMarksT1 = null; obtainedTheoryMarksT2 = null; obtainedPracticalMarksT2 = null;
                    obtainedUT1Marks = null; obtainedUT2Marks = null; theroyMaxMark = 0;
                    practicalMaxMark = 0; theroyTotalMark = 0; practicalTotalMark = 0;
                    UT1MaxMark = 0; UT2MaxMark = 0; Term1TheoryMaxMark = 0;
                    Term1PracticalMaxMark = 0; Term2TheoryMaxMark = 0; Term2PracticalMaxMark = 0;
                    OptionalUT1MaxMark = 0; OptionalUT2MaxMark = 0;
                    obtainedTheoryMarksPre1 = null; obtainedPracticalMarksPre1 = null; obtainedTheoryMarksPre2 = null;
                    obtainedPracticalMarksPre2 = null; Pre1TheoryMaxMark = 0; Pre1PracticalMaxMark = 0;
                    Pre2TheoryMaxMark = 0; Pre2PracticalMaxMark = 0; obtainedTheoryMarksPromotion = null;
                    obtainedPracticalMarksPromotion = null; PromotionTheoryMaxMark = 0; PromotionPracticalMaxMark = 0;
                    obtainedTheoryMarksSelection = null;
                    obtainedPracticalMarksSelection = null; SelectionTheoryMaxMark = 0; SelectionPracticalMaxMark = 0;
                }

                //for optional subject
                Tbl_TestRecords NewobtainedTheoryMarksT1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksT2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT2 = null;
                Tbl_TestRecords NewobtainedUT1Marks = null;
                //Tbl_TestRecords NewobtainedSelectionMarks = null;
                Tbl_TestRecords NewobtainedUT2Marks = null;

                Tbl_TestRecords NewobtainedTheoryMarksPre1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksPre2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre2 = null;
                Tbl_TestRecords NewobtainedTheoryMarksSelection = null;
                Tbl_TestRecords NewobtainedPracticalMarksSelection = null;
                Tbl_TestRecords NewobtainedTheoryMarksPromotion = null;
                Tbl_TestRecords NewobtainedPracticalMarksPromotion = null;


                decimal NewtheroyMaxMark = 0;
                decimal NewpracticalMaxMark = 0;
                decimal NewtheroyTotalMark = 0;
                decimal NewpracticalTotalMark = 0;
                decimal NewUT1MaxMark = 1;
                //decimal NewSelectionMaxMark = 1;
                decimal NewUT2MaxMark = 1;
                decimal NewTerm1TheoryMaxMark = 0;
                decimal NewTerm1PracticalMaxMark = 0;
                decimal NewTerm2TheoryMaxMark = 0;
                decimal NewTerm2PracticalMaxMark = 0;
                decimal NewOptionalUT1MaxMark = 0;
                decimal NewOptionalUT2MaxMark = 0;
                decimal NewOptionalSelectionMaxMark = 0;
                decimal NewTotalObtainedMarks = 0;

                decimal NewPre1TheoryMaxMark = 0;
                decimal NewPre1PracticalMaxMark = 0;

                decimal NewSelectionTheoryMaxMark = 0;
                decimal NewSelectionPracticalMaxMark = 0;
                decimal NewPromotionTheoryMaxMark = 0;
                decimal NewPromotionPracticalMaxMark = 0;
                decimal NewPre2TheoryMaxMark = 0;
                decimal NewPre2PracticalMaxMark = 0;

                var AllOptionalSubject = (from subj in _context.tbl_ClassSubject
                                          join test in _context.tbl_Tests
                                          on subj.SubjectId equals test.SubjectID
                                          where test.ClassID == stdInfo.ClassID && subj.ClassId == stdInfo.ClassID && (termId == 10 || test.TermID == termId) && test.IsOptional == true
                                          select subj).Distinct().ToList();

                List<OptionalSubjectData> optionalsubjectDatas = new List<OptionalSubjectData>();
                foreach (var item in AllOptionalSubject)
                {
                    OptionalSubjectData subjectData = new OptionalSubjectData();
                    foreach (var termItem in terms)
                    {

                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID).ToList();

                        if (test.Count > 0)
                        {
                            foreach (var testItem in test)
                            {
                                if (termItem.TermID == 1)//UT1
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.TermID == 1 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();
                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                    };

                                    NewobtainedUT1Marks = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    NewUT1MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += NewobtainedUT1Marks?.ObtainedMarks ?? 0;
                                }
                                if (termItem.TermID == 2)//UT2
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.TermID == 2 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                         select new
                                                         {
                                                             TestID = cog.TestID,
                                                             ObtainedMarks = cog.ObtainedMarks
                                                         }).FirstOrDefault();
                                    Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                    {
                                        ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                    };
                                    NewobtainedUT2Marks = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                    NewUT2MaxMark = testItem.MaximumMarks;
                                    TotalObtainedMarks += NewobtainedUT2Marks?.ObtainedMarks ?? 0;

                                }
                                if (termItem.TermID == 3)//Term1
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewTerm1TheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksT1?.ObtainedMarks ?? 0;

                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 3 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksT1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewTerm1PracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksT1?.ObtainedMarks ?? 0;
                                    }

                                }
                                if (termItem.TermID == 4)//Term2
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewTerm2TheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksT2?.ObtainedMarks ?? 0;

                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                             join cog in _context.tbl_TestObtainedMark
                                                             on cr.RecordID equals cog.RecordIDFK
                                                             where cr.StudentID == studentId && cr.TermID == 4 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                             select new
                                                             {
                                                                 TestID = cog.TestID,
                                                                 ObtainedMarks = cog.ObtainedMarks
                                                             }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentUT1Mark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksT2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewTerm2PracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksT2?.ObtainedMarks ?? 0;

                                    }
                                }
                                if (termItem.TermID == 5)
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 5 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksSelection = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewSelectionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksSelection?.ObtainedMarks ?? 0;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksSelection = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewSelectionPracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksSelection?.ObtainedMarks ?? 0;
                                    }
                                }
                                if (termItem.TermID == 6)
                                {
                                    if (testItem.TestType == "Theory")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedTheoryMarksPromotion = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        NewtheroyMaxMark = testItem.MaximumMarks;
                                        NewPromotionTheoryMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? 0;
                                    }
                                    if (testItem.TestType == "Practical")
                                    {
                                        var StuentSelectionMark = (from cr in _context.Tbl_TestRecord
                                                                   join cog in _context.tbl_TestObtainedMark
                                                                   on cr.RecordID equals cog.RecordIDFK
                                                                   where cr.StudentID == studentId && cr.TermID == 6 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                   select new
                                                                   {
                                                                       TestID = cog.TestID,
                                                                       ObtainedMarks = cog.ObtainedMarks
                                                                   }).FirstOrDefault();
                                        Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                        {
                                            ObtainedMarks = StuentSelectionMark?.ObtainedMarks ?? 0
                                        };
                                        NewobtainedPracticalMarksPromotion = tbl_TestRecords;//_context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                        practicalMaxMark = testItem.MaximumMarks;
                                        NewPromotionPracticalMaxMark = testItem.MaximumMarks;
                                        TotalObtainedMarks += NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? 0;
                                    }
                                }
                                // Pre-1,2 Add By Atul kumar
                                if (termId != 10)
                                {
                                    if (termItem.TermID == 7)//Pre1
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewPre1TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksPre1?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre1Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 7 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre1Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksPre1 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewPre1PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksPre1?.ObtainedMarks ?? 0;
                                        }

                                    }
                                    if (termItem.TermID == 8)//Pre8
                                    {
                                        if (testItem.TestType == "Theory")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedTheoryMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            NewtheroyMaxMark = testItem.MaximumMarks;
                                            NewPre2TheoryMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedTheoryMarksPre2?.ObtainedMarks ?? 0;

                                        }
                                        if (testItem.TestType == "Practical")
                                        {
                                            var StuentPre2Mark = (from cr in _context.Tbl_TestRecord
                                                                  join cog in _context.tbl_TestObtainedMark
                                                                  on cr.RecordID equals cog.RecordIDFK
                                                                  where cr.StudentID == studentId && cr.TermID == 8 && cog.TestID == testItem.TestID && cr.BatchId == batchId
                                                                  select new
                                                                  {
                                                                      TestID = cog.TestID,
                                                                      ObtainedMarks = cog.ObtainedMarks
                                                                  }).FirstOrDefault();
                                            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords()
                                            {
                                                ObtainedMarks = StuentPre2Mark?.ObtainedMarks ?? 0
                                            };
                                            NewobtainedPracticalMarksPre2 = tbl_TestRecords;// _context.Tbl_TestRecord.Where(x => x.TestID == testItem.TestID && x.StudentID == studentId).FirstOrDefault();
                                            practicalMaxMark = testItem.MaximumMarks;
                                            NewPre2PracticalMaxMark = testItem.MaximumMarks;
                                            TotalObtainedMarks += NewobtainedPracticalMarksPre2?.ObtainedMarks ?? 0;
                                        }

                                    }

                                }

                            }


                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();
                            subjectData.MarksUT1 = NewobtainedUT1Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT1 = NewUT1MaxMark;
                            subjectData.MarksUT1Grade = GetOptionMarkGrade(NewobtainedUT1Marks?.ObtainedMarks ?? -2);
                            subjectData.TheoryMarks = NewobtainedTheoryMarksT1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarks = NewobtainedPracticalMarksT1?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarks =
    ((NewobtainedTheoryMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksT1?.ObtainedMarks ?? -1)) +
    ((NewobtainedPracticalMarksT1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksT1?.ObtainedMarks ?? -1));

                            subjectData.GradeUT1 = GetOptionMarkGrade((NewobtainedTheoryMarksT1?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksT1?.ObtainedMarks ?? 0));
                            subjectData.MarksUT2 = NewobtainedUT2Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT2 = NewUT2MaxMark;
                            subjectData.MarksUT2Grade = GetOptionMarkGrade(NewobtainedUT2Marks?.ObtainedMarks ?? -2);
                            subjectData.TotalMarks =
      ((NewobtainedUT1Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedUT1Marks?.ObtainedMarks ?? -1)) +
      ((NewobtainedUT2Marks?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedUT2Marks?.ObtainedMarks ?? -1));

                            subjectData.TheoryMarksUT2 = NewobtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksUT2 = NewobtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarksUT2 =
      ((NewobtainedTheoryMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksT2?.ObtainedMarks ?? -1)) +
      ((NewobtainedPracticalMarksT2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksT2?.ObtainedMarks ?? -1));

                            subjectData.GradeUT2 = GetOptionMarkGrade((NewobtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksT2?.ObtainedMarks ?? 0));


                            if (termId != 10)
                            {
                                //Pre-1,2 Add By Atul Kumar
                                subjectData.TheoryMarksPre1 = NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre1 = NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre1 =
          ((NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPre1?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPre1?.ObtainedMarks ?? -1));

                                subjectData.GradePre1 = GetOptionMarkGrade((NewobtainedTheoryMarksPre1?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPre1?.ObtainedMarks ?? 0));

                                subjectData.TheoryMarksPre2 = NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre2 = NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre2 =
          ((NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPre2?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPre2?.ObtainedMarks ?? -1));

                                subjectData.GradePre2 = GetOptionMarkGrade((NewobtainedTheoryMarksPre2?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPre2?.ObtainedMarks ?? 0));


                                subjectData.TheoryMarksSelection = NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksSelection = NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarkSelection =
          ((NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksSelection?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksSelection?.ObtainedMarks ?? -1));

                                subjectData.GradeSelection = GetOptionMarkGrade((NewobtainedTheoryMarksSelection?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksSelection?.ObtainedMarks ?? 0));

                                subjectData.TheoryMarksPromotion = NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPromotion = NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarkPromotion =
          ((NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? -1)) +
          ((NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -1) == -1 ? 0 : (NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? -1));

                                subjectData.GradePromotion = GetOptionMarkGrade((NewobtainedTheoryMarksPromotion?.ObtainedMarks ?? 0) + (NewobtainedPracticalMarksPromotion?.ObtainedMarks ?? 0));






                            }

                        }
                        else
                        {

                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();
                            subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            subjectData.MaxMarksUT1 = subjectData.MaxMarksUT1 == -1 ? -1 : subjectData.MaxMarksUT1;
                            subjectData.MarksUT2 = subjectData.MarksUT2 == -1 ? -1 : subjectData.MarksUT2;
                            subjectData.MaxMarksUT2 = subjectData.MaxMarksUT2 == -1 ? -1 : subjectData.MaxMarksUT2;
                            subjectData.MarksUT1Grade = subjectData.MarksUT1Grade == "D" ? "D" : subjectData.MarksUT1Grade;
                            subjectData.MarksUT2Grade = subjectData.MarksUT2Grade == "D" ? "D" : subjectData.MarksUT2Grade;
                            subjectData.TotalMarks = subjectData.TotalMarks == -1 ? -1 : subjectData.TotalMarks;
                            subjectData.TheoryMarks = subjectData.TheoryMarks == -1 ? -1 : subjectData.TheoryMarks;
                            subjectData.PracticalMarks = subjectData.PracticalMarks == -1 ? -1 : subjectData.PracticalMarks;
                            subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == -1 ? -1 : subjectData.TotalObtainedMarks;
                            subjectData.GradeUT1 = subjectData.GradeUT1 == "D" ? "D" : subjectData.GradeUT1;
                            subjectData.TheoryMarksUT2 = subjectData.TheoryMarksUT2 == -1 ? -1 : subjectData.TheoryMarksUT2;
                            subjectData.PracticalMarksUT2 = subjectData.PracticalMarksUT2 == -1 ? -1 : subjectData.PracticalMarksUT2;
                            subjectData.TotalObtainedMarksUT2 = subjectData.TotalObtainedMarksUT2 == -1 ? -1 : subjectData.TotalObtainedMarksUT2;
                            subjectData.GradeUT2 = subjectData.GradeUT2 == "D" ? "D" : subjectData.GradeUT2;

                            if (termId != 10)
                            {
                                //Pre-1,2 Add By Atul Kumar
                                subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                                subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                                subjectData.TotalObtainedMarksPre1 = subjectData.TotalObtainedMarksPre1 == -1 ? -1 : subjectData.TotalObtainedMarksPre1;
                                subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;

                                subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                                subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                                subjectData.TotalObtainedMarksPre2 = subjectData.TotalObtainedMarksPre2 == -1 ? -1 : subjectData.TotalObtainedMarksPre2;
                                subjectData.GradePre2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;

                                subjectData.TheoryMarksSelection = subjectData.TheoryMarksSelection == -1 ? -1 : subjectData.TheoryMarksSelection;
                                subjectData.PracticalMarksSelection = subjectData.PracticalMarksSelection == -1 ? -1 : subjectData.PracticalMarksSelection;
                                subjectData.TotalObtainedMarkSelection = subjectData.TotalObtainedMarkSelection == -1 ? -1 : subjectData.TotalObtainedMarkSelection;
                                subjectData.GradeSelection = subjectData.GradeSelection == "D" ? "D" : subjectData.GradeSelection;

                                subjectData.TheoryMarksPromotion = subjectData.TheoryMarksPromotion == -1 ? -1 : subjectData.TheoryMarksPromotion;
                                subjectData.PracticalMarksPromotion = subjectData.PracticalMarksPromotion == -1 ? -1 : subjectData.PracticalMarksPromotion;
                                subjectData.TotalObtainedMarkPromotion = subjectData.TotalObtainedMarkPromotion == -1 ? -1 : subjectData.TotalObtainedMarkPromotion;
                                subjectData.GradePromotion = subjectData.GradePromotion == "D" ? "D" : subjectData.GradePromotion;

                            }

                        }


                    }
                    TotalObtainedMarks = 0;
                    optionalsubjectDatas.Add(subjectData);
                    NewobtainedTheoryMarksT1 = null;
                    NewobtainedPracticalMarksT1 = null;
                    NewobtainedTheoryMarksT2 = null;
                    NewobtainedPracticalMarksT2 = null;
                    NewobtainedUT1Marks = null;
                    NewobtainedUT2Marks = null;
                    NewobtainedTheoryMarksPre1 = null;
                    //NewobtainedSelectionMarks = null;
                    NewobtainedPracticalMarksPre1 = null;
                    NewobtainedTheoryMarksPre2 = null;
                    NewobtainedPracticalMarksPre2 = null;

                    NewobtainedTheoryMarksSelection = null;
                    NewobtainedPracticalMarksSelection = null;
                    NewobtainedTheoryMarksPromotion = null;
                    NewobtainedPracticalMarksPromotion = null;
                }
                decimal UT1Total = 0; decimal UT1MaxTotal = 0; decimal UT2Total = 0; decimal UT2MaxTotal = 0;
                decimal Term1TheoryMaxTotal = 0; decimal Term1PracticalMaxTotal = 0; decimal Term2TheoryMaxTotal = 0;
                decimal Term2PracticalMaxTotal = 0; decimal UTAllTotal = 0; decimal TheoryTotalT1 = 0;
                decimal PracticalTotalT1 = 0; decimal T1AllTotal = 0; decimal TheoryTotalT2 = 0;
                decimal PracticalTotalT2 = 0; decimal T2AllTotal = 0; decimal OverallAllTotal = 0; decimal ProjectTotalT1 = 0;
                decimal SubjectEnrichTotalT1 = 0; decimal ProjectTotalT2 = 0; decimal SubjectEnrichTotalT2 = 0;
                decimal Term1ProjectMaxTotal = 0; decimal Term1SubjectEnrichMaxTotal = 0; decimal Term2ProjectMaxTotal = 0; decimal Term2SubjectEnrichMaxTotal = 0;

                decimal Pre1TheoryMaxTotal = 0; decimal Pre1PracticalMaxTotal = 0; decimal Pre2TheoryMaxTotal = 0;
                decimal Pre2PracticalMaxTotal = 0; decimal PreAllTotal = 0;
                decimal SelectionTheoryMaxTotal = 0; decimal SelectionPracticalMaxTotal = 0; decimal SelectioAllTotal = 0;
                decimal PromotionTheoryMaxTotal = 0; decimal PromotionPracticalMaxTotal = 0; decimal PromotioAllTotal = 0;
                decimal PromotionTheoryTotal = 0; decimal PromotionPracticalTotal = 0; decimal SelectionTheoryTotal = 0; decimal SelectionPracticalTotal = 0;

                foreach (var item in subjectDatas)
                {

                    UT1Total += (item.MarksUT1 == -1 || item.MarksUT1 == -2) ? 0 : item.MarksUT1;
                    UT1MaxTotal += (item.MaxMarksUT1 == -1 || item.MaxMarksUT1 == -2 /*|| item.Subject == "English Language   " || item.Subject == "English Dictation  " || item.Subject == "English Writing" || item.Subject == "Hindi Lang " || item.Subject == "Hindi Dictation " || item.Subject == "Hindi Writing" || item.Subject == "Math Written"*/) ? 0 : item.MaxMarksUT1;
                    UT2MaxTotal += (item.MaxMarksUT2 == -1 || item.MaxMarksUT2 == -2) ? 0 : item.MaxMarksUT2;
                    Term1TheoryMaxTotal += (item.MaxMarksTerm1Theory == -1 || item.MaxMarksTerm1Theory == -2) ? 0 : item.MaxMarksTerm1Theory;
                    Term1PracticalMaxTotal += (item.MaxMarksTerm1Practical == -1 || item.MaxMarksTerm1Practical == -2) ? 0 : item.MaxMarksTerm1Practical;
                    Term2TheoryMaxTotal += (item.MaxMarksTerm2Theory == -1 || item.MaxMarksTerm2Theory == -2) ? 0 : item.MaxMarksTerm2Theory;
                    Term2PracticalMaxTotal += (item.MaxMarksTerm2Practical == -1 || item.MaxMarksTerm2Practical == -2) ? 0 : item.MaxMarksTerm2Practical;
                    UT2Total += (item.MarksUT2 == -1 || item.MarksUT2 == -2) ? 0 : item.MarksUT2;
                    UTAllTotal += (item.TotalMarks == -1 || item.TotalMarks == -2) ? 0 : item.TotalMarks;
                    TheoryTotalT1 += (item.TheoryMarks == -1 || item.TheoryMarks == -2) ? 0 : item.TheoryMarks;
                    PracticalTotalT1 += (item.PracticalMarks == -1 || item.PracticalMarks == -2) ? 0 : item.PracticalMarks;
                    T1AllTotal += (item.TotalObtainedMarks == -1 || item.TotalObtainedMarks == -2) ? 0 : item.TotalObtainedMarks;
                    TheoryTotalT2 += (item.TheoryMarksUT2 == -1 || item.TheoryMarksUT2 == -2) ? 0 : item.TheoryMarksUT2;
                    PracticalTotalT2 += (item.PracticalMarksUT2 == -1 || item.PracticalMarksUT2 == -2) ? 0 : item.PracticalMarksUT2;
                    T2AllTotal += (item.TotalObtainedMarksUT2 == -1 || item.TotalObtainedMarksUT2 == -2) ? 0 : item.TotalObtainedMarksUT2;
                    OverallAllTotal += (item.TotalMarksBothUTs == -1 || item.TotalMarksBothUTs == -2) ? 0 : item.TotalMarksBothUTs;

                    Term1ProjectMaxTotal += (item.MaxMarksTerm1Project == -1 || item.MaxMarksTerm1Project == -2) ? 0 : item.MaxMarksTerm1Project;
                    Term1SubjectEnrichMaxTotal += (item.MaxMarksTerm1SubjectEnrich == -1 || item.MaxMarksTerm1SubjectEnrich == -2) ? 0 : item.MaxMarksTerm1SubjectEnrich;

                    Term2ProjectMaxTotal += (item.MaxMarksTerm2Project == -1 || item.MaxMarksTerm2Project == -2) ? 0 : item.MaxMarksTerm2Project;
                    Term2SubjectEnrichMaxTotal += (item.MaxMarksTerm2SubjectEnrich == -1 || item.MaxMarksTerm2SubjectEnrich == -2) ? 0 : item.MaxMarksTerm2SubjectEnrich;

                    ProjectTotalT1 += (item.ProjectMarks == -1 || item.ProjectMarks == -2) ? 0 : item.ProjectMarks;
                    SubjectEnrichTotalT1 += (item.SubjectEnrichMarks == -1 || item.SubjectEnrichMarks == -2) ? 0 : item.SubjectEnrichMarks;
                    ProjectTotalT2 += (item.ProjectMarksUT2 == -1 || item.ProjectMarksUT2 == -2) ? 0 : item.ProjectMarksUT2;
                    SubjectEnrichTotalT2 += (item.SubjectEnrichMarksUT2 == -1 || item.SubjectEnrichMarksUT2 == -2) ? 0 : item.SubjectEnrichMarksUT2;

                    if (termId != 10)
                    {
                        Pre1TheoryMaxTotal += (item.MaxMarksPre1Theory == -1 || item.MaxMarksPre1Theory == -2) ? 0 : item.MaxMarksPre1Theory;
                        Pre1PracticalMaxTotal += (item.PracticalMarksPre1 == -1 || item.PracticalMarksPre1 == -2) ? 0 : item.PracticalMarksPre1;
                        Pre1AllTotal += (item.TotalObtainedMarksPre1 == -1 || item.TotalObtainedMarksPre1 == -2) ? 0 : item.TotalObtainedMarksPre1;


                        Pre2TheoryMaxTotal += (item.MaxMarksPre2Theory == -1 || item.MaxMarksPre2Theory == -2) ? 0 : item.MaxMarksPre2Theory;
                        Pre2PracticalMaxTotal += (item.MaxMarksPre2Practical == -1 || item.MaxMarksPre2Practical == -2) ? 0 : item.MaxMarksPre2Practical;
                        Pre2AllTotal += (item.TotalObtainedMarksPre2 == -1 || item.TotalObtainedMarksPre2 == -2) ? 0 : item.TotalObtainedMarksPre2;

                        SelectionTheoryTotal += (item.TheoryMarksSelection == -1 || item.TheoryMarksSelection == -2) ? 0 : item.TheoryMarksSelection;
                        PromotionPracticalTotal += (item.PracticalMarksSelection == -1 || item.PracticalMarksSelection == -2) ? 0 : item.PracticalMarksSelection;
                        SelectionTheoryMaxTotal += (item.MaxMarksSelectionTheory == -1 || item.MaxMarksSelectionTheory == -2) ? 0 : item.MaxMarksSelectionTheory;
                        SelectionPracticalMaxTotal += (item.PracticalMarksSelection == -1 || item.PracticalMarksSelection == -2) ? 0 : item.PracticalMarksSelection;
                        SelectionAllTotal += (item.TotalObtainedMarksSelection == -1 || item.TotalObtainedMarksSelection == -2) ? 0 : item.TotalObtainedMarksSelection;

                        PromotionTheoryTotal += (item.TheoryMarksPromotion == -1 || item.TheoryMarksPromotion == -2) ? 0 : item.TheoryMarksPromotion;
                        PromotionPracticalTotal += (item.PracticalMarksPromotion == -1 || item.PracticalMarksPromotion == -2) ? 0 : item.PracticalMarksPromotion;
                        PromotionTheoryMaxTotal += (item.MaxMarksPromotionTheory == -1 || item.MaxMarksPromotionTheory == -2) ? 0 : item.MaxMarksPromotionTheory;
                        PromotionPracticalMaxTotal += (item.MaxMarksPromotionPractical == -1 || item.MaxMarksPromotionPractical == -2) ? 0 : item.MaxMarksPromotionPractical;
                        PromotionAllTotal += (item.TotalObtainedMarksPromotion == -1 || item.TotalObtainedMarksPromotion == -2) ? 0 : item.TotalObtainedMarksPromotion;

                        //PromotionAllTotal += (item.TotalMarks == -1 || item.TotalMarks == -2) ? 0 : item.TotalMarks;

                    }
                    //2  
                }
                var divisor01 = (Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term1ProjectMaxTotal + Term1SubjectEnrichMaxTotal + UT1MaxTotal) == 0 ? 1 : (Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term1ProjectMaxTotal + Term1SubjectEnrichMaxTotal + UT1MaxTotal);
                var divisor02 = (Term2TheoryMaxTotal + Term2PracticalMaxTotal + Term2ProjectMaxTotal + Term2SubjectEnrichMaxTotal + UT2MaxTotal) == 0 ? 1 : (Term2TheoryMaxTotal + Term2PracticalMaxTotal + Term2ProjectMaxTotal + Term2SubjectEnrichMaxTotal + UT2MaxTotal);

                var divisorPre01 = (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal) == 0 ? 1 : (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal);
                var divisorPre02 = (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal) == 0 ? 1 : (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal);
                var divisor0Selection = (SelectionTheoryMaxTotal + SelectionPracticalMaxTotal) == 0 ? 1 : (SelectionTheoryMaxTotal + SelectionPracticalMaxTotal);
                var divisor0Promotion = (PromotionTheoryMaxTotal + PromotionPracticalMaxTotal) == 0 ? 1 : (PromotionTheoryMaxTotal + PromotionPracticalMaxTotal);
                var T1Gradee = GetGradebyTermBatch(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                TotalResult totalResult = new TotalResult()
                {
                    UT1Total = UT1Total,
                    UT1MaxTotal = UT1MaxTotal,
                    UT2Total = UT2Total,
                    UT2MaxTotal = UT2MaxTotal,
                    UT1TotalGrade = GetGradebyTermBatch(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UT2TotalGrade = GetGradebyTermBatch(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UTAllTotal = UTAllTotal,
                    TheoryTotalT1 = TheoryTotalT1,
                    PracticalTotalT1 = PracticalTotalT1,
                    ProjectTotalT1 = ProjectTotalT1,
                    SubjectEnrichTotalT1 = SubjectEnrichTotalT1,
                    ProjectTotalT2 = ProjectTotalT2,
                    SubjectEnrichTotalT2 = SubjectEnrichTotalT2,
                    T1AllTotal = T1AllTotal,
                    T1Grade = GetGradebyTermBatch(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    TheoryTotalT2 = TheoryTotalT2,
                    PracticalTotalT2 = PracticalTotalT2,
                    T2AllTotal = T2AllTotal,
                    T2Grade = GetGradebyTermBatch(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    OverallAllTotal = OverallAllTotal,
                    OverallGrade = GetGradebyTermBatch(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)), Convert.ToInt32(stdInfo.ClassID), termId, batchId),

                    Term1TheoryMaxTotal = Term1TheoryMaxTotal,
                    //SelectionMaxTotal = SelectionMaxTotal,
                    Term1PracticalMaxTotal = Term1PracticalMaxTotal,
                    Term2TheoryMaxTotal = Term2TheoryMaxTotal,
                    Term2PracticalMaxTotal = Term2PracticalMaxTotal


                };

                var totals =
                      (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal + SelectionTheoryMaxTotal + SelectionPracticalMaxTotal + PromotionTheoryMaxTotal + PromotionPracticalMaxTotal);
                //totalResult.  OverallGrade = GetGrade(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)), Convert.ToInt32(studentInfo.Class_Id));


                TotalResultpercentage totalResultpercentage = new TotalResultpercentage()
                {
                    UT1Total = PercentageCal(UT1Total, UT1MaxTotal),
                    UT1TotalGrade = GetGradebyTermBatch(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),

                    UT2Total = PercentageCal(UT2Total, UT2MaxTotal),
                    UT2TotalGrade = GetGradebyTermBatch(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    UTAllTotal = PercentageCal(UTAllTotal, UT1MaxTotal + UT2MaxTotal),
                    TheoryTotalT1 = PercentageCal(TheoryTotalT1, Term1TheoryMaxTotal),
                    PracticalTotalT1 = PercentageCal(PracticalTotalT1, Term1PracticalMaxTotal == 0 ? 1 : Term1PracticalMaxTotal),
                    ProjectTotalT1 = PercentageCal(ProjectTotalT1, Term1ProjectMaxTotal == 0 ? 1 : Term1ProjectMaxTotal),
                    SubjectEnrichTotalT1 = PercentageCal(SubjectEnrichTotalT1, Term1SubjectEnrichMaxTotal == 0 ? 1 : Term1SubjectEnrichMaxTotal),
                    ProjectTotalT2 = PercentageCal(ProjectTotalT2, Term2ProjectMaxTotal == 0 ? 1 : Term2ProjectMaxTotal),
                    SubjectEnrichTotalT2 = PercentageCal(SubjectEnrichTotalT2, Term2SubjectEnrichMaxTotal == 0 ? 1 : Term2SubjectEnrichMaxTotal),
                    T1AllTotal = PercentageCal(T1AllTotal, divisor01),
                    T1Grade = GetGrade(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID)),
                    TheoryTotalT2 = PercentageCal(TheoryTotalT2, Term2TheoryMaxTotal),
                    PracticalTotalT2 = PercentageCal(PracticalTotalT2, Term2PracticalMaxTotal == 0 ? 1 : Term2PracticalMaxTotal),
                    T2AllTotal = PercentageCal(T2AllTotal, divisor02),
                    T2Grade = GetGradebyTermBatch(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID), termId, batchId),
                    OverallAllTotal = Math.Round(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal)), 1),
                    OverallGrade = GetGradebyTermBatch(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal + SelectionPracticalMaxTotal + SelectionTheoryMaxTotal + PromotionPracticalMaxTotal + PromotionTheoryMaxTotal)), Convert.ToInt32(stdInfo.ClassID), termId, batchId)


                };
                var persoverall = PercentageCal(Convert.ToDecimal(totalResult.OverallAllTotal), Convert.ToDecimal(totals));
                var perst = Math.Round(Convert.ToDecimal(persoverall), 1);
                totalResult.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(perst), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                var pers = Math.Round(Convert.ToDecimal(totalResultpercentage.OverallAllTotal), 1);
                totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(pers), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                if (stdInfo.ClassID.ToString() == "205")
                {
                    var overallpers = PercentageCal(totalResult.OverallAllTotal, 2630);
                    var perstoverall = Math.Round(Convert.ToDecimal(overallpers), 1);
                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(perstoverall), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = Math.Round(Convert.ToDecimal(totalResultpercentage.OverallAllTotal), 1);
                    totalResultpercentage.OverallGrade = GetGradebyTermBatch(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                }


                if (stdInfo.ClassID.ToString() == "208")//kg
                {

                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 880), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGrade(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID));
                }

                if (stdInfo.ClassID.ToString() == "209")
                {
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 980), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGrade(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID));
                }



                if (stdInfo.ClassID.ToString() == "207")
                {

                    // totalResult.OverallAllTotal = 2;
                    totalResult.OverallGrade = GetGradebyTermBatch(PercentageCal(totalResult.OverallAllTotal, 680), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    var per = ((totalResultpercentage.OverallAllTotal));
                    totalResultpercentage.OverallGrade = GetGrade(Convert.ToDecimal(per), Convert.ToInt32(stdInfo.ClassID));
                }

                //if (totalResultpercentage.OverallGrade == "0 " || totalResultpercentage.OverallGrade == null)
                //{
                //    totalResultpercentage.OverallGrade = "D";
                //}
                var validGrade = false;
                if (termId != 10)
                {
                    totalResult.Pre1Grade = GetGradebyTermBatch(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.Pre1AllTotal = Pre1AllTotal;
                    totalResult.Pre2AllTotal = Pre2AllTotal;
                    totalResult.Pre2Grade = GetGradebyTermBatch(PercentageCal(Pre2AllTotal, divisorPre02), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.Pre1TheoryMaxTotal = Pre1AllTotal;
                    totalResult.Pre1PracticalMaxTotal = Pre1PracticalMaxTotal;
                    totalResult.Pre2TheoryMaxTotal = Pre2AllTotal;
                    totalResult.Pre2PracticalMaxTotal = Pre2PracticalMaxTotal;


                    totalResult.SelectionGrade = GetGradebyTermBatch(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.SelectionAllTotal = SelectionAllTotal;
                    totalResult.SelectionTheoryMaxTotal = SelectionAllTotal;
                    totalResult.SelectionPracticalMaxTotal = SelectionPracticalMaxTotal;


                    totalResult.PromotionGrade = GetGradebyTermBatch(PercentageCal(PromotionAllTotal, divisor0Promotion), Convert.ToInt32(stdInfo.ClassID), termId, batchId);
                    totalResult.PromotionAllTotal = PromotionAllTotal;
                    totalResult.PromotionTheoryMaxTotal = PromotionTheoryTotal;
                    totalResult.PromotionPracticalMaxTotal = PromotionPracticalTotal;
                    //totalResult.SelectionGrade = GetGrade(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID));
                    //totalResult.SelectionAlltotal = SelectionAllTotal;

                    totalResultpercentage.TheoryTotalPre1 = PercentageCal(Pre1AllTotal, Pre1TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre1 = PercentageCal(Pre1PracticalMaxMark, Pre1PracticalMaxTotal == 0 ? 1 : Pre1PracticalMaxTotal);
                    totalResultpercentage.Pre1AllTotal = PercentageCal(Pre1AllTotal, divisorPre01);
                    totalResultpercentage.Pre1Grade = GetGradebyTermBatch(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalPre2 = PercentageCal(Pre2AllTotal, Pre2TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre2 = PercentageCal(Pre2PracticalMaxMark, Pre2PracticalMaxTotal == 0 ? 1 : Pre2PracticalMaxTotal);
                    totalResultpercentage.Pre2AllTotal = PercentageCal(Pre2AllTotal, divisorPre01);
                    totalResultpercentage.Pre2Grade = GetGradebyTermBatch(PercentageCal(Pre2AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalSelection = PercentageCal(SelectionTheoryTotal, SelectionTheoryMaxTotal);
                    totalResultpercentage.PracticalTotalSelection = PercentageCal(SelectionPracticalTotal, SelectionPracticalMaxTotal == 0 ? 1 : SelectionPracticalMaxTotal);
                    totalResultpercentage.SelectionAllTotal = PercentageCal(SelectionAllTotal, divisor0Selection);
                    totalResultpercentage.SelectionGrade = GetGradebyTermBatch(PercentageCal(SelectionAllTotal, divisor0Selection), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    totalResultpercentage.TheoryTotalPromotion = PercentageCal(PromotionTheoryTotal, PromotionTheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPromotion = PercentageCal(PromotionPracticalTotal, PromotionPracticalMaxTotal == 0 ? 1 : PromotionPracticalMaxTotal);
                    totalResultpercentage.PromotionAllTotal = PercentageCal(PromotionAllTotal, divisor0Promotion);
                    totalResultpercentage.PromotionGrade = GetGradebyTermBatch(PercentageCal(PromotionAllTotal, divisor0Promotion), Convert.ToInt32(stdInfo.ClassID), termId, batchId);

                    if (termId == 7)
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarksPre1 <= 32);
                    }
                    else if (termId == 8)
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarksPre2 <= 32);
                    }
                    else
                    {
                        validGrade = subjectDatas.Any(x => x.TotalObtainedMarks <= 32);
                    }
                }

                if (termId == 7 || termId == 8 || termId == 10)
                {
                    if (validGrade)
                    {
                        totalResult.T1Grade = "";
                        totalResult.T2Grade = "";
                        totalResult.UT1TotalGrade = "";
                        totalResult.UT2TotalGrade = "";
                        totalResult.Pre1Grade = "";
                        totalResult.Pre2Grade = "";
                        totalResult.OverallGrade = "";
                        totalResultpercentage.T1Grade = "";
                        totalResultpercentage.T2Grade = "";
                        totalResultpercentage.UT1TotalGrade = "";
                        totalResultpercentage.UT2TotalGrade = "";
                        totalResultpercentage.Pre1Grade = "";
                        totalResultpercentage.Pre2Grade = "";
                        totalResultpercentage.OverallGrade = "";

                    }
                    else
                    {
                        totalResult.T1Grade = totalResult.T1Grade == "D" ? "" : totalResult.T1Grade;
                        totalResult.T2Grade = totalResult.T2Grade == "D" ? "" : totalResult.T2Grade;
                        totalResult.UT1TotalGrade = totalResult.UT1TotalGrade == "D" ? "" : totalResult.UT1TotalGrade;
                        totalResult.UT2TotalGrade = totalResult.UT2TotalGrade == "D" ? "" : totalResult.UT2TotalGrade;
                        totalResultpercentage.T1Grade = totalResultpercentage.T1Grade == "D" ? "" : totalResultpercentage.T1Grade;
                        totalResultpercentage.T2Grade = totalResultpercentage.T2Grade == "D" ? "" : totalResultpercentage.T2Grade;
                        totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT1TotalGrade == "D" ? "" : totalResultpercentage.UT1TotalGrade;
                        totalResultpercentage.UT2TotalGrade = totalResultpercentage.UT2TotalGrade == "D" ? "" : totalResultpercentage.UT2TotalGrade;

                        ////m
                        //totalResultpercentage.fin = totalResultpercentage.OverallGrade == "0" ? "D" : totalResultpercentage.OverallGrade;
                        totalResultpercentage.OverallGrade = totalResultpercentage.OverallGrade == "" ? "D" : totalResultpercentage.OverallGrade;
                        //m end

                        totalResult.Pre1Grade = totalResult.Pre1Grade == "D" ? "" : totalResult.Pre1Grade;
                        totalResult.Pre2Grade = totalResult.Pre2Grade == "D" ? "" : totalResult.Pre2Grade;
                        totalResultpercentage.Pre1Grade = totalResultpercentage.Pre1Grade == "D" ? "" : totalResultpercentage.Pre1Grade;
                        totalResultpercentage.Pre2Grade = totalResultpercentage.Pre2Grade == "D" ? "" : totalResultpercentage.Pre2Grade;
                        totalResult.SelectionGrade = totalResult.SelectionGrade == "D" ? "" : totalResult.SelectionGrade;
                        totalResultpercentage.SelectionGrade = totalResultpercentage.SelectionGrade == "D" ? "" : totalResultpercentage.SelectionGrade;
                        totalResult.PromotionGrade = totalResult.PromotionGrade == "D" ? "" : totalResult.PromotionGrade;
                        totalResultpercentage.PromotionGrade = totalResultpercentage.PromotionGrade == "D" ? "" : totalResultpercentage.PromotionGrade;
                    }
                }
                else
                {

                    //if (!validGrade)
                    //{
                    //    totalResult.T1Grade = "";
                    //    totalResult.T2Grade = "";
                    //    totalResult.UT1TotalGrade = "";
                    //    totalResult.UT2TotalGrade = "";
                    //    totalResult.Pre1Grade = "";
                    //    totalResult.Pre2Grade = "";
                    //    totalResult.OverallGrade = "";
                    //    totalResultpercentage.T1Grade = "";
                    //    totalResultpercentage.T2Grade = "";
                    //    totalResultpercentage.UT1TotalGrade = "";
                    //    totalResultpercentage.UT2TotalGrade = "";
                    //    totalResultpercentage.Pre1Grade = "";
                    //    totalResultpercentage.Pre2Grade = "";
                    //    totalResultpercentage.OverallGrade = "";

                    //}
                    //else
                    //{
                    //    totalResult.T1Grade = totalResult.T1Grade == "D" ? "" : totalResult.T1Grade;
                    //    totalResult.T2Grade = totalResult.T2Grade == "D" ? "" : totalResult.T2Grade;
                    //    totalResult.UT1TotalGrade = totalResult.UT1TotalGrade == "D" ? "" : totalResult.UT1TotalGrade;
                    //    totalResult.UT2TotalGrade = totalResult.UT2TotalGrade == "D" ? "" : totalResult.UT2TotalGrade;
                    //    totalResultpercentage.T1Grade = totalResultpercentage.T1Grade == "D" ? "" : totalResultpercentage.T1Grade;
                    //    totalResultpercentage.T2Grade = totalResultpercentage.T2Grade == "D" ? "" : totalResultpercentage.T2Grade;
                    //    totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT1TotalGrade == "D" ? "" : totalResultpercentage.UT1TotalGrade;
                    //    totalResultpercentage.UT2TotalGrade = totalResultpercentage.UT2TotalGrade == "D" ? "" : totalResultpercentage.UT2TotalGrade;

                    //    totalResult.Pre1Grade = totalResult.Pre1Grade == "D" ? "" : totalResult.Pre1Grade;
                    //    totalResult.Pre2Grade = totalResult.Pre2Grade == "D" ? "" : totalResult.Pre2Grade;
                    //    totalResultpercentage.Pre1Grade = totalResultpercentage.Pre1Grade == "D" ? "" : totalResultpercentage.Pre1Grade;
                    //    totalResultpercentage.Pre2Grade = totalResultpercentage.Pre2Grade == "D" ? "" : totalResultpercentage.Pre2Grade;
                    //}
                }


                //Step 1 Working
                foreach (var subjectData in subjectDatas)
                {
                    // Check if any of the properties MarksUT1, MarksUT2, TotalMarks, TheoryMarks, PracticalMarks, 
                    // TotalObtainedMarks, TheoryMarksUT2, PracticalMarksUT2, or TotalMarksBothUTs is equal to 0
                    if (subjectData.MarksUT1 == 0 && subjectData.MarksUT2 == 0 && subjectData.TotalMarks == 0 &&
                        subjectData.TheoryMarks == 0 && subjectData.PracticalMarks == 0 && subjectData.TotalObtainedMarks == 0 &&
                        subjectData.TheoryMarksUT2 == 0 && subjectData.PracticalMarksUT2 == 0 && subjectData.TotalMarksBothUTs == 0 &&
                        subjectData.TheoryMarksPre1 == 0 && subjectData.PracticalMarksPre1 == 0 && subjectData.TheoryMarksPre2 == 0 &&
                        subjectData.PracticalMarksPre2 == 0 && subjectData.TotalObtainedMarksPre1 == 0 && subjectData.TotalObtainedMarksPre2 == 0 &&
                        subjectData.TheoryMarksSelection == 0 && subjectData.PracticalMarksSelection == 0 && subjectData.TheoryMarksPromotion == 0 && subjectData.PracticalMarksPromotion == 0)
                    {
                        // Set the GradeUT1, GradeUT2, and FinalGrade properties to "AB" (Absent)
                        subjectData.GradeUT1 = "D";
                        subjectData.GradeUT2 = "D";
                        subjectData.FinalGrade = "D";
                        subjectData.GradePre1 = "D";
                        subjectData.GradePre2 = "D";
                        //m
                        // subjectData.FinalGrade = "D";

                    }
                }


                // Check if any of the properties UT1Total, UT2Total, UTAllTotal, TheoryTotalT1, PracticalTotalT1,
                // T1AllTotal, TheoryTotalT2, PracticalTotalT2, T2AllTotal, or OverallAllTotal is equal to 0
                if (totalResult.UT1Total == 0 && totalResult.UT2Total == 0 && totalResult.UTAllTotal == 0 &&
                    totalResult.TheoryTotalT1 == 0 && totalResult.PracticalTotalT1 == 0 && totalResult.T1AllTotal == 0 &&
                    totalResult.TheoryTotalT2 == 0 && totalResult.PracticalTotalT2 == 0 && totalResult.T2AllTotal == 0 &&
                    totalResult.OverallAllTotal == 0)
                {
                    // Set the T1Grade, T2Grade, and OverallGrade properties to "AB" (Absent)
                    totalResult.T1Grade = "D";
                    totalResult.T2Grade = "D";
                    totalResult.OverallGrade = "D";

                }

                if (termId == 7)
                {
                    if (totalResult.Pre1TheoryMaxTotal == 0 && totalResult.Pre1AllTotal == 0)
                    {
                        totalResult.Pre1Grade = "D";
                    }
                }
                if (termId == 7)
                {
                    if (totalResult.Pre2TheoryMaxTotal == 0 && totalResult.Pre2AllTotal == 0)
                    {
                        totalResult.Pre2Grade = "D";
                    }
                }
                List<CoscholasticAreaData> coscholasticAreaDatas = new List<CoscholasticAreaData>();

                var classCoscholastic = _context.tbl_CoScholasticClass.Where(x => x.ClassID == stdInfo.ClassID).Select(x => x.CoscholasticID).ToList();
                var CoscholasticMatchingRecords = _context.tbl_CoScholastic
                .Where(record => classCoscholastic.Contains(record.Id))
                .ToList();


                var resultTermUT1 = (from cr in _context.tbl_CoScholastic_Result
                                     join cog in _context.tbl_CoScholasticObtainedGrade
                                     on cr.Id equals cog.ObtainedCoScholasticID
                                     join c in _context.tbl_CoScholastic
                                     on cog.CoscholasticID equals c.Id
                                     where cr.StudentID == studentId && cr.TermID == 1 && cog.BatchId == batchId
                                     select new
                                     {
                                         CoscholasticID = c.Id,
                                         Title = c.Title,
                                         ObtainedGrade = cog.ObtainedGrade
                                     }).ToList();
                var resultTermUT_1 = (from c in CoscholasticMatchingRecords
                                      join cr in _context.tbl_CoScholastic_Result
                                      on c.Id equals cr.CoScholasticID into crGroup
                                      from cr in crGroup.DefaultIfEmpty()
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                      from cog in cogGroup.DefaultIfEmpty()
                                      where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 1 && cog.BatchId == batchId)
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                      }).ToList();



                var resultTermUT2 = (from cr in _context.tbl_CoScholastic_Result
                                     join cog in _context.tbl_CoScholasticObtainedGrade
                                     on cr.Id equals cog.ObtainedCoScholasticID
                                     join c in _context.tbl_CoScholastic
                                     on cog.CoscholasticID equals c.Id
                                     where cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == batchId
                                     select new
                                     {
                                         CoscholasticID = c.Id,
                                         Title = c.Title,
                                         ObtainedGrade = cog.ObtainedGrade
                                     }).ToList();
                var resultTermUT_2 = (from c in CoscholasticMatchingRecords
                                      join cr in _context.tbl_CoScholastic_Result
                                      on c.Id equals cr.CoScholasticID into crGroup
                                      from cr in crGroup.DefaultIfEmpty()
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                      from cog in cogGroup.DefaultIfEmpty()
                                      where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 2 && cog.BatchId == batchId)
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                      }).ToList();


                var resultTerm0 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == batchId
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                var resultTerm1 = (from c in CoscholasticMatchingRecords
                                   join cr in _context.tbl_CoScholastic_Result
                                   on c.Id equals cr.CoScholasticID into crGroup
                                   from cr in crGroup.DefaultIfEmpty()
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                   from cog in cogGroup.DefaultIfEmpty()
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == batchId)
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                   }).ToList();

                var resultTerm3 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 4 && cog.BatchId == batchId
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();
                var resultTerm2 = (from c in CoscholasticMatchingRecords
                                   join cr in _context.tbl_CoScholastic_Result
                                   on c.Id equals cr.CoScholasticID into crGroup
                                   from cr in crGroup.DefaultIfEmpty()
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                                   from cog in cogGroup.DefaultIfEmpty()
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 4 && cog.BatchId == batchId)
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog?.ObtainedGrade // Use ?. to access ObtainedGrade safely
                                   }).ToList();
                var resultTerm9 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 3 && cog.BatchId == batchId
                                   select new
                                   {
                                       CoscholasticID = c.Id,
                                       Title = c.Title,
                                       ObtainedGrade = cog.ObtainedGrade
                                   }).ToList();


                List<CoscholasticResultModel> coscholasticResultModelsList1 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTerm9.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsList1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTerm9.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsList1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }


                List<CoscholasticResultModel> coscholasticResultModelsListUT1 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTermUT1.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsListUT1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTermUT1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsListUT1.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }

                List<CoscholasticResultModel> coscholasticResultModelsListUT2 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTermUT_1.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTermUT_1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsListUT2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }


                var resultTerm10 = (from cr in _context.tbl_CoScholastic_Result
                                    join cog in _context.tbl_CoScholasticObtainedGrade
                                    on cr.Id equals cog.ObtainedCoScholasticID
                                    join c in _context.tbl_CoScholastic
                                    on cog.CoscholasticID equals c.Id
                                    where cr.StudentID == studentId && cr.TermID == 4 && cog.BatchId == batchId
                                    select new
                                    {
                                        CoscholasticID = c.Id,
                                        Title = c.Title,
                                        ObtainedGrade = cog.ObtainedGrade
                                    }).ToList();
                List<CoscholasticResultModel> coscholasticResultModelsList2 = new List<CoscholasticResultModel>();
                foreach (var item in CoscholasticMatchingRecords)
                {
                    if (resultTerm10.Any(x => x.CoscholasticID == item.Id))
                    {
                        coscholasticResultModelsList2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = resultTerm10.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                        });
                    }
                    else
                    {
                        coscholasticResultModelsList2.Add(new CoscholasticResultModel
                        {
                            CoscholasticID = item.Id,
                            Title = item.Title,
                            ObtainedGrade = null
                        });
                    }
                }

                List<CoscholasticResultModel> coscholasticResultModelsList3 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList4 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList5 = new List<CoscholasticResultModel>();
                List<CoscholasticResultModel> coscholasticResultModelsList6 = new List<CoscholasticResultModel>();
                if (termId != 10)
                {
                    var resultPre1 = (from cr in _context.tbl_CoScholastic_Result
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr.Id equals cog.ObtainedCoScholasticID
                                      join c in _context.tbl_CoScholastic
                                      on cog.CoscholasticID equals c.Id
                                      where cr.StudentID == studentId && cr.TermID == 7 && cog.BatchId == batchId
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog.ObtainedGrade
                                      }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultPre1.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList3.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultPre1.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList3.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                    var resultPre2 = (from cr in _context.tbl_CoScholastic_Result
                                      join cog in _context.tbl_CoScholasticObtainedGrade
                                      on cr.Id equals cog.ObtainedCoScholasticID
                                      join c in _context.tbl_CoScholastic
                                      on cog.CoscholasticID equals c.Id
                                      where cr.StudentID == studentId && cr.TermID == 8 && cog.BatchId == batchId
                                      select new
                                      {
                                          CoscholasticID = c.Id,
                                          Title = c.Title,
                                          ObtainedGrade = cog.ObtainedGrade
                                      }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultPre2.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList4.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultPre2.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList4.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                    var resultpromotion = (from cr in _context.tbl_CoScholastic_Result
                                           join cog in _context.tbl_CoScholasticObtainedGrade
                                           on cr.Id equals cog.ObtainedCoScholasticID
                                           join c in _context.tbl_CoScholastic
                                           on cog.CoscholasticID equals c.Id
                                           where cr.StudentID == studentId && cr.TermID == 6 && cog.BatchId == batchId
                                           select new
                                           {
                                               CoscholasticID = c.Id,
                                               Title = c.Title,
                                               ObtainedGrade = cog.ObtainedGrade
                                           }).ToList();


                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultpromotion.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList6.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultpromotion.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }

                    var resultSelection = (from cr in _context.tbl_CoScholastic_Result
                                           join cog in _context.tbl_CoScholasticObtainedGrade
                                           on cr.Id equals cog.ObtainedCoScholasticID
                                           join c in _context.tbl_CoScholastic
                                           on cog.CoscholasticID equals c.Id
                                           where cr.StudentID == studentId && cr.TermID == 5 && cog.BatchId == batchId
                                           select new
                                           {
                                               CoscholasticID = c.Id,
                                               Title = c.Title,
                                               ObtainedGrade = cog.ObtainedGrade
                                           }).ToList();
                    foreach (var item in CoscholasticMatchingRecords)
                    {
                        if (resultSelection.Any(x => x.CoscholasticID == item.Id))
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = resultSelection.Where(x => x.CoscholasticID == item.Id).Select(x => x.ObtainedGrade).FirstOrDefault()
                            });
                        }
                        else
                        {
                            coscholasticResultModelsList5.Add(new CoscholasticResultModel
                            {
                                CoscholasticID = item.Id,
                                Title = item.Title,
                                ObtainedGrade = null
                            });
                        }
                    }
                }

                //if (termId != 10)
                //{

                //}
                var combinedResult1 = (from coscholasticId in classCoscholastic
                                       join term1 in coscholasticResultModelsList1
                                       on coscholasticId equals term1.CoscholasticID into term1Group
                                       join term2 in coscholasticResultModelsList2
                                       on coscholasticId equals term2.CoscholasticID into term2Group
                                       join Pre1 in coscholasticResultModelsList3
                                      on coscholasticId equals Pre1.CoscholasticID into Pre1Group
                                       join Pre2 in coscholasticResultModelsList4
                                      on coscholasticId equals Pre2.CoscholasticID into Pre2Group
                                       join UT1 in coscholasticResultModelsListUT1
                                       on coscholasticId equals UT1.CoscholasticID into UT1Group
                                       join UT2 in coscholasticResultModelsListUT2
                                      on coscholasticId equals UT2.CoscholasticID into UT2Group
                                       join Selection in coscholasticResultModelsList5
                                       on coscholasticId equals Selection.CoscholasticID into selectionGroup
                                       join Promotion in coscholasticResultModelsList6
                                       on coscholasticId equals Promotion.CoscholasticID into PrmotionGroup
                                       from UT1 in UT1Group.DefaultIfEmpty()
                                       from UT2 in UT2Group.DefaultIfEmpty()
                                       from term1 in term1Group.DefaultIfEmpty()
                                       from term2 in term2Group.DefaultIfEmpty()
                                       from Pre1 in Pre1Group.DefaultIfEmpty()
                                       from Selection in selectionGroup.DefaultIfEmpty()
                                       from Promotion in PrmotionGroup.DefaultIfEmpty()
                                       from Pre2 in Pre2Group.DefaultIfEmpty()
                                       select new
                                       {
                                           Title = UT1?.Title ?? UT2?.Title ?? term1?.Title ?? term2?.Title ?? Pre1?.Title ?? Pre2?.Title ?? Selection.Title ?? Promotion.Title,
                                           GradeTerm1 = term1?.ObtainedGrade,
                                           GradeTerm2 = term2?.ObtainedGrade,
                                           GradePre1 = Pre1?.ObtainedGrade,
                                           GradeSelection = Selection?.ObtainedGrade,
                                           GradePromotion = Promotion?.ObtainedGrade,
                                           GradePre2 = Pre2?.ObtainedGrade,
                                           GradeUT1 = UT1?.ObtainedGrade,
                                           GradeUT2 = UT2?.ObtainedGrade
                                       }).ToList();

                var combinedResult = (from coscholasticId in classCoscholastic
                                      join ut1 in resultTermUT_1
                                     on coscholasticId equals ut1.CoscholasticID into ut1Group
                                      from ut1 in ut1Group.DefaultIfEmpty()
                                      join ut2 in resultTermUT_2
                                     on coscholasticId equals ut2.CoscholasticID into ut2Group
                                      from ut2 in ut2Group.DefaultIfEmpty()
                                      join term1 in resultTerm1
                                      on coscholasticId equals term1.CoscholasticID into term1Group
                                      from term1 in term1Group.DefaultIfEmpty()
                                      join term2 in resultTerm2
                                      on coscholasticId equals term2.CoscholasticID into term2Group
                                      from term2 in term2Group.DefaultIfEmpty()
                                      select new
                                      {
                                          Title = term1?.Title ?? term2?.Title,
                                          GradeTerm1 = term1?.ObtainedGrade,
                                          GradeTerm2 = term2?.ObtainedGrade
                                      }).ToList();


                // Group the combined result based on CoscholasticID count
                // Group the combined result based on CoscholasticID count
                var groupedResult = combinedResult1.GroupBy(item => item.Title)
                                                  .Select(group => new
                                                  {
                                                      Title = group.Key,
                                                      GradeTerm1 = group.FirstOrDefault(item => item.GradeTerm1 != null)?.GradeTerm1,
                                                      GradeTerm2 = group.FirstOrDefault(item => item.GradeTerm2 != null)?.GradeTerm2,
                                                      GradePre1 = group.FirstOrDefault(item => item.GradePre1 != null)?.GradePre1,
                                                      GradePre2 = group.FirstOrDefault(item => item.GradePre2 != null)?.GradePre2,
                                                      GradeUT1 = group.FirstOrDefault(item => item.GradeUT1 != null)?.GradeUT1,
                                                      GradeUT2 = group.FirstOrDefault(item => item.GradeUT2 != null)?.GradeUT2,
                                                      GradeSelection = group.FirstOrDefault(item => item.GradeSelection != null)?.GradeSelection,
                                                      GradePromotion = group.FirstOrDefault(item => item.GradePromotion != null)?.GradePromotion
                                                  })
                                                  .ToList();


                foreach (var item in groupedResult)
                {
                    CoscholasticAreaData coscholasticAreaData = new CoscholasticAreaData()
                    {
                        Name = item.Title,
                        GradeTerm1 = item.GradeTerm1 ?? "-",
                        GradeTerm2 = item.GradeTerm2 ?? "-",
                        GradePre1 = item.GradePre1 ?? "-",
                        GradeSelection = item.GradeSelection ?? "-",
                        GradePromotion = item.GradePromotion ?? "-",
                        GradePre2 = item.GradePre2 ?? "-",
                        GradeUT1 = item.GradeUT1 ?? "-",
                        GradeUT2 = item.GradeUT2 ?? "-"
                    };
                    coscholasticAreaDatas.Add(coscholasticAreaData);
                }
                var gradinglist = _context.gradingCriteria.Where(x => x.TermID == termId && x.BatchID == batchId && x.ClassID == stdInfo.ClassID).ToList();
                if (termId == 10)
                {
                    gradinglist = _context.gradingCriteria.Where(x => x.TermID == 4 && x.BatchID == batchId && x.ClassID == stdInfo.ClassID).ToList();
                }

                studentReportData.gradingCriteria = gradinglist;

                studentReportData.coscholasticAreaDatas = coscholasticAreaDatas;
                studentReportData.totalResult = totalResult;
                studentReportData.totalResultPercentage = totalResultpercentage;
                studentReportData.subjectDatas = subjectDatas;
                studentReportData.optionalSubjectDatas = optionalsubjectDatas;
                studentReportData.totalResult.T1Grade = studentReportData.totalResult.T1Grade;
                studentReportData.totalResultPercentage.T1Grade = studentReportData.totalResultPercentage.T1Grade;
                string termName = _context.tbl_Term.Where(x => x.TermID == termId).Select(t => t.TermName).FirstOrDefault();
                int DgradeCountUT1 = subjectDatas.Count(x => x.MarksUT1Grade == "D");
                int DgradeCountUT2 = subjectDatas.Count(x => x.MarksUT2Grade == "D");
                int DgradeCountTerm1 = subjectDatas.Count(x => x.GradeUT1 == "D");
                int DgradeCountTerm2 = subjectDatas.Count(x => x.GradeUT2 == "D");
                int DgradeCountPre1 = subjectDatas.Count(x => x.GradePre1 == "D");
                int DgradeCountSelection = subjectDatas.Count(x => x.GradeSelection == "D");
                int DgradeCountPromotion = subjectDatas.Count(x => x.GradePromotion == "D");
                int DgradeCountPre2 = subjectDatas.Count(x => x.GradePre2 == "D");
                int DgradeCountFinal = subjectDatas.Count(x => x.FinalGrade == "D");
                string result;
                switch (termId)
                {
                    case 1:
                        result = DgradeCountUT1 > 0 ? "" : "Pass";
                        break;
                    case 2:
                        result = DgradeCountUT2 > 0 ? "" : "Pass";
                        break;
                    case 3:
                        result = DgradeCountTerm1 > 0 ? "" : "Pass";
                        break;
                    case 4:
                        result = DgradeCountTerm2 > 0 ? "" : "Pass";
                        break;
                    case 6:
                        result = DgradeCountPromotion > 0 ? "" : "Pass";
                        break;
                    case 5:
                        result = DgradeCountSelection > 0 ? "" : "Pass";
                        break;
                    case 7:
                        result = DgradeCountPre1 > 0 ? "" : "Pass";
                        break;
                    case 8:
                        result = DgradeCountPre2 > 0 ? "" : "Pass";
                        break;
                    case 10:
                        result = DgradeCountFinal > 0 ? "" : "Pass";
                        break;
                    default:
                        result = "Invalid grade";
                        break;
                }



                studentReportData.Result = result;
                return Json(studentReportData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        //1
        public static string GetOptionMarkGrade(decimal gradeNumber)
        {
            if (gradeNumber == -1)
            {
                return "AB";
            }
            if (gradeNumber == -2 || gradeNumber == 0)
            {
                return "-";
            }

            if (gradeNumber == 1)
            {
                return "A";
            }
            if (gradeNumber == 2)
            {
                return "B";
            }
            if (gradeNumber == 3)
            {
                return "C";
            }
            if (gradeNumber > 3)
            {
                return "D";
            }

            return "-";
        }

        public class CoscholasticResultModel
        {
            public long CoscholasticID { get; set; }
            public string Title { get; set; }
            public string ObtainedGrade { get; set; }
        }
        //percentage calculate
        public decimal PercentageCal(decimal obtainedMarks, decimal totalMarks)
        {
            decimal percentage = obtainedMarks <= 0 ? 0 : ((decimal)obtainedMarks / totalMarks) * 100; ;
            return Math.Round(percentage, 2);
        }

        public string GetGradebyTerm(decimal percentage, int classid, int termId)
        {
            // Query the database to get the appropriate grade
            var grade = _context.gradingCriteria
                .Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage && classid == g.ClassID && g.TermID == termId)
                .Select(g => g.Grade)
                .FirstOrDefault();

            // If no grade is found (percentage outside the grading range), return "N/A" or handle as needed.
            return grade ?? "E";
        }
        public string GetGradebyTermBatch(decimal percentage, int classid, int termId, int BatchId)
        {
            // Query the database to get the appropriate grade
            if (termId == 10)
                termId = 4;
            var grade = _context.gradingCriteria
                .Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage && classid == g.ClassID && g.TermID == termId && g.BatchID == BatchId)
                .Select(g => g.Grade)
                .FirstOrDefault();

            // If no grade is found (percentage outside the grading range), return "N/A" or handle as needed.
            return grade ?? "D";
        }

        //grade Calculate
        public string GetGrade(decimal percentage, int classid)
        {
            // Query the database to get the appropriate grade
            var grade = _context.gradingCriteria
                .Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage && classid == g.ClassID)
                .Select(g => g.Grade)
                .FirstOrDefault();

            // If no grade is found (percentage outside the grading range), return "N/A" or handle as needed.
            return grade ?? "D";
        }

        //grades
        public JsonResult GetGradeByPercentage(decimal percentage)
        {
            try
            {// Query the database to get the appropriate grade
                var grade = _context.gradingCriteria
                    .Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage)
                    .Select(g => g.Grade)
                    .FirstOrDefault();
                var jsonResult = new
                {
                    Grade = grade ?? "D"
                };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetDescriptionByGrade(string Grade, int classId, int termid)
        {
            if (termid == 10)
                termid = 4;
            try
            {// Query the database to get the appropriate grade
                var GradeDescription = _context.gradingCriteria
                    .Where(x => x.Grade == Grade && x.ClassID == classId && x.TermID == termid)
                    .Select(g => g.GradeDescription)
                    .FirstOrDefault();
                var jsonResult = new
                {
                    GradeDescription = GradeDescription ?? ""
                };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GeAttendance(int StudnetId)
        {

            try
            {// Query the database to get the appropriate grade
                var Attendance = _context.StudentAttendanceCount
                    .Where(x => x.StudentId == StudnetId)
                    .Select(g => new
                    {
                        g.PresentDays,
                        g.TotalDays // Add more fields if needed
                    }).FirstOrDefault();
                var jsonResult = new
                {
                    Attendance = Attendance
                };
                return Json(jsonResult, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AllGrade()
        {
            try
            {
                var grades = _context.gradingCriteria.ToList();
                return Json(grades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult AllGrade2(int classId, int TermId, int BatchId)
        {
            try
            {
                var grades = _context.gradingCriteria.Where(x => x.ClassID == classId && x.TermID == TermId && x.BatchID == BatchId).ToList();
                return Json(grades, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        //Co-Scholastic Areas 
        public ActionResult CoScholastic()
        {
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();

                var coScholastic = _context.tbl_CoScholasticClass.Where(x => x.BoardID == getBoardID).ToList();
                ViewBag.CoScholasticList = _context.tbl_CoScholastic.Where(x => x.BoardID == getBoardID).ToList();
                List<CoScholasticClassList> coScholasticClassLists = new List<CoScholasticClassList>();
                foreach (var item in coScholastic)
                {
                    CoScholasticClassList coScholasticClassList = new CoScholasticClassList()
                    {
                        Id = item.Id,
                        Coscholastic = _context.tbl_CoScholastic.Where(x => x.Id == item.CoscholasticID).Select(x => x.Title).FirstOrDefault(),
                        Class = _context.DataListItems.Where(x => x.DataListItemId == item.ClassID).Select(x => x.DataListItemName).FirstOrDefault()
                    };
                    coScholasticClassLists.Add(coScholasticClassList);
                }
                ViewBag.CoScholasticListTable = coScholasticClassLists;
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();

                ViewBag.ClassList = Classes;
                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // POST: GradingCriteria/Create
        [HttpPost]
        public ActionResult CreateCoScholastic(Tbl_CoScholastic model)
        {
            try
            {
                // Add the new grading criteria to the list
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                model.BoardID = getBoardID;
                _context.tbl_CoScholastic.Add(model);
                _context.SaveChanges();
                // Redirect to the Index action to display the updated list
                return RedirectToAction("CoScholastic");
            }
            catch (Exception ex)
            {

                return RedirectToAction("CoScholastic");
            }



            // If the model state is not valid, return the Create view with the current model

        }
        [HttpPost]
        public ActionResult CreateCoScholasticClass(Tbl_CoScholasticClass model)
        {
            try
            {
                // Add the new grading criteria to the list
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                model.BoardID = getBoardID;
                _context.tbl_CoScholasticClass.Add(model);
                _context.SaveChanges();
                // Redirect to the Index action to display the updated list
                return RedirectToAction("CoScholastic");
            }
            catch (Exception ex)
            {

                return RedirectToAction("CoScholastic");
            }



            // If the model state is not valid, return the Create view with the current model

        }
        public JsonResult GetCoScholasticById(int id)
        {
            try
            {
                var data = _context.tbl_CoScholasticClass.FirstOrDefault(x => x.Id == id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult UpdateCoScholastic(Tbl_CoScholasticClass modal)
        {
            try
            {
                var data = _context.tbl_CoScholasticClass.FirstOrDefault(x => x.Id == modal.Id);
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                modal.BoardID = getBoardID;
                _context.Entry(data).CurrentValues.SetValues(modal);
                _context.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/CoScholastic');</script>");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult DeleteCoScholastic(int Id)
        {
            try
            {
                var data = _context.tbl_CoScholasticClass.FirstOrDefault(x => x.Id == Id);
                if (data != null)
                {
                    _context.tbl_CoScholasticClass.Remove(data);
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Exam/CoScholastic');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //fill Coscholastic
        public ActionResult ObtainedCoScholastic()
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
        public JsonResult CoScholasticStudentByClassSection(int classId, int sectionId, int termId, int batchId)
        {
            List<CoScholasticListStudent> listStudents;
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                var IsAlreadyExist = _context.tbl_CoScholastic_Result.Where(x => x.ClassID == classId && x.SectionId == sectionId && x.TermID == termId).ToList();
                if (IsAlreadyExist.Count() > 0)
                {
                    List<Tbl_CoScholastic> coScholastic;
                    CoScholasticStudentGrid(getBoardID, termId, classId, sectionId, batchId, out listStudents, out coScholastic, IsAlreadyExist);
                    var result = new
                    {
                        IsUpdate = false,
                        data = listStudents.OrderBy(x => x.StudentName),
                        HeaderData = coScholastic
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    List<Tbl_CoScholastic> coScholastic;
                    CoScholasticStudentGrid(getBoardID, termId, classId, sectionId, batchId, out listStudents, out coScholastic, new List<Tbl_CoScholastic_Result>());
                    var result = new
                    {
                        IsUpdate = false,
                        data = listStudents.OrderBy(x => x.StudentName),
                        HeaderData = coScholastic
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }
        public void CoScholasticStudentGrid(long getBoardID, int termId, int classId, int sectionId, int batchId, out List<CoScholasticListStudent> listStudents, out List<Tbl_CoScholastic> coScholastic, List<Tbl_CoScholastic_Result> IsAlreadyExist)
        {
            listStudents = new List<CoScholasticListStudent>();
            var coScholasticClassList = _context.tbl_CoScholasticClass
                                      .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                      .ToList();

            coScholastic = coScholasticClassList.Select(item => new Tbl_CoScholastic
            {
                Id = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID).Id,
                Title = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID)?.Title
                // Title = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.Id)?.Title
            }).ToList();

            if (IsAlreadyExist.Count() > 0)
            {
                var _studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false && x.Batch_Id == batchId).OrderBy(x => x.Name).ToList();

                var stdList = _context.tbl_CoScholastic_Result.Where(x => x.ClassID == classId && x.SectionId == sectionId && x.TermID == termId).ToList();
                //var stdList = from cos  in _context.tbl_CoScholastic_Result join
                //              st  in _studentlist on cos.StudentID equals st.StudentId where cos.ClassID = classId  cos.SectionId == sectionId
                //              && cos.TermID=termId && st.Batch_Id==batchId

                //var stdList = (from cos in _context.tbl_CoScholastic_Result
                //              join
                //             st in _studentlist on cos.StudentID equals  st.StudentId select new
                //             {
                //                 cos.Id,cos.StudentID,cos.CoScholasticID,cos.BoardID,cos.SectionId,cos.ClassID,
                //                 cos.TermID,st.Batch_Id

                //             })
                //              .Where(x => x.ClassID == classId && x.SectionId == sectionId && x.TermID == termId && x.Batch_Id==batchId).ToList();
                if (_studentlist.Count == 0)
                {
                    foreach (var item in IsAlreadyExist)
                    {
                        item.SectionId = item.SectionId;//_context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                        CoScholasticListStudent listStudent = new CoScholasticListStudent()
                        {
                            StudentId = item.StudentID,
                            //StudentName = _context.Students.Where(x => x.StudentId == item.StudentID && x.Batch_Id == batchId && x.Section_Id == sectionId).Select(x => x.Name).FirstOrDefault()
                            StudentName = _context.Students.Where(x => x.StudentId == item.StudentID).Select(x => x.Name).FirstOrDefault()
                        };
                        listStudents.Add(listStudent);
                        List<CoscholastiStuentObtData> coscholastiStuentObtDataList = new List<CoscholastiStuentObtData>();
                        List<tbl_CoScholasticObtainedGrade> commonItems = new List<tbl_CoScholasticObtainedGrade>();
                        string currentObtainedGrade = "";

                        if (IsAlreadyExist.Count() > 0)
                        {
                            var ObtainedCoScholasticID = IsAlreadyExist.Where(x1 => x1.StudentID == item.StudentID).Select(x2 => x2.Id).FirstOrDefault();
                            var coScholasticClassListNew = _context.tbl_CoScholasticClass
                                            .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                            .ToList();
                            var studentObtainedData = _context.tbl_CoScholasticObtainedGrade.Where(x => x.ObtainedCoScholasticID == ObtainedCoScholasticID && x.BatchId == batchId).ToList();
                            commonItems = (from studentData in studentObtainedData
                                           join classData in coScholasticClassListNew
                                           on studentData.CoscholasticID equals classData.CoscholasticID
                                           select studentData).ToList();
                        }

                        foreach (var data in coScholastic)
                        {
                            if (IsAlreadyExist.Count() > 0)
                            {
                                currentObtainedGrade = commonItems.Where(a => a.CoscholasticID == data.Id).Select(a => a.ObtainedGrade).FirstOrDefault();
                            }

                            CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                            {
                                CoscholasticID = data.Id,
                                ObtainedGrade = currentObtainedGrade,
                                CoscholasticName = data.Title
                            };
                            coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                        }
                        listStudent.coscholastiStuentObtDatas = coscholastiStuentObtDataList;
                    }

                }

                foreach (var item in _studentlist)
                {
                    item.Section_Id = item.Section_Id;//_context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                    CoScholasticListStudent listStudent = new CoScholasticListStudent()
                    {
                        StudentId = item.StudentId,
                        StudentName = _context.Students.Where(x => x.StudentId == item.StudentId && x.Batch_Id == batchId && x.Section_Id == sectionId && x.Class_Id == classId).Select(x => x.Name).FirstOrDefault()
                    };
                    listStudents.Add(listStudent);
                    List<CoscholastiStuentObtData> coscholastiStuentObtDataList = new List<CoscholastiStuentObtData>();
                    List<tbl_CoScholasticObtainedGrade> commonItems = new List<tbl_CoScholasticObtainedGrade>();
                    string currentObtainedGrade = "";

                    if (IsAlreadyExist.Count() > 0)
                    {
                        var ObtainedCoScholasticID = IsAlreadyExist.Where(x1 => x1.StudentID == item.StudentId).Select(x2 => x2.Id).FirstOrDefault();
                        var coScholasticClassListNew = _context.tbl_CoScholasticClass
                                        .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                        .ToList();
                        var studentObtainedData = _context.tbl_CoScholasticObtainedGrade.Where(x => x.ObtainedCoScholasticID == ObtainedCoScholasticID && x.BatchId == batchId).ToList();
                        commonItems = (from studentData in studentObtainedData
                                       join classData in coScholasticClassListNew
                                       on studentData.CoscholasticID equals classData.CoscholasticID
                                       select studentData).ToList();
                    }

                    foreach (var data in coScholastic)
                    {
                        if (IsAlreadyExist.Count() > 0)
                        {
                            currentObtainedGrade = commonItems.Where(a => a.CoscholasticID == data.Id).Select(a => a.ObtainedGrade).FirstOrDefault();
                        }

                        CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                        {
                            CoscholasticID = data.Id,
                            ObtainedGrade = currentObtainedGrade,
                            CoscholasticName = data.Title
                        };
                        coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                    }
                    listStudent.coscholastiStuentObtDatas = coscholastiStuentObtDataList;





                }
            }
            else
            {
                var studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false && x.Batch_Id == batchId).OrderBy(x => x.Name).ToList();
                foreach (var item in studentlist)
                {
                    item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                    CoScholasticListStudent listStudent = new CoScholasticListStudent()
                    {
                        StudentId = item.StudentId,
                        StudentName = item.Name
                    };
                    listStudents.Add(listStudent);
                    List<CoscholastiStuentObtData> coscholastiStuentObtDataList = new List<CoscholastiStuentObtData>();
                    List<tbl_CoScholasticObtainedGrade> commonItems = new List<tbl_CoScholasticObtainedGrade>();
                    string currentObtainedGrade = "";

                    if (IsAlreadyExist.Count() > 0)
                    {
                        var ObtainedCoScholasticID = IsAlreadyExist.Where(x1 => x1.StudentID == item.StudentId).Select(x2 => x2.Id).FirstOrDefault();
                        var coScholasticClassListNew = _context.tbl_CoScholasticClass
                                        .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                        .ToList();
                        var studentObtainedData = _context.tbl_CoScholasticObtainedGrade.Where(x => x.ObtainedCoScholasticID == ObtainedCoScholasticID).ToList();
                        commonItems = (from studentData in studentObtainedData
                                       join classData in coScholasticClassListNew
                                       on studentData.CoscholasticID equals classData.CoscholasticID
                                       select studentData).ToList();
                    }

                    foreach (var data in coScholastic)
                    {
                        if (IsAlreadyExist.Count() > 0)
                        {
                            currentObtainedGrade = commonItems.Where(a => a.CoscholasticID == data.Id).Select(a => a.ObtainedGrade).FirstOrDefault();
                        }

                        CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                        {
                            CoscholasticID = data.Id,
                            ObtainedGrade = currentObtainedGrade,
                            CoscholasticName = data.Title
                        };
                        coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                    }
                    listStudent.coscholastiStuentObtDatas = coscholastiStuentObtDataList;
                }
            }





        }
        public JsonResult CoScholasticStudentByClassSection_bkp(int classId, int sectionId, int termId)
        {
            List<CoScholasticListStudent> listStudents = new List<CoScholasticListStudent>();
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                var IsAlreadyExist = _context.tbl_CoScholastic_Result.Where(x => x.ClassID == classId && x.SectionId == sectionId && x.TermID == termId).ToList();
                if (IsAlreadyExist.Count() > 0)
                {
                    var studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
                    foreach (var item in studentlist)
                    {
                        item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                        CoScholasticListStudent listStudent = new CoScholasticListStudent()
                        {
                            StudentId = item.StudentId,
                            StudentName = item.Name
                        };

                        List<CoscholastiStuentObtData> coscholastiStuentObtDataList = new List<CoscholastiStuentObtData>();
                        var ObtainedCoScholasticID = IsAlreadyExist.Where(x1 => x1.StudentID == item.StudentId).Select(x2 => x2.Id).FirstOrDefault();
                        var coScholasticClassListNew = _context.tbl_CoScholasticClass
                                        .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                        .ToList();
                        var studentObtainedData = _context.tbl_CoScholasticObtainedGrade.Where(x => x.ObtainedCoScholasticID == ObtainedCoScholasticID).ToList();
                        var commonItems = (from studentData in studentObtainedData
                                           join classData in coScholasticClassListNew
                                           on studentData.CoscholasticID equals classData.CoscholasticID
                                           select studentData).ToList();
                        foreach (var data in commonItems)
                        {
                            CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                            {
                                CoscholasticID = data.CoscholasticID,
                                ObtainedGrade = data.ObtainedGrade,
                                CoscholasticName = _context.tbl_CoScholastic.Where(x => x.Id == data.CoscholasticID).Select(x => x.Title).FirstOrDefault()
                            };
                            coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                        }
                        //var coScholasticClassList1 = _context.tbl_CoScholasticClass
                        //               .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                        //               .ToList();
                        //var coScholastic1 = coScholasticClassList1.Select(item1 => new Tbl_CoScholastic
                        //{
                        //    Id = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item1.CoscholasticID).Id,
                        //    Title = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item1.CoscholasticID)?.Title
                        //}).ToList();
                        //for (int i = 0; i < studentObtainedData.Count; i++)
                        //{
                        //    // Check if the index is within bounds of coScholastic list
                        //    if (i < coScholastic1.Count)
                        //    {
                        //        CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                        //        {
                        //            CoscholasticID = coScholastic1[i].Id,
                        //            ObtainedGrade = studentObtainedData[i].ObtainedGrade,
                        //            CoscholasticName = coScholastic1[i].Title
                        //        };
                        //        coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                        //    }
                        //    else
                        //    {
                        //        CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                        //        {
                        //            CoscholasticID = studentObtainedData[i].CoscholasticID,
                        //            ObtainedGrade = studentObtainedData[i].ObtainedGrade,
                        //            CoscholasticName = _context.tbl_CoScholastic.Where(x => x.Id == studentObtainedData[i].CoscholasticID).Select(x => x.Title).FirstOrDefault()
                        //        };
                        //        coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                        //    }
                        //}
                        listStudent.coscholastiStuentObtDatas = coscholastiStuentObtDataList;
                        listStudents.Add(listStudent);
                    }


                    var coScholasticClassList = _context.tbl_CoScholasticClass
                                        .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                        .ToList();

                    var coScholastic = coScholasticClassList.Select(item => new Tbl_CoScholastic
                    {
                        Id = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID).Id,
                        Title = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID)?.Title
                    }).ToList();

                    var result = new
                    {
                        IsUpdate = true,
                        data = listStudents.OrderBy(x => x.StudentName),
                        HeaderData = coScholastic
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    var coScholasticClassList = _context.tbl_CoScholasticClass
                                       .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                       .ToList();

                    var coScholastic = coScholasticClassList.Select(item => new Tbl_CoScholastic
                    {
                        Id = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID).Id,
                        Title = _context.tbl_CoScholastic.FirstOrDefault(x => x.Id == item.CoscholasticID)?.Title
                    }).ToList();

                    var studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();

                    foreach (var item in studentlist)
                    {
                        item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                        CoScholasticListStudent listStudent = new CoScholasticListStudent()
                        {
                            StudentId = item.StudentId,
                            StudentName = item.Name
                        };
                        listStudents.Add(listStudent);
                        List<CoscholastiStuentObtData> coscholastiStuentObtDataList = new List<CoscholastiStuentObtData>();

                        foreach (var data in coScholastic)
                        {
                            CoscholastiStuentObtData coscholastiStuentObtData = new CoscholastiStuentObtData()
                            {
                                CoscholasticID = data.Id,
                                ObtainedGrade = "",
                                CoscholasticName = data.Title
                            };
                            coscholastiStuentObtDataList.Add(coscholastiStuentObtData);
                        }
                        listStudent.coscholastiStuentObtDatas = coscholastiStuentObtDataList;
                    }





                    var result = new
                    {
                        IsUpdate = false,
                        data = listStudents.OrderBy(x => x.StudentName),
                        HeaderData = coScholastic
                    };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult CoScholasticByClassId()
        {
            var CoScholastic = _context.tbl_CoScholastic.ToList();
            return Json(CoScholastic, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult InsertUpdateCoScholasticObtainedMarks(List<CoScholasticObtainedModel> rowData)
        {


            // bool hasMatchingRecords = _context.tbl_CoScholastic_Result.Any(tr => tr.Id == testId);
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                // Update existing records in Tbl_TestRecord
                foreach (var item in rowData)
                {
                    var existingRecord = _context.tbl_CoScholastic_Result.FirstOrDefault(tr => tr.TermID == item.TermID && tr.StudentID == item.StudentID && tr.BoardID == getBoardID && tr.ClassID == item.ClassID);
                    if (existingRecord != null)
                    {
                        var existingData1 = _context.tbl_CoScholasticObtainedGrade.Where(data => data.ObtainedCoScholasticID == existingRecord.Id).ToList();
                        for (int i = 0; i < item.CoscholasticData.Count; i++)
                        {
                            var Dt = item.CoscholasticData[i];

                            if (existingData1.Count > 0)
                            {
                                // Update existing data
                                existingData1[i].CoscholasticID = Dt.CoscholasticID;
                                existingData1[i].ObtainedGrade = Dt.ObtainedGrade;
                                Dt.BatchId = item.BatchId;
                                _context.Entry(existingData1[i]).State = EntityState.Modified;
                                _context.tbl_CoScholasticObtainedGrade.Add(Dt);
                                _context.SaveChanges();
                            }
                            else
                            {
                                // Add new CoScholasticData for the existing record
                                Dt.ObtainedCoScholasticID = existingRecord.Id;
                                Dt.BatchId = item.BatchId;
                                _context.tbl_CoScholasticObtainedGrade.Add(Dt);
                                _context.SaveChanges();
                            }

                        }
                        // Update the associated CoScholasticData
                        //foreach (var Dt in item.CoscholasticData)
                        //{
                        //    var existingData = _context.tbl_CoScholasticObtainedGrade.FirstOrDefault(data => data.ObtainedCoScholasticID == existingRecord.Id && data.CoscholasticID == Dt.CoscholasticID);
                        //    if (existingData != null)
                        //    {
                        //        // Update existing data
                        //        //existingData.ObtainedCoScholasticID = Dt.ObtainedCoScholasticID;
                        //        existingData.ObtainedGrade = Dt.ObtainedGrade;
                        //    }
                        //    else
                        //    {
                        //        // Add new CoScholasticData for the existing record
                        //        Dt.ObtainedCoScholasticID = existingRecord.Id;
                        //        _context.tbl_CoScholasticObtainedGrade.Add(Dt);
                        //    }
                        //}

                        _context.SaveChanges();
                        //return Json(new { success = true, errormsg="Data Updated .." });

                    }
                    else
                    {
                        item.BoardID = getBoardID;

                        // This item doesn't have a corresponding record in Tbl_TestRecord, so add it as a new record
                        Tbl_CoScholastic_Result tbl_CoScholastic_Result = new Tbl_CoScholastic_Result()
                        {
                            BoardID = getBoardID,
                            TermID = item.TermID,
                            StudentID = item.StudentID,
                            ClassID = item.ClassID,
                            SectionId = item.SectionId
                        };
                        _context.tbl_CoScholastic_Result.Add(tbl_CoScholastic_Result);
                        _context.SaveChanges();
                        long latestId = tbl_CoScholastic_Result.Id;
                        foreach (var Dt in item.CoscholasticData)
                        {
                            Dt.BatchId = item.BatchId;
                            Dt.ObtainedCoScholasticID = latestId;
                            _context.tbl_CoScholasticObtainedGrade.Add(Dt);
                        }
                    }
                }
                _context.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message, success = false });
            }
            // Save changes to the database
            // Return a success response if the data is saved successfully
        }
        //assgine student elective subjects
        //fill Coscholastic
        public ActionResult AssignElectiveSubejct()
        {
            try
            {
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                ViewBag.ClassList = Classes;
                var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                ViewBag.SectionList = Section;

                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JsonResult GetStuentByClassSectionElectiveSubject(int classId, int sectionId)
        {
            List<ElectiveListStudent> listStudents = new List<ElectiveListStudent>();
            bool IsUpdated = false;
            try
            {
                //var electiveSubjects= _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Subject").DataListId.ToString() && e.IsElective==true).ToList();
                var clasElective = _context.tbl_ClassSubject.Where(x => x.ClassId == classId && x.IsElective == true).Select(x => x.SubjectId).ToList();


                var electiveSubjects = _context.Tbl_SubjectsSetup
                                              .Where(x => clasElective.Contains(x.Subject_ID))
                                              .ToList();
                var students = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
                foreach (var item in students)
                {
                    ElectiveListStudent electiveListStudent = new ElectiveListStudent();
                    List<ELectiveRecord> tbl_Student_ElectiveRecordsList = new List<ELectiveRecord>();
                    electiveListStudent.StudentId = item.StudentId;
                    electiveListStudent.StudentName = item.Name;
                    foreach (var obj in electiveSubjects)
                    {
                        var IsAlreadyExist = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == item.StudentId && x.ElectiveSubjectId == obj.Subject_ID).FirstOrDefault();
                        ELectiveRecord tbl_Student_ElectiveRecord = new ELectiveRecord();

                        if (IsAlreadyExist != null)
                        {
                            tbl_Student_ElectiveRecord.Subject_ID = obj.Subject_ID;
                            tbl_Student_ElectiveRecord.Subject_Name = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == obj.Subject_ID).Select(x => x.Subject_Name).FirstOrDefault();
                            tbl_Student_ElectiveRecord.IsSelected = true;
                        }
                        else
                        {
                            tbl_Student_ElectiveRecord.Subject_ID = obj.Subject_ID;
                            tbl_Student_ElectiveRecord.Subject_Name = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == obj.Subject_ID).Select(x => x.Subject_Name).FirstOrDefault();
                            tbl_Student_ElectiveRecord.IsSelected = false;
                        }
                        tbl_Student_ElectiveRecordsList.Add(tbl_Student_ElectiveRecord);
                    }
                    electiveListStudent.ElectiveSubjectId = tbl_Student_ElectiveRecordsList;
                    listStudents.Add(electiveListStudent);
                }

                var result = new
                {
                    IsUpdate = IsUpdated,
                    data = listStudents,
                    HeaderData = electiveSubjects
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpPost]
        public ActionResult InsertUpdateElectiveSubject(List<TblStudentElectiveRecord> rowData)
        {
            try
            {
                // Update existing records in Tbl_Student_ElectiveRecord
                foreach (var item in rowData)
                {
                    var existingRecords = _context.tbl_Student_ElectiveRecord.Where(tr => tr.StudentId == item.StudentId);
                    if (existingRecords != null)
                    {

                        _context.tbl_Student_ElectiveRecord.RemoveRange(existingRecords);
                        _context.SaveChanges();
                    }
                    if (item.ElectiveSubjectId != null)
                    {
                        foreach (var obj in item.ElectiveSubjectId)
                        {
                            Tbl_Student_ElectiveRecord tbl_Student_ElectiveRecord = new Tbl_Student_ElectiveRecord
                            {
                                StudentId = item.StudentId,
                                ElectiveSubjectId = obj

                            };
                            _context.tbl_Student_ElectiveRecord.Add(tbl_Student_ElectiveRecord);
                        }
                    }

                }
                // Save changes to the database
                _context.SaveChanges();
                return Json(new { success = true }); // Return a success response if the data is saved successfully
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult SaveJson(string key, string content, string fileName)
        {
            try
            {
                // Load existing JSON content if the file already exists
                string filePath = Server.MapPath("~/App_Data/" + fileName + "_encodedContent.json");
                string existingJson = System.IO.File.Exists(filePath) ? System.IO.File.ReadAllText(filePath) : "{}";

                // Parse the existing JSON content
                var jsonData = JObject.Parse(existingJson);

                // Add or update the new key-value pair
                jsonData[key] = content;

                // Convert the JSON object back to a string
                string updatedJson = jsonData.ToString();

                // Save the updated JSON content to the file
                System.IO.File.WriteAllText(filePath, updatedJson);

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }
        public ActionResult GetEncodedContent(string id)
        {
            try
            {
                string filePath = Server.MapPath("~/App_Data/" + id + "_encodedContent.json");
                string jsonContent = System.IO.File.ReadAllText(filePath);
                return Json(jsonContent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }
        //report card setting 
        public ActionResult ReportCardSetting()
        {
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            var getBoardName = _context.schoolBoards.Where(x => x.BoardID == getBoardID).FirstOrDefault();

            var List = _context.reportCardSetting.Where(x => x.BoardId == getBoardID).ToList();
            var Term = _context.tbl_Term.ToList();
            //ViewBag.Subject = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "subject").DataListId.ToString()).ToList();
            // ViewBag.Subject = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.TestList = List;
            ViewBag.Term = Term;
            return View();
        }
        public class TblStudentElectiveRecord
        {
            public long Id { get; set; }
            public long StudentId { get; set; }
            public List<long> ElectiveSubjectId { get; set; }
        }
        //custom module for tests
        public class TestsTable
        {
            public long TestID { get; set; }
            public long ClassID { get; set; }
            public string ClassName { get; set; }
            public long SubjectID { get; set; }
            public string Subject { get; set; }
            public string TestName { get; set; }
            public string TestType { get; set; }
            public decimal MaximumMarks { get; set; }
            public long TermID { get; set; }
            public string Term { get; set; }
            public long BoardID { get; set; }
        }
        //custom module for show student
        public class ListStudent
        {
            public long StudentId { get; set; }
            public string StudentName { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }
            public string TestName { get; set; }
            public string TestType { get; set; }
            public string Subject { get; set; }
            public string Term { get; set; }
            public decimal ObtainedMarks { get; set; }
            public decimal MaximumMarks { get; set; }
            public int BatchId { get; set; }
            public string BatchName { get; set; }

            public string CurrentYear { get; set; }
            public List<StudentTestObtMarks> studentTestObtMarks { get; set; }
            public bool IsFreeze { get; set; }
            public bool IsHold { get; set; }
        }
        //custom subject data
        public class SubjectData
        {
            public string Subject { get; set; }
            public decimal MarksUT1 { get; set; }
            public decimal MaxMarksUT1 { get; set; }
            public string MarksUT1Grade { get; set; }

            public decimal MarksUT2 { get; set; }
            public decimal MaxMarksUT2 { get; set; }
            public string MarksUT2Grade { get; set; }
            public decimal TotalMarks { get; set; }
            public decimal TheoryMarks { get; set; }
            public decimal ProjectMarks { get; set; }
            public decimal SubjectEnrichMarks { get; set; }
            public decimal PracticalMarks { get; set; }
            public decimal TotalObtainedMarks { get; set; }
            public string GradeUT1 { get; set; }
            public string GradeSelection { get; set; }
            public string GradePromotion { get; set; }
            public decimal TheoryMarksUT2 { get; set; }
            public decimal PracticalMarksUT2 { get; set; }
            public decimal ProjectMarksUT2 { get; set; }
            public decimal SubjectEnrichMarksUT2 { get; set; }
            public decimal TotalObtainedMarksUT2 { get; set; }
            public string GradeUT2 { get; set; }
            public decimal TotalMarksBothUTs { get; set; }
            public string FinalGrade { get; set; }
            public decimal MaxMarksTerm1Practical { get; set; }
            public decimal MaxMarksTerm1Theory { get; set; }
            public decimal MaxMarksTerm1Project { get; set; }

            public decimal MaxMarksTerm1SubjectEnrich { get; set; }
            public decimal MaxMarksTerm2Practical { get; set; }
            public decimal MaxMarksTerm2Theory { get; set; }
            public decimal MaxMarksTerm2Project { get; set; }
            public decimal MaxMarksTerm2SubjectEnrich { get; set; }

            public decimal MaxMarksPre1Practical { get; set; }
            public decimal MaxMarksPre1Theory { get; set; }
            public decimal MaxMarksPre2Practical { get; set; }
            public decimal MaxMarksPre2Theory { get; set; }
            public string GradePre1 { get; set; }

            public decimal TheoryMarksSelection { get; set; }
            public decimal MaxMarksSelectionPractical { get; set; }
            public decimal MaxMarksSelectionTheory { get; set; }
            public decimal PracticalMarksSelection { get; set; }
            public decimal TotalObtainedMarksSelection { get; internal set; }


            public decimal TheoryMarksPromotion { get; set; }
            public decimal MaxMarksPromotionPractical { get; set; }
            public decimal MaxMarksPromotionTheory { get; set; }
            public decimal PracticalMarksPromotion { get; set; }
            public decimal TotalObtainedMarksPromotion { get; internal set; }


            public decimal TheoryMarksPre1 { get; internal set; }
            public decimal PracticalMarksPre1 { get; internal set; }
            public decimal TotalObtainedMarksPre1 { get; internal set; }

            public decimal TheoryMarksPre2 { get; internal set; }
            public decimal PracticalMarksPre2 { get; internal set; }
            public decimal TotalObtainedMarksPre2 { get; internal set; }
            public string GradePre2 { get; internal set; }
        }
        //custom optional subject  data
        public class OptionalSubjectData
        {
            public string Subject { get; set; }
            public decimal MarksUT1 { get; set; }
            public decimal MaxMarksUT1 { get; set; }
            public string MarksUT1Grade { get; set; }

            public decimal MarksUT2 { get; set; }
            public decimal MaxMarksUT2 { get; set; }
            public string MarksUT2Grade { get; set; }
            public decimal TotalMarks { get; set; }
            public decimal TheoryMarks { get; set; }
            public decimal PracticalMarks { get; set; }
            public decimal TotalObtainedMarks { get; set; }
            public string GradeUT1 { get; set; }
            public decimal TheoryMarksUT2 { get; set; }
            public decimal PracticalMarksUT2 { get; set; }
            public decimal TotalObtainedMarksUT2 { get; set; }
            public string GradeUT2 { get; set; }
            public decimal TheoryMarksPre1 { get; internal set; }
            public decimal PracticalMarksPre1 { get; internal set; }
            public decimal TotalObtainedMarksPre1 { get; internal set; }
            public decimal TotalObtainedMarkSelection { get; internal set; }
            public string GradePre1 { get; internal set; }
            public string GradeSelection { get; internal set; }
            public decimal TheoryMarksPre2 { get; internal set; }
            public decimal PracticalMarksPre2 { get; internal set; }
            public decimal TotalObtainedMarksPre2 { get; internal set; }
            public string GradePre2 { get; internal set; }


            public decimal TheoryMarksSelection { get; set; }
            public decimal PracticalMarksSelection { get; set; }
            public decimal TheoryMarksPromotion { get; set; }
            public decimal PracticalMarksPromotion { get; set; }
            public decimal TotalObtainedMarkPromotion { get; internal set; }
            public string GradePromotion { get; set; }
        }
        //custom studentData
        public class StudentReportData
        {
            public long studentID { get; set; }
            public string studentName { get; set; }
            public string LastName { get; set; }
            public string fatherName { get; set; }
            public string motherName { get; set; }
            public string scholarNo { get; set; }
            public string rollNo { get; set; }
            public string className { get; set; }
            public long classID { get; set; }
            public string sectionName { get; set; }
            public string dateOfBirth { get; set; }
            public string attandence { get; set; }
            public string academicYear { get; set; }
            public string promotedClass { get; set; }
            public string staffSignatureLink { get; set; }
            public string Remark { get; set; }
            public string Result { get; set; }
            public string ProfileAvatar { get; set; }
            public string Rank { get; set; }
            public List<SubjectData> subjectDatas { get; set; }
            public TotalResult totalResult { get; set; }
            public TotalResultpercentage totalResultPercentage { get; set; }
            public List<CoscholasticAreaData> coscholasticAreaDatas { get; set; }
            public List<OptionalSubjectData> optionalSubjectDatas { get; set; }
            public List<GradingCriteria> gradingCriteria { get; set; }
        }


        //Total
        public class TotalResult
        {
            public decimal UT1Total { get; set; }
            public decimal UT1MaxTotal { get; set; }
            public string UT1TotalGrade { get; set; }
            public decimal SelectionTotal { get; set; }
            public decimal SelectionMaxTotal { get; set; }
            public string SelectionTotalGrade { get; set; }


            public decimal UT2Total { get; set; }
            public decimal UT2MaxTotal { get; set; }
            public string UT2TotalGrade { get; set; }
            public decimal UTAllTotal { get; set; }
            public decimal TheoryTotalT1 { get; set; }
            public decimal PracticalTotalT1 { get; set; }

            public decimal ProjectTotalT1 { get; set; }
            public decimal SubjectEnrichTotalT1 { get; set; }
            public decimal ProjectTotalT2 { get; set; }
            public decimal SubjectEnrichTotalT2 { get; set; }

            public string T1Grade { get; set; }
            public decimal T1AllTotal { get; set; }
            public decimal TheoryTotalT2 { get; set; }
            public decimal PracticalTotalT2 { get; set; }
            public string T2Grade { get; set; }
            public decimal T2AllTotal { get; set; }
            public decimal OverallAllTotal { get; set; }
            public string OverallGrade { get; set; }
            public decimal Term1TheoryMaxTotal { get; set; }
            public decimal Term1PracticalMaxTotal { get; set; }
            public decimal Term2TheoryMaxTotal { get; set; }
            public decimal Term2PracticalMaxTotal { get; set; }
            public decimal Pre1TheoryMaxTotal { get; internal set; }
            public decimal Pre1PracticalMaxTotal { get; internal set; }
            public decimal Pre2TheoryMaxTotal { get; internal set; }
            public decimal Pre2PracticalMaxTotal { get; internal set; }
            public string Pre1Grade { get; internal set; }
            public string SelectionGrade { get; internal set; }
            public string PromotionGrade { get; internal set; }
            public decimal SelectionTheoryMaxTotal { get; internal set; }
            public decimal SelectionPracticalMaxTotal { get; internal set; }
            public decimal PromotionTheoryMaxTotal { get; internal set; }
            public decimal PromotionPracticalMaxTotal { get; internal set; }
            public string Pre2Grade { get; internal set; }
            public decimal Pre1AllTotal { get; internal set; }
            public decimal SelectionAllTotal { get; internal set; }
            public decimal PromotionAllTotal { get; internal set; }
            public decimal Pre2AllTotal { get; internal set; }
        }
        //Total percentage
        public class TotalResultpercentage
        {
            public decimal UT1Total { get; set; }
            public string UT1TotalGrade { get; set; }

            public decimal UT2Total { get; set; }
            public string UT2TotalGrade { get; set; }
            public decimal UTAllTotal { get; set; }
            public decimal TheoryTotalT1 { get; set; }
            public decimal PracticalTotalT1 { get; set; }
            public decimal ProjectTotalT1 { get; set; }
            public decimal SubjectEnrichTotalT1 { get; set; }
            public string T1Grade { get; set; }
            public decimal T1AllTotal { get; set; }
            public decimal TheoryTotalT2 { get; set; }
            public decimal PracticalTotalT2 { get; set; }
            public decimal ProjectTotalT2 { get; set; }
            public decimal SubjectEnrichTotalT2 { get; set; }
            public string T2Grade { get; set; }
            public decimal T2AllTotal { get; set; }
            public decimal OverallAllTotal { get; set; }
            public string OverallGrade { get; set; }
            public decimal TheoryTotalPre1 { get; internal set; }
            public decimal PracticalTotalPre1 { get; internal set; }
            public decimal Pre1AllTotal { get; internal set; }
            public string Pre1Grade { get; internal set; }
            public decimal TheoryTotalSelection { get; internal set; }
            public decimal PracticalTotalSelection { get; internal set; }
            public decimal TheoryTotalPromotion { get; internal set; }
            public decimal PracticalTotalPromotion { get; internal set; }
            public decimal SelectionAllTotal { get; internal set; }
            public decimal PromotionAllTotal { get; internal set; }
            public string SelectionGrade { get; internal set; }
            public string PromotionGrade { get; internal set; }
            public decimal TheoryTotalPre2 { get; internal set; }
            public decimal PracticalTotaPre2 { get; internal set; }
            public decimal Pre2AllTotal { get; internal set; }
            public string Pre2Grade { get; internal set; }
            public decimal PracticalTotalPre2 { get; internal set; }
        }

        //list coscholastic student
        public class CoScholasticListStudent
        {
            public long StudentId { get; set; }
            public string StudentName { get; set; }
            public string ClassName { get; set; }
            public string SectionName { get; set; }
            public string Title { get; set; }
            public string Term { get; set; }
            public decimal ObtainedMarks { get; set; }
            public List<CoscholastiStuentObtData> coscholastiStuentObtDatas { get; set; }
        }

        //list coscholastic student
        public class CoScholasticClassList
        {
            public long Id { get; set; }
            public string Class { get; set; }
            public string Coscholastic { get; set; }

        }
        public class CoScholasticObtainedModel
        {
            public long StudentID { get; set; }
            public long ClassID { get; set; }
            public long SectionId { get; set; }
            public long TermID { get; set; }
            public long BoardID { get; set; }
            public int BatchId { get; set; }
            public List<tbl_CoScholasticObtainedGrade> CoscholasticData { get; set; }
        }
        public class StudentObtainedMarkModel
        {
            public long StudentID { get; set; }
            public long ClassID { get; set; }
            public long SectionId { get; set; }
            public long TermID { get; set; }
            public long BoardID { get; set; }
            public string Remark { get; set; }
            public int BatchId { get; set; }
            public int RankInClass { get; set; }
            public List<Tbl_TestObtainedMark> ObtainedMarkData { get; set; }
        }

        public class CoscholasticAreaData
        {
            public string Name { get; set; }
            public string GradeTerm1 { get; set; }
            public string GradeTerm2 { get; set; }
            public string GradeUT1 { get; set; }
            public string GradeUT2 { get; set; }
            public string GradePre1 { get; internal set; }
            public string GradeSelection { get; internal set; }
            public string GradePromotion { get; internal set; }
            public string GradePre2 { get; internal set; }
        }
        public class CoscholastiStuentObtData
        {
            public long CoscholasticID { get; set; }
            public string ObtainedGrade { get; set; }
            public string CoscholasticName { get; set; }
        }

        //for fill marks
        public class StudentTestObtMarks
        {
            public long TestID { get; set; }
            public decimal ObtainedMarks { get; set; }
            public decimal MaximumdMarks { get; set; }
            public string TestName { get; set; }
            public string Remark { get; set; }
            public bool IsElective { get; set; }
            public bool IsOptional { get; set; }
        }

        //list elective student
        public class ElectiveListStudent
        {
            public long StudentId { get; set; }
            public string StudentName { get; set; }
            public List<ELectiveRecord> ElectiveSubjectId { get; set; }
        }

        public class ELectiveRecord
        {
            public long Subject_ID { get; set; }
            public string Subject_Name { get; set; }

            public bool IsSelected { get; set; }
        }

        public ActionResult Statistics()
        {
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.ClassList = Classes;
            var Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.SectionList = Section;
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;
            return View();
        }
        [HttpGet]
        public ActionResult TestDateAssign()
        {
            object Tests = null; object Batch = null; object Classes = null;
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;
            if (Session["TestAssignData"] == null)
            {
                Tests = _context.tbl_Tests.ToList();
                Batch = _context.Tbl_Batches.ToList();//_context.DataListItems.Where(e => e.DataListId == "9" && e.Status == "Active").ToList();
                Classes = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            }
            else
            {
                var ViewDt = Session["TestAssignData"] as TblTestAssignDate;
                Tests = _context.tbl_Tests.Where(x => x.TestID == ViewDt.TestID).ToList();
                Batch = _context.Tbl_Batches.Where(e => e.Batch_Id == ViewDt.BatchID).ToList();
                Classes = _context.DataListItems.Where(e => e.DataListId == "5" && e.DataListItemId == ViewDt.ClassID).ToList();
            }

            var innerJoin = from e in _context.TblTestAssignDate
                            join d in _context.tbl_Term on e.TestID equals d.TermID
                            join c in _context.Tbl_Batches on e.BatchID equals c.Batch_Id
                            join f in _context.DataListItems on e.ClassID equals f.DataListItemId
                            select new PL_TestAssignData
                            {
                                TestAssignId = e.TestAssignID,
                                TestName = d.TermName,
                                BatchName = c.Batch_Name,
                                ClassName = f.DataListItemName,
                                StartDate = e.StartDate,
                                EndDate = e.ToDate
                            };

            ViewBag.TestAssignList = innerJoin.ToList<PL_TestAssignData>();
            ViewBag.ClassList = Classes; ViewBag.TestList = Tests; ViewBag.SectionList = Batch;
            if (Session["Msg"] != null)
            {
                ViewBag.Msg = Session["Msg"];
            }
            else
            {
                Session["Msg"] = null;
                ViewBag.Msg = Session["Msg"];
            }
            Session.Remove("Msg");
            return View();
        }
        [HttpPost]
        public ActionResult TestDateAssign(PL_TblTestAssignDate objtblTest)
        {
            try
            {
                if (objtblTest.TestAssignID == 0)
                {
                    TblTestAssignDate testAssignDateObj = new TblTestAssignDate();
                    testAssignDateObj.StartDate = DateFormat(objtblTest.StartDate);
                    testAssignDateObj.ToDate = DateFormat(objtblTest.ToDate);
                    testAssignDateObj.CreatedAt = DateTime.Now;
                    testAssignDateObj.UpdatedAt = Convert.ToDateTime("1900-01-01");
                    testAssignDateObj.BatchID = objtblTest.BatchID;
                    testAssignDateObj.ClassID = objtblTest.ClassID;
                    testAssignDateObj.TestID = objtblTest.TermID;
                    _context.TblTestAssignDate.Add(testAssignDateObj);
                    Session["Msg"] = "1";
                }
                else
                {
                    var testData = _context.TblTestAssignDate.Where(x => x.TestAssignID == objtblTest.TestAssignID).SingleOrDefault();

                    testData.StartDate = Convert.ToDateTime(DateFormat(objtblTest.StartDate));
                    testData.ToDate = Convert.ToDateTime(DateFormat(objtblTest.ToDate));
                    testData.UpdatedAt = DateTime.Now;
                    testData.TestID = objtblTest.TermID;
                    testData.ClassID = objtblTest.ClassID;
                    testData.BatchID = objtblTest.BatchID;
                    Session["Msg"] = "2";
                }
                _context.SaveChanges();
            }
            catch (Exception Ex)
            {
                Session["Msg"] = "0";
            }
            return RedirectToAction("TestDateAssign");
        }
        [HttpGet]
        public ActionResult TestAssignDataDelete(int id)
        {
            try
            {
                var TestAssignList = _context.TblTestAssignDate.Where(x => x.TestAssignID == id).SingleOrDefault();
                if (TestAssignList != null)
                {
                    _context.TblTestAssignDate.Remove(TestAssignList);
                    _context.SaveChanges();
                    Session["Msg"] = "3";
                }
                else
                {
                    ViewBag.Msg = "4";
                }
            }
            catch (Exception Ex)
            {
                Session["Msg"] = "4";
            }
            return RedirectToAction("TestDateAssign");
        }
        [HttpGet]
        public JsonResult TestAssignDataUpdate(int id)
        {
            object JsonData;
            try
            {
                JsonData = _context.TblTestAssignDate.Where(x => x.TestAssignID == id).Select(x => new
                {
                    TestAssignID = x.TestAssignID,
                    TestID = x.TestID,
                    BatchID = x.BatchID,
                    ClassID = x.ClassID,
                    StartDate = x.StartDate.ToString(),
                    ToDate = x.ToDate.ToString()
                }).SingleOrDefault();
                //Session["TestAssignData"] = JsonData;
            }
            catch (Exception Ex)
            {
                JsonData = Ex.Message;
            }
            return Json(JsonData, JsonRequestBehavior.AllowGet);
        }

        //string DateFormat(string Date)
        //{
        //    return Date.Substring(6, 4) + "-" + Date.Substring(3, 2) + "-" + Date.Substring(0, 2);
        //}

        public DateTime DateFormat(string Date)
        {
            return DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        public class PL_TestAssignData
        {
            public long TestAssignId { get; set; }
            public string TestName { get; set; }
            public string BatchName { get; set; }
            public string ClassName { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }


        }

        public class GradingCriteriaModel
        {

            public long CriteriaID { get; set; }
            public decimal MinimumPercentage { get; set; }
            public decimal MaximumPercentage { get; set; }
            public string Grade { get; set; }
            public string GradeDescription { get; set; }
            public long BoardID { get; set; }
            public long TestID { get; set; }
            public long ClassID { get; set; }
            public long BatchID { get; set; }
            public string TestName { get; set; }
            public long TermID { get; set; }
            public string ClassName { get; set; }
            public string BatchName { get; set; }
            public string TermName { get; set; }

            public string TestType { get; set; }
        }

        public class PL_TblTestAssignDate
        {

            public long TermID { get; set; }
            public long TestAssignID { get; set; }
            public long TestID { get; set; }
            public long ClassID { get; set; }
            public long BatchID { get; set; }
            public string StartDate { get; set; }
            public string ToDate { get; set; }
            public string CreatedAt { get; set; }
            public string UpdatedAt { get; set; }
        }
        //public class GrdingCriteriaModel
        //{
        //    public long CriteriaID { get; set; }
        //    public decimal MinimumPercentage { get; set; }
        //    public decimal MaximumPercentage { get; set; }
        //    public string Grade { get; set; }
        //    public string GradeDescription { get; set; }
        //    public long BoardID { get; set; }
        //    public long TestID { get; set; }
        //    public long ClassID { get; set; }
        //    public long BatchID { get; set; }
        //    public string TestName { get; set; }
        //    public string ClassName { get; set; }
        //    public string BatchName { get; set; }
        //}
        public class FreezeUnfreezeDTO
        {
            public int? FreezeId { get; set; }
            public int ClassId { get; set; }
            public int SectionId { get; set; }
            public int TermId { get; set; }
            public bool IsFreeze { get; set; }
            public int BatchId { get; set; }
        }
        public class getFreezing
        {

            public int ClassId { get; set; }
            public int SectionId { get; set; }
            public int TermId { get; set; }
            public int BatchID { get; set; }

        }


        [HttpPost]
        //public JsonResult FreezeUnfreezeData(FreezeUnfreezeDTO freezeUnfreezeDTO)
        //{
        //    //List<ListStudent> listStudents = new List<ListStudent>();
        //    try
        //    {

        //        if (freezeUnfreezeDTO.IsFreeze)
        //        {
        //            var FreezeItem = _context.Tbl_FreezeData.Where(x => x.ClassId== freezeUnfreezeDTO.ClassId
        //                                && x.TermId== freezeUnfreezeDTO.TermId 
        //                                && x.SectionId==freezeUnfreezeDTO.SectionId)
        //                              .FirstOrDefault();

        //            if (FreezeItem != null)
        //            {
        //                FreezeItem.IsFreeze = freezeUnfreezeDTO.IsFreeze;
        //                _context.SaveChanges();
        //                return Json(new
        //                {
        //                    success = true,
        //                    msg = "Freezed successfully",
        //                    data = new { FreezeId= FreezeItem.FreezeId}

        //                }, JsonRequestBehavior.AllowGet);
        //            }
        //            else{
        //                // Create a new Remark entity and add it to the context
        //                var newFreeze = new Data.Models.Tbl_FreezeData
        //                {
        //                    TermId = freezeUnfreezeDTO.TermId,
        //                    ClassId = freezeUnfreezeDTO.ClassId,
        //                    SectionId = freezeUnfreezeDTO.SectionId,
        //                    IsFreeze = freezeUnfreezeDTO.IsFreeze
        //                };

        //                _context.Tbl_FreezeData.Add(newFreeze);
        //                // Save changes to the context
        //                _context.SaveChanges();
        //                return Json(new
        //                {
        //                    success = true,
        //                    msg = "Freezed successfully ",
        //                    data = new { FreezeId = newFreeze.FreezeId } 
        //                }, JsonRequestBehavior.AllowGet);
        //            }

        //        }
        //        else
        //        {
        //            var FreezeItem = _context.Tbl_FreezeData.Where(x => x.FreezeId == freezeUnfreezeDTO.FreezeId)
        //                              .FirstOrDefault();
        //            if (FreezeItem != null)
        //            {
        //                FreezeItem.IsFreeze = false;
        //                _context.SaveChanges();
        //              return  Json(new { success = true,
        //              msg ="Unfreezed successfully "}, JsonRequestBehavior.AllowGet);
        //            }
        //            else
        //            {
        //               return Json(new { success = false,
        //               msg = "Incorreact Freeze data Id" }, JsonRequestBehavior.AllowGet);

        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(ex, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public JsonResult FreezeUnfreezeData(FreezeUnfreezeDTO freezeUnfreezeDTO)
        {
            try
            {

                if (freezeUnfreezeDTO.IsFreeze)
                {
                    var FreezeItem = _context.Tbl_FreezeData
                        .Where(x => x.ClassId == freezeUnfreezeDTO.ClassId
                                 && x.TermId == freezeUnfreezeDTO.TermId
                                 && x.SectionId == freezeUnfreezeDTO.SectionId
                                 && x.BatchId == freezeUnfreezeDTO.BatchId)
                        .FirstOrDefault();

                    if (FreezeItem != null)
                    {
                        FreezeItem.IsFreeze = freezeUnfreezeDTO.IsFreeze;
                        _context.SaveChanges();
                        return Json(new
                        {
                            success = true,
                            msg = "Freezed successfully",
                            data = new { FreezeId = FreezeItem.FreezeId }
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if (freezeUnfreezeDTO.TermId != 0 && freezeUnfreezeDTO.BatchId != 0 && freezeUnfreezeDTO.ClassId == 0 && freezeUnfreezeDTO.SectionId == 0)
                        {

                            var Classes = _context.DataListItems
                                .Where(e => e.DataListId == _context.DataLists
                                .FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString())
                                .ToList();


                            var Sections = _context.DataListItems
                                .Where(e => e.DataListId == _context.DataLists
                                .FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString())
                                .ToList();


                            foreach (var classItem in Classes)
                            {

                                foreach (var sectionItem in Sections)
                                {
                                    var newFreeze = new Data.Models.Tbl_FreezeData
                                    {
                                        TermId = freezeUnfreezeDTO.TermId,
                                        ClassId = classItem.DataListItemId,
                                        SectionId = sectionItem.DataListItemId,
                                        BatchId = freezeUnfreezeDTO.BatchId,
                                        IsFreeze = freezeUnfreezeDTO.IsFreeze
                                    };

                                    _context.Tbl_FreezeData.Add(newFreeze);
                                }
                            }

                            _context.SaveChanges();

                            return Json(new
                            {
                                success = true,
                                msg = "Freezed successfully",
                                data = new { FreezeId = 0 }
                            }, JsonRequestBehavior.AllowGet);
                        }

                        else
                        {
                            // Create a new Freeze entity and add it to the context
                            var newFreeze = new Data.Models.Tbl_FreezeData
                            {
                                TermId = freezeUnfreezeDTO.TermId,
                                ClassId = freezeUnfreezeDTO.ClassId,
                                SectionId = freezeUnfreezeDTO.SectionId,
                                BatchId = freezeUnfreezeDTO.BatchId,
                                IsFreeze = freezeUnfreezeDTO.IsFreeze
                            };

                            _context.Tbl_FreezeData.Add(newFreeze);
                            _context.SaveChanges();
                            return Json(new
                            {
                                success = true,
                                msg = "Freezed successfully",
                                data = new { FreezeId = newFreeze.FreezeId }
                            }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }
                else
                {
                    var FreezeItem = _context.Tbl_FreezeData
                        .Where(x => x.FreezeId == freezeUnfreezeDTO.FreezeId)
                        .FirstOrDefault();
                    if (FreezeItem != null)
                    {
                        FreezeItem.IsFreeze = false;
                        _context.SaveChanges();
                        return Json(new { success = true, msg = "Unfreezed successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new { success = false, msg = "Incorrect Freeze data Id" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult GetFreezingData(getFreezing getFreezing)
        {
            //List<ListStudent> listStudents = new List<ListStudent>();
            try
            {
                var FreezeItem = _context.Tbl_FreezeData.Where(x => x.ClassId == getFreezing.ClassId
                            && x.SectionId == getFreezing.SectionId && x.TermId == getFreezing.TermId)
                                  .FirstOrDefault();

                if (FreezeItem != null)
                {

                    return Json(new
                    {
                        success = true,
                        msg = "Get Freezing data successfully",
                        data = FreezeItem
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        success = true,
                        msg = "Not found freezing data",
                    }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult GetPublishData(PublishDetail objPublishDetail)
        {

            try
            {


                var publishItem = _context.Tbl_PublishDetail
                           .Where(x => x.ClassId == objPublishDetail.ClassId
                            && x.SectionId == objPublishDetail.SectionId
                            && x.TermId == objPublishDetail.TermId
                            && x.BatchId == objPublishDetail.BatchID)?.OrderByDescending(x => x.PublishId)
                            .FirstOrDefault();

                if (publishItem != null)
                {

                    return Json(new
                    {
                        success = true,
                        msg = "Get publish data successfully",
                        data = publishItem
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new
                    {
                        success = true,
                        msg = "Not found publish data",
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objPublishDetail"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PublishUnpublish(PublishDetail objPublishDetail)
        {
            try
            {
                if (objPublishDetail.TermId == 10)
                {
                    return Json(new
                    {
                        success = true,
                        msg = "publish for all term not allow",
                        data = "msg"
                    }, JsonRequestBehavior.AllowGet);
                }

                var publishItem = _context.Tbl_PublishDetail
                           .Where(x => x.ClassId == objPublishDetail.ClassId
                            && x.SectionId == objPublishDetail.SectionId
                            && x.TermId == objPublishDetail.TermId
                            && x.BatchId == objPublishDetail.BatchID)?.OrderByDescending(x => x.PublishId)
                            .FirstOrDefault();

                if (publishItem != null)
                {
                    publishItem.IsPublish = objPublishDetail.IsPublish;
                    publishItem.ModifiedDate = DateTime.Now;
                    publishItem.PublishBy = Session["UserId"] == null ? "" : Session["UserId"].ToString();
                    _context.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        msg = "publish data update successfully",
                        data = publishItem
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Data.Models.Tbl_PublishDetail objpublish = new Data.Models.Tbl_PublishDetail();
                    objpublish.TermId = objPublishDetail.TermId;
                    objpublish.SectionId = objPublishDetail.SectionId;
                    objpublish.BatchId = objPublishDetail.BatchID;
                    objpublish.ClassId = objPublishDetail.ClassId;
                    objpublish.IsPublish = true;
                    objpublish.PublishDate = DateTime.Now;
                    objpublish.PublishBy = Session["UserId"] == null ? "" : Session["UserId"].ToString();
                    _context.Tbl_PublishDetail.Add(objpublish);
                    _context.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        msg = "publish data Save successfully",
                        data = objpublish
                    }, JsonRequestBehavior.AllowGet);

                }

            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult StudentReportCard()
        {
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;

            return View();
        }
        public ActionResult GetStudentReportCard()
        {
            List<ListStudent> listStudents = new List<ListStudent>();
            List<Tbl_Tests> Tests;

            try
            {
                long studentId = Convert.ToInt64(Session["ScolarNo"]);
                var studentInfo = _context.Students.Where(x => x.ApplicationNumber == studentId.ToString()).FirstOrDefault();
                var studentRegister = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var familyDetail = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentRegister.ApplicationNumber).FirstOrDefault();
                var staffId = _context.Subjects.Where(x => x.Class_Id == studentInfo.Class_Id && x.Section_Id == studentInfo.Section_Id && x.Class_Teacher == true).Select(x => x.StaffId).FirstOrDefault();


                // var stdInfo = new List<Tbl_TestRecords>();
                //var list = _context.Students;

                var stdInfo = _context.Tbl_TestRecord.Where(x => x.StudentID == studentInfo.StudentId).ToList();
                var studentList = new List<SchoolManagement.Data.Models.Student>();
                foreach (var item in stdInfo)
                {

                    var dd = _context.Tbl_PublishDetail.Where(x => x.ClassId == item.ClassID && x.SectionId == item.SectionID && x.BatchId == item.BatchId && x.TermId == item.TermID && x.IsPublish == true).FirstOrDefault();

                    if (dd != null)
                    {
                        var student = new SchoolManagement.Data.Models.Student();
                        student.Name = _context.Students.Where(x => x.StudentId == item.StudentID).Select(x => x.Name).FirstOrDefault();
                        student.Section = _context.DataListItems.Where(x => x.DataListItemId == item.SectionID).Select(x => x.DataListItemName).FirstOrDefault();
                        student.Section_Id = Convert.ToInt32(item.SectionID);
                        student.StudentId = Convert.ToInt32(item.StudentID);
                        student.Class_Id = Convert.ToInt32(item.ClassID);
                        student.Batch_Id = Convert.ToInt32(item.BatchId);
                        student.Class = _context.DataListItems.Where(x => x.DataListItemId == item.ClassID).Select(x => x.DataListItemName).FirstOrDefault();
                        student.BatchName = _context.DataListItems.Where(x => x.DataListItemId == item.BatchId).Select(x => x.DataListItemName).FirstOrDefault();
                        student.Sports = _context.tbl_Term.Where(x => x.TermID == item.TermID).Select(x => x.TermName).FirstOrDefault();

                        studentList.Add(student);
                    }

                }
                //return Json(studentList, JsonRequestBehavior.AllowGet);


                foreach (var item in studentList)
                {

                    item.Section = _context.DataListItems.Where(x => x.DataListItemId == item.Section_Id).Select(x => x.DataListItemName).FirstOrDefault();
                    ListStudent listStudent = new ListStudent()
                    {
                        StudentId = item.StudentId,
                        StudentName = item.Name,
                        ClassName = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault(),
                        SectionName = item.Section,
                        BatchId = item.Batch_Id,
                        Term = item.Sports,
                        TestName = Convert.ToString(_context.tbl_Term.Where(x => x.TermName == item.Sports).Select(x => x.TermID).FirstOrDefault()),

                        //BatchName = item.BatchName,  //---x-rnik--
                        BatchName = _context.Tbl_Batches.Where(x => x.Batch_Id == item.Batch_Id).FirstOrDefault().Batch_Name,
                        ObtainedMarks = 0
                        //Term = _context.tbl_Term.Where(x => x.BatchId == item.Batch_Id).FirstOrDefault   ().TermID.ToString()
                    };

                    listStudents.Add(listStudent);

                }


                //IQueryable<SchoolManagement.Data.Models.Student> query = _context.Students.Where(x => !x.IsApplyforTC);


                //if (classId != 0 && sectionId != 0)
                //{
                //    query = query.Where(x => x.Class_Id == classId && x.Section_Id == sectionId);
                //}

                //if (Batchid != 0)
                //{
                //    query = query.Where(x => x.Batch_Id == Batchid);
                //}
                //var studentlist = new List<Data.Models.Student>();



                //if (termId != 10)
                //{
                //studentlist = (from student in query
                //              join testRecord in _context.Tbl_TestRecord
                //              on student.StudentId equals testRecord.StudentID
                //              where testRecord.TermID == termId && student.StudentId == studentInfo.StudentId
                //              select student)
                //}
                //          .Distinct()
                //else
                //          .ToList();

                //}
                // foreach (var item in studentList)
                //{
                //{
                //                       on student.Batch_Id equals PublishDetail.BatchId
                //    var studentinfo = (from student in studentList
                //                       join testRecord in _context.Tbl_TestRecord
                //                        on student.StudentId equals testRecord.StudentID
                //                       join PublishDetail in _context.Tbl_PublishDetail
                //                       && student.Class_Id == PublishDetail.ClassId
                //                       where student.StudentId == studentInfo.StudentId
                //                       && student.Section_Id == PublishDetail.SectionId
                //                       //&& PublishDetail.TermId == termId
                //                       && PublishDetail.IsPublish == true
                //                       select student).Distinct()
                //               .FirstOrDefault();

                //    if (studentinfo != null)
                //    {
                //        studentlist.Add(studentinfo);
                //    }
                //}

                // }


                //foreach (var item in studentlist)
                //{
                //    item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                //    ListStudent listStudent = new ListStudent()
                //    {
                //        StudentId = item.StudentId,
                //        ClassName = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault(),
                //        SectionName = item.Section,
                //        StudentName = item.Name,
                //        BatchId = item.Batch_Id,
                //        //BatchName = item.BatchName,  //---x-rnik--
                //        BatchName = _context.Tbl_Batches.Where(x => x.Batch_Id == item.Batch_Id).FirstOrDefault().Batch_Name,
                //        ObtainedMarks = 0
                //        //Term = _context.tbl_Term.Where(x => x.BatchId == item.Batch_Id).FirstOrDefault   ().TermID.ToString()
                //    };
                //    listStudents.Add(listStudent);




                return Json(listStudents.OrderBy(x => x.StudentName), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }






        //updated code 

        private Tbl_TestRecords GetStudentMarks(int studentId, long termId, long testId)
        {
            var query = (from cr in _context.Tbl_TestRecord
                         join cog in _context.tbl_TestObtainedMark
                         on cr.RecordID equals cog.RecordIDFK
                         where cr.StudentID == studentId
                               && cr.TermID == termId
                               && cog.TestID == testId
                         select new
                         {
                             ObtainedMarks = cog.ObtainedMarks ?? -1
                         });

            var studentMark = query.FirstOrDefault();

            // Return the result mapped to Tbl_TestRecords
            return studentMark != null
                ? new Tbl_TestRecords { ObtainedMarks = studentMark.ObtainedMarks }
                : new Tbl_TestRecords { ObtainedMarks = -1 };
        }
        void SetSubjectDataDefaults(ref OptionalSubjectData subjectData)
        {
            subjectData.MarksUT1 = -1;
            subjectData.MarksUT2 = -1;
            subjectData.MarksUT1Grade = "D";
            subjectData.MarksUT2Grade = "D";
            subjectData.TotalMarks = -1;
            subjectData.TheoryMarks = -1;
            subjectData.PracticalMarks = -1;
            subjectData.TotalObtainedMarks = -1;
            subjectData.GradeUT1 = "D";
            subjectData.TheoryMarksUT2 = -1;
            subjectData.PracticalMarksUT2 = -1;
            subjectData.TotalObtainedMarksUT2 = -1;
            subjectData.GradeUT2 = "D";
            subjectData.TheoryMarksPre1 = -1;
            subjectData.PracticalMarksPre1 = -1;
            subjectData.TotalObtainedMarksPre1 = -1;
            subjectData.GradePre1 = "D";
            subjectData.TheoryMarksPre2 = -1;
            subjectData.PracticalMarksPre2 = -1;
            subjectData.TotalObtainedMarksPre2 = -1;
            subjectData.GradePre2 = "D";
            subjectData.TheoryMarksPromotion = -1;
            subjectData.PracticalMarksPromotion = -1;
            subjectData.TotalObtainedMarkPromotion = -1;
            subjectData.GradePromotion = "D";


        }

        public JsonResult PrintReportCardData1(int studentId, int termId, int BatchId) //, int classId
        {
            try
            {
                Batch_Id = BatchId;
                var stdInfo = new Tbl_TestRecords();
                if (Batch_Id > 0)
                {
                    stdInfo = _context.Tbl_TestRecord.Where(x => x.BatchId == Batch_Id && x.StudentID == studentId).FirstOrDefault();
                }
                var studentInfo = _context.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                var studentRegister = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var familyDetail = _context.FamilyDetails.Where(x => x.ApplicationNumber == studentRegister.ApplicationNumber).FirstOrDefault();
                var staffId = _context.Subjects.Where(x => x.Class_Id == stdInfo.ClassID && x.Batch_Id == Batch_Id && x.Section_Id == stdInfo.SectionID && x.Class_Teacher == true).Select(x => x.StaffId).FirstOrDefault();

                List<Tbl_StudentAttendance> ActualAttendance = new List<Tbl_StudentAttendance>();
                if (termId != 10)
                {
                    var batch = _context.Tbl_Batches.Where(x => x.Batch_Id == Batch_Id).FirstOrDefault();
                    //  var batchItems = _context.Tbl_DataListItem.Where(x => x.DataListId == "9" && x.DataListItemName== batch.Batch_Name).FirstOrDefault();
                    var attendanceDate = _context.TblTestAssignDate.Where(x => x.TestID == termId && x.BatchID == batch.Batch_Id && x.ClassID == stdInfo.ClassID).FirstOrDefault();
                    var StartDate = DateTime.Now; var ToDate = DateTime.Now;
                    if (attendanceDate == null)
                    {
                        StartDate = DateTime.Now;
                        ToDate = DateTime.Now;
                    }
                    else
                    {
                        StartDate = Convert.ToDateTime(attendanceDate.StartDate);
                        ToDate = Convert.ToDateTime(attendanceDate.ToDate);
                    }
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID &&
                x.Class_Id == stdInfo.ClassID && x.Section_Id == stdInfo.SectionID).ToList().Where(x =>
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= StartDate.Date &&
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= ToDate.Date).ToList();

                }
                else
                {
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == stdInfo.StudentID
                    && x.Class_Id == stdInfo.ClassID
                    && x.Section_Id == stdInfo.SectionID
                    ).ToList();

                }

                double attendedDays = 0;
                double attendedHalfDays = 0;
                foreach (var item in ActualAttendance)
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
                //m double totalAttendedDays = attendedDays + (attendedHalfDays / 2);

                int totalAttendedDays = Convert.ToInt32(attendedDays + (attendedHalfDays / 2));

                StudentReportData studentReportData = new StudentReportData()
                {
                    studentName = studentInfo.Name,
                    fatherName = familyDetail.FatherName,
                    motherName = familyDetail.MotherName,
                    scholarNo = studentInfo.ScholarNo.ToString(),
                    rollNo = studentInfo.RollNo.ToString(),
                    className = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID).Select(x => x.DataListItemName).FirstOrDefault(),
                    sectionName = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.SectionID).Select(x => x.DataListItemName).FirstOrDefault(),
                    dateOfBirth = studentInfo.DOB,
                    academicYear = _context.Tbl_Batches.Where(x => x.Batch_Id == stdInfo.BatchId).Select(x => x.Batch_Name).FirstOrDefault(),
                    studentID = studentInfo.StudentId,
                    attandence = totalAttendedDays + "/" + ActualAttendance.Count(),
                    promotedClass = _context.DataListItems.Where(x => x.DataListItemId == stdInfo.ClassID + 1).Select(x => x.DataListItemName).FirstOrDefault(),
                    staffSignatureLink = _context.StafsDetails.Where(x => x.StafId == staffId).Select(x => x.StaffSignatureFile).FirstOrDefault(),
                    Remark = _context.tbl_Remark.Where(x => x.StudentId == stdInfo.StudentID && x.BatchId == Batch_Id && (x.TermId == termId || (termId == 10 && x.TermId == 4) || (termId == 7 && x.TermId == 8))).Select(x => x.Remark).FirstOrDefault(),
                    classID = stdInfo.ClassID
                };
                var allSubjects = (from subj in _context.tbl_ClassSubject
                                   join test in _context.tbl_Tests on subj.SubjectId equals test.SubjectID
                                   where test.ClassID == stdInfo.ClassID
                                         && subj.ClassId == stdInfo.ClassID
                                         && (termId == 10 || test.TermID == termId)
                                         && test.IsOptional == false
                                   select subj).Distinct().ToList();

                var electiveSubjects = allSubjects.Where(x => x.IsElective == true).ToList();
                if (electiveSubjects.Any())
                {
                    var subjectsToRemove = electiveSubjects.Where(item =>
                        !_context.tbl_Student_ElectiveRecord
                            .Any(x => x.StudentId == stdInfo.StudentID && x.ElectiveSubjectId == item.SubjectId)
                    ).Select(item => item.SubjectId).ToList();

                    allSubjects.RemoveAll(subj => subjectsToRemove.Contains(subj.SubjectId));
                }

                var terms = _context.tbl_Term.Where(x => termId == 10 || x.TermID == termId).ToList();
                List<SubjectData> subjectDatas = new List<SubjectData>();

                foreach (var item in allSubjects)
                {
                    SubjectData subjectData = new SubjectData();

                    foreach (var termItem in terms)
                    {
                        var testList = _context.tbl_Tests
                            .Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID)
                            .ToList();

                        if (testList.Any())
                        {
                            foreach (var testItem in testList)
                            {
                                var isTheory = testItem.TestType == "Theory";
                                var isPractical = testItem.TestType == "Practical";
                                var maxMarks = testItem.MaximumMarks;
                                var studentMarks = GetStudentMarks(studentId, termItem.TermID, testItem.TestID);

                                // Update marks based on term and type
                                switch (termItem.TermID)
                                {
                                    case 1: // UT1
                                        subjectData.MarksUT1 = studentMarks?.ObtainedMarks ?? -2;
                                        subjectData.MaxMarksUT1 = maxMarks;
                                        break;
                                    case 2: // UT2
                                        subjectData.MarksUT2 = studentMarks?.ObtainedMarks ?? -2;
                                        subjectData.MaxMarksUT2 = maxMarks;
                                        break;
                                    case 3: // Term1 (Theory and Practical)
                                        if (isTheory) subjectData.TheoryMarks = studentMarks?.ObtainedMarks ?? -2;
                                        if (isPractical) subjectData.PracticalMarks = studentMarks?.ObtainedMarks ?? -2;
                                        subjectData.MaxMarksTerm1Theory = maxMarks;
                                        break;
                                    case 4: // Term2 (Theory and Practical)
                                        if (isTheory) subjectData.TheoryMarks = studentMarks?.ObtainedMarks ?? -2;
                                        if (isPractical) subjectData.PracticalMarks = studentMarks?.ObtainedMarks ?? -2;
                                        subjectData.MaxMarksTerm2Theory = maxMarks;
                                        break;

                                    default: // PreBoard1/PreBoard2
                                        if (isTheory) subjectData.TheoryMarksPre1 = studentMarks?.ObtainedMarks ?? -2;
                                        if (isPractical) subjectData.PracticalMarksPre1 = studentMarks?.ObtainedMarks ?? -2;
                                        break;
                                }

                                // Total obtained marks for the subject
                                subjectData.TotalObtainedMarks += studentMarks?.ObtainedMarks ?? 0;
                            }

                            // Subject name and grades for all terms
                            subjectData.Subject = _context.Tbl_SubjectsSetup
                                .Where(x => x.Subject_ID == item.SubjectId)
                                .Select(x => x.Subject_Name)
                                .FirstOrDefault();

                            subjectData.MarksUT1Grade = GetGrade(PercentageCal(subjectData.MarksUT1, subjectData.MaxMarksUT1), Convert.ToInt32(stdInfo.ClassID));
                            subjectData.MarksUT2Grade = GetGrade(PercentageCal(subjectData.MarksUT2, subjectData.MaxMarksUT2), Convert.ToInt32(stdInfo.ClassID));

                            // Combine theory and practical marks for Term1 & Term2
                            var term1Total = (subjectData.TheoryMarks + subjectData.PracticalMarks);
                            if (term1Total > 0)
                            {
                                subjectData.GradeUT1 = GetGrade((term1Total / subjectData.MaxMarksTerm1Theory) * 100, Convert.ToInt32(stdInfo.ClassID));
                            }

                            subjectData.FinalGrade = GetGrade((subjectData.TotalObtainedMarks / 240) * 100, Convert.ToInt32(stdInfo.ClassID));
                        }
                        else
                        {
                            // If no tests exist, fill with default values
                            subjectData.Subject = _context.Tbl_SubjectsSetup
                                .Where(x => x.Subject_ID == item.SubjectId)
                                .Select(x => x.Subject_Name)
                                .FirstOrDefault();

                            subjectData.MarksUT1 = subjectData.MarksUT1 == null ? -1 : subjectData.MarksUT1;
                            subjectData.MarksUT2 = subjectData.MarksUT2 == null ? -1 : subjectData.MarksUT2;
                            subjectData.TheoryMarks = subjectData.TheoryMarks == null ? -1 : subjectData.TheoryMarks;
                            subjectData.PracticalMarks = subjectData.PracticalMarks == null ? -1 : subjectData.PracticalMarks;
                            subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == null ? -1 : subjectData.TotalObtainedMarks;
                            subjectData.GradeUT1 = subjectData.GradeUT1 ?? "D";
                            subjectData.GradeUT2 = subjectData.GradeUT2 ?? "D";
                            subjectData.FinalGrade = subjectData.FinalGrade ?? "D";
                        }
                    }

                    subjectDatas.Add(subjectData);
                }


                decimal NewTotalObtainedMarks = 0;
                decimal NewTotalMarks = 0;
                decimal NewMaxMark = 1;

                // Function to set default values for the subjectData properties
                // Process optional subjects
                var allOptionalSubjects = (from subj in _context.tbl_ClassSubject
                                           join test in _context.tbl_Tests on subj.SubjectId equals test.SubjectID
                                           where test.ClassID == stdInfo.ClassID && subj.ClassId == stdInfo.ClassID
                                               && (termId == 10 || test.TermID == termId) && test.IsOptional == true
                                           select subj).Distinct().ToList();

                List<OptionalSubjectData> optionalSubjectDatas = new List<OptionalSubjectData>();

                foreach (var item in allOptionalSubjects)
                {
                    OptionalSubjectData subjectData = new OptionalSubjectData();
                    SetSubjectDataDefaults(ref subjectData); // Set default values

                    foreach (var termItem in terms)
                    {
                        var tests = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == stdInfo.ClassID && x.TermID == termItem.TermID).ToList();

                        foreach (var testItem in tests)
                        {
                            var tbl_TestRecords = ProcessMarks(Convert.ToInt32(termItem.TermID), testItem.TestType, studentId, testItem); // Process marks based on term and test type

                            // Update specific term marks and grades
                            if (termItem.TermID == 1)
                            {
                                subjectData.MarksUT1 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.MaxMarksUT1 = testItem.MaximumMarks;
                                subjectData.MarksUT1Grade = GetOptionMarkGrade(subjectData.MarksUT1);
                            }
                            else if (termItem.TermID == 2)
                            {
                                subjectData.MarksUT2 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.MaxMarksUT2 = testItem.MaximumMarks;
                                subjectData.MarksUT2Grade = GetOptionMarkGrade(subjectData.MarksUT2);
                            }

                            // Update marks for other terms (Pre-1, Pre-2, Term 1, Term 2)
                            if (termId != 10)
                            {
                                subjectData.TheoryMarksPre1 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre1 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre1 = subjectData.TheoryMarksPre1 + subjectData.PracticalMarksPre1;
                                subjectData.GradePre1 = GetOptionMarkGrade(subjectData.TotalObtainedMarksPre1);

                                subjectData.TheoryMarksPre2 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.PracticalMarksPre2 = tbl_TestRecords?.ObtainedMarks ?? -2;
                                subjectData.TotalObtainedMarksPre2 = subjectData.TheoryMarksPre2 + subjectData.PracticalMarksPre2;
                                subjectData.GradePre2 = GetOptionMarkGrade(subjectData.TotalObtainedMarksPre2);
                            }
                        }
                    }

                    subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault();
                    optionalSubjectDatas.Add(subjectData);
                }


                // Initialize total variables
                decimal UT1Total = 0, UT1MaxTotal = 0, UT2Total = 0, UT2MaxTotal = 0;
                decimal Term1TheoryMaxTotal = 0, Term1PracticalMaxTotal = 0, Term2TheoryMaxTotal = 0;
                decimal Term2PracticalMaxTotal = 0, UTAllTotal = 0, TheoryTotalT1 = 0;
                decimal PracticalTotalT1 = 0, T1AllTotal = 0, TheoryTotalT2 = 0;
                decimal PracticalTotalT2 = 0, T2AllTotal = 0, OverallAllTotal = 0;
                decimal Pre1TheoryMaxTotal = 0, Pre1PracticalMaxTotal = 0, Pre2TheoryMaxTotal = 0;
                decimal Pre2PracticalMaxTotal = 0, PreAllTotal = 0;

                foreach (var item in subjectDatas)
                {
                    // Avoiding repetitive checks with a helper method
                    UT1Total += GetValidMarks(item.MarksUT1);
                    UT1MaxTotal += GetValidMarks(item.MaxMarksUT1);
                    UT2Total += GetValidMarks(item.MarksUT2);
                    UT2MaxTotal += GetValidMarks(item.MaxMarksUT2);
                    Term1TheoryMaxTotal += GetValidMarks(item.MaxMarksTerm1Theory);
                    Term1PracticalMaxTotal += GetValidMarks(item.MaxMarksTerm1Practical);
                    Term2TheoryMaxTotal += GetValidMarks(item.MaxMarksTerm2Theory);
                    Term2PracticalMaxTotal += GetValidMarks(item.MaxMarksTerm2Practical);
                    UTAllTotal += GetValidMarks(item.TotalMarks);
                    TheoryTotalT1 += GetValidMarks(item.TheoryMarks);
                    PracticalTotalT1 += GetValidMarks(item.PracticalMarks);
                    T1AllTotal += GetValidMarks(item.TotalObtainedMarks);
                    TheoryTotalT2 += GetValidMarks(item.TheoryMarksUT2);
                    PracticalTotalT2 += GetValidMarks(item.PracticalMarksUT2);
                    T2AllTotal += GetValidMarks(item.TotalObtainedMarksUT2);
                    OverallAllTotal += GetValidMarks(item.TotalMarksBothUTs);

                    if (termId != 10)
                    {
                        Pre1TheoryMaxTotal += GetValidMarks(item.MaxMarksPre1Theory);
                        Pre1PracticalMaxTotal += GetValidMarks(item.PracticalMarksPre1);
                        Pre2TheoryMaxTotal += GetValidMarks(item.MaxMarksPre2Theory);
                        Pre2PracticalMaxTotal += GetValidMarks(item.MaxMarksPre2Practical);
                        PreAllTotal += GetValidMarks(item.TotalMarks);
                    }
                }

                // Helper method to check and return valid marks or 0
                decimal GetValidMarks(decimal marks)
                {
                    return (marks == -1 || marks == -2) ? 0 : marks;
                }

                // Divisors for grade calculations
                var divisor01 = Math.Max(Term1TheoryMaxTotal + Term1PracticalMaxTotal, 1);
                var divisor02 = Math.Max(Term2TheoryMaxTotal + Term2PracticalMaxTotal, 1);
                var divisorPre01 = Math.Max(Pre1TheoryMaxTotal + Pre1PracticalMaxTotal, 1);
                var divisorPre02 = Math.Max(Pre2TheoryMaxTotal + Pre2PracticalMaxTotal, 1);

                // Calculate grades
                TotalResult totalResult = new TotalResult()
                {
                    UT1Total = UT1Total,
                    UT1MaxTotal = UT1MaxTotal,
                    UT2Total = UT2Total,
                    UT2MaxTotal = UT2MaxTotal,
                    UT1TotalGrade = GetGrade(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID)),
                    UT2TotalGrade = GetGrade(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID)),
                    UTAllTotal = UTAllTotal,
                    TheoryTotalT1 = TheoryTotalT1,
                    PracticalTotalT1 = PracticalTotalT1,
                    T1AllTotal = T1AllTotal,
                    T1Grade = GetGrade(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID)),
                    TheoryTotalT2 = TheoryTotalT2,
                    PracticalTotalT2 = PracticalTotalT2,
                    T2AllTotal = T2AllTotal,
                    T2Grade = GetGrade(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID)),
                    OverallAllTotal = OverallAllTotal,
                    OverallGrade = GetGrade(PercentageCal(OverallAllTotal, CalculateTotalMarks()), Convert.ToInt32(stdInfo.ClassID)),

                    Term1TheoryMaxTotal = Term1TheoryMaxTotal,
                    Term1PracticalMaxTotal = Term1PracticalMaxTotal,
                    Term2TheoryMaxTotal = Term2TheoryMaxTotal,
                    Term2PracticalMaxTotal = Term2PracticalMaxTotal
                };

                // Calculate the total marks for the overall grade
                decimal CalculateTotalMarks()
                {
                    return UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal +
                        Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal +
                        Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal;
                }


                // Calculate percentages for various terms
                var totalResultpercentage = new TotalResultpercentage()
                {
                    UT1Total = PercentageCal(UT1Total, UT1MaxTotal),
                    UT1TotalGrade = GetGrade(PercentageCal(UT1Total, UT1MaxTotal), Convert.ToInt32(stdInfo.ClassID)),

                    UT2Total = PercentageCal(UT2Total, UT2MaxTotal),
                    UT2TotalGrade = GetGrade(PercentageCal(UT2Total, UT2MaxTotal), Convert.ToInt32(stdInfo.ClassID)),

                    UTAllTotal = PercentageCal(UTAllTotal, UT1MaxTotal + UT2MaxTotal),

                    TheoryTotalT1 = PercentageCal(TheoryTotalT1, Term1TheoryMaxTotal),
                    PracticalTotalT1 = PercentageCal(PracticalTotalT1, Term1PracticalMaxTotal == 0 ? 1 : Term1PracticalMaxTotal),
                    T1AllTotal = PercentageCal(T1AllTotal, divisor01),
                    T1Grade = GetGrade(PercentageCal(T1AllTotal, divisor01), Convert.ToInt32(stdInfo.ClassID)),

                    TheoryTotalT2 = PercentageCal(TheoryTotalT2, Term2TheoryMaxTotal),
                    PracticalTotalT2 = PercentageCal(PracticalTotalT2, Term2PracticalMaxTotal == 0 ? 1 : Term2PracticalMaxTotal),
                    T2AllTotal = PercentageCal(T2AllTotal, divisor02),
                    T2Grade = GetGrade(PercentageCal(T2AllTotal, divisor02), Convert.ToInt32(stdInfo.ClassID)),

                    OverallAllTotal = Math.Round(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal)), 1),
                    OverallGrade = GetGrade(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)), Convert.ToInt32(stdInfo.ClassID))
                };

                // Handle specific class adjustments (205, 208, 209, 207)
                if (stdInfo.ClassID == 205)
                {
                    var overallpers = PercentageCal(totalResult.OverallAllTotal, 2630);
                    totalResult.OverallGrade = GetGrade(PercentageCal(overallpers, 1), Convert.ToInt32(stdInfo.ClassID));
                }
                else if (stdInfo.ClassID == 208) // kg
                {
                    totalResult.OverallGrade = GetGrade(PercentageCal(totalResult.OverallAllTotal, 880), Convert.ToInt32(stdInfo.ClassID));
                }
                else if (stdInfo.ClassID == 209)
                {
                    totalResult.OverallGrade = GetGrade(PercentageCal(totalResult.OverallAllTotal, 980), Convert.ToInt32(stdInfo.ClassID));
                }
                else if (stdInfo.ClassID == 207)
                {
                    totalResult.OverallGrade = GetGrade(PercentageCal(totalResult.OverallAllTotal, 680), Convert.ToInt32(stdInfo.ClassID));
                }

                // Handle the Pre-Exam Grades if termId is not 10
                if (termId != 10)
                {
                    totalResult.Pre1Grade = GetGrade(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID));
                    totalResult.Pre1AllTotal = Pre1AllTotal;
                    totalResult.Pre2AllTotal = Pre2AllTotal;
                    totalResult.Pre2Grade = GetGrade(PercentageCal(Pre2AllTotal, divisorPre02), Convert.ToInt32(stdInfo.ClassID));

                    // Set Pre Exam grades percentage
                    totalResultpercentage.TheoryTotalPre1 = PercentageCal(Pre1AllTotal, Pre1TheoryMaxTotal);
                    // totalResultpercentage.PracticalTotalPre1 = PercentageCal(Pre1PracticalMaxMark, Pre1PracticalMaxTotal == 0 ? 1 : Pre1PracticalMaxTotal);
                    totalResultpercentage.Pre1AllTotal = PercentageCal(Pre1AllTotal, divisorPre01);
                    totalResultpercentage.Pre1Grade = GetGrade(PercentageCal(Pre1AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID));

                    totalResultpercentage.TheoryTotalPre2 = PercentageCal(Pre2AllTotal, Pre2TheoryMaxTotal);
                    //  totalResultpercentage.PracticalTotalPre2 = PercentageCal(Pre2PracticalMaxMark, Pre2PracticalMaxTotal == 0 ? 1 : Pre2PracticalMaxTotal);
                    totalResultpercentage.Pre2AllTotal = PercentageCal(Pre2AllTotal, divisorPre01);
                    totalResultpercentage.Pre2Grade = GetGrade(PercentageCal(Pre2AllTotal, divisorPre01), Convert.ToInt32(stdInfo.ClassID));
                }

                // Check for invalid grades and reset
                var validGrade = false;
                if (termId == 7 || termId == 8 || termId == 10)
                {
                    validGrade = termId == 7 ? subjectDatas.Any(x => x.TotalObtainedMarksPre1 <= 32) :
                                termId == 8 ? subjectDatas.Any(x => x.TotalObtainedMarksPre2 <= 32) :
                                subjectDatas.Any(x => x.TotalObtainedMarks <= 32);

                    if (validGrade)
                    {
                        // Reset grades to empty for invalid cases
                        ResetGrades(totalResult, totalResultpercentage);
                    }
                    else
                    {
                        // Remove grade "D" for valid cases
                        RemoveDGrade(totalResult, totalResultpercentage);
                    }
                }


                //Step 1 Working
                // Function to set grade to "D" when all relevant marks are 0
                void SetAbsentGradeForSubjectData(SubjectData subjectData)
                {
                    if (subjectData.MarksUT1 == 0 && subjectData.MarksUT2 == 0 && subjectData.TotalMarks == 0 &&
                        subjectData.TheoryMarks == 0 && subjectData.PracticalMarks == 0 && subjectData.TotalObtainedMarks == 0 &&
                        subjectData.TheoryMarksUT2 == 0 && subjectData.PracticalMarksUT2 == 0 && subjectData.TotalMarksBothUTs == 0 &&
                        subjectData.TheoryMarksPre1 == 0 && subjectData.PracticalMarksPre1 == 0 && subjectData.TheoryMarksPre2 == 0 &&
                        subjectData.PracticalMarksPre2 == 0 && subjectData.TotalObtainedMarksPre1 == 0 && subjectData.TotalObtainedMarksPre2 == 0)
                    {
                        subjectData.GradeUT1 = subjectData.GradeUT2 = subjectData.FinalGrade = subjectData.GradePre1 = subjectData.GradePre2 = "D";
                    }
                }

                // Apply the above function to all subject data
                foreach (var subjectData in subjectDatas)
                {
                    SetAbsentGradeForSubjectData(subjectData);
                }

                // Apply the function to totalResult
                SetAbsentGradeForTotalResult(totalResult);

                // Set Pre1Grade and Pre2Grade to "D" if their corresponding values are 0
                if (termId == 7)
                {
                    if (totalResult.Pre1TheoryMaxTotal == 0 && totalResult.Pre1AllTotal == 0)
                    {
                        totalResult.Pre1Grade = "D";
                    }
                    if (totalResult.Pre2TheoryMaxTotal == 0 && totalResult.Pre2AllTotal == 0)
                    {
                        totalResult.Pre2Grade = "D";
                    }
                }


                var classCoscholastic = _context.tbl_CoScholasticClass
                                                .Where(x => x.ClassID == stdInfo.ClassID)
                                                .Select(x => x.CoscholasticID)
                                                .ToList();


                var coscholasticRecords = _context.tbl_CoScholastic
                                                .Where(record => classCoscholastic.Contains(record.Id))
                                                .ToList();

                // Fetch results for all terms
                var resultTerm1 = GetResultsForTerm(1, Batch_Id, studentId, coscholasticRecords);
                var resultTerm2 = GetResultsForTerm(4, Batch_Id, studentId, coscholasticRecords);
                var resultPre1 = termId != 10 ? GetResultsForTerm(7, Batch_Id, studentId, coscholasticRecords) : new List<CoscholasticResultModel>();
                var resultPre2 = termId != 10 ? GetResultsForTerm(8, Batch_Id, studentId, coscholasticRecords) : new List<CoscholasticResultModel>();
                var resultUT1 = GetResultsForTerm(1, Batch_Id, studentId, coscholasticRecords); // Assuming UT1 and Term1 overlap
                var resultUT2 = GetResultsForTerm(3, Batch_Id, studentId, coscholasticRecords); // Assuming UT2 maps to Term3

                // Combine results
                var combinedResults = coscholasticRecords.Select(c => new
                {
                    Title = c.Title,
                    GradeTerm1 = resultTerm1.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade,
                    GradeTerm2 = resultTerm2.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade,
                    GradePre1 = resultPre1.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade,
                    GradePre2 = resultPre2.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade,
                    GradeUT1 = resultUT1.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade,
                    GradeUT2 = resultUT2.FirstOrDefault(x => x.CoscholasticID == c.Id)?.ObtainedGrade
                }).ToList();

                studentReportData.coscholasticAreaDatas = combinedResults.Select(item => new CoscholasticAreaData
                {
                    Name = item.Title,
                    GradeTerm1 = item.GradeTerm1 ?? "-",
                    GradeTerm2 = item.GradeTerm2 ?? "-",
                    GradePre1 = item.GradePre1 ?? "-",
                    GradePre2 = item.GradePre2 ?? "-",
                    GradeUT1 = item.GradeUT1 ?? "-",
                    GradeUT2 = item.GradeUT2 ?? "-"
                }).ToList();


                var gradinglist = _context.gradingCriteria.Where(x => x.TermID == termId && x.BatchID == Batch_Id && x.ClassID == stdInfo.ClassID).ToList();
                studentReportData.gradingCriteria = gradinglist;


                studentReportData.totalResult = totalResult;
                studentReportData.totalResultPercentage = totalResultpercentage;
                studentReportData.subjectDatas = subjectDatas;
                studentReportData.optionalSubjectDatas = optionalSubjectDatas;
                studentReportData.totalResult.T1Grade = studentReportData.totalResult.T1Grade;
                studentReportData.totalResultPercentage.T1Grade = studentReportData.totalResultPercentage.T1Grade;
                string termName = _context.tbl_Term.Where(x => x.TermID == termId).Select(t => t.TermName).FirstOrDefault();

                var gradeCounts = new Dictionary<int, int>
                {
                    { 1, subjectDatas.Count(x => x.MarksUT1Grade == "D") },
                    { 2, subjectDatas.Count(x => x.MarksUT2Grade == "D") },
                    { 3, subjectDatas.Count(x => x.GradeUT1 == "D") },
                    { 4, subjectDatas.Count(x => x.GradeUT2 == "D") },
                    { 7, subjectDatas.Count(x => x.GradePre1 == "D") },
                    { 8, subjectDatas.Count(x => x.GradePre2 == "D") },
                    { 10, subjectDatas.Count(x => x.FinalGrade == "D") }
                };

                string result = gradeCounts.TryGetValue(termId, out int count) ? (count > 0 ? "" : "Pass") : "Invalid grade";

                studentReportData.Result = result;
                return Json(studentReportData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }


        // Helper method to reset grades
        void ResetGrades(TotalResult totalResult, TotalResultpercentage totalResultpercentage)
        {
            totalResult.T1Grade = totalResult.T2Grade = totalResult.UT1TotalGrade = totalResult.UT2TotalGrade = totalResult.Pre1Grade = totalResult.Pre2Grade = totalResult.OverallGrade = "";
            totalResultpercentage.T1Grade = totalResultpercentage.T2Grade = totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT2TotalGrade = totalResultpercentage.Pre1Grade = totalResultpercentage.Pre2Grade = totalResultpercentage.OverallGrade = "";
        }

        // Helper method to remove grade "D"
        void RemoveDGrade(TotalResult totalResult, TotalResultpercentage totalResultpercentage)
        {
            totalResult.T1Grade = totalResult.T1Grade == "D" ? "" : totalResult.T1Grade;
            totalResult.T2Grade = totalResult.T2Grade == "D" ? "" : totalResult.T2Grade;
            totalResult.UT1TotalGrade = totalResult.UT1TotalGrade == "D" ? "" : totalResult.UT1TotalGrade;
            totalResult.UT2TotalGrade = totalResult.UT2TotalGrade == "D" ? "" : totalResult.UT2TotalGrade;
            totalResultpercentage.T1Grade = totalResultpercentage.T1Grade == "D" ? "" : totalResultpercentage.T1Grade;
            totalResultpercentage.T2Grade = totalResultpercentage.T2Grade == "D" ? "" : totalResultpercentage.T2Grade;
            totalResultpercentage.UT1TotalGrade = totalResultpercentage.UT1TotalGrade == "D" ? "" : totalResultpercentage.UT1TotalGrade;
            totalResultpercentage.UT2TotalGrade = totalResultpercentage.UT2TotalGrade == "D" ? "" : totalResultpercentage.UT2TotalGrade;
        }

        // Function to set grade to "D" for the total result if all totals are 0
        void SetAbsentGradeForTotalResult(TotalResult totalResult)
        {
            if (totalResult.UT1Total == 0 && totalResult.UT2Total == 0 && totalResult.UTAllTotal == 0 &&
                totalResult.TheoryTotalT1 == 0 && totalResult.PracticalTotalT1 == 0 && totalResult.T1AllTotal == 0 &&
                totalResult.TheoryTotalT2 == 0 && totalResult.PracticalTotalT2 == 0 && totalResult.T2AllTotal == 0 &&
                totalResult.OverallAllTotal == 0)
            {
                totalResult.T1Grade = totalResult.T2Grade = totalResult.OverallGrade = "D";
            }
        }
        private List<CoscholasticResultModel> GetResultsForTerm(int termId, int batchId, int studentId, List<Tbl_CoScholastic> coscholasticRecords)
        {
            return (from c in coscholasticRecords
                    join cr in _context.tbl_CoScholastic_Result
                    on c.Id equals cr.CoScholasticID into crGroup
                    from cr in crGroup.DefaultIfEmpty()
                    join cog in _context.tbl_CoScholasticObtainedGrade
                    on cr?.Id equals cog.ObtainedCoScholasticID into cogGroup
                    from cog in cogGroup.DefaultIfEmpty()
                    where cr == null || cog == null ||
                        (cr.StudentID == studentId && cr.TermID == termId && cog.BatchId == batchId)
                    select new CoscholasticResultModel
                    {
                        CoscholasticID = c.Id,
                        Title = c.Title,
                        ObtainedGrade = cog?.ObtainedGrade
                    }).ToList();
        }

        Tbl_TestRecords ProcessMarks(int termID, string testType, int studentId, Tbl_Tests testItem)
        {
            var studentMark = (from cr in _context.Tbl_TestRecord
                               join cog in _context.tbl_TestObtainedMark on cr.RecordID equals cog.RecordIDFK
                               where cr.StudentID == studentId && cr.TermID == termID && cog.TestID == testItem.TestID
                               select new
                               {
                                   TestID = cog.TestID,
                                   ObtainedMarks = cog.ObtainedMarks
                               }).FirstOrDefault();

            Tbl_TestRecords tbl_TestRecords = new Tbl_TestRecords
            {
                ObtainedMarks = studentMark?.ObtainedMarks ?? 0
            };

            // Update relevant marks and totals
            if (testType == "Theory")
            {
                //TotalObtainedMarks += tbl_TestRecords.ObtainedMarks;

            }
            else if (testType == "Practical")
            {
                //TotalObtainedMarks += tbl_TestRecords.ObtainedMarks;
                //practicalMaxMark = testItem.MaximumMarks;
            }

            return tbl_TestRecords;
        }

        public async Task<ActionResult> PrintStudentReportCardDataForCLass(int termId, int BatchId, int classId, int sectionId)
        {
            try
            {
                // 1. Data fetch
                var printReportCard = await GetReportCardDataByClassAsync(termId, BatchId, classId, sectionId);

                // 2. Razor view ko HTML string me convert karo
                string htmlContent = this.RenderViewToString("ClassWiseReportClas", printReportCard);

                // 3. Puppeteer launch (AspNetFramework bundled Chromium ka use karega)
                var launchOptions = new LaunchOptions
                {
                    Headless = true,
                    Args = new[] { "--no-sandbox", "--disable-setuid-sandbox", "--ignore-certificate-errors" }
                };

                using (var browser = await PuppeteerSharp.Puppeteer.LaunchAsync(launchOptions))
                using (var page = await browser.NewPageAsync())
                {
                    // 4. HTML set karo
                    await page.SetContentAsync(htmlContent, new NavigationOptions
                    {
                        WaitUntil = new[] { WaitUntilNavigation.Load, WaitUntilNavigation.Networkidle0 }
                    });

                    // 5. PDF options
                    var pdfOptions = new PdfOptions
                    {
                        Format = PaperFormat.A4,
                        MarginOptions = new PuppeteerSharp.Media.MarginOptions
                        {
                            Top = "10mm",
                            Bottom = "10mm",
                            Left = "10mm",
                            Right = "10mm"
                        },
                        PrintBackground = true
                    };

                    // 6. PDF generate
                    var pdfStream = await page.PdfStreamAsync(pdfOptions);

                    // 7. Return PDF to client
                    return File(pdfStream, "application/pdf", "ReportCard.pdf");
                }
            }
            catch (Exception ex)
            {
                // Error log karo
                throw;
            }
        }

        //        public async Task<List<PrintReportCardData>> GetReportCardDataByClassAsync(int termId, int BatchId, int classId, int sectionId)
        //        {
        //            var studentIds = new List<long>();

        //            var studentIdsQuery = @"SELECT DISTINCT s.StudentId FROM Tbl_TestRecords tr JOIN Students s ON tr.StudentID = s.StudentId WHERE s.IsApplyforTC = 0 AND tr.ClassID = @ClassId AND SectionID = @SectionId AND TermID = @TermId AND BatchId = @BatchId ORDER BY s.StudentId";

        //            var studentIdsParameters = new[]
        //            {
        //    new SqlParameter("@BatchId", BatchId),
        //    new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId),
        //    new SqlParameter("@TermId", termId),
        //    new SqlParameter("@SectionId", sectionId)
        //};

        //            var connectionString = System.Configuration.ConfigurationManager
        //                .ConnectionStrings["DefaultConnection"].ConnectionString;

        //            var schoolDetails = await _context.TblCreateSchool
        //                .Select(x => new
        //                {
        //                    x.School_Name,
        //                    x.Address,
        //                    x.CurrentYear,
        //                    x.Upload_Image
        //                })
        //                .OrderBy(x => x.School_Name)
        //                .FirstOrDefaultAsync();

        //            var schoolLogoPath = string.IsNullOrEmpty(schoolDetails?.Upload_Image) ? "/Content/Default/default-logo.jpeg" : $"/Content/SchoolImages/{Uri.UnescapeDataString(schoolDetails.Upload_Image)}";

        //            var printReportCards = new List<PrintReportCardData>();

        //            using (var connection = new SqlConnection(connectionString))
        //            {
        //                await connection.OpenAsync();

        //                // Get student IDs
        //                using (var command = connection.CreateCommand())
        //                {
        //                    command.CommandText = studentIdsQuery;
        //                    command.CommandType = CommandType.Text;
        //                    command.Parameters.AddRange(studentIdsParameters);

        //                    using (var reader = await command.ExecuteReaderAsync())
        //                    {
        //                        while (await reader.ReadAsync())
        //                        {
        //                            studentIds.Add(reader.GetInt32(0));
        //                        }
        //                    }
        //                }

        //                studentIds = studentIds.Distinct().ToList();

        //                // Load grading criteria
        //                var gradingCriteria = new List<GradingCriteria>();
        //                using (var command = connection.CreateCommand())
        //                {
        //                    command.CommandText = @"SELECT MaximumPercentage, MinimumPercentage, Grade, GradeDescription FROM GradingCriterias WHERE BatchID = @BatchId AND ClassID = @ClassId AND TermId = @TermId";

        //                    command.CommandType = CommandType.Text;
        //                    command.Parameters.Add(new SqlParameter("@BatchId", BatchId));
        //                    command.Parameters.Add(new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId));
        //                    command.Parameters.Add(new SqlParameter("@TermId", termId));

        //                    using (var reader = await command.ExecuteReaderAsync())
        //                    {
        //                        while (await reader.ReadAsync())
        //                        {
        //                            gradingCriteria.Add(new GradingCriteria
        //                            {
        //                                MaximumPercentage = reader.GetDecimal(0),
        //                                MinimumPercentage = reader.GetDecimal(1),
        //                                Grade = reader["Grade"].ToString(),
        //                                GradeDescription = reader["GradeDescription"].ToString()
        //                            });
        //                        }
        //                    }
        //                }

        //                // Call stored procedure
        //                var reportCardParameters = new[]
        //                {
        //        new SqlParameter("@Batch_Id", BatchId),
        //        new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId),
        //        new SqlParameter("@TermId", termId),
        //        new SqlParameter("@SectionId", sectionId)
        //    };

        //                using (var command = connection.CreateCommand())
        //                {
        //                    command.CommandText = "EXEC GetReportCardbyClass @Batch_Id, @ClassId, @SectionId, @TermId";
        //                    command.CommandType = CommandType.Text;
        //                    command.Parameters.AddRange(reportCardParameters);

        //                    using (var reader = await command.ExecuteReaderAsync())
        //                    {
        //                        var studentDataList = new Dictionary<long, StudentData>();
        //                        var studentMarksList = new List<TermSubjectMarks>();
        //                        var coscholasticDataLookup = new Dictionary<long, List<CoscholasticAreaDatas>>();

        //                        // First Result: Student Data
        //                        while (await reader.ReadAsync())
        //                        {
        //                            var studentId = reader.GetInt64(reader.GetOrdinal("StudentId"));
        //                            if (!studentDataList.ContainsKey(studentId))
        //                            {
        //                                studentDataList[studentId] = new StudentData
        //                                {
        //                                    StudentID = studentId,
        //                                    StudentName = reader["StudentName"].ToString(),
        //                                    FatherName = reader["FatherName"].ToString(),
        //                                    MotherName = reader["MotherName"].ToString(),
        //                                    ScholarNo = reader["ScholarNo"].ToString(),
        //                                    RollNo = reader["RollNo"].ToString(),
        //                                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? "" : TryParseDate(reader["DateOfBirth"].ToString()),
        //                                    AcademicYear = reader["AcademicYear"].ToString(),
        //                                    ClassName = reader["ClassName"].ToString(),
        //                                    SectionName = reader["SectionName"].ToString(),
        //                                    Attendance = Attendance(BatchId, classId, studentId, sectionId),
        //                                    PromotedClass = reader["PromotedClass"].ToString(),
        //                                    StaffSignatureLink = reader["StaffSignatureLink"].ToString(),
        //                                    PrincipalSign = reader["PrincipalSign"].ToString(),
        //                                    Remark = reader["Remark"].ToString(),
        //                                    RankInClass = reader["RankInClass"].ToString(),
        //                                    ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
        //                                    SectionID = sectionId,
        //                                    SchoolLogo = schoolLogoPath,
        //                                    SchoolName = schoolDetails.School_Name ?? "",
        //                                    CurrentYear = schoolDetails.CurrentYear,
        //                                    BatchID = BatchId,
        //                                    TermID = termId
        //                                };
        //                            }
        //                        }

        //                        // Second Result: Marks
        //                        if (await reader.NextResultAsync())
        //                        {
        //                            while (await reader.ReadAsync())
        //                            {
        //                                studentMarksList.Add(new TermSubjectMarks
        //                                {
        //                                    TestID = reader.GetInt64(reader.GetOrdinal("TestID")),
        //                                    TermName = reader["Term"].ToString(),
        //                                    SubjectName = reader["Subject"].ToString(),
        //                                    Mark = decimal.Parse(reader["ObtainedMarks"].ToString()),
        //                                    TestType = reader["TestType"].ToString(),
        //                                    MaximumMarks = decimal.Parse(reader["MaximumMarks"].ToString()),
        //                                    Grade = reader["Grade"].ToString(),
        //                                    IsOptional = bool.Parse(reader["IsOptional"].ToString()),
        //                                    SubjectId = reader.GetInt64(reader.GetOrdinal("SubjectID")),
        //                                    Studentid = reader.GetInt32(reader.GetOrdinal("StudentId")),
        //                                    SerialNumber = reader.GetInt32(reader.GetOrdinal("SerialNumber"))
        //                                });
        //                            }
        //                        }

        //                        foreach (var sid in studentIds)
        //                        {
        //                            var studentId = sid; // ✅ avoid CS1656 error
        //                            if (!studentDataList.TryGetValue(studentId, out var studentData))
        //                                continue;

        //                            var studentMarks = studentMarksList.Where(x => x.Studentid == studentId).ToList();

        //                            var groupedSubjects = studentMarks
        //                                .GroupBy(r => r.SubjectName)
        //                                .Select(g => new GroupedSubjects
        //                                {
        //                                    SubjectName = g.Key,
        //                                    SerialNumber = g.FirstOrDefault().SerialNumber,
        //                                    SubjectId = g.FirstOrDefault().SubjectId,
        //                                    IsOptional = g.FirstOrDefault()?.IsOptional ?? false,
        //                                    Terms = g.Where(x => x.TestType.ToUpper() != "PRACTICAL")
        //                                        .Select(t =>
        //                                        {
        //                                            var practical = g.FirstOrDefault(p =>
        //                                                p.SubjectName == g.Key &&
        //                                                p.TermName == t.TermName &&
        //                                                p.TestType.ToUpper() == "PRACTICAL");

        //                                            var practicalMark = practical?.Mark ?? 0;
        //                                            var practicalMax = practical?.MaximumMarks ?? 0;

        //                                            var totalMark = t.Mark + practicalMark;
        //                                            var maxMark = t.MaximumMarks + practicalMax;

        //                                            var percent = maxMark > 0 ? Math.Round((totalMark / maxMark) * 100, 2) : 0;
        //                                            var finalGrade = t.IsOptional ? t.Grade : GetGrade(percent, studentData.ClassID, termId, BatchId);

        //                                            return new SubjectTermRecord
        //                                            {
        //                                                Name = t.TermName,
        //                                                TheoryMark = t.Mark,
        //                                                PracticalMark = practicalMark,
        //                                                MaximumMarks = maxMark,
        //                                                TotallMark = totalMark,
        //                                                Grade = finalGrade,
        //                                                IsOptional = t.IsOptional
        //                                            };
        //                                        }).ToList()
        //                                })
        //                                .OrderBy(x => x.SerialNumber)
        //                                 .ToList();

        //                            var groupedTerms = studentMarks
        //                                .Where(x => !x.IsOptional)
        //                                .GroupBy(x => new { x.TermName, x.TestType })
        //                                .Select(tr =>
        //                                {
        //                                    var totalMarks = tr.Sum(x => x.Mark);
        //                                    var maxMarksForGroup = tr.FirstOrDefault()?.MaximumMarks ?? 0;
        //                                    var totalMaxMarks = studentMarks
        //                                        .Where(x => x.TermName == tr.Key.TermName && x.TestType == tr.Key.TestType && !x.IsOptional)
        //                                        .Sum(x => x.MaximumMarks);

        //                                    decimal percentage = totalMaxMarks > 0 ? Math.Round((totalMarks / totalMaxMarks) * 100, 2) : 0;
        //                                    var grade = GetGrade(percentage, studentData.ClassID, termId, BatchId);

        //                                    return new GroupedTerms
        //                                    {
        //                                        Term = tr.Key.TermName,
        //                                        TestType = tr.Key.TestType,
        //                                        Total = totalMarks,
        //                                        MaximumMarks = maxMarksForGroup,
        //                                        Percentage = percentage,
        //                                        Grade = grade
        //                                    };
        //                                }).ToList();

        //                            decimal ObtainedPercent = 0;
        //                            string ObtainedGrade = "";
        //                            decimal overallTotalMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.Mark);
        //                            decimal overallTotalMaxMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.MaximumMarks);

        //                            if (overallTotalMaxMarks > 0)
        //                            {
        //                                ObtainedPercent = Math.Round((overallTotalMarks / overallTotalMaxMarks) * 100, 2);
        //                                ObtainedGrade = GetGrade(ObtainedPercent, studentData.ClassID, termId, BatchId);
        //                            }

        //                            string Result = (groupedSubjects != null && groupedSubjects.Any()
        //                                && !groupedSubjects.Any(g => g.Terms.Any(t => t.Grade == "D" || t.Grade == "E")))
        //                                ? "Pass" : "";

        //                            var terms = studentMarks.Select(x => x.TermName).Distinct().ToList();

        //                            printReportCards.Add(new PrintReportCardData
        //                            {
        //                                StudentData = studentData,
        //                                GroupedSubjects = groupedSubjects,
        //                                GroupedTerms = groupedTerms,
        //                                Term = terms,
        //                                GradingCriteria = gradingCriteria,
        //                                CoscholasticAreaData = coscholasticDataLookup.ContainsKey(studentId)
        //                                    ? coscholasticDataLookup[studentId]
        //                                    : new List<CoscholasticAreaDatas>(),
        //                                ObtainedPercent = ObtainedPercent,
        //                                ObtainedGrade = ObtainedGrade,
        //                                Result = Result,
        //                            });
        //                        }
        //                    }
        //                }

        //            }

        //            return printReportCards;
        //        }
        public ActionResult CreateSubjectTest(int? ClassId = null, int? sectionId = null, int? batchId = null, int? termId = null)
        {
            var model = new CreatesubjecttestModel
            {
                ClassId = ClassId ?? 0,
                SectionId = sectionId ?? 0,
                BatchId = batchId ?? 0,
                TermId = termId ?? 0
            };

            // ---- Dropdown Data ----
            var classDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "class")
                .Select(x => x.DataListId)
                .FirstOrDefault();
            ViewBag.ClassList = _context.DataListItems
                .Where(x => x.DataListId == classDataListId.ToString())
                .ToList();

            var sectionDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "section")
                .Select(x => x.DataListId)
                .FirstOrDefault();
            ViewBag.Section = _context.DataListItems
                .Where(x => x.DataListId == sectionDataListId.ToString())
                .ToList();

            ViewBag.Batch = _context.Tbl_Batches.ToList();
            ViewBag.Term = _context.tbl_Term.ToList();

            // ---- Direct SQL Query ----
            string sql = @" exec getresultsubjectserial @ClassId,@TermId,@BatchId,@SectionId";

            var subjectList = _context.Database.SqlQuery<SubjectTestFullDTO>(
                sql,
                new SqlParameter("@ClassId", model.ClassId),
                new SqlParameter("@SectionId", model.SectionId),
                new SqlParameter("@BatchId", model.BatchId),
                new SqlParameter("@TermId", model.TermId)
            ).ToList();

            // Map for View
            model.SubjectTestList = subjectList.Select(s => new SubjectTestDTO
            {
                SubjectId = s.Subject_ID,
                SubjectName = s.Subject_Name,
                IsOptional = s.IsOptional
            }).ToList();

            return View(model);
        }
        [HttpPost]
        public ActionResult SaveSubjectTestSerial(List<SubjectTestSaveDTO> SubjectTestData)
        {
            // ✅ Check if any SerialNumber is missing
            if (SubjectTestData.Any(x => x.SerialNumber <= 0))
            {
                TempData["Message"] = "Please enter all serial numbers before saving.";
                return RedirectToAction("CreateSubjectTest", new
                {
                    ClassId = SubjectTestData.FirstOrDefault()?.ClassId,
                    SectionId = SubjectTestData.FirstOrDefault()?.SectionId,
                    BatchId = SubjectTestData.FirstOrDefault()?.BatchId,
                    TermId = SubjectTestData.FirstOrDefault()?.TermId
                });
            }

            string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (var conn = new System.Data.SqlClient.SqlConnection(connStr))
            {
                conn.Open();

                foreach (var item in SubjectTestData)
                {
                    string checkSql = @"
                SELECT COUNT(*) FROM SubjectSerials
                WHERE SubjectId = @SubjectId AND ClassId = @ClassId AND SectionId = @SectionId 
                      AND BatchId = @BatchId AND TermId = @TermId";

                    using (var checkCmd = new System.Data.SqlClient.SqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@SubjectId", item.SubjectId);
                        checkCmd.Parameters.AddWithValue("@ClassId", item.ClassId);
                        checkCmd.Parameters.AddWithValue("@SectionId", item.SectionId);
                        checkCmd.Parameters.AddWithValue("@BatchId", item.BatchId);
                        checkCmd.Parameters.AddWithValue("@TermId", item.TermId);

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // ✅ Update
                            string updateSql = @"
                        UPDATE SubjectSerials SET 
                            SerialNumber = @SerialNumber,
                            UpdatedDate = @UpdatedDate
                        WHERE SubjectId = @SubjectId AND ClassId = @ClassId 
                              AND SectionId = @SectionId AND BatchId = @BatchId AND TermId = @TermId";

                            using (var updateCmd = new System.Data.SqlClient.SqlCommand(updateSql, conn))
                            {
                                updateCmd.Parameters.AddWithValue("@SerialNumber", item.SerialNumber);
                                updateCmd.Parameters.AddWithValue("@UpdatedDate", DateTime.Now);
                                updateCmd.Parameters.AddWithValue("@SubjectId", item.SubjectId);
                                updateCmd.Parameters.AddWithValue("@ClassId", item.ClassId);
                                updateCmd.Parameters.AddWithValue("@SectionId", item.SectionId);
                                updateCmd.Parameters.AddWithValue("@BatchId", item.BatchId);
                                updateCmd.Parameters.AddWithValue("@TermId", item.TermId);

                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // ✅ Insert
                            string insertSql = @"
                        INSERT INTO SubjectSerials
                            (SubjectId, SubjectName, ClassId, SectionId, BatchId, TermId, IsOptional, SerialNumber, CreatedDate)
                        VALUES
                            (@SubjectId, @SubjectName, @ClassId, @SectionId, @BatchId, @TermId, @IsOptional, @SerialNumber, @CreatedDate)";

                            using (var insertCmd = new System.Data.SqlClient.SqlCommand(insertSql, conn))
                            {
                                insertCmd.Parameters.AddWithValue("@SubjectId", item.SubjectId);
                                insertCmd.Parameters.AddWithValue("@SubjectName", item.SubjectName ?? (object)DBNull.Value);
                                insertCmd.Parameters.AddWithValue("@ClassId", item.ClassId);
                                insertCmd.Parameters.AddWithValue("@SectionId", item.SectionId);
                                insertCmd.Parameters.AddWithValue("@BatchId", item.BatchId);
                                insertCmd.Parameters.AddWithValue("@TermId", item.TermId);
                                insertCmd.Parameters.AddWithValue("@IsOptional", (object)(item.IsOptional ?? false));
                                insertCmd.Parameters.AddWithValue("@SerialNumber", item.SerialNumber);
                                insertCmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                conn.Close();
            }

            TempData["Message"] = "Saved successfully!";
            return RedirectToAction("CreateSubjectTest", new
            {
                ClassId = SubjectTestData.FirstOrDefault()?.ClassId,
                SectionId = SubjectTestData.FirstOrDefault()?.SectionId,
                BatchId = SubjectTestData.FirstOrDefault()?.BatchId,
                TermId = SubjectTestData.FirstOrDefault()?.TermId
            });
        }

        public async Task<List<PrintReportCardData>> GetReportCardDataByClassAsync(int termId, int BatchId, int classId, int sectionId)
        {
            var studentIds = new List<long>();

            var studentIdsQuery = @"SELECT DISTINCT s.StudentId FROM Tbl_TestRecords tr JOIN Students s ON tr.StudentID = s.StudentId WHERE s.IsApplyforTC = 0 AND tr.ClassID = @ClassId AND SectionID = @SectionId AND TermID = @TermId AND BatchId = @BatchId ORDER BY s.StudentId";

            var studentIdsParameters = new[]
            {
            new SqlParameter("@BatchId", BatchId),
            new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId),
            new SqlParameter("@TermId", termId),
            new SqlParameter("@SectionId", sectionId)
        };

            var connectionString = System.Configuration.ConfigurationManager
                .ConnectionStrings["DefaultConnection"].ConnectionString;

            var schoolDetails = await _context.TblCreateSchool
                .Select(x => new
                {
                    x.School_Name,
                    x.Address,
                    x.CurrentYear,
                    x.Upload_Image
                })
                .OrderBy(x => x.School_Name)
                .FirstOrDefaultAsync();

            //var schoolLogoPath = string.IsNullOrEmpty(schoolDetails?.Upload_Image) ? "/Content/Default/default-logo.jpeg" : $"/Content/SchoolImages/{Uri.UnescapeDataString(schoolDetails.Upload_Image)}";
            var schoolLogoPath = string.IsNullOrEmpty(schoolDetails?.Upload_Image) ? "/Content/Default/default-logo.jpeg" : $"/WebsiteImages/SchoolImage/{Uri.UnescapeDataString(schoolDetails.Upload_Image)}";
            var printReportCards = new List<PrintReportCardData>();

            using (var connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                // Get student IDs
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = studentIdsQuery;
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(studentIdsParameters);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            studentIds.Add(reader.GetInt32(0));
                        }
                    }
                }

                studentIds = studentIds.Distinct().ToList();

                // Load grading criteria
                var gradingCriteria = new List<GradingCriteria>();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT MaximumPercentage, MinimumPercentage, Grade, GradeDescription FROM GradingCriterias WHERE BatchID = @BatchId AND ClassID = @ClassId AND TermId = @TermId";

                    command.CommandType = CommandType.Text;
                    command.Parameters.Add(new SqlParameter("@BatchId", BatchId));
                    command.Parameters.Add(new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId));
                    command.Parameters.Add(new SqlParameter("@TermId", termId));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            gradingCriteria.Add(new GradingCriteria
                            {
                                MaximumPercentage = reader.GetDecimal(0),
                                MinimumPercentage = reader.GetDecimal(1),
                                Grade = reader["Grade"].ToString(),
                                GradeDescription = reader["GradeDescription"].ToString()
                            });
                        }
                    }
                }

                // Call stored procedure
                var reportCardParameters = new[]
                {
                new SqlParameter("@Batch_Id", BatchId),
                new SqlParameter("@ClassId", (classId == 0) ? DBNull.Value : (object)classId),
                new SqlParameter("@TermId", termId),
                new SqlParameter("@SectionId", sectionId)
            };

                //using (var command = connection.CreateCommand())
                //{
                //    command.CommandText = "EXEC GetReportCardbyClass @Batch_Id, @ClassId, @SectionId, @TermId";
                //    command.CommandType = CommandType.Text;
                //    command.Parameters.AddRange(reportCardParameters);

                //    using (var reader = await command.ExecuteReaderAsync())
                //    {
                //        var studentDataList = new Dictionary<int, StudentData>();
                //        var studentMarksList = new List<TermSubjectMarks>();

                //        while (await reader.ReadAsync())
                //        {
                //            var studentId = reader.GetInt32(reader.GetOrdinal("StudentId"));
                //            if (!studentDataList.ContainsKey(studentId))
                //            {
                //                studentDataList[studentId] = new StudentData
                //                {
                //                    StudentID = studentId,
                //                    StudentName = reader["StudentName"].ToString(),
                //                    FatherName = reader["FatherName"].ToString(),
                //                    MotherName = reader["MotherName"].ToString(),
                //                    ScholarNo = reader["ScholarNo"].ToString(),
                //                    RollNo = reader["RollNo"].ToString(),
                //                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? "" : TryParseDate(reader["DateOfBirth"].ToString()),
                //                    AcademicYear = reader["AcademicYear"].ToString(),
                //                    ClassName = reader["ClassName"].ToString(),
                //                    SectionName = reader["SectionName"].ToString(),
                //                    Attendance = Attendance(BatchId, classId, studentId, sectionId),
                //                    PromotedClass = reader["PromotedClass"].ToString(),
                //                    StaffSignatureLink = reader["StaffSignatureLink"].ToString(),
                //                    PrincipalSign = reader["PrincipalSign"].ToString(),
                //                    Remark = reader["Remark"].ToString(),
                //                    RankInClass = reader["RankInClass"].ToString(),
                //                    ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                //                    SectionID = sectionId,
                //                    SchoolLogo = schoolLogoPath,
                //                    SchoolName = schoolDetails.School_Name ?? "",
                //                    CurrentYear = schoolDetails.CurrentYear,
                //                    BatchID = BatchId,
                //                    TermID = termId
                //                };
                //            }
                //        }

                //        if (await reader.NextResultAsync())
                //        {
                //            while (await reader.ReadAsync())
                //            {
                //                studentMarksList.Add(new TermSubjectMarks
                //                {
                //                    Studentid = reader.GetInt32(reader.GetOrdinal("StudentId")),
                //                    TestID = reader.GetInt64(reader.GetOrdinal("TestID")),
                //                    TermName = reader["Term"].ToString(),
                //                    SubjectName = reader["Subject"].ToString(),
                //                    Mark = decimal.Parse(reader["ObtainedMarks"].ToString()),
                //                    TestType = reader["TestType"].ToString(),
                //                    MaximumMarks = decimal.Parse(reader["MaximumMarks"].ToString()),
                //                    Grade = reader["Grade"].ToString(),
                //                    IsOptional = bool.Parse(reader["IsOptional"].ToString())
                //                });
                //            }

                //            foreach (var studentId in studentIds)
                //            {
                //                if (!studentDataList.TryGetValue(studentId, out var studentData))
                //                    continue;


                //                var studentMarks = studentMarksList.Where(x => x.Studentid == studentId).ToList();

                //                var groupedSubjects = studentMarks
                //                    .GroupBy(r => r.SubjectName)
                //                    .Select(g => new GroupedSubjects
                //                    {
                //                        SubjectName = g.Key,
                //                        IsOptional = g.FirstOrDefault()?.IsOptional ?? false,
                //                        Terms = g.Where(x => x.TestType.ToUpper() != "PRACTICAL")
                //                            .Select(t =>
                //                            {
                //                                var practical = g.FirstOrDefault(p =>
                //                                    p.SubjectName == g.Key &&
                //                                    p.TermName == t.TermName &&
                //                                    p.TestType.ToUpper() == "PRACTICAL");

                //                                var practicalMark = practical?.Mark ?? 0;
                //                                var practicalMax = practical?.MaximumMarks ?? 0;

                //                                var totalMark = t.Mark + practicalMark;
                //                                var maxMark = t.MaximumMarks + practicalMax;

                //                                var percent = maxMark > 0 ? Math.Round((totalMark / maxMark) * 100, 2) : 0;
                //                                var finalGrade = t.IsOptional ? t.Grade : GetGrade(percent, studentData.ClassID, termId, BatchId);

                //                                return new SubjectTermRecord
                //                                {
                //                                    Name = t.TermName,
                //                                    TheoryMark = t.Mark,
                //                                    PracticalMark = practicalMark,
                //                                    MaximumMarks = maxMark,
                //                                    TotallMark = totalMark,
                //                                    Grade = finalGrade,
                //                                    IsOptional = t.IsOptional
                //                                };
                //                            }).ToList()
                //                    }).ToList();

                //                var groupedTerms = studentMarks
                //                    .Where(x => !x.IsOptional)
                //                    .GroupBy(x => new { x.TermName, x.TestType })
                //                    .Select(tr =>
                //                    {
                //                        var totalMarks = tr.Sum(x => x.Mark);
                //                        var maxMarksForGroup = tr.FirstOrDefault()?.MaximumMarks ?? 0;
                //                        var totalMaxMarks = studentMarks
                //                            .Where(x => x.TermName == tr.Key.TermName && x.TestType == tr.Key.TestType && !x.IsOptional)
                //                            .Sum(x => x.MaximumMarks);

                //                        decimal percentage = totalMaxMarks > 0 ? Math.Round((totalMarks / totalMaxMarks) * 100, 2) : 0;
                //                        var grade = GetGrade(percentage, studentData.ClassID, termId, BatchId);

                //                        return new GroupedTerms
                //                        {
                //                            Term = tr.Key.TermName,
                //                            TestType = tr.Key.TestType,
                //                            Total = totalMarks,
                //                            MaximumMarks = maxMarksForGroup,
                //                            Percentage = percentage,
                //                            Grade = grade
                //                        };
                //                    }).ToList();

                //                decimal ObtainedPercent = 0;
                //                string ObtainedGrade = "";
                //                decimal overallTotalMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.Mark);
                //                decimal overallTotalMaxMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.MaximumMarks);

                //                if (overallTotalMaxMarks > 0)
                //                {
                //                    ObtainedPercent = Math.Round((overallTotalMarks / overallTotalMaxMarks) * 100, 2);
                //                    ObtainedGrade = GetGrade(ObtainedPercent, studentData.ClassID, termId, BatchId);
                //                }

                //                string Result = (groupedSubjects != null && groupedSubjects.Any()
                //                    && !groupedSubjects.Any(g => g.Terms.Any(t => t.Grade == "D" || t.Grade == "E")))
                //                    ? "Pass" : "";

                //                var terms = studentMarks.Select(x => x.TermName).Distinct().ToList();

                //                     var coSchResult = _context.tbl_CoScholastic_Result
                //                    .Where(c => c.ClassID == studentData.ClassID &&
                //                                c.SectionId == studentData.SectionID &&
                //                                c.TermID == termId &&
                //                                c.StudentID == studentId)
                //                    .OrderByDescending(c => c.Id) // Reverse the sort order
                //                    .FirstOrDefault();

                //                List<CoscholasticAreaDatas> coData = new List<CoscholasticAreaDatas>();
                //                string termName = _context.tbl_Term
                //                        .Where(x => x.TermID == termId)
                //                        .Select(x => x.TermName)
                //                        .FirstOrDefault() ?? "";

                //                if (coSchResult != null)
                //                {
                //                    coData = (from cog in _context.tbl_CoScholasticObtainedGrade
                //                              join cr in _context.tbl_CoScholastic on cog.CoscholasticID equals cr.Id
                //                              where cog.ObtainedCoScholasticID == coSchResult.Id &&
                //                                    cog.BatchId == BatchId
                //                              select new CoscholasticAreaDatas
                //                              {
                //                                  Name = cr.Title ?? "",
                //                                  ObtainedGrade = cog.ObtainedGrade ?? "",
                //                                  Term = termName ?? ""
                //                              }).ToList();
                //                }

                //                printReportCards.Add(new PrintReportCardData
                //                {
                //                    StudentData = studentData,
                //                    GroupedSubjects = groupedSubjects,
                //                    GroupedTerms = groupedTerms,
                //                    Term = terms,
                //                    GradingCriteria = gradingCriteria,
                //                    CoscholasticAreaData = coData,
                //                    ObtainedPercent = ObtainedPercent,
                //                    ObtainedGrade = ObtainedGrade,
                //                    Result = Result,
                //                });
                //            }
                //        }
                //    }
                //}
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "EXEC GetReportCardbyClass @Batch_Id, @ClassId, @SectionId, @TermId";
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddRange(reportCardParameters);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var studentDataList = new Dictionary<long, StudentData>();
                        var studentMarksList = new List<TermSubjectMarks>();
                        var coscholasticDataLookup = new Dictionary<long, List<CoscholasticAreaDatas>>();

                        // First Result: Student Data
                        while (await reader.ReadAsync())
                        {
                            var studentId = reader.GetInt64(reader.GetOrdinal("StudentId"));
                            if (!studentDataList.ContainsKey(studentId))
                            {
                                studentDataList[studentId] = new StudentData
                                {
                                    StudentID = studentId,
                                    StudentName = reader["StudentName"].ToString(),
                                    FatherName = reader["FatherName"].ToString(),
                                    MotherName = reader["MotherName"].ToString(),
                                    ScholarNo = reader["ScholarNo"].ToString(),
                                    RollNo = reader["RollNo"].ToString(),
                                    DateOfBirth = reader["DateOfBirth"] == DBNull.Value ? "" : TryParseDate(reader["DateOfBirth"].ToString()),
                                    AcademicYear = reader["AcademicYear"].ToString(),
                                    ClassName = reader["ClassName"].ToString(),
                                    SectionName = reader["SectionName"].ToString(),
                                    Attendance = Attendance(BatchId, classId, studentId, sectionId),
                                    PromotedClass = reader["PromotedClass"].ToString(),
                                    StaffSignatureLink = reader["StaffSignatureLink"].ToString(),
                                    PrincipalSign = reader["PrincipalSign"].ToString(),
                                    Remark = reader["Remark"].ToString(),
                                    RankInClass = reader["RankInClass"].ToString(),
                                    ClassID = reader.GetInt32(reader.GetOrdinal("ClassID")),
                                    SectionID = sectionId,
                                    SchoolLogo = schoolLogoPath,
                                    SchoolName = schoolDetails.School_Name ?? "",
                                    newAddress = schoolDetails.Address ?? "",
                                    CurrentYear = schoolDetails.CurrentYear,
                                    BatchID = BatchId,
                                    TermID = termId
                                };
                            }
                        }

                        // Second Result: Marks
                        if (await reader.NextResultAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                studentMarksList.Add(new TermSubjectMarks
                                {
                                    Studentid = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    TestID = reader.GetInt64(reader.GetOrdinal("TestID")),
                                    TermName = reader["Term"].ToString(),
                                    SubjectName = reader["Subject"].ToString(),
                                    Mark = decimal.Parse(reader["ObtainedMarks"].ToString()),
                                    TestType = reader["TestType"].ToString(),
                                    MaximumMarks = decimal.Parse(reader["MaximumMarks"].ToString()),
                                    Grade = reader["Grade"].ToString(),
                                    IsOptional = bool.Parse(reader["IsOptional"].ToString()),
                                    SubjectId = reader.GetInt64(reader.GetOrdinal("SubjectID")),
                                    //Studentid = reader.GetInt32(reader.GetOrdinal("StudentId")),
                                    SerialNumber = reader.GetInt32(reader.GetOrdinal("SerialNumber"))
                                });
                            }
                        }

                        // Third Result: Coscholastic Data
                        //if (await reader.NextResultAsync())
                        //{
                        //    while (await reader.ReadAsync())
                        //    {
                        //        var studentId = reader.GetInt32(reader.GetOrdinal("StudentId"));

                        //        var coscholastic = new CoscholasticAreaDatas
                        //        {
                        //            Name = reader["Name"].ToString(),
                        //            ObtainedGrade = reader["ObtainedGrade"].ToString(),
                        //            Term = reader["Term"].ToString()
                        //        };

                        //        if (!coscholasticDataLookup.ContainsKey(studentId))
                        //            coscholasticDataLookup[studentId] = new List<CoscholasticAreaDatas>();

                        //        coscholasticDataLookup[studentId].Add(coscholastic);
                        //    }
                        //}

                        // Process Report Cards
                        foreach (var sid in studentIds)
                        {
                            var studentId = sid; // ✅ avoid CS1656 error
                            if (!studentDataList.TryGetValue(studentId, out var studentData))
                                continue;

                            var studentMarks = studentMarksList.Where(x => x.Studentid == studentId).ToList();
                            var groupedSubjects = studentMarks
    .GroupBy(r => r.SubjectName)
    .Select(g => new GroupedSubjects
    {
        SubjectName = g.Key,
        IsOptional = g.FirstOrDefault()?.IsOptional ?? false,
        Terms = g
            .GroupBy(t => t.TermName)
            .Select(tg =>
            {
                var theory = tg.FirstOrDefault(x => x.TestType.ToUpper() == "THEORY");
                var practical = tg.FirstOrDefault(x => x.TestType.ToUpper() == "PRACTICAL");

                var theoryMark = theory?.Mark ?? 0;
                var practicalMark = practical?.Mark ?? 0;

                var totalMark = theoryMark + practicalMark;
                var maxMark = (theory?.MaximumMarks ?? 0) + (practical?.MaximumMarks ?? 0);

                var percent = maxMark > 0 ? Math.Round((totalMark / maxMark) * 100, 2) : 0;

                // Optional subjects use existing grade; non-optional calculate via GetGrade
                var finalGrade = g.FirstOrDefault()?.IsOptional == true
                    ? tg.FirstOrDefault()?.Grade
                    : GetGrade(percent, studentData.ClassID, termId, BatchId);

                return new SubjectTermRecord
                {
                    Name = tg.Key,
                    TheoryMark = theoryMark,
                    PracticalMark = practicalMark,
                    MaximumMarks = maxMark,
                    TotallMark = totalMark,
                    Grade = finalGrade,
                    IsOptional = g.FirstOrDefault()?.IsOptional ?? false
                };
            }).ToList()
    }).ToList();
                            //var groupedSubjects = studentMarks
                            //    .GroupBy(r => r.SubjectName)
                            //    .Select(g => new GroupedSubjects
                            //    {
                            //        SubjectName = g.Key,
                            //        IsOptional = g.FirstOrDefault()?.IsOptional ?? false,
                            //        Terms = g.Where(x => x.TestType.ToUpper() != "PRACTICAL")
                            //            .Select(t =>
                            //            {
                            //                var practical = g.FirstOrDefault(p =>
                            //                    p.SubjectName == g.Key &&
                            //                    p.TermName == t.TermName &&
                            //                    p.TestType.ToUpper() == "PRACTICAL");

                            //                var practicalMark = practical?.Mark ?? 0;
                            //                var practicalMax = practical?.MaximumMarks ?? 0;

                            //                var totalMark = t.Mark + practicalMark;
                            //                var maxMark = t.MaximumMarks + practicalMax;

                            //                var percent = maxMark > 0 ? Math.Round((totalMark / maxMark) * 100, 2) : 0;
                            //                var finalGrade = t.IsOptional ? t.Grade : GetGrade(percent, studentData.ClassID, termId, BatchId);

                            //                return new SubjectTermRecord
                            //                {
                            //                    Name = t.TermName,
                            //                    TheoryMark = t.Mark,
                            //                    PracticalMark = practicalMark,
                            //                    MaximumMarks = maxMark,
                            //                    TotallMark = totalMark,
                            //                    Grade = finalGrade,
                            //                    IsOptional = t.IsOptional
                            //                };
                            //            }).ToList()
                            //    }).ToList();

                            var groupedTerms = studentMarks
                                .Where(x => !x.IsOptional)
                                .GroupBy(x => new { x.TermName, x.TestType })
                                .Select(tr =>
                                {
                                    var totalMarks = tr.Sum(x => x.Mark);
                                    var maxMarksForGroup = tr.FirstOrDefault()?.MaximumMarks ?? 0;
                                    var totalMaxMarks = studentMarks
                                        .Where(x => x.TermName == tr.Key.TermName && x.TestType == tr.Key.TestType && !x.IsOptional)
                                        .Sum(x => x.MaximumMarks);

                                    decimal percentage = totalMaxMarks > 0 ? Math.Round((totalMarks / totalMaxMarks) * 100, 2) : 0;
                                    var grade = GetGrade(percentage, studentData.ClassID, termId, BatchId);

                                    return new GroupedTerms
                                    {
                                        Term = tr.Key.TermName,
                                        TestType = tr.Key.TestType,
                                        Total = totalMarks,
                                        MaximumMarks = maxMarksForGroup,
                                        Percentage = percentage,
                                        Grade = grade
                                    };
                                }).ToList();

                            decimal ObtainedPercent = 0;
                            string ObtainedGrade = "";
                            decimal overallTotalMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.Mark);
                            decimal overallTotalMaxMarks = studentMarks.Where(x => !x.IsOptional).Sum(x => x.MaximumMarks);

                            if (overallTotalMaxMarks > 0)
                            {
                                ObtainedPercent = Math.Round((overallTotalMarks / overallTotalMaxMarks) * 100, 2);
                                ObtainedGrade = GetGrade(ObtainedPercent, studentData.ClassID, termId, BatchId);
                            }

                            string Result = (groupedSubjects != null && groupedSubjects.Any()
                                && !groupedSubjects.Any(g => g.Terms.Any(t => t.Grade == "D" || t.Grade == "E")))
                                ? "Pass" : "";

                            var terms = studentMarks.Select(x => x.TermName).Distinct().ToList();

                            printReportCards.Add(new PrintReportCardData
                            {
                                StudentData = studentData,
                                GroupedSubjects = groupedSubjects,
                                GroupedTerms = groupedTerms,
                                Term = terms,
                                GradingCriteria = gradingCriteria,
                                CoscholasticAreaData = coscholasticDataLookup.ContainsKey(studentId)
                                    ? coscholasticDataLookup[studentId]
                                    : new List<CoscholasticAreaDatas>(),
                                ObtainedPercent = ObtainedPercent,
                                ObtainedGrade = ObtainedGrade,
                                Result = Result,
                            });
                        }
                    }
                }

            }

            return printReportCards;
        }


        private string TryParseDate(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
                return "";

            string[] formats = { "dd/MM/yyyy", "dd-MMM-yyyy", "d-MMM-yyyy", "d/MM/yyyy" };

            if (DateTime.TryParseExact(dateStr, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                return dt.ToString("dd/MM/yyyy");
            }

            // Fallback: try general parse
            if (DateTime.TryParse(dateStr, out dt))
            {
                return dt.ToString("dd/MM/yyyy");
            }

            return "";
        }
        public string Attendance(int BatchId, int ClassId, long StudentID, int? SectionId = 0)
        {
            List<Tbl_StudentAttendance> ActualAttendance = new List<Tbl_StudentAttendance>();

            var batch = _context.Tbl_Batches.Where(x => x.Batch_Id == BatchId).FirstOrDefault();
            //  var batchItems = _context.DataListItems.Where(x => x.DataListId == "9" && x.DataListItemName== batch.Batch_Name).FirstOrDefault();
            var attendanceDate = _context.TblTestAssignDate.Where(x => x.BatchID == BatchId && x.ClassID == ClassId).FirstOrDefault();
            var StartDate = DateTime.Now; var ToDate = DateTime.Now;
            if (attendanceDate == null)
            {
                StartDate = DateTime.Now;
                ToDate = DateTime.Now;
            }
            else
            {
                StartDate = Convert.ToDateTime(attendanceDate.StartDate);
                ToDate = Convert.ToDateTime(attendanceDate.ToDate);
            }
            ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == StudentID && x.Class_Id == ClassId && x.Section_Id == SectionId).ToList().Where(x =>
        DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= StartDate.Date &&
        DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= ToDate.Date).ToList();
            ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == StudentID && x.Class_Id == ClassId && x.Section_Id == SectionId && x.BatchId == BatchId).ToList();



            double attendedDays = 0;
            double attendedHalfDays = 0;
            foreach (var item in ActualAttendance)
            {
                if (item.Mark_FullDayAbsent == "True")
                {
                    attendedDays++;
                }
                if (item.Mark_FullDayAbsent == "True")
                {
                    attendedHalfDays++;
                }
                if (item.Others == "True")
                {
                    attendedDays++;
                }

            }
            //m double totalAttendedDays = attendedDays + (attendedHalfDays / 2);

            int totalAttendedDays = Convert.ToInt32(attendedDays + (attendedHalfDays / 2));
            string Attendance = totalAttendedDays + "/" + ActualAttendance.Count();
            return Attendance;
        }
        [HttpPost]
        public string GetGrade(decimal percentage, int classid, int termid, int BatchId)
        {
            if (termid == 10)
            {
                termid = 4;
            }
            var grade = _context.gradingCriteria.Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage && classid == g.ClassID && termid == g.TermID && BatchId == g.BatchID)
                .Select(g => g.Grade)
                .FirstOrDefault();
            return grade ?? "D";
        }
    }
}
