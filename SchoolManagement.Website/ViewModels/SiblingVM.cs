using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class SiblingsVM
    {
        public string Fathername { get; set; }
        public string Mothername { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }
        public string StudentClass { get; set; }
        public int StudentclassID { get; set; }
        public string StudentName { get; set; }
        public int Student_Id { get; set; }
        public List<SiblingDataVM> SiblingsData { get; set; }
    }

    public class SiblingDataVM
    {
        public int Siblings_Id { get; set; }
        public string Confirmation { get; set; }
        public int SibClassID { get; set; }
        public string siblingsstudentname { get; set; }
        public string siblingsStudentclass { get; set; }
        public int Student_Id { get; set; }
        public int StudentclassID { get; set; }
        public string Studentname { get; set; }
        public string SiblingStudentName { get; set; }
        public string SiblingStudentClass { get; set; }
    }
}
