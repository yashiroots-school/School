using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ClosedXML.Excel;
using DataAccess.ViewModels;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using static System.Data.Entity.Infrastructure.Design.Executor;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SchoolManagement.Website.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext _context;
        public AccountController()
        {

            _context = new ApplicationDbContext();
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //[AllowAnonymous]
        //public string GenerateDummyPassword(string password)
        //{
        //    PasswordHasher PasswordHash = new PasswordHasher();
        //    return PasswordHash.HashPassword(password);
        //}

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            //---xrnik---
            string SchoolName = ConfigurationManager.AppSettings["SchoolName"];
            ViewBag.SchoolName = SchoolName;
            //----
            ViewBag.ReturnUrl = returnUrl;

            var data = _context.Tbl_SchoolSetup.FirstOrDefault(x => x.Status == "Active");
            if(data != null)
            {
                var schooldata = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == data.School_Id);
                if(schooldata != null)
                {
                ViewBag.Schoolsetup = schooldata;
                }
            }
            else
            {
                ViewBag.Schoolsetup = null;
            }

            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                //---xrnik---
                string SchoolName = ConfigurationManager.AppSettings["SchoolName"];
                ViewBag.SchoolName = SchoolName;
                //----
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, change to shouldLockout: true
                var result = SignInManager.PasswordSignIn(model.Email, model.Password, model.RememberMe, shouldLockout: false);
                switch (result)
                {
                    case SignInStatus.Success:
                        //var roles = UserManager.GetRoles(User.Identity.GetUserId());
                        var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                        var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_context));

                        //return a list of current user's roleIDs
                        string userId = SignInManager
                           .AuthenticationManager
                           .AuthenticationResponseGrant.Identity.GetUserId();
                        ApplicationUser currentUser = UserManager.FindById(userId);
                        var roles = userManager.FindById(userId).Roles;
                        var roleId = roles.FirstOrDefault(r => r.UserId == userId).RoleId;
                        var roleName = RoleManager.FindById(roleId);
                        if (roleName.Name == "Professor" || roleName.Name == "Student")
                        {
                            var manageUser = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserName == model.Email && x.Password == model.Password);
                            //Session["loginId"] = manageUser.UserIdLogin;
                        }

                        Session["RoleName"] = roleName.Name;

                        var studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.UserId == currentUser.UserId.ToString());
                        var staffsdetails = _context.StafsDetails.FirstOrDefault(x => x.UserId == currentUser.UserId.ToString());

                        Session["StaffID"] = staffsdetails == null ? 0 : staffsdetails.StafId;
                        var rolepermissionnew = _context.Tbl_RolePermissionNew.Where(x => x.Role_Id == roleId).ToList();
                        if (staffsdetails == null)
                        {
                            Session["RolepermissionNew"] = rolepermissionnew;
                        }
                        else
                        {
                            rolepermissionnew = rolepermissionnew.Where(x => x.Staff_Id == staffsdetails.StafId).ToList();
                            Session["RolepermissionNew"] = rolepermissionnew;
                        }

                        Session["MenuList"] = _context.Tbl_MenuName.ToList();
                        Session["SubmenuList"] = _context.Tbl_SubmenuName.ToList();

                        Session["Name"] = model.Email;
                        Session["LoginRoleID"] = roleId;
                        Session["RoleName"] = roleName;
                        Session["rolename"] = roleName.Name;
                        var a = User.Identity.GetUserId();
                        Session["UserId"] = User.Identity.GetUserId();
                        //var a = currentUser.UserId.ToString();
                        //Session["UserId"] = currentUser.UserId.ToString();
                        Session["ScolarNo"] = studentDetails == null ? string.Empty : studentDetails.ApplicationNumber; // added by jairam 18/10/2021
                        Session["StudentId"] = studentDetails == null ? string.Empty : studentDetails.StudentRegisterID.ToString();
                        Session["Studentname"] = studentDetails == null ? string.Empty : studentDetails.Name + " " + studentDetails.Last_Name;
                        var rolePermissionList = _context.RolePagePermissions.Where(x => x.RoleId == roleId && x.HasPermission == true).ToList();
                        Session["rolePermissionList"] = rolePermissionList;
                        var schoolsetup = _context.Tbl_SchoolSetup.ToList();
                        TblCreateSchool schoolname = new TblCreateSchool();
                        if (schoolsetup != null)
                        {
                            foreach (var item in schoolsetup)
                            {
                                schoolname = _context.TblCreateSchool.FirstOrDefault(x => x.School_Id == item.School_Id);
                            }
                        }
                        Session["SchoolImage"] = schoolname == null ? null : schoolname.Upload_Image;
                        Session["SchoolName"] = schoolname == null ? null : schoolname.School_Name;
                        Session["SchoolAddress"] = schoolname == null ? null : schoolname.Address;
                        return RedirectToAction("Dashboard", "Dashboard");
                    //return RedirectToLocal(returnUrl);
                    case SignInStatus.LockedOut:
                        return View("Lockout");
                    case SignInStatus.RequiresVerification:
                        return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                    case SignInStatus.Failure:
                    default:
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (System.Exception ex)
            {
                ModelState.AddModelError("", "Please Try Again.");
                return View(model);

            }
        }






        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent: model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Index", "Home");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByNameAsync(model.Email);
                if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                // Send an email with this link
                // string code = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                // var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);		
                // await UserManager.SendEmailAsync(user.Id, "Reset Password", "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>");
                // return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // POST: /Account/LogOff
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion


        #region Change Username

        public ActionResult ChangeUserName()
        {
            var rolename = Session["RoleName"].ToString();
            if (rolename != "Student")
            {

                //return View();
            }
            else
            {
                var studentid = Session["StudentId"].ToString();
                if (studentid != null)
                {
                    var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID.ToString() == studentid);
                    ViewBag.Studentid = data.StudentRegisterID;
                    
                }
            }
            return View();
        }

        public ActionResult UpdateUserName(ChangeUserName changeUserName)
        {
            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == changeUserName.StudentId);
            var Loginstudentdata = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId.ToString() == studentdata.UserId);
            var url = Request.UrlReferrer.AbsoluteUri;
            StudentsRegistration studentsRegistration = new StudentsRegistration();

            #region 

            studentsRegistration.StudentRegisterID = studentdata.StudentRegisterID;
            studentsRegistration.ApplicationNumber = changeUserName.User_Name;
            studentsRegistration.UIN = studentdata.UIN;
            studentsRegistration.Date = studentdata.Date;
            studentsRegistration.Name = studentdata.Name;
            studentsRegistration.Class = studentdata.Class;
            studentsRegistration.Section = studentdata.Section;
            studentsRegistration.Gender = studentdata.Gender;
            studentsRegistration.RTE = studentdata.RTE;
            studentsRegistration.Medium = studentdata.Medium;
            studentsRegistration.Caste = studentdata.Caste;
            studentsRegistration.AgeInWords = studentdata.AgeInWords;
            studentsRegistration.DOB = studentdata.DOB;
            studentsRegistration.POB = studentdata.POB;
            studentsRegistration.Nationality = studentdata.Nationality;
            studentsRegistration.Religion = studentdata.Religion;
            studentsRegistration.MotherTongue = studentdata.MotherTongue;
            studentsRegistration.Category = studentdata.Category;
            studentsRegistration.BloodGroup = studentdata.BloodGroup;
            studentsRegistration.MedicalHistory = studentdata.MedicalHistory;
            studentsRegistration.Hobbies = studentdata.Hobbies;
            studentsRegistration.Sports = studentdata.Sports;
            studentsRegistration.OtherDetails = studentdata.OtherDetails;
            studentsRegistration.ProfileAvatar = studentdata.ProfileAvatar;
            studentsRegistration.MarkForIdentity = studentdata.MarkForIdentity;
            studentsRegistration.AdharNo = studentdata.AdharNo;
            studentsRegistration.AdharFile = studentdata.AdharFile;
            studentsRegistration.OtherLanguages = studentdata.OtherLanguages;
            studentsRegistration.IsApplyforTC = studentdata.IsApplyforTC;
            studentsRegistration.IsApplyforAdmission = studentdata.IsApplyforAdmission;
            studentsRegistration.IsApprove = studentdata.IsApprove;
            studentsRegistration.IsActive = studentdata.IsActive;
            studentsRegistration.IsAdmissionPaid = studentdata.IsAdmissionPaid;
            studentsRegistration.IsInsertFromAd = studentdata.IsInsertFromAd;
            studentsRegistration.Email = studentdata.Email;
            studentsRegistration.LastStudiedSchoolName = studentdata.LastStudiedSchoolName;
            studentsRegistration.Parents_Email = studentdata.Parents_Email;
            studentsRegistration.Class_Id = studentdata.Class_Id;
            studentsRegistration.Class_Name = studentdata.Class_Name;
            studentsRegistration.Section_Id = studentdata.Section_Id;
            studentsRegistration.Section_Name = studentdata.Section_Name;
            studentsRegistration.Last_Name = studentdata.Last_Name;
            studentsRegistration.Batch_Id = studentdata.Batch_Id;
            studentsRegistration.Batch_Name = studentdata.Batch_Name;
            studentsRegistration.BloodGroup_Id = studentdata.BloodGroup_Id;
            studentsRegistration.Religion_Id = studentdata.Religion_Id;
            studentsRegistration.Cast_Id = studentdata.Cast_Id;
            studentsRegistration.Category_Id = studentdata.Category_Id;
            studentsRegistration.Transport = studentdata.Transport;
            studentsRegistration.Transport_Options = studentdata.Transport_Options;
            studentsRegistration.Mobile = studentdata.Mobile;
            studentsRegistration.AdmissionFeePaid = studentdata.AdmissionFeePaid;
            studentsRegistration.City = studentdata.City;
            studentsRegistration.State = studentdata.State;
            studentsRegistration.Pincode = studentdata.Pincode;
            studentsRegistration.AddedYear = studentdata.AddedYear;
            studentsRegistration.Registration_Date = studentdata.Registration_Date;
            studentsRegistration.IsEmailsent = studentdata.IsEmailsent;

            #endregion

            _context.Entry(studentdata).CurrentValues.SetValues(studentsRegistration);
            _context.SaveChanges();

            #region Update Usercreation

            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement();

            tbl_UserManagement.UserId = Loginstudentdata.UserId;
            tbl_UserManagement.UserName = changeUserName.User_Name;
            tbl_UserManagement.Email = Loginstudentdata.Email;
            tbl_UserManagement.Password = Loginstudentdata.Password;
            tbl_UserManagement.Description = Loginstudentdata.Description;

            _context.Entry(Loginstudentdata).CurrentValues.SetValues(tbl_UserManagement);
            _context.SaveChanges();


            #endregion

            #region 

            

            #endregion


            return Content("<script language='javascript' type='text/javascript'>alert('UserName Updated Successully');location.replace('ChangeUserName')</script>");
        }


        #endregion

        #region Change Password

        public ActionResult ChangePassword()
        {
            var rolename = Session["RoleName"].ToString();
            if (rolename != "Student")
            {

                //return View();
            }
            else
            {
                var studentid = Session["StudentId"].ToString();
                if (studentid != null)
                {
                    var data = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID.ToString() == studentid);
                    ViewBag.Studentid = data.StudentRegisterID;

                }
            }
            return View();
        }

        public ActionResult UpdatePassword(ChangeUserName changeUserName)
        {
            var studentdata = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == changeUserName.StudentId);
            var Loginstudentdata = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId.ToString() == studentdata.UserId);

            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement();

            tbl_UserManagement.UserId = Loginstudentdata.UserId;
            tbl_UserManagement.UserName = Loginstudentdata.UserName;
            tbl_UserManagement.Email = Loginstudentdata.Email;
            tbl_UserManagement.Password = changeUserName.Password;
            tbl_UserManagement.Description = Loginstudentdata.Description;

            _context.Entry(Loginstudentdata).CurrentValues.SetValues(tbl_UserManagement);
            _context.SaveChanges();


            return Content("<script language='javascript' type='text/javascript'>alert('UserName Updated Successully');location.replace('ChangePassword')</script>");
        }

        #endregion

        //Change User Password
        #region ChangeUserPassword

        public async Task<JsonResult> ChangeUserPasswordForStaff(string oldpass, string newpass, string Confirmnewpass, string staffId)
        {
            try
            {
                int stf = Convert.ToInt32(staffId);

                var staffDetails = _context.StafsDetails.FirstOrDefault(x => x.StafId == stf);
                int staffUserId = Convert.ToInt32(staffDetails.UserId);
                PasswordHasher passwordHasher = new PasswordHasher();
                if (staffDetails != null)
                {
                    long? userid = await SignInManager.UserManager.Users.Where(x => x.UserId == staffUserId).Select(x => x.UserId).FirstOrDefaultAsync();
                    var result = new IdentityResult();
                    ApplicationUser model = UserManager.FindById(User.Identity.GetUserId());
                    //var isexisting = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                    var tblusermanager = _context.Tbl_UserManagement.Where(x => x.UserId == userid).FirstOrDefault();

                   // var stafflogin = _context.Tbl_UserManagement.Where(x => x.Description != "Student registration").ToList();

                    if (model != null && tblusermanager != null)
                    {
                        if (tblusermanager.Password == oldpass)
                        {
                            var hashpassword = passwordHasher.HashPassword(newpass);
                            if (hashpassword == model.PasswordHash)
                            {
                                return Json("Please Enter Different Password");
                            }
                            else
                            {
                                model.PasswordHash = hashpassword;
                                //await UserManager.UpdateAsync(model);
                                result = await UserManager.UpdateAsync(model);
                                
                                tblusermanager.Password = newpass;
                                _context.Entry(tblusermanager).CurrentValues.SetValues(tblusermanager);
                                _context.SaveChanges();
                                if (result.Succeeded)
                                {
                                    await UserManager.UpdateSecurityStampAsync(model.Id.ToString());
                                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                }
                                return Json("Password Changed Successfully");
                            }
                            //isexisting.PasswordHash = hashpassword;


                        }
                        else
                        {
                            return Json("Old Password Not Matched");

                        }
                    }
                    else
                    {
                        return Json("Something Went Wrong");

                    }
                }
                else
                {
                    return Json("StudentDetails Did not match");

                }

            }
            catch (Exception ex)
            {
                return Json("Error Occured");

            }
        }
        public async Task<JsonResult> ChangeUserPasswordForStaffByAdmin(string oldpass, string newpass, string Confirmnewpass, string staffId = null, string studentid = null)
        {
            try
            {
                PasswordHasher passwordHasher = new PasswordHasher();

                // Check for staffId or studentId and proceed accordingly
                string user = null;
                int intuser = 0;
                Tbl_UserManagement tblusermanager = null;

                if (!string.IsNullOrEmpty(staffId))
                {
                    int stf = Convert.ToInt32(staffId);
                    var staffDetails = _context.StafsDetails.FirstOrDefault(x => x.StafId == stf);
                    user = staffDetails?.UserId.ToString();
                    intuser = Convert.ToInt32(user);
                    long ? userid = await SignInManager.UserManager.Users.Where(x => x.UserId == intuser).Select(x => x.UserId).FirstOrDefaultAsync();
                    tblusermanager = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);
                }
                else if (!string.IsNullOrEmpty(studentid))
                {
                    int studentId = Convert.ToInt32(studentid);
                    var studentDetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == studentId);
                    user = studentDetails?.UserId.ToString();

                    intuser = Convert.ToInt32(user);
                    long? userid = await SignInManager.UserManager.Users.Where(x => x.UserId == intuser).Select(x => x.UserId).FirstOrDefaultAsync();
                    tblusermanager = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserId == userid);
                }
                var userhash = await SignInManager.UserManager.Users.Where(x => x.UserId == intuser).Select(x => x.Id).FirstOrDefaultAsync();

                ApplicationUser model = UserManager.FindById(userhash);
                // Validate if model and user management entry are found
                if (model == null || tblusermanager == null)

                    return Json("User not found");

                // Validate old password
                if (tblusermanager.Password != oldpass)
                    return Json("Old Password Not Matched");

                // Check if new password is different from the old one
                var hashPassword = passwordHasher.HashPassword(newpass);
                if (hashPassword == model.PasswordHash)
                    return Json("Please Enter Different Password");

                // Ensure new password and confirm password match
                if (newpass != Confirmnewpass)
                    return Json("New password and confirm password do not match");

                // Update password and security stamp
                model.PasswordHash = hashPassword;
                var result = await UserManager.UpdateAsync(model);
                if (result.Succeeded)
                {
                    tblusermanager.Password = newpass;
                    _context.Entry(tblusermanager).CurrentValues.SetValues(tblusermanager);
                    _context.SaveChanges();

                    await UserManager.UpdateSecurityStampAsync(model.Id.ToString());
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    return Json("Password Changed Successfully");
                }

                return Json("Error occurred while changing password");
            }
            catch (Exception ex)
            {
                return Json("Error occurred:"+ex.Message+"");
            }
        }

        public async Task<JsonResult> ChangeUserPassword(string oldpass,string newpass, string Confirmnewpass,string studentid)
        {
            try
            {
                int StudentId = Convert.ToInt32(studentid);

                var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.StudentRegisterID == StudentId);
                int StudentLoginId = Convert.ToInt32(studentdetails.UserId);
                PasswordHasher passwordHasher = new PasswordHasher();
                if(studentdetails != null)
                {
                    long? userid = await SignInManager.UserManager.Users.Where(x => x.UserId == StudentLoginId).Select(x => x.UserId).FirstOrDefaultAsync();
                    var result = new IdentityResult();
                    ApplicationUser model = UserManager.FindById(User.Identity.GetUserId());
                    //var isexisting = _context.Users.Where(x => x.UserId == userid).FirstOrDefault();
                    var tblusermanager = _context.Tbl_UserManagement.Where(x => x.UserId == userid).FirstOrDefault();
                    if(model != null && tblusermanager != null)
                    {
                        if(tblusermanager.Password == oldpass)
                        {
                            var hashpassword = passwordHasher.HashPassword(newpass);
                            if(hashpassword == model.PasswordHash)
                            {
                                return Json("Please Enter Different Password");
                            }
                            else
                            {
                                model.PasswordHash = hashpassword;
                                result = await UserManager.UpdateAsync(model);

                                tblusermanager.Password = newpass;
                                _context.Entry(tblusermanager).CurrentValues.SetValues(tblusermanager);
                                _context.SaveChanges();
                                if (result.Succeeded)
                                {
                                    await UserManager.UpdateSecurityStampAsync(model.Id.ToString());
                                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                                }
                                return Json("Password Changed Successfully");
                            }
                            //isexisting.PasswordHash = hashpassword;
                          

                        }
                        else
                        {
                            return Json("Old Password Not Matched");

                        }
                    }
                    else
                    {
                        return Json("Something Went Wrong");

                    }
                }
                else
                {
                    return Json("StudentDetails Did not match");

                }

            }
            catch (Exception ex)
            {
                return Json("Error Occured");

            }
        }

        #endregion


        #region Basicpaymaster

        public ActionResult BasicpayMaster()
        {

            var pagename = "BasicpayMaster";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            var data = _context.Tbl_BasicpayMaster.ToList();

            List<Tbl_Basicpaymasterviewmodel> tbl_Basicpaymasterviewmodels = new List<Tbl_Basicpaymasterviewmodel>();
            foreach(var item in data)
            {
                tbl_Basicpaymasterviewmodels.Add(new Tbl_Basicpaymasterviewmodel
                {
                    BasicPay_MasterId = item.BasicPay_MasterId,
                    Basicpay_Name = item.Basicpay_Name,
                    Editpermission = CheckEditpermission(pagename,editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)
                });
            }

            ViewBag.BasicDetails = tbl_Basicpaymasterviewmodels;

            return View();
        }

        public ActionResult AddBasicpaydetails(Tbl_BasicpayMaster tbl_BasicpayMaster)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_BasicpayMaster.Created_Date = DateTime.Now.ToString();
            _context.Tbl_BasicpayMaster.Add(tbl_BasicpayMaster);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
        }


        public JsonResult GetBasicpaydetailsById(int id)
        {
            var data = _context.Tbl_BasicpayMaster.FirstOrDefault(x => x.BasicPay_MasterId == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateBasicPayDetails(Tbl_BasicpayMaster tbl_BasicpayMaster)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_BasicpayMaster.FirstOrDefault(x => x.BasicPay_MasterId == tbl_BasicpayMaster.BasicPay_MasterId);
                if (data != null)
                {
                    tbl_BasicpayMaster.Created_Date = DateTime.Now.ToString();
                    _context.Entry(data).CurrentValues.SetValues(tbl_BasicpayMaster);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Updated');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DeleteBasicPaydetails(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_BasicpayMaster.FirstOrDefault(x => x.BasicPay_MasterId == id);
            if (data != null)
            {
                _context.Tbl_BasicpayMaster.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Data Not Deleted');location.replace('" + url + "')</script>");
            }
        }


        #endregion


        #region Staffcategory

        public ActionResult StaffCategory()
        {
            var pagename = "StaffCategory";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            var data = _context.Tbl_StaffCategory.ToList();

            List<Tbl_Staffcategoryviewmodel> tbl_Staffcategoryviewmodels = new List<Tbl_Staffcategoryviewmodel>();
            foreach(var item in data)
            {
                tbl_Staffcategoryviewmodels.Add(new Tbl_Staffcategoryviewmodel
                {
                    Staff_Category_Id = item.Staff_Category_Id,
                    Category_Name = item.Category_Name,
                    Editpermission = CheckEditpermission(pagename, editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)
                });
            }

            ViewBag.BasicDetails = tbl_Staffcategoryviewmodels;



            return View();
        }

        public ActionResult AddStaffCategory(Tbl_StaffCategory tbl_StaffCategory)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_StaffCategory.Created_Date = DateTime.Now.ToString();
            _context.Tbl_StaffCategory.Add(tbl_StaffCategory);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
        }


        public JsonResult GeStaffCategoryById(int id)
        {
            var data = _context.Tbl_StaffCategory.FirstOrDefault(x => x.Staff_Category_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateStaffCategory(Tbl_StaffCategory tbl_StaffCategory)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_StaffCategory.FirstOrDefault(x => x.Staff_Category_Id == tbl_StaffCategory.Staff_Category_Id);
                if (data != null)
                {
                    tbl_StaffCategory.Created_Date = DateTime.Now.ToString();
                    _context.Entry(data).CurrentValues.SetValues(tbl_StaffCategory);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Updated');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DeleteStaffCategory(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_StaffCategory.FirstOrDefault(x => x.Staff_Category_Id == id);
            if (data != null)
            {
                _context.Tbl_StaffCategory.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Data Not Deleted');location.replace('" + url + "')</script>");
            }
        }


        #endregion

        #region Account Type

        public ActionResult AccountType()
        {

            var pagename = "AccountType";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            var data = _context.Tbl_AccountType.ToList();

            List<Tbl_Accouttypeviewmodel> tbl_Accouttypeviewmodels = new List<Tbl_Accouttypeviewmodel>();
            foreach(var item in data)
            {
                tbl_Accouttypeviewmodels.Add(new Tbl_Accouttypeviewmodel
                {
                    Account_TypeId = item.Account_TypeId,
                    Account_Typename = item.Account_Typename,
                    Editpermission = CheckEditpermission(pagename,editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)
                });
            }

            ViewBag.Accounttype = tbl_Accouttypeviewmodels;



            return View();
        }

        public ActionResult AddAccountType(Tbl_AccountType tbl_AccountType)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_AccountType.Created_Date = DateTime.Now.ToString();
            _context.Tbl_AccountType.Add(tbl_AccountType);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
        }


        public JsonResult GetAccountypeById(int id)
        {
            var data = _context.Tbl_AccountType.FirstOrDefault(x => x.Account_TypeId == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateAccountType(Tbl_AccountType tbl_AccountType)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_AccountType.FirstOrDefault(x => x.Account_TypeId == tbl_AccountType.Account_TypeId);
                if (data != null)
                {
                    tbl_AccountType.Created_Date = DateTime.Now.ToString();
                    _context.Entry(data).CurrentValues.SetValues(tbl_AccountType);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Updated');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DeleteAccountype(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_AccountType.FirstOrDefault(x => x.Account_TypeId == id);
            if (data != null)
            {
                _context.Tbl_AccountType.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Data Not Deleted');location.replace('" + url + "')</script>");
            }
        }


        #endregion

        #region Basicpaydetails

        public ActionResult BasicPay()
        {

            var pagename = "BasicPay";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            var Basicpaydetails = _context.Tbl_BasicpayMaster.ToList();
            ViewBag.Basicpaydetails = Basicpaydetails;

           
            var BasicPayDetails = _context.Tbl_BasicPayDetails.ToList();
            List<Tbl_BasicPayDetails> Tbl_BasicPayDetails = new List<Tbl_BasicPayDetails>();
            List<TblBasicpadetailviewmodel> TblBasicpadetailviewmodel = new List<TblBasicpadetailviewmodel>();
            //foreach (var item in BasicPayDetails)
            //{
            //    Tbl_BasicPayDetails.Add(new Tbl_BasicPayDetails
            //    {
            //        BasicAmount_Id = item.BasicAmount_Id,
            //        BasicPay_Id = item.BasicPay_Id,
            //        Basicpay_Name = Basicpaydetails.FirstOrDefault(x => x.BasicPay_MasterId == item.BasicPay_Id)?.Basicpay_Name,
            //        Basic_Amount = item.Basic_Amount,
            //        CreatedDate = item.CreatedDate,

            //    });
            //}

            foreach(var item in BasicPayDetails)
            {
                TblBasicpadetailviewmodel.Add(new TblBasicpadetailviewmodel
                {
                    BasicAmount_Id = item.BasicAmount_Id,
                    BasicPay_Id = item.BasicPay_Id,
                    Basicpay_Name = Basicpaydetails.FirstOrDefault(x => x.BasicPay_MasterId == item.BasicPay_Id)?.Basicpay_Name,
                    Basic_Amount = item.Basic_Amount,
                    CreatedDate = item.CreatedDate,
                    Editpermission = CheckEditpermission(pagename,editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)

                });
            }


            ViewBag.BasicPayList = TblBasicpadetailviewmodel;

            return View();
        }

        public ActionResult AddBasicPay(Tbl_BasicPayDetails tbl_BasicPayDetails)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_BasicPayDetails.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            _context.Tbl_BasicPayDetails.Add(tbl_BasicPayDetails);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
        }

        public JsonResult GetBasicPayById(int Id)
        {
            try
            {
                var data = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicAmount_Id == Id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ActionResult BasicpayEdit(Tbl_BasicPayDetails tbl_BasicPayDetails)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicAmount_Id == tbl_BasicPayDetails.BasicAmount_Id);
                if (data != null)
                {
                    tbl_BasicPayDetails.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    _context.Entry(data).CurrentValues.SetValues(tbl_BasicPayDetails);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Updated Successfully');location.replace('" + url + "')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Found');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DeleteBasicPay(int Id)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicAmount_Id == Id);
                if (data != null)
                {
                    _context.Tbl_BasicPayDetails.Remove(data);
                    _context.SaveChanges();
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Deleted Successfully');location.replace('" + url + "')</script>");
                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Found');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Staff Salary

        public ActionResult StaffSalary()
        {

            var pagename = "StaffSalary";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;


            ViewBag.staffdetails = _context.StafsDetails.ToList();
            var staffdetails = _context.StafsDetails.ToList();
            var staffsalary = _context.Tbl_StaffSalary.ToList();

            List<Tbl_StaffSalary> tbl_StaffSalaries = new List<Tbl_StaffSalary>();
            //foreach (var item in staffsalary)
            //{
            //    tbl_StaffSalaries.Add(new Tbl_StaffSalary
            //    {
            //        Salary_Id = item.Salary_Id,
            //        Staff_ID = item.Staff_ID,

            //        Staff_Name = staffdetails.FirstOrDefault(x => x.StafId == item.Staff_ID)?.Name,
            //        Salary_Amount = item.Salary_Amount,
            //        CreatedDate = item.CreatedDate,
            //        Basic_Amount = item.Basic_Amount
            //    });
            //}

            List<Tbl_Staffsalaryviewmodel> tbl_Staffsalaryviewmodels = new List<Tbl_Staffsalaryviewmodel>();
            foreach(var item in staffsalary)
            {
                tbl_Staffsalaryviewmodels.Add(new Tbl_Staffsalaryviewmodel
                {
                    Salary_Id = item.Salary_Id,
                    Staff_ID = item.Staff_ID,
                    Staff_Name = staffdetails.FirstOrDefault(x => x.StafId == item.Staff_ID)?.Name,
                    Salary_Amount = item.Salary_Amount,
                    CreatedDate = item.CreatedDate,
                    Basic_Amount = item.Basic_Amount,
                    Editpermission = CheckEditpermission(pagename,editpermission),
                    DeletePermission = CheckDeletepermission(pagename,deletepermission)
                });
            }

            ViewBag.staffsalary = tbl_Staffsalaryviewmodels;

            return View();
        }

        public JsonResult GetStaffSallery()
        {
            var staffdetails = _context.StafsDetails.ToList();

            var staffsalary = _context.Tbl_StaffSalary.ToList();

            List<Tbl_StaffSalary> tbl_StaffSalaries = new List<Tbl_StaffSalary>();
            foreach (var item in staffsalary)
            {
                tbl_StaffSalaries.Add(new Tbl_StaffSalary
                {
                    Salary_Id = item.Salary_Id,
                    Staff_ID = item.Staff_ID,

                    Staff_Name = staffdetails.FirstOrDefault(x => x.StafId == item.Staff_ID)?.Name,
                    Salary_Amount = item.Salary_Amount,
                    CreatedDate = item.CreatedDate,
                    Basic_Amount = item.Basic_Amount
                });
            }

            string html = "";
            int y = 1;
            for (int i = 0; i < tbl_StaffSalaries.Count(); i++)
            {
                html += "<tr>";
                html += "<td>" + y + "</td>";
                html += "<td>" + tbl_StaffSalaries[i].Staff_Name + "</td>";
                html += "<td>" + tbl_StaffSalaries[i].Salary_Amount + "</td>";
                html += "<td>" + tbl_StaffSalaries[i].Basic_Amount + "</td>";
                html += "<td><button type='button' data-val='" + tbl_StaffSalaries[i].Salary_Id + "' class='btn btn-success BtnEdit'>Edit</button><button type='button' id='BtnDelete' data-val='" + tbl_StaffSalaries[i].Salary_Id + "' class='btn btn-danger'>Delete</button></td>";
                html += "</tr>";
                y++;
            }

            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AddStaffSalary(Tbl_StaffSalary tbl_StaffSalary)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_StaffSalary.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            _context.Tbl_StaffSalary.Add(tbl_StaffSalary);
            _context.SaveChanges();


            //Archive Staff Salary
            Tbl_ArchieveStaffSalary tbl_ArchieveStaffSalary = new Tbl_ArchieveStaffSalary();
            tbl_ArchieveStaffSalary.Basic_Amount = tbl_StaffSalary.Basic_Amount;
            tbl_ArchieveStaffSalary.Staff_ID = tbl_StaffSalary.Staff_ID;
            tbl_ArchieveStaffSalary.Salary_Amount = tbl_StaffSalary.Salary_Amount;
            tbl_ArchieveStaffSalary.CreatedDate = DateTime.Now.ToString();
            _context.Tbl_ArchieveStaffSalary.Add(tbl_ArchieveStaffSalary);
            _context.SaveChanges();


            return Content("<script language='javascript' type='text/javascript'>alert('Salary Added Successfully');location.replace('" + url + "')</script>");

        }

        public JsonResult GetSalaryDataById(int Id)
        {
            try
            {
                var data = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Salary_Id == Id);
                if (data != null)
                {
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Fail", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EditSalary(Tbl_StaffSalary tbl_StaffSalary)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Salary_Id == tbl_StaffSalary.Salary_Id);
                if (data != null)
                {
                    tbl_StaffSalary.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    _context.Entry(data).CurrentValues.SetValues(tbl_StaffSalary);
                    _context.SaveChanges();

                    //Archive Staff Salary
                    Tbl_ArchieveStaffSalary tbl_ArchieveStaffSalary = new Tbl_ArchieveStaffSalary();
                    tbl_ArchieveStaffSalary.Basic_Amount = tbl_StaffSalary.Basic_Amount;
                    tbl_ArchieveStaffSalary.Staff_ID = tbl_StaffSalary.Staff_ID;
                    tbl_ArchieveStaffSalary.Salary_Amount = tbl_StaffSalary.Salary_Amount;
                    tbl_ArchieveStaffSalary.CreatedDate = DateTime.Now.ToString();
                    _context.Tbl_ArchieveStaffSalary.Add(tbl_ArchieveStaffSalary);
                    _context.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");

                }
                else
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Date Not Updated');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult DeleteSalary(int id)
        {
            try
            {
                var url = Request.UrlReferrer.AbsoluteUri;
                var data = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Salary_Id == id);
                if (data != null)
                {
                    _context.Tbl_StaffSalary.Remove(data);
                    _context.SaveChanges();

                    return Content("<script language='javascript' type='text/javascript'>alert('Data Deleted Successfully');location.replace('" + url + "')</script>");
                }
                else
                {

                    return Content("<script language='javascript' type='text/javascript'>alert('Data Not Fount');location.replace('" + url + "')</script>");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region PfCalculation

        public ActionResult PFCalculation()
        {
            var pagename = "PFCalculation";
            
            var createpermission = "Create_permission";

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;



            ViewBag.StaffDetails = _context.StafsDetails.ToList();

            ViewBag.summarydetails = _context.Tbl_AccountSummary.ToList();


            ViewBag.ArrearDetails = _context.Tbl_Arrear.ToList();

            ViewBag.DeductinDetails = _context.Tbl_Deductions.ToList();

            return View();
        }

        public JsonResult FileterPF(int id, string Date, string checkid)
        {
            DateTime Attendacnedate = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string year = Attendacnedate.Year.ToString();
            string month = Attendacnedate.Month.ToString();
            int year1 = Attendacnedate.Year;
            int month1 = Attendacnedate.Month;
            int day = DateTime.DaysInMonth(year1, month1);
            int DAAmount = 0;
            double ESI = 0;
            double BasicAmount = 0;
            double DAAMount = 0;
            double HRAAmount = 0;
            double CCAAmount = 0;
            double OtherAllowance = 0;
            double BasicandDA = 0;
            double PF = 0;
            double TOtalDeductions = 0;
            double TotalNetPay = 0;
            double TotalGrossAmount = 0;
            double Pension = 0;
            int TotalPensionAmount = 0;
            double ReducedGrossAmount = 0;
            int Employer_contribution = 0;
            double PensionAMount = 0;
            double Employeer_Contri = 0;
            double Eplr_Contri = 0;
            float LOP = 0;
            string Emploueecode = "";
            double basicamount = 0;
            var Totalnoofdayspresent = "";
            double TotalPercentage = 0;
            //var BasicSalary = 0;
            string monthname = Attendacnedate.ToString("MMMM");
            Tbl_StaffSalary BasicSalary;
            List<StafsDetails> staffdetails;
            var data = _context.StafsDetails.FirstOrDefault(x => x.StafId == id);
            var basicpaydetails = _context.Tbl_BasicPayDetails.ToList();
            //var basicpay = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicPay_Id == 237 && x.AccountType_Id == data.Employee_AccountId);
            //var DA = _context.Tbl_BasicPayDetails.FirstOrDefault(x => x.BasicPay_Id == 238 && x.AccountType_Id == data.Employee_AccountId);
            var salarydata = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);

            var DA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 1);

            var HRA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 4);

            var CCA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 3);

            var ESI_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 5);

            var Pension_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 6);

            var Employer_contriPercentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 7);

            var PF_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 2);

            var Other_Allowancepercentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 8);

            var Basic_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 9);

            //PF Calculation Formula
            //Toal Basic and DA = Basic + DA
            //EPF = Basic + DA / 100 * 12
            //EmployerContri = Baisc + DA /100 * 3.67
            //pension = Basic + DA / 100 * 8.33
            //Professiona Tax if(Gross > 15000) Gross - 200
            //Toal Deduction PF and Professional Tax
            //Calculate Staff Attendance

            var staffattendance = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id == id && x.Attendence_Month == month && x.Attendence_Year == year).ToList();

            var StaffCategory = _context.Tbl_StaffCategory.ToList();
            //If the staff is permanent pf will be deducted else not dedeucted

            if (checkid == "1")//With LOP
            {
                if (data.Category_Id == 1 || data.Category_Id == 3)//Check whether the staff is permanent or temporary
                {
                    if (salarydata.Salary_Amount > 8000)//check total salary if the salary is > than some value pf will be deducted
                    {
                        var fulldaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "A").Count();
                        var halfdaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "½P").Count();
                        double Absentday = (double)halfdaypresent / 2;
                        LOP = Convert.ToInt32(fulldaypresent + Absentday);
                        //float Lossofpay = Convert.ToInt32( LOP);
                        float absentday = staffattendance.Count() - LOP;
                        float perdaysalary = (salarydata.Salary_Amount / staffattendance.Count());

                        double Reducedbasicamount = day - absentday;
                        Totalnoofdayspresent = Convert.ToString(staffattendance.Count() - LOP);
                        //BasicSalary = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);
                        TotalPercentage = Math.Round((absentday * 100) / day);
                        BasicSalary = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);
                        //Calculate Basic 50%
                        BasicAmount = ((BasicSalary.Salary_Amount * Basic_Percentage.Basic_Amount) / 100);
                        if (fulldaypresent == day)
                        {
                            basicamount = 0;
                        }
                        else
                        {
                            double BasicDetails = ((BasicAmount) / day);
                            int amount = Convert.ToInt32(BasicAmount * absentday);

                            basicamount = (amount / day);
                        }



                        //Calculate DA 25%
                        DAAMount = Math.Round((basicamount * DA_Percentage.Basic_Amount) / 100);

                        //Calculate HRA 40%
                        HRAAmount = Math.Round((basicamount * HRA_Percentage.Basic_Amount) / 100);

                        //Calculate CCA 15.5%
                        CCAAmount = Math.Round((basicamount * CCA_Percentage.Basic_Amount) / 100);

                        //Calculate OtherAllowance 40%
                        OtherAllowance = Math.Round((basicamount * Other_Allowancepercentage.Basic_Amount) / 100);
                        TotalGrossAmount = Math.Round(OtherAllowance + DAAMount + HRAAmount + CCAAmount + basicamount);

                        //Calculate PF Amount
                        BasicandDA = basicamount + DAAMount;
                        PF = Math.Round((BasicandDA * PF_Percentage.Basic_Amount) / 100);

                        //Calculate ESI Amount 0.75%
                        if (salarydata.Salary_Amount <= 21000)
                        {
                            ESI = ((salarydata.Salary_Amount * ESI_Percentage.Basic_Amount) / 100);
                        }
                        else
                        {
                            ESI = 0;
                        }

                        //professional tax
                        if (salarydata.Salary_Amount >= 15000)
                        {
                            ReducedGrossAmount = 200;
                        }
                        else
                        {
                            ReducedGrossAmount = 0;
                        }

                        TOtalDeductions = PF + ESI + ReducedGrossAmount;
                        TotalNetPay = TotalGrossAmount - TOtalDeductions;


                        //PF Calculation

                        double GrossAmount = basicamount + DAAmount;//Total Basic + DA Amount

                        double epfdata = Math.Round(((basicamount + DAAmount) / 100) * 12); //PF Amount


                        Pension = BasicandDA / 100 * Pension_Percentage.Basic_Amount;//Pension Amount
                        PensionAMount = Math.Round(Pension);

                        Employeer_Contri = BasicandDA / 100 * Employer_contriPercentage.Basic_Amount;//Employeer Contribution
                        Eplr_Contri = Math.Round(Employeer_Contri);



                        //if(PensionAMount > 1250)
                        {
                            TotalPensionAmount = Convert.ToInt32(PensionAMount);
                            //Employer_contribution = TotalPensionAmount - Eplr_Contri;
                        }
                        //if(Eplr_Contri > 1250)
                        {
                            Employer_contribution = Convert.ToInt32(Eplr_Contri);
                        }

                        if (salarydata.Salary_Amount < 21000)
                        {
                            ESI = ((salarydata.Salary_Amount / 100) * ESI_Percentage.Basic_Amount);
                        }


                        double Netsalary = (salarydata.Salary_Amount - TOtalDeductions);//Net Salary

                        int netsalary = Convert.ToInt32(Netsalary);
                        double netsalaryy = Convert.ToInt32(Netsalary);
                        double rountof = Math.Round(netsalaryy);
                        var Numberinwords = NumberToWords(netsalary);

                        staffdetails = _context.StafsDetails.ToList();
                        Emploueecode = id.ToString();

                        PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                        payrollBasicDetails.BasicSalary = Convert.ToInt32(basicamount);
                        payrollBasicDetails.PF = Convert.ToInt32(PF);
                        payrollBasicDetails.NetPay = Convert.ToInt32(TotalNetPay);
                        payrollBasicDetails.DA = Convert.ToInt32(DAAMount);
                        payrollBasicDetails.NumberinWords = Numberinwords;
                        payrollBasicDetails.MonthName = monthname;
                        payrollBasicDetails.Year = year;
                        payrollBasicDetails.Professional_Tax = Convert.ToInt32(ReducedGrossAmount);
                        payrollBasicDetails.Employee_Contribution = Convert.ToInt32(PF);
                        payrollBasicDetails.Employer_Contribution = Convert.ToInt32(Eplr_Contri);
                        payrollBasicDetails.Gross = Convert.ToInt32(TotalGrossAmount);
                        payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                        payrollBasicDetails.ESI = Convert.ToInt32(ESI);
                        payrollBasicDetails.Staff_Id = data.StafId.ToString();
                        payrollBasicDetails.Staff_Name = data.Name;
                        payrollBasicDetails.LOP = LOP;
                        payrollBasicDetails.HRA = Convert.ToInt32(HRAAmount);
                        payrollBasicDetails.OtherAllowance = Convert.ToInt32(OtherAllowance);
                        payrollBasicDetails.CCA = Convert.ToInt32(CCAAmount);
                        payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                        payrollBasicDetails.TotalPercentage = TotalPercentage;
                        payrollBasicDetails.Category_ID = data.Category_Id;

                        return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var fulldaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "A").Count();
                        var halfdaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "½P").Count();
                        double Absentday = (double)halfdaypresent / 2;
                        LOP = Convert.ToInt32(fulldaypresent + Absentday);
                        //float Lossofpay = Convert.ToInt32( LOP);
                        Totalnoofdayspresent = Convert.ToString(day - LOP);
                        double noofdayepresent = Convert.ToInt32(Totalnoofdayspresent);
                        TotalPercentage = Math.Round((noofdayepresent * 100) / day);
                        int netsalary = Convert.ToInt32(salarydata.Salary_Amount);

                        var Numberinwords = NumberToWords(netsalary);

                        staffdetails = _context.StafsDetails.ToList();
                        Emploueecode = id.ToString();

                        PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                        payrollBasicDetails.BasicSalary = 0;
                        payrollBasicDetails.PF = Convert.ToInt32(PF);
                        payrollBasicDetails.NetPay = Convert.ToInt32(salarydata.Salary_Amount);
                        payrollBasicDetails.DA = 0;
                        payrollBasicDetails.NumberinWords = Numberinwords;
                        payrollBasicDetails.MonthName = monthname;
                        payrollBasicDetails.Year = year;
                        payrollBasicDetails.Professional_Tax = 0;
                        payrollBasicDetails.Employee_Contribution = 0;
                        payrollBasicDetails.Employer_Contribution = 0;
                        payrollBasicDetails.Gross = Convert.ToInt32(salarydata.Salary_Amount);
                        payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                        payrollBasicDetails.ESI = 0;
                        payrollBasicDetails.Staff_Id = data.StafId.ToString();
                        payrollBasicDetails.Staff_Name = data.Name;
                        payrollBasicDetails.LOP = LOP;
                        payrollBasicDetails.HRA = 0;
                        payrollBasicDetails.OtherAllowance = 0;
                        payrollBasicDetails.CCA = 0;
                        payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                        payrollBasicDetails.TotalPercentage = TotalPercentage;
                        payrollBasicDetails.Category_ID = data.Category_Id;

                        return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var fulldaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "A").Count();
                    var halfdaypresent = staffattendance.Where(x => x.Mark_FullDayPresent == "½P").Count();
                    double Absentday = (double)halfdaypresent / 2;
                    LOP = Convert.ToInt32(fulldaypresent + Absentday);
                    //float Lossofpay = Convert.ToInt32( LOP);
                    Totalnoofdayspresent = Convert.ToString(day - LOP);
                    double noofdayepresent = Convert.ToInt32(Totalnoofdayspresent);
                    TotalPercentage = Math.Round((noofdayepresent * 100) / day);
                    int netsalary = Convert.ToInt32(salarydata.Salary_Amount);

                    var Numberinwords = NumberToWords(netsalary);

                    staffdetails = _context.StafsDetails.ToList();
                    Emploueecode = id.ToString();

                    PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                    payrollBasicDetails.BasicSalary = 0;
                    payrollBasicDetails.PF = Convert.ToInt32(PF);
                    payrollBasicDetails.NetPay = Convert.ToInt32(salarydata.Salary_Amount);
                    payrollBasicDetails.DA = 0;
                    payrollBasicDetails.NumberinWords = Numberinwords;
                    payrollBasicDetails.MonthName = monthname;
                    payrollBasicDetails.Year = year;
                    payrollBasicDetails.Professional_Tax = 0;
                    payrollBasicDetails.Employee_Contribution = 0;
                    payrollBasicDetails.Employer_Contribution = 0;
                    payrollBasicDetails.Gross = Convert.ToInt32(salarydata.Salary_Amount);
                    payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                    payrollBasicDetails.ESI = 0;
                    payrollBasicDetails.Staff_Id = data.StafId.ToString();
                    payrollBasicDetails.Staff_Name = data.Name;
                    payrollBasicDetails.LOP = LOP;
                    payrollBasicDetails.HRA = 0;
                    payrollBasicDetails.OtherAllowance = 0;
                    payrollBasicDetails.CCA = 0;
                    payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                    payrollBasicDetails.TotalPercentage = TotalPercentage;
                    payrollBasicDetails.Category_ID = data.Category_Id;

                    return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);

                }
            }
            else//Without LOP
            {
                if (data.Category_Id == 1 || data.Category_Id == 3)//Check whether the staff is permanent or temporary
                {
                    if (salarydata.Salary_Amount > 8000)
                    {
                        LOP = 0;
                        TotalPercentage = 100;
                        Totalnoofdayspresent = Convert.ToString(day);
                        staffdetails = _context.StafsDetails.ToList();
                        BasicSalary = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);

                        //calculate Basic 50%
                        BasicAmount = (salarydata.Salary_Amount * Basic_Percentage.Basic_Amount) / 100;

                        //Calculate DA 25%
                        DAAMount = (BasicAmount * DA_Percentage.Basic_Amount) / 100;

                        //Calculate HRA 40%
                        HRAAmount = (BasicAmount * HRA_Percentage.Basic_Amount) / 100;

                        //Calculate CCA 15.5%
                        CCAAmount = (BasicAmount * CCA_Percentage.Basic_Amount) / 100;

                        //Calculate OtherAllowance 40%
                        OtherAllowance = (BasicAmount * Other_Allowancepercentage.Basic_Amount) / 100;

                        //TotalGrossAmount = OtherAllowance +
                        TotalGrossAmount = OtherAllowance + DAAMount + HRAAmount + CCAAmount + BasicAmount;


                        //Calculate PF Amount
                        BasicandDA = BasicAmount + DAAMount;
                        PF = Math.Round((BasicandDA * PF_Percentage.Basic_Amount) / 100);

                        //Calculate ESI Amount 0.75%
                        if (BasicSalary.Salary_Amount <= 21000)
                        {
                            ESI = ((BasicSalary.Salary_Amount * ESI_Percentage.Basic_Amount) / 100);
                        }
                        else
                        {
                            ESI = 0;
                        }

                        if (BasicSalary.Salary_Amount >= 15000)
                        {
                            ReducedGrossAmount = 200;
                        }
                        else
                        {
                            ReducedGrossAmount = 0;
                        }

                        Pension = BasicandDA / 100 * Pension_Percentage.Basic_Amount;
                        PensionAMount = Math.Round(Pension);
                        TotalPensionAmount = Convert.ToInt32(PensionAMount);


                        Employeer_Contri = BasicandDA / 100 * Employer_contriPercentage.Basic_Amount;
                        Eplr_Contri = Math.Round(Employeer_Contri);
                        Employer_contribution = Convert.ToInt32(Eplr_Contri);

                        TOtalDeductions = PF + ESI + ReducedGrossAmount;

                        TotalNetPay = BasicSalary.Salary_Amount - TOtalDeductions;
                        int totalnetpay = Convert.ToInt32(TotalNetPay);
                        var Numberinwords = NumberToWords(totalnetpay);
                        Emploueecode = id.ToString();



                        PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                        payrollBasicDetails.BasicSalary = Convert.ToInt32(BasicAmount);
                        payrollBasicDetails.PF = Convert.ToInt32(PF);
                        payrollBasicDetails.NetPay = Convert.ToInt32(TotalNetPay);
                        payrollBasicDetails.DA = Convert.ToInt32(DAAMount);
                        payrollBasicDetails.NumberinWords = Numberinwords;
                        payrollBasicDetails.MonthName = monthname;
                        payrollBasicDetails.Year = year;
                        payrollBasicDetails.Professional_Tax = Convert.ToInt32(ReducedGrossAmount);
                        payrollBasicDetails.Employee_Contribution = Convert.ToInt32(PF);
                        payrollBasicDetails.Employer_Contribution = Employer_contribution;
                        payrollBasicDetails.Gross = salarydata.Salary_Amount;
                        payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                        payrollBasicDetails.ESI = Convert.ToInt32(ESI);
                        payrollBasicDetails.Staff_Id = data.StafId.ToString();
                        payrollBasicDetails.Staff_Name = data.Name;
                        payrollBasicDetails.LOP = LOP;
                        payrollBasicDetails.HRA = Convert.ToInt32(HRAAmount);
                        payrollBasicDetails.CCA = Convert.ToInt32(CCAAmount);
                        payrollBasicDetails.OtherAllowance = Convert.ToInt32(OtherAllowance);
                        payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                        payrollBasicDetails.TotalPercentage = TotalPercentage;
                        payrollBasicDetails.Category_ID = data.Category_Id;




                        return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        LOP = 0;
                        TotalPercentage = 100;
                        staffdetails = _context.StafsDetails.ToList();
                        BasicSalary = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);

                        Totalnoofdayspresent = Convert.ToString(day);


                        int totalnetpay = Convert.ToInt32(salarydata.Salary_Amount);
                        var Numberinwords = NumberToWords(totalnetpay);
                        Emploueecode = id.ToString();


                        PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                        payrollBasicDetails.BasicSalary = 0;
                        payrollBasicDetails.PF = Convert.ToInt32(PF);
                        payrollBasicDetails.NetPay = Convert.ToInt32(salarydata.Salary_Amount);
                        payrollBasicDetails.DA = 0;
                        payrollBasicDetails.NumberinWords = Numberinwords;
                        payrollBasicDetails.MonthName = monthname;
                        payrollBasicDetails.Year = year;
                        payrollBasicDetails.Professional_Tax = 0;
                        payrollBasicDetails.Employee_Contribution = 0;
                        payrollBasicDetails.Employer_Contribution = 0;
                        payrollBasicDetails.Gross = salarydata.Salary_Amount;
                        payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                        payrollBasicDetails.ESI = 0;
                        payrollBasicDetails.Staff_Id = data.StafId.ToString();
                        payrollBasicDetails.Staff_Name = data.Name;
                        payrollBasicDetails.LOP = LOP;
                        payrollBasicDetails.HRA = 0;
                        payrollBasicDetails.OtherAllowance = 0;
                        payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                        payrollBasicDetails.TotalPercentage = TotalPercentage;
                        payrollBasicDetails.Category_ID = data.Category_Id;
                        return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);
                    }

                }
                else
                {


                    LOP = 0;
                    TotalPercentage = 100;
                    staffdetails = _context.StafsDetails.ToList();
                    BasicSalary = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == id);

                    Totalnoofdayspresent = Convert.ToString(day);


                    int totalnetpay = Convert.ToInt32(salarydata.Salary_Amount);
                    var Numberinwords = NumberToWords(totalnetpay);
                    Emploueecode = id.ToString();


                    PayrollBasicDetails payrollBasicDetails = new PayrollBasicDetails();
                    payrollBasicDetails.BasicSalary = 0;
                    payrollBasicDetails.PF = Convert.ToInt32(PF);
                    payrollBasicDetails.NetPay = Convert.ToInt32(salarydata.Salary_Amount);
                    payrollBasicDetails.DA = 0;
                    payrollBasicDetails.NumberinWords = Numberinwords;
                    payrollBasicDetails.MonthName = monthname;
                    payrollBasicDetails.Year = year;
                    payrollBasicDetails.Professional_Tax = 0;
                    payrollBasicDetails.Employee_Contribution = 0;
                    payrollBasicDetails.Employer_Contribution = 0;
                    payrollBasicDetails.Gross = salarydata.Salary_Amount;
                    payrollBasicDetails.Total_Salary = salarydata.Salary_Amount;
                    payrollBasicDetails.ESI = 0;
                    payrollBasicDetails.Staff_Id = data.StafId.ToString();
                    payrollBasicDetails.Staff_Name = data.Name;
                    payrollBasicDetails.LOP = LOP;
                    payrollBasicDetails.HRA = 0;
                    payrollBasicDetails.OtherAllowance = 0;
                    payrollBasicDetails.Noofdayspresent = Totalnoofdayspresent;
                    payrollBasicDetails.TotalPercentage = TotalPercentage;
                    payrollBasicDetails.Category_ID = data.Category_Id;
                    return Json(payrollBasicDetails, JsonRequestBehavior.AllowGet);

                }
            }

        }

        


        public ActionResult PFCalulationSummary()
        {
            ViewBag.StaffDetails = _context.StafsDetails.ToList();
            return View();
        }

        public JsonResult FilterPFSummary(string Date)
        {
            DateTime date = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            var month = date.Month.ToString();
            var year = date.Year.ToString();

            var data = _context.Tbl_AccountSummary.Where(x => x.Added_Month == month && x.Added_Year == year).ToList();

            if (data.Count > 0)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Letter to Bank

        public ActionResult SalaryStatement()
        {
            ViewBag.AccountType = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Account Type").DataListId.ToString()).ToList();
            return View();
        }

        public JsonResult FilterAccount(string Date)
        {

            DateTime dateTime = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string monthe = dateTime.Month.ToString();
            string year = dateTime.Year.ToString();
            string Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
            //var data = _context.Tbl_SalaryStatement.Where(x => x.AccountDetails_Id == account_Id && x.Salarystatement_Month ==  monthe && x.Salarystatement_year == year).ToList();

            var accountdetails = _context.Tbl_AccountType.ToList();
            var salarystatement = _context.Tbl_SalaryStatement.ToList();
            var staffcategory = _context.Tbl_StaffCategory.ToList();

            //var result = "";
            //{
            //     result = GetCategoryfun(Date);
            //}

            string html = "";
            var category = "";
            html += "<div class='container'>";
            html += "<div class='row'>";
            html += "<div class='col-xs-2' style='margin-left:10px;'>";
            html += "<img src='/WebsiteImages/img/fevicon.png' alt='logo icon'width='100px;' style='display:inline-block;'>";
            html += "</div>";
            html += "<div class='col-xs-8' style='text-align:center'>";
            html += "<h3 class='title-of-labprintHead'>St.CarmelTeresa School</h3>";
            html += "</div>";
            html += "<div class='col-xs-2'>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            html += "<div><h2 style='text-align:center'>Salary Statement For the Month of " + Monthname + "-" + year + "</h2></div>";
            foreach (var item in accountdetails)
            {
                int Totalsalary = 0;
                int x = 1;
                var salarylist = salarystatement.Where(y => y.AccountDetails_Id == item.Account_TypeId && y.Salarystatement_Month == monthe && y.Salarystatement_year == year).ToList();

                var salarystatementlist = salarylist.OrderBy(j => j.StaffCategory_Id);

                if (salarylist.Count > 0)
                {
                    html += "<div class='col-md-12 col-sm-12 col-xs-12 table-responsive border-bottom'>";
                    html += "<div><h2 style='text-align:center'>" + item.Account_Typename + "</h2></div>";
                    html += "<table class='table table-bordered' style='margin-top:5px;'>";
                    html += "<thead>";
                    html += "<tr>";
                    html += "<th>S.No</th>";
                    html += "<th>Employee Category</th>";
                    html += "<th>Employers Designation</th>";
                    html += "<th>Employee Name</th>";
                    html += "<th>Employee Account No</th>";
                    html += "<th>Total</th>";
                    html += "</tr>";
                    html += "</thead>";
                    html += "<tbody>";
                    foreach (var obj in salarystatementlist)
                    {
                        var categorystaff = staffcategory.FirstOrDefault(z => z.Staff_Category_Id == obj.StaffCategory_Id);

                        if (categorystaff != null)
                        {
                            if (obj.Total_Salary != "0")
                            {
                                html += "<tr>";
                                html += "<td>" + x + "</td>";
                                if (category != categorystaff.Category_Name)
                                {
                                    html += "<td>" + categorystaff.Category_Name + "</td>";
                                }
                                else
                                {
                                    html += "<td></td>";
                                }
                                html += "<td>" + obj.Employers_Designation + "</td>";
                                html += "<td>" + obj.Employee_Name + "</td>";
                                html += "<td>" + obj.Employee_AccountNo + "</td>";
                                html += "<td>" + obj.Total_Salary + "</td>";
                                Totalsalary = Totalsalary + Convert.ToInt32(obj.Total_Salary);
                                html += "</tr>";
                                category = categorystaff.Category_Name;
                                x++;
                            }
                        }
                    }
                    category = "";
                    html += "<tr>";
                    html += "<td></td>";
                    html += "<td></td>";
                    html += "<td></td>";
                    html += "<td></td>";
                    html += "<td><b style='color:black'>Total</b></td>";
                    html += "<td><b style='color:black'>" + Totalsalary + "</b></td>";
                    html += "</tr>";
                    html += "</tbody>";
                    html += "</table>";
                    html += "</div>";
                }
                html += "<p style='page-break-after:always;'></p>";



            }



            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public string GetCategoryfun(string Date)
        {
            DateTime dateTime = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string monthe = dateTime.Month.ToString();
            string year = dateTime.Year.ToString();
            string Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
            var categorytype = _context.Tbl_StaffCategory.ToList();
            var staffdetails = _context.StafsDetails.ToList();
            var salarystatement = _context.Tbl_SalaryStatement.ToList();
            string html = "";
            html += "<div><h2 style='text-align:center'>Salary Statement For the Month of " + Monthname + "-" + year + "</h2></div>";

            foreach (var item in categorytype)
            {
                var staffdata = salarystatement.Where(c => c.StaffCategory_Id == item.Staff_Category_Id).ToList();

                if (staffdata != null)
                {
                    int Totalsalary = 0;
                    int x = 1;
                    html += "<div class='col-md-12 col-sm-12 col-xs-12 table-responsive border-bottom'>";
                    html += "<div><h2 style='text-align:center'>" + item.Category_Name + "</h2></div>";
                    html += "<table class='table table-bordered' style='margin-top:5px;'>";
                    html += "<thead>";
                    html += "<tr>";
                    html += "<th>S.No</th>";
                    html += "<th>Employers Designation</th>";
                    html += "<th>Employee Name</th>";
                    html += "<th>Employee Account No</th>";
                    html += "<th>Total</th>";
                    html += "</tr>";
                    html += "</thead>";
                    html += "<tbody>";
                    for (var i = 0; i < staffdata.Count(); i++)
                    {

                        {
                            html += "<tr>";
                            html += "<td>" + x + "</td>";
                            html += "<td>" + staffdata[i].Employers_Designation + "</td>";
                            html += "<td>" + staffdata[i].Employee_Name + "</td>";
                            html += "<td>" + staffdata[i].Employee_AccountNo + "</td>";
                            html += "<td>" + staffdata[i].Total_Salary + "</td>";
                            Totalsalary = Totalsalary + Convert.ToInt32(staffdata[i].Total_Salary);
                            html += "</tr>";
                            x++;
                        }

                    }
                    html += "<tr>";
                    html += "<td></td>";
                    html += "<td></td>";
                    html += "<td></td>";
                    html += "<td><b style='color:black'>Total</b></td>";
                    html += "<td><b style='color:black'>" + Totalsalary + "</b></td>";
                    html += "</tr>";
                    html += "</tbody>";
                    html += "</table>";
                    html += "</div>";
                }

            }


            return html;
        }

        public ActionResult PrintSalaryStatement(string date)
        {
            try
            {
                ViewBag.Date = date;
                //var result = GetCategoryfun(date);
                DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                string monthe = dateTime.Month.ToString();
                string year = dateTime.Year.ToString();
                string Monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
                var categorytype = _context.Tbl_StaffCategory.ToList();
                ViewBag.Categorytype = categorytype;
                var staffdetails = _context.StafsDetails.ToList();
                var salarystatement = _context.Tbl_SalaryStatement.ToList();
                ViewBag.salarystatement = salarystatement;
                List<Tbl_SalaryStatement> Tbl_SalaryStatement = new List<Tbl_SalaryStatement>();
                var result = new Printsalarystatement();
                //result.Tbl_StaffCategory = categorytype;
                foreach (var item in categorytype)
                {
                    var staffdata = staffdetails.Where(c => c.Category_Id == item.Staff_Category_Id).ToList();
                    if (staffdata != null)
                    {
                        foreach (var items in staffdata)
                        {
                            int employcode = Convert.ToInt32(items.EmployeeCode);
                            var salarylist = salarystatement.FirstOrDefault(y => y.Employee_Code == employcode && y.Salarystatement_Month == monthe && y.Salarystatement_year == year);
                            if (salarylist != null)
                            {
                                Tbl_SalaryStatement.Add(new Tbl_SalaryStatement
                                {
                                    Employee_Name = salarylist.Employee_Name,
                                    Employers_Designation = salarylist.Employers_Designation,
                                    Employee_AccountNo = salarylist.Employee_AccountNo,
                                    Total_Salary = salarylist.Total_Salary
                                });
                            }
                        }
                    }
                }
                ViewBag.SalaryList = salarystatement;

                //var customswitches = $" --header-spacing 10 --header-html \"{Url.Action("PrintHeader", "Staf", "http")}\" --footer-html \"{Url.Action("PrintFotter", "Staf", new { area = "" }, "http")}\"" + " --footer-line --footer-font-size \"12\" --footer-spacing 7 --footer-font-name \"Segoe UI\"";
                return new Rotativa.ViewAsPdf("PrintSalaryStatement")
                {
                    //CustomSwitches = customswitches
                };

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult PrintHeader()
        {
            return View();
        }

        public ActionResult PrintFotter()
        {
            return View();
        }

        public JsonResult GetSalaryStatement(string date)
        {
            var result = GetCategoryfun(date);
            if (result != null)
            {
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion


        #region Epf and Esi Statement

        public ActionResult EPFStatement()
        {
            var Epfstatement = _context.Tbl_EPFStatement.ToList();


            ViewBag.StaffDetails = _context.StafsDetails.ToList();

            return View();
        }


        public JsonResult FilterEPFData(string Date)
        {
            DateTime Attendacnedate = DateTime.ParseExact(Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string year = Attendacnedate.Year.ToString();
            string month = Attendacnedate.Month.ToString();


            var data = _context.Tbl_EPFStatement.Where(x => x.Added_Month == month && x.Added_Year == year).ToList();
            var UINNo = "";
            var uinnumber = "";
            List<Tbl_EPFStatement> tbl_EPFStatements = new List<Tbl_EPFStatement>();
            foreach (var item in data)
            {
                if (item.UIN == null)
                {
                    UINNo = uinnumber;
                }
                else
                {
                    UINNo = item.UIN;
                }

                tbl_EPFStatements.Add(new Tbl_EPFStatement
                {
                    UIN = UINNo,
                    Employee_Name = item.Employee_Name,
                    Gross_Wages = item.Gross_Wages,
                    Epf_Wages = item.Epf_Wages,
                    EPs_Wages = item.EPs_Wages,
                    EDLIWages = item.EDLIWages,
                    Employe_Contribution = item.Employe_Contribution,
                    Employer_Contribution = item.Employer_Contribution,
                    EPS_Pension = item.EPS_Pension,
                    NCP_Days = item.NCP_Days,
                    Refund_Advances = item.Refund_Advances

                });

            }
            return Json(tbl_EPFStatements, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportEPFStatement(string date)
        {
            DateTime Attendacnedate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string year = Attendacnedate.Year.ToString();
            string month = Attendacnedate.Month.ToString();


            var data = _context.Tbl_EPFStatement.Where(x => x.Added_Month == month && x.Added_Year == year).ToList();

            if (data.Count > 0)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                conn.Close();
                SqlCommand cmd = new SqlCommand("select Employee_Code,UIN,Employee_Name,Gross_Wages,Epf_Wages,EDLIWages,Employe_Contribution,Employer_Contribution,EPS_Pension,NCP_Days,Refund_Advances from Tbl_EPFStatement where Added_Month = " + month + " and Added_Year = '" + year + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();

                dt.TableName = "example";
                dt.AcceptChanges();
                da.Fill(dt);
                conn.Close();
                cmd.Dispose();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "sheet1");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ExcelFile.xlsx");
                    }
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No Data Present In the Date');location.replace('/Master/EPFStatement')</script>");
            }

        }

        //Export PFSummary
        public ActionResult ExportPfSummary(string date)
        {
            DateTime Attendacnedate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string year = Attendacnedate.Year.ToString();
            string month = Attendacnedate.Month.ToString();


            var data = _context.Tbl_AccountSummary.Where(x => x.Added_Month == month && x.Added_Year == year).ToList();

            if (data.Count > 0)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                conn.Close();
                SqlCommand cmd = new SqlCommand("select * from Tbl_AccountSummary where Added_Month = " + month + " and Added_Year = '" + year + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();

                dt.TableName = "example";
                dt.AcceptChanges();
                da.Fill(dt);
                conn.Close();
                cmd.Dispose();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "sheet1");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PFSummary.xlsx");
                    }
                }

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No Data Present In the Date');location.replace('/Staf/PFCalulationSummary')</script>");
            }

        }


        //ExportSalaryStatement
        public ActionResult ExportSalaryStatement(string date, int accountid)
        {
            DateTime Attendacnedate = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
            string year = Attendacnedate.Year.ToString();
            string month = Attendacnedate.Month.ToString();

            var data = _context.Tbl_SalaryStatement.Where(x => x.Salarystatement_Month == month && x.Salarystatement_year == year && x.AccountDetails_Id == accountid).ToList();

            if (data.Count > 0)
            {
                SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
                conn.Close();
                SqlCommand cmd = new SqlCommand("select * from Tbl_SalaryStatement where Salarystatement_Month = " + month + " and Salarystatement_year = '" + year + "' and AccountDetails_Id='" + accountid + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();

                dt.TableName = "example";
                dt.AcceptChanges();
                da.Fill(dt);
                conn.Close();
                cmd.Dispose();

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "sheet1");
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Salarystatement.xlsx");
                    }
                }

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('No Data Present In the Date');location.replace('/Staf/SalaryStatement')</script>");
            }
        }

        #endregion

        #region GeneratePaySlip

        public ActionResult GeneratePayslip()
        {

            if (Session["rolename"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["rolename"].ToString() == "Administrator")
            {

                var staffdetails = _context.StafsDetails.ToList();

                List<StaffDetailsVM> StaffDetailsVM = new List<StaffDetailsVM>();
                foreach (var item in staffdetails)
                {
                    int stafid = Convert.ToInt32(item.StafId);
                    var salarydetails = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == stafid);
                    if (salarydetails != null)
                    {
                        StaffDetailsVM.Add(new ViewModels.StaffDetailsVM
                        {
                            EmployeeCode = item.EmployeeCode,
                            Name = item.Name,
                            Designation = item.Employee_Designation,
                            NetPay = salarydetails.Salary_Amount,
                            StafId = item.StafId

                        });
                    }
                }
                ViewBag.stafdetails = StaffDetailsVM;

                ViewBag.AccountSummary = _context.Tbl_AccountSummary.ToList();
            }
            else
            {
                var stafids = Convert.ToInt32(Session["Employeeid"]);
                var staffdetails = _context.StafsDetails.Where(x => x.StafId == stafids).ToList();

                List<StaffDetailsVM> StaffDetailsVM = new List<StaffDetailsVM>();
                foreach (var item in staffdetails)
                {
                    int stafid = Convert.ToInt32(item.EmployeeCode);
                    var salarydetails = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == stafid);
                    if (salarydetails != null)
                    {
                        StaffDetailsVM.Add(new ViewModels.StaffDetailsVM
                        {
                            EmployeeCode = item.EmployeeCode,
                            Name = item.Name,
                            Designation = item.Employee_Designation,
                            NetPay = salarydetails.Salary_Amount,

                        });
                    }
                }
                ViewBag.stafdetails = StaffDetailsVM;

                ViewBag.AccountSummary = _context.Tbl_AccountSummary.ToList();
            }


            return View();
        }

        public JsonResult PaySlipById(int Id)
        {

            var staffdata = _context.Tbl_AccountSummary.Where(x => x.Staff_Id == Id).ToList();
            var salarydata = _context.Tbl_StaffSalary.FirstOrDefault(x => x.Staff_ID == Id);
            var basicpaydetails = _context.Tbl_BasicPayDetails.ToList();
            var basicmasterdetails = _context.Tbl_BasicpayMaster.ToList();
            var DA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 1);
            var DA_master = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 1);
            var HRA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 4);
            var HRA_master = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 4);
            var CCA_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 3);
            var CCA_Master = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 3);
            var ESI_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 5);
            var ESI_Master = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 5);
            var PF_Percentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 2);
            var PF_Master = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 2);
            var Other_Allowancepercentage = basicpaydetails.FirstOrDefault(x => x.BasicPay_Id == 8);
            var Other_Allowancemaster = basicmasterdetails.FirstOrDefault(x => x.BasicPay_MasterId == 8);

            string html = "";
            html += "<div class='modal fade' role='dialog'>";
            html += "<div class='modal-dialog'>";
            html += "<div class='modal-content'>";
            html += "<div class='modal-header'>";
            html += "<button type='button' class='close' data-dismiss='modal'>&times;</button>";
            html += "<h4 class='modal - title'>Payment Details</h4>";
            html += "</div>";
            for (int i = 0; i < staffdata.Count(); i++)
            {
                int totaldeductions = staffdata[i].Gross - staffdata[i].Net_Pay;

                html += "<div class='modal-body' style='height:530px;overflow-y:scroll;'>";
                html += "<h2 class='well - sm'>Date of Payment : " + staffdata[i].Added_Date + "</h2>";
                html += "<p style='font - size: 15px; padding - left:10px; padding - right:10px; text - align:left; '>How much Paid : <span>RS " + staffdata[i].Net_Pay + "/-</span></p>";
                html += "<p style='font - size: 15px; padding - left:10px; padding - right:10px; text - align:left; '>Attendance : <span>20%</span></p>";
                html += "<p style='font - size: 15px; padding - left:10px; padding - right:10px; text - align:left; '>Mode of payment : <span> Cash </span> | <span> Cheque </span></p>";
                html += "<table class='table table-bordered' style='margin-top:10px;'>";
                html += "<tr style='background-color: #1ABB9C;color: #fff;'>";
                html += "<td>Gross Earnings</td>";
                html += "<td>Amount</td>";
                html += "<td>Deductions</td>";
                html += "<td>Amount</td>";
                html += "<td>Net Pay</td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>Basicpay</td>";
                html += "<td>" + staffdata[i].Basic_Salary + "</td>";
                html += "<td>EPF</td>";
                html += "<td>" + staffdata[i].PF + "</td>";
                html += "<td></td>";
                //html += "<td>"+staffdata[i].Net_Pay+"/-</td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>" + DA_master.Basicpay_Name + "" + DA_Percentage.Basic_Amount + "%</td>";
                html += "<td>" + staffdata[i].DA + "</td>";
                html += "<td>ESI</td>";
                html += "<td>" + staffdata[i].ESI + "</td>";
                html += "<td></td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>" + HRA_master.Basicpay_Name + "" + HRA_Percentage.Basic_Amount + "%</td>";
                html += "<td>" + staffdata[i].HRA + "</td>";
                html += "<td>Professinal Tax</td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>" + CCA_Master.Basicpay_Name + "" + CCA_Percentage.Basic_Amount + "</td>";
                html += "<td>" + staffdata[i].CCA + "</td>";
                html += "<td>Others</td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>" + Other_Allowancemaster.Basicpay_Name + "" + Other_Allowancepercentage.Basic_Amount + "</td>";
                html += "<td>" + staffdata[i].OtherALlowance + "</td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "<td></td>";
                html += "</tr>";
                html += "<tr>";
                html += "<td>Total Salary</td>";
                html += "<td>" + staffdata[i].Gross + "</td>";
                html += "<td>Total Deductions</td>";
                html += "<td>" + totaldeductions + "</td>";
                html += "<td>" + staffdata[i].Net_Pay + "</td>";
                html += "</tr>";
                html += "<tr>";
                html += " <td colspan='4' style='text - align:left; '>Amount in Rupees : Twenty Thousand Ruppes Only</td>";
                html += "</tr>";
                html += "</div>";


            }
            html += "<div class='modal-footer'>";
            html += "</div>";
            html += "</div>";
            html += "</div>";
            var data = html;
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region PrintPayslip

        public ActionResult PrintPaySlip(int emploeeid)
        {
            ViewBag.Employeecode = emploeeid;
            return View();
        }


        public JsonResult GetPayslipById(string id, string fromdate, string todate)
        {
            var Fromdate = fromdate.Split('/');
            var Todate = todate.Split('/');

            var fdate = (Fromdate[1].TrimStart('0'));
            var tdate = (Todate[1].TrimStart('0'));
            var data = _context.Tbl_AccountSummary.Where(x => x.Staff_Id.ToString() == id).ToList();
            List<Payrollviewmodel> Tbl_AccountSummary = new List<Payrollviewmodel>();
            var staffdetails = _context.StafsDetails.FirstOrDefault(x => x.StafId.ToString() == id);
            if (data != null)
            {
                foreach (var item in data)
                {
                    DateTime dateTime = DateTime.ParseExact(item.Added_Date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                    string monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
                    var date = item.Added_Date.Split('/');
                    if (Convert.ToInt32(date[1].TrimStart('0')) >= Convert.ToInt32(fdate) && Convert.ToInt32(date[1].TrimStart('0')) <= Convert.ToInt32(tdate))
                    {
                        Tbl_AccountSummary.Add(new Payrollviewmodel
                        {
                            Staff_Id = item.Staff_Id,
                            Summary_Id = item.Summary_Id,
                            Staff_Name = item.Staff_Name,
                            NetPay = item.NetPay,
                            PF = item.PF,
                            Basic_Salary = item.Basic_Salary,
                            DA = item.DA,
                            Professional_Tax = item.Professional_Tax,
                            Added_Date = item.Added_Date,
                            Added_Month = item.Added_Month,
                            Added_Year = item.Added_Year,
                            Added_Day = item.Added_Day,
                            Employee_Contribution = item.Employee_Contribution,
                            Employer_Contribution = item.Employer_Contribution,
                            Net_Pay = item.Net_Pay,
                            Attendence_Percentage = item.Attendence_Percentage,
                            ESI = item.ESI,
                            Gross = item.Gross,
                            Total_Salary = item.Total_Salary,
                            LOP = item.LOP,
                            CCA = item.CCA,
                            HRA = item.HRA,
                            OtherALlowance = item.OtherALlowance,
                            NoOfdayspresent = item.NoOfdayspresent,
                            TotalPercentage = item.TotalPercentage,
                            Numberinwords = NumberToWords(item.Net_Pay),
                            Bankname = staffdetails.Bank_Name,
                            AccountNo = staffdetails.Account_No,
                            Designation = staffdetails.Employee_Designation,
                            UANNo = staffdetails.UIN == null ? string.Empty : staffdetails.UIN,
                            MonthName = monthname,
                            year = dateTime.Year
                        });
                    }
                }
                return Json(Tbl_AccountSummary, JsonRequestBehavior.AllowGet);
            }
            else
            {

                return Json(null, JsonRequestBehavior.AllowGet);
            }

        }

        #endregion


        #region Account Login

        public ActionResult CreateEmployeeLogin()
        {
            if (Session["rolename"].ToString() == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["rolename"].ToString() == "Administrator")
            {
                var stafflist = _context.StafsDetails.ToList();
                ViewBag.Stafflist = stafflist;

                var pagename = "Staff Login";
                var editpermission = "Create_permission";

                var per = CheckCreatepermission(pagename, editpermission);

                ViewBag.Permission = per;

                List<StaffLogin> StaffLogin = new List<StaffLogin>();
                var stafflogin = _context.Tbl_UserManagement.Where(x => x.Description != "Student registration").ToList();
                foreach (var item in stafflogin)
                {
                    if (item.UserName != null)
                    {
                        var stafdata = stafflist.FirstOrDefault(x => x.Email == item.UserName);
                        if(stafdata != null)
                        {
                            StaffLogin.Add(new StaffLogin
                            {
                                Username = item.UserName,
                                StaffName = stafdata.Name,
                                Password = item.Password,
                                Description = item.Description,
                                id = stafdata.StafId,
                            });
                        }
                       
                    }
                    
                }
                ViewBag.StaffLogins = StaffLogin;

                var roleStore = new RoleStore<IdentityRole>(_context);
                var roleMngr = new RoleManager<IdentityRole>(roleStore);
                List<SelectListItem> roleslist = new List<SelectListItem>();
                var roles = roleMngr.Roles.ToList();
                roleslist.Add(new SelectListItem { Text = "--Select Role--", Value = "0" });
                foreach (var item in roles)
                {
                    roleslist.Add(new SelectListItem { Text = item.Name, Value = item.Id });
                }

                ViewBag.Roles = roleslist;

                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public JsonResult CreateStaff(int id, string Rolename)
        {
            if (Session["rolename"] == null)
            {
                return Json("Signout");
            }
            try
            {
                if (Session["rolename"].ToString() == "Administrator")
                {
                    var stafdata = _context.StafsDetails.FirstOrDefault(x => x.StafId == id);
                    if (stafdata != null)
                    {

                        if (stafdata.Email == null || stafdata.Email == "")
                        {
                            return Json("Email", JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            var tblusermanagement = _context.Tbl_UserManagement.FirstOrDefault(x => x.UserName == stafdata.Email);
                            if (tblusermanagement != null)
                            {
                                return Json("AlreadyThere", JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                Tbl_UserManagementViewModel tbl_UserManagementViewModel = new Tbl_UserManagementViewModel();
                                tbl_UserManagementViewModel.Description = Rolename + "Registration";
                                tbl_UserManagementViewModel.Email = stafdata.Email;
                                tbl_UserManagementViewModel.UserName = stafdata.Email;
                                tbl_UserManagementViewModel.UserRole = Rolename;
                                tbl_UserManagementViewModel.UserId = stafdata.StafId;
                                const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
                                StringBuilder stringBuilder = new StringBuilder();
                                Random rnd = new Random();
                                int length = 8;
                                while (0 < length--)
                                {
                                    stringBuilder.Append(valid[rnd.Next(valid.Length)]);
                                }
                                var password = stringBuilder.ToString();
                                tbl_UserManagementViewModel.Password = password;

                                var userid = CreateEmployee(tbl_UserManagementViewModel);

                                StafsDetails stafsDetails = new StafsDetails();
                                #region
                                stafsDetails.StafId = stafdata.StafId;
                                stafsDetails.UIN = stafdata.UIN;
                                stafsDetails.Date = stafdata.Date;
                                stafsDetails.Name = stafdata.Name;
                                stafsDetails.Gender = stafdata.Gender;
                                stafsDetails.AgeInWords = stafdata.AgeInWords;
                                stafsDetails.DOB = stafdata.DOB;
                                stafsDetails.POB = stafdata.POB;
                                stafsDetails.Nationality = stafdata.Nationality;
                                stafsDetails.Religion = stafdata.Religion;
                                stafsDetails.Qualification = stafdata.Qualification;
                                stafsDetails.WorkExperience = stafdata.WorkExperience;
                                stafsDetails.MotherTongue = stafdata.MotherTongue;
                                stafsDetails.Category = stafdata.Category;
                                stafsDetails.BloodGroup = stafdata.BloodGroup;
                                stafsDetails.MedicalHistory = stafdata.MedicalHistory;
                                stafsDetails.Address = stafdata.Address;
                                stafsDetails.Contact = stafdata.Contact;
                                stafsDetails.Email = stafdata.Email;
                                stafsDetails.BesicSallery = stafdata.BesicSallery;
                                stafsDetails.PerksSallery = stafdata.PerksSallery;
                                stafsDetails.GrossSallery = stafdata.GrossSallery;
                                stafsDetails.FatherOrHusbandName = stafdata.FatherOrHusbandName;
                                stafsDetails.MothersName = stafdata.MothersName;
                                stafsDetails.MariedStatus = stafdata.MariedStatus;
                                stafsDetails.Children = stafdata.Children;
                                stafsDetails.BesicSallery1 = stafdata.BesicSallery1;
                                stafsDetails.PerksSallery1 = stafdata.PerksSallery1;
                                stafsDetails.GrossSallery1 = stafdata.GrossSallery1;
                                stafsDetails.Caste = stafdata.Caste;
                                stafsDetails.DateofReliving = stafdata.DateofReliving;
                                stafsDetails.LastOrganizationofEmployment = stafdata.LastOrganizationofEmployment;
                                stafsDetails.NoofYearsattheLastAssignment = stafdata.NoofYearsattheLastAssignment;
                                stafsDetails.AdharNo = stafdata.AdharNo;
                                stafsDetails.AdharFile = stafdata.AdharFile;
                                stafsDetails.PanNo = stafdata.PanNo;
                                stafsDetails.PanFile = stafdata.PanFile;
                               // stafsDetails.Staffsignature = stafsDetails.Staffsignature;
                                stafsDetails.BankACNo = stafdata.BankACNo;
                                stafsDetails.RelievingLetter = stafdata.RelievingLetter;
                                stafsDetails.PerformanceLetter = stafdata.PerformanceLetter;
                                stafsDetails.File = stafdata.File;
                                stafsDetails.OtherDetails = stafdata.OtherDetails;
                                stafsDetails.EmpId = stafdata.EmpId;
                                stafsDetails.OtherLanguages = stafdata.OtherLanguages;
                                stafsDetails.EmpDate = stafdata.EmpDate;
                                stafsDetails.FormalitiesCheck = stafdata.FormalitiesCheck;
                                stafsDetails.Designation = stafdata.Designation;
                                stafsDetails.EmployeeCode = stafdata.EmployeeCode;
                                stafsDetails.Bank_Name = stafdata.Bank_Name;
                                stafsDetails.Account_No = stafdata.Account_No;
                                stafsDetails.IFSC_Code = stafdata.IFSC_Code;
                                stafsDetails.Employee_Designation = stafdata.Employee_Designation;
                                stafsDetails.Employee_AccountId = stafdata.Employee_AccountId;
                                stafsDetails.Employee_AccountName = stafdata.Employee_AccountName;
                                stafsDetails.Category_Id = stafdata.Category_Id;
                                stafsDetails.Staff_CategoryName = stafdata.Staff_CategoryName;
                                stafsDetails.UserId = userid;

                                #endregion

                                Staffupdate stafobj = new Staffupdate()
                                {
                                    UserId = userid
                                };

                                _context.Entry(stafdata).CurrentValues.SetValues(stafsDetails);
                                _context.SaveChanges();

                                return Json("Success", JsonRequestBehavior.AllowGet);
                            }
                        }


                        //stafcreation

                    }
                    else
                    {

                        return Json("Fail", JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json("Signout");
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public string CreateEmployee(Tbl_UserManagementViewModel tbl_UserManagementViewModel)
        {
            Tbl_UserManagement tbl_UserManagement = new Tbl_UserManagement
            {
                Description = tbl_UserManagementViewModel.Description,
                Email = tbl_UserManagementViewModel.Email,
                Password = tbl_UserManagementViewModel.Password,
                UserName = tbl_UserManagementViewModel.UserName
            };
            var usermanagement = _context.Tbl_UserManagement.Add(tbl_UserManagement);
            _context.SaveChanges();
            if (usermanagement.UserId > 0)
            {
                Random rnd = new Random();
                int rndnumber = rnd.Next(1, 999999);
                UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
                PasswordHasher PasswordHash = new PasswordHasher();
                //if (UserManager.FindByEmail(tbl_UserManagementViewModel.Email) == null)
                //{
                ApplicationUser admin = new ApplicationUser
                {
                    UserName = usermanagement.UserName,
                    Email = usermanagement.Email,
                    PasswordHash = PasswordHash.HashPassword(usermanagement.Password),
                    UserId = usermanagement.UserId,
                    PhoneNumber = null,
                    IsEnable = true
                };

                IdentityResult result = UserManager.Create(admin);
                if (result.Succeeded == true)
                {
                    var data = UserManager.FindByName(usermanagement.UserName); // Name based user role assignment
                    if (data != null)
                    {
                        string id = data.Id;
                        UserManager.AddToRole(id, tbl_UserManagementViewModel.UserRole);
                    }
                }

                // }
            }
            return usermanagement.UserId.ToString();
        }

        #endregion

        #region Int To string

        public static string NumberToWords(int number)
        {
            if (number == 0)
            {
                return "Zero";
            }

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " Million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        #endregion

        #region Staffattendance

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

        public ActionResult ViewStaffAttendence()
        {

            if (Session["rolename"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["rolename"].ToString() == "Administrator")
            {
                var stafflist = _context.StafsDetails.ToList();
                ViewBag.Staff_List = stafflist;
                ViewBag.TotalCount = stafflist.Count();
                ViewBag.Rolename = Session["rolename"].ToString();

            }
            else
            {
                var stafids = Convert.ToInt32(Session["Employeeid"]);

                var staflist = _context.StafsDetails.Where(x => x.StafId == stafids).ToList();
                ViewBag.Staff_List = staflist;
                ViewBag.TotalCount = 1;
                ViewBag.Rolename = Session["rolename"].ToString();
            }

            return View();
        }


        public ActionResult StudentAttendanceList()
        {
            var data = _context.Students.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public ActionResult ViewStudentAttendence()
        {

            if (Session["rolename"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (Session["rolename"].ToString() == "Administrator")
            {
                var stafflist = _context.StafsDetails.ToList();
                ViewBag.Staff_List = stafflist;
                ViewBag.TotalCount = stafflist.Count();
                ViewBag.Rolename = Session["rolename"].ToString();

            }
            else
            {
                var stafids = Convert.ToInt32(Session["Employeeid"]);

                var staflist = _context.StafsDetails.Where(x => x.StafId == stafids).ToList();
                ViewBag.Staff_List = staflist;
                ViewBag.TotalCount = 1;
                ViewBag.Rolename = Session["rolename"].ToString();
            }






            return View();
        }
        public JsonResult FilterStaffAttendence(int stafid, string fromdate, string todate)
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
            //var employeid = Convert.ToString(stafid);
            //List<StafsDetails> staffdetails = new List<StafsDetails>();


            var staffdata = _context.StafsDetails.FirstOrDefault(x => x.StafId == stafid);
            var data = _context.Tbl_StaffAttendance.Where(x => x.Staff_Id == staffid && x.Attendence_Month == month && x.Attendence_Year == year).ToList();
            data = data.Where(x => x.Attendence_Day >= fromday && x.Attendence_Day <= today).ToList();
            var fullpresentday = data.Where(x => x.Staff_Id == staffid && x.Mark_FullDayPresent == "A").Count();
            var halpresentday = data.Where(x => x.Staff_Id == staffid && x.Mark_FullDayPresent == "½P").Count();
            double halfdayabasent = (double)halpresentday / 2;
            double totalabsent = fullpresentday + halfdayabasent;
            double totalpresent = today - totalabsent;
            double Noofworkingdays = totalpresent * 100;
            double Totalattendence = Math.Round(Noofworkingdays / today);



            var html = "";

            html += "<table class='table table-bordered'>";
            html += "<thead>";
            html += "<tr>";
            html += "<th style='background-color:#41BDE2'>StaffId</th>";
            html += "<th style='background-color:#41BDE2'>Name</th>";


            for (int j = 1; j <= today; j++)
            {
                html += "<th style='background-color:#41BDE2'>" + j + "</th>";

            }

            html += "<th style='background-color:#41BDE2'>Total</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            html += "<tr>";
            html += "<td>" + staffdata.EmployeeCode + "</td>";
            html += "<td>" + staffdata.Name + "</td>";
            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Mark_FullDayPresent == "P")
                {

                    html += "<td>&#10004;</td>";
                }
                else if (data[i].Mark_FullDayPresent == "A")
                {
                    html += "<td class='color_of_absent'>&#10006;</td>";
                }
                else
                {
                    html += "<td>" + data[i].Mark_FullDayPresent + "</td>";

                }
            }

            html += "<td>" + Totalattendence + "%</td>";
            html += "</tr>";
            html += "</tbody>";
            html += "</table>";
            //ViewBag.data = html;


            return Json(html, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ViewFullStaffaAttendance(string fromdate, string todate)
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
            html += "<table class='table table-bordered'>";
            html += "<thead>";
            html += "<tr>";
            html += "<th style='background-color:#41BDE2'>S.No</th>";
            html += "<th style='background-color:#41BDE2'>StaffId</th>";
            html += "<th style='background-color:#41BDE2'>Name</th>";


            for (int j = 1; j <= To_Day; j++)
            {
                html += "<th style='background-color:#41BDE2'>" + j + "</th>";

            }

            html += "<th style='background-color:#41BDE2'>Total</th>";
            html += "</tr>";
            html += "</thead>";
            html += "<tbody>";
            int a = 1;
            foreach (var item in stafflist)
            {
                //if (item.EmployeeCode != null)
                {
                    int employcode = item.StafId;
                    var PerdayAtte = stafattendance.Where(x => x.Staff_Id == employcode && x.Attendence_Month == month && x.Attendence_Year == year).ToList();
                    PerdayAtte = PerdayAtte.Where(x => x.Attendence_Day >= fromday && x.Attendence_Day <= To_Day).ToList();

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
                        html += "<td>" + a + "</td>";
                        html += "<td>" + item.EmployeeCode + "</td>";
                        html += "<td>" + item.Name + "</td>";
                        for (int i = 0; i < PerdayAtte.Count; i++)
                        {
                            if (PerdayAtte[i].Mark_FullDayPresent == "P")
                            {

                                html += "<td>&#10004;</td>";
                            }
                            else if (PerdayAtte[i].Mark_FullDayPresent == "A")
                            {
                                html += "<td class='color_of_absent'>&#10006;</td>";
                            }
                            else
                            {
                                html += "<td>" + PerdayAtte[i].Mark_FullDayPresent + "</td>";

                            }
                        }

                        html += "<td>" + Totalattendence + "%</td>";
                        html += "</tr>";

                    }

                }
                a++;

            }
            html += "</tbody>";
            html += "</table>";
            return Json(html, JsonRequestBehavior.AllowGet);
        }


        //public JsonResult AddStaffAttendence(StaffAttendenceViewmodel staffAttendenceViewmodel)
        //[HttpPost]
        //public JsonResult AddStaffAttendence(List<Tbl_StaffAttendance> rowData)
        //{
        //    var attendanceDate = rowData.Select(x=>x.Attendence_Date).FirstOrDefault();
        //    bool hasMatchingRecord = _context.Tbl_StaffAttendance.Any(c => c.Attendence_Date == attendanceDate);
        //    if (hasMatchingRecord)
        //    {
        //        return Json(new { success = false, errormsg = "Attendance is only taken once" });
        //    }
        //    foreach (var item in rowData)
        //    {
        //        var entity = new Tbl_StaffAttendance
        //        {
        //            Staff_Name = item.Staff_Name,
        //            Mark_CL = item.Mark_CL,
        //            Mark_Other = item.Mark_Other,
        //            Mark_ML = item.Mark_ML,
        //            Mark_L = item.Mark_L,
        //            Mark_FullDayAbsent = item.Mark_FullDayAbsent,
        //            Mark_HalfDayAbsent = item.Mark_HalfDayAbsent,
        //            Staff_Id = item.Staff_Id,
        //            Attendence_Date = item.Attendence_Date,
        //            Attendence_Day = item.Attendence_Day,

        //        };
        //        _context.Tbl_StaffAttendance.Add(entity);

        //    }
        //    _context.SaveChanges();
        //    return Json(new { success = true });

        //}

        [HttpPost]
        public JsonResult AddStaffAttendence(List<Tbl_StaffAttendance> rowData)
        {
            if (rowData == null || rowData.Count == 0)
            {
                return Json(new { success = false, errormsg = "No attendance data received" });
            }

            var attendanceDate = rowData.Select(x => x.Attendence_Date).FirstOrDefault();

            foreach (var item in rowData)
            {
                // Check if record already exists for Staff_Id + Attendence_Date
                var existingRecord = _context.Tbl_StaffAttendance
                    .FirstOrDefault(c => c.Staff_Id == item.Staff_Id && c.Attendence_Date == item.Attendence_Date);

                if (existingRecord != null)
                {
                    // UPDATE existing record
                    existingRecord.Mark_CL = item.Mark_CL;
                    existingRecord.Mark_Other = item.Mark_Other;
                    existingRecord.Mark_ML = item.Mark_ML;
                    existingRecord.Mark_L = item.Mark_L;
                    existingRecord.Mark_FullDayAbsent = item.Mark_FullDayAbsent;
                    existingRecord.Mark_HalfDayAbsent = item.Mark_HalfDayAbsent;
                    existingRecord.Staff_Name = item.Staff_Name;
                    existingRecord.Attendence_Day = item.Attendence_Day;
                    // You can update other fields if needed
                }
                else
                {
                    // INSERT new record
                    var newEntity = new Tbl_StaffAttendance
                    {
                        Staff_Name = item.Staff_Name,
                        Mark_CL = item.Mark_CL,
                        Mark_Other = item.Mark_Other,
                        Mark_ML = item.Mark_ML,
                        Mark_L = item.Mark_L,
                        Mark_FullDayAbsent = item.Mark_FullDayAbsent,
                        Mark_HalfDayAbsent = item.Mark_HalfDayAbsent,
                        Staff_Id = item.Staff_Id,
                        Attendence_Date = item.Attendence_Date,
                        Attendence_Day = item.Attendence_Day,
                    };
                    _context.Tbl_StaffAttendance.Add(newEntity);
                }
            }

            _context.SaveChanges();

            return Json(new { success = true });
        }


        public ActionResult PartialShowStaffAttendance(int? staffId, DateTime? fromDate, DateTime? toDate)
        {
            List<Tbl_StaffAttendance> staffAttendanceList = new List<Tbl_StaffAttendance>();
            staffAttendanceList = GetStaffAttendences(staffId, fromDate, toDate);
            return PartialView("_StaffAttendanceTable", staffAttendanceList);
        }
        public List<Tbl_StaffAttendance> GetStaffAttendences(int? staffId, DateTime? fromDate, DateTime? toDate)
        {
            List<Tbl_StaffAttendance> staffAttendanceList = new List<Tbl_StaffAttendance>();

            var query = _context.Tbl_StaffAttendance.AsQueryable();

            if (staffId.HasValue)
            {
                query = query.Where(s => s.Staff_Id == staffId.Value);
            }

             staffAttendanceList = query.ToList(); // First fetch the data

            if (fromDate.HasValue && toDate.HasValue)
            {
                staffAttendanceList = staffAttendanceList
                    .Where(s =>
                    {
                        DateTime attendenceDate;
                        return DateTime.TryParse(s.Attendence_Date, out attendenceDate) && attendenceDate >= fromDate.Value && attendenceDate <= toDate.Value;
                    })
                    .ToList();
            }
            else if (fromDate.HasValue)
            {
                staffAttendanceList = staffAttendanceList
                    .Where(s =>
                    {
                        DateTime attendenceDate;
                        return DateTime.TryParse(s.Attendence_Date, out attendenceDate) && attendenceDate.Date == fromDate.Value.Date;
                    })
                    .ToList();
            }
            
            return staffAttendanceList;

        }
        public JsonResult getAttendace(DateTime? fromDate)
        {
            List<Tbl_StaffAttendance> staffAttendanceList = new List<Tbl_StaffAttendance>();
            staffAttendanceList = GetStaffAttendences(null, fromDate, null);
            return Json(new { success = true, staffAttendanceList }, JsonRequestBehavior.AllowGet);
        }
        #endregion
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
    public class Staffupdate
    {
        public string UserId { get; set; }
    }

    public class StaffLogin
    {
        public string StaffName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public int id { get; set; }

    }

    public class PayrollBasicDetails
    {
        public int NetPay { get; set; }

        public int PF { get; set; }

        public int BasicSalary { get; set; }

        public string Staff_Name { get; set; }

        public string Staff_Id { get; set; }

        public int DA { get; set; }

        public string NumberinWords { get; set; }

        public string Year { get; set; }

        public string MonthName { get; set; }



        public int Basic_Salary { get; set; }


        public int Professional_Tax { get; set; }

        public string Added_Date { get; set; }

        public string Added_Month { get; set; }

        public string Added_Year { get; set; }

        public string Added_Day { get; set; }

        public int Employee_Contribution { get; set; }

        public int Employer_Contribution { get; set; }

        public int Net_Pay { get; set; }

        public int Attendence_Percentage { get; set; }

        public int Gross { get; set; }

        public int Total_Salary { get; set; }

        public int ESI { get; set; }

        public double LOP { get; set; }

        public double HRA { get; set; }

        public double CCA { get; set; }

        public double OtherAllowance { get; set; }

        public string Noofdayspresent { get; set; }

        public double TotalPercentage { get; set; }

        public int Category_ID { get; set; }

    }

    public class Printsalarystatement
    {
        public Tbl_StaffCategory Tbl_StaffCategory { get; set; }
        public string Employers_Designation { get; set; }

        public string Employee_Name { get; set; }

        public int Employee_Code { get; set; }

        public string Employee_AccountNo { get; set; }

        public int Total_Salary { get; set; }

        public int AccountDetails_Id { get; set; }

        public string Account_Details { get; set; }

        public string Salarystatement_Month { get; set; }

        public string Salarystatement_year { get; set; }

        public string SalaryStatement_Date { get; set; }
    }

    public class Payrollviewmodel
    {
        public int Summary_Id { get; set; }
        public int Staff_Id { get; set; }
        public string Staff_Name { get; set; }
        public int NetPay { get; set; }
        public int PF { get; set; }
        public int Basic_Salary { get; set; }
        public int DA { get; set; }
        public int Professional_Tax { get; set; }
        public string Added_Date { get; set; }
        public string Added_Month { get; set; }
        public string Added_Year { get; set; }
        public string Added_Day { get; set; }
        public int Employee_Contribution { get; set; }
        public int Employer_Contribution { get; set; }
        public int Net_Pay { get; set; }
        public int Attendence_Percentage { get; set; }
        public int ESI { get; set; }
        public int Gross { get; set; }
        public int Total_Salary { get; set; }
        public double LOP { get; set; }
        public int CCA { get; set; }
        public int HRA { get; set; }
        public int OtherALlowance { get; set; }
        public string NoOfdayspresent { get; set; }
        public int TotalPercentage { get; set; }
        public string Numberinwords { get; set; }
        public string Designation { get; set; }
        public string Bankname { get; set; }
        public string UANNo { get; set; }
        public string AccountNo { get; set; }
        public string MonthName { get; set; }
        public int year { get; set; }
    }

}