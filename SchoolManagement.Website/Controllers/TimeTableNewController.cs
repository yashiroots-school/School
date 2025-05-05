using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using SchoolManagement.Website.ViewModels;
using DocumentFormat.OpenXml.VariantTypes;
using System.Configuration;
using System.Data;

namespace SchoolManagement.Website.Controllers
{
    public class TimeTableNewController : Controller
    {
        private ApplicationDbContext _context;
        public TimeTableNewController()
        {

            _context = new ApplicationDbContext();
        }
        // GET: TimeTableNew
        //public ActionResult Create()
        //{
        //    var model = new MasterTimeTableViewModel();

        //    // Load subjects from Subject_setup table
        //    model.AllSubjects = _context.Tbl_SubjectsSetup.Select(s => new Subject
        //    {
        //        SubjectId = s.Subject_ID.ToString(),     // Adjust as per your column name/type
        //        SubjectName = s.Subject_Name
        //    }).ToList();
        //    var Class=_context.DataLists.Where(x=>x.DataListName.ToLower()=="class").Select(x=>x.DataListId).FirstOrDefault();
        //    // Load classes from DataListItem table
        //    var classList = _context.DataListItems.Where(x => x.DataListId == Class.ToString()).Select(c => new
        //    {
        //        Id = c.DataListItemId.ToString(),                  // Or c.ClassId, etc.
        //        Name = c.DataListItemName.ToString(),
        //    }).ToList();

        //    foreach (var cls in classList)
        //    {
        //        var classModel = new ClassTimeTableViewModel
        //        {
        //            ClassId = Convert.ToInt32(cls.Id),
        //            ClassName = cls.Name
        //        };

        //        foreach (var day in model.DaysOfWeek)
        //        {
        //            classModel.SelectedSubjectsPerDay[day] = new List<string>();
        //        }

        //        model.ClassTimeTables.Add(classModel);
        //    }

        //    return View(model);
        //}
        //public ActionResult Create()
        //{
        //    var model = new MasterTimeTableViewModel();

        //    // Step 1: Define days of the week (you can customize this list as needed)
        //    //model.DaysOfWeek = new List<string> { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
        //    model.Staff = _context.StafsDetails.Select(s => new StaffNames
        //    {
        //        StaffId=s.StafId,
        //        StaffName=s.Name
        //    }).ToList();
        //    // Step 2: Load subjects from SubjectSetup table
        //    model.AllSubjects = _context.Tbl_SubjectsSetup.Select(s => new Subject
        //    {
        //        SubjectId = s.Subject_ID.ToString(),     // Adjust as per your DB schema
        //        SubjectName = s.Subject_Name
        //    }).ToList();

        //    // Step 3: Load classes from DataListItem table using the DataListId for 'class'
        //    var classDataListId = _context.DataLists
        //        .Where(x => x.DataListName.ToLower() == "class")
        //        .Select(x => x.DataListId)
        //        .FirstOrDefault();

        //    var sectionDataListId = _context.DataLists
        //       .Where(x => x.DataListName.ToLower() == "section")
        //       .Select(x => x.DataListId)
        //       .FirstOrDefault();
        //    var sectionDataList = _context.DataListItems
        //        .Where(x => x.DataListId == sectionDataListId.ToString()).ToList();
        //    ViewBag.Section= sectionDataList;
        //    var batch=_context.Tbl_Batches.ToList();
        //    ViewBag.Batch = batch;
        //    var classList = _context.DataListItems
        //        .Where(x => x.DataListId == classDataListId.ToString())
        //        .Select(c => new 
        //        {
        //            Id = c.DataListItemId.ToString(),
        //            Name = c.DataListItemName
        //        }).ToList();

        //    // Step 4: Build class timetable entries
        //    foreach (var cls in classList)
        //    {
        //        var classModel = new ClassTimeTableViewModel
        //        {
        //            ClassId = Convert.ToInt32(cls.Id),
        //            ClassName = cls.Name,
        //            SubjectsPerDays = new List<SubjectEntryPerDay>()
        //        };

        //        foreach (var day in model.Staff)
        //        {
        //            classModel.SubjectsPerDays.Add(new SubjectEntryPerDay
        //            {
        //                StaffId = day.StaffId,
        //                SubjectIds = new List<string>()  // Start empty
        //            });
        //        }

        //        model.ClassTimeTables.Add(classModel);
        //    }

        //    return View(model);
        //}
        public ActionResult Create(int? ClassId=null, int? sectionId = null, int? batchId = null)
        {
            var model = new MasterTimeTableViewModel();
            model.SectionId = sectionId??0;
            model.BatchId = batchId??0;
            var classDataListId = _context.DataLists
                   .Where(x => x.DataListName.ToLower() == "class")
                   .Select(x => x.DataListId)
                   .FirstOrDefault();

            var sectionDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "section")
                .Select(x => x.DataListId)
                .FirstOrDefault();

            var sectionDataList = _context.DataListItems
                .Where(x => x.DataListId == sectionDataListId.ToString()).ToList();

            ViewBag.Section = sectionDataList;
            ViewBag.Batch = _context.Tbl_Batches.ToList();
            var classList = _context.DataListItems
                   .Where(x => x.DataListId == classDataListId.ToString())
                   .ToList();
            ViewBag.ClassList = classList;
            if (ClassId != null && ClassId != 0)
            {
                // Load Staff
                model.Staff = _context.StafsDetails.Select(s => new StaffNames
                {
                    StaffId = s.StafId,
                    StaffName = s.Name,
                    IsActive=s.IsActive??true,
                    StaffCategory= s.StaffCategory??0,
                    
                }).ToList();

                // Load all subjects
                model.AllSubjects = _context.Tbl_SubjectsSetup.Select(s => new Subject
                {
                    SubjectId = s.Subject_ID.ToString(),
                    SubjectName = s.Subject_Name
                }).ToList();

                // Load dropdowns
               
                // Load classes
                if (ClassId != null)
                {
                    classList = _context.DataListItems
                       .Where(x => x.DataListItemId == ClassId)
                       .ToList();
                }

                // Loop through each class
                foreach (var cls in classList)
                {
                    var classModel = new ClassTimeTableViewModel
                    {
                        ClassId = Convert.ToInt32(cls.DataListItemId),
                        ClassName = cls.DataListItemName,
                        SubjectsPerDays = new List<SubjectEntryPerDay>()
                    };

                    foreach (var staff in model.Staff)
                    {
                        var subjectIds = new List<string>();
                        bool isClassTeacher = false;

                        // ✅ Only fetch from DB if SectionId is selected
                        if (sectionId != null && batchId != null)
                        {
                            var existingSubjects = _context.Subjects
                                .Where(x => x.Class_Id == classModel.ClassId &&
                                            x.StaffId == staff.StaffId &&
                                            x.Section_Id == sectionId && x.Batch_Id==batchId)
                                .ToList();

                            subjectIds = existingSubjects.Select(x => x.Subject_ID.ToString()).ToList();
                            isClassTeacher = existingSubjects.Any(x => x.Class_Teacher);
                        }

                        classModel.SubjectsPerDays.Add(new SubjectEntryPerDay
                        {
                            StaffId = staff.StaffId,
                            SubjectIds = subjectIds,
                            IsClassTeacher = isClassTeacher
                        });
                    }

                    model.ClassTimeTables.Add(classModel);
                }

                // Set selected SectionId and BatchId in model
                model.SectionId = sectionId ?? 0;
                model.BatchId = batchId ?? 0;
            }
            return View(model);
        }

        #region post
        //[HttpPost]
        //public ActionResult SaveTimeTable(MasterTimeTableViewModel model)
        //{
        //    try
        //    {
        //        if (model != null && model.ClassTimeTables != null)
        //        {
        //            foreach (var classRow in model.ClassTimeTables)
        //            {
        //                foreach (var subjectEntry in classRow.SubjectsPerDays)
        //                {
        //                    int staffId = subjectEntry.StaffId;
        //                    int classId = classRow.ClassId;
        //                    bool isClassTeacher = subjectEntry.IsClassTeacher;

        //                    foreach (var subjectId in subjectEntry.SubjectIds)
        //                    {
        //                        // Check if an entry already exists
        //                        var existing = _context.ClassTimeTableEntries.FirstOrDefault(x =>
        //                            x.ClassId == classId &&
        //                            x.StaffId == staffId &&
        //                            x.SubjectId == subjectId);

        //                        if (existing != null)
        //                        {
        //                            // UPDATE
        //                            existing.IsClassTeacher = isClassTeacher;
        //                            // update other fields if needed
        //                        }
        //                        else
        //                        {
        //                            // INSERT
        //                            var newEntry = new ClassTimeTableEntry
        //                            {
        //                                ClassId = classId,
        //                                StaffId = staffId,
        //                                SubjectId = subjectId,
        //                                IsClassTeacher = isClassTeacher
        //                            };

        //                            _context.ClassTimeTableEntries.Add(newEntry);
        //                        }
        //                    }
        //                }
        //            }

        //            _context.SaveChanges();
        //            TempData["Success"] = "Time table saved successfully.";
        //        }

        //        return RedirectToAction("Create"); // Or wherever you want to redirect
        //    }
        //    catch (Exception ex)
        //    {
        //        return RedirectToAction("Create");
        //    }
        //}
        #endregion
        [HttpPost]
        public ActionResult SaveTimeTable(MasterTimeTableViewModel model)
        {
            try//if (model?.ClassTimeTables != null)
            {
                foreach (var classRow in model.ClassTimeTables)
                {
                    int classId = classRow.ClassId;

                    foreach (var period in classRow.SubjectsPerDays)
                    {
                        int staffId = period.StaffId;
                        bool isClassTeacher = period.IsClassTeacher;

                        // First remove existing timetable entries for this ClassId + StaffId + SectionId+
                        var existingEntries = _context.Subjects
                            .Where(x => x.Class_Id == classId && x.StaffId == staffId && x.Batch_Id==model.BatchId && x.Section_Id==model.SectionId)
                            .ToList();

                        foreach (var existing in existingEntries)
                        {
                            _context.Subjects.Remove(existing);
                        }

                        // Now insert selected subjectIds for this slot
                        foreach (var subjectId in period.SubjectIds)
                        {
                            var newEntry = new Subjects
                            {
                                Class_Id = classId,
                                StaffId = staffId,
                                Subject_ID = Convert.ToInt32(subjectId),
                                Batch_Id=model.BatchId,
                                Class_Teacher = isClassTeacher,
                                Section_Id=model.SectionId,
                            };

                            _context.Subjects.Add(newEntry);
                        }
                    }
                }

                _context.SaveChanges();
                TempData["Success"] = "Time table saved successfully.";
                return RedirectToAction("Create");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Create");
            }
        }
        public ActionResult ClassWisePerformanceReport()
        {
            var classDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "class")
                .Select(x => x.DataListId)
                .FirstOrDefault();

            var sectionDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "section")
                .Select(x => x.DataListId)
                .FirstOrDefault();

            var sectionDataList = _context.DataListItems
                .Where(x => x.DataListId == sectionDataListId.ToString()).ToList();

            ViewBag.Section = sectionDataList;
            ViewBag.Batch = _context.Tbl_Batches.ToList();

            // Load classes
            var classList = _context.DataListItems
                .Where(x => x.DataListId == classDataListId.ToString())
                .ToList();
            ViewBag.Class = classList;
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;
            return View();
        }
        //public ActionResult MarksRangeSummary(int classId,int sectionId,int batchId, int testId)
        //{
        //    List<long> testIdsToFilter = (testId == 10)
        //? new List<long> { 1, 2, 3, 4 }
        //: new List<long> { testId };
        //    var data = (from tom in _context.tbl_TestObtainedMark
        //                join tr in _context.Tbl_TestRecord on tom.RecordIDFK equals tr.RecordID
        //                join t in _context.tbl_Tests on tr.TestID equals t.TestID
        //                join s in _context.Tbl_SubjectsSetup on t.SubjectID equals s.Subject_ID
        //                where tr.ClassID == classId
        //                  && tr.SectionID == sectionId
        //                  && tr.BatchId == batchId
        //                  && testIdsToFilter.Contains(t.TermID)
        //                select new
        //                {
        //                    s.Subject_Name,
        //                    tom.ObtainedMarks
        //                }).ToList();
        //    // Step 1: Define ranges
        //    var ranges = new List<(int min, int max)>
        //    {
        //        (91, 100), (81, 90), (71, 80), (61, 70), (51, 60),
        //        (41, 50), (31, 40), (21, 30), (11, 20), (0, 10)
        //    };
        //    var subjects = data.Select(d => d.Subject_Name).Distinct().ToList();
        //    var finalList = new List<SubjectMarksRangeViewModel>();
        //    foreach (var range in ranges)
        //    {
        //        var model = new SubjectMarksRangeViewModel
        //        {
        //            MarksRange = $"{range.max}-{range.min}"
        //        };

        //        foreach (var subject in subjects)
        //        {
        //            int count = data
        //                .Where(x => x.Subject_Name == subject &&
        //                            x.ObtainedMarks >= range.min &&
        //                            x.ObtainedMarks <= range.max)
        //                .Count();

        //            model.SubjectCounts[subject] = count;
        //        }

        //        finalList.Add(model);
        //    }

        //    return PartialView("_MarksRangeSummary", finalList);
        //}

        public ActionResult MarksRangeSummary(int classId, int sectionId, int batchId, int testId)
        {
            var result = _context.Database.SqlQuery<MarksRangeResult>(
                "EXEC dbo.usp_MarksRangeSummary @ClassId, @SectionId, @BatchId, @TestId",
                new SqlParameter("@ClassId", classId),
                new SqlParameter("@SectionId", sectionId),
                new SqlParameter("@BatchId", batchId),
                new SqlParameter("@TestId", testId)
            ).ToList();
            var subjects = result.Select(r => r.Subject_Name).Distinct().ToList();
            // Get distinct subjects and ranges
            var rawRanges = result.Select(r => r.MarksRange)
                                      .Distinct()
                                      .OrderByDescending(r =>
                                      {
                                          var parts = r.Split('-');
                                          if (parts.Length == 2 && int.TryParse(parts[0], out int upperBound))
                                          {
                                              // If the format is "Upper-Lower" (e.g., "10-0"),
                                              // we want to sort by the upper bound for descending order.
                                              // Example: 100-91 should come before 90-81
                                              return upperBound;
                                          }
                                          // Handle "Invalid" or other non-numeric ranges
                                          return r == "Invalid" ? -1 : 0; // Ensures "Invalid" is at the bottom
                                      })
                                      .ThenBy(r => // Add a secondary sort for ranges with the same upper bound, like 0-0
                                      {
                                          var parts = r.Split('-');
                                          if (parts.Length == 2 && int.TryParse(parts[1], out int lowerBound))
                                          {
                                              return lowerBound;
                                          }
                                          return 0;
                                      })
                                      .ToList();
            // --- END OF MODIFIED SORTING LOGIC ---


            var finalList = new List<SubjectMarksRangeViewModel>();

            foreach (var range in rawRanges)
            {
                var model = new SubjectMarksRangeViewModel
                {
                    MarksRange = range,
                    SubjectCounts = subjects.ToDictionary(sub => sub, sub => 0)
                };

                foreach (var subject in subjects)
                {
                    var entry = result.FirstOrDefault(r => r.MarksRange == range && r.Subject_Name == subject);
                    model.SubjectCounts[subject] = entry?.Count ?? 0;
                }

                finalList.Add(model);
            }


            return PartialView("_MarksRangeSummary", finalList);
        }
        public ActionResult TeacherWisePerformanceReport()
        {
            ViewBag.Batch = _context.Tbl_Batches.ToList();
            var Terms = _context.tbl_Term.ToList();
            ViewBag.Terms = Terms;
            var Staff = _context.StafsDetails.Where(x => x.IsDeleted == false);
            ViewBag.Staff = Staff;
            return View();
        }
        public ActionResult StaffWiseMarksRangeReport(int staffId, int termId, int batchId)
        {
            // Step 1: Get all Class-Subject combinations assigned to the staff
            var result = _context.Database.SqlQuery<MarksRangeResult>(
                  "EXEC dbo.TeacherWisePerformance @StaffId,@BatchId, @TermId",
                  new SqlParameter("@StaffId", staffId),
                  new SqlParameter("@TermId", termId),
                  new SqlParameter("@BatchId", batchId)
              ).ToList();

            var subjects = result.Select(r => r.Subject_Name).Distinct().ToList();
            // Get distinct subjects and ranges
            var rawRanges = result.Select(r => r.MarksRange)
                                      .Distinct()
                                      .OrderByDescending(r =>
                                      {
                                          var parts = r.Split('-');
                                          if (parts.Length == 2 && int.TryParse(parts[0], out int upperBound))
                                          {
                                              // If the format is "Upper-Lower" (e.g., "10-0"),
                                              // we want to sort by the upper bound for descending order.
                                              // Example: 100-91 should come before 90-81
                                              return upperBound;
                                          }
                                          // Handle "Invalid" or other non-numeric ranges
                                          return r == "Invalid" ? -1 : 0; // Ensures "Invalid" is at the bottom
                                      })
                                      .ThenBy(r => // Add a secondary sort for ranges with the same upper bound, like 0-0
                                      {
                                          var parts = r.Split('-');
                                          if (parts.Length == 2 && int.TryParse(parts[1], out int lowerBound))
                                          {
                                              return lowerBound;
                                          }
                                          return 0;
                                      })
                                      .ToList();
            // --- END OF MODIFIED SORTING LOGIC ---


            var finalList = new List<SubjectMarksRangeViewModel>();

            foreach (var range in rawRanges)
            {
                var model = new SubjectMarksRangeViewModel
                {
                    MarksRange = range,
                    SubjectCounts = subjects.ToDictionary(sub => sub, sub => 0)
                };

                foreach (var subject in subjects)
                {
                    var entry = result.FirstOrDefault(r => r.MarksRange == range && r.Subject_Name == subject);
                    model.SubjectCounts[subject] = entry?.Count ?? 0;
                }

                finalList.Add(model);
            }
            return PartialView("_StaffWiseMarksRangeReport", finalList);
        }
        public ActionResult WeeklyTimeTable(string actionType = "View", int? timeTableId = null, string timeTableName = null,int? DaysPerWeek=null,int? PeriodsPerDay=null, int? BatchId=null)
        {
            var classDataListId = _context.DataLists
                  .Where(x => x.DataListName.ToLower() == "class")
                  .Select(x => x.DataListId)
                  .FirstOrDefault();

            var sectionDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "section")
                .Select(x => x.DataListId)
                .FirstOrDefault();

            var sectionDataList = _context.DataListItems
                .Where(x => x.DataListId == sectionDataListId.ToString()).ToList();

            ViewBag.Section = sectionDataList;
            ViewBag.Batch = _context.Tbl_Batches.ToList();
            var classList = _context.DataListItems
                   .Where(x => x.DataListId == classDataListId.ToString())
                   .ToList();
            ViewBag.ClassList = classList;
            var TimeTableList = _context.Tbl_TimeTableMaster.ToList();
            ViewBag.TimeTableList = TimeTableList;
            DataTable dt = new DataTable();

            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            using (var cmd = new SqlCommand("usp_GenerateWeeklyTimeTableWithAction", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ActionType", actionType);
                cmd.Parameters.AddWithValue("@TimeTableId", (object)timeTableId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@TimeTableName", (object)timeTableName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@DaysPerWeek", (object)DaysPerWeek ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PeriodsPerDay", (object)PeriodsPerDay ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@BatchId", (object)BatchId ?? DBNull.Value);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }

            ViewBag.DataTable = dt;
            return View();
        }
        public ActionResult CreateWeeklyTimeTable()
        {
            var classDataListId = _context.DataLists
                  .Where(x => x.DataListName.ToLower() == "class")
                  .Select(x => x.DataListId)
                  .FirstOrDefault();

            var sectionDataListId = _context.DataLists
                .Where(x => x.DataListName.ToLower() == "section")
                .Select(x => x.DataListId)
                .FirstOrDefault();

            var sectionDataList = _context.DataListItems
                .Where(x => x.DataListId == sectionDataListId.ToString()).ToList();

            ViewBag.Section = sectionDataList;
            ViewBag.Batch = _context.Tbl_Batches.ToList();
            var classList = _context.DataListItems
                   .Where(x => x.DataListId == classDataListId.ToString())
                   .ToList();
            ViewBag.ClassList = classList;
            var TimeTableList = _context.Tbl_TimeTableMaster.ToList();
            ViewBag.TimeTableList = TimeTableList;
           
            return View();
        }




    }
}