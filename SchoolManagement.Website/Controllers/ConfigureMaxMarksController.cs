using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class ConfigureMaxMarksController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: ConfigureMaxMarks
        public ActionResult DataList()
        {
            ViewBag.Subject_Id = _context.Tbl_ConfigureMaxMarks.ToList();
            ViewBag.Table_Name = _context.Tbl_ConfigureMaxMarks.ToList();
            ViewBag.MaximumMarks = _context.Tbl_ConfigureMaxMarks.ToList();
            ViewBag.Class = _context.Tbl_ConfigureMaxMarks.ToList();
                

            List<Tbl_ConfigureMaxMarks> MaxMarks = _context.Tbl_ConfigureMaxMarks.ToList();

                    
            return View();
        }
    }
}