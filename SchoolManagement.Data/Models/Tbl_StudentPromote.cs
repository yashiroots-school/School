using SchoolManagement.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
   public class Tbl_StudentPromote: BaseEntity
    {
        [Key]
        public int PromoteId { get; set; }
        public string ScholarNumber { get; set; }
        public string FromClass { get; set; }
        public string ToClass { get; set; }
        public string ToSection { get; set; }
        public int FromClass_Id { get; set; }
        public int ToClass_Id { get; set; }
        public int Student_Id { get; set; }
        public string Registration_Date { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public int Batch_Id { get; set; }
        public int Section_Id { get; set; }


    }
}
