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
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace SchoolManagement.Website.Controllers
{
    public class TransportFeeController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationDbContext _contextstudent = new ApplicationDbContext();
        // GET: StudentAdmission
        private IRepository<StudentRegistrationHistory> _StudentRegistrationHistory = null;
        private IRepository<TransportFeeConfiguration> _TransportFeeConfiguration = null;
        private IRepository<TransportFeeHeadings> _TransportFeeHeadings = null;
        private IRepository<FeeHeadings> _FeeHeadingsRepository = null;
        private IRepository<FeePlans> _FeePlansRepository = null;
        private IRepository<TblFeeReceipts> _TblFeeReceiptsRepository = null;



        private IRepository<Frequencys> _frequencyRepository = null;
        private IRepository<TransportFeeHeadings> _TransportFeeHeadingsRepository = null;
        private IRepository<Student> _Student = null;
        private IRepository<TblTransportFeeReceipts> _TblTransportFeeReceipts = null;
        private IRepository<TblDueFee> _TblDueFeeRepository = null;
        private IRepository<TransportFeePlans> _TransportFeePlans = null;
        // GET: TransportFee
        public TransportFeeController()
        {
            _TransportFeeConfiguration = new Repository<TransportFeeConfiguration>();
            _frequencyRepository = new Repository<Frequencys>();
            _TransportFeeHeadings = new Repository<TransportFeeHeadings>();
            _TransportFeeHeadingsRepository = new Repository<TransportFeeHeadings>();
            _TblTransportFeeReceipts = new Repository<TblTransportFeeReceipts>();
            _TblDueFeeRepository = new Repository<TblDueFee>();
            _TransportFeePlans = new Repository<TransportFeePlans>();
            _Student = new Repository<Student>();
            _FeeHeadingsRepository = new Repository<FeeHeadings>();
            _FeePlansRepository = new Repository<FeePlans>();
            _TblFeeReceiptsRepository = new Repository<TblFeeReceipts>();

        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TransportFeeConfiguration()
        {

            var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var batches = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            ViewBag.BatcheNames = batches;
            ViewBag.Classes = Classes;

            //var BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            List<TransportFeeConfigurationView> TransfeeConfig = new List<TransportFeeConfigurationView>();
            var result = _context.TransportFeeConfiguration.OrderByDescending(x => x.CreatedOn);
            foreach (var res in result)
            {
                var fee = new TransportFeeConfigurationView();

                fee.Id = res.TransportFeeConfigurationID;
                fee.Batch_Id = res.Batch_Id;
                fee.BatchName = batches.FirstOrDefault(x => x.DataListItemId == res.Batch_Id)?.DataListItemName;
                fee.Class_Id = res.Class_Id;
                fee.Class = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                fee.FromKM = res.FromKM;
                fee.ToKM = res.ToKM;
                fee.Amount = res.Amount;
                TransfeeConfig.Add(fee);
            }
            ViewBag.StudentReg = TransfeeConfig;
            return View();
        }

        [HttpPost]
        public JsonResult ConfigureTransportFee(TransportFeeConfigurationView fee)
        {
            TransportFeeConfiguration objStudentRegNumberMaster = new TransportFeeConfiguration
            {
                TransportFeeConfigurationID = fee.Id,
                BatchName = fee.BatchName,
                Batch_Id = fee.Batch_Id,
                Class = fee.Class,
                Class_Id = fee.Class_Id,
                FromKM = fee.FromKM,
                ToKM = fee.ToKM,
                Amount = fee.Amount,
                CreatedOn = DateTime.Today,
            };

            _TransportFeeConfiguration.Insert(objStudentRegNumberMaster);
            _TransportFeeConfiguration.Save();
            return Json("Ok");
        }

        public ActionResult TransportFeeHeadingsList()
        {
            var dataListId = _context.DataLists.Where(w => w.DataListName == "FeeType")
                .Select(s => s.DataListId).FirstOrDefault();

            var feeTypeId = _context.DataListItems.Where(w => w.DataListId == dataListId.ToString() && w.DataListItemName == "TransactionFee")
                .Select(s => s.DataListItemId).FirstOrDefault();


            ViewBag.Frequencys = new SelectList(_context.Frequencys
                .OrderBy(x => x.FeeFrequencyName).ToList(), "FeeFrequencyId", "FeeFrequencyName");

            var feeHeadingsList = _context.FeeHeadings
                .Where(w => w.FeeType_Id == feeTypeId).ToList();

            return View(feeHeadingsList);
        }
        [HttpPost]
        public JsonResult AddFeeHeading(TransportFeeHeadings feeHeadings)
        {
            TransportFeeHeadings data = _TransportFeeHeadingsRepository.GetAll().FirstOrDefault(x => x.FeeName == feeHeadings.FeeName);

            if (feeHeadings != null && data == null)
            {
                _TransportFeeHeadingsRepository.Insert(feeHeadings);
                _TransportFeeHeadingsRepository.Save();
                return Json("Ok");
            }
            return Json("NotOk");
        }

        public ActionResult TransportFeeReceipts()
        {

            var pagename = "Transport Fee Configure Planes";

            var createpermission = "Create_permission";

            var studentlist = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            var transportdata = _context.AdditionalInformations.ToList();
            List<StudentsRegistration> StudentsRegistration = new List<StudentsRegistration>();
            foreach (var item in studentlist)
            {
                var transport = transportdata.FirstOrDefault(x => x.StudentRefId == item.StudentRegisterID && x.DistancefromSchool != 0 && x.TransportFacility == "yes");
                if (transport != null)
                {
                    StudentsRegistration.Add(new StudentsRegistration
                    {
                        StudentRegisterID = item.StudentRegisterID,
                        Name = item.Name + " " + item.Last_Name
                    });
                }
            }
            ViewBag.StudentNames = new SelectList(StudentsRegistration.OrderBy(x => x.Name).ToList(), "StudentRegisterID", "Name");

            ViewBag.TransportRange = _context.TblTransportReducedAmount.ToList();

            var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Classes = new SelectList((Classes).ToList(), "DataListItemId", "DataListItemName");
            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

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

        public JsonResult GetStudentDetailsById(int studentId)
        {
            StudentDetailViewModel studentModel = new StudentDetailViewModel();

            //Student studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            StudentsRegistration studentDetail = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var classname = Classes.FirstOrDefault(x => x.DataListItemId == studentDetail.Class_Id)?.DataListItemName;

            FamilyDetail familyDetail = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentDetail.ApplicationNumber);
            TblTransportFeeReceipts tblFeeReceipt = _TblTransportFeeReceipts.GetAll().LastOrDefault(x => x.StudentId == studentId);
            studentModel.StudentName = studentDetail.Name;
            studentModel.FatherName = familyDetail?.FatherName;
            studentModel.Contact = familyDetail?.FMobile;
            studentModel.Class = classname == null ? "" : classname;
            studentModel.Category = studentDetail.Category;
            studentModel.RoleNumber = Convert.ToString(studentDetail.StudentRegisterID);
            studentModel.OldBalance = tblFeeReceipt == null ? 0 : tblFeeReceipt.OldBalance;
            studentModel.Batch = studentDetail.BatchName;

            return Json(studentModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudent1Fees(int studentId, string Months)
        {
            ConfigFeeDataViewModel configFeeDataViewModel = new ConfigFeeDataViewModel();
            string[] months = Months.Split(',');
            var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            //var student = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);

            System.Data.Entity.DbSet<TransportFeePlans> allFeePlanes = _context.TransportFeePlans;
            System.Data.Entity.DbSet<TransportFeeHeadings> allFeeHeading = _context.TransportFeeHeadings;

            List<TransportFeePlans> feePlanes = allFeePlanes.Where(x => x.ClassName == student.Class && x.CategoryName == student.Category && x.BatchName == student.BatchName).ToList();
            //List<TblFeeReceipts> TblFeeReceipts = _TblFeeReceiptsRepository.GetAll().Where(x => x.StudentId == studentId).ToList();

            StringBuilder html = new StringBuilder();
            html.Append("<tr>");
            html.Append("<th>");
            html.Append("S.No.");
            html.Append("</th>");

            html.Append("<th>");
            html.Append("FeeHeading");
            html.Append("</th>");

            html.Append("<th>");
            html.Append("Select ");
            html.Append("</th>");
            foreach (string column in months)
            {
                html.Append("<th>");
                html.Append(column);
                html.Append("</th>");
            }

            html.Append("<th>");
            html.Append("Total");
            html.Append("</th>");
            html.Append("<th>");
            html.Append("Collect Fee");
            html.Append("</th>");
            html.Append("</tr>");

            //html Body
            float allTotal = 0;
            int SNO = 1;
            foreach (TransportFeePlans feeplan in feePlanes)
            {
                float total = 0;
                TransportFeeHeadings FeeHeading = allFeeHeading.FirstOrDefault(x => x.TransportFeeId == feeplan.FeeId);
                bool status = CheckStudentMonFee(studentId, FeeHeading.TransportFeeId, student.Class);
                StringBuilder htmlRow = new StringBuilder();
                if (status == false)
                {
                    continue;
                }

                if (student.IsApplyforTC == true)
                {
                    if (feeplan.FeeName != "TC")
                        continue;
                }
                else if (student.IsApprove == 217)
                {
                    if (feeplan.FeeName == "Admission Fee" || feeplan.FeeName == "TC")
                        continue;
                }
                else if (student.IsApplyforAdmission)
                {
                    if (feeplan.FeeName != "Admission Fee")
                        continue;
                }

                var tblFeeReceipt = _context.TblTransportFeeReceipts.Where(x => x.FeeHeadingId == feeplan.FeeId && x.StudentId == studentId).ToList();
                var totalAmt = tblFeeReceipt.Any() ? tblFeeReceipt.FirstOrDefault().TotalFee : 0;
                var receiptAmt = tblFeeReceipt.Any() ? tblFeeReceipt.Sum(x => string.IsNullOrEmpty(x.PaidAmount) ? 0 : Convert.ToInt64(x.PaidAmount)) : 0;
                // if (totalAmt < receiptAmt)

                feeplan.FeeValue = 100;

                if (receiptAmt < feeplan.FeeValue)
                {
                    htmlRow.Append("<tr>");

                    htmlRow.Append("<td>");
                    htmlRow.Append(SNO);
                    htmlRow.Append("</td>");
                    SNO++;
                    htmlRow.Append("<td>");
                    htmlRow.Append(feeplan.FeeName);
                    htmlRow.Append("</td>");

                    htmlRow.Append("<td>");
                    htmlRow.Append("<input id='selectbox' class='selectbox' name='selectbox' type='checkbox'/>");
                    htmlRow.Append("</td>");

                    if (Months.Contains("Jan") && FeeHeading.Jan == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jan) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Jan") && FeeHeading.Jan == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }
                    if (Months.Contains("Feb") && FeeHeading.Feb == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Feb) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }
                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }
                    }
                    if (Months.Contains("Feb") && FeeHeading.Feb == 0)
                    {
                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }
                    if (Months.Contains("Mar") && FeeHeading.Mar == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Mar) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Mar") && FeeHeading.Mar == 0)
                    {
                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }
                    if (Months.Contains("Apr") && FeeHeading.Apr == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Apr) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Apr") && FeeHeading.Apr == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }
                    if (Months.Contains("May") && FeeHeading.May == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.May) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("May") && FeeHeading.May == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }
                    if (Months.Contains("Jun") && FeeHeading.Jun == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jun) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Jun") && FeeHeading.Jun == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    if (Months.Contains("Jul") && FeeHeading.Jul == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jul) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Jul") && FeeHeading.Jul == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    if (Months.Contains("Aug") && FeeHeading.Aug == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Aug) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Aug") && FeeHeading.Aug == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    if (Months.Contains("Sep") && FeeHeading.Sep == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Sep) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Sep") && FeeHeading.Sep == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }


                    if (Months.Contains("Oct") && FeeHeading.Oct == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Oct) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Oct") && FeeHeading.Oct == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    if (Months.Contains("Nov") && FeeHeading.Nov == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Nov) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Nov") && FeeHeading.Nov == 0)
                    {

                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    if (Months.Contains("Dec") && FeeHeading.Dec == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Dec) && totalAmt <= receiptAmt)
                            {
                                htmlRow.Append("<td>");
                                htmlRow.Append(0.0);
                                htmlRow.Append("</td>");
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                htmlRow.Append("<td>");
                                //htmlRow.Append(feeplan.FeeValue);
                                htmlRow.Append(currentAmount);
                                htmlRow.Append("</td>");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            htmlRow.Append("<td>");
                            htmlRow.Append(feeplan.FeeValue);
                            htmlRow.Append("</td>");
                        }

                    }
                    if (Months.Contains("Dec") && FeeHeading.Dec == 0)
                    {
                        htmlRow.Append("<td>");
                        htmlRow.Append(0.0);
                        htmlRow.Append("</td>");
                    }

                    allTotal = allTotal + total;

                    //  var receiptAmt = tblFeeReceipt.Count == 0 ? 0 : tblFeeReceipt.Sum(x => x.ReceiptAmt);
                    htmlRow.Append("<td>");
                    htmlRow.Append(total - receiptAmt);
                    htmlRow.Append("</td>");
                    htmlRow.Append("<td>");
                    htmlRow.Append("<input type='number' value='" + (total - receiptAmt) + "' class='collectFeeNumber'/>");
                    htmlRow.Append("</td>");
                    htmlRow.Append("</tr>");

                    if (total > 0)
                    {
                        html.Append(htmlRow);
                    }
                }
            }
            //html.Append("<tr>");
            //html.Append("<td>");
            //html.Append("</td>");
            //html.Append("<td>");
            //html.Append("<b>All Total</b>");
            //html.Append("</td>");
            //html.Append("<td>");
            //html.Append("</td>");

            //foreach (string column in months)
            //{
            //    html.Append("<td>");
            //    html.Append("</td>");
            //}
            //html.Append("<td>");
            //html.Append("<b>" + allTotal + "</b>");                                    
            //html.Append("</td>");
            //html.Append("</tr>");

            html.Append("<tr>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");

            foreach (string column in months)
            {
                html.Append("<td>");
                html.Append("</td>");
            }
            html.Append("<td>");
            html.Append("<b><span>Total Amount : </span></b></br><b><span><input type='button' class='btn btn-success btn-sm' value='Calculate Fee' id='verifyBtn'/> </span></b>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("<b><span id='allTotal'>0</span></b></br><b><span id='TotalCollectFee'>0</span></b>");
            html.Append("</td>");
            html.Append("</tr>");
            configFeeDataViewModel.ConfigureFeeData = html.ToString();

            return Json(configFeeDataViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentFees(int studentId)
        {

            var dataListId = _context.DataLists.Where(w => w.DataListName == "FeeType")
              .Select(s => s.DataListId).FirstOrDefault();

            var feeTypeId = _context.DataListItems.Where(w => w.DataListId == dataListId.ToString() && w.DataListItemName == "TransactionFee")
                .Select(s => s.DataListItemId).FirstOrDefault();

            var student = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);

            var additionalnfo = _context.AdditionalInformations.Where(x =>
                    x.StudentRefId == studentId
                    && x.TransportFacility == "yes" && x.DistancefromSchool != 0).FirstOrDefault();

            if (additionalnfo == null)
                return null;


            var feesConfig = _context.TransportFeeConfiguration
                .Where(w => w.Batch_Id == student.Batch_Id
                && w.FromKM <= additionalnfo.DistancefromSchool && w.ToKM >= additionalnfo.DistancefromSchool).FirstOrDefault();

            if (feesConfig == null)
                return null;


            var headings = _context.FeeHeadings
                .Where(x => x.FeeType_Id == feeTypeId).ToList();

            if (headings.Count == 0)
                return null;

            var newReceipts = new List<TblFeeReceipts>();
            var oldReceipts = new List<TblFeeReceipts>();

            //Get Allready paid receipt details
            var receipts = _context.TblFeeReceipts
                            .Where(x => x.StudentId == studentId)
                            .ToList();


            foreach (var item in receipts)
            {

                item.FeeHeadingIDs = item.FeeHeadingIDs ?? string.Empty;

                var headingIds = item.FeeHeadingIDs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);


                item.FeePaids = item.FeePaids ?? string.Empty;

                var paids = item.FeePaids.Split(',')
                                    .Where(w => decimal.TryParse(w, out _))
                                    .Select(decimal.Parse)
                                    .OrderBy(o => o).ToList();

                if (headingIds.Length <= 1)
                {
                    oldReceipts.Add(item);
                    continue;
                }

                var i = 0;
                foreach (var id in headingIds)
                {
                    TblFeeReceipts tblFee = new TblFeeReceipts();
                    tblFee.FeeHeadingIDs = id;
                    tblFee.StudentId = item.StudentId;
                    tblFee.PaidAmount = headingIds.Count() == paids.Count ? paids[i].ToString() : "0";

                    oldReceipts.Add(tblFee);
                }

            }


            //Map old to heading

            foreach (var heading in headings)
            {
                TblFeeReceipts fee = new TblFeeReceipts();
                fee.FeeHeadingIDs = heading.FeeId.ToString();

                var listbytes = new List<byte>
                {
                    heading.Jan,
                    heading.Feb,
                    heading.Mar,
                    heading.Apr,
                    heading.May,
                    heading.Jun,
                    heading.Jul,
                    heading.Aug,
                    heading.Sep,
                    heading.Oct,
                    heading.Nov,
                    heading.Dec,

                };


                fee.TotalFee = listbytes.Count(c => c == 1) * feesConfig.Amount;

                var paidAmtstr = oldReceipts.Where(w => w.FeeHeadingIDs == heading.FeeId.ToString())
                    .Select(s => s.PaidAmount).FirstOrDefault();

                int.TryParse(paidAmtstr, out int paidAmt);

                if (fee.TotalFee <= paidAmt)
                    continue;

                fee.StudentId = studentId;
                fee.PayHeadings = heading.FeeName;
                fee.PaidAmount = paidAmt.ToString();
                fee.DueAmount = (fee.TotalFee - paidAmt).ToString();
                newReceipts.Add(fee);
            }


            return Json(newReceipts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddDueFee(DueFeeModel dueFeeModel)
        {
            var currentUser = Session["Name"].ToString();
            //var studentData = _context.Students.FirstOrDefault(x => x.StudentId == dueFeeModel.StudentId);
            var studentData = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == dueFeeModel.StudentId);
            IEnumerable<TblDueFee> dueFeesData = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == studentData.StudentRegisterID && x.BatchName == studentData.BatchName && x.ClassName == studentData.Class).ToList();

            if (!dueFeesData.Any() && !studentData.IsApplyforTC)
            {
                //string feeMonths = string.Join(",", dueFeeModel.Selectedmonths);
                string FeeHeadingAmt = string.Join(",", dueFeeModel.FeeHeadingAmt);
                float[] FeeHeadingAmtValues = dueFeeModel.FeeHeadingAmt;
                string feeHeadings = string.Join(",", dueFeeModel.FeeHeadings);
                //float[] collectionfee = dueFeeModel.collectFees;

                var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == dueFeeModel.ClassName.ToLower());
                var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == dueFeeModel.CategoryName.ToLower());
                var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == dueFeeModel.StudentId);
                List<TransportFeeHeadings> feeHeadingList = _TransportFeeHeadings.GetAll().ToList();
                int i = 0;
                int j = 0;
                var unicNumber = Guid.NewGuid();
                foreach (string item in dueFeeModel.FeeHeadings)
                {

                    if (FeeHeadingAmt[i] == 0)
                    {
                        i++;
                        continue;
                    }

                    TblDueFee dueFee = new TblDueFee();
                    TransportFeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == item);
                    dueFee.FeeHeadingId = feeHeading == null ? 0 : feeHeading.TransportFeeId;
                    dueFee.FeeHeading = feeHeading == null ? "DueBalance" : feeHeading.FeeName;
                    //dueFee.CategoryId = categoryDetail.DataListItemId;
                    //dueFee.CategoryName = categoryDetail.DataListItemName;
                    dueFee.ClassId = classDetail.DataListItemId;
                    dueFee.ClassName = classDetail.DataListItemName;

                    //dueFee.PaidMonths = feeHeading == null ? "" : PaidMonthList(feeHeading, dueFeeModel.Selectedmonths);
                    dueFee.PayHeadings = feeHeadings;
                    dueFee.StudentId = dueFeeModel.StudentId;
                    dueFee.StudentName = studentdetails.Name;
                    //dueFee.TotalFee = dueFeeModel.TotalFee;
                    dueFee.TotalFee = FeeHeadingAmtValues[j];

                    dueFee.FeePaids = FeeHeadingAmt;
                    dueFee.FeeReceiptsOneTimeCreator = unicNumber.ToString();
                    //dueFee.BatchName = studentData.BatchName;

                    //dueFee.Jan = dueFeeModel.Jan[j].ToString();
                    //dueFee.Feb = dueFeeModel.Feb[j].ToString();
                    //dueFee.Mar = dueFeeModel.Mar[j].ToString();
                    //dueFee.Apr = dueFeeModel.Apr[j].ToString();
                    //dueFee.May = dueFeeModel.May[j].ToString();
                    //dueFee.Jun = dueFeeModel.Jun[j].ToString();
                    //dueFee.Jul = dueFeeModel.Jul[j].ToString();
                    //dueFee.Aug = dueFeeModel.Aug[j].ToString();
                    //dueFee.Sep = dueFeeModel.Sep[j].ToString();
                    //dueFee.Oct = dueFeeModel.Oct[j].ToString();
                    //dueFee.Nov = dueFeeModel.Nov[j].ToString();
                    //dueFee.Dec = dueFeeModel.Dec[j].ToString();

                    dueFee.DueAmount = dueFeeModel.collectFees[j];
                    dueFee.PaidAmount = FeeHeadingAmtValues[j] - dueFeeModel.collectFees[j];
                    dueFee.InsertBy = currentUser;

                    _TblDueFeeRepository.Insert(dueFee);
                    _TblDueFeeRepository.Save();
                    j++;
                }
            }

            IEnumerable<TblDueFee> feeReceipt1 = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == dueFeeModel.StudentId).ToList();
            if (feeReceipt1.Any())
            {
                var sum = feeReceipt1.Sum(x => x.TotalFee);
                return Json(sum);
            }
            return Json("");
        }
        public string PaidMonthList(TransportFeeHeadings feeHeadings, string[] monthList)
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
        public bool CheckStudentMonFee(int studentId, int FeeHeadingId, string semester)
        {
            TransportFeeHeadings feeHeading = _TransportFeeHeadings.GetById(FeeHeadingId);
            IEnumerable<TblTransportFeeReceipts> feeReceipt = _TblTransportFeeReceipts.GetAll().Where(x => x.StudentId == studentId && x.FeeHeadingId == FeeHeadingId && x.ClassName == semester).ToList();
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
        public JsonResult AddFeeReceipt(FeeReceiptViewModel feeReceiptViewModel)
        {
            //string feeMonths = string.Join(",", feeReceiptViewModel.Selectedmonths);
            string feePaids = string.Join(",", feeReceiptViewModel.FeeHeadingAmt);
            string feeHeadings = string.Join(",", feeReceiptViewModel.FeeHeadings);
            //Classes classDetail = _ClassesRepository.GetAll().FirstOrDefault(x => x.ClassName.ToLower() == feeReceiptViewModel.ClassName.ToLower());
            var feeheadings = _context.FeeHeadings.ToList();
            var feeplans = _context.FeePlans.ToList();
            string headingid = string.Empty;
            var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId.ToString() == feeReceiptViewModel.ClassName);

            for (int k = 0; k < feeReceiptViewModel.FeeHeadings.Count(); k++)
            {
                var feeheadingid = feeheadings.FirstOrDefault(x => x.FeeName == feeReceiptViewModel.FeeHeadings[k]);
                //var feeplan = feeplans.FirstOrDefault(x => x.FeeId == feeheadingid.FeeId && x.ClassId == classDetail.DataListItemId);
                headingid = string.Join(",", headingid, feeheadingid.FeeId);
            }
            //string feeheading_id = headingid.Remove(0, 1);
            headingid = headingid.TrimStart(',');


            //StudentCategorys categoryDetail = _StudentCategorysRepository.GetAll().FirstOrDefault(x => x.CategoryName.ToLower() == feeReceiptViewModel.CategoryName.ToLower());
            var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == feeReceiptViewModel.CategoryName.ToLower());
            var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == feeReceiptViewModel.StudentId);
            List<FeeHeadings> feeHeadingList = _context.FeeHeadings.ToList();
            int i = 0;
            var unicNumber = Guid.NewGuid();

            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            //FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == feeHeadings);
            tblFeeReceipts.FeeHeadingIDs = headingid;
            tblFeeReceipts.BalanceAmt = feeReceiptViewModel.BalanceAmt;
            tblFeeReceipts.OldBalance = feeReceiptViewModel.BalanceAmt;
            tblFeeReceipts.BankName = feeReceiptViewModel.BankName;
            //tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
            //tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
            tblFeeReceipts.ClassId = feeReceiptViewModel.ClassId;


            tblFeeReceipts.BatchName = feeReceiptViewModel.BatchName;

            //tblFeeReceipts.FeePaidDate = feeReceiptViewModel.DateTimeVal != null ? feeReceiptViewModel.DateTimeVal : DateTime.Now;
            tblFeeReceipts.AddedDate = DateTime.Now;
            tblFeeReceipts.Concession = feeReceiptViewModel.Concession;
            tblFeeReceipts.ConcessionAmt = feeReceiptViewModel.ConcessionAmt;
            tblFeeReceipts.LateFee = feeReceiptViewModel.LateFee;
            //tblFeeReceipts.PaidMonths = PaidMonthList(feeHeading, feeReceiptViewModel.Selectedmonths);
            tblFeeReceipts.PayHeadings = feeHeadings;
            tblFeeReceipts.PaymentMode = feeReceiptViewModel.PaymentMode;
            tblFeeReceipts.ReceiptAmt = feeReceiptViewModel.ReceiptAmt;
            tblFeeReceipts.Remark = feeReceiptViewModel.Remark;
            tblFeeReceipts.StudentId = feeReceiptViewModel.StudentId;
            tblFeeReceipts.StudentName = studentdetails.Name;
            tblFeeReceipts.TotalFee = feeReceiptViewModel.TotalFee;
            tblFeeReceipts.FeePaids = feePaids;
            tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();

            tblFeeReceipts.DueAmount = feeReceiptViewModel.FeeHeadingAmt[i].ToString();
            tblFeeReceipts.PaidAmount = feeReceiptViewModel.collectedFeeAmt[i].ToString();

            _TblFeeReceiptsRepository.Insert(tblFeeReceipts);
            _TblFeeReceiptsRepository.Save();

            var tblfeereceipt = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptsOneTimeCreator == tblFeeReceipts.FeeReceiptsOneTimeCreator);

            var student = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);
            var currentyear = DateTime.Now.Year.ToString();
            var Duefees = _context.TblDueFee.FirstOrDefault(x => x.CurrentYear.ToString() == currentyear && x.FeeHeadingId.ToString() == tblFeeReceipts.FeeHeadingIDs && x.StudentId == studentdetails.StudentRegisterID && x.ClassId == studentdetails.Class_Id);
            if (Duefees != null)
            {
                if (Duefees.TotalFee <= feeReceiptViewModel.collectedFeeAmt[i])
                {
                    _context.TblDueFee.Remove(Duefees);
                    _context.SaveChanges();
                }
                else
                {
                    Duefees.TotalFee = Duefees.TotalFee - feeReceiptViewModel.collectedFeeAmt[i];
                    Duefees.PaidAmount = Duefees.PaidAmount + feeReceiptViewModel.collectedFeeAmt[i];
                    Duefees.DueAmount = Duefees.TotalFee;

                    var currentUser = Session["Name"].ToString();
                    Duefees.UpdatedBy = currentUser;
                    _context.SaveChanges();
                }
            }


            return Json(tblfeereceipt, JsonRequestBehavior.AllowGet);
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

        public ActionResult TransportConfigureFeePlanes()
        {

            var pagename = "Transport Fee Configure Planes";

            var createpermission = "Create_permission";

            //List<FeePlans> allFeePlanes = _FeePlansRepository.GetAll().ToList();
            var allFeePlanes = _context.FeePlans.Where(x => x.FeeType_Id == 223).ToList();
            ViewBag.AllCourses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();

            ViewBag.FeeHeadings = new SelectList(_FeeHeadingsRepository.GetAll().ToList(), "FeeId", "FeeName");
            ViewBag.Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            ViewBag.Categories = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.TransportOptions = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Transport Options").DataListId.ToString()).ToList();
            ViewBag.KmDistance = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "KmDistance").DataListId.ToString()).ToList();

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;


            return View(allFeePlanes);
        }
        [HttpPost]
        public JsonResult TransportConfigureFeePlanes(FeePlanViewModel feePlan)
        {

            for (int i = 0; i < feePlan.categoryId.Count(); i++)
            {
                int categoryId = feePlan.categoryId[i];
                Tbl_DataListItem category = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == categoryId);

                for (int j = 0; j < feePlan.classesId.Count(); j++)
                {
                    int classId = feePlan.classesId[j];
                    Tbl_DataListItem classDetail = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == classId);
                    TransportFeePlans feePlanDetail = _TransportFeePlans.GetAll().FirstOrDefault(x => x.ClassId == feePlan.classesId[j] && x.CategoryId == feePlan.categoryId[i] && x.FeeId == feePlan.feeHeadingId && x.BatchName == feePlan.BatchName);
                    if (feePlanDetail != null)
                    {
                        // feePlanDetail.FeeValue = feePlan.feeValue;
                        _TransportFeePlans.Save();
                    }
                    else
                    {
                        TransportFeePlans feePlans = new TransportFeePlans()
                        {
                            FeeId = feePlan.feeHeadingId,
                            CategoryId = feePlan.categoryId[i],
                            CategoryName = category.DataListItemName,
                            ClassId = feePlan.classesId[j],
                            ClassName = classDetail.DataListItemName,
                            FeeName = feePlan.feeHeadingName,
                            // FeeValue = feePlan.feeValue,
                            BatchName = feePlan.BatchName
                        };
                        _TransportFeePlans.Insert(feePlans);
                        _TransportFeePlans.Save();
                    }
                }
            }
            return Json("Ok");
        }

        #region
        //public ActionResult AllTransportReceipts()
        //{
        //    //ViewBag.Students = new SelectList(_StudentRepository.GetAll().OrderBy(x => x.Name).ToList(),"StudentId","Name");
        //    //var currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        //    ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
        //    ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
        //    ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
        //    //ViewBag.Categorys = new SelectList(_StudentCategorysRepository.GetAll().OrderBy(x => x.CategoryName).ToList(), "CategoryName", "CategoryName");
        //    //var allFeeREceipts = _TblFeeReceiptsRepository.GetAll().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());
        //    var allFeeREceipts = _TblTransportFeeReceipts.GetAll();

        //    allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
        //    foreach (var item in allFeeREceipts.ToList())
        //    {
        //        string monthNames = string.Empty;
        //        if (Session["StudentId"] != null && Session["StudentId"].ToString().Trim() != "")
        //        {
        //            var rs = Session["StudentId"];
        //            int studid = Convert.ToInt32(Session["StudentId"]);
        //            var allFeeReceiptForOneTimeCreator = _TblTransportFeeReceipts.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator && x.StudentId == studid).ToList();
        //            foreach (var item2 in allFeeReceiptForOneTimeCreator)
        //            {
        //                monthNames = monthNames + item2.PaidMonths;
        //                if (allFeeReceiptForOneTimeCreator.Count() > 1)
        //                {
        //                    monthNames = monthNames + " | ";
        //                }
        //            }
        //            item.PaidMonths = monthNames;
        //        }
        //        else
        //        {
        //            var allFeeReceiptForOneTimeCreator = _TblTransportFeeReceipts.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator).ToList();
        //            foreach (var item2 in allFeeReceiptForOneTimeCreator)
        //            {
        //                monthNames = monthNames + item2.PaidMonths;
        //                if (allFeeReceiptForOneTimeCreator.Count() > 1)
        //                {
        //                    monthNames = monthNames + " | ";
        //                }
        //            }
        //            item.PaidMonths = monthNames;
        //        }
        //    }

        //    return View(allFeeREceipts.ToList());
        //}
        #endregion


        //Transport Fee REceipt
        #region Transport Fee Receipt

        public ActionResult AllTransportReceipts()
        {

            var pagename = "All Transport Receipts";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";

            //ViewBag.Students = new SelectList(_StudentRepository.GetAll().OrderBy(x => x.Name).ToList(),"StudentId","Name");
            //var currentDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            //ViewBag.Categorys = new SelectList(_StudentCategorysRepository.GetAll().OrderBy(x => x.CategoryName).ToList(), "CategoryName", "CategoryName");
            //var allFeeREceipts = _TblFeeReceiptsRepository.GetAll().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());
            //List<TblFeeReceipts> allFeeREceipts = new List<TblFeeReceipts>();
            //int studentid = Convert.ToInt32(Session["StudentId"]);
            var studentdata = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            if (Session["rolename"].ToString() == "Student")
            {
                int studentid = Convert.ToInt32(Session["StudentId"]);
                var AllFeeREceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentid && x.FeeHeadingIDs == "21").ToList();
                ViewBag.sessionlist = "Student";
                List<Tbl_Feereceiptsviewmodel> Tbl_Feereceiptsviewmodel = new List<Tbl_Feereceiptsviewmodel>();
                foreach (var item in AllFeeREceipts)
                {
                    if (item.FeeHeadingIDs == "21" && item.FeeHeadingIDs == "25" && item.FeeHeadingIDs == "21,25" && item.FeeHeadingIDs == "25,21")
                    {
                        Tbl_Feereceiptsviewmodel.Add(new Tbl_Feereceiptsviewmodel
                        {
                            StudentName = item.StudentName,
                            ClassName = item.ClassName,
                            PayHeadings = item.PayHeadings,
                            PaidMonths = item.PaidMonths,
                            ReceiptAmt = item.ReceiptAmt,
                            AddedDate = item.AddedDate,
                            FeeReceiptId = item.FeeReceiptId,
                            FeeReceiptsOneTimeCreator = item.FeeReceiptsOneTimeCreator,
                            Editpermission = CheckEditpermission(pagename, editpermission),
                            DeletePermission = CheckDeletepermission(pagename, deletepermission)
                        });
                    }
                }
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

                return View(Tbl_Feereceiptsviewmodel.ToList());

                //var   allFeeREceipts = _context.TblFeeReceipts.ToList();
                //allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();

            }
            else
            {
                var allFeeREceipts = _context.TblFeeReceipts.Where(x => x.FeeHeadingIDs == "21" || x.FeeHeadingIDs == "25" || x.FeeHeadingIDs == "21,25" || x.FeeHeadingIDs == "25,21").ToList();
                List<TblFeeReceipts> tblFees = new List<TblFeeReceipts>();
                List<Tbl_Feereceiptsviewmodel> Tbl_Feereceiptsviewmodel = new List<Tbl_Feereceiptsviewmodel>();
                //allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
                foreach (var item in allFeeREceipts)
                {
                    if (item.FeeHeadingIDs != null)
                    {
                        var sdata = studentdata.FirstOrDefault(x => x.StudentRegisterID == item.StudentId);
                        if (sdata != null)
                        {
                            item.StudentName = sdata.Name + " " + sdata.Last_Name;

                            Tbl_Feereceiptsviewmodel.Add(new Tbl_Feereceiptsviewmodel
                            {
                                StudentName = item.StudentName,
                                ClassName = item.ClassName,
                                PayHeadings = item.PayHeadings,
                                PaidMonths = item.PaidMonths,
                                ReceiptAmt = item.ReceiptAmt,
                                AddedDate = item.AddedDate,
                                FeeReceiptId = item.FeeReceiptId,
                                FeeReceiptsOneTimeCreator = item.FeeReceiptsOneTimeCreator,
                                Editpermission = CheckEditpermission(pagename, editpermission),
                                DeletePermission = CheckDeletepermission(pagename, deletepermission)
                            });

                        }
                    }
                }
                ViewBag.sessionlist = "Professor";
                return View(Tbl_Feereceiptsviewmodel);
            }


            //var  AllFeeREceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentid).ToList();

            //    if (Session["StudentId"] != null && Session["StudentId"].ToString().Trim() != "")
            //{

            //    allFeeREceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentid).ToList();
            //    allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
            //}
            //else {
            //    allFeeREceipts = _context.TblFeeReceipts.ToList();
            //    allFeeREceipts = allFeeREceipts.Where(x => Convert.ToString(x.AddedDate) == DateTime.Now.ToString("dd/MM/yyyy")).ToList();
            //}


            //foreach (var item in allFeeREceipts.ToList())
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
            //    else
            //    {
            //        var allFeeReceiptForOneTimeCreator = _TblFeeReceiptsRepository.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator).ToList();
            //        //foreach (var item2 in allFeeReceiptForOneTimeCreator)
            //        //{
            //        //    monthNames = monthNames + item2.PaidMonths;
            //        //    if (allFeeReceiptForOneTimeCreator.Count() > 1)
            //        //    {
            //        //        monthNames = monthNames + " | ";
            //        //    }
            //        //}
            //        //item.PaidMonths = monthNames;
            //    }
            //}

        }
        #endregion

        public JsonResult GetAllReceipts(string DDClass, DateTime ActualDate, DateTime DateFrom, DateTime DateTo)
        {
            var data = _context.TblFeeReceipts.ToList();
            var studentdetails = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            List<TblFeeReceipts> allFeeREceipts = new List<TblFeeReceipts>();
            foreach (var item in data)
            {
                if (item.FeeHeadingIDs == "21" || item.FeeHeadingIDs == "25" || item.FeeHeadingIDs == "21,25" || item.FeeHeadingIDs == "25,21")
                {
                    var sdata = studentdetails.FirstOrDefault(x => x.StudentRegisterID == item.StudentId);
                    if (sdata != null)
                    {
                        item.StudentName = sdata.Name + " " + sdata.Last_Name;
                        allFeeREceipts.Add(item);
                    }
                }
            }
            if (DDClass != null && DDClass != "")
            {
                allFeeREceipts = allFeeREceipts.Where(x => x.ClassName == DDClass).ToList();
            }

            if (ActualDate != null)
            {
                // DateTime actialdate = DateTime.ParseExact(ActualDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date1;
                allFeeREceipts = allFeeREceipts.Where(x => x.AddedDate == ActualDate).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) == DateTime.ParseExact(ActualDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }
            else if (DateFrom != null && DateTo != null)
            {

                // DateTime fromdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                //DateTime todate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date2;
                allFeeREceipts = allFeeREceipts.Where(x => x.AddedDate >= DateFrom && x.AddedDate <= DateTo).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }
            else if (DateFrom != null)
            {
                // DateTime fdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date3;
                allFeeREceipts = allFeeREceipts.Where(x => x.AddedDate >= DateFrom).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }
            else if (DateTo != null)
            {

                // DateTime tdate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date4;
                allFeeREceipts = allFeeREceipts.Where(x => x.AddedDate <= DateTo).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
            }

            //foreach (var item in allFeeREceipts.ToList())
            //{
            //    string monthNames = string.Empty;
            //    var allFeeReceiptForOneTimeCreator = _TblTransportFeeReceipts.GetAll().Where(x => x.FeeReceiptsOneTimeCreator == item.FeeReceiptsOneTimeCreator).ToList();
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

            return Json(allFeeREceipts, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteFeeReceiptbyId(string id)
        {
            var genralobj = _context.TblTransportFeeReceipts.Where(e => e.FeeReceiptsOneTimeCreator == id).ToList();
            foreach (var item in genralobj)
            {
                var dataDeleteed = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptId == item.FeeReceiptId);
                _context.TblFeeReceipts.Remove(dataDeleteed);
                _context.SaveChanges();
            }
            return Json("Record delete successfully", JsonRequestBehavior.AllowGet);

        }

        public ActionResult TransportReceiptPreview(int? id)
        {
            TblTransportFeeReceipts tblFeeReceipts = new TblTransportFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            if (id != null)
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblTransportFeeReceipts.GetById(id);
                int studentId = tblFeeReceipts.StudentId;
                ViewBag.ScollarNumber = _context.Students.FirstOrDefault(x => x.StudentId == studentId).StudentId;
                var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                ViewBag.studentDetails = studentDetails;
                //ViewBag.Total = tblFeeReceipts.TotalFee - tblFeeReceipts.ConcessionAmt + tblFeeReceipts.LateFee + tblFeeReceipts.OldBalance;
                ViewBag.Total = tblFeeReceipts.ReceiptAmt;

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
            return View(tblFeeReceipts);
        }

        public ActionResult TransportFeePaid()
        {
            ViewBag.Message = TempData["message"];
            string studentId = Convert.ToString(Session["StudentId"]);
            //var student = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == studentId);
            var student = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID.ToString() == studentId);
            var AllCategorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            student.Class = Classes.FirstOrDefault(x => x.DataListItemId == student.Class_Id)?.DataListItemName;
            student.Category = AllCategorys.FirstOrDefault(x => x.DataListItemId == student.Category_Id)?.DataListItemName;
            ViewBag.Transportrange = _context.TblTransportReducedAmount.ToList();

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


        #region Transport Report

        public ActionResult AllTransportReport()
        {
            var allStudents = _context.StudentsRegistrations.Where(x => x.IsApprove != 217).ToList();

            var lst = _context.FeeHeadings.Where(x => x.FeeId == 21).ToList();
            List<FeeHeader> HeaderList = new List<FeeHeader>();
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
                    Text = item.Name,
                    Value = Convert.ToString(item.StudentRegisterID)
                });
            }
            ViewBag.AllStudentName = studentNameList;
            ViewBag.FeeHeadings = HeaderList;
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Categorys = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            ViewBag.BatcheNames = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var allFeeReceipts = _context.TblFeeReceipts.ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();
            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate == DateTime.Now).ToList();
            List<TodayFeeReportViewModel> allReport = new List<TodayFeeReportViewModel>();
            int i = 1;
            foreach (var item in allFeeReceipts)
            {
                if (item.FeeHeadingIDs == "21")
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
        public class FeeHeader
        {
            public string FeeId { get; set; }
            public string FeeName { get; set; }
        }
        #endregion


        #region transportkm

        public ActionResult TransportKm()
        {
            return View();
        }

        #endregion


        public ActionResult TransportRange()
        {
            ViewBag.Transportrange = _context.TblTransportReducedAmount.ToList();
            return View();
        }

        public ActionResult SaveTransportRange(TblTransportReducedAmount tblTransportReducedAmount)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            try
            {
                _context.TblTransportReducedAmount.Add(tblTransportReducedAmount);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javasctipt'>location.replace('/TransportFee/TransportRange')</sctipt>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

    }



}