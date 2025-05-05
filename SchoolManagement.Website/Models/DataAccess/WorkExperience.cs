using SchoolManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class WorkExperience
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<tbl_WorkExperience> GetAll()
        {
            List<tbl_WorkExperience> res = db.tbl_WorkExperiences.ToList();
            return res;
        }

        public List<tbl_WorkExperience> GetByRegNo(string Regno)
        {
            List<tbl_WorkExperience> res = db.tbl_WorkExperiences
                                          .Where(e => e.ScholarNumber == Regno)
                                          .ToList();
            return res;


        }

        public tbl_WorkExperience GetById(int id)
        {

            var res = db.tbl_WorkExperiences
                                          .FirstOrDefault(e => e.WorkExperienceId == id);
                                          
            return res;
        }

        public string Add(tbl_WorkExperience newobj)
        {
            try
            {
                db.tbl_WorkExperiences.Add(newobj);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }

        public string Update(tbl_WorkExperience newobj)
        {
            try
            {
                var existingobj = db.tbl_WorkExperiences.FirstOrDefault(e => e.WorkExperienceId == newobj.WorkExperienceId);
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
                var genralobj = db.tbl_WorkExperiences.FirstOrDefault(e => e.WorkExperienceId == Id);
                if (genralobj != null)
                {
                    db.tbl_WorkExperiences.Remove(genralobj);
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