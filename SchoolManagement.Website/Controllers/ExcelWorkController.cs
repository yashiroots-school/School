using ClosedXML.Excel;
using DataAccess.ViewModels;
using GoogleApi.Entities.Maps.Common;
using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace SchoolManagement.Website.Controllers
{
    public class ExcelWorkController : Controller
    {
        private ApplicationDbContext _context = new ApplicationDbContext();

        // GET: ExcelWork
       public int UploadStudentExcelFile()
        {
            int status = 0;
            if(Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            if (data > 0)
                            {
                                XLWorkbook Workbook = new XLWorkbook();
                                try
                                {
                                    Workbook = new XLWorkbook(file.InputStream);
                                }
                                catch (Exception ex)
                                {
                                    return 0;
                                }
                                //var workSheet;
                                //try
                                //{

                                   var workSheet = Workbook.Worksheet(1);
                                    //workSheet.Cell("A").Value = "A1";
                                    //workSheet.Cell("b").Value = "aa1";
                                    //workSheet.Cell("c").Value = "Af1";

                                
                                //}
                                //catch
                                //{
                                //    return 0;
                                //}
                               workSheet.FirstRow().Delete();//if you want to remove ist row

                                var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                                var BloodGroup = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "bloodGroup").DataListId.ToString()).ToList();
                                var Category = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "category").DataListId.ToString()).ToList();

                                var Caste = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Caste").DataListId.ToString()).ToList();

                                var Religion = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "Religion").DataListId.ToString()).ToList();

                                foreach (var row in workSheet.RowsUsed())
                                {

                                    row.Cell(1).Value.ToString();
                                    //var studentreg = row.Cell(14).Value.ToString();
                                    var batchname = row.Cell(2).Value.ToString();
                                    var studentname = row.Cell(4).Value.ToString();
                                    var mothername = row.Cell(10).Value.ToString();
                                    var fathername = row.Cell(11).Value.ToString();
                                    var dateofadmission = row.Cell(12).Value.ToString();
                                    var applicationno = row.Cell(14).Value.ToString();
                                    var fmobile = row.Cell(17).Value.ToString();
                                    var mmobile = row.Cell(18).Value.ToString();
                                    var parentemail = row.Cell(19).Value.ToString();
                                    var dateofbirth = row.Cell(20).Value.ToString();
                                    var gender = row.Cell(21).Value.ToString();
                                    var address = row.Cell(26).Value.ToString();
                                    var caste = row.Cell(27).Value.ToString();
                                    var aadhar = row.Cell(28).Value.ToString();
                                    var religion = row.Cell(29).Value.ToString();
                                    var category = row.Cell(31).Value.ToString();
                                    var nationality = row.Cell(34).Value.ToString();
                                    var classname = row.Cell(42).Value.ToString();


                                    //var studentdetails = _context.StudentsRegistrations.FirstOrDefault(x => x.ApplicationNumber == studentreg);
                                    StudentsRegistration studentsRegistration = new StudentsRegistration();


                                    var Isapprove = _context.DataListItems.FirstOrDefault(x => x.DataListItemName == "SUBMITTED").DataListItemId;

                                    

                                    var classlist = Class.FirstOrDefault(x => x.DataListItemName == classname)?.DataListItemId;

                                    var castelist = Caste.FirstOrDefault(x => x.DataListItemName == caste)?.DataListItemId;

                                    var religionlist = Religion.FirstOrDefault(x => x.DataListItemName == religion)?.DataListItemId;


                                    var categorylist = Category.FirstOrDefault(x => x.DataListItemName == category)?.DataListItemId;

                                    studentsRegistration.Class_Id = Convert.ToInt32(classlist);
                                    studentsRegistration.IsApprove = Isapprove;
                                    studentsRegistration.Name = studentname;
                                    studentsRegistration.ApplicationNumber = applicationno;
                                    studentsRegistration.Gender = gender;
                                    studentsRegistration.DOB = dateofbirth;
                                    studentsRegistration.Gender = gender;
                                    studentsRegistration.Cast_Id = Convert.ToInt32(castelist);
                                    studentsRegistration.AdharNo = aadhar;
                                    studentsRegistration.Religion_Id = Convert.ToInt32(religionlist);
                                    studentsRegistration.Nationality = nationality;
                                    studentsRegistration.Parents_Email = parentemail;
                                    studentsRegistration.Batch_Id = 216;
                                    studentsRegistration.Registration_Date = dateofadmission;
                                    studentsRegistration.Category_Id = Convert.ToInt32(categorylist);


                                    //studentsRegistration.Parents_Email = row.Cell(12).Value.ToString();
                                    //studentsRegistration.ProfileAvatar = row.Cell(13).Value.ToString();
                                    //studentsRegistration.BloodGroup_Id = Convert.ToInt32(bloodgrouplist);
                                    //studentsRegistration.City = row.Cell(19).Value.ToString();
                                    //studentsRegistration.State = row.Cell(20).Value.ToString();
                                    //studentsRegistration.Pincode = row.Cell(21).Value.ToString();
                                    //studentsRegistration.Batch_Id = Convert.ToInt32(batchlist);
                                    //studentsRegistration.Category_Id = row.Cell(22).Value;
                                    //studentsRegistration.ApplicationNumber = DateTime.Now.ToString("yyyyddMMhhmmss").ToString();
                                    studentsRegistration.UIN = Guid.NewGuid().ToString();
                                    _context.StudentsRegistrations.Add(studentsRegistration);
                                    _context.SaveChanges();

                                    //Family Details
                                    FamilyDetail familyDetail = new FamilyDetail();
                                    familyDetail.StudentRefId = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                    familyDetail.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                    familyDetail.FMobile = fmobile;
                                    familyDetail.MMobile = mmobile;
                                    familyDetail.FatherName = fathername;
                                    familyDetail.MotherName = mothername;
                                    familyDetail.FResidentialAddress = address;
                                    _context.FamilyDetails.Add(familyDetail);
                                    _context.SaveChanges();


                                    //Guardian Details
                                    GuardianDetails guardianDetails = new GuardianDetails();
                                    guardianDetails.StudentRefId = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                    guardianDetails.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                    _context.GuardianDetails.Add(guardianDetails);
                                    _context.SaveChanges();


                                    //Additional Information
                                    AdditionalInformation additionalInformation = new AdditionalInformation();
                                    additionalInformation.StudentRefId = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                    additionalInformation.ApplicationNumber = studentsRegistration.ApplicationNumber;

                                    additionalInformation.TransportFacility = "no";

                                    additionalInformation.TransportOptions = "NA";

                                    _context.AdditionalInformations.Add(additionalInformation);
                                    _context.SaveChanges();



                                    //Past Schooling Report
                                    PastSchoolingReport pastSchoolingReport = new PastSchoolingReport();
                                    pastSchoolingReport.StudentRefId = Convert.ToInt32(studentsRegistration.StudentRegisterID);
                                    pastSchoolingReport.ApplicationNumber = studentsRegistration.ApplicationNumber;
                                    _context.PastSchoolingReports.Add(pastSchoolingReport);
                                    _context.SaveChanges();
                                }
                                status = 1;
                            }
                        }
                    }
                }
                catch( Exception ex)
                {
                    return 0;
                }
            }
            return status;
        }


        public int UploadStaffExcelFile()
        {
            int status = 0;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];

                        if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            if (data > 0)
                            {
                                XLWorkbook Workbook = new XLWorkbook();
                                try
                                {
                                    Workbook = new XLWorkbook(file.InputStream);
                                }
                                catch (Exception ex)
                                {
                                    return 4;
                                }
                                
                                var workSheet = Workbook.Worksheet(1);
                                workSheet.FirstRow().Delete();
                                //workSheet.DeleteComments(3,5);
                                var staffdetails = _context.StafsDetails.ToList();
                                foreach (var row in workSheet.RowsUsed())
                                {
                                    row.Cell(1).Value.ToString();
                                    Tbl_StaffAttendance tbl_StaffAttendance = new Tbl_StaffAttendance();

                                    var id = row.Cell(1).Value.ToString();
                                    if (id == "")
                                    {
                                        return status = 3;
                                    }
                                    else
                                    {
                                        int staffid = Convert.ToInt32(row.Cell(1).Value.ToString());
                                        var staffname = row.Cell(2).Value.ToString();
                                        //var attendancedate = row.Cell(4).ToString();
                                        tbl_StaffAttendance.Staff_Id = staffid;
                                        tbl_StaffAttendance.Staff_Name = staffdetails.FirstOrDefault(x => x.StafId == staffid) == null ? staffname : staffdetails.FirstOrDefault(x => x.StafId == staffid)?.Name;
                                        //tbl_StaffAttendance.Attendence_Month = Convert.ToString(row.Cell(3).Value.ToString());
                                        //tbl_StaffAttendance.Attendence_Year = Convert.ToString(row.Cell(4).Value.ToString()); ;
                                        string date = row.Cell(3).Value.ToString();
                                        DateTime fromdate = Convert.ToDateTime(date);
                                        //string date1 = date.Remove(10);

                                        //DateTime dateTime = DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.CurrentCulture);
                                        int year = fromdate.Year;
                                        int month = fromdate.Month;
                                        int day = fromdate.Day;
                                        int Noofdaysinmonth = DateTime.DaysInMonth(year, month);
                                        int DayCount = Noofdaysinmonth;
                                        int startPoint = 4;
                                        int Day = 1;
                                        tbl_StaffAttendance.Attendence_Month = month.ToString();
                                        //tbl_StaffAttendance.Attendence_Day = day.ToString();
                                        tbl_StaffAttendance.Attendence_Year = year.ToString();
                                        tbl_StaffAttendance.Attendence_Date = date;
                                        //DayCount = DateTime.DaysInMonth(Convert.ToInt32( tbl_StaffAttendance.Attendence_Year), Convert.ToInt32( tbl_StaffAttendance.Attendence_Month));
                                        for (int k = 0; k < DayCount; k++)
                                        {
                                            tbl_StaffAttendance.Mark_FullDayPresent = row.Cell(startPoint).Value.ToString();
                                            tbl_StaffAttendance.Attendence_Day = (Day);
                                            _context.Tbl_StaffAttendance.Add(tbl_StaffAttendance);
                                            _context.SaveChanges();
                                            Day++;
                                            startPoint++;
                                        }
                                    }
                                    


                                }
                                status = 1;


                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            else
            {
                return status = 2;
            }
            return status;
        }

        public int UploadPastStudentExcelFile()
        {
            int status = 0;
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {

                        HttpPostedFileBase file = files[i];
                        string fname;

                        if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string fileName = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            if (data > 0)
                            {
                                XLWorkbook Workbook = new XLWorkbook();
                                try
                                {
                                    Workbook = new XLWorkbook(file.InputStream);
                                }
                                catch (Exception ex)
                                {
                                    return 0;
                                }
                                //var workSheet;
                                //try
                                //{

                                var workSheet = Workbook.Worksheet(1);
                                //workSheet.Cell("A").Value = "A1";
                                //workSheet.Cell("b").Value = "aa1";
                                //workSheet.Cell("c").Value = "Af1";


                                //}
                                //catch
                                //{
                                //    return 0;
                                //}
                                workSheet.FirstRow().Delete();//if you want to remove ist row

                                foreach (var row in workSheet.RowsUsed())
                                {


                                    row.Cell(1).Value.ToString();
                                    Tbl_StudentPromote tbl_StudentPromote = new Tbl_StudentPromote();
                                    var Class = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList(); 
                                    var classdetails = row.Cell(1).Value.ToString();
                                    var classlist = Class.FirstOrDefault(x => x.DataListItemName == classdetails)?.DataListItemId;
                                    tbl_StudentPromote.FromClass_Id = Convert.ToInt32(classlist);
                                    tbl_StudentPromote.Firstname = row.Cell(3).Value.ToString();
                                    tbl_StudentPromote.Lastname = row.Cell(8).Value.ToString();
                                    //DateTime dateTime = row.Cell(22).Value.ToString();
                                    tbl_StudentPromote.Registration_Date = row.Cell(22).Value.ToString();
                                    _context.Tbl_StudentPromotes.Add(tbl_StudentPromote);
                                    _context.SaveChanges();

                                    
                                   
                                }
                                status = 1;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
            return status;
        }



        //Travel Excel
        public int UploadTransportExcelFile()
        {
            int status = 0;

            if(Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for(int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;
                        if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                        {
                            string filename = file.FileName;
                            string fileContentType = file.ContentType;
                            byte[] fileBytes = new byte[file.ContentLength];
                            var data = file.InputStream.Read(fileBytes, 0, Convert.ToInt32(file.ContentLength));
                            if(data > 0)
                            {
                                XLWorkbook workbook = new XLWorkbook();
                                try
                                {
                                    workbook = new XLWorkbook(file.InputStream);
                                }
                                catch(Exception ex)
                                {
                                    return 0;
                                }

                                var worksheet = workbook.Worksheet(1);
                                worksheet.FirstRow().Delete();
                                List<StudentDetailVM> studentsRegistrations = new List<StudentDetailVM>();
                                var studentdata = _context.StudentsRegistrations.Where(x => x.IsApprove != 192).ToList();
                                var additionalinfo = _context.AdditionalInformations.ToList();
                                var Classes = _context.DataListItems.Where(e => e.DataListId == _context.DataLists.FirstOrDefault(x => x.DataListName.ToLower() == "class").DataListId.ToString()).ToList();
                                var familydetails = _context.FamilyDetails.ToList();

                                //foreach (var item in studentdata)
                                //{
                                //    var name = item.Name + " " + item.Last_Name;
                                //    var classid = Classes.FirstOrDefault(x => x.DataListItemId == item.Class_Id)?.DataListItemName;
                                //    var fammile = familydetails.FirstOrDefault(x => x.StudentRefId == item.StudentRegisterID);
                                //    //var additonal = additionalinfo.FirstOrDefault(x => x.StudentRefId == item.StudentRegisterID);
                                //    if(fammile != null)
                                //    {
                                //        studentsRegistrations.Add(new StudentDetailVM
                                //        {
                                //            StudentName = name,
                                //            StudentId = item.StudentRegisterID,
                                //            Classname = classid,
                                //            //FatherMobileNo = fammile == null ? string.Empty:fammile.FMobile,
                                //            //MotherMobileNo = fammile == null ?string.Empty:fammile.FMobile,
                                //            FatherName = fammile == null ?string.Empty:fammile.FatherName
                                //        });
                                //    }
                                    
                                //}
                                var dtas = studentsRegistrations;
                                foreach (var row in worksheet.RowsUsed())
                                {
                                    row.Cell(1).Value.ToString();
                                    var appli = row.Cell(1).Value.ToString();
                                    var studentname = row.Cell(3).Value.ToString();
                                    var classname = row.Cell(6).Value.ToString();
                                    var kms = row.Cell(19).Value.ToString();
                                    var transportopton = row.Cell(17).Value.ToString();
                                    //var fmobile = row.Cell(8).Value.ToString();
                                    //var mmobile = row.Cell(9).Value.ToString();
                                    var faname = row.Cell(9).Value.ToString();

                                    var studentlist = studentdata.FirstOrDefault(x => x.ApplicationNumber == appli);
                                    if(studentlist != null)
                                    {
                                        var transportdata = additionalinfo.FirstOrDefault(x => x.ApplicationNumber == appli);
                                        if(transportdata != null)
                                        {
                                            if (transportopton == "P & D ")
                                            {
                                                transportdata.TransportOptions = "both";
                                            }
                                            else
                                            {
                                                transportdata.TransportOptions = "pickup";
                                            }
                                            transportdata.TransportFacility = "yes";
                                            transportdata.DistancefromSchool = float.Parse(kms);
                                            _context.SaveChanges();
                                        }
                                    }

                                    //foreach (var item in studentsRegistrations)
                                    //{
                                    //    if(item.StudentName == studentname && item.Classname == classname)
                                    //    {
                                    //        var additionaldata = additionalinfo.FirstOrDefault(x => x.StudentRefId == item.StudentId);
                                    //        if (additionaldata != null)
                                    //        {
                                    //            if(transportopton == "P&D")
                                    //            {
                                    //                additionaldata.TransportOptions = "both";
                                    //            }
                                    //            else
                                    //            {
                                    //                additionaldata.TransportOptions = "pickup";
                                    //            }
                                    //            additionaldata.TransportFacility = "yes";
                                    //            additionaldata.DistancefromSchool = float.Parse(kms);
                                    //            _context.SaveChanges();
                                    //        }
                                    //    }
                                    //}
                                    //var studentdetails = studentsRegistrations.FirstOrDefault(x => x.StudentName.ToLower() == studentname.ToLower() &&  x.Classname == classname);
                                    //if(studentdetails != null)
                                    //{
                                    //    var additionaldata = additionalinfo.FirstOrDefault(x => x.StudentRefId == studentdetails.StudentId);
                                    //    if(additionaldata != null)
                                    //    {
                                    //        additionaldata.DistancefromSchool =float.Parse(kms);
                                    //        _context.SaveChanges();
                                    //    }
                                    //}

                                }
                                status = 1;
                            }
                        }


                    }
                }
                catch(Exception ex)
                {
                    return 0;
                }
            }

            return status;
        }

    }
}



