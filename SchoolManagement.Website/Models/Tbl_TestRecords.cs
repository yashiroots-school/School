using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_TestRecords
    {
        [Key]
        public long RecordID { get; set; }
        public long StudentID { get; set; }
        public long ClassID { get; set; }
        public long SectionID { get; set; }
        public long TestID { get; set; }
        public long TermID { get; set; }
        public long BoardID { get; set; }
        public int? BatchId { get; set; }
        public int? RankInClass { get; set; }
        public decimal?  ObtainedMarks { get; set; }
    }
}