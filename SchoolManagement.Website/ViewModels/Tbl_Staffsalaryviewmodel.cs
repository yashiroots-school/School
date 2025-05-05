using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class Tbl_Staffsalaryviewmodel
    {
        public int Salary_Id { get; set; }

        public int Staff_ID { get; set; }

        public string Staff_Name { get; set; }

        public int Salary_Amount { get; set; }

        public string CreatedDate { get; set; }

        public int Basic_Amount { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }

    }
}