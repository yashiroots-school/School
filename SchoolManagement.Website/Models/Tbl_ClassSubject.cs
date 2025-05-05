using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_ClassSubject
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long SubjectId { get; set; }
        public long BoardId { get; set; }
        public bool? IsElective { get; set; }
    }
}