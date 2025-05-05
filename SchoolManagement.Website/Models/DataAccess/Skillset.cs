using SchoolManagement.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class Skillset
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<tbl_skillset> GetAll()
        {
            List<tbl_skillset> res = db.tbl_skillsets.ToList();
            return res;
        }
        public List<tbl_skillset> GetByRegNo(string Regno)
        {
            List<tbl_skillset> res = db.tbl_skillsets
                                          .Where(e => e.ScholarNumber == Regno)
                                          .ToList();
            return res;


        }
        public List<tbl_skillset> GetById(int id)
        {

            List<tbl_skillset> res = db.tbl_skillsets
                                          .Where(e => e.SkillsetId == id)
                                          .ToList();
            return res;
        }
        public string Add(tbl_skillset newobj)
        {
            try
            {
                db.tbl_skillsets.Add(newobj);
                db.SaveChanges();
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "Failed";
            }
        }
        public string Update(tbl_skillset newobj)
        {
            try
            {
                var existingobj = db.tbl_skillsets.FirstOrDefault(e => e.SkillsetId == newobj.SkillsetId);
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
                var genralobj = db.tbl_skillsets.FirstOrDefault(e => e.SkillsetId == Id);
                if (genralobj != null)
                {
                    db.tbl_skillsets.Remove(genralobj);
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