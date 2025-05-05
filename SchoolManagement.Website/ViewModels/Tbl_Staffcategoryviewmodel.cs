using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class Tbl_Staffcategoryviewmodel
    {
        public int Staff_Category_Id { get; set; }

        public string Category_Name { get; set; }

        public string Created_Date { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }
    }
}