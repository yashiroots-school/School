using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class SmsEailViewModel
    {
        public string[] Class_Name { get; set; }

        public string[] First_name { get; set; }

        public string EmailSubject { get; set; }

        public string EmailBody { get; set; }

        public string AttachedFile { get; set; }

        public int Classid { get; set; }

        public int[] StudentId { get; set; }

        public HttpPostedFileBase AttachementFile { get; set; }

    }
}