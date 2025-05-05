using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Student_ElectiveRecord
    {
        public long Id { get; set; }
        public long StudentId { get; set; }
        public long ElectiveSubjectId { get; set; }
    }

}