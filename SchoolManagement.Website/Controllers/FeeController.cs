using DocumentFormat.OpenXml.Bibliography;
using EmployeeManagement.Repository;
using Microsoft.Extensions.Primitives;
using Rotativa;
using SchoolManagement.Data.Constants;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.EnumData;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;




namespace SchoolManagement.Website.Controllers
{
    [Authorize]
    public class FeeController : Controller
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

        public FeeController()
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

        [HttpPost]
        public JsonResult AddFeeHeading(FeeHeadings feeHeadings, int Feetype)
        {
            FeeHeadings data = _FeeHeadingsRepository.GetAll().FirstOrDefault(x => x.FeeName == feeHeadings.FeeName);

            if (feeHeadings != null && data == null)
            {
                if (Feetype == 1)
                {
                    feeHeadings.FeeType_Id = 222;
                }
                else if (Feetype == 2)
                {
                    feeHeadings.FeeType_Id = 223;
                }
                else
                {
                    feeHeadings.FeeType_Id = 243;
                }
                _FeeHeadingsRepository.Insert(feeHeadings);
                _FeeHeadingsRepository.Save();
                return Json("Ok");
            }
            return Json("NotOk");
        }

        public JsonResult GetFrequencyMessage(string name)
        {
            string frequencyMessage = FrequencyEnum.GetMessageOfFrequencyEnum(name);
            return Json(frequencyMessage, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult ConfigureFeePlanes(FeePlanViewModel feePlan, int Fee_Id)//Fee_Id = 1 Academy, Fee_Id = 2 Transport
        {
            {
                var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
                if (Fee_Id == 1)
                {
                    //for (int i = 0; i < feePlan.categoryId.Count(); i++)
                    {
                        //int categoryId = feePlan.categoryId[i];
                        //Tbl_DataListItem category = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == categoryId);

                        for (int j = 0; j < feePlan.classesId.Count(); j++)
                        {
                            int classId = feePlan.classesId[j];
                            Tbl_DataListItem classDetail = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == classId);
                            FeePlans feePlanDetail = _FeePlansRepository.GetAll().FirstOrDefault(x => x.ClassId == feePlan.classesId[j] && x.FeeId == feePlan.feeHeadingId);
                            if (feePlanDetail != null)
                            {
                                feePlanDetail.FeeValue = feePlan.feeValue;
                                feePlanDetail.FeeType_Id = 222;
                                feePlanDetail.FeeConfigurationname = activeschool.Fee_Configuratinname;
                                feePlanDetail.Fee_configurationid = activeschool.Fee_configurationId;
                                feePlanDetail.Batch_Id = feePlan.Batch_Id;
                                feePlanDetail.Medium = feePlan.Medium;
                                _FeePlansRepository.Save();
                            }
                            else
                            {
                                //Fee_ID = 1 Academy Fee
                                //Fee_ID = 2 Transport Fee
                                FeePlans feePlans = new FeePlans();
                                feePlans.FeeId = feePlan.feeHeadingId;
                                //CategoryId = feePlan.categoryId[i],
                                //CategoryName = category.DataListItemName,
                                feePlans.ClassId = feePlan.classesId[j];
                                feePlans.ClassName = classDetail.DataListItemName;
                                feePlans.FeeName = feePlan.feeHeadingName;
                                feePlans.FeeValue = feePlan.feeValue;
                                feePlans.Batch_Id = feePlan.Batch_Id;
                                feePlans.FeeType_Id = 222;
                                feePlans.Medium = feePlan.Medium;
                                //BatchName = feePlan.BatchName
                                _FeePlansRepository.Insert(feePlans);
                                _FeePlansRepository.Save();

                            }
                        }
                    }
                }
                else
                {
                    FeePlans feeplans = new FeePlans()
                    {
                        FeeId = feePlan.feeHeadingId,
                        FeeName = feePlan.feeHeadingName,
                        FeeValue = feePlan.feeValue,
                        TransportOpt_Id = feePlan.Transportopt_Id,
                        KmDistance_Id = feePlan.KmDistance_Id,
                        FeeType_Id = 223
                    };
                    _FeePlansRepository.Insert(feeplans);
                    _FeePlansRepository.Save();
                }

            }


            return Json("Ok");
        }


        [HttpPost]
        public JsonResult AdmissionFeePlanes(FeePlanViewModel feePlan, int Fee_Id)//Fee_Id = 1 Academy, Fee_Id = 2 Transport
        {
            {
                var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
                if (Fee_Id == 1)
                {
                    //for (int i = 0; i < feePlan.categoryId.Count(); i++)
                    {
                        //int categoryId = feePlan.categoryId[i];
                        //Tbl_DataListItem category = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == categoryId);

                        for (int j = 0; j < feePlan.classesId.Count(); j++)
                        {
                            int classId = feePlan.classesId[j];
                            Tbl_DataListItem classDetail = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == classId);
                            FeePlans feePlanDetail = _FeePlansRepository.GetAll().FirstOrDefault(x => x.ClassId == feePlan.classesId[j] && x.FeeId == feePlan.feeHeadingId);
                            if (feePlanDetail != null)
                            {
                                feePlanDetail.FeeValue = feePlan.feeValue;
                                feePlanDetail.FeeType_Id = 243;
                                feePlanDetail.FeeConfigurationname = activeschool.Fee_Configuratinname;
                                feePlanDetail.Fee_configurationid = activeschool.Fee_configurationId;
                                feePlanDetail.Batch_Id = feePlan.Batch_Id;
                                _FeePlansRepository.Save();
                            }
                            else
                            {
                                //Fee_ID = 1 Academy Fee
                                //Fee_ID = 2 Transport Fee
                                FeePlans feePlans = new FeePlans()
                                {
                                    FeeId = feePlan.feeHeadingId,
                                    //CategoryId = feePlan.categoryId[i],
                                    //CategoryName = category.DataListItemName,
                                    ClassId = feePlan.classesId[j],
                                    ClassName = classDetail.DataListItemName,
                                    FeeName = feePlan.feeHeadingName,
                                    FeeValue = feePlan.feeValue,
                                    Batch_Id = feePlan.Batch_Id,
                                    FeeType_Id = 243,
                                    Jan = feePlan.Jan,
                                    Feb = feePlan.Feb,
                                    Mar = feePlan.Mar,
                                    Apr = feePlan.Apr,
                                    May = feePlan.Apr,
                                    Jun = feePlan.Jun,
                                    Jul = feePlan.Jul,
                                    Aug = feePlan.Aug,
                                    Sep = feePlan.Sep,
                                    Oct = feePlan.Oct,
                                    Nov = feePlan.Nov,
                                    Dec = feePlan.Dec,
                                    FeeConfigurationname = activeschool.Fee_Configuratinname,
                                    Fee_configurationid = activeschool.Fee_configurationId
                                    //BatchName = feePlan.BatchName
                                };
                                _FeePlansRepository.Insert(feePlans);
                                _FeePlansRepository.Save();
                            }
                        }
                    }
                }


            }


            return Json("Ok");
        }


        public JsonResult GetStudentFees(string ApplicationNumber)
        {
            int studentId = _context.Students.FirstOrDefault(x => x.ApplicationNumber == ApplicationNumber.ToString()).StudentId; //xrnik

            ConfigFeeDataViewModel configFeeDataViewModel = new ConfigFeeDataViewModel();

            var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");

            //var Batchdetails = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Batch").DataListId.ToString()).ToList();

            //var activebatch = Batchdetails.FirstOrDefault(x => x.Status == "Active");
            var activeBatches = _context.Tbl_Batches.Where(x => x.IsActiveForPayments).Select(x => x.Batch_Id).ToList();

            var feeheading = _context.FeeHeadings.ToList();

            //var feeplansdetails = _context.FeePlans.Where(x =>  x.Batch_Id == activebatch.DataListItemId ).ToList();
            var feeplansdetails = _context.FeePlans.Where(x => activeBatches.Contains(x.Batch_Id)).ToList();

            //string[] months = Months.Split(',');
            List<FeePlans> feePlanes = new List<FeePlans>();
            System.Data.Entity.DbSet<FeePlans> allFeePlanes = _context.FeePlans;
            System.Data.Entity.DbSet<FeeHeadings> allFeeHeading = _context.FeeHeadings;
            Student student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            var studentBatch = (from s in _context.Students
                                join b in _context.Tbl_Batches on s.Batch_Id equals b.Batch_Id
                                where s.StudentId == student.StudentId
                                select new
                                {
                                    BatchName = b.Batch_Name
                                }).FirstOrDefault();
            var studentdata = _context.Students.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            {
                var classname = Classes.FirstOrDefault(x => x.DataListItemId == studentdata.Class_Id)?.DataListItemName;
                /*feePlanes = feeplansdetails.Where(x => x.ClassName == classname && x.FeeType_Id == 222 && x.Medium.Contains(studentdata.Medium)).ToList();*/

                feePlanes = (from fd in feeplansdetails
                             join b in _context.Tbl_Batches on fd.Batch_Id equals b.Batch_Id
                             join c in _context.Students on b.Batch_Id equals c.Batch_Id
                             //let batchYear = int.TryParse(b.Batch_Name?.Split('-').FirstOrDefault(), out int year) ? year : 0
                             where fd.ClassName == classname
                                && fd.FeeType_Id == 222
                                && fd.Medium.Contains(studentdata.Medium)
                                && c.Batch_Id == b.Batch_Id

                             //&& c.CurrentYear == fd.Batch_Id
                             //&& ((c.CurrentYear != c.Batch_Id && fd.FeeName != "New Admission") ||
                             //(c.CurrentYear == c.Batch_Id && fd.FeeName != "Re-Admission")
                             //)

                             //&& (
                             //    (batchYear == DateTime.Now.Year && fd.FeeName != "Re-Admission") ||      // Case 1: Current Year
                             //    (batchYear > DateTime.Now.Year && fd.FeeName != "Re-Admission") ||      // Case 2: Greater than Current Year
                             //    (batchYear < DateTime.Now.Year && fd.FeeName == "Re-Admission")
                             //   )
                             select fd).Distinct().ToList();

                //feePlanes = (from fd in _context.FeePlans
                //                join b in _context.Tbl_Batches on fd.Batch_Id equals b.Batch_Id
                //                join c in _context.Students on b.Batch_Id equals c.CurrentYear
                //                where fd.ClassName == classname
                //&& fd.FeeType_Id == 222
                //&& (fd.Medium.Contains(studentdata.Medium) || string.IsNullOrEmpty(studentdata.Medium))  // Handle possible null or empty values for medium
                //&& c.CurrentYear == b.Batch_Id
                //                select fd).Distinct().ToList();

            }


            //check Monthly fee

            var allFeePlaness = feePlanes;
            var TotalFeeceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentdata.StudentId).ToList();
            var feereceipt = TotalFeeceipt.Select(x => x.FeeHeadingIDs).ToList();
            List<FeePlans> feeplanlist = new List<FeePlans>();
            foreach (var itm in TotalFeeceipt)
            {
                if (itm.FeeHeadingIDs.Contains("22"))
                {
                    foreach (var feelis in feePlanes)
                    {
                        if (feelis.FeeId != 26 && feelis.FeeId != 27)
                        {
                            feeplanlist.Add(feelis);
                        }
                    }
                    break;
                }
            }
            if (feeplanlist.Count > 0)
            {
                feePlanes = feeplanlist;
            }



            StringBuilder html = new StringBuilder();


            //html Body
            float allTotal = 0;
            int SNO = 1;
            foreach (FeePlans feeplan in feePlanes)
            {

                if (feeplan.FeeId != 22)
                {
                    float total = 0;
                    FeeHeadings FeeHeading = allFeeHeading.FirstOrDefault(x => x.FeeId == feeplan.FeeId);
                    StringBuilder htmlRow = new StringBuilder();

                    List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
                    //var studentdetails = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
                    if (TotalFeeceipt.Count > 0)
                    {
                        foreach (var item in TotalFeeceipt)
                        {
                            if (item.FeeHeadingIDs == feeplan.FeeId.ToString())
                            {
                                tblFeeReceipts.Add(item);
                            }
                            else
                            {
                                var feeid = item.FeeHeadingIDs.Split(',');
                                foreach (var fee in feeid)
                                {
                                    if (feeplan.FeeId.ToString() == fee)
                                    {
                                        tblFeeReceipts.Add(item);
                                    }
                                }
                            }

                        }
                    }

                    var tblFeeReceipt = TotalFeeceipt.Where(x => x.FeeHeadingIDs == (feeplan.FeeId).ToString()).ToList();
                    var totalAmt = tblFeeReceipts.Any() ? tblFeeReceipts.FirstOrDefault().TotalFee : 0;
                    var receiptAmt = tblFeeReceipts.Any() ? tblFeeReceipts.Sum(x => string.IsNullOrEmpty(x.PaidAmount) ? 0 : Convert.ToInt64(x.PaidAmount)) : 0;
                    // if (totalAmt < receiptAmt)
                    bool hasComputerElective = _context.tbl_Student_ElectiveRecord.Any(x => x.StudentId == studentdata.StudentId && x.ElectiveSubjectId == 77);

                    if (receiptAmt < feeplan.FeeValue)
                    {
                        if (feeplan.FeeName == "Computer Fee" && !hasComputerElective)
                        {
                            continue; // skip this fee
                        }

                        //                   if (((studentdata.CurrentYear == studentdata.Batch_Id && feeplan.FeeName == "New Admission") ||
                        //(studentdata.CurrentYear != studentdata.Batch_Id && feeplan.FeeName == "Re-Admission")))

                        if (!((studentdata.CurrentYear == studentdata.Batch_Id && feeplan.FeeName == "Re-Admission") || (studentdata.CurrentYear != studentdata.Batch_Id && feeplan.FeeName == "New Admission")))
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

                            if (tblFeeReceipt.Count() != 0)
                            {
                                if (totalAmt <= receiptAmt)
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



                            allTotal = allTotal + total;

                            //  var receiptAmt = tblFeeReceipt.Count == 0 ? 0 : tblFeeReceipt.Sum(x => x.ReceiptAmt);
                            htmlRow.Append("<td>");
                            htmlRow.Append("<input type='number' value='" + (total - receiptAmt) + "' class='collectFeeNumber'/>");
                            //htmlRow.Append(total - receiptAmt);
                            htmlRow.Append("</td>");


                            htmlRow.Append("</tr>");

                            if (total > 0)
                            {
                                html.Append(htmlRow);
                            }
                        }
                    }
                }
            }



            html.Append("<tr>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");


            html.Append("<td>");
            html.Append("<b><span>Total Amount : </span></b></br><b><span><input type='button' class='btn btn-success btn-sm' value='Calculate Fee' id='verifyBtn'/> </span></b>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("<b><span id='allTotal'></span></b></br>");
            html.Append("</td>");
            html.Append("</tr>");
            configFeeDataViewModel.ConfigureFeeData = html.ToString();

            return Json(configFeeDataViewModel, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIndivualStudentFees(int studentId)
        {
            //studentId = (int)_context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentId.ToString()).StudentRegisterID;  //xrnik1
            List<StudentFeePaidViewModel> studentFeePaidlist = new List<StudentFeePaidViewModel>();


            var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");

            //var Batchdetails = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Batch").DataListId.ToString()).ToList();

            //var activebatch = Batchdetails.FirstOrDefault(x => x.Status == "Active");
            var activeBatches = _context.Tbl_Batches.Where(x => x.IsActiveForPayments).Select(x => x.Batch_Id).ToList();

            var feeheading = _context.FeeHeadings.ToList();

            //var feeplansdetails = _context.FeePlans.Where(x =>  x.Batch_Id == activebatch.DataListItemId ).ToList();
            var feeplansdetails = _context.FeePlans.Where(x => activeBatches.Contains(x.Batch_Id)).ToList();

            //string[] months = Months.Split(',');
            List<FeePlans> feePlanes = new List<FeePlans>();
            System.Data.Entity.DbSet<FeePlans> allFeePlanes = _context.FeePlans;
            System.Data.Entity.DbSet<FeeHeadings> allFeeHeading = _context.FeeHeadings;
            //var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            {
                var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                var classname = Classes.FirstOrDefault(x => x.DataListItemId == studentdata.Class_Id)?.DataListItemName;
                feePlanes = feeplansdetails.Where(x => x.ClassName == classname && x.FeeType_Id == 222 && x.Medium == studentdata.Medium).ToList();
            }


            //check Monthly fee

            var allFeePlaness = feePlanes;
            var TotalFeeceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
            var feereceipt = TotalFeeceipt.Select(x => x.FeeHeadingIDs).ToList();
            List<FeePlans> feeplanlist = new List<FeePlans>();
            foreach (var itm in TotalFeeceipt)
            {
                if (itm.FeeHeadingIDs.Contains("22"))
                {
                    foreach (var feelis in feePlanes)
                    {
                        if (feelis.FeeId != 26 && feelis.FeeId != 27)
                        {
                            feeplanlist.Add(feelis);
                        }
                    }
                    break;
                }
            }
            if (feeplanlist.Count > 0)
            {
                feePlanes = feeplanlist;
            }

            StringBuilder html = new StringBuilder();


            //html Body
            float allTotal = 0;
            int SNO = 1;
            foreach (FeePlans feeplan in feePlanes)
            {
                StudentFeePaidViewModel temp = new StudentFeePaidViewModel();
                if (feeplan.FeeId != 22)
                {
                    float total = 0;
                    FeeHeadings FeeHeading = allFeeHeading.FirstOrDefault(x => x.FeeId == feeplan.FeeId);
                    StringBuilder htmlRow = new StringBuilder();

                    List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
                    //var studentdetails = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
                    if (TotalFeeceipt.Count > 0)
                    {
                        foreach (var item in TotalFeeceipt)
                        {
                            if (item.FeeHeadingIDs == feeplan.FeeId.ToString())
                            {
                                tblFeeReceipts.Add(item);
                            }
                            else
                            {
                                var feeid = item.FeeHeadingIDs.Split(',');
                                foreach (var fee in feeid)
                                {
                                    if (feeplan.FeeId.ToString() == fee)
                                    {
                                        tblFeeReceipts.Add(item);
                                    }
                                }
                            }

                        }
                    }

                    var tblFeeReceipt = TotalFeeceipt.Where(x => x.FeeHeadingIDs == (feeplan.FeeId).ToString()).ToList();
                    var totalAmt = tblFeeReceipts.Any() ? tblFeeReceipts.FirstOrDefault().TotalFee : 0;
                    var receiptAmt = tblFeeReceipts.Any() ? tblFeeReceipts.Sum(x => string.IsNullOrEmpty(x.PaidAmount) ? 0 : Convert.ToInt64(x.PaidAmount)) : 0;
                    // if (totalAmt < receiptAmt)

                    if (receiptAmt < feeplan.FeeValue)
                    {
                        temp.FeeHeading = feeplan.FeeName;

                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (totalAmt <= receiptAmt)
                            {
                                temp.Amount = 0;
                            }
                            else
                            {
                                var paidAmount = tblFeeReceipt.Sum(x => Convert.ToInt64(x.PaidAmount));
                                var currentAmount = feeplan.FeeValue - paidAmount;
                                total = total + feeplan.FeeValue;
                                temp.Amount = currentAmount;
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            temp.Amount = feeplan.FeeValue;
                        }



                        allTotal = allTotal + total;


                        if (total > 0)
                        {
                            studentFeePaidlist.Add(temp);
                        }
                    }
                }
            }

            return Json(studentFeePaidlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStudentAdmissionFees(int studentId)
        {
            //studentId = (int)_context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentId.ToString()).StudentRegisterID;  //xrnik1
            ConfigFeeDataViewModel configFeeDataViewModel = new ConfigFeeDataViewModel();
            //string[] months = Months.Split(',');
            List<FeePlans> feePlanes = new List<FeePlans>();
            System.Data.Entity.DbSet<FeePlans> allFeePlanes = _context.FeePlans;
            System.Data.Entity.DbSet<FeeHeadings> allFeeHeading = _context.FeeHeadings;
            //var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);

            var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");


            var Batchdetails = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Batch").DataListId.ToString()).ToList();

            var activebatch = Batchdetails.FirstOrDefault(x => x.Status == "Active");


            var feeplansdetails = _context.FeePlans.Where(x => x.Batch_Id == activebatch.DataListItemId).ToList();


            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            {
                var studentdata = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                var classname = Classes.FirstOrDefault(x => x.DataListItemId == studentdata.Class_Id)?.DataListItemName;
                feePlanes = feeplansdetails.Where(x => x.ClassName == classname && x.FeeType_Id == 243).ToList();
            }

            var allFeePlaness = feePlanes;
            var TotalFeeceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
            var feereceipt = TotalFeeceipt.Select(x => x.FeeHeadingIDs).ToList();
            List<FeePlans> feeplanlist = new List<FeePlans>();
            foreach (var itm in TotalFeeceipt)
            {
                if (itm.FeeHeadingIDs.Contains("22"))
                {
                    foreach (var feelis in feePlanes)
                    {
                        if (feelis.FeeId != 26 && feelis.FeeId != 27)
                        {
                            feeplanlist.Add(feelis);
                        }
                    }
                    break;
                }
            }
            if (feeplanlist.Count > 0)
            {
                feePlanes = feeplanlist;
            }


            StringBuilder html = new StringBuilder();


            //html Body
            float allTotal = 0;
            int SNO = 1;
            foreach (FeePlans feeplan in feePlanes)
            {
                if (feeplan.FeeId != 22)
                {
                    float total = 0;
                    FeeHeadings FeeHeading = allFeeHeading.FirstOrDefault(x => x.FeeId == feeplan.FeeId);
                    StringBuilder htmlRow = new StringBuilder();

                    List<TblFeeReceipts> tblFeeReceipts = new List<TblFeeReceipts>();
                    //var studentdetails = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
                    if (TotalFeeceipt.Count > 0)
                    {
                        foreach (var item in TotalFeeceipt)
                        {
                            if (item.FeeHeadingIDs == feeplan.FeeId.ToString())
                            {
                                tblFeeReceipts.Add(item);
                            }
                            else
                            {
                                var feeid = item.FeeHeadingIDs.Split(',');
                                foreach (var fee in feeid)
                                {
                                    if (feeplan.FeeId.ToString() == fee)
                                    {
                                        tblFeeReceipts.Add(item);
                                    }
                                }
                            }

                        }
                    }

                    var tblFeeReceipt = TotalFeeceipt.Where(x => x.FeeHeadingIDs == (feeplan.FeeId).ToString()).ToList();
                    var totalAmt = tblFeeReceipts.Any() ? tblFeeReceipts.FirstOrDefault().TotalFee : 0;
                    var receiptAmt = tblFeeReceipts.Any() ? tblFeeReceipts.Sum(x => string.IsNullOrEmpty(x.PaidAmount) ? 0 : Convert.ToInt64(x.PaidAmount)) : 0;
                    // if (totalAmt < receiptAmt)

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

                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (totalAmt <= receiptAmt)
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



                        allTotal = allTotal + total;

                        //  var receiptAmt = tblFeeReceipt.Count == 0 ? 0 : tblFeeReceipt.Sum(x => x.ReceiptAmt);
                        htmlRow.Append("<td>");
                        htmlRow.Append("<input type='number' value='" + (total - receiptAmt) + "' class='collectFeeNumber'/>");
                        //htmlRow.Append(total - receiptAmt);
                        htmlRow.Append("</td>");

                        htmlRow.Append("</tr>");

                        if (total > 0)
                        {
                            html.Append(htmlRow);
                        }
                    }
                }
            }



            html.Append("<tr>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("</td>");


            html.Append("<td>");
            html.Append("<b><span>Total Amount : </span></b></br><b><span><input type='button' class='btn btn-success btn-sm' value='Calculate Fee' id='verifyBtn'/> </span></b>");
            html.Append("</td>");
            html.Append("<td>");
            html.Append("<b><span id='allTotal'>0</span></b></br>");
            html.Append("</td>");
            html.Append("</tr>");
            configFeeDataViewModel.ConfigureFeeData = html.ToString();

            return Json(configFeeDataViewModel, JsonRequestBehavior.AllowGet);
        }

        //--xrnik---
        public JsonResult GetStudentIDByApplicationNumber(string ApplicationNumber)
        {
            int StudentId = _context.Students.FirstOrDefault(x => x.ApplicationNumber == ApplicationNumber).StudentId;

            return Json(StudentId, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetStudentRegIDByApplicationNumber(string ApplicationNumber)
        {
            long StudentId = _context.Students.FirstOrDefault(x => x.ApplicationNumber == ApplicationNumber).StudentId;

            return Json(StudentId, JsonRequestBehavior.AllowGet);
        }
        //---

        /// <summary>
        /// Add due fees
        /// </summary>
        /// <param name="feeReceiptViewModel"></param>
        /// <returns></returns>
        public JsonResult AddDueFee(DueFeeModel dueFeeModel)
        {
            var currentUser = Session["Name"].ToString();
            var studentregistration = (dynamic)null;
            //var studentData = _context.Students.FirstOrDefault(x => x.StudentId == dueFeeModel.StudentId);
            Student student = _context.Students.FirstOrDefault(x => x.StudentId == dueFeeModel.StudentId);
            if (student != null)
            {
                studentregistration = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
            }
            else
            {
                studentregistration = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == dueFeeModel.ApplicationId);
            }
            IEnumerable<TblDueFee> dueFeesData;
            {
                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

                var classname = Classes.FirstOrDefault(x => x.DataListItemId == studentregistration.Class_Id)?.DataListItemName;
                dueFeesData = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == studentregistration.StudentRegisterID && x.ClassName == classname).ToList();
            }
            {
                //string feeMonths = string.Join(",", dueFeeModel.Selectedmonths);
                string FeeHeadingAmt = string.Join(",", dueFeeModel.FeeHeadingAmt);
                float[] FeeHeadingAmtValues = dueFeeModel.FeeHeadingAmt;
                string feeHeadings = string.Join(",", dueFeeModel.FeeHeadings);
                //float[] collectionfee = dueFeeModel.collectFees;

                var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId.ToString() == dueFeeModel.ClassName);
                var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName == dueFeeModel.CategoryName.ToLower());
                List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();
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
                    FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == item);
                    dueFee.FeeHeadingId = feeHeading == null ? 0 : feeHeading.FeeId;
                    dueFee.FeeHeading = feeHeading == null ? "DueBalance" : feeHeading.FeeName;
                    //dueFee.CategoryId = categoryDetail.DataListItemId;
                    //dueFee.CategoryName = categoryDetail.DataListItemName;
                    if (classDetail != null)
                    {
                        dueFee.ClassId = classDetail.DataListItemId;
                        dueFee.ClassName = classDetail.DataListItemName;
                    }
                    else
                    {
                        dueFee.ClassId = studentregistration.Class_Id;
                        dueFee.ClassName = dueFeeModel.ClassName;

                    }
                    //dueFee.PaidMonths = feeHeading == null ? "" : PaidMonthList(feeHeading, dueFeeModel.Selectedmonths);
                    dueFee.PayHeadings = feeHeadings;
                    dueFee.StudentId = dueFeeModel.StudentId;
                    dueFee.StudentName = dueFeeModel.StudentName;
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


            IEnumerable<TblDueFee> feeReceipt1 = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == studentregistration.StudentRegisterID).ToList();
            if (feeReceipt1.Any())
            {
                var sum = feeReceipt1.Sum(x => x.TotalFee);
                return Json(sum);
            }
            return Json("");
        }

        public async Task<JsonResult> SaveTcDetails(TcDetailsViewModel tcDetailsViewModel)
        {
            //tcDetailsViewModel.StudentId = (int)_context.Students.FirstOrDefault(x => x.ApplicationNumber == tcDetailsViewModel.StudentId.ToString()).StudentId;  //xrnik1
            string msg = String.Empty;
            try
            {
                if (tcDetailsViewModel.StudentTcDetailsId == 0)
                {
                    tcDetailsViewModel.StudentTcDetailsId = null;
                }
                TcFeeDetails tcFeeDetails = new TcFeeDetails()
                {
                    StudentId = tcDetailsViewModel.StudentId,
                    StudentTcDetailsId = tcDetailsViewModel.StudentTcDetailsId,
                    ReceiptAmount = tcDetailsViewModel.ReceiptAmount,
                    PaymentMode = tcDetailsViewModel.PaymentMode,
                    CreatedOn = DateTime.Now,
                    PaidDate = tcDetailsViewModel.CreatedOn == null ? DateTime.Now : tcDetailsViewModel.CreatedOn,
                    IsTcfee = tcDetailsViewModel.StudentTcDetailsId == null ? false : true
                };
                _context.TcFeeDetails.Add(tcFeeDetails);

                if (tcDetailsViewModel.StudentTcDetailsId == null)
                {
                    var student = await _context.Students.Where(x => x.StudentId == tcDetailsViewModel.StudentId).FirstOrDefaultAsync();
                    student.IsAdmissionPaid = true;
                }
                else
                {
                    var studentTcDetails = await _context.Tbl_StudentTcDetails.Where(x => x.Id == tcDetailsViewModel.StudentTcDetailsId && x.StudentId == tcDetailsViewModel.StudentId).FirstOrDefaultAsync();
                    studentTcDetails.Ispaid = true;
                }

                await _context.SaveChangesAsync();
                msg = "1";
            }
            catch (Exception
            )
            {
                msg = "0";
            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddFeeReceipt(FeeReceiptViewModel feeReceiptViewModel)
        {
            //string feeMonths = string.Join(",", feeReceiptViewModel.Selectedmonths);
            string feePaids = string.Join(",", feeReceiptViewModel.FeeHeadingAmt);
            string feeHeadings = string.Join(",", feeReceiptViewModel.FeeHeadings);
            string FeeHeadingsId = string.Empty;
            var feeheadings = _context.FeeHeadings.ToList();
            var feeplans = _context.FeePlans.ToList();
            Student studentregistrations;
            //var tblstudent = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);
            {
                studentregistrations = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);
            }

            string headingid = string.Empty;
            var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId.ToString() == feeReceiptViewModel.ClassName.ToLower());
            float totalfeeamount = 0;
            //float fullfeeamount = 0;
            string paidmonths = string.Empty;
            string Pmonths = string.Empty;
            string feetypeid = string.Empty;
            string batchname = string.Empty;

            for (int k = 0; k < feeReceiptViewModel.FeeHeadings.Count(); k++)
            {
                var feeheadingid = feeheadings.FirstOrDefault(x => x.FeeName == feeReceiptViewModel.FeeHeadings[k]);
                var feeplan = feeplans.FirstOrDefault(x => x.FeeId == feeheadingid.FeeId && x.ClassId == classDetail.DataListItemId);
                if (feeplan != null)
                {
                    feetypeid = feeplan.FeeType_Id.ToString();
                    batchname = feeplan.Batch_Id.ToString();
                }
                totalfeeamount = totalfeeamount + feeplan.FeeValue;
                headingid = string.Join(",", headingid, feeheadingid.FeeId);
                //Pmonths =  string.Join(",",Pmonths, paidmonths);
            }
            string feeheading_id = headingid.Remove(0, 1);

            //var ad = Pmonths.TrimStart(',');
            //var totalfeeplan = feeplans.Where(x => x.ClassId == classDetail.DataListItemId).ToList();
            //foreach(var item in totalfeeplan)
            //{
            //    fullfeeamount = fullfeeamount + item.FeeValue;
            //}
            //float totaldueamount = fullfeeamount - totalfeeamount;
            var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == feeReceiptViewModel.CategoryName.ToLower());


            var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");


            List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();

            var unicNumber = Guid.NewGuid();
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            tblFeeReceipts.FeeHeadingIDs = feeheading_id;
            tblFeeReceipts.BalanceAmt = feeReceiptViewModel.BalanceAmt;
            tblFeeReceipts.OldBalance = feeReceiptViewModel.BalanceAmt;
            tblFeeReceipts.BankName = feeReceiptViewModel.BankName;
            tblFeeReceipts.Type = feetypeid;//Fee Type
            tblFeeReceipts.BatchName = batchname;
            //tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
            //tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
            tblFeeReceipts.ClassId = classDetail.DataListItemId;
            tblFeeReceipts.ClassName = classDetail.DataListItemName;
            //tblFeeReceipts.BatchName = _context.Students.Where(x => x.StudentId == feeReceiptViewModel.StudentId).Select(x => x.BatchName).FirstOrDefault();
            //tblFeeReceipts.FeePaidDate = feeReceiptViewModel.DateTimeVal != null ? feeReceiptViewModel.DateTimeVal : DateTime.Now;
            tblFeeReceipts.AddedDate = DateTime.Now;
            tblFeeReceipts.ModifiedDate = DateTime.Now;
            tblFeeReceipts.Concession = feeReceiptViewModel.Concession;
            tblFeeReceipts.ConcessionAmt = feeReceiptViewModel.ConcessionAmt;
            tblFeeReceipts.LateFee = feeReceiptViewModel.LateFee;
            //tblFeeReceipts.PaidMonths = ad;
            tblFeeReceipts.PayHeadings = feeHeadings;
            tblFeeReceipts.PaymentMode = feeReceiptViewModel.PaymentMode;
            tblFeeReceipts.ReceiptAmt = feeReceiptViewModel.ReceiptAmt;
            tblFeeReceipts.Remark = feeReceiptViewModel.Remark;
            tblFeeReceipts.StudentId = Convert.ToInt32(studentregistrations.StudentId);
            tblFeeReceipts.StudentName = feeReceiptViewModel.StudentName;
            tblFeeReceipts.TotalFee = feeReceiptViewModel.TotalFee;
            tblFeeReceipts.FeePaids = feePaids;
            tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
            tblFeeReceipts.DueAmount = "0";
            tblFeeReceipts.PaidAmount = feeReceiptViewModel.ReceiptAmt.ToString();
            //tblFeeReceipts.Jan = Jan;
            //tblFeeReceipts.Feb = Feb;
            //tblFeeReceipts.Mar = Mar;
            //tblFeeReceipts.Apr = Apr;
            //tblFeeReceipts.May = May;
            //tblFeeReceipts.Jun = Jun;
            //tblFeeReceipts.Jul = Jul;
            //tblFeeReceipts.Aug = Aug;
            //tblFeeReceipts.Sep = Sep;
            //tblFeeReceipts.Oct = Oct;
            //tblFeeReceipts.Nov = Nov;
            //tblFeeReceipts.Dec = Dec;
            //tblFeeReceipts.FeeconfigurationId = activeschool.Fee_configurationId;
            //tblFeeReceipts.Feeconfigurationname = activeschool.Fee_Configuratinname;

            _TblFeeReceiptsRepository.Insert(tblFeeReceipts);
            _TblFeeReceiptsRepository.Save();
            var feereceipt = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptsOneTimeCreator == tblFeeReceipts.FeeReceiptsOneTimeCreator);
            var currentyear = DateTime.Now.Year.ToString();

            for (int a = 0; a < feeReceiptViewModel.FeeHeadings.Count(); a++)
            {
                var feeheadingid = feeheadings.FirstOrDefault(x => x.FeeName == feeReceiptViewModel.FeeHeadings[a]);

                var Duefees = _context.TblDueFee.FirstOrDefault(x => x.CurrentYear.ToString() == currentyear && x.FeeHeadingId == feeheadingid.FeeId && x.StudentId == studentregistrations.StudentId && x.ClassName == classDetail.DataListItemName);
                if (Duefees != null)
                {
                    if (Duefees.TotalFee <= feeReceiptViewModel.collectedFeeAmt[a])
                    {
                        _context.TblDueFee.Remove(Duefees);
                        _context.SaveChanges();
                    }
                    else
                    {
                        Duefees.TotalFee = Duefees.TotalFee - feeReceiptViewModel.collectedFeeAmt[a];
                        Duefees.PaidAmount = Duefees.PaidAmount + feeReceiptViewModel.collectedFeeAmt[a];
                        Duefees.DueAmount = Duefees.TotalFee;

                        var currentUser = Session["Name"].ToString();
                        Duefees.UpdatedBy = currentUser;
                        _context.SaveChanges();
                    }
                }
            }





            return Json(feereceipt, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get All Student's fee Receipts 
        /// </summary>
        /// <returns></returns>
        /// 

        public async Task<JsonResult> FilterTcBillReceipt(string DDClass, string DDBatch, string ActualDate, DateTime DateFrom, DateTime DateTo)
        {

            var allFeeREceipts = await _context.TcFeeDetails.Include(x => x.studentFeeDetails)
                                                       .Include(x => x.student)
                                                       .Select(x => new StudentTcRceiptViewModel
                                                       {
                                                           Id = x.Id,
                                                           Studnt = x.student.Name,
                                                           Batch = x.student.BatchName,
                                                           Category = x.student.Category,
                                                           Class = x.student.Class,
                                                           paymode = x.PaymentMode,
                                                           createdon = x.PaidDate
                                                       }).ToListAsync();
            try
            {
                if (DDClass != null && DDClass != "")
                {
                    allFeeREceipts = allFeeREceipts.Where(x => x.Class == DDClass).ToList();
                }
                if (DDBatch != null && DDBatch != "")
                {
                    allFeeREceipts = allFeeREceipts.Where(x => x.Batch == DDBatch).ToList();
                }

                else if (DateFrom != null && DateTo != null)
                {
                    allFeeREceipts = allFeeREceipts.Where(x => x.createdon >= DateFrom && x.createdon <= DateTo).ToList();
                }
                else if (DateFrom != null)
                {
                    allFeeREceipts = allFeeREceipts.Where(x => x.createdon >= DateFrom).ToList();
                }
                else if (DateTo != null)
                {
                    allFeeREceipts = allFeeREceipts.Where(x => x.createdon <= DateTo).ToList();
                }
            }
            catch (Exception ex)
            {

                throw;
            }


            return Json(allFeeREceipts, JsonRequestBehavior.AllowGet);
        }


        public JsonResult DeleteFeeReceiptbyId(string id)
        {
            var genralobj = _context.TblFeeReceipts.Where(e => e.FeeReceiptsOneTimeCreator == id).ToList();
            foreach (var item in genralobj)
            {
                var dataDeleteed = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptId == item.FeeReceiptId);
                _context.TblFeeReceipts.Remove(dataDeleteed);
                _context.SaveChanges();
            }
            return Json("Record delete successfully", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetAllReceipts(string DDClass, string ActualDate, string DateFrom, string DateTo)
        {
            var data = _TblFeeReceiptsRepository.GetAll();
            var studentdata = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();

            var pagename = "All Receipts";
            var editpermission = "Delete_Permission";

            //var allFeeREceipts = _TblFeeReceiptsRepository.GetAll();
            List<TblFeeReceipts> allFeeREceipts = new List<TblFeeReceipts>();
            List<Tbl_Feereceiptsviewmodel> Tbl_Feereceiptsviewmodel = new List<Tbl_Feereceiptsviewmodel>();
            foreach (var item in data)
            {
                if (item.FeeHeadingIDs != "21" && item.FeeHeadingIDs != "25" && item.FeeHeadingIDs != "21,25" && item.FeeHeadingIDs != "25,21")
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
                            PaymentMode = item.PaymentMode,
                            TotalFee = item.TotalFee,
                            AddedDate = item.AddedDate,
                            FeeReceiptId = item.FeeReceiptId,
                            FeeReceiptsOneTimeCreator = item.FeeReceiptsOneTimeCreator,
                            DeletePermission = CheckDeletepermission(pagename, editpermission),
                            ReceiptAmt = item.ReceiptAmt
                        });

                        //allFeeREceipts.Add(item);
                    }
                }

            }
            if (DDClass != null && DDClass != "")
            {
                Tbl_Feereceiptsviewmodel = Tbl_Feereceiptsviewmodel.Where(x => x.ClassName == DDClass).ToList();
            }
            //if (DDBatch != null && DDBatch != "")
            //{
            //    allFeeREceipts = allFeeREceipts.Where(x => x.BatchName == DDBatch);
            //}
            if (ActualDate != null && ActualDate != "")
            {
                DateTime actialdate = DateTime.ParseExact(ActualDate, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date1;
                Tbl_Feereceiptsviewmodel = Tbl_Feereceiptsviewmodel.Where(x => x.AddedDate == actialdate).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact( x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) == DateTime.ParseExact( ActualDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateFrom != null && DateFrom != "" && DateTo != null && DateTo != "")
            {
                DateTime fromdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime todate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date2;
                Tbl_Feereceiptsviewmodel = Tbl_Feereceiptsviewmodel.Where(x => x.AddedDate >= fromdate && x.AddedDate <= todate).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateFrom != null && DateFrom != "")
            {
                DateTime fdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date3;
                Tbl_Feereceiptsviewmodel = Tbl_Feereceiptsviewmodel.Where(x => x.AddedDate >= fdate).ToList();

                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateTo != null && DateTo != "")
            {
                DateTime tdate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                DateTime date4;
                Tbl_Feereceiptsviewmodel = Tbl_Feereceiptsviewmodel.Where(x => x.AddedDate <= tdate).ToList();
                //allFeeREceipts = allFeeREceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }



            return Json(Tbl_Feereceiptsviewmodel, JsonRequestBehavior.AllowGet);
        }




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

        public JsonResult GetAllBatch()
        {
            var AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            return Json(AllBatchs, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllCourses()
        {
            var Courses = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "course").DataListId.ToString()).ToList();
            return Json(Courses, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetAllSemesters()
        {
            var GetAllSemesters = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            return Json(GetAllSemesters, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetStudentsFeeDetails(StudentFeeInputModel studentFeeInputModel)
        {
            List<StudentTotalFeeViewModel> studentTotalFeeViewModels = new List<StudentTotalFeeViewModel>();
            //var allStudents = _context.Students.ToList();
            var currentatch = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true && x.IsActiveForPayments == true).OrderByDescending(x => x.Batch_Id).Select(x => x.Batch_Id).FirstOrDefault();
            var studentregistrations = _context.Students.Where(x => x.IsApprove == 217 && x.IsApplyforTC == false && x.Batch_Id == currentatch).OrderBy(x => x.Name).ToList();
            var GetAllSemesters = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var GetAllSections = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            //if (!string.IsNullOrEmpty(studentFeeInputModel.Batch))
            //{
            //    allStudents = allStudents.Where(x => x.BatchName == studentFeeInputModel.Batch).ToList();
            //}
            var studentclass = _context.Students.FirstOrDefault(x => x.StudentId.ToString() == studentFeeInputModel.ScholarNumer);
            if (!string.IsNullOrEmpty(studentFeeInputModel.ScholarNumer) && studentFeeInputModel.ScholarNumer != "0")
            {
                //allStudents = allStudents.Where(x => x.StudentId.ToString() == studentFeeInputModel.ScholarNumer).ToList();
                studentregistrations = studentregistrations.Where(x => x.StudentId.ToString() == studentFeeInputModel.ScholarNumer).ToList();
            }

            if (!string.IsNullOrEmpty(studentFeeInputModel.Semester))
            {
                //allStudents = allStudents.Where(x => x.Class == studentFeeInputModel.Semester).ToList();
                var classid = GetAllSemesters.FirstOrDefault(x => x.DataListItemId.ToString() == studentFeeInputModel.Semester)?.DataListItemId;
                var sectionid = GetAllSections.FirstOrDefault(x => x.DataListItemId.ToString() == studentFeeInputModel.Section)?.DataListItemId;
                studentregistrations = studentregistrations.Where(x => x.Class_Id == classid && x.Section_Id == sectionid).ToList();
            }
            foreach (var item in studentregistrations)
            {
                int studentid = Convert.ToInt32(studentregistrations.FirstOrDefault(x => x.ApplicationNumber == item.ApplicationNumber)?.StudentId);

                if (studentid != 0)
                {
                    float TotalAmount1 = GetStudentWillPayAmmount(studentid);
                    float PaidAmount1 = GetStudentPaidAmmount(studentid);
                    float ConcesionAmount1 = GetStudentConcessionAmmount(studentid);
                    float PaidOldAmount1 = GetStudentOldAmmount(studentid);

                    studentTotalFeeViewModels.Add(new StudentTotalFeeViewModel
                    {
                        //Batch = item.BatchName,
                        ScholarNumber = Convert.ToString(item.StudentId),
                        StudentName = item.Name + " " + item.Last_Name,
                        Semester = GetAllSemesters.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                        TotalAmount = TotalAmount1,
                        ConcesionAmount = ConcesionAmount1,
                        PaidAmount = PaidAmount1,
                        oldAmmount = PaidOldAmount1,
                        PendingAmount = TotalAmount1 - PaidAmount1 - ConcesionAmount1
                    });
                }


            }
            string htmlData = GenerateHtmlForTotalPendingAmmount(studentTotalFeeViewModels);
            return Json(htmlData, JsonRequestBehavior.AllowGet);
        }
        public float GetStudentWillPayAmmount(int studentId)
        {
            float totalFeeForStudent = 0;
            //var studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            var studentregistrationdetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            if (studentregistrationdetails != null)
            {
                totalFeeForStudent = GetTotalFeeForStudent(studentregistrationdetails);
            }
            return totalFeeForStudent;
        }
        public float GetTotalFeeForStudent(Student tbl_StudentDetail)
        {
            float total = 0;
            if (tbl_StudentDetail != null)
            {
                //var allFeePlanes = _context.FeePlans.Where(x => x.BatchName == tbl_StudentDetail.BatchName && x.ClassName == tbl_StudentDetail.Class
                //                    && x.CategoryName == tbl_StudentDetail.Category
                //                    && x.FeeId != (int)FeeHeadingsEnum.TC && x.FeeId != (int)FeeHeadingsEnum.ADMISSION_FEE).ToList();//GroupBy(x => x.FeeId).Select(x => x.LastOrDefault());
                var allFeePlanes = _context.FeePlans.Where(x => x.ClassId == tbl_StudentDetail.Class_Id && x.FeeId != 22).ToList();
                foreach (var item in allFeePlanes)
                {
                    var FeeHeadings = _context.FeeHeadings.FirstOrDefault(x => x.FeeId == item.FeeId);
                    if (FeeHeadings != null)
                    {
                        if (!((tbl_StudentDetail.CurrentYear == tbl_StudentDetail.Batch_Id && item.FeeName == "Re-Admission") || (tbl_StudentDetail.CurrentYear != tbl_StudentDetail.Batch_Id && item.FeeName == "New Admission")))
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
            }
            return total;

        }

        public float GetStudentConcessionAmmount(int studentId)
        {
            float concessionAmount = 0;
            if (studentId > 0)
            {
                var studentregistration = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                //var studentDetails = _context.Students.FirstOrDefault(x => x.ApplicationNumber == studentregistration.ApplicationNumber);
                if (studentregistration != null)
                {
                    //var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentregistration.StudentRegisterID
                    //                        && x.FeeHeadingIDs != Convert.ToString( (int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs != Convert.ToString( (int)FeeHeadingsEnum.ADMISSION_FEE)).ToList()
                    //                        .GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();

                    var allFeeReceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == studentregistration.StudentId);

                    //if (allFeeReceipts.Count > 0)
                    //{
                    //    foreach (var item in allFeeReceipts.GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList())
                    //    {
                    //        concessionAmount += item.ConcessionAmt;
                    //    }
                    //}

                    if (allFeeReceipts != null)
                    {
                        //string[] FeeHeadings = allFeeReceipts.PayHeadings.Split(',');
                        //for (int i = 0; i < FeeHeadings.Length; i++)
                        {

                            concessionAmount += allFeeReceipts.ConcessionAmt;
                        }
                    }
                    //else
                    //{
                    //    var feereceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == studentDetails.StudentId);
                    //    string[] FeeHeadings = feereceipts.PayHeadings.Split(',');
                    //    for (int i = 0; i < FeeHeadings.Length; i++)
                    //    {

                    //        concessionAmount += feereceipts.ConcessionAmt;
                    //    }
                    //}



                }
            }
            return concessionAmount;

        }

        public float GetStudentPaidAmmount(int studentId)
        {
            float totalPaidAmount = 0;
            if (studentId > 0)
            {
                //var studentDetails = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                var studentregistration = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                var FeeheadingsList = _context.FeeHeadings.ToList();
                if (studentregistration != null)
                {
                    //var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentDetails.StudentId
                    //                        && x.FeeHeadingIDs != Convert.ToString( (int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs != Convert.ToString((int)FeeHeadingsEnum.ADMISSION_FEE)).ToList();//.GroupBy(x=>x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());


                    //TblFeeReceipts allFeeReceipts;
                    //allFeeReceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == studentregistration.StudentRegisterID);

                    var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.StudentId == studentregistration.StudentId);

                    if (allFeeReceipts != null && allFeeReceipts.Any())
                    {
                        //string[] FeeHeadings = allFeeReceipts.PayHeadings.Split(',');
                        //for (int i = 0; i < FeeHeadings.Length; i++)
                        {
                            totalPaidAmount += allFeeReceipts.Sum(x => x.ReceiptAmt);
                            //totalPaidAmount += allFeeReceipts.ReceiptAmt;
                        }
                    }
                    //else
                    //{
                    //    var tblstudent = _context.Students.FirstOrDefault(x => x.ApplicationNumber == studentregistration.ApplicationNumber);
                    //    var allreceiptlist = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == tblstudent.StudentId);
                    //    string[] FeeHeadings = allreceiptlist.PayHeadings.Split(',');
                    //    for (int i = 0; i < FeeHeadings.Length; i++)
                    //    {

                    //        totalPaidAmount += allreceiptlist.ReceiptAmt;
                    //    }

                    //}




                    //if (allFeeReceipts.Count > 0)
                    //{
                    //    foreach (var item in allFeeReceipts.GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList())
                    //    {
                    //        totalPaidAmount += item.ReceiptAmt;
                    //    }
                    //}

                }
            }

            return totalPaidAmount;
        }

        public float GetStudentOldAmmount(int studentId)
        {
            float discount = 0;
            if (studentId > 0)
            {
                var studentregistrations = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
                //var studentDetails = _context.Students.FirstOrDefault(x => x.ApplicationNumber == studentregistrations.ApplicationNumber);
                if (studentregistrations != null)
                {
                    //var LastFeeReceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentDetails.StudentId
                    //                    && x.FeeHeadingIDs !=Convert.ToString( (int)FeeHeadingsEnum.TC) && x.FeeHeadingIDs !=Convert.ToString( (int)FeeHeadingsEnum.ADMISSION_FEE))
                    //                    .OrderByDescending(x => x.FeeReceiptId).FirstOrDefault();

                    var LastFeeReceipt = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == studentregistrations.StudentId);

                    if (LastFeeReceipt != null)
                    {
                        //string[] FeeHeadings = LastFeeReceipt.PayHeadings.Split(',');
                        //for (int i = 0; i < FeeHeadings.Length; i++)
                        {
                            discount += LastFeeReceipt.OldBalance;
                        }
                    }
                    //else
                    //{
                    //    var feereceipt = _context.TblFeeReceipts.FirstOrDefault(c => c.StudentId == studentDetails.StudentId);
                    //    string[] FeeHeadings = feereceipt.PayHeadings.Split(',');
                    //    for (int i = 0; i < FeeHeadings.Length; i++)
                    //    {
                    //        discount += feereceipt.OldBalance;
                    //    }
                    //}


                    //if (LastFeeReceipt != null)
                    //{
                    //    discount = LastFeeReceipt.OldBalance;
                    //}
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
                var tamt = item.TotalAmount.ToString() == "0" ? "" : item.TotalAmount.ToString();
                var pamt = item.PaidAmount.ToString() == "0" ? "" : item.PaidAmount.ToString();
                var oamt = item.oldAmmount.ToString() == "0" ? "" : item.oldAmmount.ToString();
                var camt = item.ConcesionAmount.ToString() == "0" ? "" : item.ConcesionAmount.ToString();
                var penamt = item.PendingAmount.ToString() == "0" ? "" : item.PendingAmount.ToString();
                html.Append("<tr>");
                html.Append("<td style='padding:5px;'>" + i + "</td>");
                html.Append("<td style='padding:5px;'>" + item.StudentName + "</td>");
                html.Append("<td style='padding:5px;'>" + item.ScholarNumber + "</td>");
                //html.Append("<td>" + item.Batch + "</td>");
                html.Append("<td style='padding:5px;'>" + item.Semester + "</td>");
                html.Append("<td style='padding:5px;'>" + tamt + "</td>");
                html.Append("<td style='padding:5px;'>" + pamt + "</td>");
                html.Append("<td style='padding:5px;'>" + oamt + "</td>");
                html.Append("<td style='padding:5px;'>" + camt + "</td>");
                html.Append("<td style='padding:5px;'>" + penamt + "</td>");
                totalPaidFromStudent += item.PaidAmount;
                totalPaidDisAmount += item.oldAmmount;
                totalPendingAmount += item.PendingAmount;
                totalConcesionAmount += item.ConcesionAmount;
                totalPaidAmountByStudent += item.TotalAmount;
                html.Append("</tr>");
                i++;

            }
            var topadidamt = totalPaidFromStudent.ToString() == "0" ? "" : totalPaidFromStudent.ToString();
            var topadisamt = totalPaidDisAmount.ToString() == "0" ? "" : totalPaidDisAmount.ToString();
            var topenamt = totalPendingAmount.ToString() == "0" ? "" : totalPendingAmount.ToString();
            var totconcamt = totalConcesionAmount.ToString() == "0" ? "" : totalConcesionAmount.ToString();
            var totpabystu = totalPaidAmountByStudent.ToString() == "0" ? "" : totalPaidAmountByStudent.ToString();

            html.Append("<tr>");
            //html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td></td>");
            html.Append("<td><b>Total :</b></td>");

            html.Append("<td><b>" + totpabystu + "</b></td>");
            html.Append("<td><b>" + topadidamt + "</b></td>");
            html.Append("<td><b>" + topadisamt + "</b></td>");
            html.Append("<td><b>" + totconcamt + "</b></td>");
            html.Append("<td><b>" + topenamt + "</b></td>");

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


        public async Task<JsonResult> GenerateFeeReports(string DDClass, string FeeHeader, string DateFrom, string DateTo, string StudentId, string FeeHeading, string SchoolType)
        {
            var allReport = new List<TodayFeeReportViewModel>();
            DateTime fromdt = DateTime.Now; DateTime todt = DateTime.Now;
            try
            {
                int i = 1;

                if (DateFrom != null && DateFrom != "")
                {
                    fromdt = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                if (DateTo != null && DateTo != "")
                {
                    todt = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                }
                var allStudents = await _context.Students.Where(x => x.IsApprove != 192).ToListAsync();

                if (SchoolType == "1")
                {

                    var datalistId = _context.DataLists.FirstOrDefault(x => x.DataListName == "class").DataListId;
                    var DataListItemIds = _context.DataListItems.Where(e => e.DataListId == datalistId.ToString() && (e.DataListItemName == "Class-Nursery" || e.DataListItemName == "Class-LKG" || e.DataListItemName == "Class-UKG" || e.DataListItemName == "Class-PlayGroup")).Select(x => x.DataListItemId).ToList();

                    if (DataListItemIds.Any())
                        allStudents = allStudents.Where(x => DataListItemIds.Contains(x.Class_Id)).ToList();
                }
                else if (SchoolType == "2")
                {
                    var datalistId = _context.DataLists.FirstOrDefault(x => x.DataListName == "class").DataListId;
                    var DataListItemIds = _context.DataListItems.Where(e => e.DataListId == datalistId.ToString() && (e.DataListItemName == "Class-Nursery" || e.DataListItemName == "Class-LKG" || e.DataListItemName == "Class-UKG" || e.DataListItemName == "Class-PlayGroup")).Select(x => x.DataListItemId).ToList();

                    if (DataListItemIds.Any())
                        allStudents = allStudents.Where(x => !DataListItemIds.Contains(x.Class_Id)).ToList();
                }

                var tcDetails = await _context.TcFeeDetails.ToListAsync();
                var feeheadings = _context.FeeHeadings.ToList();
                var feereceipt = _context.TblFeeReceipts.ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault());
                if (FeeHeading == "NOTTransport")
                {
                    if (FeeHeader == "10")
                    {
                        foreach (var item in tcDetails.Where(x => x.StudentTcDetailsId != null))
                        {
                            allReport.Add(new TodayFeeReportViewModel
                            {
                                SNO = i,
                                StudentName = allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.Name).FirstOrDefault(),
                                ScholarNumber = Convert.ToString(allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.StudentId).FirstOrDefault()),
                                BillNo = Convert.ToString(item.Id),
                                Amount = Convert.ToString(item.ReceiptAmount),
                                Heading = "Tc Fee",
                                PaidDate = item.PaidDate
                            });
                            i++;
                        }


                        if (DateFrom != null && DateTo != null)
                        {

                            allReport = allReport.Where(x => x.PaidDate >= fromdt && x.PaidDate <= todt).ToList();

                        }
                        else if (DateFrom != null)
                        {
                            allReport = allReport.Where(x => x.PaidDate >= fromdt).ToList();
                        }
                        else if (DateTo != null)
                        {
                            allReport = allReport.Where(x => x.PaidDate <= todt).ToList();
                        }
                    }
                    else if (FeeHeader == "11")
                    {
                        foreach (var item in tcDetails.Where(x => x.StudentTcDetailsId == null))
                        {
                            allReport.Add(new TodayFeeReportViewModel
                            {
                                SNO = i,
                                StudentName = allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.Name).FirstOrDefault(),
                                ScholarNumber = Convert.ToString(allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.StudentId).FirstOrDefault()),
                                BillNo = Convert.ToString(item.Id),
                                Amount = Convert.ToString(item.ReceiptAmount),
                                Heading = "Admission Fee",
                                PaidDate = item.PaidDate
                            });
                            i++;
                        }

                        if (DateFrom != null && DateTo != null)
                        {
                            allReport = allReport.Where(x => x.PaidDate >= fromdt && x.PaidDate <= todt).ToList();
                        }
                        else if (DateFrom != null)
                        {
                            allReport = allReport.Where(x => x.PaidDate >= fromdt).ToList();
                        }
                        else if (DateTo != null)
                        {
                            allReport = allReport.Where(x => x.PaidDate <= todt).ToList();
                        }

                    }
                    else
                    {
                        List<TblFeeReceipts> allFeeReceipts = new List<TblFeeReceipts>();
                        foreach (var item in feereceipt)
                        {
                            if (item.FeeHeadingIDs != "21" && item.FeeHeadingIDs != "25" && item.FeeHeadingIDs != "21,25" && item.FeeHeadingIDs != "25,21")
                            {
                                allFeeReceipts.Add(item);
                            }

                        }
                        //var allFeeReceipts = _context.TblFeeReceipts.Where(x =>x.FeeHeadingIDs != "21").ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();

                        if (!string.IsNullOrWhiteSpace(DDClass))
                        {
                            string[] classes = DDClass.Split(',');
                            allFeeReceipts = allFeeReceipts.Where(x => classes.Contains(x.ClassName)).ToList();
                        }
                        if (!string.IsNullOrWhiteSpace(StudentId))
                        {
                            allFeeReceipts = allFeeReceipts.Where(x => x.StudentId == Convert.ToInt32(StudentId)).ToList();
                        }

                        if (FeeHeader != null && FeeHeader != "")
                        {
                            List<TblFeeReceipts> tblfee = new List<TblFeeReceipts>();
                            foreach (var obj in allFeeReceipts)
                            {
                                if (obj.FeeHeadingIDs != null)
                                {
                                    string[] feeid = obj.FeeHeadingIDs.Split(',');
                                    string[] payamount = obj.FeePaids.Split(',');
                                    int y = 0;
                                    foreach (var data in feeid)
                                    {

                                        if (data == FeeHeader)
                                        {
                                            var feeheader = feeheadings.FirstOrDefault(x => x.FeeId.ToString() == FeeHeader);
                                            if (feeheader != null)
                                            {
                                                obj.PayHeadings = feeheader.FeeName;
                                                obj.ReceiptAmt = float.Parse(payamount[y]);
                                                tblfee.Add(obj);
                                            }
                                        }
                                        y++;
                                    }
                                }
                            }

                            //allFeeReceipts = allFeeReceipts.Where(x => x.FeeHeadingIDs == Convert.ToString(FeeHeader)).ToList();
                            allFeeReceipts = tblfee;
                        }

                        if (DateFrom != null && DateTo != null)
                        {
                            // DateTime fromdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            //DateTime todate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            DateTime date1;
                            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate >= fromdt && x.AddedDate <= todt).ToList();



                            //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                        }
                        else if (DateFrom != null)
                        {
                            // DateTime fdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            DateTime date2;

                            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate >= fromdt).ToList();

                            //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                        }
                        else if (DateTo != null)
                        {

                            // DateTime tdate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                            DateTime date3;

                            allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate <= todt).ToList();

                            //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                        }
                        foreach (var item in allFeeReceipts)
                        {
                            var studentname = allStudents.FirstOrDefault(x => x.StudentId == item.StudentId);
                            if (studentname != null)
                            {
                                allReport.Add(new TodayFeeReportViewModel
                                {
                                    SNO = i,
                                    StudentName = studentname.Name + " " + studentname.Last_Name,
                                    ScholarNumber = Convert.ToString(allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.StudentId).FirstOrDefault()),
                                    BillNo = Convert.ToString(item.FeeReceiptId),
                                    Amount = Convert.ToString(item.ReceiptAmt),
                                    Heading = item.PayHeadings,
                                    PaidDate = item.AddedDate
                                });
                                i++;
                            }

                        }
                    }
                }
                else
                {
                    List<TblFeeReceipts> allFeeReceipts = new List<TblFeeReceipts>();
                    foreach (var item in feereceipt)
                    {
                        if (item.FeeHeadingIDs == "21" || item.FeeHeadingIDs == "25" || item.FeeHeadingIDs == "21,25" || item.FeeHeadingIDs == "25,21")
                        {
                            allFeeReceipts.Add(item);
                        }


                    }
                    //var allFeeReceipts = _context.TblFeeReceipts.Where(x => x.FeeHeadingIDs == "21").ToList().GroupBy(x => x.FeeReceiptsOneTimeCreator).Select(x => x.LastOrDefault()).ToList();

                    if (!string.IsNullOrWhiteSpace(DDClass))
                    {
                        string[] classes = DDClass.Split(',');
                        allFeeReceipts = allFeeReceipts.Where(x => classes.Contains(x.ClassName)).ToList();
                    }

                    //if (DDClass != null && DDClass != "")
                    //{
                    //    allFeeReceipts = allFeeReceipts.Where(x => x.ClassName == DDClass).ToList();
                    //}
                    if (!string.IsNullOrWhiteSpace(StudentId))
                    {
                        allFeeReceipts = allFeeReceipts.Where(x => x.StudentId == Convert.ToInt32(StudentId)).ToList();
                    }

                    if (FeeHeader != null && FeeHeader != "")
                    {
                        allFeeReceipts = allFeeReceipts.Where(x => x.FeeHeadingIDs == Convert.ToString(FeeHeader)).ToList();
                    }

                    if (DateFrom != null && DateTo != null)
                    {

                        //  DateTime fromdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        //  DateTime todate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        DateTime date1;
                        allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate >= fromdt && x.AddedDate <= todt).ToList();

                        //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                    }
                    else if (DateFrom != null)
                    {

                        //  DateTime fdate = DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        DateTime date2;

                        allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate >= fromdt).ToList();

                        //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                    }
                    else if (DateTo != null)
                    {

                        // DateTime tdate = DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                        DateTime date3;

                        allFeeReceipts = allFeeReceipts.Where(x => x.AddedDate <= todt).ToList();

                        //allFeeReceipts = allFeeReceipts.Where(x => DateTime.ParseExact(x.AddedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToList();
                    }
                    foreach (var item in allFeeReceipts)
                    {
                        allReport.Add(new TodayFeeReportViewModel
                        {
                            SNO = i,
                            StudentName = allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.Name).FirstOrDefault(),
                            ScholarNumber = Convert.ToString(allStudents.Where(x => x.StudentId == item.StudentId).Select(x => x.StudentId).FirstOrDefault()),
                            BillNo = Convert.ToString(item.FeeReceiptId),
                            Amount = Convert.ToString(item.ReceiptAmt),
                            Heading = item.PayHeadings,
                            PaidDate = item.AddedDate
                        });
                        i++;
                    }
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }



            return Json(allReport, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GeStudentListForApplyTc(int? batch, string className, string sectionList)
        {
            System.Text.StringBuilder html = new System.Text.StringBuilder();

            //try
            //{
            //////var result = new List<Student>();

            //////result = _context.Students.Where(x => !x.IsDeleted).ToList();
            //////if (batch > 0)
            //////{
            //////    result = result.Where(x => x.Batch_Id == batch).ToList();
            //////}
            //////if (!string.IsNullOrEmpty(sectionList))
            //////{
            //////    result = result.Where(x => x.Section_Id == Convert.ToInt64(sectionList)).ToList();
            //////}
            //////if (!string.IsNullOrEmpty(className))
            //////{
            //////    if (Int32.Parse(className) > 0)
            //////    {
            //////        result = result.Where(x => x.Class_Id == Int32.Parse(className)).ToList();
            //////    }
            //////}
            // new addition
            var query = (from s in _context.Students
                         where !s.IsDeleted
                         join sReg in _context.StudentsRegistrations on s.ApplicationNumber equals sReg.ApplicationNumber
                         join familyDetail in _context.FamilyDetails on s.ApplicationNumber equals familyDetail.ApplicationNumber
                         select new
                         {
                             s.StudentId,
                             s.Name,
                             s.DOB,
                             s.Class,
                             //s.Gender,
                             s.IsApplyforTC,
                             s.Class_Id,
                             s.ApplicationNumber,
                             Contact = s.Mobile,
                             s.Batch_Id,
                             s.Section_Id,
                             familyDetail.FatherName,
                             familyDetail.MotherName,
                             s.ScholarNo
                         });

            if (batch > 0 && string.IsNullOrEmpty(sectionList) && string.IsNullOrEmpty(className))
            {
                query = query.Where(x => x.Batch_Id == batch && x.IsApplyforTC);
            }
            else if (batch > 0)
            {
                query = query.Where(x => x.Batch_Id == batch);
            }
            if (!string.IsNullOrEmpty(sectionList))
            {
                var sectionId = Convert.ToInt64(sectionList);
                query = query.Where(x => x.Section_Id == sectionId);
            }

            if (!string.IsNullOrEmpty(className))
            {
                int classId;
                if (Int32.TryParse(className, out classId) && classId > 0)
                    query = query.Where(x => x.Class_Id == classId);

            }

            var result = query.ToList();
            //new addtion close
            //if (sectionName>0)
            //{
            //    result = result.Where(x => x.Section_Id == Convert.ToInt32(sectionName)).ToList();
            //}

            //var tcNotProvidedStudents = _context.Students.Where(x => !x.IsApplyforTC).Select(x => x.StudentId).ToList();
            //if (tcNotProvidedStudents != null)
            //{
            //    result = result.Where(x => tcNotProvidedStudents.Any(t => t == x.StudentId)).ToList();
            //}
            int count = 0;
            foreach (var item in result)
            {
                count++;
                html.Append("<tr>");
                //html.Append("<td>" + item.UIN + "</td>");
                html.Append("<td style='display:none;'>" + item.StudentId + "</td>");
                html.Append("<td>" + count + "</td>");
                html.Append("<td><a href='/Student/UdateStudentDetails?id=" + item.StudentId + "&routingid=3' >" + item.Name + "</a></td>");
                html.Append("<td>" + item.DOB + "</td>");
                html.Append("<td>" + item.Class + "</td>");
                html.Append("<td>" + item.ScholarNo + "</td>");
                //html.Append("<td>" + item.BatchName + "</td>");
                //html.Append("<td>" + item.Gender + "</td>");
                html.Append("<td>" + item.Contact + "</td>");
                html.Append("<td>" + item.FatherName + "</td>");
                html.Append("<td>" + item.MotherName + "</td>");
                //html.Append("<td>" + item.Category + "</td>");
                if (item.IsApplyforTC)
                {

                    html.Append("<td>");
                    html.Append("<input type='checkbox' name='applyTcCk'  class='applytcclass' data-val='" + item.StudentId + "' checked/>");
                    html.Append("</td>");
                    StudentTcDetails objStudentTcDetails = _context.Tbl_StudentTcDetails.FirstOrDefault(i => i.StudentId == item.StudentId);
                    if (objStudentTcDetails != null && objStudentTcDetails.StudentId > 0)
                    {
                        int PromoteClassId = (objStudentTcDetails.PromoteClassId == null) ? 0 : (int)objStudentTcDetails.PromoteClassId;
                        int PromoteSectionId = (objStudentTcDetails.PromoteSectionId == null) ? 0 : (int)objStudentTcDetails.PromoteSectionId;
                        string Remark = (objStudentTcDetails.Remark == null) ? "No" : objStudentTcDetails.Remark;
                        string Reason = (objStudentTcDetails.Reason == null) ? "No" : objStudentTcDetails.Reason;
                        int RemarksId = (objStudentTcDetails.RemarksID == null) ? 0 : (int)objStudentTcDetails.RemarksID;
                        int ReasonId = (objStudentTcDetails.ReasonID == null) ? 0 : (int)objStudentTcDetails.ReasonID;
                        string FeePaidUpto = string.IsNullOrEmpty(objStudentTcDetails.FeePaidUpto) ? "" : objStudentTcDetails.FeePaidUpto;
                        string OtherRemarks = string.IsNullOrEmpty(objStudentTcDetails.OtherRemarks) ? "" :
                            objStudentTcDetails.OtherRemarks;
                        string TotalAttendance = string.IsNullOrEmpty(objStudentTcDetails.TotalAttendance) ? "" :
                           objStudentTcDetails.TotalAttendance;
                        string TotalWorkingDays = string.IsNullOrEmpty(objStudentTcDetails.TotalWorkingDays) ? "" :
                          objStudentTcDetails.TotalWorkingDays;

                        var data = /*"," +*/ "\"" + RemarksId + "\"" + "," + "\"" + ReasonId + "\"" + "," + "\"" + objStudentTcDetails.SchoolLeftDate?.ToString("yyyy-MM-dd") + "\"" + "," + "\"" + FeePaidUpto + "\"" + "," + "\"" + OtherRemarks + "\"" + "," + "\"" + TotalAttendance + "\"," + "\"" + TotalWorkingDays + "\"";

                        html.Append("<td><Button Id='EditTCDetails' title='Edit' class='btn btn-primary' onclick='EditTCDetails(" + item.StudentId + "," + item.Class_Id + "," + item.Batch_Id + "," + item.ApplicationNumber + "," + objStudentTcDetails.ExamStatusId + "," + PromoteClassId + "," + PromoteSectionId + "," + data + ")' ><i class=\"fa fa-edit\"></i></Button>");
                        html.Append("<Button Id='BtnPrintTCDetails' title='Print' class='btn btn-primary' onclick='PrintTC(" + item.StudentId + ")' ><i class=\"fa fa-print\"></i></Button>");


                        html.Append("<Button Id='BtnPrintTCDetails' title='Print' class='btn btn-primary' onclick='PrintTC2(" + item.StudentId + ")' ><i class=\"fa fa-print\"></i></Button>");
                        // New Code for cancel TC
                        html.Append("<Button Id='BtnCancelTC' title='Cancel TC' class='btn btn-danger' style='background-color:#d9534f' onclick='CancelTC(" + item.StudentId + ")' ><i class=\"fa fa-ban\"></i></Button></td>");

                    }
                    else
                    {
                        html.Append("<td></td>");
                    }
                }
                else
                {
                    html.Append("<td>");
                    html.Append("<input type='checkbox' name='applyTcCk'  class='applytcclass' data-val='" + item.StudentId + "'/>");
                    html.Append("</td>");
                    html.Append("<td>");
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
            return Json(html.ToString(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStudentTCDetails(int StudentId)
        {
            PrintStudentTcDetailsViewModel objPrintStudentTcDetails = new PrintStudentTcDetailsViewModel();
            Student objStudent = _context.Students.FirstOrDefault(x => x.StudentId == StudentId);
            var Classes = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            var Batchs = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();
            var Remarks = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Remarks").DataListId.ToString()).ToList();
            var Reason = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Reason").DataListId.ToString()).ToList();
            if (objStudent.StudentId > 0)
            {
                string AdmissionDate = DeteToWords(objStudent.AddedDate.Day) + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(objStudent.AddedDate.Month) + " " + NumberToWords(objStudent.AddedDate.Year);
                objPrintStudentTcDetails.AdmissionDateWord = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(AdmissionDate.ToLower());




                objPrintStudentTcDetails.AdmittedDate = objStudent.AddedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                objPrintStudentTcDetails.StudentId = objStudent.StudentId;
                objPrintStudentTcDetails.StudentName = objStudent.Name;
                objPrintStudentTcDetails.Gender = objStudent.Gender;
                objPrintStudentTcDetails.ApplicationNo = objStudent.ApplicationNumber;
                objPrintStudentTcDetails.ScholarNo = objStudent.ScholarNo.ToString();
                //var attendance = _context.Tbl_StudentAttendance
                //    .Where(x => x.StudentRegisterID == objStudent.StudentId
                //    && x.Class_Id == objStudent.Class_Id
                //    && x.Section_Id == objStudent.Section_Id)
                //    .GroupBy(att => 1)
                //    .Select(x => new
                //    {
                //        FullDayAbsent = x.Sum(att => att.Mark_FullDayAbsent == "True" ? 1 : 0),
                //        HalfDayAbsent = x.Sum(att => att.Mark_HalfDayAbsent == "True" ? 1 : 0),
                //        TotalDays = x.Count()
                //    }).FirstOrDefault();
                var attQuery = _context.Tbl_StudentAttendance
                    .Where(x => x.StudentRegisterID == objStudent.StudentId
                    && x.Class_Id == objStudent.Class_Id
                    && x.Section_Id == objStudent.Section_Id);
                var fullDayAbsent = attQuery.Where(x => x.Mark_FullDayAbsent == "True").Count();
                var halfDayAbsent = attQuery.Where(x => x.Mark_HalfDayAbsent == "True").Count();
                var totalWorkingDays = attQuery.Count();


                //objPrintStudentTcDetails.TotAttendance = totalWorkingDays - fullDayAbsent - halfDayAbsent / 2;
                //objPrintStudentTcDetails.TotWorkingDays = totalWorkingDays;

                StudentsRegistration objStudentsRegistration = _context.StudentsRegistrations.FirstOrDefault(i => i.ApplicationNumber == objStudent.ApplicationNumber);
                if (objStudentsRegistration.StudentRegisterID > 0)
                {
                    DateTime parsedDate;
                    string formattedDate = string.Empty;
                    if (!string.IsNullOrWhiteSpace(objStudentsRegistration.Registration_Date))
                    {
                        // Try to parse the string using multiple possible formats
                        string[] formats = { "dd/MM/yyyy", "d-MMM-yyyy", "dd-MM-yyyy", "MM/dd/yyyy" }; // add more formats if needed

                        if (DateTime.TryParseExact(objStudentsRegistration.Registration_Date, formats, CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None, out parsedDate))
                        {
                            //formattedDate = parsedDate.ToString("dd/MM/yyyy");
                            formattedDate = parsedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                        }
                        else
                        {
                            // Optional: handle parse failure
                            formattedDate = ""; // or log an error message
                        }
                    }

                    // Assign the formatted value
                    objPrintStudentTcDetails.AdmissionDate = formattedDate;
                    //objPrintStudentTcDetails.AdmissionDate = objStudentsRegistration.AddedDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    objPrintStudentTcDetails.PerEduNumber = objStudentsRegistration.PerEduNumber ?? "";
                    objPrintStudentTcDetails.SSSMIdNumber = objStudentsRegistration.SSSMIdNumber ?? "";
                    objPrintStudentTcDetails.FamilySSSMID = objStudentsRegistration.FamilySSSMID ?? "";
                    objPrintStudentTcDetails.Nationality = objStudentsRegistration.Nationality ?? "";
                    objPrintStudentTcDetails.CastCategory = objStudentsRegistration.Category ?? "";
                    objPrintStudentTcDetails.DOB = objStudentsRegistration.DOB ?? "";
                    objPrintStudentTcDetails.ApaarId = objStudentsRegistration.ApaarId;
                    //DateTime convertedDate = DateTime.ParseExact(objStudentsRegistration.DOB, "dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    //string dob = DeteToWords(convertedDate.Day) + " " + CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(convertedDate.Month) + " " + DeteToWords(convertedDate.Year);
                    //objPrintStudentTcDetails.DOBWord = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dob.ToLower());
                    if (!string.IsNullOrWhiteSpace(objStudentsRegistration.DOB))
                    {
                        DateTime dobDate;
                        string[] formats = { "dd/MM/yyyy", "d-MMM-yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd" }; // jitne formats aapke DB me possible hain

                        if (DateTime.TryParseExact(objStudentsRegistration.DOB, formats, CultureInfo.InvariantCulture,
                                                   DateTimeStyles.None, out dobDate))
                        {
                            string dob = DeteToWords(dobDate.Day) + " " +
                                         CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dobDate.Month) + " " +
                                         DeteToWords(dobDate.Year);

                            objPrintStudentTcDetails.DOBWord = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(dob.ToLower());
                        }
                        else
                        {
                            // fallback: agar parse nahi hua to blank rakhna ya log karna
                            objPrintStudentTcDetails.DOBWord = "";
                        }
                    }


                    FamilyDetail objFamilyDetail = _context.FamilyDetails.FirstOrDefault(i => i.ApplicationNumber == objStudentsRegistration.ApplicationNumber);
                    if (objFamilyDetail.StudentRefId > 0)
                    {
                        objPrintStudentTcDetails.FatherName = objFamilyDetail.FatherName;
                        objPrintStudentTcDetails.MotherName = objFamilyDetail.MotherName;
                        objPrintStudentTcDetails.Rvill = objFamilyDetail.Rvill;
                        objPrintStudentTcDetails.Rpost = objFamilyDetail.Rpost;
                        objPrintStudentTcDetails.Rdist = objFamilyDetail.Rdist;
                        objPrintStudentTcDetails.Rstate = objFamilyDetail.Rstate;
                    }
                }
                StudentTcDetails objStudentTcDetails = _context.Tbl_StudentTcDetails.FirstOrDefault(i => i.StudentId == StudentId);
                if (objStudentTcDetails.StudentId > 0)
                {
                    var ClassName = Classes.Where(w => w.DataListItemId == objStudentTcDetails.ClassId)
                .Select(s => s.DataListName).FirstOrDefault();
                    var RomanClassName = Classes.Where(w => w.DataListItemId == objStudentTcDetails.ClassId)
                .Select(s => s.DataListItemName).FirstOrDefault()/* + " " + ClassName*/;
                    objPrintStudentTcDetails.ClassName = ClassName;
                    objPrintStudentTcDetails.LastClass = RomanClassName;
                    objPrintStudentTcDetails.LastClassWord = ClassName;
                    objPrintStudentTcDetails.TCSNo = objStudentTcDetails.TCSNo;
                   
                    objPrintStudentTcDetails.FeePaidUpto = objStudentTcDetails.FeePaidUpto;
                    objPrintStudentTcDetails.OtherRemarks = objStudentTcDetails.OtherRemarks;
                    //objPrintStudentTcDetails.TotAttendance = string.IsNullOrWhiteSpace(objStudentTcDetails.TotalAttendance) ? 0 : Convert.ToInt32(objStudentTcDetails.TotalAttendance);
                    objPrintStudentTcDetails.TotAttendance = int.TryParse(objStudentTcDetails.TotalAttendance, out var att) ? att : 0;

                    objPrintStudentTcDetails.TotWorkingDays = string.IsNullOrWhiteSpace(objStudentTcDetails.TotalWorkingDays) ? 0 : Convert.ToInt32(objStudentTcDetails.TotalWorkingDays);
                    objPrintStudentTcDetails.BatchYear = Batchs.Where(w => w.DataListItemId == objStudentTcDetails.BatchId).Select(s => s.DataListItemName).FirstOrDefault();
                    if (objStudentTcDetails.RemarksID != null)
                    {
                        var RemarksIDnAME = Remarks.Where(w => w.DataListItemId == objStudentTcDetails.RemarksID).Select(s => s.DataListItemName).FirstOrDefault();
                        objPrintStudentTcDetails.Remark = RemarksIDnAME;
                    }
                        if (objStudentTcDetails.PromoteClassId != null)
                    {
                        var PromotedClassItemName = Classes.Where(w => w.DataListItemId == objStudentTcDetails.PromoteClassId).Select(s => s.DataListName).FirstOrDefault();
                        var PromitedClass = Classes.Where(w => w.DataListItemId == objStudentTcDetails.PromoteClassId).Select(s => s.DataListItemName).FirstOrDefault() /*+ " " + PromotedClassItemName*/;

                        objPrintStudentTcDetails.QualifiedForPromotion = "Granted";
                        objPrintStudentTcDetails.PromoteClassName = PromitedClass;
                        objPrintStudentTcDetails.PromoteClassNameWord = PromotedClassItemName;
                    }
                    else
                    {
                        objPrintStudentTcDetails.QualifiedForPromotion = "Not Granted";
                        objPrintStudentTcDetails.PromoteClassName = "N/A";
                        objPrintStudentTcDetails.PromoteClassNameWord = "N/A";
                    }
                    if (objStudentTcDetails.PromoteSectionId != null)
                    {
                        objPrintStudentTcDetails.PromoteSectionName = Section.Where(w => w.DataListItemId == objStudentTcDetails.PromoteSectionId).Select(s => s.DataListItemName).FirstOrDefault();
                    }

                    if (objStudentTcDetails.ExamStatusId == (int)ExamStatusEnum.Pass)
                    {
                        objPrintStudentTcDetails.ExamStatus = "had Passed";
                    }
                    else
                    {
                        objPrintStudentTcDetails.ExamStatus = "had Not Passed";
                    }

                    var RemarksName = Remarks.Where(w => w.DataListItemId == objStudentTcDetails.RemarksID)
                .Select(s => s.DataListItemName).FirstOrDefault();

                    var ReasonName = Reason.Where(w => w.DataListItemId == objStudentTcDetails.ReasonID)
               .Select(s => s.DataListItemName).FirstOrDefault();

                    objPrintStudentTcDetails.Remark = RemarksName;
                    objPrintStudentTcDetails.Reason = ReasonName;
                    objPrintStudentTcDetails.TCDate = objStudentTcDetails.CreatedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    objPrintStudentTcDetails.TCApplyDate = objStudentTcDetails.SchoolLeftDate?.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    objPrintStudentTcDetails.SNo = Convert.ToString(objStudentTcDetails.Id) + "/" + Convert.ToString(objStudentTcDetails.CreatedOn.Year);
                    if (objStudentTcDetails?.SchoolLeftDate != null)
                    {
                        objPrintStudentTcDetails.SchoolLeftDate = objStudentTcDetails?.SchoolLeftDate.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                TblCreateSchool objSchoolDetails = _context.TblCreateSchool.FirstOrDefault();
                if (objSchoolDetails.School_Id > 0)
                {
                    objPrintStudentTcDetails.SchoolName = objSchoolDetails.School_Name;
                    objPrintStudentTcDetails.SchoolEmail = objSchoolDetails.Email;
                    objPrintStudentTcDetails.SchoolWebsite = objSchoolDetails.Website;
                    objPrintStudentTcDetails.SchoolLogo = objSchoolDetails.Upload_Image;
                }
            }


            return Json(objPrintStudentTcDetails, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public async Task<JsonResult> ApplyTCDetailstoStudent(StudentTcDetailsViewModel objStudentTcDetailsVM)
        {
            try
            {

                StudentTcDetails objStudentTcDetails = await _context.Tbl_StudentTcDetails.FirstOrDefaultAsync(x => x.StudentId == objStudentTcDetailsVM.StudentId);
                if (objStudentTcDetails.StudentId > 0)
                {
                    objStudentTcDetails.ClassId = objStudentTcDetailsVM.ClassId;
                    objStudentTcDetails.BatchId = objStudentTcDetailsVM.BatchId;
                    objStudentTcDetails.ExamStatusId = objStudentTcDetailsVM.ExamStatusId;
                    objStudentTcDetails.PromoteClassId = objStudentTcDetailsVM.PromoteClassId;
                    objStudentTcDetails.PromoteSectionId = objStudentTcDetailsVM.PromoteSectionId;
                    objStudentTcDetails.RemarksID = objStudentTcDetailsVM.RemarksID;
                    objStudentTcDetails.ReasonID = objStudentTcDetailsVM.ReasonID;
                    objStudentTcDetails.SchoolLeftDate = objStudentTcDetailsVM.SchoolLeftDate;
                    objStudentTcDetails.FeePaidUpto = objStudentTcDetailsVM.FeePaidUpto;
                    objStudentTcDetails.OtherRemarks = objStudentTcDetailsVM.OtherRemarks;
                    objStudentTcDetails.TotalAttendance = objStudentTcDetailsVM.TotalAttendance;
                    objStudentTcDetails.TotalWorkingDays = objStudentTcDetailsVM.TotalWorkingDays;
                    _context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return Json("Error occurred, please try again!");
            }

            return Json("TC Deatils Saved Sucessfully");
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleventh", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }
        public static string DeteToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + DeteToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += DeteToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += DeteToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += DeteToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "";

                var unitsMap = new[] { "zero", "first", "second", "third", "fourth", "fiveth", "sixth", "seventh", "eightth", "nineth", "tenth", "eleventh", "twelveth", "thirteenth", "fourteenth", "fifteenth", "sixteenth", "seventeenth", "eighteenth", "nineteenth" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }

            return words;
        }

        //public async Task<JsonResult> ApplyTcToStudents(ApplyTCModel tcDetailsModel)
        //{
        //    try
        //    {

        //        var tcId = await _context.DataListItems.Where(x => x.DataListItemName.Contains("TC")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
        //        foreach (var item in tcDetailsModel.studentIdList)
        //        {
        //            Student student = new Student();
        //            student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == item && !x.IsApplyforTC);
        //            StudentsRegistration studentReg = await _context.StudentsRegistrations.FirstOrDefaultAsync(x => x.ApplicationNumber == student.ApplicationNumber && !x.IsApplyforTC);
        //            if (studentReg != null)
        //            {
        //                studentReg.IsApplyforTC = true;
        //                studentReg.IsApplyforAdmission = false;
        //                studentReg.IsApprove = 0;
        //            }
        //            if (student != null)
        //            {
        //                student.IsApplyforTC = true;
        //                student.IsApplyforAdmission = false;
        //                //Note:We are change the datatype bool to int.
        //                //student.IsApprove = false;

        //                StudentTcDetails objStudentTcDetails = await _context.Tbl_StudentTcDetails.FirstOrDefaultAsync(i => i.StudentId == student.StudentId);
        //                if (objStudentTcDetails == null)
        //                {
        //                    var studentTcDetails = new StudentTcDetails();
        //                    studentTcDetails.StudentId = Convert.ToInt32(item);
        //                    studentTcDetails.TcId = tcId;
        //                    studentTcDetails.ClassId = student.Class_Id;
        //                    studentTcDetails.BatchId = student.Batch_Id;
        //                    studentTcDetails.CreatedOn = DateTime.Now;
        //                    studentTcDetails.Ispaid = false;

        //                    _context.Tbl_StudentTcDetails.Add(studentTcDetails);
        //                }

        //                _context.SaveChanges();

        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string msg = ex.Message;
        //        return Json("Error occurred, please try again!");
        //    }

        //    return Json("TC has been provided");
        //}
        [HttpPost]
        public async Task<JsonResult> CancelTc(int id)
        {
            try
            {
                //Student student = new Student();
                var student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == id && x.IsApplyforTC);
                if (student != null)
                {
                    student.IsApplyforTC = false;
                    StudentsRegistration studentReg = await _context.StudentsRegistrations.FirstOrDefaultAsync(x => x.ApplicationNumber == student.ApplicationNumber && x.IsApplyforTC);
                    if (studentReg != null) { studentReg.IsApplyforTC = false; }

                    StudentTcDetails studentTcDetails = await _context.Tbl_StudentTcDetails.FirstOrDefaultAsync(i => i.StudentId == student.StudentId);
                    if (studentTcDetails != null)
                    {
                        _context.Tbl_StudentTcDetails.Remove(studentTcDetails);
                    }
                    await _context.SaveChangesAsync();
                    return Json(new { msg = "TC has been cancelled", Success = true });
                }
                return Json(new { msg = "TC is not applied.", Success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Json(new { msg = "Some error occured while processing", Success = false });
            }
        }

        public async Task<JsonResult> ApplyTcToStudents(ApplyTCModel tcDetailsModel)
        {
            try
            {

                var tcId = await _context.DataListItems.Where(x => x.DataListItemName.Contains("TC")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
                StudentTcMaster studentTc = await _context.StudentTcMaster.FirstOrDefaultAsync();
                foreach (var item in tcDetailsModel.studentIdList)
                {
                    Student student = new Student();
                    student = await _context.Students.FirstOrDefaultAsync(x => x.StudentId == item && !x.IsApplyforTC);
                    if (student != null)
                    {
                        StudentsRegistration studentReg = await _context.StudentsRegistrations.FirstOrDefaultAsync(x => x.ApplicationNumber == student.ApplicationNumber && !x.IsApplyforTC);
                        if (studentReg != null)
                        {
                            if (tcDetailsModel.isCancelTC == false)
                            {
                                studentReg.IsApplyforTC = true;
                                studentReg.IsApplyforAdmission = false;
                                studentReg.IsApprove = 0;
                            }
                            else
                            {
                                studentReg.IsApplyforTC = false;
                            }
                        }
                    }
                    if (student != null && tcDetailsModel.isCancelTC == false)
                    {
                        student.IsApplyforTC = true;
                        student.IsApplyforAdmission = false;
                        //Note:We are change the datatype bool to int.
                        //student.IsApprove = false;

                        StudentTcDetails objStudentTcDetails = await _context.Tbl_StudentTcDetails.FirstOrDefaultAsync(i => i.StudentId == student.StudentId);
                        if (objStudentTcDetails == null)
                        {
                            var CurrentTCNo = studentTc.TcSeriesCurrentNo + 1;
                            var SchoolCode = studentTc.SchoolCode.ToString();
                            var studentTcDetails = new StudentTcDetails();
                            studentTcDetails.StudentId = Convert.ToInt32(item);
                            studentTcDetails.TcId = tcId;
                            studentTcDetails.ClassId = student.Class_Id;
                            studentTcDetails.BatchId = student.Batch_Id;
                            studentTcDetails.CreatedOn = DateTime.Now;
                            studentTcDetails.Ispaid = false;
                            studentTcDetails.TCSNo = $"{SchoolCode} TC/{CurrentTCNo}/{DateTime.Now.Year}";

                            _context.Tbl_StudentTcDetails.Add(studentTcDetails);
                            studentTc.TcSeriesCurrentNo = CurrentTCNo;


                        }

                        _context.SaveChanges();

                    }

                }

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return Json("Error occurred, please try again!");
            }

            return Json("TC has been provided");
        }

        [HttpPost]
        public JsonResult GetStudentsTcFeeDetails(StudentFeeInputModel studentFeeInputModel)
        {
            List<StudentTotalFeeViewModel> feeData = new List<StudentTotalFeeViewModel>();

            if (studentFeeInputModel.StudentId == 0 || string.IsNullOrEmpty(studentFeeInputModel.Batch) || string.IsNullOrEmpty(studentFeeInputModel.ClassName))
            {
                return Json(feeData, JsonRequestBehavior.AllowGet);
            }

            var studentDetails = _context.Students.Where(x => x.StudentId == studentFeeInputModel.StudentId && x.BatchName == studentFeeInputModel.Batch
                                && x.Class == studentFeeInputModel.ClassName).FirstOrDefault();
            //if (!string.IsNullOrEmpty(studentFeeInputModel.StudentId.ToString()))
            //{
            //    allStudents = allStudents.Where(x => x.StudentId == studentFeeInputModel.StudentId).ToList();
            //}
            //if (!string.IsNullOrEmpty(studentFeeInputModel.Batch))
            //{
            //    allStudents = allStudents.Where(x => x.BatchName == studentFeeInputModel.Batch).ToList();
            //}


            //if (!string.IsNullOrEmpty(studentFeeInputModel.ClassName))
            //{
            //    allStudents = allStudents.Where(x => x.Class == studentFeeInputModel.ClassName).ToList();
            //}

            if (studentDetails != null)
            {
                System.Data.Entity.DbSet<FeeHeadings> allFeeHeading = _context.FeeHeadings;
                var feePlan = _context.FeePlans.Where(x => x.FeeId == allFeeHeading.Where(f => f.FeeName == "TC").Select(f => f.FeeId).FirstOrDefault() && x.ClassName == studentDetails.Class && x.CategoryName == studentDetails.Category && x.BatchName == studentDetails.BatchName).FirstOrDefault();
                if (feePlan == null)
                {
                    return Json(feeData, JsonRequestBehavior.AllowGet);
                }

                var tblFeeReceipt = _context.TblFeeReceipts.Where(x => x.FeeHeadingIDs == Convert.ToString(feePlan.FeeId) && x.StudentId == studentDetails.StudentId).ToList();
                var totalFeeAmount = feePlan.FeeValue;
                var paidAmount = tblFeeReceipt.Any() ? tblFeeReceipt.Sum(x => string.IsNullOrEmpty(x.PaidAmount) ? 0 : Convert.ToInt64(x.PaidAmount)) : 0;
                var dueAmount = totalFeeAmount - paidAmount;

                feeData.Add(new StudentTotalFeeViewModel
                {
                    FeeName = feePlan.FeeName,
                    TotalAmount = totalFeeAmount,
                    PaidAmount = paidAmount,
                    PendingAmount = dueAmount
                });
            }

            return Json(feeData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTcStudentList()
        {
            return Json(new SelectList(_context.Students.Where(x => !x.IsDeleted && x.IsApplyforTC).ToList().OrderBy(x => x.Name).ToList(), "StudentId", "Name"), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GeNotApprovedStudentlist()
        {
            //return Json(new SelectList(_context.Students.Where(x => !x.IsDeleted && !x.IsApplyforTC && !x.IsApprove && x.IsApplyforAdmission && (x.IsAdmissionPaid == null || x.IsAdmissionPaid == false)).ToList().OrderBy(x => x.Name).ToList(), "StudentId", "Name"), JsonRequestBehavior.AllowGet);
            return Json(new SelectList(_context.Students.Where(x => !x.IsDeleted && !x.IsApplyforTC && x.IsApplyforAdmission && (x.IsAdmissionPaid == null || x.IsAdmissionPaid == false)).ToList().OrderBy(x => x.Name).ToList(), "StudentId", "Name"), JsonRequestBehavior.AllowGet);
        }

        private List<StudentFeePaidViewModel> getDifferentFeeList(List<FeePlans> feePlanes, int studentId, Student student, out List<int> addedFeeId)
        {
            addedFeeId = new List<int>();
            float allTotal = 0;
            List<StudentFeePaidViewModel> studentFeePaidlist = new List<StudentFeePaidViewModel>();
            string Months = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec";
            var allFeeHeading = _context.FeeHeadings.Where(x => x.Active == "0").ToList();
            var paidFeeHeadingIDs = _context.TblFeeReceipts.Where(x => x.StudentId == studentId)
                   .Select(v => v.FeeHeadingIDs).ToList();
            //bool moreThanOneFeeId = false;
            foreach (FeePlans feeplan in feePlanes)
            {
                if (feeplan.FeeId != 22)
                {
                    bool moreThanOneFeeId = false;

                    float total = 0;
                    FeeHeadings FeeHeading = allFeeHeading.FirstOrDefault(x => x.FeeId == feeplan.FeeId);
                    bool status = CheckStudentMonFee(studentId, FeeHeading.FeeId, student.Class);
                    StringBuilder htmlRow = new StringBuilder();
                    List<string> paidmonths = new List<string>();

                    StudentFeePaidViewModel studentFeePaidViewModel = new StudentFeePaidViewModel();
                    if (status == false)
                    {
                        continue;
                    }

                    if (feeplan.FeeName.Trim().ToLower() == "Enrollment Fee".Trim().ToLower())
                    {

                    }
                    if (feeplan.FeeName == "Other State Fee" || feeplan.FeeName == "Other State")
                    {

                    }
                    if (feeplan.FeeName == "Immigration Fee")
                    {

                    }
                    if (feeplan.FeeName == "Eligibility Fee")
                    {

                    }

                    string feeid = Convert.ToString(feeplan.FeeId);
                    string[] fullPaidFeeHeadingIDs = new string[50];
                    var tblFeeReceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentId && x.FeeHeadingIDs == feeid).ToList();
                    studentFeePaidViewModel.FeeHeading = feeplan.FeeName;
                    if (paidFeeHeadingIDs != null && paidFeeHeadingIDs.Count > 0)
                    {
                        foreach (var u in paidFeeHeadingIDs)
                        {
                            fullPaidFeeHeadingIDs = u.Split(',');
                            if (fullPaidFeeHeadingIDs != null && fullPaidFeeHeadingIDs.Length > 0)
                            {
                                //if (fullPaidFeeHeadingIDs.Contains("22"))
                                //    paidFeeHeadingIDs.Remove(u);

                                if (fullPaidFeeHeadingIDs.Contains(feeid))
                                {
                                    //paidFeeHeadingIDs.Remove(u);
                                    if (fullPaidFeeHeadingIDs.Length > 1)
                                    {
                                        moreThanOneFeeId = true;
                                    }
                                    break;
                                }
                            }
                            continue;
                        }

                    }
                    if (moreThanOneFeeId)
                    {
                        continue;

                    }
                    else
                    //}
                    {
                        if (tblFeeReceipt.Count == 0 && (FeeHeading.Jan == 1 || FeeHeading.Feb == 1 || FeeHeading.Mar == 1 || FeeHeading.Apr == 1 || FeeHeading.May == 1 || FeeHeading.Jun == 1 || FeeHeading.Jul == 1 || FeeHeading.Aug == 1
                                    || FeeHeading.Sep == 1 || FeeHeading.Oct == 1 || FeeHeading.Nov == 1 || FeeHeading.Dec == 1))
                        {
                            addedFeeId.Add(feeplan.FeeId);
                        }
                    }
                    if (Months.Contains("Jan") && FeeHeading.Jan == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jan) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();

                                paidmonths.Add("Jan");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;

                            paidmonths.Add("Jan");
                        }

                    }

                    if (Months.Contains("Feb") && FeeHeading.Feb == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Feb) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();

                                paidmonths.Add("Feb");
                            }
                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Feb");
                        }
                    }

                    if (Months.Contains("Mar") && FeeHeading.Mar == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Mar) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();

                                paidmonths.Add("Mar");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;

                            paidmonths.Add("Mar");
                        }

                    }

                    if (Months.Contains("Apr") && FeeHeading.Apr == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Apr) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Apr");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;

                            paidmonths.Add("Apr");
                        }

                    }

                    if (Months.Contains("May") && FeeHeading.May == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.May) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();

                                paidmonths.Add("May");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;

                            paidmonths.Add("May");
                        }

                    }

                    if (Months.Contains("Jun") && FeeHeading.Jun == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jun) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Jun");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Jun");
                        }

                    }

                    if (Months.Contains("Jul") && FeeHeading.Jul == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Jul) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Jul");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Jul");
                        }

                    }

                    if (Months.Contains("Aug") && FeeHeading.Aug == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Aug) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Aug");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Aug");
                        }

                    }

                    if (Months.Contains("Sep") && FeeHeading.Sep == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Sep) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Sep");
                            }
                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Sep");
                        }
                    }

                    if (Months.Contains("Oct") && FeeHeading.Oct == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Oct) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Oct");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Oct");
                        }
                    }

                    if (Months.Contains("Nov") && FeeHeading.Nov == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Nov) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Nov");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Nov");
                        }

                    }

                    if (Months.Contains("Dec") && FeeHeading.Dec == 1)
                    {
                        if (tblFeeReceipt.Count() != 0)
                        {
                            if (tblFeeReceipt.Any(x => x.Dec) && tblFeeReceipt.Any(x => x.BalanceAmt <= 0))
                            {

                            }
                            else
                            {
                                total = total + tblFeeReceipt.Select(x => x.BalanceAmt).LastOrDefault();
                                paidmonths.Add("Dec");
                            }

                        }
                        else
                        {
                            total = total + feeplan.FeeValue;
                            paidmonths.Add("Dec");
                        }

                    }
                    allTotal = allTotal + total;
                    if (total > 0)
                    {
                        studentFeePaidViewModel.Amount = total;
                        studentFeePaidViewModel.TotalAmount = allTotal;
                        studentFeePaidViewModel.Months = string.Join(",", paidmonths);
                        studentFeePaidlist.Add(studentFeePaidViewModel);
                    }
                }


            }

            if (addedFeeId.Count == 0)
            {
                studentFeePaidlist = null;
            }
            return studentFeePaidlist;
        }

        public JsonResult GetStudentLoginForFeesPaid(int Fee_id, int transportrange)
        {
            List<StudentFeePaidViewModel> studentFeePaidlist = new List<StudentFeePaidViewModel>();
            var activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");

            var feeplansdetails = _context.FeePlans.Where(x => x.Fee_configurationid == activeschool.Fee_configurationId).ToList();
            int studentId = Session["StudentId"] == null ? 0 : Convert.ToInt32(Session["StudentId"]);
            ConfigFeeDataViewModel configFeeDataViewModel = new ConfigFeeDataViewModel();

            string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            //var student = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            //StudentsRegistration student = new StudentsRegistration();
            var student = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);


            var _studentDetails = _context.Students.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);

            var allFeePlaness = feeplansdetails.Where(x => x.ClassId == student.Class_Id).ToList();
            var feereceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).Select(x => x.FeeHeadingIDs).ToList();
            List<FeePlans> allFeePlane = new List<FeePlans>();

            string[] fullPaidFeeHeadingIDs = new string[50];
            bool issecondinstallment = false;
            if (feereceipt != null && feereceipt.Count > 0)
            {
                foreach (var obj in feereceipt)
                {
                    fullPaidFeeHeadingIDs = obj.Split(',');
                    if (fullPaidFeeHeadingIDs != null && fullPaidFeeHeadingIDs.Length > 0)
                    {
                        foreach (var item in fullPaidFeeHeadingIDs)
                        {
                            if (item == "22")
                            {
                                issecondinstallment = true;
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                allFeePlane = allFeePlaness;
            }
            if (issecondinstallment)
            {
                foreach (var item in allFeePlaness)
                {
                    if (item.FeeId == 22)
                        continue;
                    else if (item.FeeId == 26)
                        continue;
                    else if (item.FeeId == 27)
                        continue;
                    else
                        allFeePlane.Add(item);
                }
            }

            if (allFeePlane.Count == 0)
            {
                allFeePlane = allFeePlaness;
            }

            //allFeePlane = allFeePlanes.Where(x => x.FeeId != 22 || x.FeeId != 26 || x.FeeId != 27).ToList();

            //1 - Admission Fee
            //2 - Transp
            var additionalinfo = new AdditionalInformation();
            List<FeePlans> feePlanes = new List<FeePlans>();

            List<int> addedFeeId = new List<int>();

            var Batches = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Batch").DataListId.ToString()).ToList();

            var activebatch = Batches.FirstOrDefault(x => x.Status == "Active");
            var activeBatches = _context.Tbl_Batches.Where(x => x.IsActiveForPayments).Select(x => x.Batch_Id).ToList();
            //var activebatch = activeBatches.Contains(x => x.Batch_Id);

            if (Fee_id == 1)
            {
                //feePlanes = allFeePlanes.Where(x => x.ClassId == student.Class_Id  && x.CategoryId == student.Category_Id).ToList();
                feePlanes = allFeePlane.Where(x => x.ClassId == student.Class_Id && activeBatches.Contains(x.Batch_Id)).ToList();
                if (feePlanes != null)
                {
                    //studentFeePaidlist = getDifferentFeeList(feePlanes, _studentDetails.StudentId, student, out addedFeeId);
                    studentFeePaidlist = getDifferentFeeList(feePlanes, _studentDetails.StudentId, _studentDetails, out addedFeeId);
                }
                else
                {
                    studentFeePaidlist = null;
                }
            }
            else if (Fee_id == 2)
            {
                bool isTransportFeeFlow = false;
                //var transportfeeplan = _context.FeePlans.Where(x => x.FeeType_Id == 223).ToList();
                //var Alltransportoption = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Transport Options").DataListId.ToString()).ToList();
                //var KmDistance = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "KmDistance").DataListId.ToString()).ToList();
                var studentregistration = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == student.ApplicationNumber);
                additionalinfo = _context.AdditionalInformations.FirstOrDefault(x => x.ApplicationNumber == studentregistration.ApplicationNumber);
                var transport = _context.TblTransportReducedAmount.ToList();
                var transrange = transport.FirstOrDefault(x => x.ReducedAmount_Id == transportrange);
                //var transportoptionid = Alltransportoption.FirstOrDefault(x => x.DataListItemName.ToLower() == additionalinfo.TransportFacility.ToLower())?.DataListItemId;

                //var KMDistance_Id = KmDistance.FirstOrDefault(x => x.DataListItemName.ToLower() == additionalinfo.DistancefromSchool.ToString().ToLower())?.DataListItemId;

                //feePlanes = allFeePlanes.Where(x => x.TransportOpt_Id == transportoptionid && x.KmDistance_Id == KMDistance_Id && x.FeeType_Id == 223).ToList();
                //feePlanes = allFeePlanes.Where(x => x.FeeType_Id == 223).ToList();

                //  TransportFee---> TransportFee. flow
                var allFeePlanes = _context.FeePlans.ToList();
                if (additionalinfo.DistancefromSchool != 0)
                {
                    var onlyTransportFee = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223
                    && x.KmDistance_Id == null && x.FeeValue == 0).ToList().FirstOrDefault();
                    if (onlyTransportFee != null)
                    {
                        isTransportFeeFlow = true;
                        float firstKMFee = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223 && x.KmDistance_Id == 227).Select(x => x.FeeValue).FirstOrDefault();

                        float additionalKMFee = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223 && x.KmDistance_Id == 228).Select(x => x.FeeValue).FirstOrDefault();

                        float pickupadditionalkm = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223 && x.KmDistance_Id == 240).Select(x => x.FeeValue).FirstOrDefault();

                        float PDFirstkmAugtoapr = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223 && x.KmDistance_Id == 241).Select(x => x.FeeValue).FirstOrDefault();

                        float PFirstKmAugtoApr = allFeePlanes.Where(x => x.FeeName.ToString().Trim().ToLower() == "transport fee" && x.FeeType_Id == 223 && x.KmDistance_Id == 242).Select(x => x.FeeValue).FirstOrDefault();

                        float allTotal = 0;

                        if (firstKMFee != 0 || additionalKMFee != 0)
                        {
                            //var KmDistance = _context.DataListItems.First(x => x.DataListItemName == "FirstKm")?.DataListItemId;
                            //var KmRange = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == 28)?.DataListItemName;

                            var kmRange = _context.DataListItems.First(x => x.DataListItemId == 226)?.DataListItemName;

                            if (transrange.ReducedAmount_Id == 1)//julytoapril
                            {
                                if (additionalinfo.TransportOptions == "both")
                                {
                                    if (additionalinfo.DistancefromSchool <= float.Parse(kmRange))
                                    {
                                        allTotal = firstKMFee;
                                    }
                                    else
                                    {
                                        var distance = additionalinfo.DistancefromSchool;
                                        float totalAdditionFee = 0;

                                        if (distance > 2 && distance <= 4)
                                            totalAdditionFee = 1 * float.Parse(transrange.Amount);
                                        else if (distance > 4 && distance <= 6)
                                            totalAdditionFee = 2 * float.Parse(transrange.Amount);
                                        else if (distance > 6 && distance <= 8)
                                            totalAdditionFee = 3 * float.Parse(transrange.Amount);
                                        else if (distance > 8 && distance <= 10)
                                            totalAdditionFee = 4 * float.Parse(transrange.Amount);
                                        else if (distance > 10 && distance <= 12)
                                            totalAdditionFee = 5 * float.Parse(transrange.Amount);

                                        //var additionalKM = additionalinfo.DistancefromSchool / float.Parse(kmRange);
                                        //var totalAdditionFee = additionalKM * additionalKMFee;
                                        allTotal = firstKMFee + totalAdditionFee;
                                    }
                                }
                                else
                                {
                                    if (additionalinfo.DistancefromSchool <= float.Parse(kmRange))
                                    {
                                        allTotal = pickupadditionalkm;
                                    }
                                    else
                                    {
                                        var distance = additionalinfo.DistancefromSchool;
                                        float totalAdditionFee = 0;

                                        if (distance > 2 && distance <= 4)
                                            totalAdditionFee = 1 * float.Parse(transrange.Amount);
                                        else if (distance > 4 && distance <= 6)
                                            totalAdditionFee = 2 * float.Parse(transrange.Amount);
                                        else if (distance > 6 && distance <= 8)
                                            totalAdditionFee = 3 * float.Parse(transrange.Amount);
                                        else if (distance > 8 && distance <= 10)
                                            totalAdditionFee = 4 * float.Parse(transrange.Amount);
                                        else if (distance > 10 && distance <= 12)
                                            totalAdditionFee = 5 * float.Parse(transrange.Amount);

                                        //var additionalKM = additionalinfo.DistancefromSchool / float.Parse(kmRange);
                                        //var totalAdditionFee = additionalKM * additionalKMFee;
                                        allTotal = pickupadditionalkm + totalAdditionFee;
                                    }
                                }
                            }
                            else if (transrange.ReducedAmount_Id == 2)//AugtoApr
                            {
                                if (additionalinfo.TransportOptions == "both")
                                {
                                    if (additionalinfo.DistancefromSchool <= float.Parse(kmRange))
                                    {
                                        allTotal = PDFirstkmAugtoapr;
                                    }
                                    else
                                    {
                                        var distance = additionalinfo.DistancefromSchool;
                                        float totalAdditionFee = 0;

                                        if (distance > 2 && distance <= 4)
                                            totalAdditionFee = 1 * float.Parse(transrange.Amount);
                                        else if (distance > 4 && distance <= 6)
                                            totalAdditionFee = 2 * float.Parse(transrange.Amount);
                                        else if (distance > 6 && distance <= 8)
                                            totalAdditionFee = 3 * float.Parse(transrange.Amount);
                                        else if (distance > 8 && distance <= 10)
                                            totalAdditionFee = 4 * float.Parse(transrange.Amount);
                                        else if (distance > 10 && distance <= 12)
                                            totalAdditionFee = 5 * float.Parse(transrange.Amount);

                                        //var additionalKM = additionalinfo.DistancefromSchool / float.Parse(kmRange);
                                        //var totalAdditionFee = additionalKM * additionalKMFee;
                                        allTotal = PDFirstkmAugtoapr + totalAdditionFee;
                                    }
                                }
                                else
                                {
                                    if (additionalinfo.DistancefromSchool <= float.Parse(kmRange))
                                    {
                                        allTotal = PFirstKmAugtoApr;
                                    }
                                    else
                                    {
                                        var distance = additionalinfo.DistancefromSchool;
                                        float totalAdditionFee = 0;

                                        if (distance > 2 && distance <= 4)
                                            totalAdditionFee = 1 * float.Parse(transrange.Amount);
                                        else if (distance > 4 && distance <= 6)
                                            totalAdditionFee = 2 * float.Parse(transrange.Amount);
                                        else if (distance > 6 && distance <= 8)
                                            totalAdditionFee = 3 * float.Parse(transrange.Amount);
                                        else if (distance > 8 && distance <= 10)
                                            totalAdditionFee = 4 * float.Parse(transrange.Amount);
                                        else if (distance > 10 && distance <= 12)
                                            totalAdditionFee = 5 * float.Parse(transrange.Amount);

                                        //var additionalKM = additionalinfo.DistancefromSchool / float.Parse(kmRange);
                                        //var totalAdditionFee = additionalKM * additionalKMFee;
                                        allTotal = PFirstKmAugtoApr + totalAdditionFee;
                                    }
                                }
                            }


                            float totalamount = allTotal / 2;

                            var feeheading = _context.FeeHeadings.ToList();
                            List<FeeHeadings> feeHeadings = new List<FeeHeadings>();
                            //List<StudentFeePaidViewModel> studentFeePaidlist1 = new List<StudentFeePaidViewModel>();

                            foreach (var item in feeheading)
                            {
                                if (item.FeeId == 21 || item.FeeId == 25)
                                {
                                    StudentFeePaidViewModel studentFeePaidViewModel = new StudentFeePaidViewModel();
                                    studentFeePaidViewModel.Amount = totalamount;
                                    studentFeePaidViewModel.TotalAmount = totalamount;
                                    studentFeePaidViewModel.FeeHeading = item.FeeName;
                                    studentFeePaidViewModel.FeeheadingId = item.FeeId;
                                    studentFeePaidlist.Add(studentFeePaidViewModel);
                                }
                            }
                            //var feereceipt = _context.TblFeeReceipts.Where(x => x.StudentId == studentId).ToList();
                            //if (studentFeePaidlist1.Count > 0)
                            //{
                            //    if (feereceipt.Count > 0)
                            //    {
                            //        foreach (var item in feereceipt)
                            //        {
                            //            if (item.FeeHeadingIDs == "21" || item.FeeHeadingIDs == "25" || item.FeeHeadingIDs == "21,25" || item.FeeHeadingIDs == "25,21")
                            //            {
                            //                var feerece = item.FeeHeadingIDs.Split(',');
                            //                if (feerece.Length > 0)
                            //                {
                            //                    foreach (var obj in feerece)
                            //                    {
                            //                        StudentFeePaidViewModel tblFee = new StudentFeePaidViewModel();
                            //                        tblFee.FeeheadingId = Convert.ToInt32(obj);
                            //                        studentFeePaidlist.Add(tblFee);

                            //                    }
                            //                }
                            //                else
                            //                {
                            //                    studentFeePaidlist.Add(item);
                            //                }
                            //            }

                            //        }
                            //    }
                            //}
                            //StudentFeePaidViewModel studentFeePaidViewModel = new StudentFeePaidViewModel();
                            //studentFeePaidViewModel.Ammount = allTotal * 12;
                            //studentFeePaidViewModel.TotalAmmount = allTotal * 12;
                            //studentFeePaidViewModel.FeeHeading = "Transport Fee";
                            //studentFeePaidlist.Add(studentFeePaidViewModel);
                        }
                    }
                }
                else
                {
                    return null;
                }

                // Other than TransportFee---> 1st.2nd,3rd etc.. flow
                if (!isTransportFeeFlow)
                {

                    feePlanes = allFeePlanes.Where(x => x.FeeName.Trim().ToLower() != "Transport Fee" && x.FeeType_Id == 223
                    && x.KmDistance_Id == null && x.FeeValue != 0).ToList();

                    //studentFeePaidlist = getDifferentFeeList(feePlanes, studentId, student, out addedFeeId);
                    studentFeePaidlist = getDifferentFeeList(feePlanes, studentId, _studentDetails, out addedFeeId);
                }
            }
            //List<FeePlans> feePlanes = allFeePlanes.Where(x => x.ClassName == student.Class && x.CategoryName == student.Category && x.BatchName == "2021-2035").ToList();

            //List<FeePlans> feePlanes = allFeePlanes.Where(x => x.ClassId == student.Class_Id && x.FeeName != "Admission Fee").ToList();



            //int SNO = 1;

            ////GetStudentOldAmmount fees check
            //TblFeeReceipts tblFeeReceiptOld = _TblFeeReceiptsRepository.GetAll().LastOrDefault(x => x.StudentId == studentId);
            //float OldBalance = tblFeeReceiptOld == null ? 0 : tblFeeReceiptOld.OldBalance;
            //if (OldBalance > 0)
            //{
            //    StudentFeePaidViewModel studentFeePaidOld = new StudentFeePaidViewModel();
            //    studentFeePaidOld.FeeHeading = "Due Fees";
            //    studentFeePaidOld.Ammount = OldBalance;
            //    allTotal += OldBalance;
            //    studentFeePaidOld.Months = string.Empty;
            //    studentFeePaidlist.Add(studentFeePaidOld);
            //}



            //DueFeeViewModel feeReceiptViewModel = new DueFeeViewModel();
            //IEnumerable<TblDueFee> feeReceipt = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == student.StudentId && x.ClassName == student.Class).ToList();
            IEnumerable<TblDueFee> feeReceipt = _TblDueFeeRepository.GetAll().Where(x => x.StudentId == student.StudentRegisterID && x.ClassName == student.Class).ToList();

            if (!feeReceipt.Any())
            {

                //var test = feePlanes.Select(x => x.FeeValue);
                //string feeMonths = string.Join(",", months);
                ////string FeeHeadingAmt = string.Join(",", feePlanes.Select(x => x.FeeValue).ToArray());
                //float[] FeeHeadingAmtValues = feePlanes.Select(x => x.FeeValue).ToArray();
                List<string> feeHeadingArray = new List<string>();
                List<string> feeHeadingAmounts = new List<string>();
                foreach (int id in addedFeeId)
                {
                    feeHeadingArray.Add(feePlanes.Where(x => x.FeeId == id).Select(x => x.FeeName).FirstOrDefault());
                    feeHeadingAmounts.Add(feePlanes.Where(x => x.FeeId == id).Select(x => x.FeeValue).FirstOrDefault().ToString());
                }

                string feeHeadings = string.Join(",", feeHeadingArray);
                string feeHeadingAmt = string.Join(",", feeHeadingAmounts);

                //float[] collectionfee = feeReceiptViewModel.collectFees;


                var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "semester").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == _studentDetails.Class.ToLower());
                //var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "semester").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == student.Class.ToLower());
                var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId == student.Category_Id);
                //List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();                
                var unicNumber = Guid.NewGuid();
                foreach (int feeID in addedFeeId)
                {
                    //var allFeeHeading = _context.FeeHeadings.ToList();
                    FeeHeadings feeDetails = _context.FeeHeadings.FirstOrDefault(x => x.FeeId == feeID);
                    if (feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue == 0)
                    {
                        continue;
                    }

                    TblDueFee tblFeeReceipts = new TblDueFee();
                    //FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == item);                    
                    tblFeeReceipts.FeeHeadingId = feeDetails == null ? 0 : feeDetails.FeeId;
                    tblFeeReceipts.FeeHeading = feeDetails == null ? "DueBalance" : feeDetails.FeeName;
                    //tblFeeReceipts.Other1 = feeDetails.FeeName;
                    //tblFeeReceipts.Other2 = Convert.ToString(feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue);
                    //tblFeeReceipts.Other3 = Convert.ToString(feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue);
                    ////tblFeeReceipts.Other4 = feeReceiptViewModel.DueAmountYesNo;
                    //tblFeeReceipts.Other4 = "no";
                    //tblFeeReceipts.BalanceAmt = 0;
                    //tblFeeReceipts.OldBalance = 0;
                    //tblFeeReceipts.BankName = null;
                    //tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
                    //tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
                    //tblFeeReceipts.ClassId = classDetail.DataListItemId;
                    //tblFeeReceipts.ClassName = classDetail.DataListItemName;

                    //tblFeeReceipts.Concession = 0;
                    //tblFeeReceipts.ConcessionAmt = 0;
                    //tblFeeReceipts.LateFee = 0;
                    tblFeeReceipts.PaidMonths = feeDetails == null ? "" : PaidMonthList(feeDetails, months);
                    tblFeeReceipts.PayHeadings = feeHeadings;
                    //tblFeeReceipts.PaymentMode = null;
                    //tblFeeReceipts.ReceiptAmt = 0;
                    //tblFeeReceipts.Remark = null;
                    //tblFeeReceipts.StudentId = (int)student.StudentId;
                    tblFeeReceipts.StudentId = _studentDetails.StudentId;
                    tblFeeReceipts.StudentName = student.Name;
                    tblFeeReceipts.TotalFee = feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue;

                    tblFeeReceipts.FeePaids = feeHeadingAmt;
                    tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
                    tblFeeReceipts.Course = student.Class;
                    //tblFeeReceipts.BatchName = student.Batch;
                    //tblFeeReceipts.CourseSpecialization = student.Specialization;

                    tblFeeReceipts.Jan = feeDetails.Jan == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Feb = feeDetails.Feb == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Mar = feeDetails.Mar == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Apr = feeDetails.Apr == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.May = feeDetails.May == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Jun = feeDetails.Jun == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Jul = feeDetails.Jul == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Aug = feeDetails.Aug == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Sep = feeDetails.Sep == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Oct = feeDetails.Oct == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Nov = feeDetails.Nov == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";
                    tblFeeReceipts.Dec = feeDetails.Dec == 1 ? feePlanes.FirstOrDefault(x => x.FeeId == feeID).FeeValue.ToString() : "0";

                    _TblDueFeeRepository.Insert(tblFeeReceipts);
                    _TblDueFeeRepository.Save();
                }
            }

            return Json(studentFeePaidlist, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AmountTransferGateway(FeeReceiptViewModel feeReceiptViewModel, bool isStudent)
        {
            try
            {
                string StudentId = feeReceiptViewModel.StudentId.ToString();
                string Amount = feeReceiptViewModel.ReceiptAmt.ToString();
                string FeeHeadingAmt = string.Join(",", feeReceiptViewModel.FeeHeadingAmt);
                float[] FeeHeadingAmtValues = feeReceiptViewModel.FeeHeadingAmt;
                string feeHeadings = string.Join(",", feeReceiptViewModel.FeeHeadings);
                float[] collectionfee = feeReceiptViewModel.collectFees;
                var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var Categorylist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();


                PaymentViewModels objPaymentViewModels = new PaymentViewModels();

                if (isStudent == true)
                {
                    //var  studentData =_context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);
                    var studentData = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);
                    var Section = _context.DataListItems.Where(x => x.DataListName == "Section" && x.DataListItemId == studentData.Section_Id).Select(x => x.DataListItemName).FirstOrDefault();
                    studentData.Class = Classlist.FirstOrDefault(x => x.DataListItemId == studentData.Class_Id)?.DataListItemName;
                    studentData.Category = Categorylist.FirstOrDefault(x => x.DataListItemId == studentData.Category_Id)?.DataListItemName;
                    //Classes classDetail = _ClassesRepository.GetAll().FirstOrDefault(x => x.ClassName.ToLower() == feeReceiptViewModel.ClassName.ToLower());
                    var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "semester").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == studentData.Class.ToLower());

                    objPaymentViewModels.BloodGroup = studentData.BloodGroup;
                    objPaymentViewModels.Category = studentData.Category;
                    objPaymentViewModels.Class = studentData.Class;
                    objPaymentViewModels.Section = studentData.Section;
                    objPaymentViewModels.RoleNumber = studentData.RollNo.ToString();
                    objPaymentViewModels.DOB = studentData.DOB;
                    objPaymentViewModels.Gender = studentData.Gender;
                    objPaymentViewModels.MotherTongue = studentData.MotherTongue;
                    objPaymentViewModels.Name = studentData.Name;
                    objPaymentViewModels.Nationality = studentData.Nationality;
                    objPaymentViewModels.POB = studentData.POB;
                    objPaymentViewModels.Religion = studentData.Religion;
                    objPaymentViewModels.Nationality = studentData.Nationality;
                    objPaymentViewModels.ApplicationNumber = studentData.ApplicationNumber;
                    objPaymentViewModels.Concession = feeReceiptViewModel.Concession;
                    objPaymentViewModels.ConcessionAmt = feeReceiptViewModel.ConcessionAmt;
                    objPaymentViewModels.Email = studentData.ParentEmail;

                }
                else
                {
                    var studentData = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);

                    studentData.Class = Classlist.FirstOrDefault(x => x.DataListItemId == studentData.Class_Id)?.DataListItemName;
                    studentData.Category = Categorylist.FirstOrDefault(x => x.DataListItemId == studentData.Category_Id)?.DataListItemName;
                    //Classes classDetail = _ClassesRepository.GetAll().FirstOrDefault(x => x.ClassName.ToLower() == feeReceiptViewModel.ClassName.ToLower());
                    var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "semester").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == studentData.Class.ToLower());
                    var Section = _context.DataListItems.Where(x => x.DataListName == "Section" && x.DataListItemId == studentData.Section_Id).Select(x => x.DataListItemName).FirstOrDefault();
                    objPaymentViewModels.BloodGroup = studentData.BloodGroup;
                    objPaymentViewModels.Category = studentData.Category;
                    objPaymentViewModels.Class = studentData.Class;
                    objPaymentViewModels.Section = Section.ToString(); ;
                    objPaymentViewModels.RoleNumber = studentData.RollNo.ToString();
                    objPaymentViewModels.DOB = studentData.DOB;
                    objPaymentViewModels.Gender = studentData.Gender;
                    objPaymentViewModels.MotherTongue = studentData.MotherTongue;
                    objPaymentViewModels.Name = studentData.Name;
                    objPaymentViewModels.Nationality = studentData.Nationality;
                    objPaymentViewModels.POB = studentData.POB;
                    objPaymentViewModels.Religion = studentData.Religion;
                    objPaymentViewModels.Nationality = studentData.Nationality;
                    objPaymentViewModels.ApplicationNumber = studentData.ApplicationNumber;
                    objPaymentViewModels.Email = studentData.ParentEmail;

                }
                //var studentData = _context.Students.FirstOrDefault(x => x.StudentId == feeReceiptViewModel.StudentId);

                //StudentCategorys categoryDetail = _StudentCategorysRepository.GetAll().FirstOrDefault(x => x.CategoryName.ToLower() == feeReceiptViewModel.CategoryName.ToLower());
                //var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == studentData.Category.ToLower());
                //var categoryDetail1 = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == feeReceiptViewModel.CategoryName.ToLower());


                List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();
                int i = 0;
                //int j = 0;
                var unicNumber = Guid.NewGuid();
                var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                StringBuilder sbfeeidwithamount = new StringBuilder();
                foreach (string item in feeReceiptViewModel.FeeHeadings)
                {
                    if (FeeHeadingAmt[i] == 0)
                    {
                        i++;
                        continue;
                    }
                    FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == item);
                    int FeeHeadingId = feeHeading == null ? 0 : feeHeading.FeeId;
                    sbfeeidwithamount.Append(FeeHeadingId.ToString() + "~" + feeReceiptViewModel.FeeHeadingAmt[i].ToString() + ",");
                    i++;
                }
                #region FeeInsert
                //foreach (string item in feeReceiptViewModel.FeeHeadings)
                //{

                //    if (FeeHeadingAmt[i] == 0)
                //    {
                //        i++;
                //        continue;
                //    }

                //    TblFeeReceiptsAudit tblFeeReceipts = new TblFeeReceiptsAudit();
                //    FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeName == item);
                //    tblFeeReceipts.FeeHeadingId = feeHeading == null ? 0 : feeHeading.FeeId;
                //    tblFeeReceipts.Other1 = feeHeading == null ? "DueBalance" : feeHeading.FeeName;
                //    tblFeeReceipts.Other2 = Convert.ToString(FeeHeadingAmtValues[j]);
                //    tblFeeReceipts.Other3 = Convert.ToString(collectionfee[j]);
                //    tblFeeReceipts.Other4 = feeReceiptViewModel.DueAmountYesNo;
                //    tblFeeReceipts.BalanceAmt = feeReceiptViewModel.BalanceAmt;  //remaining fee
                //    tblFeeReceipts.OldBalance = feeReceiptViewModel.BalanceAmt; //remaing fee
                //    tblFeeReceipts.BankName = feeReceiptViewModel.BankName;
                //    tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
                //    tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
                //    tblFeeReceipts.ClassId = classDetail.DataListItemId;
                //    tblFeeReceipts.ClassName = classDetail.DataListItemName;
                //    tblFeeReceipts.TrackId = trackId;

                //    tblFeeReceipts.Concession = feeReceiptViewModel.Concession;
                //    tblFeeReceipts.ConcessionAmt = feeReceiptViewModel.ConcessionAmt;
                //    tblFeeReceipts.LateFee = feeReceiptViewModel.LateFee;
                //    // tblFeeReceipts.PaidMonths = PaidMonthList(feeHeading, feeReceiptViewModel.Selectedmonths);
                //    tblFeeReceipts.PaidMonths = "Jun";
                //    tblFeeReceipts.PayHeadings = feeHeadings;
                //    tblFeeReceipts.PaymentMode = feeReceiptViewModel.PaymentMode;
                //    tblFeeReceipts.ReceiptAmt = feeReceiptViewModel.ReceiptAmt; //Collect fee
                //    tblFeeReceipts.Remark = feeReceiptViewModel.Remark;
                //    tblFeeReceipts.StudentId = feeReceiptViewModel.StudentId;
                //    tblFeeReceipts.StudentName = studentData.StudentName;
                //    tblFeeReceipts.TotalFee = feeReceiptViewModel.TotalFee;
                //    tblFeeReceipts.FeePaids = FeeHeadingAmt;
                //    tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
                //    tblFeeReceipts.Course = studentData.Course;
                //    tblFeeReceipts.BatchName = studentData.Batch;
                //    tblFeeReceipts.CourseSpecialization = studentData.Specialization;
                //    j++;
                //    //if (feeMonths.Contains("Jan"))
                //    //{
                //    tblFeeReceipts.Jan = true;
                //    //}

                //    //if (feeMonths.Contains("Feb"))
                //    //{
                //    tblFeeReceipts.Feb = true;
                //    //}
                //    //if (feeMonths.Contains("Mar"))
                //    //{
                //    tblFeeReceipts.Mar = true;
                //    //}
                //    //if (feeMonths.Contains("Apr"))
                //    //{
                //    tblFeeReceipts.Apr = true;
                //    //}
                //    //if (feeMonths.Contains("May"))
                //    //{
                //    tblFeeReceipts.May = true;
                //    //}
                //    //if (feeMonths.Contains("Jun"))
                //    //{
                //    tblFeeReceipts.Jun = true;
                //    //}
                //    //if (feeMonths.Contains("Jul"))
                //    //{
                //    tblFeeReceipts.Jul = true;
                //    //}
                //    //if (feeMonths.Contains("Aug"))
                //    //{
                //    tblFeeReceipts.Aug = true;
                //    //}
                //    //if (feeMonths.Contains("Sep"))
                //    //{
                //    tblFeeReceipts.Sep = true;
                //    //}
                //    //if (feeMonths.Contains("Oct"))
                //    //{
                //    tblFeeReceipts.Oct = true;
                //    //}
                //    //if (feeMonths.Contains("Nov"))
                //    //{
                //    tblFeeReceipts.Nov = true;
                //    //}
                //    //if (feeMonths.Contains("Dec"))
                //    //{
                //    tblFeeReceipts.Dec = true;
                //    //}

                //    //var log = "FeeHeadingId = " + feeHeading.FeeId +
                //    //",  Other1 = " + feeHeading.FeeName +
                //    //",  Other2 = " + Convert.ToString(FeeHeadingAmtValues[j]) +
                //    //",  Other3 = " + Convert.ToString(collectionfee[j]) +
                //    //",  Other4 = " + feeReceiptViewModel.DueAmountYesNo +
                //    //",  BalanceAmt = " + feeReceiptViewModel.BalanceAmt +
                //    //",  OldBalance = " + feeReceiptViewModel.BalanceAmt +
                //    //",  BankName = " + feeReceiptViewModel.BankName +
                //    //",  CategoryId = " + categoryDetail.DataListItemId +
                //    //",  CategoryName = " + categoryDetail.DataListItemName +
                //    //",  ClassId = " + classDetail.DataListItemId +
                //    //",  ClassName = " + classDetail.DataListItemName +
                //    //",  ClassName = " + trackId +
                //    //",  Concession = " + feeReceiptViewModel.Concession +
                //    //",  ConcessionAmt = " + feeReceiptViewModel.ConcessionAmt +
                //    //",  LateFee = " + feeReceiptViewModel.LateFee +
                //    //",  ts.PaidMonths = " + PaidMonthList(feeHeading, feeReceiptViewModel.Selectedmonths) +
                //    //",  PaidMonths = " + "Jun" +
                //    //",  PayHeadings = " + feeHeadings +
                //    //",  PaymentMode = " + feeReceiptViewModel.PaymentMode +
                //    //",  ReceiptAmt = " + feeReceiptViewModel.ReceiptAmt + //Collect fee
                //    //",  Remark = " + feeReceiptViewModel.Remark +
                //    //",  StudentId = " + feeReceiptViewModel.StudentId +
                //    //",  StudentName = " + studentData.StudentName +
                //    //",  TotalFee = " + feeReceiptViewModel.TotalFee +
                //    //",  FeePaids = " + FeeHeadingAmt +
                //    //",  FeeReceiptsOneTimeCreator = " + unicNumber.ToString() +
                //    //",  Course = " + feeReceiptViewModel.Course +
                //    //",  BatchName = " + studentData.BatchName +
                //    //",  CourseSpecialization = " + studentData.Specialization;

                //    //TransactionLog(log, 2);
                //    _TblFeeReceiptsRepositoryAudit.Insert(tblFeeReceipts);
                //    _TblFeeReceiptsRepositoryAudit.Save();
                //}
                #endregion

                var Batchdetails = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true).Select(x => x.Batch_Name).FirstOrDefault();

                objPaymentViewModels.StudentId = StudentId;
                objPaymentViewModels.TCBal = Amount;
                objPaymentViewModels.FeeHeadings = feeHeadings.ToString();
                objPaymentViewModels.Feeheadingamt = sbfeeidwithamount.ToString();
                // objPaymentViewModels.Batch = "2021-2035";
                objPaymentViewModels.Batch = Batchdetails.ToString();
                TempData["objPaymentViewModels"] = objPaymentViewModels;

            }
            catch (Exception ex)
            {
                ex.ToString();

                //  writelog.ExceptionLogFile("Exception in HostedBuy HTTP ", ex.Message.ToString());
            }
            //  return RedirectToAction("StudnetPaymentSummary");
            //  return View("StudnetPaymentSummary", ObjPaymentResultModels);
            return Json("");
            //  return Json("");
        }

        //public ActionResult payduefeeamount()
        //{

        //}

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


    }
}

