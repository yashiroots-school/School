using SchoolManagement.Website.Models;
using System;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class StudentLoginController : Controller
    {
        public StudentLoginController()
        {

        }
        // GET: StudentLogin
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                return RedirectToAction("Dashboard", "Dashboard");
            }
            catch (Exception )
            {
                ModelState.AddModelError("", "Please Try Again.");
                return View(model);
            }
        }
    }
}