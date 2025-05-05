using EmployeeManagement.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Ajax.Utilities;
using Rotativa;
using SchoolManagement.Data.Constants;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.EnumData;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc; 


namespace SchoolManagement.Website.Controllers
{
    [Authorize]
    public class AdmissionFeeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        //private IRepository<Accounts> _accountsRepository = null;
        private IRepository<Frequencys> _frequencyRepository = null;
        //private IRepository<FeeHeadingGroups> _feeHeadingGroupsRepository = null;
        private IRepository<FeeHeadings> _FeeHeadingsRepository = null;
        private IRepository<Classes> _ClassesRepository = null;
        private IRepository<StudentCategorys> _StudentCategorysRepository = null;
        private IRepository<FeePlans> _FeePlansRepository = null;
        private IRepository<Student> _StudentRepository = null;
        private IRepository<TblFeeReceipts> _TblFeeReceiptsRepository = null;
        private IRepository<TblDueFee> _TblDueFeeRepository = null;
        // private IRepository<TblFeeReceiptsAudit> _TblFeeReceiptsRepositoryAudit = null;

        public AdmissionFeeController()
        {
            //_accountsRepository = new Repository<Accounts>();
            _frequencyRepository = new Repository<Frequencys>();
            //_feeHeadingGroupsRepository = new Repository<FeeHeadingGroups>();
            _FeeHeadingsRepository = new Repository<FeeHeadings>();
            _ClassesRepository = new Repository<Classes>();
            _StudentCategorysRepository = new Repository<StudentCategorys>();
            _FeePlansRepository = new Repository<FeePlans>();
            _StudentRepository = new Repository<Student>();
            _TblFeeReceiptsRepository = new Repository<TblFeeReceipts>();
            _TblDueFeeRepository = new Repository<TblDueFee>();

        }

        // GET: FeeConfiguration
        public ActionResult FeeHeadingsList()
        {
            //ViewBag.Accounts = new SelectList(_accountsRepository.GetAll().OrderBy(x => x.AccountName).ToList(), "AccountId", "AccountName");
            ViewBag.Frequencys = new SelectList(_frequencyRepository.GetAll().OrderBy(x => x.FeeFrequencyName).ToList(), "FeeFrequencyId", "FeeFrequencyName");
            //ViewBag.FeeHeadingGroups = new SelectList(_feeHeadingGroupsRepository.GetAll().OrderBy(x => x.FeeHeadingGroupName).ToList(), "FeeHeadingGroupId", "FeeHeadingGroupName");
            List<FeeHeadings> feeHeadingsList = _context.FeeHeadings.Where(x => x.FeeType_Id == 222).ToList();

            var pagename = "Fee Headings List";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;
            return View(feeHeadingsList);
        }

        public ActionResult AdmissionFeeHeadingsList()
        {
            //ViewBag.Accounts = new SelectList(_accountsRepository.GetAll().OrderBy(x => x.AccountName).ToList(), "AccountId", "AccountName");
            ViewBag.Frequencys = new SelectList(_frequencyRepository.GetAll().OrderBy(x => x.FeeFrequencyName).ToList(), "FeeFrequencyId", "FeeFrequencyName");
            //ViewBag.FeeHeadingGroups = new SelectList(_feeHeadingGroupsRepository.GetAll().OrderBy(x => x.FeeHeadingGroupName).ToList(), "FeeHeadingGroupId", "FeeHeadingGroupName");
            List<FeeHeadings> feeHeadingsList = _context.FeeHeadings.Where(x => x.FeeType_Id == 243).ToList();

            var pagename = "Fee Headings List";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;
            return View(feeHeadingsList);
        }


        public ActionResult ConfigureFeePlanes()
        {
            //List<FeePlans> allFeePlanes = _FeePlansRepository.GetAll().ToList();
            var allFeePlanes = _context.FeePlans.Where(x => x.FeeType_Id == 222).ToList();
            //List<FeePlans> allFeePlanes = new List<FeePlans>();
            //foreach(var item in data)
            //{
            //    allFeePlanes.Add(new FeePlans{
            //        ClassName = 
            //    });
            //}
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();

            ViewBag.FeeHeadings = new SelectList(_FeeHeadingsRepository.GetAll().ToList(), "FeeId", "FeeName");
            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();

            ViewBag.Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            //ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var batches = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true).ToList();


            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");


            ViewBag.Categories = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).ToList();


            var pagename = "Configure Fee Planes";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;

            return View(allFeePlanes);
        }


        public ActionResult AdmissionFeeConfigureFeePlanes()
        {
            //List<FeePlans> allFeePlanes = _FeePlansRepository.GetAll().ToList();
            var allFeePlanes = _context.FeePlans.Where(x => x.FeeType_Id == 243).ToList();
            //List<FeePlans> allFeePlanes = new List<FeePlans>();
            //foreach(var item in data)
            //{
            //    allFeePlanes.Add(new FeePlans{
            //        ClassName = 
            //    });
            //}
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();


            var feeheadings = _context.FeeHeadings.Where(x => x.FeeType_Id == 243).ToList();

            ViewBag.FeeHeadings = new SelectList(feeheadings, "FeeId", "FeeName");

            ViewBag.Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            //ViewBag.BatcheNames = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();

            ViewBag.Categories = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            var batches = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");
            ViewBag.Medium = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "medium").DataListId.ToString()).ToList();

            var pagename = "Configure Fee Planes";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;

            return View(allFeePlanes);
        }

        public ActionResult FeeReceipts()
        {
            //var studentlist = _context.StudentsRegistrations
            //.Where(w => w.IsApprove != 192).ToList();

            //var studentlist = (from sr in _context.StudentsRegistrations
            //                  join fp in _context.FeePlans on new { a = sr.Class_Id, sr.Medium } equals new { a = fp.ClassId, fp.Medium }
            //                  where sr.IsApprove != 192
            //                  select sr).DistinctBy(a=>a.StudentRegisterID).ToList();

            var studentlist = (from a in _context.Students
                               join fp in _context.FeePlans on new { a = a.Class_Id, a.Medium } equals new { a = fp.ClassId, fp.Medium }
                               where a.IsApprove != 192
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

            var pagename = "Fee Receipts";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;

            //check active school
            var branchdata = _context.Tbl_CreateBranch.ToList();
            var schoolsetup = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
            Tbl_CreateBranch tbl_CreateBranch = new Tbl_CreateBranch();
            if (schoolsetup != null)
            {
                tbl_CreateBranch = branchdata.FirstOrDefault(x => x.Bank_Id == schoolsetup.Bank_Id && x.Branch_ID == schoolsetup.Branch_Id);
            }

            ViewBag.TblBranch = tbl_CreateBranch.Branch_Name;

            return View();
        }

        //AdmissionFee
        public ActionResult AdmissionFeeReceipts()
        {
            //

            var batch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).FirstOrDefault();
            int batchId = 0;
            if (batch != null)
                batchId = batch.Batch_Id;

            //var studentlist = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).OrderBy(x => x.Name).ToList();
            var studentlist = _context.Students.Where(x => x.IsApprove == 191 && x.Batch_Id == batchId).OrderBy(x => x.Name).ToList();
            List<StudentsRegistration> studentsRegistrations = new List<StudentsRegistration>();
            foreach (var item in studentlist)
            {
                studentsRegistrations.Add(new StudentsRegistration
                {
                    StudentRegisterID = item.StudentId,
                    Name = item.Name + " " + item.Last_Name,
                });
            }

            ViewBag.StudentNames = new SelectList((studentsRegistrations), "StudentRegist" +
                "erID", "Name");

            var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Classes = new SelectList((Classes).ToList(), "DataListItemId", "DataListItemName");

            var pagename = "Fee Receipts";
            var editpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, editpermission);
            ViewBag.Permission = per;

            //check active school
            var branchdata = _context.Tbl_CreateBranch.ToList();
            var schoolsetup = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
            Tbl_CreateBranch tbl_CreateBranch = new Tbl_CreateBranch();
            if (schoolsetup != null)
            {
                tbl_CreateBranch = branchdata.FirstOrDefault(x => x.Bank_Id == schoolsetup.Bank_Id && x.Branch_ID == schoolsetup.Branch_Id);
            }

            ViewBag.TblBranch = tbl_CreateBranch.Branch_Name;

            return View();
        }

        public JsonResult GetStudentByClass(int classid, int Filtertype)
        {
            //var studentlist = _context.StudentsRegistrations
            //    .Where(w => w.Class_Id == classid && w.IsApprove != 192)
            //    .ToList();

            //---x-rnik
            //var lstStudents = _context.Students.Where(x => x.Class_Id == classid).ToList();
            var studentlist1 = (from sr in _context.Students
                                join fp in _context.FeePlans on new { a = sr.Class_Id, sr.Medium } equals new { a = fp.ClassId, fp.Medium }
                               where sr.Class_Id == classid && sr.IsApprove != 192
                               select sr).DistinctBy(a => a.StudentId).ToList();

            //---

            //var studentlist = (from sr in _context.StudentsRegistrations
            //                   join fp in _context.FeePlans on new { a = sr.Class_Id, sr.Medium } equals new { a = fp.ClassId, fp.Medium }
            //                   where sr.Class_Id == classid && sr.IsApprove != 192
            //                   select sr).DistinctBy(a => a.StudentRegisterID).ToList();

            var dataListId = _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString();
            var Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();

            var Classes = _context.DataListItems.Where(x => x.DataListId == dataListId).ToList();

            var data = new List<Student>();

            //foreach (var fe in studentlist)
            //{

            //    if (Filtertype != 1)
            //    {
            //        var exist = _context.AdditionalInformations
            //       .Where(w => w.DistancefromSchool != 0
            //                  && w.TransportFacility == "yes"
            //                  && w.StudentRefId == fe.StudentRegisterID).Any();

            //        if (!exist)
            //            continue;

            //    }

            //    fe.Class_Name = Classes.Where(w => w.DataListItemId == fe.Class_Id)
            //    .Select(s => s.DataListItemName).FirstOrDefault();

            //    fe.Section_Name = Section.Where(w => w.DataListItemId == fe.Section_Id)
            //   .Select(s => s.DataListItemName).FirstOrDefault();

            //    data.Add(fe);
            //}

            //---x-rnik---
            foreach (var fe in studentlist1)
            {
                // var stdReg = _context.StudentsRegistrations.Where(x => x.ApplicationNumber == fe.ApplicationNumber).FirstOrDefault();
                if (Filtertype != 1)
                {
                    var exist = _context.AdditionalInformations
                   .Where(w => w.DistancefromSchool != 0
                              && w.TransportFacility == "yes"
                              && w.StudentRefId == fe.StudentId).Any();

                    if (!exist)
                        continue;

                }
                fe.Class = Classes.Where(w => w.DataListItemId == fe.Class_Id)
               .Select(s => s.DataListItemName).FirstOrDefault();

                fe.Section = Section.Where(w => w.DataListItemId == fe.Section_Id)
               .Select(s => s.DataListItemName).FirstOrDefault();

                data.Add(fe);
            }
            //---

            if (data.Count > 0)
                return Json(data, JsonRequestBehavior.AllowGet);

            return Json("Fail", JsonRequestBehavior.AllowGet);

        }


        public async Task<ActionResult> TcBillReceipt()
        {
            List<StudentTcRceiptViewModel> stdTcReciept = new List<StudentTcRceiptViewModel>();
            try
            {
                ViewBag.Classes = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToListAsync();
                ViewBag.Categorys = await _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToListAsync();
                ViewBag.BatcheNames = await _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToListAsync();

                //stdTcReciept = await _context.TcFeeDetails.Include(x => x.studentFeeDetails)
                //                                      .Include(x => x.student)
                //                                      .Select(x => new StudentTcRceiptViewModel {
                //                                          Id=x.Id,
                //                                          Studnt=x.student.Name,
                //                                          Batch=x.student.BatchName,
                //                                          Category=x.student.Category,
                //                                          Class=x.student.Class,
                //                                          paymode=x.PaymentMode,
                //                                          IsTcfree=x.IsTcfee,
                //                                          Amount=x.ReceiptAmount
                //                                      }).ToListAsync();

            }
            catch (Exception ex)
            {

                throw;
            }

            return View(stdTcReciept);
        }

        public async Task<ActionResult> TcBillPreview(long id)
        {
            var allFeeREceipts = await _context.TcFeeDetails.Include(x => x.studentFeeDetails)
                                                      .Include(x => x.student).Where(x => x.Id == id)
                                                      .Select(x => new StudentTcRceiptViewModel
                                                      {
                                                          ScollarNumber = x.StudentId.ToString(),
                                                          Id = x.Id,
                                                          Studnt = x.student.Name,
                                                          Batch = x.student.BatchName,
                                                          Category = x.student.Category,
                                                          Class = x.student.Class,
                                                          paymode = x.PaymentMode,
                                                          createdon = x.PaidDate,
                                                          Amount = x.ReceiptAmount,
                                                          IsTcfree = x.IsTcfee
                                                      }).FirstOrDefaultAsync();

            return View(allFeeREceipts);
        }

        //public async Task<ActionResult> GetStudentTcRecieptList()
        //{

        //}

        public ActionResult AllReceipts()
        {
            //ViewBag.Students = new SelectList(_StudentRepository.GetAll().OrderBy(x => x.Name).ToList(),"StudentId","Name");
            //var currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            //ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //ViewBag.Categorys = new SelectList(_StudentCategorysRepository.GetAll().OrderBy(x => x.CategoryName).ToList(), "CategoryName", "CategoryName");
            //var allFeeREceipts = _TblFeeReceiptsRepository.GetAll().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());
            //List<TblFeeReceipts> allFeeREceipts = new List<TblFeeReceipts>();
            //int studentid = Convert.ToInt32(Session["StudentId"]);

            var pagename = "All Receipts";
            var editpermission = "Delete_Permission";

            if (Session["rolename"].ToString() == "Student")
            {
                int studentid = Convert.ToInt32(Session["StudentId"]);

                //var tblstudent = _context.Students.ToList();
                var tblstudentregistrations = _context.Students.FirstOrDefault(x => x.StudentId == studentid);
                int applicationid;
                //var tblstudentdata = tblstudent.FirstOrDefault(x => x.StudentId == studentid);
                //if(tblstudentdata != null)
                //{
                //     applicationid = tblstudentdata.StudentId;
                //}
                //else
                {
                    applicationid = Convert.ToInt32(tblstudentregistrations.StudentId);
                }

                List<Tbl_Feereceiptsviewmodel> tbl_Feereceiptsviewmodels = new List<Tbl_Feereceiptsviewmodel>();

                var AllFeeREceipts = _context.TblFeeReceipts.Where(x => x.StudentId == applicationid && x.FeeHeadingIDs != "21").ToList();

                foreach (var item in AllFeeREceipts)
                {
                    tbl_Feereceiptsviewmodels.Add(new Tbl_Feereceiptsviewmodel
                    {
                        StudentName = item.StudentName,
                        ClassName = item.ClassName,
                        PayHeadings = item.PayHeadings,
                        PaymentMode = item.PaymentMode,
                        TotalFee = item.TotalFee,
                        PaidAmount = item.PaidAmount,
                        AddedDate = item.AddedDate,
                        FeeReceiptId = item.FeeReceiptId,
                        FeeReceiptsOneTimeCreator = item.FeeReceiptsOneTimeCreator,
                        DeletePermission = CheckDeletepermission(pagename, editpermission)

                    });
                }

                ViewBag.sessionlist = "Student";

                //AllFeeREceipts = AllFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                //foreach (var item in AllFeeREceipts)
                //{
                //    string monthNames = string.Empty;
                //    if (Session["StudentId"] != null && Session["StudentId"].ToString().Trim() != "")
                //    {
                //        var rs = Session["StudentId"];
                //        int studid = Convert.ToInt32(Session["StudentId"]);
                //        var allFeeReceiptForOneTimeCreator = _TblFeeReceiptsRepository.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator && x.StudentId == studid).ToList();
                //        foreach (var item2 in allFeeReceiptForOneTimeCreator)
                //        {
                //            monthNames = monthNames + item2.PaidMonths;
                //            if (allFeeReceiptForOneTimeCreator.Count() > 1)
                //            {
                //                monthNames = monthNames + " | ";
                //            }
                //        }
                //        item.PaidMonths = monthNames;
                //    }

                //}           var   allFeeREceipts = _contextTblFeeReceipts.ToList();
                //allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();

                return View(tbl_Feereceiptsviewmodels.ToList());

                //var   allFeeREceipts = _context.TblFeeReceipts.ToList();
                //allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();

            }
            else
            {
                List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
                List<Tbl_Feereceiptsviewmodel> tbl_Feereceiptsviewmodels = new List<Tbl_Feereceiptsviewmodel>();
                var feelist = _context.TblFeeReceipts.Where(x => x.FeeHeadingIDs != null).ToList().OrderByDescending(x=>x.AddedDate);
                var studentlist = _context.Students.Where(x => x.IsApprove != 192).ToList();
                List<TblFeeReceipts> transportfee = new List<TblFeeReceipts>();

                foreach (var item in feelist)
                {
                    if (item.FeeHeadingIDs == "21" || item.FeeHeadingIDs == "25" || item.FeeHeadingIDs == "21,25" || item.FeeHeadingIDs == "25,21")
                    {
                        transportfee.Add(item);
                    }
                    else
                    {
                        var studentdata = studentlist.FirstOrDefault(x => x.StudentId == item.StudentId);
                        if (studentdata != null)
                        {
                            item.StudentName = studentdata.Name + " " + studentdata.Last_Name;
                            //tblFeeReceipts.Add(item);
                            tbl_Feereceiptsviewmodels.Add(new Tbl_Feereceiptsviewmodel
                            {
                                StudentName = item.StudentName,
                                ClassName = item.ClassName,
                                PayHeadings = item.PayHeadings,
                                PaymentMode = item.PaymentMode,
                                TotalFee = item.TotalFee,
                                PaidAmount = item.PaidAmount,
                                AddedDate = item.AddedDate,
                                FeeReceiptId = item.FeeReceiptId,
                                FeeReceiptsOneTimeCreator = item.FeeReceiptsOneTimeCreator,
                                DeletePermission = CheckDeletepermission(pagename, editpermission)

                            });

                        }
                    }
                }

                //var allFeeREceipts = _context.TblFeeReceipts.Where(x => x.FeeHeadingIDs != "21").ToList();
                //allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                //foreach (var item in allFeeREceipts)
                //{
                //    string monthNames = string.Empty;

                //    var allFeeReceiptForOneTimeCreator = _TblFeeReceiptsRepository.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator).ToList();
                //    foreach (var item2 in allFeeReceiptForOneTimeCreator)
                //    {
                //        monthNames = monthNames + item2.PaidMonths;
                //        if (allFeeReceiptForOneTimeCreator.Count() > 1)
                //        {
                //            monthNames = monthNames + " | ";
                //        }
                //    }
                //    item.PaidMonths = monthNames;



                //}
                ViewBag.sessionlist = "Professor";

                return View(tbl_Feereceiptsviewmodels);

            }


        }

        public ActionResult UserFeeReport()
        {

            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Classes = new SelectList((Class).ToList(), "DataListItemId", "DataListItemName");
            ViewBag.Classes = Class;

            var batch = _context.Tbl_Batches.ToList();
            ViewBag.Batches = new SelectList((batch).ToList(), "Batch_Id", "Batch_Name");
            ViewBag.Batches = batch;

            var feetype = _context.FeeHeadings.ToList();
            ViewBag.Feetype = new SelectList((feetype).ToList(), "FeeId", "FeeName");
            ViewBag.Feetype = feetype;

            return View();
        }

        [HttpPost]
        public JsonResult GetUserFeeReport(Tbl_Feereceiptsviewmodel feereceiptsviewmodel)
        {
            List<Tbl_Feereceiptsviewmodel> studentTotalFeeViewModels = new List<Tbl_Feereceiptsviewmodel>();

            //  List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
            List<TblFeeReceipts> Admissionfeereceipts = new List<TblFeeReceipts>();

            return Json(Admissionfeereceipts, JsonRequestBehavior.AllowGet);
            //var  tblFeeReceipts = (_context.TblFeeReceipts.Join(_context.StudentsRegistrations,
            //      feereceipt=> feereceipt.StudentId,
            //      studentreg=> studentreg.StudentRegisterID,
            //      (feereceipt, studentreg) => new { feereceipt , studentreg }).
            //      Join(_context.FeeHeadings,
            //      feereceipt1 => (feereceipt1.feereceipt.FeeId as string),
            //      feeheading => feeheading.FeeHeadingIDs,
            //      (feereceipt1, feeheading) => new { feeid = feereceipt1.feereceipt.FeeId, feehead = feeheading.FeeHeadingIDs})).sele
            //      ToList();

            //  var tblfee = from t in _context.TblFeeReceipts
            //               join s in _context.StudentsRegistrations on t.StudentId equals s.StudentRegisterID
            //               join f in _context.FeeHeadings on new { feeid = t.FeeId.ToString() } equals new
            //               {
            //                   feeid = f.FeeHeadingIDs
            //               }

            //var studentregistrations = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();


            //if (feereceiptsviewmodel.DateFrom != "" && feereceiptsviewmodel.DateFrom != null && feereceiptsviewmodel.DateTo != "" && feereceiptsviewmodel.DateTo != null)
            //{
            //    //tblFeeReceipts = tblFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/mm/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(studentFeeInputModel.DateFrom, "dd/mm/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/mm/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(studentFeeInputModel.DateTo, "dd/mm/yyyy", CultureInfo.InvariantCulture)).ToList();
            //    DateTime fromdt = DateTime.Now; DateTime todt = DateTime.Now;

            //    //fromdt = DateTime.ParseExact(studentFeeInputModel.DateFrom, "yyyy-MM-dd hh:mm:ss", null);

            //    fromdt = DateTime.ParseExact(feereceiptsviewmodel.DateFrom, "d/M/yyyy", CultureInfo.InvariantCulture);

            //    todt = DateTime.ParseExact(feereceiptsviewmodel.DateTo, "d/M/yyyy", CultureInfo.InvariantCulture);

            //    tblFeeReceipts = tblFeeReceipts.Where(x => x.AddedDate >= fromdt && x.AddedDate <=
            //    todt).ToList();

            //    //studentregistrations = studentregistrations.Where(x=> tblFeeReceipts.Contains(Convert.ToInt16(x.StudentRegisterID))
            //}


            //if (!string.IsNullOrEmpty(studentFeeInputModel.ScholarNumer) && studentFeeInputModel.ScholarNumer != "0")
            //{
            //    int studentid = Convert.ToInt32(studentFeeInputModel.ScholarNumer);
            //    tblFeeReceipts = tblFeeReceipts.Where(x => x.StudentId == studentid).ToList();
            //    var studentname = studentregistrations.FirstOrDefault(x => x.StudentRegisterID == studentid);
            //    foreach (var item in tblFeeReceipts)
            //    {
            //        if (item.FeeHeadingIDs != null && studentname != null)
            //        {
            //            if (item.FeeHeadingIDs == "19")
            //            {
            //                item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
            //                Admissionfeereceipts.Add(item);
            //            }
            //            else
            //            {
            //                var feeud = item.FeeHeadingIDs.Split(',');
            //                foreach (var items in feeud)
            //                {
            //                    if (items == "19")
            //                    {
            //                        item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
            //                        Admissionfeereceipts.Add(item);
            //                    }
            //                }
            //            }
            //        }

            //    }

            //}

            //if (!string.IsNullOrEmpty(studentFeeInputModel.Semester))
            //{
            //    int claassid = Convert.ToInt32(studentFeeInputModel.Semester);
            //    tblFeeReceipts = tblFeeReceipts.Where(x => x.ClassId == claassid).ToList();

            //    foreach (var item in tblFeeReceipts)
            //    {
            //        if (item.FeeHeadingIDs != null)
            //        {
            //            int studentid = Convert.ToInt32(item.StudentId);
            //            var studentname = studentregistrations.FirstOrDefault(x => x.StudentRegisterID == studentid);
            //            if (studentname != null)
            //            {
            //                if (item.FeeHeadingIDs == "19")
            //                {
            //                    item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
            //                    Admissionfeereceipts.Add(item);
            //                }
            //                else
            //                {
            //                    var feeud = item.FeeHeadingIDs.Split(',');
            //                    var feevalie = item.FeePaids.Split(',');
            //                    foreach (var items in feeud)
            //                    {
            //                        int i = 0;
            //                        if (items == "19")
            //                        {
            //                            item.PaidAmount = feevalie[i];
            //                            item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
            //                            Admissionfeereceipts.Add(item);
            //                        }
            //                        i++;
            //                    }
            //                }
            //            }

            //        }

            //    }
            //}

            //  return Json(Admissionfeereceipts, JsonRequestBehavior.AllowGet);
        }
        //Admission Fee Receipt Page
        public ActionResult AdmissionFeeReports()
        {

            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Classes = new SelectList((Class).ToList(), "DataListItemId", "DataListItemName");
            ViewBag.Classes = Class;

            return View();
        }


        [HttpPost]
        public JsonResult GetStudentsAdmissionFeeDetails(StudentFeeInputModel studentFeeInputModel)
        {
            List<StudentTotalFeeViewModel> studentTotalFeeViewModels = new List<StudentTotalFeeViewModel>();

            List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
            List<TblFeeReceipts> Admissionfeereceipts = new List<TblFeeReceipts>();

            tblFeeReceipts = _context.TblFeeReceipts.ToList();
            var studentregistrations = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();


            if (studentFeeInputModel.DateFrom != "" && studentFeeInputModel.DateFrom != null && studentFeeInputModel.DateTo != "" && studentFeeInputModel.DateTo != null)
            {
                //tblFeeReceipts = tblFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/mm/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(studentFeeInputModel.DateFrom, "dd/mm/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/mm/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(studentFeeInputModel.DateTo, "dd/mm/yyyy", CultureInfo.InvariantCulture)).ToList();
                DateTime fromdt = DateTime.Now; DateTime todt = DateTime.Now;

                //fromdt = DateTime.ParseExact(studentFeeInputModel.DateFrom, "yyyy-MM-dd hh:mm:ss", null);

                fromdt = DateTime.ParseExact(studentFeeInputModel.DateFrom, "d/M/yyyy", CultureInfo.InvariantCulture);

                todt = DateTime.ParseExact(studentFeeInputModel.DateTo, "d/M/yyyy", CultureInfo.InvariantCulture);

                tblFeeReceipts = tblFeeReceipts.Where(x => x.AddedDate >= fromdt && x.AddedDate <=
                todt).ToList();

                //studentregistrations = studentregistrations.Where(x=> tblFeeReceipts.Contains(Convert.ToInt16(x.StudentRegisterID))
            }


            if (!string.IsNullOrEmpty(studentFeeInputModel.ScholarNumer) && studentFeeInputModel.ScholarNumer != "0")
            {
                int studentid = Convert.ToInt32(studentFeeInputModel.ScholarNumer);
                tblFeeReceipts = tblFeeReceipts.Where(x => x.StudentId == studentid).ToList();
                var studentname = studentregistrations.FirstOrDefault(x => x.StudentRegisterID == studentid);
                foreach (var item in tblFeeReceipts)
                {
                    if (item.FeeHeadingIDs != null && studentname != null)
                    {
                        if (item.FeeHeadingIDs == "19")
                        {
                            item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
                            Admissionfeereceipts.Add(item);
                        }
                        else
                        {
                            var feeud = item.FeeHeadingIDs.Split(',');
                            foreach (var items in feeud)
                            {
                                if (items == "19")
                                {
                                    item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
                                    Admissionfeereceipts.Add(item);
                                }
                            }
                        }
                    }

                }

            }

            if (!string.IsNullOrEmpty(studentFeeInputModel.Semester))
            {
                int claassid = Convert.ToInt32(studentFeeInputModel.Semester);
                tblFeeReceipts = tblFeeReceipts.Where(x => x.ClassId == claassid).ToList();

                foreach (var item in tblFeeReceipts)
                {
                    if (item.FeeHeadingIDs != null)
                    {
                        int studentid = Convert.ToInt32(item.StudentId);
                        var studentname = studentregistrations.FirstOrDefault(x => x.StudentRegisterID == studentid);
                        if (studentname != null)
                        {
                            if (item.FeeHeadingIDs == "19")
                            {
                                item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
                                Admissionfeereceipts.Add(item);
                            }
                            else
                            {
                                var feeud = item.FeeHeadingIDs.Split(',');
                                var feevalie = item.FeePaids.Split(',');
                                foreach (var items in feeud)
                                {
                                    int i = 0;
                                    if (items == "19")
                                    {
                                        item.PaidAmount = feevalie[i];
                                        item.StudentName = studentname == null ? "" : studentname.Name + "" + studentname.Last_Name;
                                        Admissionfeereceipts.Add(item);
                                    }
                                    i++;
                                }
                            }
                        }

                    }

                }
            }

            return Json(Admissionfeereceipts, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ReceiptPreview(int? id, int ReceiptId)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            Student student = new Student();
            var session = Session["rolename"].ToString();
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.session = session;
            }
            if (id != null)
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                //tblFeeReceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == id);
                int? studentId = tblFeeReceipts.StudentId;

                
     
               
                //var tblstudentid = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                //if(tblstudentid != null)
                //{
                //    ViewBag.ScollarNumber = _context.Students.FirstOrDefault(x => x.StudentId == studentId).StudentId;

                //    var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                //    ViewBag.studentDetails = studentDetails;
                //    //ViewBag.Total = tblFeeReceipts.TotalFee - tblFeeReceipts.ConcessionAmt + tblFeeReceipts.LateFee + tblFeeReceipts.OldBalance;
                //    ViewBag.Total = tblFeeReceipts.ReceiptAmt;

                //    var studentregistration = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentDetails.ApplicationNumber);
                //    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.StudentRefId == studentregistration.StudentRegisterID);
                //    ViewBag.ContactNo = familydetails.FMobile;
                //}
                //else
                {
                    ViewBag.ScollarNumber = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId).StudentRegisterID;
                    ViewBag.ClassName = _context.StudentsRegistrations.FirstOrDefault();
                    //ViewBag.Section = _context.StudentsRegistrations.FirstOrDefault();
                    //ViewBag.Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                    ViewBag.SectionName = _context.DataListItems.Where(x => x.DataListItemId == student.Section_Id).Select(x => x.DataListItemName.ToLower() == "section").FirstOrDefault();
                    var studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                    ViewBag.studentDetails = studentDetails;
                    //ViewBag.Total = tblFeeReceipts.TotalFee - tblFeeReceipts.ConcessionAmt + tblFeeReceipts.LateFee + tblFeeReceipts.OldBalance;
                    ViewBag.Total = tblFeeReceipts.ReceiptAmt;

                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentDetails.ApplicationNumber);
                    ViewBag.ContactNo = familydetails == null ? string.Empty : familydetails.FMobile;
                }


                string[] AllHeadings = tblFeeReceipts.PayHeadings.Split(',');
                AllHeadings = AllHeadings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] HeadingPaid = tblFeeReceipts.FeePaids.Split(',');
                HeadingPaid = HeadingPaid.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                for (int i = 0; i < AllHeadings.Length; i++)
                {
                    if (AllHeadings[i] != "" && HeadingPaid[i] != "")
                    {
                        PreviewFeeReceiptViewModel previewFeeReceiptViewModel = new PreviewFeeReceiptViewModel()
                        {
                            CreatedDate = tblFeeReceipts.AddedDate.ToString(),
                            FeePaid = HeadingPaid[i],
                            HeadingNames = AllHeadings[i],
                            SelectedMonths = tblFeeReceipts.PaidMonths
                        };
                        ReceiptPreviewList.Add(previewFeeReceiptViewModel);
                    }

                }
                ViewBag.FeeReceiptsTbl = ReceiptPreviewList;

                if (ReceiptId == 1)
                    ViewBag.Receiptid = 1;
                else
                    ViewBag.Receiptid = 2;



                // return View(tblFeeReceipts);
            }
            return View(tblFeeReceipts);
        }

        public ActionResult PrintpdfusingItextsharp(int? id, int ReceiptId)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            var session = Session["rolename"].ToString();
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.session = session;
            }
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                StudentsRegistration studentDetails = new StudentsRegistration();
                var student_Name = "";
                int? studentId = tblFeeReceipts.StudentId;
                {
                    ViewBag.ScollarNumber = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId).StudentRegisterID;
                    studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                    ViewBag.studentDetails = studentDetails;
                    ViewBag.Total = tblFeeReceipts.ReceiptAmt;
                    ViewBag.classname = tblFeeReceipts.ClassName;
                    ViewBag.receiptid = tblFeeReceipts.FeeReceiptId;
                    ViewBag.concessionamt = tblFeeReceipts.ConcessionAmt;
                    ViewBag.studentname = studentDetails.Name + " " + studentDetails.Last_Name;
                    student_Name = studentDetails.Name + " " + studentDetails.Last_Name;
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentDetails.ApplicationNumber);
                    ViewBag.ContactNo = familydetails == null ? string.Empty : familydetails.FMobile;
                }


                string[] AllHeadings = tblFeeReceipts.PayHeadings.Split(',');
                AllHeadings = AllHeadings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] HeadingPaid = tblFeeReceipts.FeePaids.Split(',');
                HeadingPaid = HeadingPaid.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                for (int i = 0; i < AllHeadings.Length; i++)
                {
                    if (AllHeadings[i] != "" && HeadingPaid[i] != "")
                    {
                        PreviewFeeReceiptViewModel previewFeeReceiptViewModel = new PreviewFeeReceiptViewModel()
                        {
                            CreatedDate = tblFeeReceipts.AddedDate.ToString(),
                            FeePaid = HeadingPaid[i],
                            HeadingNames = AllHeadings[i],
                            SelectedMonths = tblFeeReceipts.PaidMonths
                        };
                        ReceiptPreviewList.Add(previewFeeReceiptViewModel);
                    }

                }
                ViewBag.FeeReceiptsTbl = ReceiptPreviewList;

                if (ReceiptId == 1)
                    ViewBag.Receiptid = 1;
                else
                    ViewBag.Receiptid = 2;
                var data = tblFeeReceipts;
                var htmltostrin = Renderviewtostring(ControllerContext, "~/Views/AdmissionFee/FeeReceiptPreview.cshtml", data);

                using (MemoryStream stream = new MemoryStream())
                {
                    StringReader sr = new StringReader(htmltostrin);
                    Document pdfdoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                    PdfWriter writter = PdfWriter.GetInstance(pdfdoc, stream);
                    pdfdoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writter, pdfdoc, sr);
                    pdfdoc.Close();
                    return File(stream.ToArray(), "application/pdf", "" + student_Name + ".pdf");

                }

            }
        }

        static string Renderviewtostring(ControllerContext context, string viewpath, object model = null)
        {
            ViewEngineResult viewEngineResult = null;
            viewEngineResult = ViewEngines.Engines.FindView(context, viewpath, null);

            if (viewEngineResult == null)
            {
                throw new System.IO.FileNotFoundException("View Cannot be found");
            }

            var view = viewEngineResult.View;
            context.Controller.ViewData.Model = model;

            string result = null;

            using (var sw = new StringWriter())
            {
                var ctx = new ViewContext(context, view, context.Controller.ViewData, context.Controller.TempData, sw);
                view.Render(ctx, sw);
                result = sw.ToString();
            }
            return result;
        }

        public ActionResult FeeReceiptPreview()
        {
            return View();
        }

      
        public ActionResult PrintReceiptusingrotativa(int? id, int? ReceiptId)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            Student student = new Student();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            var session = Session["rolename"].ToString();
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.session = session;
            }
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                int? studentId = tblFeeReceipts.StudentId;
                {
                    ViewBag.ScollarNumber = _context.Students.FirstOrDefault(x => x.StudentId == studentId).StudentId;

                    var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                    var sections = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == studentDetails.Section_Id);
                    ViewBag.studentDetails = studentDetails;
                    ViewBag.Section = sections.DataListItemName;
                    ViewBag.Total = tblFeeReceipts.ReceiptAmt;
                    ViewBag.classname = tblFeeReceipts.ClassName + " " + sections.DataListItemName;                   
                    ViewBag.receiptid = tblFeeReceipts.FeeReceiptId;
                    ViewBag.concessionamt = tblFeeReceipts.ConcessionAmt;
                    ViewBag.studentname = studentDetails.Name + "-" + studentDetails.Last_Name;
                    ViewBag.balanceamt = tblFeeReceipts.BalanceAmt;
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentDetails.ApplicationNumber);
                    ViewBag.ContactNo = familydetails == null ? string.Empty : familydetails.FMobile;
                }


                string[] AllHeadings = tblFeeReceipts.PayHeadings.Split(',');
                AllHeadings = AllHeadings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] HeadingPaid = tblFeeReceipts.FeePaids.Split(',');
                HeadingPaid = HeadingPaid.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                for (int i = 0; i < AllHeadings.Length; i++)
                {
                    if (AllHeadings[i] != "" && HeadingPaid[i] != "")
                    {
                        PreviewFeeReceiptViewModel previewFeeReceiptViewModel = new PreviewFeeReceiptViewModel()
                        {
                            CreatedDate = tblFeeReceipts.AddedDate.ToString(),
                            FeePaid = tblFeeReceipts.TotalFee.ToString(),
                            HeadingNames = AllHeadings[i],
                            SelectedMonths = tblFeeReceipts.PaidMonths,
                            HeadingPaidAmount= HeadingPaid[i]
                        };
                        ReceiptPreviewList.Add(previewFeeReceiptViewModel);
                    }

                }
                ViewBag.FeeReceiptsTbl = ReceiptPreviewList;

                var schoolsetup = _context.Tbl_SchoolSetup.ToList();
                TblCreateSchool tblCreateSchool = new TblCreateSchool();
                foreach (var item in schoolsetup)
                {
                    tblCreateSchool = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == item.School_Id);
                }

                ViewBag.Schoolsetup = tblCreateSchool;
                if (ReceiptId == 1)
                    ViewBag.Receiptid = 1;
                else
                    ViewBag.Receiptid = 2;
                var data = tblFeeReceipts;

                return new Rotativa.ViewAsPdf("FeeReceiptPreview", data)
                {
                    //FileName = "FeeReceipt.pdf",
                    //PageMargins = new Rotativa.Options.Margins(10, 5, 10, 5),
                    //PageSize = Rotativa.Options.Size.A5,
                    //PageOrientation = Rotativa.Options.Orientation.Portrait,
                };
            }

        }
       


        public ActionResult TransportReceiptPreview(int? id, int ReceiptId)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            var session = Session["rolename"].ToString();
            if (session == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                ViewBag.session = session;
            }
            if (id != null)
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                //tblFeeReceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == id);
                int? studentId = tblFeeReceipts.StudentId;

                {
                    ViewBag.ScollarNumber = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId).StudentRegisterID;

                    var studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                    ViewBag.studentDetails = studentDetails;
                    //ViewBag.Total = tblFeeReceipts.TotalFee - tblFeeReceipts.ConcessionAmt + tblFeeReceipts.LateFee + tblFeeReceipts.OldBalance;
                    ViewBag.Total = tblFeeReceipts.ReceiptAmt;
                    var familydetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentDetails.ApplicationNumber);
                    ViewBag.ContactNo = familydetails == null ? string.Empty : familydetails.FMobile;
                }


                string[] AllHeadings = tblFeeReceipts.PayHeadings.Split(',');
                AllHeadings = AllHeadings.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                string[] HeadingPaid = tblFeeReceipts.FeePaids.Split(',');
                HeadingPaid = HeadingPaid.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                for (int i = 0; i < AllHeadings.Length; i++)
                {
                    if (AllHeadings[i] != "" && HeadingPaid[i] != "")
                    {
                        PreviewFeeReceiptViewModel previewFeeReceiptViewModel = new PreviewFeeReceiptViewModel()
                        {
                            CreatedDate = tblFeeReceipts.AddedDate.ToString(),
                            FeePaid = HeadingPaid[i],
                            HeadingNames = AllHeadings[i],
                            SelectedMonths = tblFeeReceipts.PaidMonths
                        };
                        ReceiptPreviewList.Add(previewFeeReceiptViewModel);
                    }

                }
                ViewBag.FeeReceiptsTbl = ReceiptPreviewList;

                if (ReceiptId == 1)
                    ViewBag.Receiptid = 1;
                else
                    ViewBag.Receiptid = 2;
            }
            return View(tblFeeReceipts);
        }


        public ActionResult DownloadReceiptPreview(int? id)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            if (id != null)
            {
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                int? studentId = tblFeeReceipts.StudentId;
                var studentDetails = _StudentRepository.GetById(studentId);
                ViewBag.studentDetails = studentDetails;
                ViewBag.Total = tblFeeReceipts.TotalFee - tblFeeReceipts.ConcessionAmt + tblFeeReceipts.LateFee + tblFeeReceipts.OldBalance;

                string[] AllHeadings = tblFeeReceipts.PayHeadings.Split(',');
                string[] HeadingPaid = tblFeeReceipts.FeePaids.Split(',');
                for (int i = 0; i < AllHeadings.Length; i++)
                {
                    PreviewFeeReceiptViewModel previewFeeReceiptViewModel = new PreviewFeeReceiptViewModel()
                    {
                        CreatedDate = tblFeeReceipts.AddedDate.ToString(),
                        FeePaid = HeadingPaid[i],
                        HeadingNames = AllHeadings[i],
                        SelectedMonths = tblFeeReceipts.PaidMonths
                    };
                    ReceiptPreviewList.Add(previewFeeReceiptViewModel);
                }
                ViewBag.FeeReceiptsTbl = ReceiptPreviewList;
                // return View(tblFeeReceipts);
            }
            return new ViewAsPdf(tblFeeReceipts);
        }

        //public ActionResult PrintSalarySlip(int id)
        //{
        //    var report = new ActionAsPdf("IndexById", new { id = id });
        //    return report;
        //}


        public ActionResult FeeReportsByClass()
        {
            //ViewBag.Classes = new SelectList(_ClassesRepository.GetAll().OrderBy(x => x.ClassName).ToList(), "Id", "ClassName");
            var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var sections =  _context.DataListItems.Where(e=>e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            ViewBag.Classes = new SelectList((Class).ToList(), "DataListItemId", "DataListItemName");



            ViewBag.Classes = Class;
            ViewBag.Sections = sections;
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        /// 
        //public ActionResult AllFeeReports(string download, string FromDate, string ToDate)
        //{
        //    var getAllFeePlanes = _FeePlansRepository.GetAll().GroupBy(x => new { x.ClassName, x.CategoryName, x.FeeName }, (key, group) => group.First()).ToList();
        //    List<string> allStudentClasses = _StudentRepository.GetAll().Select(x => x.Class).Distinct().ToList();
        //    List<Classes> allClasses = _ClassesRepository.GetAll().ToList();
        //    List<ViewReportViewModel> allFeeReports = new List<ViewReportViewModel>();

        //    foreach (var item in allClasses)
        //    {
        //        float allAmountByClass = 0;
        //        float receivedAmountbyClass = 0;
        //        float remainingAmountByClass = 0;
        //        var allStudents = _StudentRepository.GetAll().Where(x => x.Class == item.ClassName);
        //        foreach (var student in allStudents)
        //        {
        //            float remainingAmount = 0;
        //            var totalClassFee = GetAllFeebyClassCategory(item.ClassName, student.Category);
        //            allAmountByClass = allAmountByClass + totalClassFee;

        //            var allFeeReceipts = _TblFeeReceiptsRepository.GetAll().Where(x => x.StudentId == student.StudentId).GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();
        //            foreach (var feeReceipt in allFeeReceipts)
        //            {
        //                receivedAmountbyClass = receivedAmountbyClass + feeReceipt.ReceiptAmt;
        //                remainingAmount = allFeeReceipts.LastOrDefault().OldBalance;
        //            }
        //            remainingAmountByClass = remainingAmountByClass + remainingAmount;
        //        }
        //        ViewReportViewModel viewReport = new ViewReportViewModel()
        //        {
        //            ClassName = item.ClassName,
        //            GrossFee = allAmountByClass,
        //            ReceiveFee = receivedAmountbyClass,
        //            DueFee = remainingAmountByClass
        //        };
        //        allFeeReports.Add(viewReport);


        //    }
        //    ViewBag.allFeeReports = allFeeReports;
        //    //var allClasses = _ClassesRepository.GetAll().ToList();
        //    if (download == "Ok")
        //    {
        //        return new ViewAsPdf();
        //    }
        //    return View();
        //}

        /// <summary>
        /// Get All Fee Reports based on Classes and B/w Classes
        /// </summary>
        public void GetFeeReportsByClasses()
        {

        }
        public bool CheckStudentMonFee(int studentId, int FeeHeadingId, string semester)
        {
            FeeHeadings feeHeading = _FeeHeadingsRepository.GetById(FeeHeadingId);
            IEnumerable<TblFeeReceipts> feeReceipt = _TblFeeReceiptsRepository.GetAll().Where(x => x.StudentId == studentId && x.FeeHeadingIDs == Convert.ToString(FeeHeadingId) && x.ClassName == semester).ToList();
            var totalAmt = feeReceipt.Any() ? feeReceipt.FirstOrDefault().TotalFee : 0;
            var receiptAmt = feeReceipt.Any() ? feeReceipt.Sum(x => x.ReceiptAmt) : 0;

            if (feeReceipt.Count() == 0)
            {
                return true;
            }

            if (feeHeading.Jan == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Jan == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Feb == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Feb == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Mar == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Mar == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Apr == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Apr == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.May == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.May == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Jun == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Jun == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Jul == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Jul == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Aug == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Aug == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Sep == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Sep == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Oct == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Oct == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Nov == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Nov == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            if (feeHeading.Dec == 1)
            {
                if (feeReceipt.FirstOrDefault(x => x.Dec == true) != null)
                {
                    if (totalAmt > receiptAmt)
                        return true;
                }
                else
                {
                    return true;
                }
            }
            return false;

        }

        public float GetAllFeebyClassCategory(string className, string category)
        {
            float totalClassFee = 0;
            var allFeePlanesByClass = _FeePlansRepository.GetAll()
                                        .Where(x => x.ClassName
                                        .ToLower() == className && x.CategoryName.ToLower() == category.ToLower())
                                        .ToList();
            var allFeeHeadings = _FeeHeadingsRepository.GetAll().ToList();

            foreach (var item in allFeePlanesByClass)
            {
                float feePlaneValue = item.FeeValue;
                var feeHeadingDetail = allFeeHeadings
                                      .FirstOrDefault(x => x.FeeId == item.FeeId);

                if (feeHeadingDetail.Jan == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;
                }
                if (feeHeadingDetail.Feb == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Mar == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Apr == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.May == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Jun == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Jul == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Aug == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Sep == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Oct == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Nov == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
                if (feeHeadingDetail.Dec == 1)
                {
                    totalClassFee = totalClassFee + feePlaneValue;

                }
            }
            return totalClassFee;

        }

        [HttpPost]
        public bool PayDueAmmount(PayDuaAmmountViewModel PayDuaAmmountViewModel)
        {
            if (PayDuaAmmountViewModel != null)
            {
                string Class = PayDuaAmmountViewModel.Class;
                var feeReceipts1 = _context.TblFeeReceipts.ToList().LastOrDefault(x => x.StudentId == PayDuaAmmountViewModel.StudentId && x.ClassName == Class);
                //feeReceipts.OldBalance = feeReceipts.OldBalance - Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                //feeReceipts.ReceiptAmt = feeReceipts.ReceiptAmt + Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                //feeReceipts.BalanceAmt = feeReceipts.BalanceAmt - Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                var feeReceipts = new TblFeeReceipts();


                feeReceipts.StudentId = feeReceipts1.StudentId;
                feeReceipts.Jan = feeReceipts1.Jan;
                feeReceipts.Feb = feeReceipts1.Feb;
                feeReceipts.Mar = feeReceipts1.Mar;
                feeReceipts.Apr = feeReceipts1.Apr;
                feeReceipts.May = feeReceipts1.May;
                feeReceipts.Jun = feeReceipts1.Jun;
                feeReceipts.Jul = feeReceipts1.Jul;
                feeReceipts.Aug = feeReceipts1.Aug;
                feeReceipts.Sep = feeReceipts1.Sep;
                feeReceipts.Oct = feeReceipts1.Oct;
                feeReceipts.Nov = feeReceipts1.Nov;
                feeReceipts.Dec = feeReceipts1.Dec;
                feeReceipts.Type = feeReceipts1.Type;
                feeReceipts.PaidMonths = feeReceipts1.PaidMonths;
                feeReceipts.ClassId = feeReceipts1.ClassId;
                feeReceipts.CategoryId = feeReceipts1.CategoryId;
                feeReceipts.AddedDate = feeReceipts1.AddedDate;
                feeReceipts.ModifiedDate = feeReceipts1.ModifiedDate;
                feeReceipts.IP = feeReceipts1.IP;
                feeReceipts.UserId = feeReceipts1.UserId;
                feeReceipts.IsDeleted = feeReceipts1.IsDeleted;
                feeReceipts.CreateBy = feeReceipts1.CreateBy;
                feeReceipts.Concession = feeReceipts1.Concession;
                feeReceipts.ConcessionAmt = feeReceipts1.ConcessionAmt;
                feeReceipts.StudentName = feeReceipts1.StudentName;
                feeReceipts.PayHeadings = feeReceipts1.PayHeadings;
                // feeReceipts.OldBalance                 =feeReceipts1.OldBalance                ;
                // feeReceipts.ReceiptAmt                 =feeReceipts1.ReceiptAmt                ;
                feeReceipts.ClassName = feeReceipts1.ClassName;
                feeReceipts.CategoryName = feeReceipts1.CategoryName;
                feeReceipts.TotalFee = feeReceipts1.TotalFee;
                feeReceipts.LateFee = feeReceipts1.LateFee;
                //feeReceipts.BalanceAmt                 =feeReceipts1.BalanceAmt                ;
                feeReceipts.PaymentMode = feeReceipts1.PaymentMode;
                feeReceipts.BankName = feeReceipts1.BankName;
                feeReceipts.CheckId = feeReceipts1.CheckId;
                feeReceipts.Remark = feeReceipts1.Remark;
                feeReceipts.FeePaids = feeReceipts1.FeePaids;
                feeReceipts.FeeReceiptsOneTimeCreator = feeReceipts1.FeeReceiptsOneTimeCreator;
                feeReceipts.CurrentYear = feeReceipts1.CurrentYear;
                feeReceipts.InsertBy = feeReceipts1.InsertBy;
                feeReceipts.BatchName = feeReceipts1.BatchName;



                feeReceipts.OldBalance = Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                feeReceipts.ReceiptAmt = feeReceipts.ReceiptAmt + Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                feeReceipts.BalanceAmt = Convert.ToInt32(PayDuaAmmountViewModel.DuaAmmount);
                _context.TblFeeReceipts.Add(feeReceipts);
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }

        }

        public string PaidMonthList(FeeHeadings feeHeadings, string[] monthList)
        {
            List<string> monthNames = new List<string>();
            foreach (var month in monthList)
            {
                if (month == "Jan" && feeHeadings.Jan == 1)
                {
                    monthNames.Add("Jan");
                }
                else if (month == "Feb" && feeHeadings.Feb == 1)
                {
                    monthNames.Add("Feb");
                }
                else if (month == "Mar" && feeHeadings.Mar == 1)
                {
                    monthNames.Add("Mar");

                }
                else if (month == "Apr" && feeHeadings.Apr == 1)
                {
                    monthNames.Add("Apr");
                }
                else if (month == "May" && feeHeadings.May == 1)
                {
                    monthNames.Add("May");
                }
                else if (month == "Jun" && feeHeadings.Jun == 1)
                {
                    monthNames.Add("Jun");
                }
                else if (month == "Jul" && feeHeadings.Jul == 1)
                {
                    monthNames.Add("Jul");
                }
                else if (month == "Aug" && feeHeadings.Aug == 1)
                {
                    monthNames.Add("Aug");
                }
                else if (month == "Sep" && feeHeadings.Sep == 1)
                {
                    monthNames.Add("Sep");
                }
                else if (month == "Oct" && feeHeadings.Oct == 1)
                {
                    monthNames.Add("Oct");
                }
                else if (month == "Nov" && feeHeadings.Nov == 1)
                {
                    monthNames.Add("Nov");
                }
                else if (month == "Dec" && feeHeadings.Dec == 1)
                {
                    monthNames.Add("Dec");
                }
            }
            return string.Join(",", monthNames);
        }


        public float GetStudentWillPayAmmount(int studentId)
        {
            float totalFeeForStudent = 0;
            var studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            if (studentId > 0)
            {
                totalFeeForStudent = GetTotalFeeForStudent(studentDetail);
            }
            return totalFeeForStudent;
        }
        public float GetTotalFeeForStudent(Student tbl_StudentDetail)
        {
            float total = 0;
            if (tbl_StudentDetail != null)
            {
                var allFeePlanes = _context.FeePlans.Where(x => x.BatchName == tbl_StudentDetail.BatchName && x.ClassName == tbl_StudentDetail.Class
                                    && x.CategoryName == tbl_StudentDetail.Category
                                    && x.FeeId != (int)FeeHeadingsEnum.TC && x.FeeId != (int)FeeHeadingsEnum.ADMISSION_FEE).ToList();//GroupBy(x => x.FeeId).Select(x => x.LastOrDefault());
                foreach (var item in allFeePlanes)
                {
                    var FeeHeadings = _context.FeeHeadings.FirstOrDefault(x => x.FeeId == item.FeeId);
                    if (FeeHeadings != null)
                    {
                        if (FeeHeadings.Jan == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Feb == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Mar == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Apr == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.May == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Jun == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Jul == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Aug == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Sep == 1)
                        {
                            total = total + item.FeeValue;

                        }
                        if (FeeHeadings.Oct == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Nov == 1)
                        {
                            total = total + item.FeeValue;
                        }
                        if (FeeHeadings.Dec == 1)
                        {
                            total = total + item.FeeValue;
                        }

                    }
                }
            }
            return total;

        }

        public float GetStudentConcessionAmmount(int studentId)
        {
            float concessionAmount = 0;
            if (studentId > 0)
            {
                var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (studentDetails != null)
                {
                    var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentDetails.StudentId
                                            && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.ADMISSION_FEE)).ToList()
                                            .GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();

                    if (allFeeReceipts.Count > 0)
                    {
                        foreach (var item in allFeeReceipts.GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList())
                        {
                            concessionAmount += item.ConcessionAmt;
                        }
                    }

                }
            }
            return concessionAmount;

        }

        public float GetStudentPaidAmmount(int studentId)
        {
            float totalPaidAmount = 0;
            if (studentId > 0)
            {
                var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (studentDetails != null)
                {
                    var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentDetails.StudentId
                                            && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.ADMISSION_FEE)).ToList();//.GroupBy(x=>x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());

                    if (allFeeReceipts.Count > 0)
                    {
                        foreach (var item in allFeeReceipts.GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList())
                        {
                            totalPaidAmount += item.ReceiptAmt;
                        }
                    }

                }
            }

            return totalPaidAmount;
        }

        public float GetStudentOldAmmount(int studentId)
        {
            float discount = 0;
            if (studentId > 0)
            {
                var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                if (studentDetails != null)
                {
                    var LastFeeReceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentDetails.StudentId
                                        && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.ADMISSION_FEE))
                                        .OrderByDescending(x => x.FeeReceiptId).FirstOrDefault();

                    if (LastFeeReceipt != null)
                    {
                        discount = LastFeeReceipt.OldBalance;
                    }
                }
            }
            return discount;

        }

        public string GenerateHtmlForTotalPendingAmmount(List<StudentTotalFeeViewModel> studentTotalFeeViewModels)
        {
            StringBuilder html = new StringBuilder();
            int i = 1;

            float totalPaidFromStudent = 0;
            float totalPaidAmountByStudent = 0;
            float totalPaidDisAmount = 0;
            float totalPendingAmount = 0;
            float totalConcesionAmount = 0;
            foreach (var item in studentTotalFeeViewModels)
            {
                html.Append("<tr>");
                html.Append("<td>" + i + "</td>");
                html.Append("<td>" + item.StudentName + "</td>");
                html.Append("<td>" + item.ScholarNumber + "</td>");
                html.Append("<td>" + item.Batch + "</td>");
                html.Append("<td>" + item.Semester + "</td>");
                html.Append("<td>" + item.TotalAmount + "</td>");
                html.Append("<td>" + item.PaidAmount + "</td>");
                html.Append("<td>" + item.oldAmmount + "</td>");
                html.Append("<td>" + item.ConcesionAmount + "</td>");

                html.Append("<td>" + item.PendingAmount + "</td>");

                totalPaidFromStudent += item.PaidAmount;
                totalPaidDisAmount += item.oldAmmount;
                totalPendingAmount += item.PendingAmount;
                totalConcesionAmount += item.ConcesionAmount;
                totalPaidAmountByStudent += item.TotalAmount;
                html.Append("</tr>");
                i++;

            }

            html.Append("<tr>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td><b>Total :</b></td>");

            html.Append("<td><b>" + totalPaidAmountByStudent + "</b></td>");
            html.Append("<td><b>" + totalPaidFromStudent + "</b></td>");
            html.Append("<td><b>" + totalPaidDisAmount + "</b></td>");
            html.Append("<td><b>" + totalConcesionAmount + "</b></td>");
            html.Append("<td><b>" + totalPendingAmount + "</b></td>");

            html.Append("</tr>");
            return Convert.ToString(html);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        /// 

        public class FeeHeader
        {
            public string FeeId { get; set; }
            public string FeeName { get; set; }
        }
        public ActionResult AllFeeReports()
        {
            //var allStudents = _context.Students.ToList();
            var batchdetails = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true).FirstOrDefault();
            var allStudents = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Batch_Id == batchdetails.Batch_Id).ToList();
            //var tblstudent = _context.Students.ToList();
            var lst = _context.FeeHeadings.Where(x => x.FeeId != 21 && x.FeeId != 24 && x.FeeId != 25).ToList();
            List<FeeHeader> HeaderList = new List<FeeHeader>();
            //var currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            foreach (var lt in lst)
            {
                FeeHeader data = new FeeHeader();
                data.FeeId = Convert.ToString(lt.FeeId);
                data.FeeName = lt.FeeName;
                HeaderList.Add(data);
            }

            var studentNameList = new List<SelectListItem>();

            foreach (var item in allStudents)
            {
                studentNameList.Add(new SelectListItem()
                {
                    Text = item.Name + " " + item.Last_Name,
                    Value = Convert.ToString(item.StudentRegisterID)
                });
            }
            ViewBag.AllStudentName = studentNameList;

            //var schoolTypeList = new List<SelectListItem>();
            //var schoolTypeName = "";
            //var schoolTypeId = "";
            //for (int ii = 0; ii < 2; ii++)
            //{
            //    schoolTypeName = ii == 0 ? "Pre School" : "High School";
            //    schoolTypeId = ii == 0 ? "1" : "2";
            //    schoolTypeList.Add(new SelectListItem()
            //    {
            //        Text = schoolTypeName,
            //        Value = schoolTypeId
            //    });
            //}
            ViewBag.SchoolType = GetschoolTypeList("0");


            ViewBag.FeeHeadings = HeaderList;
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var allFeeReceipts = _context.TblFeeReceipts.ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();
            //var allFeeReceipts = _context.TblFeeReceipts.ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();
            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate == DateTime.Now).ToList();
            List<TodayFeeReportViewModel> allReport = new List<TodayFeeReportViewModel>();
            int i = 1;
            foreach (var item in allFeeReceipts)
            {
                if (item.FeeHeadingIDs != "21" || item.FeeHeadingIDs != "25" || item.FeeHeadingIDs != "21,25")
                {
                    var scholoarno = allStudents.FirstOrDefault(x => x.StudentRegisterID == item.StudentId);
                    string sname, sno;
                    //if(scholoarno != null)
                    //{
                    //    sname = tblstudent.FirstOrDefault(x => x.StudentId == scholoarno.StudentId).Name;
                    //    sno = Convert.ToString( tblstudent.FirstOrDefault(x => x.StudentId == scholoarno.StudentId)?.StudentId);
                    //}
                    //else
                    {
                        sname = allStudents.FirstOrDefault(x => x.StudentRegisterID == scholoarno.StudentRegisterID).Name;
                        sno = Convert.ToString(allStudents.FirstOrDefault(x => x.StudentRegisterID == scholoarno.StudentRegisterID).StudentRegisterID);
                    }
                    allReport.Add(new TodayFeeReportViewModel
                    {
                        SNO = i,
                        StudentName = sname,
                        ScholarNumber = sno,
                        BillNo = Convert.ToString(item.FeeReceiptId),
                        Amount = Convert.ToString(item.ReceiptAmt),
                        Heading = item.PayHeadings,
                        PaidDate = item.AddedDate
                    });
                    i++;
                }

            }

            ViewBag.allFeeReceipts = allReport;
            //if (todayFeeCollection.Download == "Ok")
            //{
            //    return new ViewAsPdf();
            //}
            return View();
        }


        public ActionResult AllTransportFeeReports()
        {
            //var allStudents = _context.Students.ToList();
            var allStudents = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            //var tblstudent = _context.Students.ToList();
            var feelist = _context.FeeHeadings.ToList();
            List<FeeHeadings> FeeHeadings = new List<FeeHeadings>();
            foreach (var item in feelist)
            {
                if (item.FeeId == 21 || item.FeeId == 25)
                {
                    FeeHeadings.Add(item);
                }
            }
            var lst = FeeHeadings.ToList();
            List<FeeHeader> HeaderList = new List<FeeHeader>();
            //var currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            foreach (var lt in lst)
            {
                FeeHeader data = new FeeHeader();
                data.FeeId = Convert.ToString(lt.FeeId);
                data.FeeName = lt.FeeName;
                HeaderList.Add(data);
            }

            var studentNameList = new List<SelectListItem>();

            foreach (var item in allStudents)
            {
                studentNameList.Add(new SelectListItem()
                {
                    Text = item.Name + " " + item.Last_Name,
                    Value = Convert.ToString(item.StudentRegisterID)
                });
            }
            ViewBag.AllStudentName = studentNameList;

            var schoolTypeList = new List<SelectListItem>();
            var schoolTypeName = "";
            var schoolTypeId = "";
            for (int ii = 0; ii < 2; ii++)
            {
                schoolTypeName = ii == 0 ? "Pre School" : "High School";
                schoolTypeId = ii == 0 ? "1" : "2";
                schoolTypeList.Add(new SelectListItem()
                {
                    Text = schoolTypeName,
                    Value = schoolTypeId
                });
            }
            ViewBag.SchoolType = schoolTypeList;

            ViewBag.FeeHeadings = HeaderList;
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //var feerec = _context.TblFeeReceipts.ToList();
            var allFeeReceipts = _context.TblFeeReceipts.ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();
            string date = DateTime.Now.ToString("dd/MM/yyyy").Replace('-', '/');
            List<TblFeeReceipts> TblFeeReceipts = new List<TblFeeReceipts>();
            foreach (var item in allFeeReceipts)
            {

                var feedate = (item.AddedDate.ToString());
                if (feedate.Length > 9)
                {
                    feedate.Remove(9);
                }
                if (feedate == date)
                {
                    TblFeeReceipts.Add(item);
                }
            }
            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate == DateTime.Now).ToList();
            List<TodayFeeReportViewModel> allReport = new List<TodayFeeReportViewModel>();
            int i = 1;
            foreach (var item in TblFeeReceipts)
            {
                if (item.FeeHeadingIDs == "21" || item.FeeHeadingIDs == "25" || item.FeeHeadingIDs == "21,25" || item.FeeHeadingIDs == "25,21")
                {
                    var scholoarno = allStudents.FirstOrDefault(x => x.StudentRegisterID == item.StudentId);
                    string sname, sno;
                    //if(scholoarno != null)
                    //{
                    //    sname = tblstudent.FirstOrDefault(x => x.StudentId == scholoarno.StudentId).Name;
                    //    sno = Convert.ToString( tblstudent.FirstOrDefault(x => x.StudentId == scholoarno.StudentId)?.StudentId);
                    //}
                    //else
                    {
                        sname = allStudents.FirstOrDefault(x => x.StudentRegisterID == scholoarno.StudentRegisterID).Name;
                        sno = Convert.ToString(allStudents.FirstOrDefault(x => x.StudentRegisterID == scholoarno.StudentRegisterID).StudentRegisterID);
                    }
                    allReport.Add(new TodayFeeReportViewModel
                    {
                        SNO = i,
                        StudentName = sname,
                        ScholarNumber = sno,
                        BillNo = Convert.ToString(item.FeeReceiptId),
                        Amount = Convert.ToString(item.ReceiptAmt),
                        Heading = item.PayHeadings,
                        PaidDate = item.AddedDate
                    });
                    i++;
                }

            }

            ViewBag.allFeeReceipts = allReport;
            return View();
        }

        #region Apply TC START
        /// <summary>
        /// Apply TC page
        /// </summary>
        /// <returns></returns>
        public ActionResult ApplyTC()
        {
            try
            {
                //ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
                //ViewBag.AllYears = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "year").DataListId.ToString()).ToList();
                //ViewBag.AllClass = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                ViewBag.AllClass = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                ViewBag.AllBatch = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
                ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
                //ViewBag.AllBatchs = _context.Tbl_Batches.ToList();
                //ViewBag.Batches = new SelectList(_context.Tbl_Batches.ToList().OrderBy(x => x.Batch_Name).ToList(), "id", "BatchName");
                ViewBag.Remarks = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Remarks").DataListId.ToString()).ToList();
                //ViewBag.Reason = _context.DataListItems.Where(e => e.DataListId == "38").ToList();
                ViewBag.Reason = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Reason").DataListId.ToString()).ToList();

                var AllBatch = _context.Tbl_Batches.ToList();
                ViewBag.BatcheNames = new SelectList(AllBatch, "Batch_Id", "Batch_Name", AllBatch.OrderByDescending(x=>x.Batch_Id).FirstOrDefault().Batch_Id);
            }
            catch (Exception ex)
            {
                string exceptionDetails = ex.Message;
            }

            return View();
        }




        #endregion Apply TC END

        #region Print TC START
        /// <summary>
        /// Print TC page
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintTC()
        {
            return View();
        }
        public ActionResult PrintTC2()
        {
            return View();
        }




        #endregion Print TC END

        #region TC Billing START

        public ActionResult TCBilling()
        {
            return View();
        }




        #endregion


        public ActionResult FeePaid()
        {
            ViewBag.Message = TempData["message"];
            string studentId = Convert.ToString(Session["StudentId"]);
            //var studentlst = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == studentId);
            var student = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID.ToString() == studentId);
            var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            student.Name = student.Name + " " + student.Last_Name;//john 12-09-2022
            student.Class = Classes.FirstOrDefault(x => x.DataListItemId == student.Class_Id)?.DataListItemName;
            student.Category = AllCategorys.FirstOrDefault(x => x.DataListItemId == student.Category_Id)?.DataListItemName;
            ViewBag.Classes=Classes.ToList();

            var branchdata = _context.Tbl_CreateBranch.ToList();
            var schoolsetup = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
            Tbl_CreateBranch tbl_CreateBranch = new Tbl_CreateBranch();
            if (schoolsetup != null)
            {
                tbl_CreateBranch = branchdata.FirstOrDefault(x => x.Bank_Id == schoolsetup.Bank_Id && x.Branch_ID == schoolsetup.Branch_Id);
            }

            ViewBag.TblBranch = tbl_CreateBranch.Branch_Name;

            return View(student);
        }

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
        private List<SelectListItem> GetschoolTypeList(string selectedId)
        {
            var items = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "Select School",
                    Value = "0",


                },

                new SelectListItem()
                {
                    Text = "Pre School",
                    Value = "1",


                },

                new SelectListItem()
                {
                    Text = "High School",
                    Value = "2"
                }
            };

            var temp = items.FirstOrDefault(f => f.Value == selectedId);

            if (temp != null)
                temp.Selected = true;


            return items;

        }

    }
}

