using ClosedXML.Excel;
using SchoolManagement.Data.Models;
using SchoolManagement.Data.Test;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context = new ApplicationDbContext();

        // GET: Test
        public ActionResult Index()
        {
            List<Student> studentlist = _context.Students.ToList();

            ViewBag.SchoolType = GetschoolTypeList("0");


            ViewBag.Batches = GetBactesList("0");

            DataTable dt = new DataTable("MyTable");


            ViewBag.Columns = dt.Columns;
            ViewBag.Rows = dt.Rows;

            return View();
        }
        [HttpPost]
        public JsonResult HoldUnHoldStudentReportCard(int StudentId, int term, int Batch, int classid, string Remark, bool isHold)
        {
            try
            {
                // First, check if a record already exists with the given criteria
                var existingRecord = _context.Tbl_HoldDetail
           .Where(x => x.StudentId == StudentId && x.TermId == term && x.BatchId == Batch && x.ClassId == classid)
           .FirstOrDefault();
                if (existingRecord != null)
                {
                    existingRecord.IsHold = existingRecord.IsHold ? false : true;
                    _context.SaveChanges();
                }
                else
                {
                    // If no record is found, insert a new record
                    var newHoldDetail = new Tbl_HoldDetail
                    {
                        StudentId = StudentId,
                        TermId = term,
                        BatchId = Batch,
                        ClassId = classid,
                        Remark = Remark,
                        IsHold = isHold,
                        HoldDate = DateTime.Now,
                        // Add appropriate date-time values
                    };
                    _context.Tbl_HoldDetail.Add(newHoldDetail);
                    _context.SaveChanges();
                }
                return Json(new { success = true, message = "Operation completed successfully." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
            
        }

        //POST 
        [HttpPost]
        public ActionResult Index(ReportInput input)
        {
            ViewBag.SchoolType = GetschoolTypeList(input.SchollType);
            ViewBag.Batches = GetBactesList(input.BatchId);

            var reportId = _context.MasterReports.Where(w => w.Name == "Tuition Fee")
                .Select(x => x.Id).FirstOrDefault();

            var columns = (from rh in _context.ReportHeadings
                           join h in _context.FeeHeadings on rh.HeadingId equals h.FeeId
                           where rh.ReportId == reportId
                           select new TableColumnss
                           {
                               HeadingId = rh.HeadingId,
                               OrderNo = rh.OrderNo,
                               FeeName = h.FeeName
                           }).OrderBy(o => o.OrderNo).ToList();


            DataTable dt = new DataTable("MyTable");

            dt.Columns.Add(new DataColumn("Bill No", typeof(string)));   //---x-rnik---
            dt.Columns.Add(new DataColumn("Student Name", typeof(string)));
            //dt.Columns.Add(new DataColumn("Addmission Number", typeof(string)));
            //dt.Columns.Add(new DataColumn("Register Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Class", typeof(string)));
            dt.Columns.Add(new DataColumn("Section", typeof(string)));
            dt.Columns.Add(new DataColumn("Mode of Payment", typeof(string)));
            dt.Columns.Add(new DataColumn("Paid Date", typeof(DateTime)));

            foreach (var column in columns)
            {
                if (!dt.Columns.Contains(column.FeeName)) // Check if column already exists
                {
                    dt.Columns.Add(new DataColumn(column.FeeName, typeof(string)));
                }
            }

            dt.Columns.Add(new DataColumn("Total", typeof(string)));

            ViewBag.Columns = dt.Columns;

            //var temp = (from s in _context.StudentsRegistrations
            //            join r in _context.TblFeeReceipts on s.StudentRegisterID equals (long)r.StudentId
            //            select new FeesReport
            //            {
            //                RegNumber = s.StudentRegisterID,
            //                ApplicationNumber = s.ApplicationNumber,
            //                PaidAmount = r.PaidAmount,
            //                Class = s.Class_Id.ToString(),  //---x-rnik--
            //                FeeHeadingIDs = r.FeeHeadingIDs,
            //                FeePaids = r.FeePaids,
            //                Name = s.Name,
            //                AddedDate = r.AddedDate,
            //                PaymentMode = r.PaymentMode
            //            })
            //    .Where(w => w.AddedDate.Date >= input.FromDate.Date && w.AddedDate.Date <= input.ToDate.Date).OrderByDescending(o => o.AddedDate)
            //    .ToList();

            var temp = (from s in _context.Students
                        join r in _context.TblFeeReceipts on s.StudentId equals (long)r.StudentId
                        select new FeesReport
                        {
                            FeeReceiptId=r.FeeReceiptId, //---x-rnik--
                            //RegNumber = s.StudentRegisterID,
                            //ApplicationNumber = s.ApplicationNumber,
                            PaidAmount = r.PaidAmount,
                            Class = s.Class_Id.ToString(),  //---x-rnik--
                            Section = s.Section.ToString(),
                            FeeHeadingIDs = r.FeeHeadingIDs,
                            FeePaids = r.FeePaids,
                            Name = s.Name,
                            AddedDate = r.AddedDate,
                            PaymentMode = r.PaymentMode
                        }).ToList();
           
            if (input.FromDate == input.ToDate)
            {
                temp = temp.Where(x => x.AddedDate.Date == input.ToDate.Date).OrderByDescending(o => o.AddedDate).ToList();
            }
            else
            {
                temp = temp.Where(w => w.AddedDate.Date >= input.FromDate.Date && w.AddedDate.Date <= input.ToDate.Date).OrderByDescending(o => o.AddedDate).ToList();
            }
            var lastColumnIndex = dt.Columns.Count - 1;

            //--x-rnik---"
            var DataListItems = _context.DataListItems.Where(x => x.Status == "Active").ToList();
            //----

            foreach (var data in temp)
            {
                //---x-rnik--
                var c = DataListItems.Where(x => x.DataListItemId == Convert.ToInt32(data.Class)).FirstOrDefault();
                if (c != null)
                {
                    if (c.DataListItemName != null)
                    {
                        data.Class = c.DataListItemName;
                    }
                }
                //---
                    DataRow dr = dt.NewRow(); //Create New Row
                    dr[0] = data.FeeReceiptId;
                    dr[1] = data.Name;
                    //dr[2] = data.ApplicationNumber;
                    //dr[3] = data.RegNumber;
                    dr[2] = data.Class;
                    dr[3] = data.Section;
                    dr[4] = data.PaymentMode;
                    dr[5] = data.AddedDate;
                for (var i = 0; data.HeadingIDs.Count > i; i++)
                {
                    var column = columns.FirstOrDefault(a => a.HeadingId == data.HeadingIDs[i]);

                    if (column == null)
                        continue;

                    dr[column.OrderNo + 5] = data.Paids.Count == data.HeadingIDs.Count ? data.Paids[i] : 0;
                    column.Total += data.Paids.Count == data.HeadingIDs.Count ? data.Paids[i] : 0;
                }
                dr[lastColumnIndex] = data.PaidAmount;
                dt.Rows.InsertAt(dr, dt.Rows.Count);

            }

            DataRow totalRowe = dt.NewRow(); //Create New Row

            totalRowe[0] = "Total";

            foreach (var column in columns)
                totalRowe[column.OrderNo + 5] = column.Total;

            totalRowe[lastColumnIndex] = columns.Sum(s => s.Total);

            dt.Rows.InsertAt(totalRowe, dt.Rows.Count);

            ViewBag.Rows = dt.Rows;

            if (Request.Form["Filter"] != null)
            {
                // Code for function 1
            }
            else if (Request.Form["ExportTestData"] != null)
            {
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

            return View();
        }

        [HttpGet]
        public ActionResult TestPage()
        {
            List<Student> studentlist = _context.Students.ToList();

            ViewBag.SchoolType = GetschoolTypeList("0");


            ViewBag.Batches = GetBactesList("0");

            DataTable dt = new DataTable("MyTable");


            ViewBag.Columns = dt.Columns;
            ViewBag.Rows = dt.Rows;


            return View();
        }

        [HttpPost]
        public ActionResult TestPage(ReportInput input)
        {
            ViewBag.SchoolType = GetschoolTypeList(input.SchollType);
            ViewBag.Batches = GetBactesList(input.BatchId);


            var dataListId = _context.DataLists.Where(w => w.DataListName == "FeeType")
             .Select(s => s.DataListId).FirstOrDefault();

            var feeTypeId = _context.DataListItems.Where(w => w.DataListId == dataListId.ToString() && w.DataListItemName == "TransactionFee")
                .Select(s => s.DataListItemId).FirstOrDefault();

            var headings = _context.FeeHeadings
               .Where(x => x.FeeType_Id == feeTypeId)
               .Select(s => new TableColumnss
               {
                   HeadingId = s.FeeId,
                   FeeName = s.FeeName
               }).OrderByDescending(o=> o.HeadingId).ToList();

            var orderno = 1;


            headings.ForEach(fe => fe.OrderNo = orderno++);


            DataTable dt = new DataTable("MyTable");


            dt.Columns.Add(new DataColumn("Student Name", typeof(string)));
            //dt.Columns.Add(new DataColumn("Addmission Number", typeof(string)));
            //dt.Columns.Add(new DataColumn("Register Number", typeof(string)));
            dt.Columns.Add(new DataColumn("Class", typeof(string)));
            dt.Columns.Add(new DataColumn("Section", typeof(string)));
            dt.Columns.Add(new DataColumn("Mode of Payment", typeof(string)));
            dt.Columns.Add(new DataColumn("Paid Date", typeof(DateTime)));


            foreach (var column in headings)
                dt.Columns.Add(new DataColumn(column.FeeName, typeof(string)));

            dt.Columns.Add(new DataColumn("Total", typeof(string)));

            ViewBag.Columns = dt.Columns;


            var temp = (from s in _context.Students
                        join r in _context.TblFeeReceipts on s.StudentId equals (long)r.StudentId
                        select new FeesReport
                        {
                            //RegNumber = s.StudentId,
                            //ApplicationNumber = s.ApplicationNumber,
                            PaidAmount = r.PaidAmount,
                            Class = s.Class,
                            Section=s.Section,
                            FeeHeadingIDs = r.FeeHeadingIDs,
                            FeePaids = r.FeePaids,
                            Name = s.Name,
                            AddedDate = r.AddedDate,
                            PaymentMode = r.PaymentMode
                        })
                .Where(w => w.AddedDate >= input.FromDate && w.AddedDate <= input.ToDate).OrderByDescending(o => o.AddedDate)
                .ToList();

            var lastColumnIndex = dt.Columns.Count - 1;

            foreach (var data in temp)
            {

                for (var i = 0; data.HeadingIDs.Count > i; i++)
                {
                    var column = headings.FirstOrDefault(a => a.HeadingId == data.HeadingIDs[i]);

                    if (column == null)
                        continue;

                    DataRow dr = dt.NewRow(); //Create New Row

                    dr[0] = data.Name;
                    //dr[1] = data.ApplicationNumber;
                    //dr[2] = data.RegNumber;
                    dr[1] = data.Class;
                    dr[2] = data.Section;
                    dr[3] = data.PaymentMode;
                    dr[4] = data.AddedDate;
                    dr[column.OrderNo + 4] = data.Paids.Count == data.HeadingIDs.Count ? data.Paids[i] : 0;

                    dr[lastColumnIndex] = data.Paids.Count == data.HeadingIDs.Count ? data.Paids[i] : 0;

                    dt.Rows.InsertAt(dr, dt.Rows.Count);

                    column.Total += data.Paids.Count == data.HeadingIDs.Count ? data.Paids[i] : 0;
                }

            }

            DataRow totalRowe = dt.NewRow(); //Create New Row

            totalRowe[0] = "Total";

          
            foreach (var column in headings)
                totalRowe[column.OrderNo + 4] = column.Total;

            totalRowe[lastColumnIndex] = headings.Sum(s => s.Total);

            dt.Rows.InsertAt(totalRowe, dt.Rows.Count);

            ViewBag.Rows = dt.Rows;

            return View();
        }

        #region Private
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

        private List<SelectListItem> GetBactesList(string selectedId)
        {

            var items = new List<SelectListItem>
            {
                new SelectListItem()
                {
                    Text = "Select Batch",
                    Value = "0",

                }
            };


            var listItems = _context.Tbl_Batches.Select(s => new SelectListItem
            {

                Value = s.Batch_Id.ToString(),
                Text = s.Batch_Name
            }).ToList();


            if (listItems.Count == 0)
                return items;

            items.AddRange(listItems);

            items.ForEach(fe =>
            {
                fe.Selected = fe.Value == selectedId;
            });

            return items;

        }

        #endregion
    }
}