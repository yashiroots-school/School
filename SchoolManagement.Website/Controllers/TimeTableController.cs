using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class TimeTableController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: TimeTable
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddPeriodTimings()
        {
            var allStaf = _context.StafsDetails.ToList();
            ViewBag.allStaf = allStaf;
            var Time = _context.TimeSettings.ToList();
            ViewBag.GetTime = Time;

            var Subject1 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject = Subject1;

            var Subject2 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject2 = Subject2;

            var Subject3 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject3 = Subject3;

            var Subject4 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject4 = Subject4;

            var Subject5 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject5 = Subject5;

            var Subject6 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject6 = Subject6;

            ViewBag.RoomNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "RoomNo").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            ViewBag.Subject = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Group").DataListId.ToString()).ToList();

            return View();
        }
        [HttpPost]
        public ActionResult AddPeriodTiming(TimeSettings TimeSettings)
        {
            if (TimeSettings != null)
            {
                _context.TimeSettings.Add(TimeSettings);
                _context.SaveChanges();
            }

            return RedirectToAction("AddPeriodTimings");
            
        }

        [HttpPost]
        public ActionResult AddPeriod(PeriodSchedule PeriodSchedule)
        {
            if (PeriodSchedule != null)
            {
                _context.PeriodSchedule.Add(PeriodSchedule);
                _context.SaveChanges();
            }

            return RedirectToAction("AddPeriodTimings");

        }

        public ActionResult ViewPeriodTimings()
        {
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            var Time = _context.TimeSettings.ToList();
            ViewBag.GetTime = Time;

            var Subject1 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject = Subject1;

            var Subject2 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject2 = Subject2;

            var Subject3 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject3 = Subject3;

            var Subject4 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject4 = Subject4;

            var Subject5 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject5 = Subject5;

            var Subject6 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject6 = Subject6;

            return View ();
        }

        public ActionResult ManagePeriodTimings()
        {
            ViewBag.RoomNo = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "RoomNo").DataListId.ToString()).ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Section").DataListId.ToString()).ToList();
            var Time = _context.TimeSettings.ToList();
            ViewBag.GetTime = Time;

            var Subject1 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject = Subject1;

            var Subject2 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject2 = Subject2;

            var Subject3 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject3 = Subject3;

            var Subject4 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject4 = Subject4;

            var Subject5 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject5 = Subject5;

            var Subject6 = _context.PeriodSchedule.ToList();
            ViewBag.AllSubject6 = Subject6;
            return View();
        }

        //Get Id for Edit
        public JsonResult GetPeriodDataById(int id)
        {
            var data = _context.PeriodSchedule.FirstOrDefault(x => x.Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

       
        public JsonResult GetPeriodData(string section)
        {
            var result = new List<PeriodSchedule>();
            result = _context.PeriodSchedule.ToList();
            if (!string.IsNullOrEmpty(section))
            {
                result = result.Where(x => x.Class == section).ToList();
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}