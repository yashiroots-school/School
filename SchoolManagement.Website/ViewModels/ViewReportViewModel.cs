using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class ViewReportViewModel
    {
        public string ClassName { get; set; }
        public float GrossFee { get; set; }
        public float DueFee { get; set; }
        public float ReceiveFee { get; set; }
    }
}