using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class promotionviewmodel
    {
        public int StudentID { get; set; }

        public string[] Name { get; set; }

        public string[] Username { get; set; }

        public string[] Last_Name { get; set; }

        public string[] Gender { get; set; }

        public string[] DOB { get; set; }


        public string CheckAll { get; set; }
    }
}