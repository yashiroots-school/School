using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.StudentRemarkModel
{
    public class Tbl_StudentRemark
    {
        [Key]
        public int StudentRemarkId { get; set; }
        public string Reward { get; set; }
        public string Awards { get; set; }
        public string Punishment { get; set; }
        public int Class_Id { get; set; }
        public int Section_Id { get; set; }
        public int Batch_Id { get; set; }
        public int StudentId { get; set; }
        public int TermID { get; set; }
        public string Remark { get; set; }
    }
}