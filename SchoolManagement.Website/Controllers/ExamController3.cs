using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using log4net;
using Newtonsoft.Json.Linq;
using SchoolManagement.Website.Models;

namespace SchoolManagement.Website.Controllers
{

    public class ExamController3 : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        public decimal Pre1AllTotal { get; private set; }
        public decimal Pre2AllTotal { get; private set; }

        //GradingCriteria
        public ActionResult ExamGrading()
        {
            List<GradingCriteria> gradingCriteriaList = new List<GradingCriteria>();
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            var getBoardName = _context.schoolBoards.Where(x => x.BoardID == getBoardID).FirstOrDefault();

            //gradingCriteriaList = _context.gringCriteria.Where(x => x.BoardID == getBoardID
            var gradingCriteria = (from e in _context.gradingCriteria
                                   join d in _context.tbl_Term on e.TermID equals d.TermID
                                   //  join te in _context.tbl_Tests on e.TestID equals te.TestID
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
                                       // TestName= te.TestName,
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
                Tests = _context.tbl_Tests.ToList();
            }
            else
            {
                var ViewDt = Session["TestAssignData"] as TblTestAssignDate;
                Tests = _context.tbl_Tests.Where(x => x.TestID == ViewDt.TestID).ToList();
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
                //GradingCriteria grade = new GradingCriteria()
                //{
                //    TermID = data.TermID,
                //    BatchID = data.BatchID,
                //    TestID=data.TestID,
                //    ClassID=data.ClassID,
                //    MaximumPercentage=data.MaximumPercentage,
                //    MinimumPercentage=data.MinimumPercentage,
                //    Grade=data.Grade,
                //    GradeDescription=data.GradeDescription,


                //    BoardId = data.BoardId,

                //};
                var Term = _context.tbl_Term.Where(e => e.TermID == data.TermID).ToList();
                // var Tests= _context.tbl_Tests.Where(x => x.TestID == data.//TestID).ToList();
                var Batch = _context.Tbl_Batches.Where(e => e.Batch_Id == data.BatchID).ToList();
                var Classes = _context.DataListItems.Where(e => e.DataListId == "5" && e.DataListItemId == data.ClassID).ToList();
                ViewBag.ClassList = Classes; ViewBag.SectionList = Batch;
                // ViewBag.TestList = Tests;
                ViewBag.Terms = Term;
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(null, JsonRequestBehavior.AllowGet);
                }
                //if (grade != null)
                //{
                // return Json(grade, JsonRequestBehavior.AllowGet);
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

      
        //public ActionResult ExamGrading()
        //{
        //    List<GradingCriteria> gradingCriteriaList = new List<GradingCriteria>();
        //    var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
        //    var getBoardName = _context.schoolBoards.Where(x => x.BoardID == getBoardID).FirstOrDefault();

        //    gradingCriteriaList = _context.gradingCriteria.Where(x => x.BoardID == getBoardID).ToList();

        //    ViewBag.GradingCriteriaList = gradingCriteriaList;
        //    return View();
        //}
        //// POST: GradingCriteria/Create
        //[HttpPost]
        //public ActionResult Create(GradingCriteria model)
        //{
        //    try
        //    {
        //        // Add the new grading criteria to the list
        //        var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
        //        model.BoardID = getBoardID;
        //        _context.gradingCriteria.Add(model);
        //        _context.SaveChanges();
        //        // Redirect to the Index action to display the updated list
        //        return RedirectToAction("ExamGrading");
        //    }
        //    catch (Exception)
        //    {

        //        return RedirectToAction("ExamGrading");
        //    }



        //    // If the model state is not valid, return the Create view with the current model

        //}

        //public JsonResult GetGradingById(int id)
        //{
        //    try
        //    {
        //        var data = _context.gradingCriteria.FirstOrDefault(x => x.CriteriaID == id);
        //        if (data != null)
        //        {
        //            return Json(data, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(null, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

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
                    }
                }



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

        public JsonResult StudentByClassSection(int classId, int sectionId, int testId, int termId, int staffId)
        {
            List<ListStudent> listStudents = new List<ListStudent>();
            List<Tbl_Tests> Tests;

            try
            {

                bool IsClassTeacher = _context.Subjects.Any(x => x.Class_Id == classId && x.StaffId == staffId && x.Section_Id == sectionId && x.Class_Teacher == true);
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
                                .Where(x => x.StaffId == staffId && x.Class_Id == classId)
                                .Select(x => x.Subject_ID)
                                .ToList();

                    var tests = _context.tbl_Tests
                                        .Where(x => staffsubjectids.Contains((int)x.SubjectID) && x.TermID == termId && x.ClassID == classId)
                                        .ToList();

                    Tests = _context.Subjects
                             .Where(x => x.StaffId == staffId && x.Class_Id == classId && x.Section_Id == sectionId)
                             .SelectMany(subject => _context.tbl_Tests.Where(test => test.SubjectID == subject.Subject_ID && test.TermID == termId && test.ClassID == classId))
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
                              .Where(x => SubjectIds.Contains(x.SubjectId) && x.ClassId == classId && x.IsElective == true)
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
                studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).OrderBy(x => x.Name).ToList();
                List<long> fk = new List<long>();
                foreach (var item in studentlist)
                {
                    var studentElectiveSubjectIds = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == item.StudentId).Select(x => x.ElectiveSubjectId).ToList();
                    var IsAlreadyExist1 = _context.Tbl_TestRecord.Where(x => x.ClassID == classId && x.SectionID == sectionId && x.TermID == termId).ToList();
                    var RecordFKID = IsAlreadyExist1.Where(x1 => x1.StudentID == item.StudentId).Select(x2 => x2.RecordID).FirstOrDefault();
                    fk.Add(RecordFKID);
                    var studentObtainedData = _context.tbl_TestObtainedMark.Where(x => x.RecordIDFK == RecordFKID).ToList();

                    List<StudentTestObtMarks> studentTestObtMarksList = new List<StudentTestObtMarks>();

                    foreach (var data in Tests)
                    {
                        if (data.TestID == 0)
                        {
                            var RemarkData = _context.tbl_Remark.Where(x => x.StudentId == item.StudentId && x.TermId == termId).Select(x => x.Remark).FirstOrDefault();
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
                        StudentName = item.Name,
                    };
                    listStudent.studentTestObtMarks = studentTestObtMarksList;
                    listStudents.Add(listStudent);

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


        public JsonResult GetStudentByClassSection(int classId, int sectionId)
        {
            List<ListStudent> listStudents = new List<ListStudent>();
            try
            {
                var studentlist = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId && x.IsApplyforTC == false).Distinct().OrderBy(x => x.Name).ToList();
                foreach (var item in studentlist)
                {
                    item.Section = _context.DataListItems.Where(x => x.DataListItemId == sectionId).Select(x => x.DataListItemName).FirstOrDefault();
                    ListStudent listStudent = new ListStudent()
                    {
                        StudentId = item.StudentId,
                        ClassName = _context.DataListItems.Where(x => x.DataListItemId == item.Class_Id).Select(x => x.DataListItemName).FirstOrDefault(),
                        SectionName = item.Section,
                        StudentName = item.Name,
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
                var existingRecord = _context.Tbl_TestRecord.FirstOrDefault(tr => tr.TermID == item.TermID && tr.StudentID == item.StudentID && tr.ClassID == item.ClassID && tr.SectionID == item.SectionId && tr.BoardID == getBoardID);
                if (existingRecord != null)
                {
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
                            var newRemark = new Tbl_Remark
                            {
                                TermId = item.TermID,
                                StudentId = item.StudentID,
                                BoardId = getBoardID,
                                Remark = item.Remark
                            };

                            _context.tbl_Remark.Add(newRemark);
                        }

                        // Save changes to the context
                        _context.SaveChanges();
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
                        SectionID = item.SectionId
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
        [HttpPost]
        public string covertopdf(string Htmlcontent)
        //public FileStreamResult covertopdf(file fo)
        {
            var result = ExecuteAction(() =>
            {
                var fileViewmodel = new FileViewModel
                {
                    Content = ConvertHtmlToPdf(Htmlcontent),
                    //Content= ConvertHtmlToPdf(fo.cont),
                    Extension = "application/pdf",
                    FileName = "Policy Information.pdf"
                };
                return fileViewmodel;
            }, "covertopdf");
            // return result;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            // Content is the file 
            var fileName = "output.pdf";//@"C:\Users\banso\Pranay\KcK\KcK\SchoolManagement.Website\Rotativa\output.pdf";
            var path = Server.MapPath("~/Rotativa/") + fileName;//@"C:\Users\banso\Pranay\KcK\KcK\SchoolManagement.Website\Rotativa\output.pdf";

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            var stream = new MemoryStream(result.Content);
            using (FileStream fileStream = new FileStream(path, FileMode.Create))
            {
                Byte[] title = stream.ToArray();
                fileStream.Write(title, 0, title.Length);
            }
            return fileName;
        }



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

        public ActionResult PrintReportCard()
        {
            try
            {

                return View();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JsonResult PrintReportCardData(int studentId, int termId) //, int classId
        {
            try
            {
                var studentInfo = _context.Students.Where(x => x.StudentId == studentId).FirstOrDefault();
                var studentRegister = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == studentInfo.ApplicationNumber).FirstOrDefault();
                var familyDetail = _context.FamilyDetails.Where(x => x.StudentRefId == studentRegister.StudentRegisterID).FirstOrDefault();
                var staffId = _context.Subjects.Where(x => x.Class_Id == studentInfo.Class_Id && x.Section_Id == studentInfo.Section_Id && x.Class_Teacher == true).Select(x => x.StaffId).FirstOrDefault();


                List<Tbl_StudentAttendance> ActualAttendance = new List<Tbl_StudentAttendance>();
                if (termId != 10)
                {
                    var batch = _context.Tbl_Batches.Where(x => x.Batch_Id == studentInfo.Batch_Id).FirstOrDefault();
                  //  var batchItems = _context.DataListItems.Where(x => x.DataListId == "9" && x.DataListItemName== batch.Batch_Name).FirstOrDefault();
                    var attendanceDate = _context.TblTestAssignDate.Where(x => x.TestID == termId && x.BatchID == batch.Batch_Id && x.ClassID==studentInfo.Class_Id).FirstOrDefault();
                    var StartDate = DateTime.Now;  var ToDate= DateTime.Now;
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
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == studentInfo.StudentId &&
                x.Class_Id == studentInfo.Class_Id && x.Section_Id == studentInfo.Section_Id).ToList().Where(x =>
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date >= StartDate.Date &&
                DateTime.ParseExact(x.Created_Date, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date <= ToDate.Date).ToList();

                }
                else
                {
                    ActualAttendance = _context.Tbl_StudentAttendance.Where(x => x.StudentRegisterID == studentInfo.StudentId && x.Class_Id == studentInfo.Class_Id && x.Section_Id == studentInfo.Section_Id).ToList();

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
                double totalAttendedDays = attendedDays + (attendedHalfDays / 2);

                StudentReportData studentReportData = new StudentReportData()
                {
                    studentName = studentInfo.Name,
                    fatherName = familyDetail.FatherName,
                    motherName = familyDetail.MotherName,
                    scholarNo = studentInfo.ScholarNo.ToString(),
                    rollNo = studentInfo.RollNo.ToString(),
                    className = _context.DataListItems.Where(x => x.DataListItemId == studentInfo.Class_Id).Select(x => x.DataListItemName).FirstOrDefault(),
                    sectionName = _context.DataListItems.Where(x => x.DataListItemId == studentInfo.Section_Id).Select(x => x.DataListItemName).FirstOrDefault(),
                    dateOfBirth = studentInfo.DOB,
                    academicYear = _context.Tbl_Batches.Where(x => x.Batch_Id == studentInfo.Batch_Id).Select(x => x.Batch_Name).FirstOrDefault(),
                    studentID = studentInfo.StudentId,
                    attandence = totalAttendedDays + "/" + ActualAttendance.Count(),
                    promotedClass = _context.DataListItems.Where(x => x.DataListItemId == studentInfo.Class_Id + 1).Select(x => x.DataListItemName).FirstOrDefault(),
                    staffSignatureLink = _context.StafsDetails.Where(x => x.StafId == staffId).Select(x => x.StaffSignatureFile).FirstOrDefault(),
                    Remark = _context.tbl_Remark.Where(x => x.StudentId == studentInfo.StudentId && (x.TermId == termId || (termId == 10 && x.TermId == 4) || (termId == 7 && x.TermId == 8))).Select(x => x.Remark).FirstOrDefault(),
                    classID = studentInfo.Class_Id
                };
                var AllSubject = (from subj in _context.tbl_ClassSubject
                                  join test in _context.tbl_Tests
                                  on subj.SubjectId equals test.SubjectID
                                  where test.ClassID == studentInfo.Class_Id && subj.ClassId == studentInfo.Class_Id && (termId == 10 || test.TermID == termId) && test.IsOptional == false
                                  select subj).Distinct().ToList();

                
                var electiveSubjectId = AllSubject.Where(x => x.IsElective == true).ToList();
                if (electiveSubjectId.Count > 0)
                {
                    var subjectsToRemove = new List<long>();
                    foreach (var item in electiveSubjectId)
                    {
                        var isAssignedSubject = _context.tbl_Student_ElectiveRecord.Where(x => x.StudentId == studentInfo.StudentId && x.ElectiveSubjectId == item.SubjectId).FirstOrDefault();
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
                Tbl_TestRecords obtainedUT2Marks = null;
                decimal theroyMaxMark = 1; decimal practicalMaxMark = 1; decimal theroyTotalMark = 0;
                decimal practicalTotalMark = 0; decimal UT1MaxMark = 1; decimal UT2MaxMark = 1;
                decimal Term1TheoryMaxMark = 0; decimal Term1PracticalMaxMark = 0; decimal Term2TheoryMaxMark = 0;
                decimal Term2PracticalMaxMark = 0; decimal OptionalUT1MaxMark = 1; decimal OptionalUT2MaxMark = 1; decimal TotalObtainedMarks = 0;

                //Add Pre-1 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre1 = null; Tbl_TestRecords obtainedPracticalMarksPre1 = null;
                decimal Pre1TheoryMaxMark = 0; decimal Pre1PracticalMaxMark = 0;

                //Add Pre-2 By Atul Kumar
                Tbl_TestRecords obtainedTheoryMarksPre2 = null; Tbl_TestRecords obtainedPracticalMarksPre2 = null;
                decimal Pre2TheoryMaxMark = 0; decimal Pre2PracticalMaxMark = 0;
                //changes by Atul Kumar
                var terms=new List<Tbl_Term>();
                if (termId == 10) {
                terms = _context.tbl_Term.ToList();
                }
                else
                {
                    terms = _context.tbl_Term.Where(x=>x.TermID==termId).ToList();
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

                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == studentInfo.Class_Id && x.TermID == termItem.TermID).ToList();

                        if (test.Count > 0)
                        {
                            foreach (var testItem in test)
                            {
                                if (termItem.TermID == 1)//UT1
                                {
                                    var StuentUT1Mark = (from cr in _context.Tbl_TestRecord
                                                         join cog in _context.tbl_TestObtainedMark
                                                         on cr.RecordID equals cog.RecordIDFK
                                                         where cr.StudentID == studentId && cr.ClassID == studentInfo.Class_Id && cr.SectionID == studentInfo.Section_Id && cr.TermID == 1 && cog.TestID == testItem.TestID
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

                                if (termId != 10) {
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

                            subjectData.MarksUT1Grade = GetGrade(PercentageCal(obtainedUT1Marks?.ObtainedMarks ?? 0, OptionalUT1MaxMark));

                            subjectData.TheoryMarks = obtainedTheoryMarksT1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarks = obtainedPracticalMarksT1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksTerm1Practical = Term1PracticalMaxMark;
                            subjectData.MaxMarksTerm1Theory = Term1TheoryMaxMark;
                            subjectData.MaxMarksTerm2Practical = Term2PracticalMaxMark;
                            subjectData.MaxMarksTerm2Theory = Term2TheoryMaxMark;
                            subjectData.TotalObtainedMarks = ((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksT1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT1?.ObtainedMarks ?? 0));
                            var divisor = (Term1PracticalMaxMark + Term1TheoryMaxMark) == 0 ? 1 : (Term1PracticalMaxMark + Term1TheoryMaxMark);
                            subjectData.GradeUT1 = GetGrade((((obtainedTheoryMarksT1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT1?.ObtainedMarks ?? 0)) / (divisor)) * 100);

                            subjectData.MarksUT2 = obtainedUT2Marks?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksUT2 = UT2MaxMark;
                            subjectData.MarksUT2Grade = GetGrade(PercentageCal(obtainedUT2Marks?.ObtainedMarks ?? -1, OptionalUT2MaxMark));
                            subjectData.TotalMarks =
     ((obtainedUT1Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT1Marks?.ObtainedMarks ?? 0)) +
     ((obtainedUT2Marks?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedUT2Marks?.ObtainedMarks ?? 0));

                            subjectData.TheoryMarksUT2 = obtainedTheoryMarksT2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksUT2 = obtainedPracticalMarksT2?.ObtainedMarks ?? -2;
                            subjectData.TotalObtainedMarksUT2 =
    ((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksT2?.ObtainedMarks ?? 0)) +
    ((obtainedPracticalMarksT2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksT2?.ObtainedMarks ?? 0));
                            var divisor1 = (Term2PracticalMaxMark + Term2TheoryMaxMark) == 0 ? 1 : (Term2PracticalMaxMark + Term2TheoryMaxMark);
                            subjectData.GradeUT2 = GetGrade((((obtainedTheoryMarksT2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksT2?.ObtainedMarks ?? 0)) / (divisor1)) * 100);
                            subjectData.TotalMarksBothUTs = TotalObtainedMarks;
                            subjectData.FinalGrade = GetGrade((TotalObtainedMarks / 240) * 100);


                            //megha
                            if (item.SubjectId.ToString() == "45")
                            {
                                //  if (subid.Subject_ID.ToString() == "45")
                                //  {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "45" && x.ClassID == studentInfo.Class_Id).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorviva = Convert.ToDecimal(divisorfinalut1.MaximumMarks) + Convert.ToDecimal(divisorfinalut2.MaximumMarks) + Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks) + Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks) + (divisorfinalterm2theory.MaximumMarks) + Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);

                                subjectData.FinalGrade = GetGrade((TotalObtainedMarks / finaldivisorviva) * 100);

                            }

                            // }


                            ///   var subjectscomp = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID.ToString() == "20").ToList();
                            //  foreach (var subid in subjectscomp)
                            // {
                            // if(subid.Subject_ID.ToString() == "20")
                            if (item.SubjectId.ToString() == "20")
                            {
                                var divisovia = _context.tbl_Tests.Where(x => x.SubjectID.ToString() == "20" && x.ClassID == studentInfo.Class_Id).ToList();


                                var divisorfinalut1 = divisovia.Where(x => x.TermID.ToString() == "1").FirstOrDefault();
                                var divisorfinalut2 = divisovia.Where(x => x.TermID.ToString() == "2").FirstOrDefault();
                                var divisorfinalterm1theory = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm1pract = divisovia.Where(x => x.TermID.ToString() == "3" && x.TestType == "Practical").FirstOrDefault();
                                var divisorfinalterm2theory = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Theory").FirstOrDefault();
                                var divisorfinalterm2pract = divisovia.Where(x => x.TermID.ToString() == "4" && x.TestType == "Practical").FirstOrDefault();

                                decimal finaldivisorviva = Convert.ToDecimal(divisorfinalut1.MaximumMarks) + Convert.ToDecimal(divisorfinalut2.MaximumMarks) + Convert.ToDecimal(divisorfinalterm1theory.MaximumMarks) + Convert.ToDecimal(divisorfinalterm1pract.MaximumMarks) + Convert.ToDecimal(divisorfinalterm2theory.MaximumMarks) + Convert.ToDecimal(divisorfinalterm2pract.MaximumMarks);
                                //subjectData.FinalGrade = GetGrade((TotalObtainedMarks / 440) * 100);

                                subjectData.FinalGrade = GetGrade((TotalObtainedMarks / finaldivisorviva) * 100);
                            }





                            // Calculate Pre-1,Pre-2 Marks By Using Atul Kumar
                            subjectData.TheoryMarksPre1 = obtainedTheoryMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre1 = obtainedPracticalMarksPre1?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre1Practical = Pre1PracticalMaxMark;
                            subjectData.MaxMarksPre1Theory = Pre1TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre1 = ((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre1?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksPre1?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0));

                            var divisorPre = (Pre1PracticalMaxMark + Pre1TheoryMaxMark) == 0 ? 1 : (Pre1PracticalMaxMark + Pre1TheoryMaxMark);
                            subjectData.GradePre1 = GetGrade((((obtainedTheoryMarksPre1?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre1?.ObtainedMarks ?? 0)) / (divisorPre)) * 100);

                            subjectData.TheoryMarksPre2 = obtainedTheoryMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.PracticalMarksPre2 = obtainedPracticalMarksPre2?.ObtainedMarks ?? -2;
                            subjectData.MaxMarksPre2Practical = Pre2PracticalMaxMark;
                            subjectData.MaxMarksPre2Theory = Pre2TheoryMaxMark;
                            subjectData.TotalObtainedMarksPre2 = ((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedTheoryMarksPre2?.ObtainedMarks ?? 0)) + ((obtainedPracticalMarksPre2?.ObtainedMarks ?? 0) == -1 ? 0 : (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0));

                            var divisorPre2 = (Pre2PracticalMaxMark + Pre2TheoryMaxMark) == 0 ? 1 : (Pre2PracticalMaxMark + Pre2TheoryMaxMark);
                            subjectData.GradePre2 = GetGrade((((obtainedTheoryMarksPre2?.ObtainedMarks ?? 0) + (obtainedPracticalMarksPre2?.ObtainedMarks ?? 0)) / (divisorPre2)) * 100);
                        }
                        else
                        {

                            //subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault(); ;
                            //subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            //subjectData.MaxMarksUT1 = UT1MaxMark;
                            //subjectData.MarksUT2 = subjectData.MarksUT2 == -1 ? -1 : subjectData.MarksUT2;
                            //subjectData.MaxMarksUT2 = UT2MaxMark;
                            //subjectData.MarksUT1Grade = subjectData.MarksUT1Grade == "D" ? "D" : subjectData.MarksUT1Grade;
                            //subjectData.MarksUT2Grade = subjectData.MarksUT2Grade == "D" ? "D" : subjectData.MarksUT2Grade;
                            //subjectData.TotalMarks = subjectData.TotalMarks == -1 ? -1 : subjectData.TotalMarks;
                            //subjectData.TheoryMarks = subjectData.TheoryMarks == -1 ? -1 : subjectData.TheoryMarks;
                            //subjectData.PracticalMarks = subjectData.PracticalMarks == -1 ? -1 : subjectData.PracticalMarks;
                            //subjectData.TotalObtainedMarks = subjectData.TotalObtainedMarks == -1 ? -1 : subjectData.TotalObtainedMarks;
                            //subjectData.GradeUT1 = subjectData.GradeUT1 == "D" ? "D" : subjectData.GradeUT1;
                            //subjectData.TheoryMarksUT2 = subjectData.TheoryMarksUT2 == -1 ? -1 : subjectData.TheoryMarksUT2;
                            //subjectData.PracticalMarksUT2 = subjectData.PracticalMarksUT2 == -1 ? -1 : subjectData.PracticalMarksUT2;
                            //subjectData.TotalObtainedMarksUT2 = subjectData.TotalObtainedMarksUT2 == -1 ? -1 : subjectData.TotalObtainedMarksUT2;
                            //subjectData.GradeUT2 = subjectData.GradeUT2 == "D" ? "D" : subjectData.GradeUT2;
                            //subjectData.TotalMarksBothUTs = subjectData.TotalMarksBothUTs == -1 ? -1 : subjectData.TotalMarksBothUTs;
                            //subjectData.FinalGrade = subjectData.FinalGrade == "D" ? "D" : subjectData.FinalGrade;
                            ////Pre1,2 Add By Atul Kumar
                            //subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                            //subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                            //subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;
                            //subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                            //subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                            ////subjectData.GradeUT2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;
                            ///


                            subjectData.Subject = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault(); ;
                            subjectData.MarksUT1 = subjectData.MarksUT1 == -1 ? -1 : subjectData.MarksUT1;
                            subjectData.MaxMarksUT1 = UT1MaxMark;
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
                    Pre2TheoryMaxMark = 0; Pre2PracticalMaxMark = 0;
                }

                //for optional subject
                Tbl_TestRecords NewobtainedTheoryMarksT1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksT2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksT2 = null;
                Tbl_TestRecords NewobtainedUT1Marks = null;
                Tbl_TestRecords NewobtainedUT2Marks = null;

                Tbl_TestRecords NewobtainedTheoryMarksPre1 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre1 = null;
                Tbl_TestRecords NewobtainedTheoryMarksPre2 = null;
                Tbl_TestRecords NewobtainedPracticalMarksPre2 = null;

                decimal NewtheroyMaxMark = 1;
                decimal NewpracticalMaxMark = 1;
                decimal NewtheroyTotalMark = 0;
                decimal NewpracticalTotalMark = 0;
                decimal NewUT1MaxMark = 1;
                decimal NewUT2MaxMark = 1;
                decimal NewTerm1TheoryMaxMark = 1;
                decimal NewTerm1PracticalMaxMark = 1;
                decimal NewTerm2TheoryMaxMark = 1;
                decimal NewTerm2PracticalMaxMark = 1;
                decimal NewOptionalUT1MaxMark = 1;
                decimal NewOptionalUT2MaxMark = 1;
                decimal NewTotalObtainedMarks = 0;

                decimal NewPre1TheoryMaxMark = 1;
                decimal NewPre1PracticalMaxMark = 1;
                decimal NewPre2TheoryMaxMark = 1;
                decimal NewPre2PracticalMaxMark = 1;

                var AllOptionalSubject = (from subj in _context.tbl_ClassSubject
                                          join test in _context.tbl_Tests
                                          on subj.SubjectId equals test.SubjectID
                                          where test.ClassID == studentInfo.Class_Id && subj.ClassId == studentInfo.Class_Id && (termId == 10 || test.TermID == termId) && test.IsOptional == true
                                          select subj).Distinct().ToList();

                List<OptionalSubjectData> optionalsubjectDatas = new List<OptionalSubjectData>();
                foreach (var item in AllOptionalSubject)
                {
                    OptionalSubjectData subjectData = new OptionalSubjectData();
                    foreach (var termItem in terms)
                    {

                        var test = _context.tbl_Tests.Where(x => x.SubjectID == item.SubjectId && x.ClassID == studentInfo.Class_Id && x.TermID == termItem.TermID).ToList();

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
                                // Pre-1,2 Add By Atul kumar
                                if (termId != 10) {
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


                            if (termId != 10) {
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

                            if (termId != 10) {
                                //Pre-1,2 Add By Atul Kumar
                                subjectData.TheoryMarksPre1 = subjectData.TheoryMarksPre1 == -1 ? -1 : subjectData.TheoryMarksPre1;
                                subjectData.PracticalMarksPre1 = subjectData.PracticalMarksPre1 == -1 ? -1 : subjectData.PracticalMarksPre1;
                                subjectData.TotalObtainedMarksPre1 = subjectData.TotalObtainedMarksPre1 == -1 ? -1 : subjectData.TotalObtainedMarksPre1;
                                subjectData.GradePre1 = subjectData.GradePre1 == "D" ? "D" : subjectData.GradePre1;

                                subjectData.TheoryMarksPre2 = subjectData.TheoryMarksPre2 == -1 ? -1 : subjectData.TheoryMarksPre2;
                                subjectData.PracticalMarksPre2 = subjectData.PracticalMarksPre2 == -1 ? -1 : subjectData.PracticalMarksPre2;
                                subjectData.TotalObtainedMarksPre2 = subjectData.TotalObtainedMarksPre2 == -1 ? -1 : subjectData.TotalObtainedMarksPre2;
                                subjectData.GradePre2 = subjectData.GradePre2 == "D" ? "D" : subjectData.GradePre2;

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
                    NewobtainedPracticalMarksPre1 = null;
                    NewobtainedTheoryMarksPre2 = null;
                    NewobtainedPracticalMarksPre2 = null;
                }
                decimal UT1Total = 0; decimal UT1MaxTotal = 0; decimal UT2Total = 0; decimal UT2MaxTotal = 0;
                decimal Term1TheoryMaxTotal = 0; decimal Term1PracticalMaxTotal = 0; decimal Term2TheoryMaxTotal = 0;
                decimal Term2PracticalMaxTotal = 0; decimal UTAllTotal = 0; decimal TheoryTotalT1 = 0;
                decimal PracticalTotalT1 = 0; decimal T1AllTotal = 0; decimal TheoryTotalT2 = 0;
                decimal PracticalTotalT2 = 0; decimal T2AllTotal = 0; decimal OverallAllTotal = 0;

                decimal Pre1TheoryMaxTotal = 0; decimal Pre1PracticalMaxTotal = 0; decimal Pre2TheoryMaxTotal = 0;
                decimal Pre2PracticalMaxTotal = 0; decimal PreAllTotal = 0;

                foreach (var item in subjectDatas)
                {
                    UT1Total += (item.MarksUT1 == -1 || item.MarksUT1 == -2) ? 0 : item.MarksUT1;
                    UT1MaxTotal += (item.MaxMarksUT1 == -1 || item.MaxMarksUT1 == -2) ? 0 : item.MaxMarksUT1;
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


                    if (termId != 10) {
                        Pre1TheoryMaxTotal += (item.MaxMarksPre1Theory == -1 || item.MaxMarksPre1Theory == -2) ? 0 : item.MaxMarksPre1Theory;
                        Pre1PracticalMaxTotal += (item.PracticalMarksPre1 == -1 || item.PracticalMarksPre1 == -2) ? 0 : item.PracticalMarksPre1;
                        Pre1AllTotal += (item.TotalObtainedMarksPre1 == -1 || item.TotalObtainedMarksPre1 == -2) ? 0 : item.TotalObtainedMarksPre1;
                        Pre2TheoryMaxTotal += (item.MaxMarksPre2Theory == -1 || item.MaxMarksPre2Theory == -2) ? 0 : item.MaxMarksPre2Theory;
                        Pre2PracticalMaxTotal += (item.MaxMarksPre2Practical == -1 || item.MaxMarksPre2Practical == -2) ? 0 : item.MaxMarksPre2Practical;
                        Pre2AllTotal += (item.TotalObtainedMarksPre2 == -1 || item.TotalObtainedMarksPre2 == -2) ? 0 : item.TotalObtainedMarksPre2;
                        PreAllTotal += (item.TotalMarks == -1 || item.TotalMarks == -2) ? 0 : item.TotalMarks;
                    }
                   
                }
                var divisor01 = (Term1TheoryMaxTotal + Term1PracticalMaxTotal) == 0 ? 1 : (Term1TheoryMaxTotal + Term1PracticalMaxTotal);
                var divisor02 = (Term2TheoryMaxTotal + Term2PracticalMaxTotal) == 0 ? 1 : (Term2TheoryMaxTotal + Term2PracticalMaxTotal);

                var divisorPre01 = (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal) == 0 ? 1 : (Pre1TheoryMaxTotal + Pre1PracticalMaxTotal);
                var divisorPre02 = (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal) == 0 ? 1 : (Pre2TheoryMaxTotal + Pre2PracticalMaxTotal);

                var T1Gradee = GetGrade(PercentageCal(T1AllTotal, divisor01));
                TotalResult totalResult = new TotalResult()
                {
                    UT1Total = UT1Total,
                    UT1MaxTotal = UT1MaxTotal,
                    UT2Total = UT2Total,
                    UT2MaxTotal = UT2MaxTotal,
                    UT1TotalGrade = GetGrade(PercentageCal(UT1Total, UT1MaxTotal)),
                    UT2TotalGrade = GetGrade(PercentageCal(UT2Total, UT2MaxTotal)),
                    UTAllTotal = UTAllTotal,
                    TheoryTotalT1 = TheoryTotalT1,
                    PracticalTotalT1 = PracticalTotalT1,
                    T1AllTotal = T1AllTotal,
                    T1Grade = GetGrade(PercentageCal(T1AllTotal, divisor01)),
                    TheoryTotalT2 = TheoryTotalT2,
                    PracticalTotalT2 = PracticalTotalT2,
                    T2AllTotal = T2AllTotal,
                    T2Grade = GetGrade(PercentageCal(T2AllTotal, divisor02)),
                    OverallAllTotal = OverallAllTotal,
                    OverallGrade = GetGrade(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal))),

                    Term1TheoryMaxTotal = Term1TheoryMaxTotal,
                    Term1PracticalMaxTotal = Term1PracticalMaxTotal,
                    Term2TheoryMaxTotal = Term2TheoryMaxTotal,
                    Term2PracticalMaxTotal = Term2PracticalMaxTotal,

                   
                };
                TotalResultpercentage totalResultpercentage = new TotalResultpercentage()
                {
                    UT1Total = PercentageCal(UT1Total, UT1MaxTotal),
                    UT1TotalGrade = GetGrade(PercentageCal(UT1Total, UT1MaxTotal)),

                    UT2Total = PercentageCal(UT2Total, UT2MaxTotal),
                    UT2TotalGrade = GetGrade(PercentageCal(UT2Total, UT2MaxTotal)),
                    UTAllTotal = PercentageCal(UTAllTotal, UT1MaxTotal + UT2MaxTotal),
                    TheoryTotalT1 = PercentageCal(TheoryTotalT1, Term1TheoryMaxTotal),
                    PracticalTotalT1 = PercentageCal(PracticalTotalT1, Term1PracticalMaxTotal == 0 ? 1 : Term1PracticalMaxTotal),
                    T1AllTotal = PercentageCal(T1AllTotal, divisor01),
                    T1Grade = GetGrade(PercentageCal(T1AllTotal, divisor01)),
                    TheoryTotalT2 = PercentageCal(TheoryTotalT2, Term2TheoryMaxTotal),
                    PracticalTotalT2 = PercentageCal(PracticalTotalT2, Term2PracticalMaxTotal == 0 ? 1 : Term2PracticalMaxTotal),
                    T2AllTotal = PercentageCal(T2AllTotal, divisor02),
                    T2Grade = GetGrade(PercentageCal(T2AllTotal, divisor02)),
                    OverallAllTotal = PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal)),
                    OverallGrade = GetGrade(PercentageCal(OverallAllTotal, (UT1MaxTotal + UT2MaxTotal + Term1TheoryMaxTotal + Term1PracticalMaxTotal + Term2TheoryMaxTotal + Term2PracticalMaxTotal + Pre1TheoryMaxTotal + Pre1PracticalMaxTotal + Pre2TheoryMaxTotal + Pre2PracticalMaxTotal)))
              

                };
                if (totalResultpercentage.OverallGrade == "0 " || totalResultpercentage.OverallGrade == null)
                {
                    totalResultpercentage. OverallGrade = "D";
                }
                var validGrade = false;
                if (termId != 10) {
                    totalResult.Pre1Grade = GetGrade(PercentageCal(Pre1AllTotal, divisorPre01));
                    totalResult.Pre1AllTotal = Pre1AllTotal;
                    totalResult.Pre2AllTotal = Pre2AllTotal;
                    totalResult.Pre2Grade = GetGrade(PercentageCal(Pre2AllTotal, divisorPre02));
                    totalResult.Pre1TheoryMaxTotal = Pre1AllTotal;
                    totalResult.Pre1PracticalMaxTotal = Pre1PracticalMaxTotal;
                    totalResult.Pre2TheoryMaxTotal = Pre2AllTotal;
                    totalResult.Pre2PracticalMaxTotal = Pre2PracticalMaxTotal;


                    totalResultpercentage.TheoryTotalPre1 = PercentageCal(Pre1AllTotal, Pre1TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre1 = PercentageCal(Pre1PracticalMaxMark, Pre1PracticalMaxTotal == 0 ? 1 : Pre1PracticalMaxTotal);
                    totalResultpercentage.Pre1AllTotal = PercentageCal(Pre1AllTotal, divisorPre01);
                    totalResultpercentage.Pre1Grade = GetGrade(PercentageCal(Pre1AllTotal, divisorPre01));

                    totalResultpercentage.TheoryTotalPre2 = PercentageCal(Pre2AllTotal, Pre2TheoryMaxTotal);
                    totalResultpercentage.PracticalTotalPre2 = PercentageCal(Pre2PracticalMaxMark, Pre2PracticalMaxTotal == 0 ? 1 : Pre2PracticalMaxTotal);
                    totalResultpercentage.Pre2AllTotal = PercentageCal(Pre2AllTotal, divisorPre01);
                   totalResultpercentage.Pre2Grade = GetGrade(PercentageCal(Pre2AllTotal, divisorPre01));


                  
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
              
                if (termId == 7 || termId == 8 || termId==10)
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

                        totalResult.Pre1Grade = totalResult.Pre1Grade == "D" ? "" : totalResult.Pre1Grade;
                        totalResult.Pre2Grade = totalResult.Pre2Grade == "D" ? "" : totalResult.Pre2Grade;
                        totalResultpercentage.Pre1Grade = totalResultpercentage.Pre1Grade == "D" ? "" : totalResultpercentage.Pre1Grade;
                        totalResultpercentage.Pre2Grade = totalResultpercentage.Pre2Grade == "D" ? "" : totalResultpercentage.Pre2Grade;
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
                        subjectData.PracticalMarksPre2 == 0 && subjectData.TotalObtainedMarksPre1 == 0 && subjectData.TotalObtainedMarksPre2 == 0)
                    {
                        // Set the GradeUT1, GradeUT2, and FinalGrade properties to "AB" (Absent)
                        subjectData.GradeUT1 = "D";
                        subjectData.GradeUT2 = "D";
                        subjectData.FinalGrade = "D";
                        subjectData.GradePre1 = "D";
                        subjectData.GradePre2 = "D";
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

                var classCoscholastic = _context.tbl_CoScholasticClass.Where(x => x.ClassID == studentInfo.Class_Id).Select(x => x.CoscholasticID).ToList();
                var CoscholasticMatchingRecords = _context.tbl_CoScholastic
                .Where(record => classCoscholastic.Contains(record.Id))
                .ToList();



                var resultTerm0 = (from cr in _context.tbl_CoScholastic_Result
                                   join cog in _context.tbl_CoScholasticObtainedGrade
                                   on cr.Id equals cog.ObtainedCoScholasticID
                                   join c in _context.tbl_CoScholastic
                                   on cog.CoscholasticID equals c.Id
                                   where cr.StudentID == studentId && cr.TermID == 3
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
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 3)
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
                                   where cr.StudentID == studentId && cr.TermID == 4
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
                                   where cr == null || cog == null || (cr.StudentID == studentId && cr.TermID == 4)
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
                                   where cr.StudentID == studentId && cr.TermID == 3
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
                if (termId != 10) {
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
                                       from term1 in term1Group.DefaultIfEmpty()
                                       from term2 in term2Group.DefaultIfEmpty()
                                       from Pre1 in Pre1Group.DefaultIfEmpty()
                                       from Pre2 in Pre2Group.DefaultIfEmpty()
                                       select new
                                       {
                                           Title = term1?.Title ?? term2?.Title ?? Pre1?.Title ?? Pre2?.Title,
                                           GradeTerm1 = term1?.ObtainedGrade,
                                           GradeTerm2 = term2?.ObtainedGrade,
                                           GradePre1 = Pre1?.ObtainedGrade,
                                           GradePre2 = Pre2?.ObtainedGrade
                                       }).ToList();

                var combinedResult = (from coscholasticId in classCoscholastic
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
                                                      GradePre2 = group.FirstOrDefault(item => item.GradePre2 != null)?.GradePre2
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
                        GradePre2 = item.GradePre2 ?? "-"
                    };
                    coscholasticAreaDatas.Add(coscholasticAreaData);
                }

                studentReportData.coscholasticAreaDatas = coscholasticAreaDatas;
                studentReportData.totalResult = totalResult;
                studentReportData.totalResultPercentage = totalResultpercentage;
                studentReportData.subjectDatas = subjectDatas;
                studentReportData.optionalSubjectDatas = optionalsubjectDatas;
                return Json(studentReportData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex, JsonRequestBehavior.AllowGet);
            }
        }
        public static string GetOptionMarkGrade(decimal gradeNumber)
        {
            if (gradeNumber == -1)
            {
                return "AB";
            }
            if (gradeNumber == -2)
            {
                return "-";
            }
            if (gradeNumber < 1 || gradeNumber > 4)
            {
                return "D";
            }

            char grade = (char)('A' + (int)(gradeNumber - 1));
            return grade.ToString();
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

        //grade Calculate
        public string GetGrade(decimal percentage)
        {
            // Query the database to get the appropriate grade
            var grade = _context.gradingCriteria
                .Where(g => percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage)
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
                    Grade = grade ?? "F"
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
                    }
                }

                return View();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public JsonResult CoScholasticStudentByClassSection(int classId, int sectionId, int termId)
        {
            List<CoScholasticListStudent> listStudents;
            try
            {
                var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
                var IsAlreadyExist = _context.tbl_CoScholastic_Result.Where(x => x.ClassID == classId && x.SectionId == sectionId && x.TermID == termId).ToList();
                if (IsAlreadyExist.Count() > 0)
                {
                    List<Tbl_CoScholastic> coScholastic;
                    CoScholasticStudentGrid(getBoardID, classId, sectionId, out listStudents, out coScholastic, IsAlreadyExist);
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
                    CoScholasticStudentGrid(getBoardID, classId, sectionId, out listStudents, out coScholastic, new List<Tbl_CoScholastic_Result>());
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
        public void CoScholasticStudentGrid(long getBoardID, int classId, int sectionId, out List<CoScholasticListStudent> listStudents, out List<Tbl_CoScholastic> coScholastic, List<Tbl_CoScholastic_Result> IsAlreadyExist)
        {
            listStudents = new List<CoScholasticListStudent>();
            var coScholasticClassList = _context.tbl_CoScholasticClass
                                      .Where(x => x.BoardID == getBoardID && x.ClassID == classId)
                                      .ToList();

            coScholastic = coScholasticClassList.Select(item => new Tbl_CoScholastic
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
                    var existingRecord = _context.tbl_CoScholastic_Result.FirstOrDefault(tr => tr.TermID == item.TermID && tr.StudentID == item.StudentID && tr.BoardID == getBoardID);
                    if (existingRecord != null)
                    {
                        var existingData1 = _context.tbl_CoScholasticObtainedGrade.Where(data => data.ObtainedCoScholasticID == existingRecord.Id).ToList();
                        for (int i = 0; i < item.CoscholasticData.Count; i++)
                        {
                            var Dt = item.CoscholasticData[i];

                            if (existingData1[i] != null)
                            {
                                // Update existing data
                                existingData1[i].CoscholasticID = Dt.CoscholasticID;
                                existingData1[i].ObtainedGrade = Dt.ObtainedGrade;
                                _context.Entry(existingData1[i]).State = EntityState.Modified;
                                _context.tbl_CoScholasticObtainedGrade.Add(Dt);
                                _context.SaveChanges();
                            }
                            else
                            {
                                // Add new CoScholasticData for the existing record
                                Dt.ObtainedCoScholasticID = existingRecord.Id;
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
                var students = _context.Students.Where(x => x.Class_Id == classId && x.Section_Id == sectionId).OrderBy(x => x.Name).ToList();
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
            public List<StudentTestObtMarks> studentTestObtMarks { get; set; }
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
            public decimal PracticalMarks { get; set; }
            public decimal TotalObtainedMarks { get; set; }
            public string GradeUT1 { get; set; }
            public decimal TheoryMarksUT2 { get; set; }
            public decimal PracticalMarksUT2 { get; set; }
            public decimal TotalObtainedMarksUT2 { get; set; }
            public string GradeUT2 { get; set; }
            public decimal TotalMarksBothUTs { get; set; }
            public string FinalGrade { get; set; }
            public decimal MaxMarksTerm1Practical { get; set; }
            public decimal MaxMarksTerm1Theory { get; set; }
            public decimal MaxMarksTerm2Practical { get; set; }
            public decimal MaxMarksTerm2Theory { get; set; }

            public decimal MaxMarksPre1Practical { get; set; }
            public decimal MaxMarksPre1Theory { get; set; }
            public decimal MaxMarksPre2Practical { get; set; }
            public decimal MaxMarksPre2Theory { get; set; }
            public string GradePre1 { get; set; }
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
            public string GradePre1 { get; internal set; }
            public decimal TheoryMarksPre2 { get; internal set; }
            public decimal PracticalMarksPre2 { get; internal set; }
            public decimal TotalObtainedMarksPre2 { get; internal set; }
            public string GradePre2 { get; internal set; }
        }
        //custom studentData
        public class StudentReportData
        {
            public long studentID { get; set; }
            public string studentName { get; set; }
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
            public List<SubjectData> subjectDatas { get; set; }
            public TotalResult totalResult { get; set; }
            public TotalResultpercentage totalResultPercentage { get; set; }
            public List<CoscholasticAreaData> coscholasticAreaDatas { get; set; }
            public List<OptionalSubjectData> optionalSubjectDatas { get; set; }
        }

        //Total
        public class TotalResult
        {
            public decimal UT1Total { get; set; }
            public decimal UT1MaxTotal { get; set; }
            public string UT1TotalGrade { get; set; }
            public decimal UT2Total { get; set; }
            public decimal UT2MaxTotal { get; set; }
            public string UT2TotalGrade { get; set; }
            public decimal UTAllTotal { get; set; }
            public decimal TheoryTotalT1 { get; set; }
            public decimal PracticalTotalT1 { get; set; }
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
            public string Pre2Grade { get; internal set; }
            public decimal Pre1AllTotal { get; internal set; }
            public decimal Pre2AllTotal { get; internal set; }
        }


        public class GradingCriteriaModel
        {

            public long CriteriaID { get; set; }
            public decimal MinimumPercentage { get; set; }
            public decimal MaximumPercentage { get; set; }
            public string Grade { get; set; }
            public string GradeDescription { get; set; }
            public long BoardID { get; set; }
            //  public long TestID { get; set; }
            public long ClassID { get; set; }
            public long BatchID { get; set; }
            public string TestName { get; set; }
            public long TermID { get; set; }
            public string ClassName { get; set; }
            public string BatchName { get; set; }
            public string TermName { get; set; }

            public string TestType { get; set; }
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
            public string T1Grade { get; set; }
            public decimal T1AllTotal { get; set; }
            public decimal TheoryTotalT2 { get; set; }
            public decimal PracticalTotalT2 { get; set; }
            public string T2Grade { get; set; }
            public decimal T2AllTotal { get; set; }
            public decimal OverallAllTotal { get; set; }
            public string OverallGrade { get; set; }
            public decimal TheoryTotalPre1 { get; internal set; }
            public decimal PracticalTotalPre1 { get; internal set; }
            public decimal Pre1AllTotal { get; internal set; }
            public string Pre1Grade { get; internal set; }
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
            public List<Tbl_TestObtainedMark> ObtainedMarkData { get; set; }
        }

        public class CoscholasticAreaData
        {
            public string Name { get; set; }
            public string GradeTerm1 { get; set; }
            public string GradeTerm2 { get; set; }
            public string GradePre1 { get; internal set; }
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
                Batch = _context.Tbl_Batches.Where(e =>  e.Batch_Id == ViewDt.BatchID).ToList();
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


    }
}