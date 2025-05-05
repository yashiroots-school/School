using SchoolManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class AcademicDetail
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<tbl_AcademicDetail> GetAll()
        {
            List<tbl_AcademicDetail> res = db.tbl_AcademicDetails.ToList();
            return res;
        }

        public List<tbl_AcademicDetail> GetByRegNo(string Regno)
        {
            List<tbl_AcademicDetail> res = db.tbl_AcademicDetails
                                          .Where(e => e.ScholarNumber == Regno)
                                          .ToList();        
            return res;


        }

        public  tbl_AcademicDetail GetById(int id)
        {

            var res = db.tbl_AcademicDetails
                                          .FirstOrDefault(e => e.AcademicDetailId == id);
                                          
            return res;
        }

        public string Add(tbl_AcademicDetail newobj)
        {
            try
            {
                db.tbl_AcademicDetails.Add(newobj);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }

        public string Update(tbl_AcademicDetail newobj)
        {
            try
            {
                var existingobj = db.tbl_AcademicDetails.FirstOrDefault(e => e.AcademicDetailId == newobj.AcademicDetailId);
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
                var genralobj = db.tbl_AcademicDetails.FirstOrDefault(e => e.AcademicDetailId == Id);
                if (genralobj != null)
                {
                    db.tbl_AcademicDetails.Remove(genralobj);
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

    }
}