using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class TodayFeeReportViewModel
    {
        public int SNO { get; set; }
        public string StudentName { get; set; }
        public string BillNo { get; set; }
        public string ScholarNumber { get; set; }
        public string Amount { get; set; }
        public string Heading { get; set; }
        public DateTime PaidDate { get; set; }
    }
}