using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Remark
    {
        [Key]
        public long RemarkId { get; set; } // bigint -> long

        public string Remark { get; set; } // varchar(MAX) -> string

        public long TermId { get; set; } // bigint -> long

        public long? BoardId { get; set; } // bigint -> long
        public long? StudentId { get; set; } // bigint -> long
        public int? BatchId { get; set; } // bigint -> long
    }
}