using SchoolManagement.Data.Models;
using SchoolManagement.Website.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class AllStaffDetails
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public List<StafsDetails> GetAll()
        {
            List<StafsDetails> res = db.StafsDetails.ToList();
            return res;
        }

        public List<StaffDetailsVM>
               GetAllDetails()
        {
            List<StaffDetailsVM> staff = new List<StaffDetailsVM>();
            var result = db.StafsDetails.ToList();
            foreach (var res in result)
            {
                StaffDetailsVM staffobj = new StaffDetailsVM();

                staffobj.EmpId = res.EmpId;
                staffobj.Name = res.Name;
                staffobj.Qualification = res.Qualification;
                staffobj.Gender = res.Gender;
                staffobj.EmpDate = res.EmpDate;
                staffobj.File = res.File;
                staff.Add(staffobj);
            }

            return staff;
        }

    }
}