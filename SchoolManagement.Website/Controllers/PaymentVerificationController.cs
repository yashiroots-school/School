using EmployeeManagement.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Payments.EncrypDecrpt;
using Payrequest;
using Payresponse;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.Models.Payment;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SchoolManagement.Website.Models;
using MerchDetails = SchoolManagement.Website.Models.MerchDetails;
using System.Data.Entity;

namespace SchoolManagement.Website.Controllers
{
    public class PaymentVerificationController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private IRepository<tbl_PaymentTransactionDetails> objrep_tbl_PaymentTransactionDetails = null;
        private IRepository<tbl_PaymentTransactionFeeDetails> objrep_tbl_PaymentTransactionFeeDetails = null;
        private IRepository<TblFeeReceipts> _TblFeeReceiptsRepository = null;
        private IRepository<FeeHeadings> _FeeHeadingsRepository = null;
        log4net.ILog logger = log4net.LogManager.GetLogger(typeof(PaymentController));
        public PaymentVerificationController()
        {
            objrep_tbl_PaymentTransactionDetails = new Repository<tbl_PaymentTransactionDetails>();
            objrep_tbl_PaymentTransactionFeeDetails = new Repository<tbl_PaymentTransactionFeeDetails>();
            _TblFeeReceiptsRepository = new Repository<TblFeeReceipts>();
            _FeeHeadingsRepository = new Repository<FeeHeadings>();
        }
        // GET: PaymentVerification
        public ActionResult Index()
        {
            var pendingTxns = _context.tbl_PaymentTransactionDetails
                .Where(p => (p.TransactionStatus == null || p.TransactionStatus == "") &&  p.PaymentId != null && p.PaymentId != "")
                .ToList().OrderByDescending(x=>x.PaymentId);

            var Password = _context.Tbl_CreateMerchantId.Select(x => x.Password).FirstOrDefault();
            List<Payverify.Payverify> li = new List<Payverify.Payverify>();
            foreach (var txn in pendingTxns)
            {
                txn.TxnDate = ConvertToYyyyMmDd(txn.TxnDate);
               VerifyTransaction(txn);
               
            }

            return RedirectToAction("Dashboard", "Dashboard"); //View(");
        }
        public static string ConvertToYyyyMmDd(string inputDate)
        {
            if (string.IsNullOrWhiteSpace(inputDate))
                return null;

            DateTime parsedDate;
            if (DateTime.TryParse(inputDate, out parsedDate))
            {
                // yyyy-MM-dd format me return karega
                return parsedDate.ToString("yyyy-MM-dd");
            }

            // agar parsing fail ho jaye
            return null;
        }
        public ActionResult VerifyTransaction(tbl_PaymentTransactionDetails txn)
        {
            try
            {
                var merchant = _context.Tbl_CreateMerchantId.FirstOrDefault();
                if (merchant == null)
                    throw new Exception("Merchant credentials not found.");

                var encryptor = new EncrypDecrpt();

                // Step 1: Prepare the payload
                var head = new HeadDetails
                {
                    api = "TXNVERIFICATION",
                    source = "OTS"
                };

                var merch = new MerchDetails
                {
                    merchId = Convert.ToInt32(merchant.MerchantMID),
                    password = merchant.Password,
                    merchTxnId = txn.TransactionId,
                    merchTxnDate = txn.TxnDate // Correct format
                };

                var pay = new PayDetails
                {
                    amount = Convert.ToDouble(txn.Amount, System.Globalization.CultureInfo.InvariantCulture),
                    txnCurrency = "INR",
                    atomTxnId = txn.PaymentId
                };

                // Step 2: Generate signature
                string signString = merch.merchId + merch.password + merch.merchTxnId + Convert.ToDecimal(txn.Amount) + pay.txnCurrency + head.api;// $"{merch.merchId}{merch.password}{merch.merchTxnId}{txn.Amount}{pay.txnCurrency}{head.api}";
                var reqHashKey = ConfigurationManager.AppSettings["atomtechReqHashkey"];
                byte[] bytes = Encoding.UTF8.GetBytes(reqHashKey);
                byte[] bt = new System.Security.Cryptography.HMACSHA512(bytes).ComputeHash(Encoding.UTF8.GetBytes(signString));
                string signature = byteToHexString(bt).ToLower();

                pay.signature = signature;

                var payload = new Root
                {
                    payInstrument = new PayInstrument
                    {
                        headDetails = head,
                        merchDetails = merch,
                        payDetails = pay
                    }
                };

                // Step 3: Serialize to JSON and encrypt
                var settings = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                };
                string json = JsonConvert.SerializeObject(payload, Formatting.None, settings);
                string encData = encryptor.Encrypt(json);

                string baseUrl = "https://payment1.atomtech.in/ots/payment/status";
                string fullUrl = $"{baseUrl}?merchId={merch.merchId}&encData={HttpUtility.UrlEncode(encData)}";
                System.IO.File.AppendAllText(@"C:\Yash\RequestData.txt", json + Environment.NewLine);
                System.IO.File.AppendAllText(@"C:\Yash\RequestDataURL.txt", fullUrl + Environment.NewLine);

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(fullUrl);
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;
                Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(json);
                request.ProtocolVersion = HttpVersion.Version11;
                request.Method = "POST";
                request.ContentType = "application/json";
                request.ContentLength = 0;
                request.Timeout = 1000000;

                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    string jsonresponse = response.ToString();
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    string temp = null;
                    string status = "";
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                    var result = jsonresponse.Replace("System.Net.HttpWebResponse", "");
                    var uri = new Uri(ConfigurationManager.AppSettings["atomtechResulturl"] + result);
                    var query = HttpUtility.ParseQueryString(uri.Query);
                    string encData2 = query.Get("encData");
                    if (!string.IsNullOrEmpty(encData2))
                    {
                        string Decryptval = encryptor.decrypt(encData2);
                        var objPayverify = JsonConvert.DeserializeObject<SchoolManagement.Website.Models.RootResponse>(Decryptval);
                        ViewBag.DecryptedData = Decryptval;
                        if (objPayverify != null && objPayverify.payInstrument != null && objPayverify.payInstrument.Count > 0)
                        {
                            var response2 = objPayverify.payInstrument[0].responseDetails;

                            if (objPayverify.payInstrument[0].merchDetails != null)
                            {
                                // Example: transaction ko identify karne ke liye merchTxnId use kar rahe hain
                                var merchTxnId = objPayverify.payInstrument[0].payDetails.atomTxnId;
                                var txns = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.PaymentId == merchTxnId.ToString());
                                var studentdetails = _context.Students.Where(x => x.StudentId == txn.StudentId).FirstOrDefault();
                                var feeIdandAmount = SplitFeeString(txn.FeeIds);

                                using (var dbTransaction = _context.Database.BeginTransaction())
                                {
                                    try
                                    {
                                        if (txn != null)
                                        {
                                            if (response2.statusCode == "OTS0000" || response2.statusCode == "OTS0002")
                                            {
                                                // Insert Fee
                                                var unicNumber = Guid.NewGuid();
                                                var fee = new TblFeeReceipts()
                                                {
                                                    StudentId = txn.StudentId,
                                                    ApplicationNumber = studentdetails.ApplicationNumber,
                                                    ClassId = studentdetails.Class_Id,
                                                    CategoryId = studentdetails.Category_Id,
                                                    AddedDate = Convert.ToDateTime(objPayverify.payInstrument[0].merchDetails.merchTxnDate),
                                                    IsDeleted = false,
                                                    CreateBy = 0,
                                                    Concession = 0,
                                                    ConcessionAmt = 0,
                                                    StudentName = studentdetails.Name,
                                                    PayHeadings = txn.FeeIds,
                                                    OldBalance = 0,
                                                    ReceiptAmt = (float)objPayverify.payInstrument[0].payDetails.amount,
                                                    ClassName = _context.DataListItems.Where(x => x.DataListItemId == studentdetails.StudentId).Select(x => x.DataListItemName).FirstOrDefault(),
                                                    TotalFee = (float)objPayverify.payInstrument[0].payDetails.amount,
                                                    LateFee = 0,
                                                    BalanceAmt = 0,
                                                    PaymentMode = "Online",
                                                    Remark = "Late Fee Update by Requery",
                                                    FeePaids = feeIdandAmount.Amounts,
                                                    FeeReceiptsOneTimeCreator = unicNumber.ToString(),
                                                    CurrentYear = studentdetails.CurrentYear,
                                                    BatchName = studentdetails.Batch_Id.ToString(),
                                                    DueAmount = 0.ToString(),
                                                    PaidAmount = objPayverify.payInstrument[0].payDetails.amount.ToString(),
                                                    FeeHeadingIDs = feeIdandAmount.FeeIds,
                                                    Jan = false,
                                                    Feb = false,
                                                    Mar = false,
                                                    Apr = false,
                                                    May = false,
                                                    Jun = false,
                                                    Jul = false,
                                                    Aug = false,
                                                    Sep = false,
                                                    Oct = false,
                                                    Nov = false,
                                                    Dec = false,
                                                    Type = 222.ToString()
                                                };

                                                _context.TblFeeReceipts.Add(fee);
                                                _context.SaveChanges();

                                                // Update Transaction
                                                txn.TransactionStatus = response2.message;
                                                txn.TransactionError = null;
                                            }
                                            else
                                            {
                                                txn.TransactionError = response2.description;
                                            }

                                            _context.Entry(txn).State = EntityState.Modified;
                                            _context.SaveChanges();
                                        }

                                        dbTransaction.Commit();
                                    }
                                    catch (Exception)
                                    {
                                        dbTransaction.Rollback();
                                        throw;
                                    }
                                }
                            }
                            else if(objPayverify.payInstrument[0].responseDetails != null)
                            {
                                var txnss = _context.tbl_PaymentTransactionDetails.FirstOrDefault(x => x.PaymentId == txn.PaymentId.ToString());
                                if (txnss != null)
                                {
                                    var responsede = objPayverify.payInstrument[0].responseDetails;
                                    txn.TransactionStatus = responsede.message;
                                    txn.TransactionError = responsede.description;
                                    _context.Entry(txn).State = EntityState.Modified;
                                    _context.SaveChanges();
                                }
                            }

                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }

                return View(); // Will show decrypted data in view
            
        }
        public static (string FeeIds, string Amounts) SplitFeeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (string.Empty, string.Empty);

            var pairs = input.Split(',');
            List<string> feeIds = new List<string>();
            List<string> amounts = new List<string>();

            foreach (var pair in pairs)
            {
                var parts = pair.Split('~');
                if (parts.Length == 2)
                {
                    feeIds.Add(parts[0]);
                    amounts.Add(parts[1]);
                }
            }

            return (string.Join(",", feeIds), string.Join(",", amounts));
        }

        public static string byteToHexString(byte[] byData)
        {
            StringBuilder sb = new StringBuilder((byData.Length * 2));
            for (int i = 0; (i < byData.Length); i++)
            {
                int v = (byData[i] & 255);
                if ((v < 16))
                {
                    sb.Append('0');
                }

                sb.Append(v.ToString("X"));

            }

            return sb.ToString();
        }
        public class Root
        {
            public PayInstrument payInstrument { get; set; }
        }

        public class HeadDetails
        {
            public string api { get; set; }
            public string source { get; set; }
        }

       

        public class PayDetails
        {
            public string atomTxnId { get; set; }
            public double amount { get; set; }
            public string txnCurrency { get; set; }
            public string signature { get; set; }
            // Read-only formatted string
            //public string formattedAmount => amount.ToString("F2", System.Globalization.CultureInfo.InvariantCulture);
        }

        public class PayInstrument
        {
            public HeadDetails headDetails { get; set; }
            public MerchDetails merchDetails { get; set; }
            public PayDetails payDetails { get; set; }
        }
        public class RootResponse
        {
            public List<PayInstrument> payInstrument { get; set; }
        }

       




    }

}