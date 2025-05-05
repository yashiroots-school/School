using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class ChangeUserName
    {
        public int StudentId { get; set; }

        public string Student_Name { get; set; }

        public string Old_UserName { get; set; }

        public string User_Name { get; set; }

        public string Old_Password { get; set; }

        public string Password { get; set; }

        
    }
}