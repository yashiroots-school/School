using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class Tbl_MenuViewmodel
    {
        public int Menu_Id { get; set; }

        public string Menu_Name { get; set; }

        public string Upload_Image { get; set; }

        public int Submenu_Id { get; set; }

        public string Submenu_Name { get; set; }

        public string MenuName { get; set; }
        public string Submenu_Url { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }


    }
}