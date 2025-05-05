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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Collections;
using System.Net.Mime;
using System.Globalization;
using System.Collections.Specialized;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;

namespace SchoolManagement.Website.Controllers
{
    public class SMSandEmailController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        private ApplicationDbContext _contextstudent = new ApplicationDbContext();
        private IRepository<SMSEMAILSCHEDULE> _SMSEMAILSCHEDULE = null;
        private IRepository<SMSEMAILSENDHISTORY> _SMSEMAILSENDHISTORY = null;
        private IRepository<SMSEMAILTEMPLETE> _SMSEMAILTEMPLETE = null;
        public SMSandEmailController()
        {
            _SMSEMAILSCHEDULE = new Repository<SMSEMAILSCHEDULE>();
            _SMSEMAILSENDHISTORY = new Repository<SMSEMAILSENDHISTORY>();
            _SMSEMAILTEMPLETE = new Repository<SMSEMAILTEMPLETE>();
        }
        // GET: SMSandEmail
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SMSandEmailNotification()
        {
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

            ViewBag.AllBatchs = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "batch").DataListId.ToString()).ToList();

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            //var student = _context.Students.OrderBy(x => x.BatchName);
            var studentregistrations = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Class_Id != 0).ToList();

            List<studentlist> studentlist = new List<studentlist>();
            foreach (var res in studentregistrations)
            {
                studentlist studentobj = new studentlist();
                studentobj.ClassName = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.StudentName = res.Name+" "+res.Last_Name;
                studentobj.StudentId = Convert.ToInt32(res.StudentRegisterID);
                studentlist.Add(studentobj);
            }
            ViewBag.studentlist = studentlist;
            //ArrayList addarray = new ArrayList();
            //List<studentlist> studentlistbatch = new List<studentlist>();
            //foreach (var res in student)
            //{
            //    if (res.BatchName != null && res.BatchName != "" && !addarray.Contains(res.Class + "-" + res.BatchName.ToString()))
            //    {
            //        studentlist studentobj = new studentlist();
            //        studentobj.BatchName = res.Class + "-" + res.BatchName.ToString();
            //        addarray.Add(res.Class + "-" + res.BatchName.ToString());
            //        studentlistbatch.Add(studentobj);
            //    }
            //}
            //ViewBag.studentbatchlist = studentlistbatch;
            List<Userlist> Userlistobj = new List<Userlist>();
            UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            var userlist = UserManager.Users.OrderBy(x => x.UserName);
            foreach (var res in userlist)
            {
                Userlist Userlistobjlist = new Userlist();
                Userlistobjlist.UserName = res.UserName;
                Userlistobjlist.Email = res.Email;
                Userlistobjlist.PhoneNumber = res.PhoneNumber;
                Userlistobj.Add(Userlistobjlist);
            }
            ViewBag.userlist = Userlistobj;

            List<SMSEMAILTEMPLETESUMMARY> SMSEMAILTEMPLETESUMMARYS = new List<SMSEMAILTEMPLETESUMMARY>();
            var result = _context.SMSEMAILTEMPLETE.OrderByDescending(x => x.CREATEDDATE);
            foreach (var res in result)
            {
                SMSEMAILTEMPLETESUMMARY SMSEMAILOBJ = new SMSEMAILTEMPLETESUMMARY();
                string Type = string.Empty;
                if (res.NOTIFICATIONTYPE == "S")
                {
                    Type = "SMS";
                }
                else if (res.NOTIFICATIONTYPE == "E")
                {
                    Type = "Email";
                }
                else if (res.NOTIFICATIONTYPE == "B")
                {
                    Type = "Both";
                }
                SMSEMAILOBJ.NOTIFICATIONTYPE = Type.ToString();
                SMSEMAILOBJ.SUBJECT = res.SUBJECT;
                SMSEMAILOBJ.TempleteID = res.SMSEMAILID;
                SMSEMAILTEMPLETESUMMARYS.Add(SMSEMAILOBJ);
            }
            ViewBag.SMSEMAILTEM = SMSEMAILTEMPLETESUMMARYS;
            return View();
        }

        public ActionResult SMSandEmailTemplete()
        {
            List<SMSEMAILTEMPLETESUMMARY> SMSEMAILTEMPLETESUMMARYS = new List<SMSEMAILTEMPLETESUMMARY>();
            var result = _context.SMSEMAILTEMPLETE.OrderByDescending(x => x.CREATEDDATE);
            foreach (var res in result)
            {
                SMSEMAILTEMPLETESUMMARY SMSEMAILOBJ = new SMSEMAILTEMPLETESUMMARY();
                string Type = string.Empty;
                if (res.NOTIFICATIONTYPE == "S")
                {
                    Type = "SMS";
                }
                else if (res.NOTIFICATIONTYPE == "E")
                {
                    Type = "Email";
                }
                else if (res.NOTIFICATIONTYPE == "B")
                {
                    Type = "Both";
                }
                SMSEMAILOBJ.NOTIFICATIONTYPE = Type.ToString();
                SMSEMAILOBJ.SUBJECT = res.SUBJECT;
                SMSEMAILTEMPLETESUMMARYS.Add(SMSEMAILOBJ);
            }
            ViewBag.SMSEMAILTEM = SMSEMAILTEMPLETESUMMARYS;
            return View();
        }
        public ActionResult AddSMSandEmailTemplete()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AddSMSandEmailTemplete(AddSMSEmailTemplete AddSMSEmailTemplete)
        {
            SMSEMAILTEMPLETE SMSEMAILTEMPLETEobj = new SMSEMAILTEMPLETE
            {
                SUBJECT = AddSMSEmailTemplete.SMSSubject,
                SMS = AddSMSEmailTemplete.SMSText,
                EMAIL = AddSMSEmailTemplete.Eamilmessage,
                NOTIFICATIONTYPE = AddSMSEmailTemplete.SMSEmailtype,
                CREATEDDATE = DateTime.Now.ToString("dd/MM/yyyy")
            };
            _SMSEMAILTEMPLETE.Insert(SMSEMAILTEMPLETEobj);
            _SMSEMAILTEMPLETE.Save();
            return Json("ok");
        }

        [HttpPost]
        public JsonResult CheckTempleteData(SMSEMAILTEMPLETESUMMARY SMSEMAILTEMPLETESUMMARYobj)
        {
            var ResutltTemplete = _context.SMSEMAILTEMPLETE.Where(x => x.SMSEMAILID == SMSEMAILTEMPLETESUMMARYobj.TempleteID);
            AddSMSEmailTempleteview AddSMSEmailTempleteviewobj = new AddSMSEmailTempleteview();
            foreach (var res in ResutltTemplete)
            {
                AddSMSEmailTempleteviewobj.NOTIFICATIONTYPE = res.NOTIFICATIONTYPE;
                AddSMSEmailTempleteviewobj.SMS = res.SMS;
                AddSMSEmailTempleteviewobj.EMAIL = res.EMAIL;
                AddSMSEmailTempleteviewobj.SUBJECT = res.SUBJECT;
            }
            return Json(AddSMSEmailTempleteviewobj, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetClassById(int id)
        {
            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            var data = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult AddSmsTemplate(SMSEMAILTEMPLETE sMSEMAILTEMPLETEs, HttpPostedFileBase ATTACHEDFILE)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            if (ATTACHEDFILE != null)
            {
                if (ATTACHEDFILE.ContentLength > 0)
                {
                    var filename = Path.GetFileName(ATTACHEDFILE.FileName);
                    var path = Path.Combine(Server.MapPath("/WebsiteImages/SMSFile"), filename);
                    ATTACHEDFILE.SaveAs(path);
                    sMSEMAILTEMPLETEs.ATTACHEDFILE = filename;
                }
            }

            sMSEMAILTEMPLETEs.CREATEDDATE = DateTime.Now.ToString("dd/MM/yyyy");
            _context.SMSEMAILTEMPLETE.Add(sMSEMAILTEMPLETEs);
            _context.SaveChanges();

            return Content("<script language='javascript' type = 'text/javascript'>alert('Templates Added Successfully');location.replace('" + url + "')</script>");


        }
        public bool SendAbsentStudentEmail(int studentId)
        {
            try
            {
                // Fetch the family phone number based on StudentId
                var absentDetails = (from s in _context.Students
                                   join f in _context.FamilyDetails on s.ApplicationNumber equals f.ApplicationNumber
                                     where s.StudentId == studentId
                                   select new { f.FMobile, s.Name }).FirstOrDefault();

                StudentSendSMS($"{absentDetails.Name} is absent", absentDetails.FMobile);
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (use a logging library like Serilog or NLog)
                return false;
            }
        }

        public JsonResult SendAbsentStudentsBulk(List<int> studentIds)
        {
            try
            {
                if (studentIds == null || studentIds.Count == 0)
                {
                    return Json(new { success = false, message = "No student IDs provided." });
                }

                var absentDetailsList = (from s in _context.Students
                                         join f in _context.FamilyDetails on s.ApplicationNumber equals f.ApplicationNumber
                                         where studentIds.Contains(s.StudentId)
                                         select new { f.FMobile, s.Name }).ToList();

                foreach (var absentDetails in absentDetailsList)
                {
                    // Call the SMS sending function for each student
                    StudentSendSMS($"{absentDetails.Name} is absent", absentDetails.FMobile);
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                return Json(new { success = false, message = ex.Message });
            }
        }


        public JsonResult StudentSendSMS(string text, string mobileNumber)
        {
            return Json(SendSMS(text, mobileNumber));
        }

        public bool SendSMS(string text, string mobileNumber)
        {
            try
            {
                var apikey = "opFQEyWudbc-zd0WtMLwlIH3oTUm4wfiWuGCN4v63S"; //textlocal sms  key
                                                                           // var apikey = "opFQEyWudbc-5DaQSzuIQh6eXASBcouUSAc8IJPx2Z";
                String message = HttpUtility.UrlEncode(text);
                message = HttpUtility.UrlEncode(text).Replace("+", "%20");

                using (var wb = new WebClient())
                {
                    byte[] response = wb.UploadValues("https://api.textlocal.in/send/", new NameValueCollection()
                         {
                         {"apikey" , apikey},
                         {"numbers" , mobileNumber},
                         {"message" , message},
                         {"sender" , "TXTLCL"}
                         });
                    string result = System.Text.Encoding.UTF8.GetString(response);
                }

                // var fff = System.Web.HttpUtility.UrlEncode(text, System.Text.Encoding.GetEncoding("ISO-8859-1"));

                return true;
            }
            catch (Exception ex)
            {
            }
            return false;
        }
        public JsonResult SendBulkEmail(SmsEailViewModel smsemailviewmodel)
        {
            string xr = "";
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            
            if(Convert.ToString(Session["EmailUploadedPath"]) != "")
            {

            smsemailviewmodel.AttachedFile = Convert.ToString(Session["EmailUploadedPath"]);
            }
            else
            {
                smsemailviewmodel.AttachedFile = "";
            }
            var classid = Classes.FirstOrDefault(x => x.DataListItemId == smsemailviewmodel.Classid)?.DataListItemId;
           
            if(smsemailviewmodel.Classid != 0)
            {
                //var data = _context.Students.Where(x => x.Class_Id == classid).ToList();
                var data = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Class_Id == classid).ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    //xr = SendEmail("" + data[i].Parents_Email + "", "" + smsemailviewmodel.EmailSubject + "", "" + smsemailviewmodel.EmailBody + "", smsemailviewmodel.AttachedFile);

                    ////Email Archieve
                    //EmailViewModel emailViewModel = new EmailViewModel();
                    //emailViewModel.Student_id = Convert.ToInt32(data[i].StudentRegisterID);
                    //emailViewModel.ApplicationNumber = data[i].ApplicationNumber;
                    //emailViewModel.Name = data[i].Name + " " + data[i].Last_Name;
                    //emailViewModel.Parent_Email = data[i].Parents_Email;
                    //emailViewModel.Email_Date = DateTime.Now.ToString();
                    //emailViewModel.Email_Content = smsemailviewmodel.EmailSubject;

                    //var emailarchieve = AddEmailArchieve(emailViewModel);


                }
            }
            else
            {
                for(int j =0; j< smsemailviewmodel.Class_Name.Count(); j++)
                {
                    var classid1 = Classes.FirstOrDefault(x => x.DataListItemName == smsemailviewmodel.Class_Name[j])?.DataListItemId;
                    int studentregsterid = Convert.ToInt32(smsemailviewmodel.StudentId[j]);
                    var firstname = smsemailviewmodel.First_name[j];
                    var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.Class_Id == classid1 && x.StudentRegisterID == studentregsterid);
                    if (studentdata == null)
                    {
                        var studentApplicationId = _context.Students.FirstOrDefault(x => x.StudentId == studentregsterid).ApplicationNumber;
                        studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.Class_Id == classid1 && x.ApplicationNumber == studentApplicationId);
                    }
                    //var studentdata = _context.Students.FirstOrDefault(x => x.Class_Id == classid1 && x.Name == firstname);
                    xr = SendEmail("" + studentdata.Parents_Email + "", "" + smsemailviewmodel.EmailSubject + "", "" + smsemailviewmodel.EmailBody + "", smsemailviewmodel.AttachedFile);

                    EmailViewModel emailViewModel = new EmailViewModel();
                    emailViewModel.Student_id = Convert.ToInt32(studentdata.StudentRegisterID);
                    emailViewModel.ApplicationNumber = studentdata.ApplicationNumber;
                    emailViewModel.Name = studentdata.Name + " " + studentdata.Last_Name;
                    emailViewModel.Parent_Email = studentdata.Parents_Email;
                    emailViewModel.Email_Date = DateTime.Now.ToString();
                    emailViewModel.Email_Content = smsemailviewmodel.EmailSubject;

                    var emailarchieve = AddEmailArchieve(emailViewModel);


                }
            }
           
            if(xr=="S")
            return Json("Success");
            else
                return Json("Failed");

        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase file)
        {
            //var url = Request.UrlReferrer.AbsoluteUri;
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/EmailDocUpload"), fileName);
                //var path = Path.Combine(Server.MapPath("~/WebsiteImages/StudentPhoto"), fileName);
                file.SaveAs(path);
                Session["EmailUploadedPath"] = path;
                TempData["EmailUploadedFilename"]= fileName; 
                TempData.Keep("EmailUploadedFilename");
                // return Content("<script language='javascript' type='text/javascript'>location.replace('SMSandEmailNotification');alert('Document Updated Successully')</script>");
            }
            else
            {
                //return Content("<script language='javascript' type='text/javascript'> location.replace('SMSandEmailNotification');alert('Issue in Uploading the Document, Please re-try!') </script>");
            }

            return RedirectToAction("SMSandEmailNotification");
        }

        public string SendEmail(string Tomailid, string subject, string bodymessage/*, HttpPostedFileBase attchment*/,string attchment)
        {
            try
            {
                //string Frommailid = ConfigurationManager.AppSettings["ToMailid"];
                //string smtps = ConfigurationManager.AppSettings["smtps"];
                //int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);
                //string MailPassword = ConfigurationManager.AppSettings["MailPassword"];

                //MailMessage msg = new MailMessage();
                //msg.From = new MailAddress(Frommailid, subject);
                //// msg.To.Add(new MailAddress(tomailid));
                //string[] ToMuliId = Tomailid.Split(',');
                //foreach (string ToEMailId in ToMuliId)
                //{
                //    msg.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
                //}
                //msg.Subject = subject;
                //msg.IsBodyHtml = true;
                //msg.Priority = MailPriority.High;
                //string body = "<html><body><table border=0 cellSpacing=2 cellPadding=1 width=100%> ";
                //body = body + "" + "<tr><td ColSpan=1 RowSpan=1>" + bodymessage + "</td></tr>";
                //body = body + "" + "</table></body></html>";

                //msg.Body = body;

                //msg.BodyEncoding = System.Text.Encoding.UTF8;
                //SmtpClient smtp = new SmtpClient(smtps, port);
                //smtp.Send(msg);
                /* Beginning of Attachment1 process   & 
   Check the first open file dialog for a attachment */

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
                message.IsBodyHtml=true;
                if (attchment != null && attchment != "")
                {
                    //bool exists = System.IO.Directory.Exists(Server.MapPath(attchment));
                    //if (!exists)
                    //{
                    //var srPth = Server.MapPath(attchment);
                    //var xpath = srPth.Replace(attchment, "");
                    //DirectoryInfo di = Directory.CreateDirectory(xpath);

                    Attachment attachment = new Attachment(attchment, MediaTypeNames.Application.Octet);
                    ContentDisposition disposition = attachment.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(attchment);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(attchment);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(attchment);
                    disposition.FileName = Path.GetFileName(attchment);
                    disposition.Size = new FileInfo(attchment).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    message.Attachments.Add(new Attachment(attchment));

                    //di.Delete();
                    //}
                }
                Session["EmailUploadedPath"] = "";
                using (message)
                {
                    smtp.Send(message);
                }
                






                
            }
            catch (SmtpFailedRecipientsException sfrEx)
            {
                // TODO: Handle exception
                // When email could not be delivered to all receipients.
                //var x = sfrEx.Message;
                return sfrEx.InnerException.Message;
            }
            catch (SmtpException sEx)
            {
                // TODO: Handle exception
                // When SMTP Client cannot complete Send operation.
                string errormsg = "";
                if (sEx.InnerException == null)
                {
                    errormsg = sEx.Message;
                }
                else
                {
                    errormsg = sEx.InnerException.Message;
                }
                //return sEx.InnerException.Message;
                return errormsg;
            }
            catch (Exception ex)
            {
                string errormsg = "";
                if (ex.InnerException == null)
                {
                    errormsg = ex.Message;
                }
                else
                {
                    errormsg = ex.InnerException.Message;
                }
                //return sEx.InnerException.Message;
                return errormsg;
               // return ex.InnerException.Message;
            }
            return "S";
            
        }



        public ActionResult Sendusernamepassword()
        {

            var data = _context.StudentsRegistrations.ToList();
            // var data = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
            var data1 = (from sr in _context.StudentsRegistrations.Where(x=>x.IsApprove!=192)
                         // Uncomment below if you want to apply IsApprove filter
                         // && sr.IsApprove != 192
                         join s in _context.Students on sr.ApplicationNumber equals s.ApplicationNumber into sGroup
                         from s in sGroup.DefaultIfEmpty()
                         join dl in _context.DataListItems on s.Class_Id equals dl.DataListItemId into dlGroup
                         from dl in dlGroup.DefaultIfEmpty()
                         join us in _context.Tbl_UserManagement on sr.UserId equals us.UserId.ToString() into usGroup
                         from us in usGroup.DefaultIfEmpty()
                         select new UserNamePassword
                         {
                             Studentid=sr.StudentRegisterID,
                             FirstName = s.Name,
                             LastName = s.Last_Name,
                             Class = dl != null ? dl.DataListItemName : null,
                             Userid = us != null ? us.UserId.ToString() : null,
                             Username = us != null ? us.UserName : null,
                             Password = us != null ? us.Password : null,
                             Email = us != null ? us.Email : null
                         }).ToList();

            //var data1 = _context.StudentsRegistrations.Where(x => x.StudentRegisterID >= 1775 && x.StudentRegisterID <= 2087).ToList();

            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.Batch = _context.Tbl_Batches.ToList();
            List<UserNamePassword> UserNamePassword = new List<UserNamePassword>();
            //foreach(var item in data)
            //{
            //    if(item.UserId != null)
            //    {
                  
            //        int userid;
            //        if (!int.TryParse(item.UserId?.ToString(), out userid))
            //        {
                        
            //        }
            //        var userlist = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);

            //        if (userlist != null)
            //        {
            //            UserNamePassword.Add(new UserNamePassword
            //            {
            //                Userid = item.UserId,
            //                Username = userlist.UserName,
            //                Password = userlist.Password,
            //                Email = userlist.Email,
            //                FirstName = item.Name,
            //                LastName = item.Last_Name,
            //                Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName
            //            });
            //        }


            //    }


            //}
            ViewBag.Userlist = data1;
            return View();
        }


        public JsonResult GetClasslistById(int id)
        {
            var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            var data = _context.StudentsRegistrations.Where(x => x.IsApprove != 192 && x.Class_Id == id).ToList();
            List<studentlist> studentlist = new List<studentlist>();
            foreach (var res in data)
            {
                studentlist studentobj = new studentlist();
                studentobj.ClassName = Classes.FirstOrDefault(x => x.DataListItemId == res.Class_Id)?.DataListItemName;
                studentobj.StudentName = res.Name + " " + res.Last_Name;
                studentobj.StudentId = Convert.ToInt32(res.StudentRegisterID);
                studentlist.Add(studentobj);
            }
            //var data = _context.Students.Where(x => x.Class_Id == id).ToList();
            return Json(studentlist, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SendEmailTostudent(UsernamePasswordViewmodel usernamepasswordviewmodel)
        {
            string str = "";
            string year = "";
            //var subject = "Regarding Username and Password";
            //var link = "http://www.carmelteresaschool.in";
            //var emailmessage1 = "Your Username is " + username + " and Password is " + password + ", You can Login the portal using this username and password and the link is "+link+"";

            //var data = "";
            //SendEmail("" + parentemail + "", "" + subject + "", "" + emailmessage1 + "");
            //var dbContextTransaction = _context.Database.BeginTransaction();
           
            //string year = Convert.ToDateTime(usernamepasswordviewmodel.Date).Year.ToString();
            var subject = "Regarding Username and Password";
            //var link = "https://www.carmelteresaschool.in/";
            var websitename = _context.TblCreateSchool.Select(a=> a.Website).FirstOrDefault();
            // var link = "http://nsmslumen.nirmalaconventsrsecschool.com/";
            if(websitename == null)
            {
                throw new Exception("Please Enter School website");
            }
            var link = websitename;

            if (usernamepasswordviewmodel.Username == null)
            {
                return Json("F");
            }
            else
            {
                if (usernamepasswordviewmodel.Date != null)
                {
                    DateTime date = DateTime.ParseExact(usernamepasswordviewmodel.Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    year = Convert.ToDateTime(date).Year.ToString();
                }


                if (usernamepasswordviewmodel.Classid == 0 && usernamepasswordviewmodel.Date == null)
                {
                    
                    if (usernamepasswordviewmodel.Username != null)
                    {
                        int count = usernamepasswordviewmodel.Username.Count();
                        for (int i = 0; i < count; i++)
                        {
                            //var applicationnumber = usernamepasswordviewmodel.Username[i];
                            var userid = usernamepasswordviewmodel.Userid[i];
                            //var sendate = DateTime.Now.ToString("dd/MM/yyyy");
                            //var data = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == applicationnumber);
                            var data = _context.StudentsRegistrations.FirstOrDefault(x => x.UserId == userid);
                            StudentsRegistration studentsRegistration = new StudentsRegistration();
                            if (data != null)
                            {
                                #region Updatestatus
                                studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                                studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                                studentsRegistration.UIN = data.UIN;
                                studentsRegistration.Date = Convert.ToString(DateTime.Now);
                                studentsRegistration.Name = data.Name;
                                studentsRegistration.Class = data.Class;
                                studentsRegistration.Section = data.Section;
                                studentsRegistration.Gender = data.Gender;
                                studentsRegistration.RTE = data.RTE;
                                studentsRegistration.Medium = data.Medium;
                                studentsRegistration.Caste = data.Caste;
                                studentsRegistration.AgeInWords = data.AgeInWords;
                                studentsRegistration.DOB = data.DOB;
                                studentsRegistration.POB = data.POB;
                                studentsRegistration.Nationality = data.Nationality;
                                studentsRegistration.Religion = data.Religion;
                                studentsRegistration.MotherTongue = data.MotherTongue;
                                studentsRegistration.Category = data.Category;
                                studentsRegistration.BloodGroup = data.BloodGroup;
                                studentsRegistration.Hobbies = data.Hobbies;
                                studentsRegistration.Sports = data.Sports;
                                studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                                studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                                studentsRegistration.AdharNo = data.AdharNo;
                                studentsRegistration.AdharFile = data.AdharFile;
                                studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                                studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                                studentsRegistration.IsApprove = data.IsApprove;
                                studentsRegistration.IsActive = data.IsActive;
                                studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                                studentsRegistration.Email = data.Email;
                                studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                                studentsRegistration.Parents_Email = data.Parents_Email;
                                studentsRegistration.Class_Id = data.Class_Id;
                                studentsRegistration.Class_Name = data.Class_Name;
                                studentsRegistration.Last_Name = data.Last_Name;
                                studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                                studentsRegistration.Religion_Id = data.Religion_Id;
                                studentsRegistration.Cast_Id = data.Cast_Id;
                                studentsRegistration.Category_Id = data.Category_Id;
                                studentsRegistration.Transport = data.Transport;
                                studentsRegistration.Mobile = data.Mobile;
                                studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                                studentsRegistration.City = data.City;
                                studentsRegistration.State = data.State;
                                studentsRegistration.Pincode = data.Pincode;
                                studentsRegistration.AddedYear = data.AddedYear;
                                studentsRegistration.Registration_Date = data.Registration_Date;
                                studentsRegistration.IsEmailsent = false;
                                studentsRegistration.UserId = data.UserId;
                                studentsRegistration.Email_SendDate = data.Email_SendDate;
                                studentsRegistration.Email_send = data.Email_send;

                                #endregion
                                _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                                _context.SaveChanges();

                                if (data.IsEmailsent == false)
                                {
                                    var emailmessage1 = "Your Username is " + usernamepasswordviewmodel.Username[i] + " and Password is " + usernamepasswordviewmodel.Password[i] + ", You can Login the portal using this username and password and the link is " + link + "";

                                    str = SendEmail("" + usernamepasswordviewmodel.Parentemail[i] + "", "" + subject + "", "" + emailmessage1 + "", string.Empty);
                                    if (str == "S")
                                    {
                                        int emailsend = data.Email_send;
                                        #region Updatestatus
                                        studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                                        studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                                        studentsRegistration.UIN = data.UIN;
                                        studentsRegistration.Date = Convert.ToString(DateTime.Now);
                                        studentsRegistration.Name = data.Name;
                                        studentsRegistration.Class = data.Class;
                                        studentsRegistration.Section = data.Section;
                                        studentsRegistration.Gender = data.Gender;
                                        studentsRegistration.RTE = data.RTE;
                                        studentsRegistration.Medium = data.Medium;
                                        studentsRegistration.Caste = data.Caste;
                                        studentsRegistration.AgeInWords = data.AgeInWords;
                                        studentsRegistration.DOB = data.DOB;
                                        studentsRegistration.POB = data.POB;
                                        studentsRegistration.Nationality = data.Nationality;
                                        studentsRegistration.Religion = data.Religion;
                                        studentsRegistration.MotherTongue = data.MotherTongue;
                                        studentsRegistration.Category = data.Category;
                                        studentsRegistration.BloodGroup = data.BloodGroup;
                                        studentsRegistration.Hobbies = data.Hobbies;
                                        studentsRegistration.Sports = data.Sports;
                                        studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                                        studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                                        studentsRegistration.AdharNo = data.AdharNo;
                                        studentsRegistration.AdharFile = data.AdharFile;
                                        studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                                        studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                                        studentsRegistration.IsApprove = data.IsApprove;
                                        studentsRegistration.IsActive = data.IsActive;
                                        studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                                        studentsRegistration.Email = data.Email;
                                        studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                                        studentsRegistration.Parents_Email = data.Parents_Email;
                                        studentsRegistration.Class_Id = data.Class_Id;
                                        studentsRegistration.Class_Name = data.Class_Name;
                                        studentsRegistration.Last_Name = data.Last_Name;
                                        studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                                        studentsRegistration.Religion_Id = data.Religion_Id;
                                        studentsRegistration.Cast_Id = data.Cast_Id;
                                        studentsRegistration.Category_Id = data.Category_Id;
                                        studentsRegistration.Transport = data.Transport;
                                        studentsRegistration.Mobile = data.Mobile;
                                        studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                                        studentsRegistration.City = data.City;
                                        studentsRegistration.State = data.State;
                                        studentsRegistration.Pincode = data.Pincode;
                                        studentsRegistration.AddedYear = data.AddedYear;
                                        studentsRegistration.Registration_Date = data.Registration_Date;
                                        studentsRegistration.IsEmailsent = true;
                                        studentsRegistration.UserId = data.UserId;
                                        studentsRegistration.Email_SendDate = Convert.ToString(DateTime.Now);
                                        studentsRegistration.Email_send = emailsend++;

                                        #endregion

                                        _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                                        _context.SaveChanges();

                                        EmailViewModel emailViewModel = new EmailViewModel();
                                        emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                        emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                        emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                                        emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                                        emailViewModel.Email_Date = DateTime.Now.ToString();
                                        emailViewModel.Email_Content = subject;

                                        var emailarchieve = AddEmailArchieve(emailViewModel);

                                    }

                                   
                                }

                            }


                        }
                    }

                }

                else if (usernamepasswordviewmodel.Classid != 0 && usernamepasswordviewmodel.Date != null && usernamepasswordviewmodel.CheckAll == "False")
                {
                    if (usernamepasswordviewmodel.Username != null)
                    {
                        int count = usernamepasswordviewmodel.Username.Count();
                        for (int i = 0; i < count; i++)
                        {
                            //var applicationnumber = usernamepasswordviewmodel.Username[i];
                            var userid = usernamepasswordviewmodel.Userid[i];

                            var data = _context.StudentsRegistrations.FirstOrDefault(x => x.UserId == userid);
                            StudentsRegistration studentsRegistration = new StudentsRegistration();
                            if (data != null)
                            {
                                #region Updatestatus
                                studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                                studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                                studentsRegistration.UIN = data.UIN;
                                studentsRegistration.Date = Convert.ToString(DateTime.Now);
                                studentsRegistration.Name = data.Name;
                                studentsRegistration.Class = data.Class;
                                studentsRegistration.Section = data.Section;
                                studentsRegistration.Gender = data.Gender;
                                studentsRegistration.RTE = data.RTE;
                                studentsRegistration.Medium = data.Medium;
                                studentsRegistration.Caste = data.Caste;
                                studentsRegistration.AgeInWords = data.AgeInWords;
                                studentsRegistration.DOB = data.DOB;
                                studentsRegistration.POB = data.POB;
                                studentsRegistration.Nationality = data.Nationality;
                                studentsRegistration.Religion = data.Religion;
                                studentsRegistration.MotherTongue = data.MotherTongue;
                                studentsRegistration.Category = data.Category;
                                studentsRegistration.BloodGroup = data.BloodGroup;
                                studentsRegistration.Hobbies = data.Hobbies;
                                studentsRegistration.Sports = data.Sports;
                                studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                                studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                                studentsRegistration.AdharNo = data.AdharNo;
                                studentsRegistration.AdharFile = data.AdharFile;
                                studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                                studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                                studentsRegistration.IsApprove = data.IsApprove;
                                studentsRegistration.IsActive = data.IsActive;
                                studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                                studentsRegistration.Email = data.Email;
                                studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                                studentsRegistration.Parents_Email = data.Parents_Email;
                                studentsRegistration.Class_Id = data.Class_Id;
                                studentsRegistration.Class_Name = data.Class_Name;
                                studentsRegistration.Last_Name = data.Last_Name;
                                studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                                studentsRegistration.Religion_Id = data.Religion_Id;
                                studentsRegistration.Cast_Id = data.Cast_Id;
                                studentsRegistration.Category_Id = data.Category_Id;
                                studentsRegistration.Transport = data.Transport;
                                studentsRegistration.Mobile = data.Mobile;
                                studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                                studentsRegistration.City = data.City;
                                studentsRegistration.State = data.State;
                                studentsRegistration.Pincode = data.Pincode;
                                studentsRegistration.AddedYear = data.AddedYear;
                                studentsRegistration.Registration_Date = data.Registration_Date;
                                studentsRegistration.IsEmailsent = false;
                                studentsRegistration.UserId = data.UserId;
                                studentsRegistration.Email_send = data.Email_send;
                                studentsRegistration.Email_SendDate = data.Email_SendDate;

                                #endregion
                                _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                                _context.SaveChanges();

                                if (data.IsEmailsent == false)
                                {
                                    var emailmessage1 = "Your Username is " + usernamepasswordviewmodel.Username[i] + " and Password is " + usernamepasswordviewmodel.Password[i] + ", You can Login the portal using this username and password and the link is " + link + "";

                                    str = SendEmail("" + usernamepasswordviewmodel.Parentemail[i] + "", "" + subject + "", "" + emailmessage1 + "", string.Empty);
                                    if (str == "S")
                                    {
                                        #region Updatestatus
                                        studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                                        studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                                        studentsRegistration.UIN = data.UIN;
                                        studentsRegistration.Date = Convert.ToString(DateTime.Now);
                                        studentsRegistration.Name = data.Name;
                                        studentsRegistration.Class = data.Class;
                                        studentsRegistration.Section = data.Section;
                                        studentsRegistration.Gender = data.Gender;
                                        studentsRegistration.RTE = data.RTE;
                                        studentsRegistration.Medium = data.Medium;
                                        studentsRegistration.Caste = data.Caste;
                                        studentsRegistration.AgeInWords = data.AgeInWords;
                                        studentsRegistration.DOB = data.DOB;
                                        studentsRegistration.POB = data.POB;
                                        studentsRegistration.Nationality = data.Nationality;
                                        studentsRegistration.Religion = data.Religion;
                                        studentsRegistration.MotherTongue = data.MotherTongue;
                                        studentsRegistration.Category = data.Category;
                                        studentsRegistration.BloodGroup = data.BloodGroup;
                                        studentsRegistration.Hobbies = data.Hobbies;
                                        studentsRegistration.Sports = data.Sports;
                                        studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                                        studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                                        studentsRegistration.AdharNo = data.AdharNo;
                                        studentsRegistration.AdharFile = data.AdharFile;
                                        studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                                        studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                                        studentsRegistration.IsApprove = data.IsApprove;
                                        studentsRegistration.IsActive = data.IsActive;
                                        studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                                        studentsRegistration.Email = data.Email;
                                        studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                                        studentsRegistration.Parents_Email = data.Parents_Email;
                                        studentsRegistration.Class_Id = data.Class_Id;
                                        studentsRegistration.Class_Name = data.Class_Name;
                                        studentsRegistration.Last_Name = data.Last_Name;
                                        studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                                        studentsRegistration.Religion_Id = data.Religion_Id;
                                        studentsRegistration.Cast_Id = data.Cast_Id;
                                        studentsRegistration.Category_Id = data.Category_Id;
                                        studentsRegistration.Transport = data.Transport;
                                        studentsRegistration.Mobile = data.Mobile;
                                        studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                                        studentsRegistration.City = data.City;
                                        studentsRegistration.State = data.State;
                                        studentsRegistration.Pincode = data.Pincode;
                                        studentsRegistration.AddedYear = data.AddedYear;
                                        studentsRegistration.Registration_Date = data.Registration_Date;
                                        studentsRegistration.IsEmailsent = true;
                                        studentsRegistration.UserId = data.UserId;
                                        studentsRegistration.Email_send = data.Email_send + 1;
                                        studentsRegistration.Email_SendDate = Convert.ToString(DateTime.Now);

                                        #endregion
                                        _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                                        _context.SaveChanges();

                                        EmailViewModel emailViewModel = new EmailViewModel();
                                        emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                        emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                        emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                                        emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                                        emailViewModel.Email_Date = DateTime.Now.ToString();
                                        emailViewModel.Email_Content = subject;

                                        var emailarchieve = AddEmailArchieve(emailViewModel);

                                    }
                                    
                                }

                            }


                        }
                    }

                }

                else
                {
                    var studentlist = _context.StudentsRegistrations.Where(x => x.Class_Id == usernamepasswordviewmodel.Classid && x.IsApprove != 192).ToList();
                    StudentsRegistration studentsRegistration = new StudentsRegistration();

                    foreach (var item in studentlist)
                    {
                        if (item.UserId != null)
                        {
                            var data = _context.StudentsRegistrations.FirstOrDefault(x => x.UserId == item.UserId);
                            #region Updatestatus
                            studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                            studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                            studentsRegistration.UIN = data.UIN;
                            studentsRegistration.Date = Convert.ToString(DateTime.Now);
                            studentsRegistration.Name = data.Name;
                            studentsRegistration.Class = data.Class;
                            studentsRegistration.Section = data.Section;
                            studentsRegistration.Gender = data.Gender;
                            studentsRegistration.RTE = data.RTE;
                            studentsRegistration.Medium = data.Medium;
                            studentsRegistration.Caste = data.Caste;
                            studentsRegistration.AgeInWords = data.AgeInWords;
                            studentsRegistration.DOB = data.DOB;
                            studentsRegistration.POB = data.POB;
                            studentsRegistration.Nationality = data.Nationality;
                            studentsRegistration.Religion = data.Religion;
                            studentsRegistration.MotherTongue = data.MotherTongue;
                            studentsRegistration.Category = data.Category;
                            studentsRegistration.BloodGroup = data.BloodGroup;
                            studentsRegistration.Hobbies = data.Hobbies;
                            studentsRegistration.Sports = data.Sports;
                            studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                            studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                            studentsRegistration.AdharNo = data.AdharNo;
                            studentsRegistration.AdharFile = data.AdharFile;
                            studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                            studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                            studentsRegistration.IsApprove = data.IsApprove;
                            studentsRegistration.IsActive = data.IsActive;
                            studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                            studentsRegistration.Email = data.Email;
                            studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                            studentsRegistration.Parents_Email = data.Parents_Email;
                            studentsRegistration.Class_Id = data.Class_Id;
                            studentsRegistration.Class_Name = data.Class_Name;
                            studentsRegistration.Last_Name = data.Last_Name;
                            studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                            studentsRegistration.Religion_Id = data.Religion_Id;
                            studentsRegistration.Cast_Id = data.Cast_Id;
                            studentsRegistration.Category_Id = data.Category_Id;
                            studentsRegistration.Transport = data.Transport;
                            studentsRegistration.Mobile = data.Mobile;
                            studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                            studentsRegistration.City = data.City;
                            studentsRegistration.State = data.State;
                            studentsRegistration.Pincode = data.Pincode;
                            studentsRegistration.AddedYear = data.AddedYear;
                            studentsRegistration.Registration_Date = data.Registration_Date;
                            studentsRegistration.IsEmailsent = false;
                            studentsRegistration.UserId = data.UserId;
                            studentsRegistration.Email_send = data.Email_send;
                            studentsRegistration.Email_SendDate = data.Email_SendDate;

                            #endregion
                            _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                            _context.SaveChanges();
                            if (data.IsEmailsent == false)
                            {

                                int userid = Convert.ToInt32(item.UserId);
                                var userlist = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);
                                var emailmessage1 = "Your Username is " + userlist.UserName + " and Password is " + userlist.Password + ", You can Login the portal using this username and password and the link is " + link + "";
                                str = SendEmail("" + userlist.Email + "", "" + subject + "", "" + emailmessage1 + "", string.Empty);
                                if (str == "S")
                                {
                                    #region Updatestatus
                                    int emailsend = data.Email_send;
                                    studentsRegistration.StudentRegisterID = data.StudentRegisterID;
                                    studentsRegistration.ApplicationNumber = data.ApplicationNumber;
                                    studentsRegistration.UIN = data.UIN;
                                    studentsRegistration.Date = Convert.ToString(DateTime.Now);
                                    studentsRegistration.Name = data.Name;
                                    studentsRegistration.Class = data.Class;
                                    studentsRegistration.Section = data.Section;
                                    studentsRegistration.Gender = data.Gender;
                                    studentsRegistration.RTE = data.RTE;
                                    studentsRegistration.Medium = data.Medium;
                                    studentsRegistration.Caste = data.Caste;
                                    studentsRegistration.AgeInWords = data.AgeInWords;
                                    studentsRegistration.DOB = data.DOB;
                                    studentsRegistration.POB = data.POB;
                                    studentsRegistration.Nationality = data.Nationality;
                                    studentsRegistration.Religion = data.Religion;
                                    studentsRegistration.MotherTongue = data.MotherTongue;
                                    studentsRegistration.Category = data.Category;
                                    studentsRegistration.BloodGroup = data.BloodGroup;
                                    studentsRegistration.Hobbies = data.Hobbies;
                                    studentsRegistration.Sports = data.Sports;
                                    studentsRegistration.ProfileAvatar = data.ProfileAvatar;
                                    studentsRegistration.MarkForIdentity = data.MarkForIdentity;
                                    studentsRegistration.AdharNo = data.AdharNo;
                                    studentsRegistration.AdharFile = data.AdharFile;
                                    studentsRegistration.IsApplyforTC = data.IsApplyforTC;
                                    studentsRegistration.IsApplyforAdmission = data.IsApplyforAdmission;
                                    studentsRegistration.IsApprove = data.IsApprove;
                                    studentsRegistration.IsActive = data.IsActive;
                                    studentsRegistration.IsAdmissionPaid = data.IsAdmissionPaid;
                                    studentsRegistration.Email = data.Email;
                                    studentsRegistration.LastStudiedSchoolName = data.LastStudiedSchoolName;
                                    studentsRegistration.Parents_Email = data.Parents_Email;
                                    studentsRegistration.Class_Id = data.Class_Id;
                                    studentsRegistration.Class_Name = data.Class_Name;
                                    studentsRegistration.Last_Name = data.Last_Name;
                                    studentsRegistration.BloodGroup_Id = data.BloodGroup_Id;
                                    studentsRegistration.Religion_Id = data.Religion_Id;
                                    studentsRegistration.Cast_Id = data.Cast_Id;
                                    studentsRegistration.Category_Id = data.Category_Id;
                                    studentsRegistration.Transport = data.Transport;
                                    studentsRegistration.Mobile = data.Mobile;
                                    studentsRegistration.AdmissionFeePaid = data.AdmissionFeePaid;
                                    studentsRegistration.City = data.City;
                                    studentsRegistration.State = data.State;
                                    studentsRegistration.Pincode = data.Pincode;
                                    studentsRegistration.AddedYear = data.AddedYear;
                                    studentsRegistration.Registration_Date = data.Registration_Date;
                                    studentsRegistration.IsEmailsent = true;
                                    studentsRegistration.UserId = data.UserId;
                                    studentsRegistration.Email_send = emailsend++;
                                    studentsRegistration.Email_SendDate = Convert.ToString(DateTime.Now);

                                    #endregion
                                    _context.Entry(data).CurrentValues.SetValues(studentsRegistration);
                                    _context.SaveChanges();

                                    EmailViewModel emailViewModel = new EmailViewModel();
                                    emailViewModel.Student_id = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                    emailViewModel.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                    emailViewModel.Name = studentsRegistration.Name + " " + studentsRegistration.Last_Name;
                                    emailViewModel.Parent_Email = studentsRegistration.Parents_Email;
                                    emailViewModel.Email_Date = DateTime.Now.ToString();
                                    emailViewModel.Email_Content = subject;

                                    var emailarchieve = AddEmailArchieve(emailViewModel);


                                }
                               
                            }



                        }


                    }

                }

                return Json(str,JsonRequestBehavior.AllowGet);
            }
            //if (str == "S")
            //{
            //   // dbContextTransaction.Commit();
            //    return Json("Success");
            //}

            //else
            //{
            //    //dbContextTransaction.Rollback();
            //    //dbContextTransaction = null;
            //    return Json("Failed");
            //}
        }

        public string AddEmailArchieve(EmailViewModel emailViewModel)
        {
            try
            {
                TblEmailArchieve tblEmailArchieve = new TblEmailArchieve();
                tblEmailArchieve.Student_id = emailViewModel.Student_id;
                tblEmailArchieve.ApplicationNumber = emailViewModel.ApplicationNumber;
                tblEmailArchieve.Name = emailViewModel.Name;
                tblEmailArchieve.Email_Content = emailViewModel.Email_Content;
                tblEmailArchieve.Email_Date = emailViewModel.Email_Date;
                tblEmailArchieve.Parent_Email = emailViewModel.Parent_Email;
                _context.TblEmailArchieve.Add(tblEmailArchieve);
                _context.SaveChanges();
                return "s";
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //public JsonResult FilterClass(int id, int section, int BatchId)
        //{
        //    List<UserNamePassword> data = new List<UserNamePassword>();
        //    //var data =string.Empty;
        //    if (section != null && section != 0)
        //    {
        //        //data = _context.Students.Where(x => x.Class_Id == id && x.Section_Id == section && x.IsApprove != 192).ToList();
        //         data = (from sr in _context.StudentsRegistrations
        //                     join s in _context.Students.Where(x => x.IsApprove != 192 && x.Class_Id == id && x.Section_Id == section && x.Batch_Id == BatchId) on sr.ApplicationNumber equals s.ApplicationNumber into sGroup
        //                     from s in sGroup.DefaultIfEmpty()
        //                     join dl in _context.DataListItems on s.Class_Id equals dl.DataListItemId into dlGroup
        //                     from dl in dlGroup.DefaultIfEmpty()
        //                     join us in _context.Tbl_UserManagement on sr.UserId equals us.UserId.ToString() into usGroup
        //                     from us in usGroup.DefaultIfEmpty()
        //                     select new UserNamePassword
        //                     {
        //                         FirstName = sr.Name,
        //                         LastName = sr.Last_Name,
        //                         Class = dl != null ? dl.DataListItemName : null,
        //                         Userid = us != null ? us.UserId.ToString() : null,
        //                         Username = us != null ? us.UserName : null,
        //                         Password = us != null ? us.Password : null,
        //                         Email = us != null ? us.Email : null
        //                     }).ToList();
        //    }
        //    else
        //    {
        //        //data = _context.Students.Where(x => x.Class_Id == id && x.IsApprove != 192).ToList();
        //         data = (from sr in _context.StudentsRegistrations
        //                     join s in _context.Students.Where(x => x.IsApprove != 192 && x.Class_Id == id && x.Batch_Id == BatchId) on sr.ApplicationNumber equals s.ApplicationNumber into sGroup
        //                     from s in sGroup.DefaultIfEmpty()
        //                     join dl in _context.DataListItems on s.Class_Id equals dl.DataListItemId into dlGroup
        //                     from dl in dlGroup.DefaultIfEmpty()
        //                     join us in _context.Tbl_UserManagement on sr.UserId equals us.UserId.ToString() into usGroup
        //                     from us in usGroup.DefaultIfEmpty()
        //                     select new UserNamePassword
        //                     {
        //                         FirstName = sr.Name,
        //                         LastName = sr.Last_Name,
        //                         Class = dl != null ? dl.DataListItemName : null,
        //                         Userid = us != null ? us.UserId.ToString() : null,
        //                         Username = us != null ? us.UserName : null,
        //                         Password = us != null ? us.Password : null,
        //                         Email = us != null ? us.Email : null
        //                     }).ToList();
        //    }

        //    //var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
        //    //List<UserNamePassword> UserNamePassword = new List<UserNamePassword>();
        //    //foreach (var item in data)
        //    //{
        //    //    if (item.Userid != null)
        //    //    {
        //    //        int userid = Convert.ToInt32(item.Userid);
        //    //        var userlist = _context.Tbl_UserManagement.First(x => x.UserId == userid);

        //    //        UserNamePassword.Add(new UserNamePassword
        //    //        {
        //    //            Userid = item.Userid,
        //    //            Username = userlist.UserName,
        //    //            Password = userlist.Password,
        //    //            Email = userlist.Email,
        //    //            FirstName = item.FirstName,
        //    //            LastName = item.LastName,
        //    //            Class = Classes.FirstOrDefault(x => x.DataListItemId == item.Clas)?.DataListItemName
        //    //        });

        //    //    }


        //    //}

        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}
        public JsonResult FilterClass(int id, int section, int BatchId)
        {
            var studentQuery = _context.Students.AsQueryable();

            studentQuery = studentQuery.Where(x => x.IsApprove != 192 && x.Class_Id == id && x.Batch_Id == BatchId);

            if (section != 0)
                studentQuery = studentQuery.Where(x => x.Section_Id == section);

            var filteredStudents = studentQuery.ToList(); // Only filtered students fetched

            var applicationNumbers = filteredStudents.Select(s => s.ApplicationNumber).ToList();

            var registrations = _context.StudentsRegistrations
                .Where(sr => applicationNumbers.Contains(sr.ApplicationNumber))
                .ToList();

            var userIds = registrations.Select(sr => sr.UserId).ToList();

            var users = _context.Tbl_UserManagement
                .Where(us => userIds.Contains(us.UserId.ToString()))
                .ToList();

            var classIds = filteredStudents.Select(s => s.Class_Id).Distinct().ToList();

            var classList = _context.DataListItems
                .Where(dl => classIds.Contains(dl.DataListItemId))
                .ToList();

            var result = (from sr in registrations
                          join s in filteredStudents on sr.ApplicationNumber equals s.ApplicationNumber
                          join dl in classList on s.Class_Id equals dl.DataListItemId into dlGroup
                          from dl in dlGroup.DefaultIfEmpty()
                          join us in users on sr.UserId equals us.UserId.ToString() into usGroup
                          from us in usGroup.DefaultIfEmpty()
                          select new UserNamePassword
                          {
                              Studentid=sr.StudentRegisterID,
                              FirstName = s.Name,
                              LastName = s.Last_Name,
                              Class = dl?.DataListItemName,
                              Userid = us?.UserId.ToString(),
                              Username = us?.UserName,
                              Password = us?.Password,
                              Email = us?.Email
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChangePassword(int id ,string password)
        {
            var userlist = _context.Tbl_UserManagement.First(x => x.UserId == id);
            if (id != 0)
            {
                Tbl_UserManagement userManagement = new Tbl_UserManagement();
                userManagement.UserId = id;
                userManagement.Password=password;
                userManagement.UserName = userlist.UserName;
                userManagement.Email= userlist.Email;
                userManagement.Description = userlist.Description;
                var data = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == id);

                _context.Entry(data).CurrentValues.SetValues(userManagement);
                _context.SaveChanges();
                //_context.Tbl_UserManagement.Add(userManagement);
                //_context.SaveChanges();

            }
            return Json(new {success=true },JsonRequestBehavior.AllowGet);
        }


        public ActionResult EmailReport()
        {
            var data = _context.TblEmailArchieve.ToList();
            ViewBag.EmailReport = data;
            return View();
        }

        //Filterreport
        public JsonResult FilterEmailReport(string fromdate,string todate)
        {
            try
            {
                DateTime fdate = new DateTime();
                DateTime tdate = new DateTime();
               

                var data = _context.TblEmailArchieve.ToList();
                List<TblEmailArchieve> TblEmailArchieve = new List<TblEmailArchieve>();

                if(fromdate != null && fromdate != "" && todate != null && todate != "")
                {
                    fdate = Convert.ToDateTime(fromdate);
                    tdate = Convert.ToDateTime(todate);
                    DateTime emaildate;

                    TblEmailArchieve = data.Where(x => DateTime.TryParse(x.Email_Date, out emaildate) && emaildate >= fdate && emaildate <= tdate).ToList();

                }
                else if(fromdate != null && fromdate != "")
                {
                    fdate = Convert.ToDateTime(fromdate);
                    DateTime emaildate;
                    TblEmailArchieve = data.Where(x => DateTime.TryParse(x.Email_Date, out emaildate) && emaildate >= fdate).ToList();
                }
                else if(todate != null && todate != "")
                {
                    tdate = Convert.ToDateTime(todate);
                    DateTime emaildate;
                    TblEmailArchieve = data.Where(x => DateTime.TryParse(x.Email_Date, out emaildate) && emaildate <= tdate).ToList();
                }
                else
                {
                    TblEmailArchieve = data;
                }
                return Json(TblEmailArchieve, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }


    }
    public class UserNamePassword
    {
        public long Studentid { get; set; }

        public string Userid { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string Class { get; set; }

    }
}