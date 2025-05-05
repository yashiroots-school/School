using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class StudentTcRceiptViewModel
    {
        public long Id { get; set; }
        public string Studnt { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public string ScollarNumber { get; set; }
        public string Batch { get; set; }
        public string paymode { get; set; }
        public DateTime createdon { get; set; }
        public decimal Amount { get; set; }
        public bool? IsTcfree { get; set; }
    }
}