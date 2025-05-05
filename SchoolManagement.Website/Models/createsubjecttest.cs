using SchoolManagement.Data.Models;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Web;
using System.Collections.Generic;


namespace SchoolManagement.Website.Models
{
    public class CreatesubjecttestModel
    {
        public int BatchId { get; set; }
        public int TermId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }

        // View me isko use karenge
        public List<SubjectTestDTO> SubjectTestList { get; set; }
    }

    // View ke liye short DTO
    public class SubjectTestDTO
    {
        public long SubjectId { get; set; }         // ✅ INT now
        public string SubjectName { get; set; }
        public bool? IsOptional { get; set; }
    }


    // SQL se map hone wala DTO
    public class SubjectTestFullDTO
    {
        public long Subject_ID { get; set; }         // from Tbl_SubjectsSetup
        public int Class_Id { get; set; }           // from Tbl_SubjectsSetup
        public int Batch_Id { get; set; }           // from Tbl_SubjectsSetup
        public int Section_Id { get; set; }         // from Tbl_SubjectsSetup
        public string Subject_Name { get; set; }    // from Tbl_SubjectsSetup
        public long? TermID { get; set; }           // from Tbl_Tests (BIGINT → long?)
        public bool? IsOptional { get; set; }       // from Tbl_Tests (BIT → bool?)
    }

    public class SubjectTestSaveDTO
    {
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public bool? IsOptional { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int BatchId { get; set; }
        public int TermId { get; set; }
        public int SerialNumber { get; set; }
    }
    public class SubjectSerial
    {
        public int SubjectSerialId { get; set; }
        public long SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int BatchId { get; set; }
        public long TermId { get; set; }
        public bool? IsOptional { get; set; }
        public int SerialNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
