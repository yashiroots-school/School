using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentFeePaidViewModel
    {
        public string   FeeHeading { get; set; }
        public string   Months { get; set; }
        public float   Amount { get; set; }
        public float   TotalAmount { get; set; }
        public int FeeheadingId { get; set; }
    }
}