using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_CoScholastic_Result
    {
        [Key]
        public long  Id { get; set; }
        public long BoardID { get; set; }
        public long? CoScholasticID { get; set; }
        public long StudentID { get; set; }
        public long ClassID { get; set; }
        public long? SectionId { get; set; }
        public long TermID { get; set; }
        public string ObtainedGrade { get; set; }
    }
}