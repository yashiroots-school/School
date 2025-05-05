using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using SchoolManagement.Website.Models.Master;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace SchoolManagement.Website.Controllers
{
    public class MasterController : Controller
    {

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

        public DataTable Res_Getmessage { get; set; }

        private ApplicationDbContext _context = new ApplicationDbContext();
        // GET: Master
        public ActionResult DataList()
        {
            List<Tbl_DataList> dataList = _context.DataLists.ToList();
            var pagename = "Data List";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";
            List<TblDataListViewModel> DatalListItemDetails = new List<TblDataListViewModel>();
            foreach (var item in dataList)
            {
                DatalListItemDetails.Add(new TblDataListViewModel
                {
                    DataListId = item.DataListId,
                    DataListName = item.DataListName,
                    Editpermission = CheckEditpermission(pagename, editpermission),
                    DeletePermission = CheckDeletepermission(pagename, deletepermission)
                });
            }

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            return View(DatalListItemDetails);
        }
        public ActionResult SignatureSetup()
        {
            return View();
        }

        public ActionResult AddDataList(Tbl_DataList dataListitem)
        {
            if (dataListitem != null)
            {
                _context.DataLists.Add(dataListitem);
                _context.SaveChanges();
            }

            return RedirectToAction("DataList");
        }
        public JsonResult GetDataListItemById(int id)
        {
            var data = _context.DataLists.FirstOrDefault(x => x.DataListId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UpdateDataList(Tbl_DataList dataListitem1)
        {
            var existingobj = _context.DataLists.FirstOrDefault(e => e.DataListId == dataListitem1.DataListId);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(dataListitem1);
                _context.SaveChanges();
            }
            return RedirectToAction("DataList");
        }
        public JsonResult DeleteDataList(int id)
        {

            var genralobj = _context.DataLists.FirstOrDefault(e => e.DataListId == id);
            if (genralobj != null)
            {
                _context.DataLists.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        ////Data List item
        // GET: Master
        public ActionResult DataListItem()
        {

            var pagename = "Data List Item";
            var editpermission = "Edit_Permission";
            var deletepermission = "Delete_Permission";
            var createpermission = "Create_permission";

            ViewBag.DataList = new SelectList(_context.DataLists.ToList(), "DataListId", "DataListName");
            List<Tbl_DataListItem> dataListItem = new List<Tbl_DataListItem>();
            List<DataListItemModel> DataListItemModel = new List<DataListItemModel>();
            dataListItem = _context.DataListItems.ToList();
            var dataList = _context.DataLists.ToList();
            foreach (var item in dataListItem)
            {
                DataListItemModel.Add(new DataListItemModel
                {
                    DataListItemId = item.DataListItemId,
                    DataListItemName = item.DataListItemName,
                    DataListName = dataList.FirstOrDefault(x => x.DataListId.ToString() == item.DataListId) == null ? string.Empty : dataList.FirstOrDefault(x => x.DataListId.ToString() == item.DataListId).DataListName,
                    Editpermission = CheckEditpermission(pagename, editpermission),
                    DeletePermission = CheckDeletepermission(pagename, deletepermission),
                    Status = item.Status == null ? "" : item.Status
                });
            }

            var per = CheckCreatepermission(pagename, createpermission);
            ViewBag.Permission = per;

            return View(DataListItemModel);
        }
        public ActionResult AddDataListitem(Tbl_DataListItem dataitem)
        {

            if (dataitem != null)
            {
                _context.DataListItems.Add(dataitem);
                _context.SaveChanges();
            }

            return RedirectToAction("DataListItem");
        }
        public JsonResult GetDataItem1ById(int id)
        {
            var data = _context.DataListItems.FirstOrDefault(x => x.DataListItemId == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #region ||| Tc Amount |||
        [HttpGet]
        public async Task<ActionResult> TcAmount()
        {
            ViewBag.DataListItems = await _context.DataListItems.Where(x => x.DataListId.Contains("22")).Select(x => new SelectListItem()
            {
                Text = x.DataListItemId.ToString(),
                Value = x.DataListItemName
            }).ToListAsync();

            return View(await _context.TcAmount.Where(x => x.IsDeleted == false).Select(x => new TcAmountViewModel()
            {
                Amount = x.Amount,
                TypeName = _context.DataListItems.Where(y => y.DataListItemId.ToString().Contains(x.Type.ToString())).Select(y => y.DataListItemName).FirstOrDefault(),
                Id = x.Id
            }).ToListAsync());
        }

        [HttpGet]
        public async Task<ActionResult> TcEdit(long id)
        {
            var data = await _context.TcAmount.Where(x => x.Id == id).FirstOrDefaultAsync();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> TcAmountCreateOrUpdate(TcAmountViewModel model)
        {
            try
            {
                if (!model.IsEdit)
                {
                    var entity = new Tbl_TcAmount()
                    {
                        Amount = Convert.ToDecimal(model.Amount),
                        Type = model.TypeId,
                        IsDeleted = false
                    };

                    _context.TcAmount.Add(entity);
                }
                else
                {
                    var data = await _context.TcAmount.Where(x => x.Id == model.Id).FirstOrDefaultAsync();

                    data.Amount = model.Amount;
                    data.Type = model.TypeId;
                    data.IsDeleted = false;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> TcAmountDelete(long Id)
        {
            try
            {
                var data = await _context.TcAmount.Where(x => x.Id == Id).FirstOrDefaultAsync();
                if (data == null)
                {
                    throw new Exception("tc amount data not found");
                }
                data.IsDeleted = true;

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message, ex);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public ActionResult UpdateDataListitem(Tbl_DataListItem dataListitem1)
        {
            var existingobj = _context.DataListItems.FirstOrDefault(e => e.DataListItemId == dataListitem1.DataListItemId);
            if (existingobj != null)
            {
                _context.Entry(existingobj).CurrentValues.SetValues(dataListitem1);
                _context.SaveChanges();
            }
            return RedirectToAction("DataListItem");
        }
        public JsonResult DeleteDataListItem(int id)
        {
            var genralobj = _context.DataListItems.FirstOrDefault(e => e.DataListItemId == id);
            if (genralobj != null)
            {
                _context.DataListItems.Remove(genralobj);
                _context.SaveChanges();
                return Json("Record delete Success", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Record Not Found");
            }
        }

        #region Class

        public ActionResult Class_section_setup()
        {
            //ViewBag.ClassList = _context.Tbl_Class.ToList();
            ViewBag.ClasseDetails = _context.Tbl_Class.ToList();
            //ViewBag.SectionDetails = _context.Tbl_SectionSetup.ToList();
            ViewBag.Class = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            //ViewBag.ClassList = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.Tbl_SubjectsSetup.ToList();
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            List<ClassSubject> tbl_ClassSubjects = new List<ClassSubject>();
            var result = _context.tbl_ClassSubject.Where(x => x.BoardId == getBoardID).ToList();
            foreach (var item in result)
            {
                ClassSubject classSubject = new ClassSubject
                {
                    Id = item.Id,
                    ClassName = _context.DataListItems.Where(x => x.DataListItemId == item.ClassId).Select(x => x.DataListItemName).FirstOrDefault(),
                    SubjectName = _context.Tbl_SubjectsSetup.Where(x => x.Subject_ID == item.SubjectId).Select(x => x.Subject_Name).FirstOrDefault()
                };
                tbl_ClassSubjects.Add(classSubject);

            }

            ViewBag.SectionDetails = tbl_ClassSubjects;

            return View();
        }
        public class ClassSubject
        {
            public long Id { get; set; }
            public string ClassName { get; set; }
            public string SubjectName { get; set; }
        }
        public ActionResult AddClassSubject(Tbl_ClassSubject tbl_Class)
        {
            var getBoardID = _context.TblCreateSchool.Select(x => x.BoardID).FirstOrDefault();
            tbl_Class.BoardId = getBoardID;
            var IsExist = _context.tbl_ClassSubject.Any(entity => entity.ClassId == tbl_Class.ClassId && entity.SubjectId == tbl_Class.SubjectId);

            var url = Request.UrlReferrer.AbsoluteUri;
            if (IsExist)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Already Have That Subject!');location.replace('" + url + "')</script>");
            }
            _context.tbl_ClassSubject.Add(tbl_Class);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Class  Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetClassSubject(int id)
        {
            var data = _context.tbl_ClassSubject.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateClassSubject(Tbl_ClassSubject tbl_Class)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.tbl_ClassSubject.FirstOrDefault(x => x.Id == tbl_Class.Id);
            var IsExist = _context.tbl_ClassSubject.Any(entity => entity.ClassId == tbl_Class.ClassId && entity.SubjectId == tbl_Class.SubjectId);
            if (IsExist)
            {
                data.IsElective = tbl_Class.IsElective;
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Already Have That Subject!');location.replace('" + url + "')</script>");
            }
            if (data != null)
            {
                data.ClassId = tbl_Class.ClassId;
                data.SubjectId = tbl_Class.SubjectId;
                data.IsElective = tbl_Class.IsElective;
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }
        public ActionResult DeleteClassSubject(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.tbl_ClassSubject.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                _context.tbl_ClassSubject.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Somethig Went Wrong');location.replace('" + url + "')</script>");

            }
        }
        public ActionResult AddClassSetup(Tbl_Class tbl_Class)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_Class.Add(tbl_Class);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Class  Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetClassById(int id)
        {
            var data = _context.Tbl_Class.FirstOrDefault(x => x.Class_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateClass(Tbl_Class tbl_Class)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Class.FirstOrDefault(x => x.Class_Id == tbl_Class.Class_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Class);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteClass(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Class.FirstOrDefault(x => x.Class_Id == id);
            if (data != null)
            {
                _context.Tbl_Class.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Class  Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Somethig Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult AddSection(Tbl_SectionSetup tbl_SectionSetup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_SectionSetup.Add(tbl_SectionSetup);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Section Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetSectionById(int id)
        {
            var data = _context.Tbl_SectionSetup.FirstOrDefault(x => x.Section_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateSection(Tbl_SectionSetup tbl_SectionSetup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_SectionSetup.FirstOrDefault(x => x.Section_Id == tbl_SectionSetup.Section_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_SectionSetup);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Section Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteSection(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_SectionSetup.FirstOrDefault(x => x.Section_Id == id);
            if (data != null)
            {
                _context.Tbl_SectionSetup.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Section Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        #endregion

        public ActionResult SubjectSetup()
        {

            ViewBag.Teacher = _context.StafsDetails.ToList();

            ViewBag.SectionLIst = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "section").DataListId.ToString()).ToList();
            ViewBag.ClassList = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Section = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "section").DataListId.ToString()).ToList();

            ViewBag.Class = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "class").DataListId.ToString()).ToList();

            ViewBag.classes = _context.DataListItems.Where(x => x.DataListId == _context.DataListItems.FirstOrDefault(e => e.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            //ViewBag.Subject = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "subject").DataListId.ToString()).ToList();
            ViewBag.Subject = _context.Tbl_SubjectsSetup.ToList();

            //ViewBag.schoolClasses=_context.Classes.ToList();

            ViewBag.schoolClasses = _context.DataListItems.Where(x => x.DataListId == "5").ToList();

            var batches = _context.Tbl_Batches/*.Where(x => x.IsActiveForAdmission == true).*/.ToList();
            ViewBag.BatcheNames = new SelectList(batches, "Batch_Id", "Batch_Name");


            return View();
        }


        // MasterController.cs

        public ActionResult Addsubject(Subjects subjects)
        {
            //Lingesh (Validation for Teacher)

            var url = Request.UrlReferrer.AbsoluteUri;

            SqlCommand cmdExists = new SqlCommand();
            cmdExists.CommandType = CommandType.Text;
            cmdExists.CommandText = "SELECT COUNT(*) FROM Subjects WHERE StaffId = @StaffId AND Class_Id = @Class_Id AND Subject_ID = @Subject_ID AND Section_Id = @Section_Id AND Batch_Id = @Batch_Id";
            cmdExists.Connection = con;

            cmdExists.Parameters.AddWithValue("@StaffId", subjects.StaffId);
            cmdExists.Parameters.AddWithValue("@Class_Id", subjects.Class_Id);
            cmdExists.Parameters.AddWithValue("@Subject_ID", subjects.Subject_ID);
            cmdExists.Parameters.AddWithValue("@Section_Id", subjects.Section_Id);
            cmdExists.Parameters.AddWithValue("@Batch_Id", subjects.Batch_Id);

            con.Open();
            int count = (int)cmdExists.ExecuteScalar();
            con.Close();

            if (count == 0)
            {
                _context.Subjects.Add(subjects);
                _context.SaveChanges();
                return Content("<script>alert('Subject Added Successfully!'); window.location = '" + url + "';</script>");
            }
            else
            {
                return Content("<script>alert('Subject already exists!'); window.location = '" + url + "';</script>");
            }
        }

        public JsonResult GetSubjectById(int id)
        {
            var data = _context.Subjects.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateSubjects(Subjects Subjects)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Subjects.FirstOrDefault(x => x.Id == Subjects.Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(Subjects);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Subject  Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteSubject(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Subjects.FirstOrDefault(x => x.Id == id);
            if (data != null)
            {
                _context.Subjects.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Subject  Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('SOmething Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public JsonResult GetAllSubjectDetails(JqueryDatatableParam param)
        {
            try
            {

                List<Subjects> Allsubjects = new List<Subjects>();
                List<Subjects> Allsubjectsout = new List<Subjects>();
                List<StafsDetails> stafsDetails = new List<StafsDetails>();
                List<Tbl_DataListItem> Classlist = new List<Tbl_DataListItem>();
                List<Tbl_DataListItem> Subjectsetup = new List<Tbl_DataListItem>();
                List<Tbl_DataListItem> Sectionsetup = new List<Tbl_DataListItem>();
                List<Tbl_SubjectsSetup> subject = new List<Tbl_SubjectsSetup>();
                List<Tbl_Batches> batches = new List<Tbl_Batches>();

                Classlist = _context.DataListItems.Where(X => X.DataListId == "5").ToList();
                Subjectsetup = _context.DataListItems.ToList();
                Allsubjects = _context.Subjects.ToList();
                Sectionsetup = _context.DataListItems.ToList();
                stafsDetails = _context.StafsDetails.ToList();
                subject = _context.Tbl_SubjectsSetup.ToList();
                batches=_context.Tbl_Batches.ToList();

                foreach (var item in Allsubjects)
                {
                    Allsubjectsout.Add(new Subjects
                    {
                        Id = item.Id,
                        StaffId = item.StaffId,
                        Class_Id = item.Id,
                        Subject_ID = item.Subject_ID,
                        //Batch_Name = _context.Tbl_Batches.Where(x=>x.Batch_Id== item.Batch_Id).Select(X=>X.Batch_Name).FirstOrDefault(),
                        BatchName=batches.FirstOrDefault(x => x.Batch_Id == item.Batch_Id)?.Batch_Name,
                        Class_Teacher=item.Class_Teacher,
                        Class = Classlist.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                        //,Subject = Subjectsetup.FirstOrDefault(x => x.Subject_Id == item.Subject_ID)?.Subject_Name,
                        Subject = subject.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                        Teacher = stafsDetails.FirstOrDefault(x => x.StafId == item.StaffId)?.Name,
                        Section = Sectionsetup.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,
                        //Class_Teacher = Subjectsetup.FirstOrDefault(x => x.DataListItemId == item.Class_Teacher)?.DataListItemName
                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    Allsubjectsout = Allsubjectsout.Where(x => !string.IsNullOrEmpty(x.Class) && x.Class.ToLower().Contains(param.SSearch.ToLower())
                    || !string.IsNullOrEmpty(x.Subject) && x.Subject.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Teacher) && x.Teacher.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())

                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allsubjectsout = Allsubjectsout.OrderByDescending(x => x.Class).ToList();
                        else
                        {
                            Allsubjectsout = Allsubjectsout.OrderBy(x => x.Class).ToList();
                        }

                        break;

                    case 1:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allsubjectsout = Allsubjectsout.OrderByDescending(x => x.Subject).ToList();
                        else
                        {
                            Allsubjectsout = Allsubjectsout.OrderBy(x => x.Subject).ToList();
                        }

                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allsubjectsout = Allsubjectsout.OrderByDescending(x => x.Teacher).ToList();
                        else
                        {
                            Allsubjectsout = Allsubjectsout.OrderBy(x => x.Teacher).ToList();
                        }

                        break;

                    default:
                        break;
                }
                var data = Allsubjectsout.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = Allsubjectsout.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }


        #region Subject Name

        public ActionResult Subject_Name()
        {
            ViewBag.SubjectName = _context.Tbl_SubjectsSetup.ToList();

            return View();
        }

        public ActionResult AddSubjectName(Tbl_SubjectsSetup tbl_SubjectsSetup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_SubjectsSetup.Add(tbl_SubjectsSetup);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('SubjectName Added Successfully!');location.replace('" + url + "')</script>");


        }

        public JsonResult GetSubjectNameById(int id)
        {
            var data = _context.Tbl_SubjectsSetup.FirstOrDefault(x => x.Subject_ID == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateSubject(Tbl_SubjectsSetup tbl_SubjectsSetup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_SubjectsSetup.FirstOrDefault(x => x.Subject_ID == tbl_SubjectsSetup.Subject_ID);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_SubjectsSetup);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('SubjectName updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult DeleteSubjectName(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_SubjectsSetup.FirstOrDefault(x => x.Subject_ID == id);
            if (data != null)
            {
                _context.Tbl_SubjectsSetup.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('SubjectName Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        #endregion

        #region Teacher Allocation
        public ActionResult TeacherAllocation()
        {
            ViewBag.TeacherList = _context.StafsDetails.ToList();

            ViewBag.Classlist = _context.Tbl_Class.ToList();
            ViewBag.SubjectName = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.Subjectlist = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.Examdetails = _context.Tbl_ExamTypes.ToList();
            ViewBag.Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
            ViewBag.Subject = _context.DataListItems.Where(x => x.DataListId == _context.DataLists.FirstOrDefault(e => e.DataListName.ToLower() == "subject").DataListId.ToString()).ToList();

            return View();
        }

        public ActionResult AddTeacher(Tbl_TeacherAllocation tbl_TeacherAllocation)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_TeacherAllocations.Add(tbl_TeacherAllocation);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Teacher Allocated Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetTeacherById(int id)
        {
            var data = _context.Tbl_TeacherAllocations.FirstOrDefault(x => x.Allocate_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateTeacher(Tbl_TeacherAllocation tbl_TeacherAllocation)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_TeacherAllocations.FirstOrDefault(x => x.Allocate_Id == tbl_TeacherAllocation.Allocate_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_TeacherAllocation);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Teacher Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult DeleteTeacher(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_TeacherAllocations.FirstOrDefault(x => x.Allocate_Id == id);
            if (data != null)
            {
                _context.Tbl_TeacherAllocations.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Teacher Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public JsonResult GetAllTeacherDetails(JqueryDatatableParam param)
        {
            try
            {

                List<Tbl_TeacherAllocation> AllTeacher = new List<Tbl_TeacherAllocation>();
                List<Tbl_TeacherAllocation> AllTeacherout = new List<Tbl_TeacherAllocation>();
                List<StafsDetails> stafsDetails = new List<StafsDetails>();
                List<Tbl_Class> classdetails = new List<Tbl_Class>();
                List<Tbl_SubjectsSetup> subjectdetails = new List<Tbl_SubjectsSetup>();
                AllTeacher = _context.Tbl_TeacherAllocations.ToList();
                stafsDetails = _context.StafsDetails.ToList();
                classdetails = _context.Tbl_Class.ToList();
                subjectdetails = _context.Tbl_SubjectsSetup.ToList();

                foreach (var item in AllTeacher)
                {
                    AllTeacherout.Add(new Tbl_TeacherAllocation
                    {

                        Allocate_Id = item.Allocate_Id,
                        Class_Name = classdetails.FirstOrDefault(x => x.Class_Id == item.Class_Id)?.Class_Name,
                        Subject_Name = subjectdetails.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                        Teacher_Name = stafsDetails.FirstOrDefault(x => x.StafId == item.StaffId)?.Name,


                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    AllTeacherout = AllTeacherout.Where(x => !string.IsNullOrEmpty(x.Class_Name) && x.Class_Name.ToLower().Contains(param.SSearch.ToLower())
                    || !string.IsNullOrEmpty(x.Subject_Name) && x.Subject_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Teacher_Name) && x.Teacher_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())

                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllTeacherout = AllTeacherout.OrderByDescending(x => x.Class_Name).ToList();
                        else
                        {
                            AllTeacherout = AllTeacherout.OrderBy(x => x.Class_Name).ToList();
                        }

                        break;

                    case 1:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllTeacherout = AllTeacherout.OrderByDescending(x => x.Subject_Name).ToList();
                        else
                        {
                            AllTeacherout = AllTeacherout.OrderBy(x => x.Subject_Name).ToList();
                        }

                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllTeacherout = AllTeacherout.OrderByDescending(x => x.Teacher_Name).ToList();
                        else
                        {
                            AllTeacherout = AllTeacherout.OrderBy(x => x.Teacher_Name).ToList();
                        }

                        break;

                    default:
                        break;
                }
                var data = AllTeacherout.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = AllTeacherout.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }


        #endregion



        #region Scheduletimetable

        public ActionResult ScheduleTimeTable()
        {
            return View();
        }

        #endregion
        #region SetTimeTable

        public ActionResult SetTimeTable()
        {

            ViewBag.Timeperiod = _context.tbl_SetTimes.ToList();
            ViewBag.WeekDays = _context.Tbl_WeekDays.ToList();
            var Class = _context.DataLists.Where(x => x.DataListName == "Class").Select(x=>x.DataListId).FirstOrDefault();
            ViewBag.Classes = _context.DataListItems.Where(x=>x.DataListId== Class.ToString()).ToList();
            var Section  = _context.DataLists.Where(x => x.DataListName == "Section").Select(x => x.DataListId).FirstOrDefault();
            ViewBag.Section = _context.DataListItems.Where(x => x.DataListId == Section.ToString()).ToList();

            ViewBag.StaffDetails = _context.StafsDetails.ToList();
            ViewBag.RoomList = _context.Tbl_Rooms.ToList();

            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionLIst = _context.DataListItems.Where(x => x.DataListId == Section.ToString()).ToList();

            return View();
        }

        public ActionResult TimeTable()
        {
            ViewBag.Timeperiod = _context.tbl_SetTimes.ToList();
            ViewBag.WeekDays = _context.Tbl_WeekDays.ToList();

            ViewBag.Classes = _context.Tbl_Class.ToList();
            ViewBag.Section = _context.Tbl_SectionSetup.ToList();

            ViewBag.StaffDetails = _context.StafsDetails.ToList();
            ViewBag.RoomList = _context.Tbl_Rooms.ToList();

            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();

            ViewBag.TimeTable = _context.Tbl_TimeTable.ToList();
            return View();
        }

        public ActionResult ViewTimeTable()
        {
            ViewBag.Timeperiod = _context.tbl_SetTimes.ToList();
            ViewBag.WeekDays = _context.Tbl_WeekDays.ToList();

            ViewBag.Classes = _context.Tbl_Class.ToList();
            ViewBag.Section = _context.Tbl_SectionSetup.ToList();

            ViewBag.StaffDetails = _context.StafsDetails.ToList();
            ViewBag.RoomList = _context.Tbl_Rooms.ToList();

            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();

            ViewBag.TimeTable = _context.Tbl_TimeTable.ToList();
            List<TimeTableViewModel> TimeTableViewModes = new List<TimeTableViewModel>();
            List<Tbl_TimeTable> Tbl_TimeTable = new List<Tbl_TimeTable>();
            List<Tbl_TimeTable> TimeTable = new List<Tbl_TimeTable>();
            List<Tbl_SetTime> tbl_SetTimes = new List<Tbl_SetTime>();
            List<Tbl_Class> tbl_Classes = new List<Tbl_Class>();
            List<Tbl_SectionSetup> Sectionsetup = new List<Tbl_SectionSetup>();
            List<Tbl_SubjectsSetup> Subjectsetup = new List<Tbl_SubjectsSetup>();
            List<StafsDetails> StaffDetails = new List<StafsDetails>();
            List<Tbl_Room> RoomSetup = new List<Tbl_Room>();

            List<Tbl_SectionSetup> tbl_SectionSetups = new List<Tbl_SectionSetup>();
            Tbl_TimeTable = _context.Tbl_TimeTable.ToList();
            tbl_SetTimes = _context.tbl_SetTimes.ToList();
            tbl_Classes = _context.Tbl_Class.ToList();
            Sectionsetup = _context.Tbl_SectionSetup.ToList();
            Subjectsetup = _context.Tbl_SubjectsSetup.ToList();
            StaffDetails = _context.StafsDetails.ToList();
            RoomSetup = _context.Tbl_Rooms.ToList();
            var TodayDate = DateTime.Now.ToString("dd/MM/yyyy");
            var data = _context.Tbl_TimeTable.Where(x => x.CreatedDate == TodayDate).ToList();


            List<ViewTimeTableViewModel> viewtimetableviewmodel = new List<ViewTimeTableViewModel>();
            foreach (var item in data)
            {
                ViewTimeTableViewModel timeTableViewmodel = new ViewTimeTableViewModel
                {
                    Class_Id = item.Class_Id,
                    Day_Time_Id = item.Day_Time_Id,
                    Section_Id = item.Section_Id,
                    Subject_ID = item.Subject_ID,
                    Room_Id = item.Room_Id,
                    StafId = item.StafId,
                    Class_Name = tbl_Classes.FirstOrDefault(x => x.Class_Id == item.Class_Id)?.Class_Name,
                    Section_Name = Sectionsetup.FirstOrDefault(x => x.Section_Id == item.Section_Id)?.Section,
                    Subject_Name = Subjectsetup.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                    Staff_Name = StaffDetails.FirstOrDefault(x => x.StafId == item.StafId)?.Name,
                    Room_Name = RoomSetup.FirstOrDefault(x => x.Room_Id == item.Room_Id)?.Room_Name
                };

                viewtimetableviewmodel.Add(timeTableViewmodel);
            }

            foreach (var item in tbl_SetTimes)
            {
                ViewTimeTableViewModel timeTableViewmodel1 = new ViewTimeTableViewModel
                {
                    Time_Id = item.Time_Id,
                    Time = item.Time
                };
                viewtimetableviewmodel.Add(timeTableViewmodel1);
            }


            ViewBag.TimeTableList = viewtimetableviewmodel;

            if (Session["rolename"].ToString() == "Student")
            {
                ViewBag.sessionlist = "Student";
            }
            else
            {
                ViewBag.sessionlist = "Professor";
            }

            return View();
        }

        public ActionResult ManageTimeTable()
        {

            ViewBag.Timeperiod = _context.tbl_SetTimes.ToList();
            ViewBag.WeekDays = _context.Tbl_WeekDays.ToList();

            ViewBag.Classes = _context.Tbl_Class.ToList();
            ViewBag.Section = _context.Tbl_SectionSetup.ToList();

            ViewBag.StaffDetails = _context.StafsDetails.ToList();
            ViewBag.RoomList = _context.Tbl_Rooms.ToList();

            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();

            ViewBag.TimeTable = _context.Tbl_TimeTable.ToList();
            return View();
        }

        public JsonResult TimeTableView(int classid, int sectionid, string DateId)
        {
            var ClassList = _context.Tbl_Class.ToList();
            var SubjectList = _context.Tbl_SubjectsSetup.ToList();
            var SectionLIst = _context.Tbl_SectionSetup.ToList();
            List<Tbl_TimeTable> NewData = new List<Tbl_TimeTable>();
            string NewDate = DateId.Replace("/", "-");
            var data = _context.Tbl_TimeTable.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Date == NewDate).ToList();
            foreach (var item in data)
            {
                NewData.Add(new Tbl_TimeTable
                {
                    Class_Id = item.Class_Id,
                    Section_Id = item.Section_Id,
                    Subject_ID = item.Subject_ID,
                    StafId = item.StafId,
                    Class_Name = ClassList.FirstOrDefault(x => x.Class_Id == item.Class_Id)?.Class_Name,
                    Section_Name = SectionLIst.FirstOrDefault(x => x.Section_Id == item.Section_Id)?.Section,
                    Subject_Name = SubjectList.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                    Day_ID = item.Day_ID,
                    Time_ID = item.Time_ID

                });
            }
            var Daylist = _context.Tbl_WeekDays.ToList();
            var TimeList = _context.tbl_SetTimes.ToList();
            string html = "";
            for (int i = 1; i <= Daylist.Count(); i++)
            {
                html += "<tr>";
                html += "<th class='head_color'>" + Daylist[i - 1].Week_day + "</th>";
                for (int j = 1; j <= TimeList.Count(); j++)
                {
                    var temPData = NewData.Where(x => x.Day_ID == i && x.Time_ID == j).FirstOrDefault();
                    if (temPData != null)
                    {
                        html += "<td class='subject_bgclr_emty'>" + temPData.Subject_Name + "</td>";
                    }
                    else
                    {
                        html += "<td class='subject_bgclr_emty'>" + "" + "</td>";
                    }
                }
                html += "</tr>";
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TimeTableManage(int classid, int sectionid, string DateId)
        {
            var ClassList = _context.Tbl_Class.ToList();
            var SubjectList = _context.Tbl_SubjectsSetup.ToList();
            var SectionLIst = _context.Tbl_SectionSetup.ToList();
            List<Tbl_TimeTable> NewData = new List<Tbl_TimeTable>();
            string NewDate = DateId.Replace("/", "-");
            var data = _context.Tbl_TimeTable.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Date == NewDate).ToList();
            foreach (var item in data)
            {
                NewData.Add(new Tbl_TimeTable
                {
                    Class_Id = item.Class_Id,
                    Section_Id = item.Section_Id,
                    Subject_ID = item.Subject_ID,
                    StafId = item.StafId,
                    Class_Name = ClassList.FirstOrDefault(x => x.Class_Id == item.Class_Id)?.Class_Name,
                    Section_Name = SectionLIst.FirstOrDefault(x => x.Section_Id == item.Section_Id)?.Section,
                    Subject_Name = SubjectList.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                    Day_ID = item.Day_ID,
                    Time_ID = item.Time_ID,
                    TimeTable_Id = item.TimeTable_Id

                });
            }
            var Daylist = _context.Tbl_WeekDays.ToList();
            var TimeList = _context.tbl_SetTimes.ToList();
            string html = "";
            for (int i = 1; i <= Daylist.Count(); i++)
            {
                html += "<tr>";
                html += "<th class='head_color'>" + Daylist[i - 1].Week_day + "</th>";
                for (int j = 1; j <= TimeList.Count(); j++)
                {
                    var temPData = NewData.Where(x => x.Day_ID == i && x.Time_ID == j).FirstOrDefault();
                    if (temPData != null)
                    {
                        html += "<td class='subject_bgclr_emty' data-toggle='modal' href='#2' onclick='Managetimetable(" + temPData.TimeTable_Id + ")'>" + temPData.Subject_Name + "</td>";
                    }
                    else
                    {
                        html += "<td class='subject_bgclr_emty'>" + "" + "</td>";
                    }
                }
                html += "</tr>";
            }
            return Json(html, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ManageTimeById(int id)
        {
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.TimeTable_Id == id);
            var classlist = _context.Tbl_Class.ToList();
            var sectionlist = _context.Tbl_SectionSetup.ToList();
            var subjectlist = _context.Tbl_SubjectsSetup.ToList();
            var roomlist = _context.Tbl_Rooms.ToList();
            var stafflist = _context.StafsDetails.ToList();
            data.Class_Name = classlist.FirstOrDefault(x => x.Class_Id == data.Class_Id)?.Class_Name;
            data.Section_Name = sectionlist.FirstOrDefault(x => x.Section_Id == data.Section_Id)?.Section;
            data.Subject_Name = subjectlist.FirstOrDefault(x => x.Subject_ID == data.Subject_ID)?.Subject_Name;
            data.Room_Name = roomlist.FirstOrDefault(x => x.Room_Id == data.Room_Id)?.Room_Name;
            data.Staff_Name = stafflist.FirstOrDefault(x => x.StafId == data.StafId)?.Name;
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTimeId(int id)
        {
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.Day_Time_Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddTimeTable(Tbl_TimeTable tbl_TimeTable)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            try
            {
                if (tbl_TimeTable.Class_Id == 0)
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Something Weent Wrong');location.replace('" + url + "')</script>");

                }

                else
                {
                    tbl_TimeTable.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                    _context.Tbl_TimeTable.Add(tbl_TimeTable);
                    _context.SaveChanges();
                    SetCookies("200");
                    return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");
                }
            }
            catch (Exception)
            {
                SetCookies("204");
                return Content("<script language='javascript' type='text/javascript'>location.replace('" + url + "')</script>");

            }



        }


        public JsonResult GetTimeTableId(int Day_id, int Time_id)
        {
            var DayId = Day_id;
            var TimeId = Time_id;
            return Json(new
            {
                DayId,
                TimeId
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllClasses()
        {
            var data = _context.Tbl_Class.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSection()
        {
            var data = _context.Tbl_SectionSetup.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllSubject()
        {
            var data = _context.Tbl_SubjectsSetup.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllStafff()
        {
            var data = _context.StafsDetails.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAllRoom()
        {
            var data = _context.Tbl_Rooms.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisplayTimeTable(int id)
        {
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.Day_Time_Id == id);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetClassName(int id)
        {
            var data = _context.Tbl_Class.FirstOrDefault(x => x.Class_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetsectionName(int id)
        {
            var data = _context.Tbl_SectionSetup.FirstOrDefault(x => x.Section_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetSubjectName(int id)
        {
            var data = _context.Tbl_SubjectsSetup.FirstOrDefault(x => x.Subject_ID == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetstaffName(int id)
        {
            var data = _context.StafsDetails.FirstOrDefault(x => x.StafId == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        //public JsonResult AddTimeTable(int DayTimeid, int Sectionid, int Classid, int Subjectid, int Staffid, int Room_Id)
        //{
        //    Tbl_TimeTable tbl_TimeTable = new Tbl_TimeTable
        //    {
        //        Class_Id = Classid,
        //        Day_Time_Id = DayTimeid,
        //        Section_Id = Sectionid,
        //        Subject_ID = Subjectid,
        //        StafId = Staffid,
        //        Room_Id = Room_Id
        //    };
        //    _context.Tbl_TimeTable.Add(tbl_TimeTable);
        //    _context.SaveChanges();
        //    return Json(new
        //    {
        //        status = "Success"
        //    }, JsonRequestBehavior.AllowGet);

        //}


        public JsonResult GetTimeTableById(int id)
        {
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.TimeTable_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateTimeTable(Tbl_TimeTable tbl_TimeTable)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.TimeTable_Id == tbl_TimeTable.TimeTable_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_TimeTable);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('TimeTable Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteTimeTable(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.TimeTable_Id == id);
            if (data != null)
            {
                _context.Tbl_TimeTable.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('TimeTable Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public JsonResult CheckTimeTable(int classid, int sectionid, int Dayid, int Timeid, string Date)
        {
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Day_ID == Dayid && x.Time_ID == Timeid && x.Date == Date);
            //var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.Day_ID == Dayid && x.Time_ID == Timeid);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("Fail", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ReplaceTimeTable(Tbl_TimeTable tbl_TimeTable)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.TimeTable_Id == tbl_TimeTable.TimeTable_Id && x.Day_ID == tbl_TimeTable.Day_ID && x.Time_ID == tbl_TimeTable.Time_ID);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_TimeTable);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('TimeTable Updated Successfully!');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }
        //public JsonResult ReplaceTimeTable(int classid, int sectionid, int subjectid, int staffid, int roomid, int Dayid, int Timeid, string Date)
        //{
        //    var data = _context.Tbl_TimeTable.FirstOrDefault(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Subject_ID == subjectid && x.StafId == staffid && x.Room_Id == roomid && x.Day_ID == Dayid && x.Time_ID == Timeid && x.Date == Date);
        //    Tbl_TimeTable tbl_TimeTable = new Tbl_TimeTable();
        //    if(data != null)
        //    {
        //        tbl_TimeTable = data;
        //        tbl_TimeTable.Class_Id = classid;
        //        tbl_TimeTable.Section_Id = sectionid;
        //        tbl_TimeTable.Subject_ID = subjectid;
        //        tbl_TimeTable.StafId = staffid;
        //        tbl_TimeTable.Room_Id = roomid;
        //        tbl_TimeTable.Day_ID = Dayid;
        //        tbl_TimeTable.Time_ID = Timeid;
        //        tbl_TimeTable.Date = Date;
        //        tbl_TimeTable.TimeTable_Id = data.TimeTable_Id;
        //        tbl_TimeTable.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
        //        _context.Entry(data).CurrentValues.SetValues(tbl_TimeTable);
        //        _context.SaveChanges();
        //        return Json(new
        //        {
        //            status = "success"
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            status = "Fail"
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        public JsonResult AddTimeTable(int classid, int sectionid, int subjectid, int staffid, int roomid, int Dayid, int Timeid, string Date)
        {
            Tbl_TimeTable tbl_TimeTable = new Tbl_TimeTable
            {
                Class_Id = classid,
                Section_Id = sectionid,
                Subject_ID = subjectid,
                StafId = staffid,
                Room_Id = roomid,
                Day_ID = Dayid,
                Time_ID = Timeid,
                Date = Date,
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy")
            };
            _context.Tbl_TimeTable.Add(tbl_TimeTable);
            _context.SaveChanges();
            return Json(new
            {
                status = "success"
            }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Department

        public ActionResult Department()
        {
            ViewBag.DepartmentList = _context.tbl_Departments.ToList();

            return View();
        }

        public ActionResult AddDepartment(tbl_Department tbl_Department)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.tbl_Departments.Add(tbl_Department);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Department  Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetDepartmentById(int id)
        {
            var data = _context.tbl_Departments.FirstOrDefault(x => x.DepartmentId == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateDepartment(tbl_Department tbl_Department)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.tbl_Departments.FirstOrDefault(x => x.DepartmentId == tbl_Department.DepartmentId);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Department);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Department Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteDepartment(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.tbl_Departments.FirstOrDefault(x => x.DepartmentId == id);
            if (data != null)
            {
                _context.tbl_Departments.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Department Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public JsonResult GetAllDepartmentlist(JqueryDatatableParam param)
        {
            try
            {
                List<tbl_Department> AllDepartment = new List<tbl_Department>();
                List<tbl_Department> AllDepartmentOut = new List<tbl_Department>();
                AllDepartment = _context.tbl_Departments.ToList();

                foreach (var item in AllDepartment)
                {
                    AllDepartmentOut.Add(new tbl_Department
                    {
                        DepartmentId = item.DepartmentId,
                        DepartmentName = item.DepartmentName
                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    AllDepartmentOut = AllDepartmentOut.Where(x => !string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.ToLower().Contains(param.SSearch.ToLower())


                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllDepartmentOut = AllDepartmentOut.OrderByDescending(x => x.DepartmentName).ToList();
                        else
                        {
                            AllDepartmentOut = AllDepartmentOut.OrderBy(x => x.DepartmentName).ToList();
                        }

                        break;



                    default:
                        break;
                }
                var data = AllDepartmentOut.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = AllDepartmentOut.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region Room Types

        public ActionResult RoomTypes()
        {
            ViewBag.Roomtype = _context.Tbl_RoomTypes.ToList();

            ViewBag.Room_Type = _context.Tbl_RoomTypes.ToList();

            ViewBag.Rooms = _context.Tbl_Rooms.ToList();

            List<Tbl_Room> Tbl_Room = new List<Tbl_Room>();
            var RoomList = _context.Tbl_Rooms.ToList();
            var RoomTypelist = _context.Tbl_RoomTypes.ToList();

            foreach (var item in RoomList)
            {
                Tbl_Room.Add(new Tbl_Room
                {
                    RoomType_ID = item.RoomType_ID,
                    Room_Id = item.Room_Id,
                    Room_Name = item.Room_Name,
                    Seating_Capacity = item.Seating_Capacity,
                    Location = item.Location,
                    Remarks = item.Remarks,
                    Room_No = item.Room_No,
                    Room_Type = RoomTypelist.FirstOrDefault(x => x.Room_Id == item.RoomType_ID)?.Room_Type
                });
            }
            ViewBag.RoomTypeList = Tbl_Room;
            return View();
        }

        public ActionResult AddRoom_Type(Tbl_RoomType tbl_RoomType)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_RoomTypes.Add(tbl_RoomType);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Room Type Added Successfully!');location.replace('" + url + "')</script>");
        }

        public JsonResult GetRoom_TypeBy_Id(int id)
        {
            var data = _context.Tbl_RoomTypes.FirstOrDefault(x => x.Room_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateRoom_Type(Tbl_RoomType tbl_RoomType)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_RoomTypes.FirstOrDefault(x => x.Room_Id == tbl_RoomType.Room_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_RoomType);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Room Type Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteRoom_Type(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_RoomTypes.FirstOrDefault(x => x.Room_Id == id);
            if (data != null)
            {
                _context.Tbl_RoomTypes.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Room Type Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        //For RoomSetup
        public ActionResult AddRooksetup(Tbl_Room tbl_Room)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_Rooms.Add(tbl_Room);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Room Setup Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetRoomsetupById(int id)
        {
            var data = _context.Tbl_Rooms.FirstOrDefault(x => x.Room_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateRoomsetup(Tbl_Room tbl_Room)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Rooms.FirstOrDefault(x => x.Room_Id == tbl_Room.Room_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Room);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Room Setup Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult DeleteRooom_Setup(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Rooms.FirstOrDefault(x => x.Room_Id == id);
            if (data != null)
            {
                _context.Tbl_Rooms.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Room Setup Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrongf');location.replace('" + url + "')</script>");

            }
        }

        #endregion

        #region Exam Type

        public ActionResult ExamTypes()
        {
            ViewBag.Examdetails = _context.Tbl_ExamTypes.ToList();

            return View();
        }

        public ActionResult AddExamTypes(Tbl_ExamTypes tbl_ExamTypes)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_ExamTypes.Add(tbl_ExamTypes);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('ExamType Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetExamTypeById(int id)
        {
            var data = _context.Tbl_ExamTypes.FirstOrDefault(x => x.Exam_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateExamType(Tbl_ExamTypes tbl_ExamTypes)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_ExamTypes.FirstOrDefault(x => x.Exam_Id == tbl_ExamTypes.Exam_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_ExamTypes);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('ExamType Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteExamType(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_ExamTypes.FirstOrDefault(x => x.Exam_Id == id);
            if (data != null)
            {
                _context.Tbl_ExamTypes.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('ExamType Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }
        #endregion

        #region GetTime

        public ActionResult SetTime()
        {

            ViewBag.Timeperiod = _context.tbl_SetTimes.ToList();

            return View();
        }

        public ActionResult AddTime(Tbl_SetTime tbl_SetTime)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.tbl_SetTimes.Add(tbl_SetTime);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Time Added Successfully!');location.replace('" + url + "')</script>");

        }

        public JsonResult GetTimeById(int id)
        {
            var data = _context.tbl_SetTimes.FirstOrDefault(x => x.Time_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateTime(Tbl_SetTime tbl_SetTime)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            //var data = _context.tbl_Semesters.FirstOrDefault(x => x.SemesterId == tbl_SetTime.Time_Id);
            var data = _context.tbl_SetTimes.FirstOrDefault(x => x.Time_Id == tbl_SetTime.Time_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_SetTime);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Time Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        public ActionResult DeleteTime(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.tbl_SetTimes.FirstOrDefault(x => x.Time_Id == id);
            if (data != null)
            {
                _context.tbl_SetTimes.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Time Deleted Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }


        #endregion

        #region Weekdays

        public ActionResult WeekDays()
        {
            ViewBag.WeekDaylist = _context.Tbl_WeekDays.ToList();


            return View();
        }

        public ActionResult AddWeekDays(Tbl_WeekDays tbl_WeekDays)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_WeekDays.Add(tbl_WeekDays);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Days Added Successfully!');location.replace('" + url + "')</script>");

        }


        public JsonResult GetDaysById(int id)
        {
            var data = _context.Tbl_WeekDays.FirstOrDefault(x => x.Day_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateDate(Tbl_WeekDays tbl_WeekDays)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_WeekDays.FirstOrDefault(x => x.Day_Id == tbl_WeekDays.Day_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_WeekDays);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Days Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult DeleteDays(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_WeekDays.FirstOrDefault(x => x.Day_Id == id);
            if (data != null)
            {
                _context.Tbl_WeekDays.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Days Updated Successfully!');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        #endregion

        //Atul has worked in this method
        #region Table Assignment
        public ActionResult TableAssignment()
        {
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();

            return View();
        }

        public ActionResult AddAssignment(Tbl_Assignment tbl_Assignment)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_Assignment.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            _context.Tbl_Assignments.Add(tbl_Assignment);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Assignment Added Successfully');location.replace('" + url + "')</script>");

        }
        //public async Task<bool> SendPushNotification(string title, string message)
        //{
        //    var fcmUrl = "https://fcm.googleapis.com/fcm/send";
        //    var serverKey = "YOUR_FCM_SERVER_KEY";

        //    var payload = new
        //    {
        //        to = "/topics/all_users", // Send to all users subscribed to the topic
        //        notification = new
        //        {
        //            title = title,
        //            body = message,
        //            sound = "default"
        //        }
        //    };

        //    var jsonPayload = JsonConvert.SerializeObject(payload);

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "key=" + serverKey);
        //        client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

        //        var response = await client.PostAsync(fcmUrl, new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

        //        return response.IsSuccessStatusCode;
        //    }
        //}

        public JsonResult GetAssignmentById(int id)
        {
            var data = _context.Tbl_Assignments.FirstOrDefault(x => x.Assignment_Id == id);
            List<Tbl_DataListItem> ClassList = new List<Tbl_DataListItem>();
            List<Tbl_SubjectsSetup> SubjectSetup = new List<Tbl_SubjectsSetup>();
            List<Tbl_DataListItem> Sectionsetup = new List<Tbl_DataListItem>();
            List<Tbl_Assignment> Assignmentlist = new List<Tbl_Assignment>();

            data.Class_Name = ClassList.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
            data.Subject_Name = SubjectSetup.FirstOrDefault(x => x.Subject_ID == data.Subject_ID)?.Subject_Name;
            data.Section_Name = Sectionsetup.FirstOrDefault(x => x.DataListItemId == data.Section_Id)?.DataListItemName;
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateAssignment(Tbl_Assignment tbl_Assignment)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Assignments.FirstOrDefault(x => x.Assignment_Id == tbl_Assignment.Assignment_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Assignment);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Assignment Updated Successfully');location.replace('ManageAssignment')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }

        public ActionResult DeleteAssignment(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Assignments.FirstOrDefault(x => x.Assignment_Id == id);
            if (data != null)
            {
                _context.Tbl_Assignments.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Assignment Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public JsonResult GetAllAssignmentDetails(JqueryDatatableParam param)
        {
            try
            {
                int Ind = 0;
                int classid = Convert.ToInt32(Request["classid"]);
                int sectionid = Convert.ToInt32(Request["sectionid"]);
                int subjectid = Convert.ToInt32(Request["subjectid"]);
                if (Session["rolename"].ToString() == "Student")
                {
                    if (Convert.ToInt32(Request["classid"]) == 0 && Convert.ToInt32(Request["sectionid"]) == 0 && Convert.ToInt32(Request["subjectid"]) == 0)
                    {
                        Ind = 1;
                        int STDID = Convert.ToInt32(Session["StudentId"]);
                        var StudentTbl = _context.StudentsRegistrations.Where(x => x.StudentRegisterID == STDID).SingleOrDefault();
                        classid = StudentTbl.Class_Id;
                        sectionid = StudentTbl.Section_Id;
                    }
                }
                List<Tbl_Assignment> Allassignment = new List<Tbl_Assignment>();
                List<Tbl_Assignment> Allassignmentout = new List<Tbl_Assignment>();
                List<StafsDetails> stafsDetails = new List<StafsDetails>();
                List<Tbl_DataListItem> classdetails = new List<Tbl_DataListItem>();
                List<Tbl_SubjectsSetup> subjectdetails = new List<Tbl_SubjectsSetup>();
                List<Tbl_DataListItem> Sectionsetup = new List<Tbl_DataListItem>();
                if (Ind == 0)
                {
                    Allassignment = _context.Tbl_Assignments.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Subject_ID == subjectid).ToList();
                }
                else
                {
                    Allassignment = _context.Tbl_Assignments.Where(x => x.Class_Id == classid && x.Section_Id == sectionid).ToList();
                }
                stafsDetails = _context.StafsDetails.ToList();
                classdetails = _context.DataListItems.Where(x => x.DataListId == "5").ToList();
                subjectdetails = _context.Tbl_SubjectsSetup.ToList();
                Sectionsetup = _context.DataListItems.Where(x => x.DataListId == "6").ToList();

                foreach (var item in Allassignment)
                {
                    Allassignmentout.Add(new Tbl_Assignment
                    {
                        Assignment_Id = item.Assignment_Id,
                        Assignment_Date = item.Assignment_Date,

                        Class_Name = classdetails.FirstOrDefault(x => x.DataListItemId == item.Class_Id).DataListItemName,
                        Section_Name = Sectionsetup.FirstOrDefault(x => x.DataListItemId == item.Section_Id).DataListItemName,
                        Subject_Name = subjectdetails.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                        New_Assignment = item.New_Assignment,
                        Submitted_Date = item.Submitted_Date,
                        CreatedDate = item.CreatedDate


                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    Allassignmentout = Allassignmentout.Where(x => !string.IsNullOrEmpty(x.Class_Name) && x.Class_Name.ToLower().Contains(param.SSearch.ToLower())
                    || !string.IsNullOrEmpty(x.Subject_Name) && x.Subject_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Section_Name) && x.Section_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Assignment_Date) && x.Assignment_Date.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.New_Assignment) && x.New_Assignment.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Submitted_Date) && x.Submitted_Date.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.CreatedDate) && x.CreatedDate.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())

                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.Class_Name).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.Class_Name).ToList();
                        }

                        break;

                    case 1:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.Subject_Name).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.Subject_Name).ToList();
                        }

                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.Section_Name).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.Section_Name).ToList();
                        }

                        break;

                    case 3:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.New_Assignment).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.New_Assignment).ToList();
                        }
                        break;

                    case 4:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.Assignment_Date).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.Assignment_Date).ToList();
                        }
                        break;

                    case 5:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.Submitted_Date).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.Submitted_Date).ToList();
                        }
                        break;

                    case 6:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            Allassignmentout = Allassignmentout.OrderByDescending(x => x.CreatedDate).ToList();
                        else
                        {
                            Allassignmentout = Allassignmentout.OrderBy(x => x.CreatedDate).ToList();
                        }
                        break;


                    default:
                        break;
                }
                var data = Allassignmentout.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = Allassignmentout.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }

        public ActionResult ViewAssignment()
        {
            if (Session["rolename"].ToString() == "Student")
            {
                ViewBag.sessionlist = "Student";
                int STID = Convert.ToInt32(Session["StudentId"]);
                var StudentTbl = _context.StudentsRegistrations.Where(x => x.StudentRegisterID == STID).SingleOrDefault();
                ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5" && e.DataListItemId == StudentTbl.Class_Id).ToList();
                ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
                ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6" && e.DataListItemId == StudentTbl.Section_Id).ToList();
            }
            else
            {
                ViewBag.sessionlist = "Professor";
                ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
                ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
                ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            }
            return View();
        }

        public ActionResult ManageAssignment()
        {
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            return View();
        }

        public ActionResult UpdateAssignmentById(int id)
        {

            var data = _context.Tbl_Assignments.FirstOrDefault(x => x.Assignment_Id == id);

            List<Tbl_DataListItem> ClassList = new List<Tbl_DataListItem>();
            List<Tbl_SubjectsSetup> SubjectSetup = new List<Tbl_SubjectsSetup>();
            List<Tbl_DataListItem> Sectionsetup = new List<Tbl_DataListItem>();
            List<Tbl_Assignment> Assignmentlist = new List<Tbl_Assignment>();

            ClassList = _context.DataListItems.Where(x => x.DataListId == "5").ToList();
            SubjectSetup = _context.Tbl_SubjectsSetup.ToList();
            Sectionsetup = _context.DataListItems.Where(x => x.DataListId == "6").ToList();

            data.Class_Name = ClassList.FirstOrDefault(x => x.DataListItemId == data.Class_Id)?.DataListItemName;
            data.Subject_Name = SubjectSetup.FirstOrDefault(x => x.Subject_ID == data.Subject_ID)?.Subject_Name;
            data.Section_Name = Sectionsetup.FirstOrDefault(x => x.DataListItemId == data.Section_Id)?.DataListItemName;
            ViewBag.AssignmentDetails = data;

            ViewBag.ClassList = ClassList;
            ViewBag.SubjectList = SubjectSetup;
            ViewBag.SectionLIst = Sectionsetup;

            return View();
        }

        #endregion
        //Atul has worked in this method
        #region Revision

        public ActionResult TableRevision()
        {
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            return View();
        }


        public ActionResult AddRevision(Tbl_Revision tbl_Revision)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            tbl_Revision.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
            if (tbl_Revision.Revision_Date == null)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Please Fill All the details');location.replace('" + url + "')</script>");
            }
            else
            {
                _context.Tbl_Revision.Add(tbl_Revision);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Revision Added Successfully!');location.replace('" + url + "')</script>");
            }


        }

        public JsonResult GetRevisionById(int id)
        {
            var data = _context.Tbl_Revision.FirstOrDefault(x => x.Revision_Id == id);
            List<Tbl_Class> ClassList = new List<Tbl_Class>();
            List<Tbl_SubjectsSetup> SubjectSetup = new List<Tbl_SubjectsSetup>();
            List<Tbl_SectionSetup> Sectionsetup = new List<Tbl_SectionSetup>();
            List<Tbl_Assignment> Assignmentlist = new List<Tbl_Assignment>();

            data.Class_Name = ClassList.FirstOrDefault(x => x.Class_Id == data.Class_Id)?.Class_Name;
            data.Subject_Name = SubjectSetup.FirstOrDefault(x => x.Subject_ID == data.Subject_ID)?.Subject_Name;
            data.Section_Name = Sectionsetup.FirstOrDefault(x => x.Section_Id == data.Section_Id)?.Section;
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdateRevision(Tbl_Revision tbl_Revision)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Revision.FirstOrDefault(x => x.Revision_Id == tbl_Revision.Revision_Id);
            if (data != null)
            {
                data.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                _context.Entry(data).CurrentValues.SetValues(tbl_Revision);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Revision Updated Successfully');location.replace('TableRevision')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('TableRevision')</script>");
            }
        }


        public ActionResult DeleteRevision(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Revision.FirstOrDefault(x => x.Revision_Id == id);
            if (data != null)
            {
                _context.Tbl_Revision.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Revision Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult ViewRevision()
        {
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            return View();
        }

        public ActionResult ManageRevision()
        {
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            return View();
        }

        public ActionResult ManageRevisionById(int id)
        {

            var data = _context.Tbl_Revision.FirstOrDefault(x => x.Revision_Id == id);
            ViewBag.RevisionList = data;
            ViewBag.ClassList = _context.DataListItems.Where(e => e.DataListId == "5").ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionList = _context.DataListItems.Where(e => e.DataListId == "6").ToList();
            return View();
        }



        public JsonResult GetAllRevisionDetails(JqueryDatatableParam param)
        {
            try
            {

                int classid = Convert.ToInt32(Request["classid"]);
                int sectionid = Convert.ToInt32(Request["sectionid"]);
                int subjectid = Convert.ToInt32(Request["subjectid"]);

                List<Tbl_Revision> AllRevision = new List<Tbl_Revision>();
                List<Tbl_Revision> AllRevisionOut = new List<Tbl_Revision>();
                List<StafsDetails> stafsDetails = new List<StafsDetails>();
                List<Tbl_DataListItem> classdetails = new List<Tbl_DataListItem>();
                List<Tbl_SubjectsSetup> subjectdetails = new List<Tbl_SubjectsSetup>();
                List<Tbl_DataListItem> Sectionsetup = new List<Tbl_DataListItem>();

                AllRevision = _context.Tbl_Revision.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Subject_ID == subjectid).ToList();
                stafsDetails = _context.StafsDetails.ToList();
                classdetails = _context.DataListItems.Where(x => x.DataListId == "5").ToList();
                subjectdetails = _context.Tbl_SubjectsSetup.ToList();
                Sectionsetup = _context.DataListItems.Where(x => x.DataListId == "6").ToList();

                foreach (var item in AllRevision)
                {
                    AllRevisionOut.Add(new Tbl_Revision
                    {
                        Revision_Id = item.Revision_Id,
                        Revision_Date = item.Revision_Date,
                        Description = item.Description,
                        Class_Name = classdetails.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName,
                        Subject_Name = subjectdetails.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                        Section_Name = Sectionsetup.FirstOrDefault(x => x.DataListItemId == item.Section_Id)?.DataListItemName,
                        CreatedDate = item.CreatedDate


                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    AllRevisionOut = AllRevisionOut.Where(x => !string.IsNullOrEmpty(x.Class_Name) && x.Class_Name.ToLower().Contains(param.SSearch.ToLower())
                    || !string.IsNullOrEmpty(x.Subject_Name) && x.Subject_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Section_Name) && x.Section_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Revision_Date) && x.Revision_Date.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Description) && x.Description.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.CreatedDate) && x.CreatedDate.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())

                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.Class_Name).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.Class_Name).ToList();
                        }

                        break;

                    case 1:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.Subject_Name).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.Subject_Name).ToList();
                        }

                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.Section_Name).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.Section_Name).ToList();
                        }

                        break;

                    case 3:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.Revision_Date).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.Revision_Date).ToList();
                        }
                        break;

                    case 4:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.Description).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.Description).ToList();
                        }
                        break;


                    case 6:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllRevisionOut = AllRevisionOut.OrderByDescending(x => x.CreatedDate).ToList();
                        else
                        {
                            AllRevisionOut = AllRevisionOut.OrderBy(x => x.CreatedDate).ToList();
                        }
                        break;


                    default:
                        break;
                }
                var data = AllRevisionOut.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = AllRevisionOut.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion


        #region Portions

        public ActionResult TablePortions()
        {
            ViewBag.ClassList = _context.Tbl_Class.ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionLIst = _context.Tbl_SectionSetup.ToList();
            return View();
        }

        public ActionResult AddPortions(Tbl_Portions tbl_Portions)
        {
            var url = Request.UrlReferrer.AbsoluteUri;

            if (tbl_Portions.Class_Id == 0)
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Please Fill All the Details');location.replace('" + url + "')</script>");

            }
            else
            {
                tbl_Portions.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                _context.Tbl_Portions.Add(tbl_Portions);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Portions Added Successfully');location.replace('" + url + "')</script>");

            }
        }

        public JsonResult GetPortionsById(int id)
        {
            var data = _context.Tbl_Portions.FirstOrDefault(x => x.Portion_Id == id);
            List<Tbl_Class> ClassList = new List<Tbl_Class>();
            List<Tbl_SubjectsSetup> SubjectSetup = new List<Tbl_SubjectsSetup>();
            List<Tbl_SectionSetup> Sectionsetup = new List<Tbl_SectionSetup>();
            List<Tbl_Assignment> Assignmentlist = new List<Tbl_Assignment>();

            data.Class_Name = ClassList.FirstOrDefault(x => x.Class_Id == data.Class_Id)?.Class_Name;
            data.Subject_Name = SubjectSetup.FirstOrDefault(x => x.Subject_ID == data.Subject_ID)?.Subject_Name;
            data.Section_Name = Sectionsetup.FirstOrDefault(x => x.Section_Id == data.Section_Id)?.Section;
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        public ActionResult UpdatePortions(Tbl_Portions tbl_Portions)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Portions.FirstOrDefault(x => x.Portion_Id == tbl_Portions.Portion_Id);
            if (data != null)
            {
                data.CreatedDate = DateTime.Now.ToString("dd/MM/yyyy");
                _context.Entry(data).CurrentValues.SetValues(tbl_Portions);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Portions Updated Successfully');location.replace('TablePortions')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('TablePortions')</script>");
            }
        }



        public ActionResult DeletePortions(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Portions.FirstOrDefault(x => x.Portion_Id == id);
            if (data != null)
            {
                _context.Tbl_Portions.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Please Fill All the Details');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult ViewPortions()
        {
            ViewBag.ClassList = _context.Tbl_Class.ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionLIst = _context.Tbl_SectionSetup.ToList();
            return View();
        }

        public ActionResult ManagePortions()
        {
            ViewBag.ClassList = _context.Tbl_Class.ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionLIst = _context.Tbl_SectionSetup.ToList();
            return View();
        }

        public ActionResult ManagePortionById(int id)
        {
            var data = _context.Tbl_Portions.FirstOrDefault(x => x.Portion_Id == id);
            ViewBag.PortionList = data;
            ViewBag.ClassList = _context.Tbl_Class.ToList();
            ViewBag.SubjectList = _context.Tbl_SubjectsSetup.ToList();
            ViewBag.SectionLIst = _context.Tbl_SectionSetup.ToList();

            return View();
        }

        public JsonResult GetAllPortionsDetails(JqueryDatatableParam param)
        {
            try
            {

                int classid = Convert.ToInt32(Request["classid"]);
                int sectionid = Convert.ToInt32(Request["sectionid"]);
                int subjectid = Convert.ToInt32(Request["subjectid"]);

                List<Tbl_Portions> AllPortions = new List<Tbl_Portions>();
                List<Tbl_Portions> AllPortionsOut = new List<Tbl_Portions>();
                List<StafsDetails> stafsDetails = new List<StafsDetails>();
                List<Tbl_Class> classdetails = new List<Tbl_Class>();
                List<Tbl_SubjectsSetup> subjectdetails = new List<Tbl_SubjectsSetup>();
                List<Tbl_SectionSetup> Sectionsetup = new List<Tbl_SectionSetup>();

                AllPortions = _context.Tbl_Portions.Where(x => x.Class_Id == classid && x.Section_Id == sectionid && x.Subject_ID == subjectid).ToList();
                stafsDetails = _context.StafsDetails.ToList();
                classdetails = _context.Tbl_Class.ToList();
                subjectdetails = _context.Tbl_SubjectsSetup.ToList();
                Sectionsetup = _context.Tbl_SectionSetup.ToList();

                foreach (var item in AllPortions)
                {
                    AllPortionsOut.Add(new Tbl_Portions
                    {
                        Portion_Id = item.Portion_Id,
                        Portion_Date = item.Portion_Date,
                        Description = item.Description,
                        Class_Name = classdetails.FirstOrDefault(x => x.Class_Id == item.Class_Id)?.Class_Name,
                        Subject_Name = subjectdetails.FirstOrDefault(x => x.Subject_ID == item.Subject_ID)?.Subject_Name,
                        Section_Name = Sectionsetup.FirstOrDefault(x => x.Section_Id == item.Section_Id)?.Section,
                        CreatedDate = item.CreatedDate


                    });
                }

                if (!string.IsNullOrEmpty(param.SSearch))
                {
                    AllPortionsOut = AllPortionsOut.Where(x => !string.IsNullOrEmpty(x.Class_Name) && x.Class_Name.ToLower().Contains(param.SSearch.ToLower())
                    || !string.IsNullOrEmpty(x.Subject_Name) && x.Subject_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Section_Name) && x.Section_Name.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Portion_Date) && x.Portion_Date.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.Description) && x.Description.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())
                    || !string.IsNullOrEmpty(x.CreatedDate) && x.CreatedDate.ToLowerInvariant().Contains(param.SSearch.ToLowerInvariant())

                    ).ToList();
                }

                switch (param.ISortCol_0)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.Class_Name).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.Class_Name).ToList();
                        }

                        break;

                    case 1:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.Subject_Name).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.Subject_Name).ToList();
                        }

                        break;
                    case 2:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.Section_Name).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.Section_Name).ToList();
                        }

                        break;

                    case 3:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.Portion_Date).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.Portion_Date).ToList();
                        }
                        break;

                    case 4:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.Description).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.Description).ToList();
                        }
                        break;


                    case 6:
                        if (!string.IsNullOrEmpty(param.SSortDir_0) && param.SSortDir_0.ToLower() == "desc")
                            AllPortionsOut = AllPortionsOut.OrderByDescending(x => x.CreatedDate).ToList();
                        else
                        {
                            AllPortionsOut = AllPortionsOut.OrderBy(x => x.CreatedDate).ToList();
                        }
                        break;


                    default:
                        break;
                }
                var data = AllPortionsOut.Skip(param.IDisplayStart).Take(param.IDisplayLength).ToList();

                var totalrecords = AllPortionsOut.Count();
                return Json(new
                {
                    param.SEcho,
                    iTotalRecords = totalrecords,
                    iTotalDisplayRecords = totalrecords,
                    aaData = data
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                return null;
            }

        }

        #endregion


        public void SetCookies(string code)
        {
            HttpCookie cookie = new HttpCookie("iscode", code);
            Response.Cookies.Add(cookie);
        }

        #region Batches

        public ActionResult TableBatches()
        {
            ViewBag.BatchList = _context.Tbl_Batches.ToList();

            return View();
        }

        public ActionResult AddBatches(Tbl_Batches tbl_Batches)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            if (tbl_Batches == null || string.IsNullOrWhiteSpace(tbl_Batches.Batch_Name))
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Batch Name cannot be empty!');location.replace('" + url + "')</script>");
            }
            else
            {
                Tbl_Batches batch = _context.Tbl_Batches.Where(x => x.Batch_Name.Equals(tbl_Batches.Batch_Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                if (batch != null && !string.IsNullOrWhiteSpace(batch.Batch_Name))
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Batch already exists!');location.replace('" + url + "')</script>");
                }
                else
                {
                    if (tbl_Batches.IsActiveForAdmission == true && _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).Count() > 0)
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Another batch is already active for admission!');location.replace('" + url + "')</script>");
                    }
                    else
                    {
                        _context.Tbl_Batches.Add(tbl_Batches);
                        _context.SaveChanges();
                        return Content("<script language='javascript' type='text/javascript'>alert('Batches Added Successfully');location.replace('" + url + "')</script>");
                    }
                }
            }
        }

        public JsonResult GetBatchById(int id)
        {
            var data = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateBatches(Tbl_Batches tbl_Batches)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == tbl_Batches.Batch_Id);
            if (data != null)
            {
                if (string.IsNullOrWhiteSpace(tbl_Batches.Batch_Name))
                {
                    return Content("<script language='javascript' type='text/javascript'>alert('Batch Name cannot be empty!');location.replace('" + url + "')</script>");
                }
                else
                {
                    Tbl_Batches batch = _context.Tbl_Batches.Where(x => x.Batch_Name.Equals(tbl_Batches.Batch_Name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

                    if (batch != null && batch.Batch_Id != tbl_Batches.Batch_Id && !string.IsNullOrWhiteSpace(batch.Batch_Name))
                    {
                        return Content("<script language='javascript' type='text/javascript'>alert('Batch already exists!');location.replace('" + url + "')</script>");
                    }
                    else
                    {
                        if (tbl_Batches.IsActiveForAdmission == true)
                        {
                            List<Tbl_Batches> activeBatches = _context.Tbl_Batches.Where(x => x.IsActiveForAdmission == true).ToList();
                            if (activeBatches.Count > 1 || (activeBatches.Count == 1 && activeBatches[0].Batch_Id != tbl_Batches.Batch_Id))
                            {
                                return Content("<script language='javascript' type='text/javascript'>alert('Another batch is already active for admission!');location.replace('" + url + "')</script>");
                            }
                            else
                            {
                                _context.Entry(data).CurrentValues.SetValues(tbl_Batches);
                                _context.SaveChanges();
                                return Content("<script language='javascript' type='text/javascript'>alert('Batches Updated Successfully');location.replace('" + url + "')</script>");
                            }
                        }
                        else
                        {
                            _context.Entry(data).CurrentValues.SetValues(tbl_Batches);
                            _context.SaveChanges();
                            return Content("<script language='javascript' type='text/javascript'>alert('Batches Updated Successfully');location.replace('" + url + "')</script>");
                        }
                    }
                }
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult DeleteBatches(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Batches.FirstOrDefault(x => x.Batch_Id == id);
            if (data != null)
            {
                _context.Tbl_Batches.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Batches Deleted Successfully');location.replace('" + url + "')</script>");

            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");

            }
        }
        #endregion

        #region Bloodgroup

        public ActionResult TableBloodgroup()
        {
            ViewBag.BloodGroupList = _context.Tbl_BloodGroup.ToList();
            return View();
        }

        public ActionResult AddBloodGroup(Tbl_BloodGroup tbl_BloodGroup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_BloodGroup.Add(tbl_BloodGroup);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('BloodGroup Added Successfully');location.replace('" + url + "')</script>");
        }

        public JsonResult GetBloodGroupById(int id)
        {
            var data = _context.Tbl_BloodGroup.FirstOrDefault(x => x.BloodGroup_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateBloodgroup(Tbl_BloodGroup tbl_BloodGroup)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_BloodGroup.FirstOrDefault(x => x.BloodGroup_Id == tbl_BloodGroup.BloodGroup_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_BloodGroup);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Bloodgroup Updated Successfullys');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult DeleteBloodGroup(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_BloodGroup.FirstOrDefault(x => x.BloodGroup_Id == id);
            if (data != null)
            {
                _context.Tbl_BloodGroup.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('BloodGroup Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        #endregion

        #region Category

        public ActionResult TableCategory()
        {
            ViewBag.CategoryList = _context.Tbl_Category.ToList();
            return View();
        }

        public ActionResult AddCategory(Tbl_Category tbl_Category)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_Category.Add(tbl_Category);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Category Added Successfully');location.replace('" + url + "')</script>");
        }


        public JsonResult GetCategoryById(int id)
        {
            var data = _context.Tbl_Category.FirstOrDefault(x => x.Category_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateCategory(Tbl_Category tbl_Category)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Category.FirstOrDefault(x => x.Category_Id == tbl_Category.Category_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Category);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Category Updated Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult DeleteCategory(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Category.FirstOrDefault(x => x.Category_Id == id);
            if (data != null)
            {
                _context.Tbl_Category.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Category Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        #endregion

        #region Religion

        public ActionResult TableReligion()
        {
            ViewBag.ReligionLIst = _context.Tbl_Religion.ToList();
            return View();
        }

        public ActionResult AddReligion(Tbl_Religion tbl_Religion)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_Religion.Add(tbl_Religion);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Religion Added Successfully');location.replace('" + url + "')</script>");

        }

        public JsonResult GetReligionById(int id)
        {
            var data = _context.Tbl_Religion.FirstOrDefault(x => x.Religion_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateReligion(Tbl_Religion tbl_Religion)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Religion.FirstOrDefault(x => x.Religion_Id == tbl_Religion.Religion_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Religion);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Religion Updated Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult DeleteReligion(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Religion.FirstOrDefault(x => x.Religion_Id == id);
            if (data != null)
            {
                _context.Tbl_Religion.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Religion Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        #endregion

        #region Caste

        public ActionResult TableCaste()
        {
            ViewBag.CasteList = _context.Tbl_Caste.ToList();
            return View();
        }

        public ActionResult AddCaste(Tbl_Caste tbl_Caste)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            _context.Tbl_Caste.Add(tbl_Caste);
            _context.SaveChanges();
            return Content("<script language='javascript' type='text/javascript'>alert('Caste Added Successfully');location.replace('" + url + "')</script>");
        }

        public JsonResult GetCasteById(int id)
        {
            var data = _context.Tbl_Caste.FirstOrDefault(x => x.Caste_Id == id);
            if (data != null)
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult UpdateCaste(Tbl_Caste tbl_Caste)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Caste.FirstOrDefault(x => x.Caste_Id == tbl_Caste.Caste_Id);
            if (data != null)
            {
                _context.Entry(data).CurrentValues.SetValues(tbl_Caste);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Caste Updated Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        public ActionResult DeleteCaste(int id)
        {
            var url = Request.UrlReferrer.AbsoluteUri;
            var data = _context.Tbl_Caste.FirstOrDefault(x => x.Caste_Id == id);
            if (data != null)
            {
                _context.Tbl_Caste.Remove(data);
                _context.SaveChanges();
                return Content("<script language='javascript' type='text/javascript'>alert('Caste Deleted Successfully');location.replace('" + url + "')</script>");
            }
            else
            {
                return Content("<script language='javascript' type='text/javascript'>alert('Something Went Wrong');location.replace('" + url + "')</script>");
            }
        }

        #endregion



        //check tappermission
        //edit permission
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

    //public class PayrollBasicDetails
    //{
    //    public int NetPay { get; set; }

    //    public int PF { get; set; }

    //    public int BasicSalary { get; set; }
    //}


    public class ViewTimeTableViewModel
    {
        public int TimeTable_Id { get; set; }

        public string Class_Name { get; set; }

        public int Class_Id { get; set; }

        public string Section_Name { get; set; }

        public int Section_Id { get; set; }

        public string Staff_Name { get; set; }

        public int StafId { get; set; }

        public int Room_Id { get; set; }

        public string Room_Name { get; set; }

        public int Day_Time_Id { get; set; }

        public string CreatedDate { get; set; }

        public int Subject_ID { get; set; }

        public string Subject_Name { get; set; }

        public int Time_Id { get; set; }

        public string Time { get; set; }
    }



}