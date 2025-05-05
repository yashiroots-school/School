using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using com.awl.MerchantToolKit;
using SchoolManagement.Website.Models;
using SchoolManagement.Data.Models;
using System.Data.Entity;
using EmployeeManagement.Repository;
using System.Globalization;
using System.Configuration;
using SchoolManagement.Website.ViewModels;
using System.Text;
using System.IO;
using Razorpay.Api;
using System.Net.Mail;
using System.Net;
using SchoolManagement.Website.Models.Payment;

namespace SchoolManagement.Website.Controllers
{
    public class PaymentV2Controller : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private IRepository<tbl_PaymentTransactionDetails> objrep_tbl_PaymentTransactionDetails = null;
        private IRepository<tbl_PaymentTransactionFeeDetails> objrep_tbl_PaymentTransactionFeeDetails = null;
        private IRepository<TblFeeReceipts> _TblFeeReceiptsRepository = null;
        private IRepository<FeeHeadings> _FeeHeadingsRepository = null;
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PaymentController));
        // GET: Payment
        public ActionResult Index()
        {
            return View();
        }
        public PaymentV2Controller()
        {
            objrep_tbl_PaymentTransactionDetails = new Repository<tbl_PaymentTransactionDetails>();
            objrep_tbl_PaymentTransactionFeeDetails = new Repository<tbl_PaymentTransactionFeeDetails>();
            _TblFeeReceiptsRepository = new Repository<TblFeeReceipts>();
            _FeeHeadingsRepository = new Repository<FeeHeadings>();
        }
        public ActionResult PaymentProcess()
        {
            PaymentViewModels objPaymentViewModels = (PaymentViewModels)TempData["objPaymentViewModels"];
            var ClassList = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
           

            PaymentResultModels ObjPaymentResultModels = new PaymentResultModels();
            if (objPaymentViewModels != null)
            {
                var studnentID = objPaymentViewModels.StudentId;
                var classdetails = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId;
                ViewBag.PaymentStudentName = objPaymentViewModels.Name;
                ObjPaymentResultModels.StudentName = objPaymentViewModels.Name;
                
                ViewBag.PaymentStudentClass = objPaymentViewModels.Class;
                ObjPaymentResultModels.Class = objPaymentViewModels.Class;
                ViewBag.PaymentStudentCategory = objPaymentViewModels.Category;
                ObjPaymentResultModels.Category = objPaymentViewModels.Category;
                ViewBag.PaymentStudentRoleNumber = objPaymentViewModels.RoleNumber;
                ObjPaymentResultModels.RoleNumber = objPaymentViewModels.RoleNumber;
                ViewBag.PaymentStudentTCBal = objPaymentViewModels.TCBal;
                ObjPaymentResultModels.TCBal = objPaymentViewModels.TCBal;
                ObjPaymentResultModels.FeeHeadings = objPaymentViewModels.FeeHeadings;
                ObjPaymentResultModels.Feeheadingamt = objPaymentViewModels.Feeheadingamt;
                ObjPaymentResultModels.studentid = studnentID;
                ObjPaymentResultModels.Concession = objPaymentViewModels.Concession;
                ObjPaymentResultModels.ConcessionAmt = objPaymentViewModels.ConcessionAmt;

                string PaymentAmount = ObjPaymentResultModels.TCBal;
          
                decimal payAmount = 0;
                payAmount = Convert.ToDecimal(PaymentAmount);
                payAmount = payAmount * 100;
                Guid g = Guid.NewGuid();

                var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                InputPayment objInputPayment = new InputPayment();
                objInputPayment.OrderId = trackId.ToString();

                if (objPaymentViewModels.FeeHeadings == "Transport Fee,Transport Fee(IIndTerm)" || objPaymentViewModels.FeeHeadings == "Transport Fee" || objPaymentViewModels.FeeHeadings == "Transport Fee(IIndTerm)" || objPaymentViewModels.FeeHeadings == "Transport Fee(IIndTerm),Transport Fee")
                {
                    objInputPayment.Mid = ConfigurationManager.AppSettings["SchoolTransportWID"];
                    objInputPayment.Enckey = ConfigurationManager.AppSettings["SchoolTransportkey"];
                    objInputPayment.ResponseUrl = ConfigurationManager.AppSettings["TrabsportUrl"];
                }

                else if (classdetails != null && classdetails.ToString() == "207" || classdetails.ToString() == "208" || classdetails.ToString() == "209" || classdetails.ToString() == "210" && objPaymentViewModels.FeeHeadings != "Transport Fee")
                {
                    //PRE PRIMARY SCHOOL
                    objInputPayment.Mid = ConfigurationManager.AppSettings["PreSchoolPaymentWID"];
                    objInputPayment.Enckey = ConfigurationManager.AppSettings["PreSchoolPaymentkey"];
                    objInputPayment.ResponseUrl = ConfigurationManager.AppSettings["PreSchoolUrl"];
                }

                else
                {
                    //CARMEL TERESA SCHOOL
                    objInputPayment.Mid = ConfigurationManager.AppSettings["SchoolPaymentWID"];
                    objInputPayment.Enckey = ConfigurationManager.AppSettings["SchoolPaymentkey"];
                    objInputPayment.ResponseUrl = ConfigurationManager.AppSettings["SchoolUrl"];
                }


                



                objInputPayment.MeTransReqType = "S";
                objInputPayment.TrnAmt = payAmount.ToString();
                objInputPayment.TrnRemarks = "Student Registration Fees";
                objInputPayment.TrnCurrency = "INR";

                objInputPayment.AddField1 = ObjPaymentResultModels.StudentName;// form["StudentName"];
                objInputPayment.AddField2 = ObjPaymentResultModels.Class;// form["Class"];
                objInputPayment.AddField3 = ObjPaymentResultModels.Category;// form["Category"];
                objInputPayment.AddField4 = Convert.ToString(ObjPaymentResultModels.ConcessionAmt);// form["RoleNumber"];
                objInputPayment.AddField5 = ObjPaymentResultModels.TCBal;// form["TCBal"];
                objInputPayment.AddField6 = ObjPaymentResultModels.studentid;//form["studentid"];
                objInputPayment.AddField7 = ObjPaymentResultModels.FeeHeadings;
                objInputPayment.AddField8 = ObjPaymentResultModels.Feeheadingamt;
                objInputPayment.AddField9 = ObjPaymentResultModels.ApplicationNumber;
                        

                var Result = InitiatePayment(objInputPayment);

                tbl_PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new tbl_PaymentTransactionDetails()
                {
                    TxnDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Amount = ObjPaymentResultModels.TCBal.ToString(),
                    TransactionId = trackId.ToString(),
                    Pmntmode = "Online",
                    StudentId = Convert.ToInt32(ObjPaymentResultModels.studentid)
                };
                objrep_tbl_PaymentTransactionDetails.Insert(objinsert_tbl_PaymentTransactionDetails);
                objrep_tbl_PaymentTransactionDetails.Save();
                int studId = Convert.ToInt32(ObjPaymentResultModels.studentid);
                var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == studId && x.TransactionId == trackId.ToString());

                if (ObjPaymentResultModels.Feeheadingamt != null && ObjPaymentResultModels.Feeheadingamt != "" && tblpaymentDetails != null)
                {
                    string[] FeeHeadingSplit = ObjPaymentResultModels.Feeheadingamt.Split(',');
                    if (FeeHeadingSplit.Length > 0)
                    {
                        for (int fi = 0; fi < FeeHeadingSplit.Length; fi++)
                        {
                            if (FeeHeadingSplit[fi] != null && FeeHeadingSplit[fi] != "")
                            {
                                string[] FeeheadingSecond = FeeHeadingSplit[fi].Split('~');
                                tbl_PaymentTransactionFeeDetails objinsert_tbl_PaymentTransactionFeeDetails = new tbl_PaymentTransactionFeeDetails()
                                {
                                    PaymentTransactionId = tblpaymentDetails.PaymentTransactionId,
                                    FeeAmount = FeeheadingSecond[1].ToString(),
                                    FeeID = Convert.ToInt32(FeeheadingSecond[0]),
                                    CreatedOn = DateTime.Now
                                };
                                objrep_tbl_PaymentTransactionFeeDetails.Insert(objinsert_tbl_PaymentTransactionFeeDetails);
                                objrep_tbl_PaymentTransactionFeeDetails.Save();
                            }
                        }
                    }
                }
            }
            return View(ObjPaymentResultModels);
           
        }


        //For Pre Primary School
        public ActionResult PaymentProcessResult()
        {
            InputPaymentResult objPaymentStatus = new InputPaymentResult();
            string merchantResponse = Request.Form["merchantResponse"];
            if (Request.Form["merchantResponse"] != null)
            {
                string key = ConfigurationManager.AppSettings["PreSchoolPaymentkey"];
                AWLMEAPI transact = new AWLMEAPI();
                ResMsgDTO objResMsgDTO = transact.parseTrnResMsg(merchantResponse, key);

                decimal Amount = 0;
                Amount = Convert.ToDecimal(objResMsgDTO.TrnAmt) / 100;


                objPaymentStatus.AddField1 = objResMsgDTO.AddField1;
                objPaymentStatus.AddField2 = objResMsgDTO.AddField2;
                objPaymentStatus.AddField3 = objResMsgDTO.AddField3;
                objPaymentStatus.AddField4 = objResMsgDTO.AddField4;
                objPaymentStatus.AddField5 = objResMsgDTO.AddField5;
                objPaymentStatus.AddField6 = objResMsgDTO.AddField6;
                objPaymentStatus.AddField7 = objResMsgDTO.AddField7;
                objPaymentStatus.AddField8 = objResMsgDTO.AddField8;
                objPaymentStatus.AddField9 = objResMsgDTO.AddField9;
                objPaymentStatus.AuthZCode = objResMsgDTO.AuthZCode;
                objPaymentStatus.PgMeTrnRefNo = objResMsgDTO.PgMeTrnRefNo;

                objPaymentStatus.TrnAmt = Amount.ToString();


                objPaymentStatus.TrnReqDate = objResMsgDTO.TrnReqDate;
                objPaymentStatus.Rrn = objResMsgDTO.Rrn;
                objPaymentStatus.OrderId = objResMsgDTO.OrderId;

                string Status = string.Empty;
                if (objResMsgDTO.StatusCode == "S")
                {
                    Status = "Success";
                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                    var discountamount = Convert.ToString(objPaymentStatus.AddField4);

                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();
                    var feeid = genericFeeCalculator(objPaymentStatus, stuid);


                    ////  newly added 18.12.2022
                    if (feeid != "")
                    {
                        var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == stuid);
                        if (studentdata != null)
                        {
                            if (studentdata.Parents_Email != null)
                            {
                                var studentname = studentdata.Name + " " + studentdata.Last_Name;
                                //need to change the link when publish into server
                                //var link = "http://svd.orootsfoundations.org/Payment/AdmissionFeereceipt?id=" + feeid + "";
                                var link = "https://www.carmelteresaschool.in/Payment/AdmissionFeereceipt?id=" + feeid + "";

                                //For Receipt
                                var str = SendEmail("" + studentdata.Parents_Email + "", "Admission Fee Payment Receipt", "" + studentname + " You can Download your Admission Fee Receipt through this Link" + link + "");
                            }
                        }
                    }
                    ///

                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Completed Successfully!, Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ", Transaction Amount:" + objPaymentStatus.TrnAmt + ", Transaction Request Date:" + objPaymentStatus.TrnReqDate + ", Transaction Status:" + objPaymentStatus.StatusCode + ", Transaction Description" + objPaymentStatus.StatusDesc + ", Student Name:" + objPaymentStatus.AddField1 + ", Class:" + objPaymentStatus.AddField2 + "')</script>");

                }
                else
                {
                    Status = "Failed";
                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                    var discountamount = Convert.ToString(objPaymentStatus.AddField4);

                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Failed,Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ",Transaction Amount:" + objPaymentStatus.TrnAmt + ",Transaction Request Date:" + objPaymentStatus.TrnReqDate + ",Transaction Status:" + objPaymentStatus.StatusCode + ",Transaction Description:" + objPaymentStatus.StatusDesc + ",Student Name:" + objPaymentStatus.AddField1 + ",Class:" + objPaymentStatus.AddField2 + "')</script>");
                }


            }
            else
            {
             
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
            }
        }

        //for layout 



        //For School 
        public ActionResult PaymentProcessSchoolResult()
        {
            try
            {
                logger.Debug("Inside Payment Process School Result method");
                InputPaymentResult objPaymentStatus = new InputPaymentResult();
                string merchantResponse = Request.Form["merchantResponse"];
                if (Request.Form["merchantResponse"] != null)
                {
                    
                    string key = ConfigurationManager.AppSettings["SchoolPaymentkey"];


                    AWLMEAPI transact = new AWLMEAPI();
                    ResMsgDTO objResMsgDTO = transact.parseTrnResMsg(merchantResponse, key);

                    decimal Amount = 0;
                    Amount = Convert.ToDecimal(objResMsgDTO.TrnAmt) / 100;

                    objPaymentStatus.AddField1 = objResMsgDTO.AddField1;
                    objPaymentStatus.AddField2 = objResMsgDTO.AddField2;
                    objPaymentStatus.AddField3 = objResMsgDTO.AddField3;
                    objPaymentStatus.AddField4 = objResMsgDTO.AddField4;
                    objPaymentStatus.AddField5 = objResMsgDTO.AddField5;
                    objPaymentStatus.AddField6 = objResMsgDTO.AddField6;
                    objPaymentStatus.AddField7 = objResMsgDTO.AddField7;
                    objPaymentStatus.AddField8 = objResMsgDTO.AddField8;
                    objPaymentStatus.AddField10 = objResMsgDTO.AddField10;
                    
                    objPaymentStatus.AuthZCode = objResMsgDTO.AuthZCode;
                    objPaymentStatus.PgMeTrnRefNo = objResMsgDTO.PgMeTrnRefNo;

                  
                    objPaymentStatus.TrnAmt = Amount.ToString();

                    objPaymentStatus.TrnReqDate = objResMsgDTO.TrnReqDate;
                    objPaymentStatus.Rrn = objResMsgDTO.Rrn;
                    objPaymentStatus.OrderId = objResMsgDTO.OrderId;
                    
                    string Status = string.Empty;
                    if (objResMsgDTO.StatusCode == "S")
                    {
                        Status = "Success";
                        objPaymentStatus.StatusCode = Status.ToString();
                        objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                        objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                        int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                        var discountamount = Convert.ToString(objPaymentStatus.AddField4);
                        PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                        {
                            TransactionStatus = Status,
                            TransactionError = objResMsgDTO.StatusDesc,
                            ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                            TrackId = objResMsgDTO.Rrn,
                            PaymentId = objResMsgDTO.ResponseCode
                        };
                        var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                        _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                        _context.SaveChanges();
                        genericFeeCalculator(objPaymentStatus, stuid);
                        return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Completed Successfully!, Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ", Transaction Amount:" + objPaymentStatus.TrnAmt + ", Transaction Request Date:" + objPaymentStatus.TrnReqDate + ", Transaction Status:" + objPaymentStatus.StatusCode + ", Transaction Description" + objPaymentStatus.StatusDesc + ", Student Name:" + objPaymentStatus.AddField1 + ", Class:" + objPaymentStatus.AddField2 + "')</script>");
                    }
                    else
                    {
                        Status = "Failed";
                        objPaymentStatus.StatusCode = Status.ToString();
                        objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                        objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                        int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                        var discountamount = Convert.ToString(objPaymentStatus.AddField4);
                        PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                        {
                            TransactionStatus = Status,
                            TransactionError = objResMsgDTO.StatusDesc,
                            ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                            TrackId = objResMsgDTO.Rrn,
                            PaymentId = objResMsgDTO.ResponseCode
                        };
                        var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                        _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                        _context.SaveChanges();
                        return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Failed,Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ",Transaction Amount:" + objPaymentStatus.TrnAmt + ",Transaction Request Date:" + objPaymentStatus.TrnReqDate + ",Transaction Status:" + objPaymentStatus.StatusCode + ",Transaction Description:" + objPaymentStatus.StatusDesc + ",Student Name:" + objPaymentStatus.AddField1 + ",Class:" + objPaymentStatus.AddField2 + "')</script>");
                    }


                }
                else
                {
                    
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
                }
                
            }
            catch (Exception e)
            {
                logger.Error("An exception occurred inside PaymentProcessSchoolResult() method. The exception is :" + e.ToString());

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
            }
        }

        public string genericFeeCalculator(InputPaymentResult objPaymentStatus, int stuid)
        {
            try
            {
                logger.Debug("ObjPaymentStatus in genericFeeCalculator - " + Newtonsoft.Json.JsonConvert.SerializeObject(objPaymentStatus) + " stuid - " + stuid);
               
                var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == stuid);
                int calssid = Convert.ToInt32(studentdetails.Class_Id);
                int Categoryid = Convert.ToInt32(studentdetails.Category_Id);
                var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId == calssid);
                var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId == Categoryid);
                List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();
                var unicNumber = Guid.NewGuid();


                

                var Batchdetails = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true).Select(x => x.Batch_Id).ToArray();
                var feeplans = _context.FeePlans.Where(x => x.ClassId == calssid && Batchdetails.Contains(x.Batch_Id)).ToList();
               
                string feeHeadingNames = string.Empty;
                string feeHeadingAmts = string.Empty;
                string feeHeadingIDs = string.Empty;
                string batchname = string.Empty;
                string feetypeid = string.Empty;
                logger.Debug("Checking for AddField8");
                if (objPaymentStatus.AddField8 != null && objPaymentStatus.AddField8 != "")
                {
                    string[] FeeHeadingSplit = objPaymentStatus.AddField8.Split(',');
                    if (FeeHeadingSplit != null)
                    {
                        var fh = FeeHeadingSplit.Take(FeeHeadingSplit.Length - 1).ToList();

                        if (fh.Count > 0)
                        {
                            for (int i = 0; i < fh.Count; i++)
                            {
                                if (fh[i] != null && fh[i] != "")
                                {
                                    string[] FeeheadingSecond = fh[i].Split('~');
                                    int feeid = Convert.ToInt32(FeeheadingSecond[0]);
                                    float feeamont = (float)Convert.ToDecimal(FeeheadingSecond[1]);
                                    FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeId == feeid);
                                    batchname = feeplans.FirstOrDefault(x => x.FeeId == feeHeading.FeeId)?.Batch_Id.ToString();
                                    
                                    feetypeid = feeplans.FirstOrDefault(x => x.FeeId == feeHeading.FeeId)?.FeeType_Id.ToString();
                                    if (feeHeadingNames != string.Empty)
                                        feeHeadingNames = string.Join(",", feeHeadingNames, feeHeading.FeeName);
                                    else
                                        feeHeadingNames = feeHeading.FeeName;
                                    if (feeHeadingAmts != string.Empty)
                                        feeHeadingAmts = string.Join(",", feeHeadingAmts, Convert.ToString(feeamont));
                                    else
                                        feeHeadingAmts = Convert.ToString(feeamont);
                                    if (feeHeadingIDs != string.Empty)
                                        feeHeadingIDs = string.Join(",", feeHeadingIDs, Convert.ToString(FeeheadingSecond[0]));
                                    else
                                        feeHeadingIDs = Convert.ToString(FeeheadingSecond[0]);
                                }

                            }
                        }
                    }
                }

                #region oldcode

                TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
                tblFeeReceipts.FeeHeadingIDs = feeHeadingIDs;
                string feeMonths = "Jun";

                if (!string.IsNullOrWhiteSpace(batchname))
                {
                    int tempBatchId = 0;
                    int.TryParse(batchname, out tempBatchId);
                    if (tempBatchId != 0)
                    {
                        batchname = _context.Tbl_Batches.Where(x => x.Batch_Id == tempBatchId).Select(x => x.Batch_Name).FirstOrDefault();
                    }
                }

                tblFeeReceipts.BalanceAmt = 0;
                tblFeeReceipts.OldBalance = 0;
                //tblFeeReceipts.BankName = "";
                tblFeeReceipts.CategoryId = Convert.ToInt32(categoryDetail?.DataListItemId);
                tblFeeReceipts.CategoryName = categoryDetail?.DataListItemName;
                tblFeeReceipts.ClassId = Convert.ToInt32(studentdetails.Class_Id);
                tblFeeReceipts.ClassName = classDetail.DataListItemName;
                tblFeeReceipts.Type = feetypeid;
                tblFeeReceipts.BatchName = batchname;
                //tblFeeReceipts.FeePaidDate = feeReceiptViewModel.DateTimeVal != null ? feeReceiptViewModel.DateTimeVal : DateTime.Now;
                tblFeeReceipts.AddedDate = DateTime.Now;
                tblFeeReceipts.Concession = 0;
                tblFeeReceipts.ConcessionAmt = float.Parse(objPaymentStatus.AddField4);
                tblFeeReceipts.LateFee = 0;
                tblFeeReceipts.PaidMonths = "Jun";
                tblFeeReceipts.PayHeadings = feeHeadingNames;
                tblFeeReceipts.PaymentMode = "Online";
                tblFeeReceipts.ReceiptAmt = float.Parse(objPaymentStatus.TrnAmt);
                // tblFeeReceipts.Remark = "Online Payment";
                tblFeeReceipts.StudentId = stuid;
                tblFeeReceipts.StudentName = objPaymentStatus.AddField1;
                tblFeeReceipts.TotalFee = float.Parse(objPaymentStatus.TrnAmt);
                tblFeeReceipts.FeePaids = feeHeadingAmts;
                tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
                tblFeeReceipts.DueAmount = objPaymentStatus.TrnAmt;
                tblFeeReceipts.PaidAmount = objPaymentStatus.TrnAmt;



                objPaymentStatus.AddField2 = classDetail.DataListItemName;
                objPaymentStatus.AddField3 = categoryDetail?.DataListItemName;


                if (feeMonths.Contains("Jan"))
                {
                    tblFeeReceipts.Jan = true;
                }
                if (feeMonths.Contains("Feb"))
                {
                    tblFeeReceipts.Feb = true;
                }
                if (feeMonths.Contains("Mar"))
                {
                    tblFeeReceipts.Mar = true;
                }
                if (feeMonths.Contains("Apr"))
                {
                    tblFeeReceipts.Apr = true;
                }
                if (feeMonths.Contains("May"))
                {
                    tblFeeReceipts.May = true;
                }
                if (feeMonths.Contains("Jun"))
                {
                    tblFeeReceipts.Jun = true;
                }
                if (feeMonths.Contains("Jul"))
                {
                    tblFeeReceipts.Jul = true;
                }
                if (feeMonths.Contains("Aug"))
                {
                    tblFeeReceipts.Aug = true;
                }
                if (feeMonths.Contains("Sep"))
                {
                    tblFeeReceipts.Sep = true;
                }
                if (feeMonths.Contains("Oct"))
                {
                    tblFeeReceipts.Oct = true;
                }
                if (feeMonths.Contains("Nov"))
                {
                    tblFeeReceipts.Nov = true;
                }
                if (feeMonths.Contains("Dec"))
                {
                    tblFeeReceipts.Dec = true;
                }
                _TblFeeReceiptsRepository.Insert(tblFeeReceipts);
                _TblFeeReceiptsRepository.Save();

                ////

                var feereceiptsid = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == stuid && x.FeeReceiptsOneTimeCreator == unicNumber.ToString());
                var receiptid = "";
                if (feereceiptsid != null)
                {
                    receiptid = feereceiptsid.FeeReceiptId.ToString();
                }
                return receiptid;

                #endregion

                #region newcode
                //if (objPaymentStatus.AddField8 != null && objPaymentStatus.AddField8 != "" && tblpaymentDetails != null)
                //{
                //    string[] FeeHeadingSplit = objPaymentStatus.AddField8.Split(',');
                //    if (FeeHeadingSplit.Length > 0)
                //    {
                //        for (int fi = 0; fi < FeeHeadingSplit.Length; fi++)
                //        {
                //            if (FeeHeadingSplit[fi] != null && FeeHeadingSplit[fi] != "")
                //            {
                //                string[] FeeheadingSecond = FeeHeadingSplit[fi].Split('~');
                //                string feeMonths = "Jun";
                //                int feeid = Convert.ToInt32(FeeheadingSecond[0]);
                //                float feeamont = (float)Convert.ToDecimal(FeeheadingSecond[1]);
                //                FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeId == feeid);
                //                TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
                //                tblFeeReceipts.FeeHeadingId = Convert.ToInt32(FeeheadingSecond[0]);
                //                tblFeeReceipts.BalanceAmt = 0;
                //                tblFeeReceipts.OldBalance = 0;
                //                //tblFeeReceipts.BankName = "";
                //                tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
                //                tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
                //                tblFeeReceipts.ClassId = classDetail.DataListItemId;
                //                tblFeeReceipts.ClassName = classDetail.DataListItemName;
                //                tblFeeReceipts.BatchName = "2021-2035";
                //                //tblFeeReceipts.FeePaidDate = feeReceiptViewModel.DateTimeVal != null ? feeReceiptViewModel.DateTimeVal : DateTime.Now;
                //                tblFeeReceipts.AddedDate = DateTime.Now.ToString("dd/MM/yyyy");
                //                tblFeeReceipts.Concession = 0;
                //                tblFeeReceipts.ConcessionAmt = 0;
                //                tblFeeReceipts.LateFee = 0;
                //                tblFeeReceipts.PaidMonths = "Jun";
                //                tblFeeReceipts.PayHeadings = feeHeading.FeeName;
                //                tblFeeReceipts.PaymentMode = "Online";
                //                tblFeeReceipts.ReceiptAmt = feeamont;
                //                // tblFeeReceipts.Remark = "Online Payment";
                //                tblFeeReceipts.StudentId = stuid;
                //                tblFeeReceipts.StudentName = objPaymentStatus.AddField1;
                //                tblFeeReceipts.TotalFee = feeamont;
                //                tblFeeReceipts.FeePaids = feeamont.ToString();
                //                tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
                //                tblFeeReceipts.DueAmount = feeamont.ToString();
                //                tblFeeReceipts.PaidAmount = feeamont.ToString();
                //                if (feeMonths.Contains("Jan"))
                //                {
                //                    tblFeeReceipts.Jan = true;
                //                }
                //                if (feeMonths.Contains("Feb"))
                //                {
                //                    tblFeeReceipts.Feb = true;
                //                }
                //                if (feeMonths.Contains("Mar"))
                //                {
                //                    tblFeeReceipts.Mar = true;
                //                }
                //                if (feeMonths.Contains("Apr"))
                //                {
                //                    tblFeeReceipts.Apr = true;
                //                }
                //                if (feeMonths.Contains("May"))
                //                {
                //                    tblFeeReceipts.May = true;
                //                }
                //                if (feeMonths.Contains("Jun"))
                //                {
                //                    tblFeeReceipts.Jun = true;
                //                }
                //                if (feeMonths.Contains("Jul"))
                //                {
                //                    tblFeeReceipts.Jul = true;
                //                }
                //                if (feeMonths.Contains("Aug"))
                //                {
                //                    tblFeeReceipts.Aug = true;
                //                }
                //                if (feeMonths.Contains("Sep"))
                //                {
                //                    tblFeeReceipts.Sep = true;
                //                }
                //                if (feeMonths.Contains("Oct"))
                //                {
                //                    tblFeeReceipts.Oct = true;
                //                }
                //                if (feeMonths.Contains("Nov"))
                //                {
                //                    tblFeeReceipts.Nov = true;
                //                }
                //                if (feeMonths.Contains("Dec"))
                //                {
                //                    tblFeeReceipts.Dec = true;
                //                }
                //                _TblFeeReceiptsRepository.Insert(tblFeeReceipts);
                //                _TblFeeReceiptsRepository.Save();
                //            }
                //        }
                //    }
                //}
                #endregion
            }
            catch (Exception e)
            {
                logger.Error("Error in genericFeeCalculatorMethod - " + e.ToString());
                return "";
            }
        }


        //For Transport
        public ActionResult PaymentProcessTransportResult()
        {

            InputPaymentResult objPaymentStatus = new InputPaymentResult();
            string merchantResponse = Request.Form["merchantResponse"];
            if (Request.Form["merchantResponse"] != null)
            {
                //string merchantResponse = Request.Form["merchantResponse"];
                //string key = ConfigurationManager.AppSettings["SchoolPaymentkey"];
                string key = ConfigurationManager.AppSettings["SchoolTransportkey"];


                AWLMEAPI transact = new AWLMEAPI();
                ResMsgDTO objResMsgDTO = transact.parseTrnResMsg(merchantResponse, key);

                decimal Amount = 0;
                Amount = Convert.ToDecimal(objResMsgDTO.TrnAmt) / 100;

                objPaymentStatus.AddField1 = objResMsgDTO.AddField1;
                objPaymentStatus.AddField2 = objResMsgDTO.AddField2;
                objPaymentStatus.AddField3 = objResMsgDTO.AddField3;
                objPaymentStatus.AddField4 = objResMsgDTO.AddField4;
                objPaymentStatus.AddField5 = objResMsgDTO.AddField5;
                objPaymentStatus.AddField6 = objResMsgDTO.AddField6;
                objPaymentStatus.AddField7 = objResMsgDTO.AddField7;
                objPaymentStatus.AddField8 = objResMsgDTO.AddField8;
                //objPaymentStatus.AddField9 = objResMsgDTO.AddField9;
                objPaymentStatus.AuthZCode = objResMsgDTO.AuthZCode;
                objPaymentStatus.PgMeTrnRefNo = objResMsgDTO.PgMeTrnRefNo;

                objPaymentStatus.TrnAmt = Amount.ToString();
                //objPaymentStatus.TrnAmt = objResMsgDTO.TrnAmt;

                objPaymentStatus.TrnReqDate = objResMsgDTO.TrnReqDate;
                objPaymentStatus.Rrn = objResMsgDTO.Rrn;
                objPaymentStatus.OrderId = objResMsgDTO.OrderId;

                string Status = string.Empty;
                if (objResMsgDTO.StatusCode == "S")
                {
                    Status = "Success";
                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();
                    genericFeeCalculator(objPaymentStatus, stuid);
                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Completed Successfully!, Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ", Transaction Amount:" + objPaymentStatus.TrnAmt + ", Transaction Request Date:" + objPaymentStatus.TrnReqDate + ", Transaction Status:" + objPaymentStatus.StatusCode + ", Transaction Description" + objPaymentStatus.StatusDesc + ", Student Name:" + objPaymentStatus.AddField1 + ", Class:" + objPaymentStatus.AddField2 + "')</script>");
                }

                else
                {
                    Status = "Failed";
                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                    var discountamount = Convert.ToString(objPaymentStatus.AddField4);

                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Failed,Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ",Transaction Amount:" + objPaymentStatus.TrnAmt + ",Transaction Request Date:" + objPaymentStatus.TrnReqDate + ",Transaction Status:" + objPaymentStatus.StatusCode + ",Transaction Description:" + objPaymentStatus.StatusDesc + ",Student Name:" + objPaymentStatus.AddField1 + ",Class:" + objPaymentStatus.AddField2 + "')</script>");
                }

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
            }
        }


        public string InitiatePayment(InputPayment objInputPayment)
        {
            try
            {
                ReqMsgDTO objReqMsgDTO = new ReqMsgDTO();

                objReqMsgDTO.OrderId = objInputPayment.OrderId;
                objReqMsgDTO.Mid = objInputPayment.Mid;
                objReqMsgDTO.Enckey = objInputPayment.Enckey;
                objReqMsgDTO.MeTransReqType = objInputPayment.MeTransReqType;
                objReqMsgDTO.TrnAmt = objInputPayment.TrnAmt;
                objReqMsgDTO.ResponseUrl = objInputPayment.ResponseUrl;
                objReqMsgDTO.TrnRemarks = objInputPayment.TrnRemarks;
                objReqMsgDTO.TrnCurrency = objInputPayment.TrnCurrency;
                //Optional Fields
                objReqMsgDTO.AddField1 = objInputPayment.AddField1;
                objReqMsgDTO.AddField2 = objInputPayment.AddField2;
                objReqMsgDTO.AddField3 = objInputPayment.AddField3;
                objReqMsgDTO.AddField4 = objInputPayment.AddField4;
                objReqMsgDTO.AddField5 = objInputPayment.AddField5;
                objReqMsgDTO.AddField6 = objInputPayment.AddField6;
                objReqMsgDTO.AddField7 = objInputPayment.AddField7;
                objReqMsgDTO.AddField8 = objInputPayment.AddField8;
                objReqMsgDTO.AddField9 = objInputPayment.AddField9;
                //objReqMsgDTO.AddField10 = objInputPayment.AddField10;

                ReqMsgDTO ObjResultPayment = new ReqMsgDTO();
                AWLMEAPI objawlmerchantkit = new AWLMEAPI();
                ObjResultPayment = objawlmerchantkit.generateTrnReqMsg(objReqMsgDTO);
                string Message;
                if (ObjResultPayment.StatusDesc == "Success")
                {
                    Message = ObjResultPayment.ReqMsg;
                    ViewBag.PaymentRequestMessage = Message;
                    ViewBag.Mid = objInputPayment.Mid;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in Initiate Payment method. " + ex.ToString());
            }
            return "Payment Success";
        }


        public string StatusPayment(PaymentStatus objStatusPayment)
        {
            try
            {
                AWLMEAPI objawlmerchantkit = new AWLMEAPI();
                ResMsgDTO objResMsgDTO = new ResMsgDTO();
                //Step 3: Call the status API
                objResMsgDTO = objawlmerchantkit.getTransactionStatus(objStatusPayment.Mid, objStatusPayment.OrderId,
                 objStatusPayment.pgTrnRefNo, objStatusPayment.Enckey);
                //Step 4: Retrieve Status:
                if (objResMsgDTO.StatusCode == "S")
                {
                    //Success
                }
                else
                {
                    //Failed 
                }

            }
            catch (Exception ex)
            {

            }
            return "Payment Success";
        }


        public string CancelPayment(CancelPayment objCancelPayment)
        {
            try
            {
                AWLMEAPI objawlmerchantkit = new AWLMEAPI();
                ReqMsgDTO objReqMsgDTO = new ReqMsgDTO();
                objReqMsgDTO.OrderId = objCancelPayment.OrderId;
                objReqMsgDTO.Mid = objCancelPayment.Mid;
                objReqMsgDTO.Enckey = objCancelPayment.Enckey;
                objReqMsgDTO.PgMeTrnRefNo = objCancelPayment.pgTrnRefNo;
                //Step 3: API call to get the Cancel API
                ResMsgDTO objResMsgDTO = objawlmerchantkit.cancelTransaction(objReqMsgDTO);
                //Step 4: Retrieve Status:
                if (objResMsgDTO.StatusCode == "S")
                {
                    //Success
                }
                else
                {
                    //Failed 
                }

            }
            catch (Exception ex)
            {

            }
            return "Payment Success";
        }


        public string RefundPayment(RefundPayment objRefundPayment)
        {
            try
            {
                AWLMEAPI objawlmerchantkit = new AWLMEAPI();
                ReqMsgDTO objReqMsgDTO = new ReqMsgDTO();
                objReqMsgDTO.OrderId = objRefundPayment.OrderId;
                objReqMsgDTO.Mid = objRefundPayment.Mid;
                objReqMsgDTO.RefundAmt = objRefundPayment.Amount; //Paisa Format
                objReqMsgDTO.PgMeTrnRefNo = objRefundPayment.pgTrnRefNo;
                objReqMsgDTO.Enckey = objRefundPayment.Enckey;
                //Step 3: Construct the request DTO with respective Parameter
                ResMsgDTO objResMsgDTO = objawlmerchantkit.refundTransaction(objReqMsgDTO);
                //Step 4: Retrieve Status:
                if (objResMsgDTO.StatusCode == "S")
                {
                    //Success
                }
                else
                {
                    //Failed 
                }

            }
            catch (Exception ex)
            {

            }
            return "Payment Success";
        }

        public async Task<JsonResult> GetStudentTCDetailsById(int studentId, bool isAdmissionStudent = false)
        {
            StudentTcDetailViewModel studentModel = new StudentTcDetailViewModel();
            var data = 0;
            var amount = new decimal();
            Student studentDetail = _context.Students.FirstOrDefault(x => x.StudentId == studentId);
            FamilyDetail familyDetail = await _context.FamilyDetails.FirstOrDefaultAsync(x => x.ApplicationNumber == studentDetail.ApplicationNumber);
            var tcDetails = new StudentTcDetails();
            if (isAdmissionStudent)
            {
                if (!string.IsNullOrEmpty(studentDetail.Class))
                {
                    studentDetail.Class = studentDetail.Class.Trim();
                }
                if (studentDetail.Class == "XI - I - MAT/PHY/CHE/BIO " || studentDetail.Class == "XI - II - MAT/PHY/CHE/BOT/ZOO " ||
                    studentDetail.Class == "XI - III - MAT/PHY/C.SCI" || studentDetail.Class == "XII - II - BIOLOGY "
                    || studentDetail.Class == "XI - IV - ECO/COMM/ACC"
                    || studentDetail.Class == "XII - I - COM.SCI"
                    || studentDetail.Class == "XII - III - COM / BUS.MATHS "
                    || studentDetail.Class == "XII - IV - PURE SCIENCE"
                    || studentDetail.Class == "XII - V - COMM / COM.SCIENCE")
                {

                    data = await _context.DataListItems.Where(x => x.DataListItemName.Contains("Admission")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
                }
                else
                {
                    data = await _context.DataListItems.Where(x => x.DataListItemName.Contains("Junior Admission Fee")).Select(x => x.DataListItemId).FirstOrDefaultAsync();
                }

                amount = await _context.TcAmount.Where(x => x.Type == data).Select(x => x.Amount).FirstOrDefaultAsync();
            }
            else
            {
                tcDetails = await _context.Tbl_StudentTcDetails.Where(x => x.StudentId == studentId).FirstOrDefaultAsync();
                if (tcDetails != null)
                {
                    amount = await _context.TcAmount.Where(x => x.Type == tcDetails.TcId).Select(x => x.Amount).FirstOrDefaultAsync();
                }
            }

            studentModel.StudentName = studentDetail == null ? "" : studentDetail.Name;
            studentModel.FatherName = studentDetail == null ? "" : familyDetail?.FatherName;
            studentModel.Contact = studentDetail == null ? "" : familyDetail?.FMobile;
            studentModel.Class = studentDetail == null ? "" : studentDetail.Class;
            studentModel.Category = studentDetail == null ? "" : studentDetail.Category;
            studentModel.RoleNumber = studentDetail == null ? "" : Convert.ToString(studentDetail.StudentId);
            studentModel.TCBal = amount;
            studentModel.Batch = studentDetail == null ? string.Empty : studentDetail.BatchName;
            studentModel.studentTcid = tcDetails?.Id;
            return Json(studentModel, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> PaymentProcessResultSummary()
        {

            List<PaymentResultSummary> studentls = new List<PaymentResultSummary>();
            var result = await _context.tbl_PaymentTransactionDetails.OrderByDescending(x => x.PaymentTransactionId).ToListAsync();

            var Studentlist = _context.StudentsRegistrations.ToList();

            foreach (var res in result)
            {
                PaymentResultSummary studentobj = new PaymentResultSummary();

                StudentsRegistration objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentRegisterID == res.StudentId);
                var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
                studentobj.StudentId = res.StudentId.ToString();
                studentobj.StudentName = objTbl_DataListItem.Name;
                studentobj.Category = Category.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Category_Id)?.DataListItemName;
                studentobj.Course = Classlist.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Class_Id)?.DataListItemName;
                studentobj.PaymentStatus = res.TransactionStatus;
                studentobj.TransactionID = res.TransactionId;
                studentobj.PaymentMode = res.Pmntmode;
                studentobj.TransactionAmount = res.Amount;
                studentobj.TransactionDate = res.TxnDate;
                studentobj.ReferenceNumber = res.ReferenceNo;
                studentobj.Last_Name = objTbl_DataListItem.Last_Name;
                studentls.Add(studentobj);
            }
            ViewBag.StudentSummaryDetails = studentls;
            return View();
        }

        public JsonResult GetAllpaymentDetails(string DateFrom, string DateTo)
        {

            var allpaymentDetails = objrep_tbl_PaymentTransactionDetails.GetAll();
            var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            if (DateFrom != null && DateFrom != "" && DateTo != null && DateTo != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateFrom != null && DateFrom != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateTo != null && DateTo != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }

            var Studentlist = _context.StudentsRegistrations.ToList();
            List<PaymentResultSummary> studentls = new List<PaymentResultSummary>();
            foreach (var res in allpaymentDetails)
            {
                PaymentResultSummary studentobj = new PaymentResultSummary();

                //Student objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentId == res.StudentId);
                StudentsRegistration objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentRegisterID == res.StudentId);
                studentobj.StudentId = res.StudentId.ToString();
                studentobj.StudentName = objTbl_DataListItem.Name;
                studentobj.Last_Name = objTbl_DataListItem == null ? string.Empty : objTbl_DataListItem.Last_Name;
                studentobj.Category = Category.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Category_Id)?.DataListItemName;
                studentobj.Course = Classlist.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Class_Id)?.DataListItemName;
                studentobj.PaymentStatus = res.TransactionStatus == null ? string.Empty : res.TransactionStatus;
                studentobj.TransactionID = res.TransactionId == null ? string.Empty : res.TransactionId;
                studentobj.PaymentMode = res.Pmntmode;
                studentobj.TransactionAmount = res.Amount;
                studentobj.TransactionDate = res.TxnDate;
                studentobj.ReferenceNumber = res.ReferenceNo == null ? string.Empty : res.ReferenceNo;
                studentls.Add(studentobj);
            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }


        public async Task<ActionResult> PaymentProcessSuccessSummary()
        {
            List<PaymentResultSummary> studentls = new List<PaymentResultSummary>();
            var result = await _context.tbl_PaymentTransactionDetails.Where(x => x.TransactionStatus == "Success").OrderByDescending(y => y.PaymentTransactionId).ToListAsync();

            var Studentlist = _context.StudentsRegistrations.ToList();

            foreach (var res in result)
            {
                PaymentResultSummary studentobj = new PaymentResultSummary();

                StudentsRegistration objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentRegisterID == res.StudentId);
                var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
                studentobj.StudentId = res.StudentId.ToString();
                studentobj.StudentName = objTbl_DataListItem.Name;
                studentobj.Category = Category.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Category_Id)?.DataListItemName;
                studentobj.Course = Classlist.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Class_Id)?.DataListItemName;
                studentobj.PaymentStatus = res.TransactionStatus;
                studentobj.TransactionID = res.TransactionId;
                studentobj.PaymentMode = res.Pmntmode;
                studentobj.TransactionAmount = res.Amount;
                studentobj.TransactionDate = res.TxnDate;
                studentobj.ReferenceNumber = res.ReferenceNo;
                studentobj.Last_Name = objTbl_DataListItem.Last_Name;
                studentls.Add(studentobj);
            }
            ViewBag.StudentSummaryDetails = studentls;
            return View();
        }

        public JsonResult GetAllSuccesspaymentDetails(string DateFrom, string DateTo)
        {
            var allpaymentDetails = objrep_tbl_PaymentTransactionDetails.GetAll();
            //var allpaymentDetails = _context.tbl_PaymentTransactionDetails.Where(x => x.TransactionStatus == "Success").OrderByDescending(y => y.PaymentTransactionId).ToList();
            var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

            if (DateFrom != null && DateFrom != "" && DateTo != null && DateTo != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture) && DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateFrom != null && DateFrom != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) >= DateTime.ParseExact(DateFrom, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }
            else if (DateTo != null && DateTo != "")
            {
                allpaymentDetails = allpaymentDetails.Where(x => DateTime.ParseExact(x.TxnDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) <= DateTime.ParseExact(DateTo, "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }

            var Studentlist = _context.StudentsRegistrations.ToList();
            List<PaymentResultSummary> studentls = new List<PaymentResultSummary>();
            foreach (var res in allpaymentDetails)
            {
                if (res.TransactionStatus == "Success")
                {
                    PaymentResultSummary studentobj = new PaymentResultSummary();

                    //Student objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentId == res.StudentId);
                    StudentsRegistration objTbl_DataListItem = Studentlist.FirstOrDefault(x => x.StudentRegisterID == res.StudentId);
                    studentobj.StudentId = res.StudentId.ToString();
                    studentobj.StudentName = objTbl_DataListItem.Name;
                    studentobj.Last_Name = objTbl_DataListItem == null ? string.Empty : objTbl_DataListItem.Last_Name;
                    studentobj.Category = Category.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Category_Id)?.DataListItemName;
                    studentobj.Course = Classlist.FirstOrDefault(x => x.DataListItemId == objTbl_DataListItem.Class_Id)?.DataListItemName;
                    studentobj.PaymentStatus = res.TransactionStatus == null ? string.Empty : res.TransactionStatus;
                    studentobj.TransactionID = res.TransactionId;
                    studentobj.PaymentMode = res.Pmntmode;
                    studentobj.TransactionAmount = res.Amount;
                    studentobj.TransactionDate = res.TxnDate;
                    studentobj.ReferenceNumber = res.ReferenceNo;
                    studentls.Add(studentobj);
                }

            }
            return Json(studentls, JsonRequestBehavior.AllowGet);
        }

        //For Admission Fees only
        #region Admission Fees

        public ActionResult AdmissionFee()
        {
            return View();
        }



        public JsonResult GetAdmissionNoById(string appnumber)
        {
            var data = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == appnumber && x.IsApprove.ToString() == "191");
            var Classlist = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();
            data.Class = Classlist.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
            data.Category = Category.FirstOrDefault(x => x.DataListItemId == data.Category_Id)?.DataListItemName;

            var feedetails = _context.FeePlans.FirstOrDefault(x => x.ClassId == data.Class_Id && x.FeeName == "Admission Fee");


            return Json(new { data, feedetails }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdmissionFeePaymentProcess()
        {
            PaymentViewModels objPaymentViewModels = (PaymentViewModels)TempData["objPaymentViewModels"];
            var ClassList = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //  var transactionamount = "100";
            string orderId = "";
            PaymentResultModels ObjPaymentResultModels = new PaymentResultModels();
            if (objPaymentViewModels != null)
            {
                var studnentID = objPaymentViewModels.StudentId;
                var classdetails = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId;
                ViewBag.PaymentStudentName = objPaymentViewModels.Name;
                ObjPaymentResultModels.StudentName = objPaymentViewModels.Name;
                //ViewBag.PaymentStudentClass = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId;
                //ObjPaymentResultModels.Class = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId.ToString();
                ViewBag.PaymentStudentClass = objPaymentViewModels.Class;
                ObjPaymentResultModels.Class = objPaymentViewModels.Class;
                ViewBag.PaymentStudentCategory = objPaymentViewModels.Category;
                ObjPaymentResultModels.Category = objPaymentViewModels.Category;
                ViewBag.PaymentStudentRoleNumber = objPaymentViewModels.RoleNumber;
                ObjPaymentResultModels.RoleNumber = objPaymentViewModels.RoleNumber;
                //ViewBag.PaymentStudentBatch = objPaymentViewModels.Batch;
                //ObjPaymentResultModels.Batch = objPaymentViewModels.Batch;
                ViewBag.PaymentStudentTCBal = objPaymentViewModels.TCBal;
                ObjPaymentResultModels.TCBal = objPaymentViewModels.TCBal;
                ObjPaymentResultModels.FeeHeadings = objPaymentViewModels.FeeHeadings;
                ObjPaymentResultModels.Feeheadingamt = objPaymentViewModels.Feeheadingamt;
                ObjPaymentResultModels.studentid = studnentID;
                ObjPaymentResultModels.Concession = objPaymentViewModels.Concession;
                ObjPaymentResultModels.ConcessionAmt = objPaymentViewModels.ConcessionAmt;

                ObjPaymentResultModels.Amount = objPaymentViewModels.TCBal;
                ObjPaymentResultModels.Currency = "INR";
                ObjPaymentResultModels.Email = objPaymentViewModels.Email;
                int sid = Convert.ToInt32(studnentID);

                string PaymentAmount = ObjPaymentResultModels.TCBal;
                //string PaymentAmount = transactionamount;
                decimal payAmount = 0;
                payAmount = Convert.ToDecimal(PaymentAmount);
                payAmount = payAmount * 100;
                Guid g = Guid.NewGuid();

                //Random r = new Random();
                //int rInt = r.Next(0, 10000); //for ints
                //int range = 1000;
                //double rDouble = r.NextDouble() * range; //for doubles
                var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                InputPayment objInputPayment = new InputPayment();
                objInputPayment.OrderId = trackId.ToString();

                string key = "";
                string secret = "";

                TblMerchantclass tblMerchantclass = new TblMerchantclass();
                List<Tbl_CreateMerchantId> tbl_CreateMerchantIds = new List<Tbl_CreateMerchantId>();
                var merchantid = _context.Tbl_CreateMerchantId.ToList();
                var mercantdetails = _context.Tbl_MerchantName.ToList();
                //Active Merchant
                var merchantdata = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
                if (merchantdata != null)
                {
                    foreach (var item in merchantid)
                    {
                        if (item.Bank_Id == merchantdata.Bank_Id && item.Branch_Id == merchantdata.Branch_Id && item.School_Id == merchantdata.School_Id)
                        {
                            tbl_CreateMerchantIds.Add(new Tbl_CreateMerchantId
                            {
                                MerchantMID = item.MerchantMID,
                                MerchantKey = item.MerchantKey,
                                MerchantName_Id = item.MerchantName_Id
                            });
                        }
                    }
                }

                if (objPaymentViewModels.FeeHeadings == "Admission Fee" )
                {
                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Nursery")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }
                }



                if (classdetails != null && classdetails.ToString() == "207" || classdetails.ToString() == "208" || classdetails.ToString() == "209" || classdetails.ToString() == "210")
                {

                    //PRE PRIMARY SCHOOL
                    //objInputPayment.Mid = ConfigurationManager.AppSettings["PreSchoolPaymentWID"];
                    //objInputPayment.Enckey = ConfigurationManager.AppSettings["PreSchoolPaymentkey"];
                    //objInputPayment.ResponseUrl = ConfigurationManager.AppSettings["PreSchoolFeeUrl"];

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Nursery")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    ObjPaymentResultModels.Key = key;

                    //objInputPayment.Mid = "WL0000000023382";
                    //objInputPayment.Enckey = "6c79a03b0f5f41dc3ac8cdc50ff39c19";
                }
                //else if (objPaymentViewModels.FeeHeadings == "Transaction")
                //{
                //    //TRANSPORTATION
                //    objInputPayment.Mid = ConfigurationManager.AppSettings["SchoolTransportWID"];
                //    objInputPayment.Enckey = ConfigurationManager.AppSettings["SchoolTransportkey"];
                //}
                else
                {
                    //CARMEL TERESA SCHOOL
                    //objInputPayment.Mid = ConfigurationManager.AppSettings["SchoolPaymentWID"];
                    //objInputPayment.Enckey = ConfigurationManager.AppSettings["SchoolPaymentkey"];
                    //objInputPayment.ResponseUrl = ConfigurationManager.AppSettings["SchoolFeeUrl"];

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Primary")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    ObjPaymentResultModels.Key = key;
                }


                //objInputPayment.MeTransReqType = "S";
                //objInputPayment.TrnAmt = payAmount.ToString();
                //objInputPayment.TrnRemarks = "Admission Fee";
                //objInputPayment.TrnCurrency = "INR";

                objInputPayment.AddField1 = ObjPaymentResultModels.StudentName;// form["StudentName"];
                objInputPayment.AddField2 = ObjPaymentResultModels.Class;// form["Class"];
                objInputPayment.AddField3 = ObjPaymentResultModels.Category;// form["Category"];
                objInputPayment.AddField4 = ObjPaymentResultModels.RoleNumber;// form["RoleNumber"];
                objInputPayment.AddField5 = ObjPaymentResultModels.TCBal;// form["TCBal"];
                objInputPayment.AddField6 = studnentID;//form["studentid"];
                objInputPayment.AddField7 = ObjPaymentResultModels.FeeHeadings;
                objInputPayment.AddField8 = ObjPaymentResultModels.Feeheadingamt;
                objInputPayment.AddField9 = ObjPaymentResultModels.ApplicationNumber;

                //_context.tbl_PaymentTransactionDetails             

                var Result = InitiatePayment(objInputPayment);

                PaymentTransactionId paymentTransactionId = new PaymentTransactionId();
                paymentTransactionId.Merchant_Key = key;
                paymentTransactionId.Secret_Key = secret;

                TempData["MerchantDetails"] = paymentTransactionId;

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", payAmount); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", trackId);
                input.Add("payment_capture", "0");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();
                ViewBag.Orderid = orderId;
                ObjPaymentResultModels.OrdedrId = orderId;

                tbl_PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new tbl_PaymentTransactionDetails()
                {
                    TxnDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Amount = ObjPaymentResultModels.TCBal.ToString(),
                    TransactionId = trackId.ToString(),
                    Pmntmode = "Online",
                    StudentId = Convert.ToInt32(studnentID),
                    ApplicationNumber = ObjPaymentResultModels.ApplicationNumber
                };
                objrep_tbl_PaymentTransactionDetails.Insert(objinsert_tbl_PaymentTransactionDetails);
                objrep_tbl_PaymentTransactionDetails.Save();
                //int studId = Convert.ToInt32(ObjPaymentResultModels.studentid);
                var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.ApplicationNumber == objPaymentViewModels.ApplicationNumber && x.TransactionId == trackId.ToString());

                if (ObjPaymentResultModels.Feeheadingamt != null && ObjPaymentResultModels.Feeheadingamt != "" && tblpaymentDetails != null)
                {
                    string[] FeeHeadingSplit = ObjPaymentResultModels.Feeheadingamt.Split(',');
                    if (FeeHeadingSplit.Length > 0)
                    {
                        for (int fi = 0; fi < FeeHeadingSplit.Length; fi++)
                        {
                            if (FeeHeadingSplit[fi] != null && FeeHeadingSplit[fi] != "")
                            {
                                string[] FeeheadingSecond = FeeHeadingSplit[fi].Split('~');
                                tbl_PaymentTransactionFeeDetails objinsert_tbl_PaymentTransactionFeeDetails = new tbl_PaymentTransactionFeeDetails()
                                {
                                    PaymentTransactionId = tblpaymentDetails.PaymentTransactionId,
                                    FeeAmount = FeeheadingSecond[1].ToString(),
                                    FeeID = Convert.ToInt32(FeeheadingSecond[0]),
                                    CreatedOn = DateTime.Now,


                                };
                                objrep_tbl_PaymentTransactionFeeDetails.Insert(objinsert_tbl_PaymentTransactionFeeDetails);
                                objrep_tbl_PaymentTransactionFeeDetails.Save();
                                //break;
                                fi++;
                            }
                        }
                    }
                }
                // return View();
            }
            return View(ObjPaymentResultModels);
        }

        public ActionResult RegistrationFeePaymentProcess()
        {
            PaymentViewModels paymentViewModels = (PaymentViewModels)TempData["objPaymentViewModels"];
            var ClassList = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            string orderId = "";
            PaymentResultModels paymentResultModels = new PaymentResultModels();
            if (paymentViewModels != null)
            {
                var studentid = paymentViewModels.StudentId;
                var classdetails = ClassList.FirstOrDefault(x => x.DataListItemName == paymentResultModels.Class)?.DataListItemId;
                ViewBag.PaymentStudentName = paymentViewModels.Name;
                paymentResultModels.StudentName = paymentViewModels.Name;
                //ViewBag.PaymentStudentClass = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId;
                //ObjPaymentResultModels.Class = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId.ToString();
                ViewBag.PaymentStudentClass = paymentViewModels.Class;
                paymentResultModels.Class = paymentViewModels.Class;
                ViewBag.PaymentStudentCategory = paymentViewModels.Category;
                paymentResultModels.Category = paymentViewModels.Category;
                ViewBag.PaymentStudentRoleNumber = paymentViewModels.RoleNumber;
                paymentResultModels.RoleNumber = paymentViewModels.RoleNumber;
                //ViewBag.PaymentStudentBatch = objPaymentViewModels.Batch;
                //ObjPaymentResultModels.Batch = objPaymentViewModels.Batch;
                ViewBag.PaymentStudentTCBal = paymentViewModels.TCBal;
                paymentResultModels.TCBal = paymentViewModels.TCBal;
                paymentResultModels.FeeHeadings = paymentViewModels.FeeHeadings;
                paymentResultModels.Feeheadingamt = paymentViewModels.Feeheadingamt;
                paymentResultModels.studentid = studentid;
                paymentResultModels.Concession = paymentViewModels.Concession;
                paymentResultModels.ConcessionAmt = paymentViewModels.ConcessionAmt;
                paymentResultModels.Amount = paymentViewModels.TCBal;
                paymentResultModels.Currency = "INR";
                paymentResultModels.Email = paymentViewModels.Email;
                int sid = Convert.ToInt32(studentid);
                var App = _context.Students.FirstOrDefault(x => x.StudentId == sid).ApplicationNumber;
                var famildetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == App);

                paymentResultModels.Contact = famildetails == null ? "" : famildetails.FMobile;

                string PaymentAmount = paymentResultModels.TCBal;
                //string PaymentAmount = transactionamount;
                decimal payAmount = 0;
                payAmount = Convert.ToDecimal(PaymentAmount);
                payAmount = payAmount * 100;
                Guid g = Guid.NewGuid();

                var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                InputPayment objInputPayment = new InputPayment();
                objInputPayment.OrderId = trackId.ToString();

                string key = "";
                string secret = "";

                TblMerchantclass tblMerchantclass = new TblMerchantclass();
                List<Tbl_CreateMerchantId> tbl_CreateMerchantIds = new List<Tbl_CreateMerchantId>();
                var merchantid = _context.Tbl_CreateMerchantId.ToList();
                var mercantdetails = _context.Tbl_MerchantName.ToList();
                //Active Merchant
                var merchantdata = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
                if (merchantdata != null)
                {
                    foreach (var item in merchantid)
                    {
                        if (item.Bank_Id == merchantdata.Bank_Id && item.Branch_Id == merchantdata.Branch_Id && item.School_Id == merchantdata.School_Id)
                        {
                            tbl_CreateMerchantIds.Add(new Tbl_CreateMerchantId
                            {
                                MerchantMID = item.MerchantMID,
                                MerchantKey = item.MerchantKey,
                                MerchantName_Id = item.MerchantName_Id
                            });
                        }
                    }
                }

                if (paymentViewModels.FeeHeadings == "Transport Fee,Transport Fee(IIndTerm)" || paymentViewModels.FeeHeadings == "Transport Fee" || paymentViewModels.FeeHeadings == "Transport Fee(IIndTerm)" || paymentViewModels.FeeHeadings == "Transport Fee(IIndTerm),Transport Fee")
                {
                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Transport")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }
                }
                else if (classdetails != null && classdetails.ToString() == "207" || classdetails.ToString() == "208" || classdetails.ToString() == "209" || classdetails.ToString() == "210")
                {
                    //PRE PRIMARY SCHOOL

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Nursery")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    paymentResultModels.Key = key;
                }

                else
                {
                    //CARMEL TERESA SCHOOL

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Primary")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    paymentResultModels.Key = key;
                }
                PaymentTransactionId paymentTransactionId = new PaymentTransactionId();
                paymentTransactionId.Merchant_Key = key;
                paymentTransactionId.Secret_Key = secret;

                TempData["MerchantDetails"] = paymentTransactionId;

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", payAmount); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", trackId);
                input.Add("payment_capture", "0");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();
                ViewBag.Orderid = orderId;
                paymentResultModels.OrdedrId = orderId;
                tbl_PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new tbl_PaymentTransactionDetails()
                {
                    TxnDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Amount = PaymentAmount,
                    TransactionId = trackId.ToString(),
                    Pmntmode = "Online",
                    StudentId = Convert.ToInt32(studentid),
                    TrackId = orderId
                };
                objrep_tbl_PaymentTransactionDetails.Insert(objinsert_tbl_PaymentTransactionDetails);
                objrep_tbl_PaymentTransactionDetails.Save();

                TempData["StudentDetails"] = paymentResultModels;

            }

            return View(paymentResultModels);
        }

        public ActionResult PaymentProcessFeeResult()
        {
            InputPaymentResult objPaymentStatus = new InputPaymentResult();
            string merchantResponse = Request.Form["merchantResponse"];
            if (Request.Form["merchantResponse"] != null)
            {
                //string merchantResponse = Request.Form["merchantResponse"];
                //string key = ConfigurationManager.AppSettings["SchoolPaymentkey"];
                string key = ConfigurationManager.AppSettings["PreSchoolPaymentkey"];


                AWLMEAPI transact = new AWLMEAPI();
                ResMsgDTO objResMsgDTO = transact.parseTrnResMsg(merchantResponse, key);

                decimal Amount = 0;
                Amount = Convert.ToDecimal(objResMsgDTO.TrnAmt) / 100;


                objPaymentStatus.AddField1 = objResMsgDTO.AddField1;
                objPaymentStatus.AddField2 = objResMsgDTO.AddField2;
                //objPaymentStatus.AddField3 = objResMsgDTO.AddField3;
                objPaymentStatus.AddField4 = objResMsgDTO.AddField4;
                objPaymentStatus.AddField5 = objResMsgDTO.AddField5;
                objPaymentStatus.AddField6 = objResMsgDTO.AddField6;
                objPaymentStatus.AddField7 = objResMsgDTO.AddField7;
                objPaymentStatus.AddField8 = objResMsgDTO.AddField8;
                objPaymentStatus.AddField9 = objResMsgDTO.AddField9;
                objPaymentStatus.AuthZCode = objResMsgDTO.AuthZCode;
                objPaymentStatus.PgMeTrnRefNo = objResMsgDTO.PgMeTrnRefNo;

                objPaymentStatus.TrnAmt = Amount.ToString();

                //objPaymentStatus.TrnAmt = objResMsgDTO.TrnAmt.ToString();

                objPaymentStatus.TrnReqDate = objResMsgDTO.TrnReqDate;
                objPaymentStatus.Rrn = objResMsgDTO.Rrn;
                objPaymentStatus.OrderId = objResMsgDTO.OrderId;
                //var id = 75;
                //var link = "/AdmissionFee/PrintReceiptusingrotativa?id=1&ReceiptId=" +id+ "";
                string Status = string.Empty;
                if (objResMsgDTO.StatusCode == "S")
                {
                    Status = "Success";

                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);

                    var discountamount = Convert.ToString(objPaymentStatus.AddField4);

                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();
                    var feeid = genericadmissionfeecalculator(objPaymentStatus, stuid);


                    if (feeid != "")
                    {
                        var studentreereipt = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptId == Convert.ToInt32(feeid));

                        var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == stuid);
                        if (studentdata != null)
                        {
                            if (studentdata.Parents_Email != null)
                            {
                                var studentname = studentdata.Name + " " + studentdata.Last_Name;
                                //need to change the link when publish into server
                                //var link = "http://svd.orootsfoundations.org/Payment/AdmissionFeereceipt?id=" + feeid + "";
                                var websitename = _context.TblCreateSchool.Select(a => a.Website).FirstOrDefault();
                                if(websitename == null)
                                {
                                    throw new Exception("Please enter school website!.");
                                }
                                //var link = "https://www.carmelteresaschool.in/Payment/AdmissionFeereceipt?id=" + feeid + "";
                                var link = websitename + "/payment/AdmissionFeereceipt?id=" + feeid + "";

                                var str = SendEmail("" + studentdata.Parents_Email + "", "Admission Fee Payment Receipt", "" + studentname + " You can Download your Admission Fee Receipt through this Link " + link + "");

                                if (str == "S")
                                {
                                    EmailViewModel emailViewModel = new EmailViewModel();
                                    emailViewModel.Student_id = Convert.ToInt32(studentdata.StudentRegisterID);
                                    emailViewModel.ApplicationNumber = studentdata.ApplicationNumber;
                                    emailViewModel.Name = studentdata.Name + " " + studentdata.Last_Name;
                                    emailViewModel.Parent_Email = studentdata.Parents_Email;
                                    emailViewModel.Email_Date = DateTime.Now.ToString();
                                    emailViewModel.Email_Content = link;

                                    var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                                }

                                var createserlogin = new StudentAdmissionController().CreateUserlogin(stuid);

                                if (createserlogin != null)
                                {
                                    //var loginpagelink = "https://www.carmelteresaschool.in/";
                                    //var loginpagelink = "http://svd.orootsfoundations.org/";
                                    var loginpagelink = websitename;
                                    int userid = Convert.ToInt32(createserlogin);

                                    var userlogin = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);
                                    if (userlogin != null)
                                    {
                                        var useremail = SendEmail("" + studentdata.Parents_Email + "", "Regarding Username and Password", "You can login the website using this link" + loginpagelink + ",Username is " + userlogin.UserName + " and password is " + userlogin.Password + ",Your Application has been to the Admitted status successfully");

                                        if (useremail == "S")
                                        {
                                            EmailViewModel emailView = new EmailViewModel();
                                            emailView.Student_id = Convert.ToInt32(studentdata.StudentRegisterID);
                                            emailView.ApplicationNumber = studentdata.ApplicationNumber;
                                            emailView.Name = studentdata.Name + " " + studentdata.Last_Name;
                                            emailView.Parent_Email = studentdata.Parents_Email;
                                            emailView.Email_Date = DateTime.Now.ToString();
                                            emailView.Email_Content = "Regarding username and password and application has been moved to the admitted status";

                                            var emailarchievelink = new SMSandEmailController().AddEmailArchieve(emailView);


                                        }

                                    }
                                }

                            }
                        }
                    }

                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Completed Successfully!, Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ", Transaction Amount:" + objPaymentStatus.TrnAmt + ", Transaction Request Date:" + objPaymentStatus.TrnReqDate + ", Transaction Status:" + objPaymentStatus.StatusCode + ", Transaction Description" + objPaymentStatus.StatusDesc + ", Student Name:" + objPaymentStatus.AddField1 + ", Class:" + objPaymentStatus.AddField2 + "')</script>");
                }
                else
                {
                    Status = "Failed";

                    objPaymentStatus.StatusCode = Status.ToString();
                    objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                    objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                    int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                    var discountamount = Convert.ToString(objPaymentStatus.AddField4);
                    PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                    {
                        TransactionStatus = Status,
                        TransactionError = objResMsgDTO.StatusDesc,
                        ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                        TrackId = objResMsgDTO.Rrn,
                        PaymentId = objResMsgDTO.ResponseCode
                    };
                    var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                    _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                    _context.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Failed,Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ",Transaction Amount:" + objPaymentStatus.TrnAmt + ",Transaction Request Date:" + objPaymentStatus.TrnReqDate + ",Transaction Status:" + objPaymentStatus.StatusCode + ",Transaction Description:" + objPaymentStatus.StatusDesc + ",Student Name:" + objPaymentStatus.AddField1 + ",Class:" + objPaymentStatus.AddField2 + "')</script>");
                }

            }
            else
            {
                //ViewBag.ResultText = "No Data Can be Displayed......Session is Null";
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
            }
            //return View(objPaymentStatus);
        }

        //Send Email Funtuib
        public string SendEmail(string Tomailid, string subject, string bodymessage)
        {
            try
            {

                var schoolemail = _context.TblCreateSchool.FirstOrDefault();
                if (schoolemail == null)
                {
                    throw new Exception("Please enter school details!.");
                }
                var sourceMailID = new MailAddress(schoolemail.Email);
                string sourceMailPassword = schoolemail.Password;
                if (sourceMailID == null || sourceMailPassword == null)
                {
                    throw new Exception("Please enter the Email details!.");
                }




                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(sourceMailID.Address, sourceMailPassword)
                };

                var message = new MailMessage(sourceMailID.Address, Tomailid)
                {
                    Subject = subject,
                    Body = bodymessage,
                };

                using (message)
                {
                    smtp.Send(message);
                }




                //string sourceMailID = ConfigurationManager.AppSettings["SourceMailID"];
                //string sourceMailPassword = ConfigurationManager.AppSettings["SourceMailPassword"];
                //string smtpsHost = ConfigurationManager.AppSettings["smtpsHost"];
                //int port = Convert.ToInt32(ConfigurationManager.AppSettings["smtpsPort"]);
                //bool enableSSL = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
                //bool useDefaultCredentials = Convert.ToBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
                //using (MailMessage mail = new MailMessage())
                //{
                //    mail.From = new MailAddress(sourceMailID);
                //    mail.To.Add(Tomailid);
                //    mail.To.Add(sourceMailID);
                //    mail.Subject = subject;
                //    mail.Body = bodymessage;
                //    mail.IsBodyHtml = true;
                //    mail.ReplyToList.Add(Tomailid);
                //    //  mail.Attachments.Add(new Attachment("C:\\file.zip"));

                //    using (SmtpClient smtp1 = new SmtpClient(smtpsHost.Trim(), port))
                //    {
                //        smtp1.EnableSsl = enableSSL;
                //        smtp1.UseDefaultCredentials = useDefaultCredentials;
                //        //smtp1.Timeout = 20000;
                //        smtp1.Credentials = new NetworkCredential(sourceMailID.Trim(), sourceMailPassword.Trim());
                //        smtp1.DeliveryMethod = SmtpDeliveryMethod.Network;
                //        smtp1.Send(mail);
                //    }
                //}
            }
            catch (SmtpFailedRecipientsException sfrEx)
            {
                // TODO: Handle exception
                // When email could not be delivered to all receipients.
                return "F";
            }
            catch (SmtpException sEx)
            {
                // TODO: Handle exception
                // When SMTP Client cannot complete Send operation.
                return "F";
            }
            catch (Exception ex)
            {
                return "F";
            }
            return "S";
        }

        public ActionResult PaymentProcessSchoolFeeResult()
        {
            try
            {
                InputPaymentResult objPaymentStatus = new InputPaymentResult();
                string merchantResponse = Request.Form["merchantResponse"];
                Console.WriteLine("Inside PaymentProcessSchoolFeeResult. " + merchantResponse);
                if (Request.Form["merchantResponse"] != null)
                {
                    Console.WriteLine("Inside PaymentProcessSchoolFeeResult. Merchant Response is not null.");
                    //string merchantResponse = Request.Form["merchantResponse"];
                    //string key = ConfigurationManager.AppSettings["SchoolPaymentkey"];
                    string key = ConfigurationManager.AppSettings["SchoolPaymentkey"];


                    AWLMEAPI transact = new AWLMEAPI();
                    ResMsgDTO objResMsgDTO = transact.parseTrnResMsg(merchantResponse, key);

                    decimal Amount = 0;
                    Amount = Convert.ToDecimal(objResMsgDTO.TrnAmt) / 100;

                    objPaymentStatus.AddField1 = objResMsgDTO.AddField1;
                    objPaymentStatus.AddField2 = objResMsgDTO.AddField2;
                    //objPaymentStatus.AddField3 = objResMsgDTO.AddField3;
                    objPaymentStatus.AddField4 = objResMsgDTO.AddField4;
                    objPaymentStatus.AddField5 = objResMsgDTO.AddField5;
                    objPaymentStatus.AddField6 = objResMsgDTO.AddField6;
                    objPaymentStatus.AddField7 = objResMsgDTO.AddField7;
                    objPaymentStatus.AddField8 = objResMsgDTO.AddField8;
                    objPaymentStatus.AddField9 = objResMsgDTO.AddField9;
                    objPaymentStatus.AuthZCode = objResMsgDTO.AuthZCode;
                    objPaymentStatus.PgMeTrnRefNo = objResMsgDTO.PgMeTrnRefNo;

                    objPaymentStatus.TrnAmt = Amount.ToString();
                    //objPaymentStatus.TrnAmt = objResMsgDTO.TrnAmt;

                    objPaymentStatus.TrnReqDate = objResMsgDTO.TrnReqDate;
                    objPaymentStatus.Rrn = objResMsgDTO.Rrn;
                    objPaymentStatus.OrderId = objResMsgDTO.OrderId;

                    string Status = string.Empty;
                    if (objResMsgDTO.StatusCode == "S")
                    {
                        //Console.WriteLine("Inside PaymentProcessSchoolFeeResult. Status Code is S");
                        Status = "Success";

                        objPaymentStatus.StatusCode = Status.ToString();
                        objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                        objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                        int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                        var discountamount = Convert.ToString(objPaymentStatus.AddField4);

                        PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                        {
                            TransactionStatus = Status,
                            TransactionError = objResMsgDTO.StatusDesc,
                            ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                            TrackId = objResMsgDTO.Rrn,
                            PaymentId = objResMsgDTO.ResponseCode
                        };

                        var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                        _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                        _context.SaveChanges();

                        //var id = 75;
                        //var link = "/AdmissionFee/PrintReceiptusingrotativa?id=1&ReceiptId=" + id + "";

                        var feeid = genericadmissionfeecalculator(objPaymentStatus, stuid);

                        if (feeid != "")
                        {
                            int feereceiptId = Convert.ToInt32(feeid);
                            var studentreereipt = _context.TblFeeReceipts.FirstOrDefault(x => x.FeeReceiptId == feereceiptId);

                            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == stuid);
                            if (studentdata != null)
                            {
                                if (studentdata.Parents_Email != null)
                                {
                                    var studentname = studentdata.Name + " " + studentdata.Last_Name;
                                    //need to change the link when publish into server
                                    //var link = "http://svd.orootsfoundations.org/Payment/AdmissionFeereceipt?id=" + feeid + "";
                                    var link = "https://www.carmelteresaschool.in/Payment/AdmissionFeereceipt?id=" + feeid + "";

                                    //For Receipt
                                    var str = SendEmail("" + studentdata.Parents_Email + "", "Admission Fee Payment Receipt", "" + studentname + " You can Download your Admission Fee Receipt through this Link" + link + "");

                                    if (str == "S")
                                    {
                                        EmailViewModel emailViewModel = new EmailViewModel();
                                        emailViewModel.Student_id = Convert.ToInt32(studentdata.StudentRegisterID);
                                        emailViewModel.ApplicationNumber = studentdata.ApplicationNumber;
                                        emailViewModel.Name = studentdata.Name + " " + studentdata.Last_Name;
                                        emailViewModel.Parent_Email = studentdata.Parents_Email;
                                        emailViewModel.Email_Date = DateTime.Now.ToString();
                                        emailViewModel.Email_Content = link;

                                        var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                                    }

                                    //Admit the student
                                    var createuser = new StudentAdmissionController().CreateUserlogin(stuid);
                                    //need to change the link when publish into server.
                                    var loginpagelink = "https://www.carmelteresaschool.in/";
                                    //var loginpagelink = "http://svd.orootsfoundations.org/";
                                    if (createuser != null)
                                    {
                                        int userid = Convert.ToInt32(createuser);
                                        var userlogin = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);

                                        if (userlogin != null)
                                        {

                                            //For username and password
                                            var useremail = SendEmail("" + studentdata.Parents_Email + "", "Regarding Username and Password", "You can login the website using this link" + loginpagelink + ",Username is " + userlogin.UserName + " and password is " + userlogin.Password + ",Your Application has been to the Admitted status successfully");
                                            if (useremail == "S")
                                            {
                                                EmailViewModel emailViewModel = new EmailViewModel();
                                                emailViewModel.Student_id = Convert.ToInt32(studentdata.StudentRegisterID);
                                                emailViewModel.ApplicationNumber = studentdata.ApplicationNumber;
                                                emailViewModel.Name = studentdata.Name + " " + studentdata.Last_Name;
                                                emailViewModel.Parent_Email = studentdata.Parents_Email;
                                                emailViewModel.Email_Date = DateTime.Now.ToString();
                                                emailViewModel.Email_Content = loginpagelink;

                                                var emailarchieve = new SMSandEmailController().AddEmailArchieve(emailViewModel);
                                            }
                                        }



                                    }
                                }
                            }
                        }

                        return Content("<script language='javascript' type='text/javascript'>window.location.href='/Account/Login';alert('Transaction Completed Successfully!, Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ", Transaction Amount:" + objPaymentStatus.TrnAmt + ", Transaction Request Date:" + objPaymentStatus.TrnReqDate + ", Transaction Status:" + objPaymentStatus.StatusCode + ", Transaction Description" + objPaymentStatus.StatusDesc + ", Student Name:" + objPaymentStatus.AddField1 + ", Class:" + objPaymentStatus.AddField2 + "')</script>");



                    }
                    else
                    {
                        Status = "Failed";
                        objPaymentStatus.StatusCode = Status.ToString();
                        objPaymentStatus.StatusDesc = objResMsgDTO.StatusDesc;
                        objPaymentStatus.ResponseCode = objResMsgDTO.ResponseCode;

                        int stuid = Convert.ToInt32(objPaymentStatus.AddField6);
                        var discountamount = Convert.ToString(objPaymentStatus.AddField4);
                        PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                        {
                            TransactionStatus = Status,
                            TransactionError = objResMsgDTO.StatusDesc,
                            ReferenceNo = objResMsgDTO.PgMeTrnRefNo,
                            TrackId = objResMsgDTO.Rrn,
                            PaymentId = objResMsgDTO.ResponseCode
                        };
                        var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TransactionId == objPaymentStatus.OrderId.ToString());
                        _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                        _context.SaveChanges();

                        return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Failed,Transaction Reference: " + objPaymentStatus.PgMeTrnRefNo + ",Transaction Amount:" + objPaymentStatus.TrnAmt + ",Transaction Request Date:" + objPaymentStatus.TrnReqDate + ",Transaction Status:" + objPaymentStatus.StatusCode + ",Transaction Description:" + objPaymentStatus.StatusDesc + ",Student Name:" + objPaymentStatus.AddField1 + ",Class:" + objPaymentStatus.AddField2 + "')</script>");
                    }
                }
                else
                {
                    //ViewBag.ResultText = "No Data Can be Displayed......Session is Null";
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception. " + e.ToString());
                throw (e);
            }
            //return View(objPaymentStatus);
        }


        //public ActionResult AdmissionFeePaymentProcessschoolfeeresult()
        //{

        //}

        //genericfeecalculator
        private string genericadmissionfeecalculator(InputPaymentResult objPaymentStatus, int stuid)
        {
            //var studentdetails = _context.Students.FirstOrDefault(x => x.StudentId == stuid);
            var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == stuid);
            int calssid = Convert.ToInt32(studentdetails.Class_Id);
            int Categoryid = Convert.ToInt32(studentdetails.Category_Id);
            var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "Class").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId == calssid);
            var categoryDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "category").DataListId.ToString()).FirstOrDefault(x => x.DataListItemId == Categoryid);
            List<FeeHeadings> feeHeadingList = _FeeHeadingsRepository.GetAll().ToList();
            var unicNumber = Guid.NewGuid();
            //string[] Feeheadings;
            //string[] feeheader = new string[50];
            //string[] feepaiddetails = new string[50];
            //string[] feeheadingid = new string[50];

            string feeHeadingNames = string.Empty;
            string feeHeadingAmts = string.Empty;
            string feeHeadingIDs = string.Empty;
            if (objPaymentStatus.AddField8 != null && objPaymentStatus.AddField8 != "")
            {
                string[] FeeHeadingSplit = objPaymentStatus.AddField8.Split(',');
                if (FeeHeadingSplit != null)
                {
                    var fh = FeeHeadingSplit.Take(FeeHeadingSplit.Length - 1).ToList();

                    if (fh.Count > 0)
                    {
                        for (int i = 0; i < fh.Count; i++)
                        {
                            if (fh[i] != null && fh[i] != "")
                            {
                                string[] FeeheadingSecond = fh[i].Split('~');
                                int feeid = Convert.ToInt32(FeeheadingSecond[0]);
                                float feeamont = (float)Convert.ToDecimal(FeeheadingSecond[1]);
                                FeeHeadings feeHeading = feeHeadingList.FirstOrDefault(x => x.FeeId == feeid);
                                //TblFeeReceipts Tbl_FeeReceipts = new TblFeeReceipts();
                                //Tbl_FeeReceipts.FeeHeadingIDs = FeeheadingSecond[0];
                                //var feeheader = feeHeading.FeeName;
                                if (feeHeadingNames != string.Empty)
                                    feeHeadingNames = string.Join(",", feeHeadingNames, feeHeading.FeeName);
                                else
                                    feeHeadingNames = feeHeading.FeeName;
                                if (feeHeadingAmts != string.Empty)
                                    feeHeadingAmts = string.Join(",", feeHeadingAmts, Convert.ToString(feeamont));
                                else
                                    feeHeadingAmts = Convert.ToString(feeamont);
                                if (feeHeadingIDs != string.Empty)
                                    feeHeadingIDs = string.Join(",", feeHeadingIDs, Convert.ToString(FeeheadingSecond[0]));
                                else
                                    feeHeadingIDs = Convert.ToString(FeeheadingSecond[0]);
                            }

                        }
                    }
                }
            }

            #region oldcode

            var batchdetails = _context.Tbl_Batches.Where(x => x.IsActiveForPayments == true).FirstOrDefault();
           
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            tblFeeReceipts.FeeHeadingIDs = feeHeadingIDs;
            string feeMonths = "Jun";

            tblFeeReceipts.BalanceAmt = 0;
            tblFeeReceipts.OldBalance = 0;
            //tblFeeReceipts.BankName = "";
            //tblFeeReceipts.CategoryId = categoryDetail.DataListItemId;
            //tblFeeReceipts.CategoryName = categoryDetail.DataListItemName;
            tblFeeReceipts.ClassId = classDetail.DataListItemId;
            tblFeeReceipts.ClassName = classDetail.DataListItemName;
            //tblFeeReceipts.BatchName = "2021-2035";
            tblFeeReceipts.BatchName = batchdetails.Batch_Name;
            //tblFeeReceipts.FeePaidDate = feeReceiptViewModel.DateTimeVal != null ? feeReceiptViewModel.DateTimeVal : DateTime.Now;
            tblFeeReceipts.AddedDate = DateTime.Now;
            tblFeeReceipts.Concession = 0;
            tblFeeReceipts.ConcessionAmt = 0;
            tblFeeReceipts.LateFee = 0;
            tblFeeReceipts.PaidMonths = "Jun";
            tblFeeReceipts.PayHeadings = feeHeadingNames;
            tblFeeReceipts.PaymentMode = "Online";
            tblFeeReceipts.ReceiptAmt = float.Parse(objPaymentStatus.TrnAmt);
            // tblFeeReceipts.Remark = "Online Payment";
            tblFeeReceipts.StudentId = stuid;
            tblFeeReceipts.StudentName = objPaymentStatus.AddField1;
            tblFeeReceipts.TotalFee = float.Parse(objPaymentStatus.TrnAmt);
            tblFeeReceipts.FeePaids = feeHeadingAmts;
            tblFeeReceipts.FeeReceiptsOneTimeCreator = unicNumber.ToString();
            tblFeeReceipts.DueAmount = "0";
            tblFeeReceipts.PaidAmount = objPaymentStatus.TrnAmt;



            objPaymentStatus.AddField2 = classDetail.DataListItemName;
            //objPaymentStatus.AddField3 = categoryDetail.DataListItemName;


            if (feeMonths.Contains("Jan"))
            {
                tblFeeReceipts.Jan = true;
            }
            if (feeMonths.Contains("Feb"))
            {
                tblFeeReceipts.Feb = true;
            }
            if (feeMonths.Contains("Mar"))
            {
                tblFeeReceipts.Mar = true;
            }
            if (feeMonths.Contains("Apr"))
            {
                tblFeeReceipts.Apr = true;
            }
            if (feeMonths.Contains("May"))
            {
                tblFeeReceipts.May = true;
            }
            if (feeMonths.Contains("Jun"))
            {
                tblFeeReceipts.Jun = true;
            }
            if (feeMonths.Contains("Jul"))
            {
                tblFeeReceipts.Jul = true;
            }
            if (feeMonths.Contains("Aug"))
            {
                tblFeeReceipts.Aug = true;
            }
            if (feeMonths.Contains("Sep"))
            {
                tblFeeReceipts.Sep = true;
            }
            if (feeMonths.Contains("Oct"))
            {
                tblFeeReceipts.Oct = true;
            }
            if (feeMonths.Contains("Nov"))
            {
                tblFeeReceipts.Nov = true;
            }
            if (feeMonths.Contains("Dec"))
            {
                tblFeeReceipts.Dec = true;
            }
            _TblFeeReceiptsRepository.Insert(tblFeeReceipts);
            _TblFeeReceiptsRepository.Save();

            var feereceiptsid = _context.TblFeeReceipts.FirstOrDefault(x => x.StudentId == stuid && x.FeeReceiptsOneTimeCreator == unicNumber.ToString());
            var receiptid = "";
            if (feereceiptsid != null)
            {
                receiptid = feereceiptsid.FeeReceiptId.ToString();
            }
            return receiptid;
            #endregion
        }



        //Admission Feepage
        public ActionResult AdmissionFeepage(int studentId)
        {
            //int studentId = 2639;

            //var feereceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.)

            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
            StudentDetailVM studentsRegistration = new StudentDetailVM();
            if (studentdata != null)
            {
                var feevalue = _context.FeePlans.FirstOrDefault(x => x.FeeType_Id == 243 && x.ClassId == studentdata.Class_Id);
                var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                if (feevalue != null)
                {
                    studentsRegistration.Name = studentdata.Name + "" + studentdata.Last_Name;
                    studentsRegistration.Last_Name = studentdata.Last_Name;
                    studentsRegistration.EmailId = studentdata.Parents_Email;
                    studentsRegistration.FatherName = familydata == null ? "" : familydata.FatherName;
                    studentsRegistration.MotherName = familydata == null ? "" : familydata.MotherName;
                    studentsRegistration.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdata.Class_Id)?.DataListItemName;
                    studentsRegistration.AmissionFee = feevalue.FeeValue.ToString();
                    studentsRegistration.FatherMobileNo = familydata == null ? "" : familydata.FMobile;
                    studentsRegistration.ResidenceLocation = familydata == null ? "" : familydata.FResidentialAddress;
                    studentsRegistration.ApplicationNo = studentdata.ApplicationNumber;
                    studentsRegistration.Class_Id = studentdata.Class_Id;
                    studentsRegistration.Studentid = studentId;
                }
            }


            return View(studentsRegistration);
        }

        //Admission Feepage
        public ActionResult RegistrationFeepage(int studentRegisterId)
        {
            //int studentId = 2639;

            //var feereceipts = _context.TblFeeReceipts.FirstOrDefault(x => x.)

            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentRegisterId);
            StudentDetailVM studentsRegistration = new StudentDetailVM();
            if (studentdata != null)
            {
                var feevalue = _context.FeePlans.FirstOrDefault(x => x.FeeId == 39 && x.ClassId == studentdata.Class_Id);
                var familydata = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == studentdata.ApplicationNumber);
                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
                if (feevalue != null)
                {
                    studentsRegistration.Name = studentdata.Name + "" + studentdata.Last_Name;
                    studentsRegistration.Last_Name = studentdata.Last_Name;
                    studentsRegistration.EmailId = studentdata.Parents_Email;
                    studentsRegistration.FatherName = familydata == null ? "" : familydata.FatherName;
                    studentsRegistration.MotherName = familydata == null ? "" : familydata.MotherName;
                    studentsRegistration.Class = Class.FirstOrDefault(x => x.DataListItemId == studentdata.Class_Id)?.DataListItemName;
                    studentsRegistration.AmissionFee = feevalue.FeeValue.ToString();
                    studentsRegistration.FatherMobileNo = familydata == null ? "" : familydata.FMobile;
                    studentsRegistration.ResidenceLocation = familydata == null ? "" : familydata.FResidentialAddress;
                    studentsRegistration.ApplicationNo = studentdata.ApplicationNumber;
                    studentsRegistration.Class_Id = studentdata.Class_Id;
                    studentsRegistration.Studentid = studentRegisterId;
                }
            }


            return View(studentsRegistration);
        }


        public JsonResult AdmissionAmountTransferGateway(FeeReceiptViewModel feeReceiptViewModel, bool isStudent)
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

                var studentData = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == feeReceiptViewModel.StudentId);

                studentData.Class = Classlist.FirstOrDefault(x => x.DataListItemId == studentData.Class_Id)?.DataListItemName;
                studentData.Category = Categorylist.FirstOrDefault(x => x.DataListItemId == studentData.Category_Id)?.DataListItemName;
                //Classes classDetail = _ClassesRepository.GetAll().FirstOrDefault(x => x.ClassName.ToLower() == feeReceiptViewModel.ClassName.ToLower());
                var classDetail = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(c => c.DataListName.ToLower() == "semester").DataListId.ToString()).FirstOrDefault(x => x.DataListItemName.ToLower() == studentData.Class.ToLower());

                objPaymentViewModels.BloodGroup = studentData.BloodGroup;
                objPaymentViewModels.Category = studentData.Category;
                objPaymentViewModels.Class = studentData.Class;
                objPaymentViewModels.DOB = studentData.DOB;
                objPaymentViewModels.Gender = studentData.Gender;
                objPaymentViewModels.MotherTongue = studentData.MotherTongue;
                objPaymentViewModels.Name = studentData.Name;
                objPaymentViewModels.Nationality = studentData.Nationality;
                objPaymentViewModels.POB = studentData.POB;
                objPaymentViewModels.Religion = studentData.Religion;
                objPaymentViewModels.Nationality = studentData.Nationality;
                objPaymentViewModels.ApplicationNumber = studentData.ApplicationNumber;
                objPaymentViewModels.DueFee = feeReceiptViewModel.DueFee;
                objPaymentViewModels.Section = studentData.Section;
                objPaymentViewModels.RoleNumber = studentData.RollNo.ToString();

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
                throw ex;
            }
            return Json("");

        }

        //Receipt For Admission Fee
        public ActionResult AdmissionFeereceipt(int? id)
        {
            TblFeeReceipts tblFeeReceipts = new TblFeeReceipts();
            List<PreviewFeeReceiptViewModel> ReceiptPreviewList = new List<PreviewFeeReceiptViewModel>();
            //var session = Session["rolename"].ToString();
            //if (session == null)
            //{
            //    return RedirectToAction("Login", "Account");
            //}
            //else
            //{
            //    ViewBag.session = session;z
            //}
            {
                ViewBag.Date = System.DateTime.Now;
                tblFeeReceipts = _TblFeeReceiptsRepository.GetById(id);
                int? studentId = tblFeeReceipts.StudentId;
                {
                    ViewBag.ScollarNumber = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId).StudentRegisterID;
                    var studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                    ViewBag.studentDetails = studentDetails;
                    ViewBag.Total = tblFeeReceipts.ReceiptAmt;
                    ViewBag.classname = tblFeeReceipts.ClassName;
                    ViewBag.receiptid = tblFeeReceipts.FeeReceiptId;
                    ViewBag.concessionamt = tblFeeReceipts.ConcessionAmt;
                    ViewBag.studentname = studentDetails.Name + " " + studentDetails.Last_Name;
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
                            FeePaid = HeadingPaid[i],
                            HeadingNames = AllHeadings[i],
                            SelectedMonths = tblFeeReceipts.PaidMonths
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

                var data = tblFeeReceipts;

                return new Rotativa.ViewAsPdf("AdmissionFeeReceiptPreview", data)
                {
                    //FileName = "FeeReceipt.pdf",
                    //PageMargins = new Rotativa.Options.Margins(10, 5, 10, 5),
                    //PageSize = Rotativa.Options.Size.A5,
                    //PageOrientation = Rotativa.Options.Orientation.Portrait,
                };
            }

        }

        public ActionResult AdmissionFeeReceiptPreview()
        {




            return View();
        }

        #endregion


        //Bank Module
        #region Bank Module


        //Main view for banksetup
        public ActionResult CreateSchool()
        {
            List<TblCreateSchool> tblCreateSchools = new List<TblCreateSchool>();
            tblCreateSchools = _context.TblCreateSchool.ToList();
            ViewBag.Schoolist = tblCreateSchools;

            List<Tbl_CreateBank> tbl_CreateBanks = new List<Tbl_CreateBank>();
            tbl_CreateBanks = _context.Tbl_CreateBank.ToList();
            ViewBag.BanlLIst = tbl_CreateBanks;

            var Bramchlist = _context.Tbl_CreateBranch.ToList();
            List<Tbl_CreateBranch> Tbl_CreateBranch = new List<Tbl_CreateBranch>();
            foreach (var item in Bramchlist)
            {
                item.Bank_Name = tbl_CreateBanks.FirstOrDefault(x => x.Bank_Id == item.Bank_Id)?.Bank_Name;
                Tbl_CreateBranch.Add(item);
            }
            ViewBag.Bramchlist = Tbl_CreateBranch.ToList();

            List<TblMerchantclass> TblMerchantclass = new List<TblMerchantclass>();
            var Tbl_MerchantName = _context.Tbl_MerchantName.ToList();
            foreach (var item in Tbl_MerchantName)
            {
                TblMerchantclass.Add(new TblMerchantclass
                {
                    Schoolname = tblCreateSchools.FirstOrDefault(x => x.School_Id == item.School_Id)?.School_Name,
                    BankName = tbl_CreateBanks.FirstOrDefault(x => x.Bank_Id == item.Bank_Id)?.Bank_Name,
                    BranchName = Bramchlist.FirstOrDefault(x => x.Branch_ID == item.Branch_Id)?.Branch_Name,
                    MerchantName = item.MerchantName,
                    MerchantName_Id = item.MerchantName_Id
                });
            }
            ViewBag.Merchantname = TblMerchantclass;
            List<TblMerchantclass> tbl_CreateMerchantIds = new List<TblMerchantclass>();
            var Merchantidlist = _context.Tbl_CreateMerchantId.ToList();
            foreach (var item in Merchantidlist)
            {
                tbl_CreateMerchantIds.Add(new TblMerchantclass
                {
                    Schoolname = tblCreateSchools.FirstOrDefault(x => x.School_Id == item.School_Id)?.School_Name,
                    BankName = tbl_CreateBanks.FirstOrDefault(x => x.Bank_Id == item.Bank_Id)?.Bank_Name,
                    BranchName = Bramchlist.FirstOrDefault(x => x.Branch_ID == item.Branch_Id)?.Branch_Name,
                    MerchantName = Tbl_MerchantName.FirstOrDefault(x => x.MerchantName_Id == item.MerchantName_Id)?.MerchantName,
                    MerchantId = item.Merchant_Id,
                    MerchantMID = item.MerchantMID,
                    MerchantKey = item.MerchantKey

                });
            }
            ViewBag.MerchantIdlist = tbl_CreateMerchantIds;


            ViewBag.Merchantnamelist = Tbl_MerchantName.ToList();

            List<TblMerchantclass> tblschoolsetup = new List<TblMerchantclass>();
            var schoolsetup = _context.Tbl_SchoolSetup.ToList();
            foreach (var item in schoolsetup)
            {
                tblschoolsetup.Add(new TblMerchantclass
                {
                    Schoolname = tblCreateSchools.FirstOrDefault(x => x.School_Id == item.School_Id)?.School_Name,
                    BankName = tbl_CreateBanks.FirstOrDefault(x => x.Bank_Id == item.Bank_Id)?.Bank_Name,
                    BranchName = Bramchlist.FirstOrDefault(x => x.Branch_ID == item.Branch_Id)?.Branch_Name,
                    Status = item.Status,
                    Schoolsetup_Id = item.Schoolsetup_Id,
                    Feeconfiguration = item.Fee_Configuratinname
                });
            }
            ViewBag.Schoolsetup = tblschoolsetup;

            //Fee Configuration
            var feeconfiguration = _context.DataListItems.Where(x => x.DataListId == "31").ToList();
            ViewBag.FeeConfiguration = feeconfiguration;

            //Board Dev- Yash Talaiche Date-12-07-2023
            var schoolBoard = _context.schoolBoards.ToList();
            ViewBag.SchoolBoards = schoolBoard;
            return View();
        }

        //Create School
        #region Create School

        public ActionResult AddSchool(TblCreateSchool tblCreateSchool, HttpPostedFileBase Upload_Image)
        {
            try
            {
                if (Upload_Image != null)
                {
                    if (Upload_Image.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Upload_Image.FileName);
                        var path = Path.Combine(Server.MapPath("~/WebsiteImages/SchoolImage/") + filename);
                        Upload_Image.SaveAs(path);
                        tblCreateSchool.Upload_Image = filename;
                    }
                }
                _context.TblCreateSchool.Add(tblCreateSchool);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public JsonResult GetMerchantBySchool(int id)
        {
            List<Tbl_MerchantName> merchants = _context.Tbl_MerchantName.Where(x => x.School_Id == id).ToList();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetSchoolById(int id)
        {
            try
            {
                var data = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == id);
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


        public ActionResult UpdateSchool(TblCreateSchool tblCreateSchool, HttpPostedFileBase Upload_Image)
        {
            try
            {
                var data = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == tblCreateSchool.School_Id);
                if (Upload_Image != null)
                {
                    if (Upload_Image.ContentLength > 0)
                    {
                        var filename = Path.GetFileName(Upload_Image.FileName);
                        var path = Path.Combine(Server.MapPath("~/WebsiteImages/SchoolImage/") + filename);
                        Upload_Image.SaveAs(path);
                        tblCreateSchool.Upload_Image = filename;
                    }
                }
                else
                {
                    tblCreateSchool.Upload_Image = data.Upload_Image;
                }

                _context.Entry(data).CurrentValues.SetValues(tblCreateSchool);
                _context.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteSchool(int Id)
        {
            try
            {
                var data = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == Id);
                if (data != null)
                {
                    _context.TblCreateSchool.Remove(data);
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        //Create Bank

        #region CreateBank
        public ActionResult AddBank(Tbl_CreateBank tbl_CreateBank)
        {
            try
            {
                _context.Tbl_CreateBank.Add(tbl_CreateBank);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetBankById(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateBank.FirstOrDefault(x => x.Bank_Id == Id);
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

        public ActionResult UpdateBank(Tbl_CreateBank tbl_CreateBank)
        {
            try
            {
                var data = _context.Tbl_CreateBank.FirstOrDefault(x => x.Bank_Id == tbl_CreateBank.Bank_Id);
                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_CreateBank);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert(Data Not Updated);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteBank(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateBank.FirstOrDefault(x => x.Bank_Id == Id);
                if (data != null)
                {
                    _context.Tbl_CreateBank.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Create Branch(Gateways)
        #region Create Branch
        public ActionResult AddBranch(Tbl_CreateBranch tbl_CreateBranch)
        {
            try
            {
                _context.Tbl_CreateBranch.Add(tbl_CreateBranch);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetBranchById(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateBranch.FirstOrDefault(x => x.Branch_ID == Id);
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

        public ActionResult UpdateBranch(Tbl_CreateBranch tbl_CreateBranch)
        {
            try
            {
                var data = _context.Tbl_CreateBranch.FirstOrDefault(x => x.Branch_ID == tbl_CreateBranch.Branch_ID);
                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_CreateBranch);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert('Data Not Updated');</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteBranch(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateBranch.FirstOrDefault(x => x.Branch_ID == Id);
                if (data != null)
                {
                    _context.Tbl_CreateBranch.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //Create Merchant Name
        #region Create Merchant Name
        public ActionResult AddMerchantName(Tbl_MerchantName tbl_MerchantName)
        {
            try
            {
                _context.Tbl_MerchantName.Add(tbl_MerchantName);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetMerchantNameById(int Id)
        {
            try
            {
                var data = _context.Tbl_MerchantName.FirstOrDefault(x => x.MerchantName_Id == Id);
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

        public ActionResult UpdateMerchantName(Tbl_MerchantName tbl_MerchantName)
        {
            try
            {
                var data = _context.Tbl_MerchantName.FirstOrDefault(x => x.MerchantName_Id == tbl_MerchantName.MerchantName_Id);
                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_MerchantName);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert('Data Not Updated');</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteMerchantName(int Id)
        {
            try
            {
                var data = _context.Tbl_MerchantName.FirstOrDefault(x => x.MerchantName_Id == Id);
                if (data != null)
                {
                    _context.Tbl_MerchantName.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        //Create Merchant ID
        #region Create Merchant Name
        public ActionResult AddMerchantId(Tbl_CreateMerchantId tbl_CreateMerchantId)
        {
            try
            {
                _context.Tbl_CreateMerchantId.Add(tbl_CreateMerchantId);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public JsonResult GetMerchantIdById(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateMerchantId.FirstOrDefault(x => x.Merchant_Id == Id);
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

        public ActionResult UpdateMerchantId(Tbl_CreateMerchantId tbl_CreateMerchantId)
        {
            try
            {
                var data = _context.Tbl_CreateMerchantId.FirstOrDefault(x => x.Merchant_Id == tbl_CreateMerchantId.Merchant_Id);
                if (data != null)
                {
                    _context.Entry(data).CurrentValues.SetValues(tbl_CreateMerchantId);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert('Data Not Updated');</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteMerchantID(int Id)
        {
            try
            {
                var data = _context.Tbl_CreateMerchantId.FirstOrDefault(x => x.Merchant_Id == Id);
                if (data != null)
                {
                    _context.Tbl_CreateMerchantId.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');alert(Data Not Deleted);</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion


        public JsonResult GetbankdetailsById(int id)
        {
            try
            {
                var data = _context.Tbl_CreateBranch.Where(x => x.Bank_Id == id).ToList();
                if (data.Count > 0)
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

        public JsonResult GetmerchantdetailsById(int id)
        {
            try
            {
                var data = _context.Tbl_MerchantName.Where(x => x.Branch_Id == id).ToList();
                if (data.Count > 0)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(string.Empty, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //School Setup
        #region Create School

        public ActionResult AddSchoolsetup(Tbl_SchoolSetup tbl_SchoolSetup)
        {
            try
            {
                Tbl_SchoolSetup data = _context.Tbl_SchoolSetup.Where(x => x.School_Id == tbl_SchoolSetup.School_Id).FirstOrDefault();

                var datalist = _context.DataListItems.ToList();
                int feeid = Convert.ToInt32(tbl_SchoolSetup.Fee_configurationId);
                tbl_SchoolSetup.Fee_Configuratinname = datalist.FirstOrDefault(x => x.DataListItemId == feeid)?.DataListItemName;

                if (data != null && data.School_Id > 0)
                {
                    tbl_SchoolSetup.Schoolsetup_Id = data.Schoolsetup_Id;
                    _context.Entry(data).CurrentValues.SetValues(tbl_SchoolSetup);
                    _context.SaveChanges();
                }
                else
                {
                    _context.Tbl_SchoolSetup.Add(tbl_SchoolSetup);
                    _context.SaveChanges();
                }

                //if(data.Count > 0)
                //{
                //    foreach (var item in data)
                //    {
                //        _context.Tbl_SchoolSetup.Remove(item);
                //        _context.SaveChanges();
                //    }
                //}

                Tbl_SchoolSetup activeschool = _context.Tbl_SchoolSetup.Where(x => x.Status.Equals("Active", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (activeschool == null || activeschool.School_Id == 0)
                    activeschool = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.School_Id == tbl_SchoolSetup.School_Id);

                var activeschol = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == activeschool.School_Id);

                Session["SchoolImage"] = activeschol.Upload_Image;
                Session["SchoolName"] = activeschol.School_Name;

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult GetSchoolsetupById(int id)
        {
            try
            {
                var data = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Schoolsetup_Id == id);
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


        public ActionResult UpdateSchoolsetup(Tbl_SchoolSetup tbl_SchoolSetup)
        {
            try
            {
                var data = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Schoolsetup_Id == tbl_SchoolSetup.Schoolsetup_Id);
                _context.Entry(data).CurrentValues.SetValues(tbl_SchoolSetup);
                _context.SaveChanges();

                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public ActionResult DeleteSchoolsetup(int Id)
        {
            try
            {
                var data = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Schoolsetup_Id == Id);
                if (data != null)
                {
                    _context.Tbl_SchoolSetup.Remove(data);
                    _context.SaveChanges();
                }
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Payment/CreateSchool');</script>");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion


        //Razor Pay
        public ActionResult RazorpayPaymentprocess()
        {
            PaymentViewModels paymentViewModels = (PaymentViewModels)TempData["objPaymentViewModels"];
            var ClassList = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            string orderId = "";
            PaymentResultModels paymentResultModels = new PaymentResultModels();
            if (paymentViewModels != null)
            {
                var studentid = paymentViewModels.StudentId;
                var classdetails = ClassList.FirstOrDefault(x => x.DataListItemName == paymentResultModels.Class)?.DataListItemId;
                ViewBag.PaymentStudentName = paymentViewModels.Name;
                paymentResultModels.StudentName = paymentViewModels.Name;
                //ViewBag.PaymentStudentClass = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId;
                //ObjPaymentResultModels.Class = ClassList.FirstOrDefault(x => x.DataListItemName == objPaymentViewModels.Class)?.DataListItemId.ToString();
                ViewBag.PaymentStudentClass = paymentViewModels.Class;
                paymentResultModels.Class = paymentViewModels.Class;
                ViewBag.PaymentStudentSection = paymentViewModels.Section;
                paymentResultModels.SectionName= paymentViewModels.Section;
                ViewBag.PaymentStudentCategory = paymentViewModels.Category;
                paymentResultModels.Category = paymentViewModels.Category;
                ViewBag.PaymentStudentRoleNumber = paymentViewModels.RoleNumber;
                paymentResultModels.RoleNumber = paymentViewModels.RoleNumber;
                //ViewBag.PaymentStudentBatch = objPaymentViewModels.Batch;
                //ObjPaymentResultModels.Batch = objPaymentViewModels.Batch;
                ViewBag.PaymentStudentTCBal = paymentViewModels.TCBal;
                paymentResultModels.TCBal = paymentViewModels.TCBal;
                paymentResultModels.FeeHeadings = paymentViewModels.FeeHeadings;
                paymentResultModels.Feeheadingamt = paymentViewModels.Feeheadingamt;
                paymentResultModels.studentid = studentid;
                paymentResultModels.Concession = paymentViewModels.Concession;
                paymentResultModels.ConcessionAmt = paymentViewModels.ConcessionAmt;
                paymentResultModels.Amount = paymentViewModels.TCBal;
                paymentResultModels.Currency = "INR";
                paymentResultModels.Email = paymentViewModels.Email;
                int sid = Convert.ToInt32(studentid);
                var app = _context.Students.FirstOrDefault(x => x.StudentId == sid).ApplicationNumber;
                var famildetails = _context.FamilyDetails.FirstOrDefault(x => x.ApplicationNumber == app);

                paymentResultModels.Contact = famildetails == null ? "" : famildetails.FMobile;

                string PaymentAmount = paymentResultModels.TCBal;
                //string PaymentAmount = transactionamount;
                decimal payAmount = 0;
                payAmount = Convert.ToDecimal(PaymentAmount);
                payAmount = payAmount * 100;
                Guid g = Guid.NewGuid();

                var trackId = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                InputPayment objInputPayment = new InputPayment();
                objInputPayment.OrderId = trackId.ToString();

                string key = "";
                string secret = "";

                TblMerchantclass tblMerchantclass = new TblMerchantclass();
                List<Tbl_CreateMerchantId> tbl_CreateMerchantIds = new List<Tbl_CreateMerchantId>();
                var merchantid = _context.Tbl_CreateMerchantId.ToList();
                var mercantdetails = _context.Tbl_MerchantName.ToList();
                //Active Merchant
                var merchantdata = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
                if (merchantdata != null)
                {
                    foreach (var item in merchantid)
                    {
                        if (item.Bank_Id == merchantdata.Bank_Id && item.Branch_Id == merchantdata.Branch_Id && item.School_Id == merchantdata.School_Id)
                        {
                            tbl_CreateMerchantIds.Add(new Tbl_CreateMerchantId
                            {
                                MerchantMID = item.MerchantMID,
                                MerchantKey = item.MerchantKey,
                                MerchantName_Id = item.MerchantName_Id
                            });
                        }
                    }
                }

                if (paymentViewModels.FeeHeadings == "Transport Fee,Transport Fee(IIndTerm)" || paymentViewModels.FeeHeadings == "Transport Fee" || paymentViewModels.FeeHeadings == "Transport Fee(IIndTerm)" || paymentViewModels.FeeHeadings == "Transport Fee(IIndTerm),Transport Fee")
                {
                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Transport")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }
                }
                else if (classdetails != null && classdetails.ToString() == "207" || classdetails.ToString() == "208" || classdetails.ToString() == "209" || classdetails.ToString() == "210")
                {
                    //PRE PRIMARY SCHOOL

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Nursery")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    paymentResultModels.Key = key;
                }

                else
                {
                    //CARMEL TERESA SCHOOL

                    foreach (var item in tbl_CreateMerchantIds)
                    {
                        var merchantname = mercantdetails.FirstOrDefault(x => x.MerchantName == "Primary")?.MerchantName_Id;
                        if (item.MerchantName_Id == merchantname)
                        {
                            key = item.MerchantMID;
                            secret = item.MerchantKey;
                        }
                    }

                    //key = "rzp_test_YiLpQuKkRgXvaO";
                    //secret = "wHX97vwO4liEZmBge05UNQEd";
                    paymentResultModels.Key = key;
                }
                PaymentTransactionId paymentTransactionId = new PaymentTransactionId();
                paymentTransactionId.Merchant_Key = key;
                paymentTransactionId.Secret_Key = secret;

                TempData["MerchantDetails"] = paymentTransactionId;

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

                Dictionary<string, object> input = new Dictionary<string, object>();
                input.Add("amount", payAmount); // this amount should be same as transaction amount
                input.Add("currency", "INR");
                input.Add("receipt", trackId);
                input.Add("payment_capture", "0");

                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Razorpay.Api.Order order = client.Order.Create(input);
                orderId = order["id"].ToString();
                ViewBag.Orderid = orderId;
                paymentResultModels.OrdedrId = orderId;
                tbl_PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new tbl_PaymentTransactionDetails()
                {
                    TxnDate = DateTime.Now.ToString("dd/MM/yyyy"),
                    Amount = PaymentAmount,
                    TransactionId = trackId.ToString(),
                    Pmntmode = "Online",
                    StudentId = Convert.ToInt32(studentid),
                    TrackId = orderId
                };
                objrep_tbl_PaymentTransactionDetails.Insert(objinsert_tbl_PaymentTransactionDetails);
                objrep_tbl_PaymentTransactionDetails.Save();

                TempData["StudentDetails"] = paymentResultModels;

            }

            return View(paymentResultModels);
        }


        [HttpPost]
        public ActionResult CapturePayment(PaymentResultModels paymentResultModels)
        {

            string paymentid = Request.Form["razorpay_payment_id"];//Paymentid
            string orderid = Request.Form["razorpay_order_id"];//orderid

            string Key = "";
            string Secret = "";

            PaymentTransactionId paymentTransaction = (PaymentTransactionId)TempData["MerchantDetails"];
            if (paymentTransaction != null)
            {
                Key = paymentTransaction.Merchant_Key;
                Secret = paymentTransaction.Secret_Key;
            }


            string Pid = paymentResultModels.paymentid;
            string OId = paymentResultModels.OrdedrId;

            PaymentTransactionId paymentTransactionId = new PaymentTransactionId();
            paymentTransactionId.Paymentid = Pid;
            paymentTransactionId.Orderid = OId;

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(Key, Secret);


            Payment payment = client.Payment.Fetch(Pid);

            //for capture payment
            Dictionary<string, object> options = new Dictionary<string, object>();
            options.Add("amount", payment.Attributes["amount"]);
            Razorpay.Api.Payment paymentcaptured = payment.Capture(options);
            string amt = paymentcaptured.Attributes["amount"];
            TempData["Paymentid"] = paymentTransactionId;
            if (paymentcaptured.Attributes["status"] == "captured")
            {
                return RedirectToAction("PaymentSuccessResult");
            }
            else
            {
                return RedirectToAction("Failed");
            }



        }


        public ActionResult PaymentSuccessResult()
        {

            PaymentResultModels paymentViewModels = (PaymentResultModels)TempData["StudentDetails"];
            PaymentTransactionId paymentTransaction = (PaymentTransactionId)TempData["Paymentid"];
            int stuid = 0;

            InputPaymentResult objPaymentStatus = new InputPaymentResult();
            if (paymentViewModels != null)
            {
                objPaymentStatus.AddField1 = paymentViewModels.StudentName;
                objPaymentStatus.TrnAmt = paymentViewModels.TCBal;
                objPaymentStatus.AddField4 = paymentViewModels.ConcessionAmt.ToString();
                objPaymentStatus.AddField8 = paymentViewModels.Feeheadingamt;
                stuid = Convert.ToInt32(paymentViewModels.studentid);


                PaymentTransactionDetails objinsert_tbl_PaymentTransactionDetails = new PaymentTransactionDetails()
                {
                    TransactionStatus = "Captured",
                    PaymentId = paymentTransaction.Paymentid,
                    TrackId = paymentTransaction.Orderid
                };
                var tblpaymentDetails = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.StudentId == stuid && x.TrackId == paymentTransaction.Orderid);
                _context.Entry(tblpaymentDetails).CurrentValues.SetValues(objinsert_tbl_PaymentTransactionDetails);
                _context.SaveChanges();

                genericFeeCalculator(objPaymentStatus, stuid);
                return Content("<script language='javascript' type='text/javascript'>window.location.href='/Dashboard/Dashboard';alert('Transaction Completed Successfully!')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>location.replace('/Dashboard/Dashboard');alert('No Data Can be Displayed......Session is Null')</script>");
            }



        }

        public ActionResult Success()
        {
            return View();
        }


        public ActionResult Failed()
        {
            return View();
        }

        //Check active school function
        public TblMerchantclass CheckActiveSchool()
        {

            var schooldata = _context.TblCreateSchool.ToList();

            var bankdata = _context.Tbl_CreateBank.ToList();

            var branchdata = _context.Tbl_CreateBranch.ToList();

            var merchantname = _context.Tbl_MerchantName.ToList();

            var merchantid = _context.Tbl_CreateMerchantId.ToList();

            var data = _context.Tbl_SchoolSetup.ToList();
            TblMerchantclass tblMerchantclass = new TblMerchantclass();
            foreach (var item in data)
            {
                if (item.Status == "Active")
                {
                    tblMerchantclass.Schoolname = schooldata.FirstOrDefault(x => x.School_Id == item.School_Id)?.School_Name;
                    tblMerchantclass.BankName = bankdata.FirstOrDefault(x => x.Bank_Id == item.Bank_Id)?.Bank_Name;
                    tblMerchantclass.BranchName = branchdata.FirstOrDefault(x => x.Branch_ID == item.Branch_Id)?.Branch_Name;
                    tblMerchantclass.Status = item.Status;
                }
            }



            return tblMerchantclass;
        }



    }



}