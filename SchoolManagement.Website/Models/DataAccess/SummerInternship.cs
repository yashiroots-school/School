using SchoolManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class SummerInternship
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<tbl_SummerInternship> GetAll()
        {
            List<tbl_SummerInternship> res = db.tbl_SummerInternships.ToList();
            return res;
        }

        public List<tbl_SummerInternship> GetById(int id)
        {

            List<tbl_SummerInternship> res = db.tbl_SummerInternships
                                          .Where(e => e.SummerInternshipId == id)
                                          .ToList();
            return res;
        }

        public tbl_SummerInternship GetByRegNo(string Regno)
        {
            var res = db.tbl_SummerInternships
                                          .FirstOrDefault(e => e.ScholarNumber == Regno);
            return res;


        }

        public string Add(tbl_SummerInternship newobj)
        {
            try
            {
                db.tbl_SummerInternships.Add(newobj);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }

        public string Update(tbl_SummerInternship newobj)
        {
            try
            {
                var existingobj = db.tbl_SummerInternships.FirstOrDefault(e => e.SummerInternshipId == newobj.SummerInternshipId);
                db.Entry(existingobj).CurrentValues.SetValues(newobj);
                db.SaveChanges();
                return "Success";
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
                var genralobj = db.tbl_SummerInternships.FirstOrDefault(e => e.SummerInternshipId == Id);
                if (genralobj != null)
                {
                    db.tbl_SummerInternships.Remove(genralobj);
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