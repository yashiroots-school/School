using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentTotalFeeViewModel
    {
        public string ScholarNumber { get; set; }
        public string StudentName { get; set; }
        public string Batch { get; set; }
        public string Course { get; set; }
        public string Semester { get; set; }
        public float PaidAmount { get; set; }
        public float PendingAmount { get; set; }
        public float ConcesionAmount { get; set; }
        public float TotalAmount { get; set; }
        public float oldAmmount { get; set; }

        public string FeeName { get; set; }

    }
}