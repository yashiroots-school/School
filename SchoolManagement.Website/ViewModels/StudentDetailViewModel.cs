using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentDetailViewModel
    {
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string Category { get; set; }
        public float OldBalance { get; set; }
        public string FatherName { get; set; }
        public string RoleNo { get; set; }
        public string Contact { get; set; }
        public int ClassId { get; set; }
        public int CategoryId { get; set; }
        public string RoleNumber { get; set; }
        public string Batch { get; set; }
    }
}