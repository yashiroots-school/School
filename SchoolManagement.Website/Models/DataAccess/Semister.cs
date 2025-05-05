using SchoolManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class Semister
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<tbl_Semester> GetAll()
        {
            List<tbl_Semester> res = db.tbl_Semesters.ToList();
            return res;
        }

        public List<tbl_Semester> GetByRegNo(string Regno)
        {
            List<tbl_Semester> res = db.tbl_Semesters
                                          .Where(e => e.ScholarNumber == Regno)
                                          .ToList();
            return res;


        }

        public List<tbl_Semester> GetById(int id)
        {

            List<tbl_Semester> res = db.tbl_Semesters
                                          .Where(e => e.SemesterId == id)
                                          .ToList();
            return res;
        }

        public string Add(tbl_Semester newobj)
        {
            try
            {
                db.tbl_Semesters.Add(newobj);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }

        public string Update(tbl_Semester newobj)
        {
            try
            {
                var existingobj = db.tbl_Semesters.FirstOrDefault(e => e.SemesterId == newobj.SemesterId);

                existingobj.Percentage = newobj.Percentage;
                existingobj.Year = newobj.Year;
                existingobj.Sem = newobj.Sem;

                //db.Entry(existingobj).CurrentValues.SetValues(newobj);
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
                var genralobj = db.tbl_Semesters.FirstOrDefault(e => e.SemesterId == Id);
                if (genralobj != null)
                {
                    db.tbl_Semesters.Remove(genralobj);
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
