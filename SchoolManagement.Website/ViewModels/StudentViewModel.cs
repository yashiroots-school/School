using SchoolManagement.Data.Models;
using SchoolManagement.Website.Models;

namespace SchoolManagement.Website.ViewModels
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        
        public FamilyDetail FamilyDetail { get; set; }
        public AdditionalInformation AdditionalInformation { get; set; }
        public PastSchoolingReport PastSchoolingReport { get; set; }
        public StudentRemoteAccess StudentRemoteAccess { get; set; }
        public GuardianDetails GuardianDetails { get; set; }
        public StudentsRegistration StudentRegistration { get; set; }
        public int RoutingId { get; set; }
        public Tbl_Batches tbl_Batches { get; set; }

    }


    public class SiblingsViewModel
    {
        public int Siblings_Id { get; set; }

        public int Student_Id { get; set; }

        public string Studentname { get; set; }
        public string Fathername { get; set; }
        public string Mothername { get; set; }
        public string SiblingClass { get; set; }
        public string SiblingSection { get; set; }
        public string ContactNumber { get; set; }
        public string Address { get; set; }

        public int Class_id { get; set; }

        public string Confirmation { get; set; }

        public int FamilyDetails_Id { get; set; }

        public string Classname { get; set; }

        public string Studentclass { get; set; }

        public string siblingsstudentname { get; set; }
        

    }

}
