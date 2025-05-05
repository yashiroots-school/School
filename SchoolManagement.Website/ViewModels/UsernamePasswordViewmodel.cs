using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class UsernamePasswordViewmodel
    {

        public string[] Firstname { get; set; }

        public string[] Lastname { get; set; }

        public string[] Username { get; set; }

        public string[] Password { get; set; }

        public string[] Parentemail { get; set; }

        public string[] Classname { get; set; }

        public int Classid { get; set; }

        public string Date { get; set; }

        public string CheckAll { get; set; }

        public string[] Userid { get; set; }

    }


    public class EmailViewModel
    {
        public int Student_id { get; set; }

        public string ApplicationNumber { get; set; }

        public string Name { get; set; }

        public string Parent_Email { get; set; }

        public string Email_Date { get; set; }

        public string Email_Content { get; set; }
    }

}