using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class NoticeViewModel
    {
        public string NoticeName { get; set; }
        public DateTime NoticeDate { get; set; }
        public int DaysAgo { get; set; }
    }
}