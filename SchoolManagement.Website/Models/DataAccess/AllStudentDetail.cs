using DataAccess.ViewModels;
using SchoolManagement.Data.Models;
using SFIMAR.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class AllStudentDetail
        {
            ApplicationDbContext db = new ApplicationDbContext();

            public List<tbl_StudentDetail> GetAll()
            {
                List<tbl_StudentDetail> res = db.tbl_StudentDetails.ToList();
                return res;
            }


            /// <summary>
            /// Function to get the list of student including details of work experience.
            /// </summary>
            /// <returns></returns>
            public List<StudentDetailVM>
                GetAllDetails()
            {
                List<StudentDetailVM> studentls = new List<StudentDetailVM>();
                var result = db.tbl_StudentDetails.ToList();
                foreach (var res in result)
                {
                    StudentDetailVM studentobj = new StudentDetailVM();
                    var workexpRecords = db.tbl_WorkExperiences.Where(c => c.ScholarNumber == res.ScholarNumber).Select(c => c.FromDate).ToList();

                    if (workexpRecords != null && workexpRecords.Count() > 0)
                    {
                        studentobj.TotalExperience = workexpRecords.Sum();
                    }
                    else
                    {
                        studentobj.TotalExperience = 0;
                    }


                    studentobj.ScholarNumber = res.ScholarNumber;
                    studentobj.StudentName = res.StudentName;
                    studentobj.Category = res.Category;
                    studentobj.Course = res.Course;
                    studentobj.Batch = res.Batch;
                    studentobj.Specialization = res.Specialization;
                    studentobj.MobileNo = res.MobileNo;
                    studentobj.Spare1 = res.Spare1;
                    studentobj.StudentId = res.StudentId;
                    studentls.Add(studentobj);
                }

                return studentls;
            }

       

            public List<tbl_StudentDetail> GetByRegNo(string Regno)
            {
                List<tbl_StudentDetail> res = db.tbl_StudentDetails
                                              .Where(e => e.ScholarNumber == Regno)
                                              .ToList();
                return res;


            }

            public List<tbl_StudentDetail> GetById(int id)
            {

                List<tbl_StudentDetail> res = db.tbl_StudentDetails
                                              .Where(e => e.StudentId == id)
                                              .ToList();
                return res;
            }

            public string Add(tbl_StudentDetail newobj)
            {

                try
                {
                    db.tbl_StudentDetails.Add(newobj);
                    db.SaveChanges();

                    return "Success";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return "Failed";
                }
            }


            //public string UpdatePassword(string no, string newpassword, string oldpassword)
            //{
            //    try
            //    {
            //        string returnstring = string.Empty;
            //        var existingobj = db.tbl_UserLogin.Where(c => c.Username == no).SingleOrDefault();
            //        if (existingobj != null)
            //        {
            //            if (existingobj.Password == oldpassword)
            //            {
            //                existingobj.Password = newpassword;
            //                db.Entry(existingobj).CurrentValues.SetValues(existingobj);
            //                db.SaveChanges();
            //                returnstring = "Sucess";
            //            }
            //            else
            //            {
            //                returnstring = "failure";
            //            }
            //        }

            //        return returnstring;
            //    }
            //    catch (Exception ex)
            //    {
            //        return "Failed -" + ex.Message;
            //    }
            //}

            public string Update(tbl_StudentDetail newobj)
            {
                try
                {
                    var existingobj = db.tbl_StudentDetails.FirstOrDefault(e => e.StudentId == newobj.StudentId);
                    db.Entry(existingobj).CurrentValues.SetValues(newobj);
                    db.SaveChanges();
                    return "Sucess";
                }
                catch (Exception ex)
                {
                    return "Failed -" + ex.Message;
                }
            }

            public string Delete(int Id)
            {
                try
                {
                    var genralobj = db.tbl_StudentDetails.FirstOrDefault(e => e.StudentId == Id);
                    if (genralobj != null)
                    {
                        db.tbl_StudentDetails.Remove(genralobj);
                        db.SaveChanges();
                        return "Success";
                    }
                    else
                    {
                        return "No Records Found";
                    }
                }
                catch (Exception ex)
                {
                    return "Failed -" + ex.Message;
                }
            }


            //public string Add(tbl_ApplyStatus newobj)
            //{
            //    try
            //    {
            //        db.tbl_ApplyStatus.Add(newobj);
            //        db.SaveChanges();
            //        return "Success";
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine(ex.Message);
            //        return "Failed";
            //    }
            //}
        }
    
}