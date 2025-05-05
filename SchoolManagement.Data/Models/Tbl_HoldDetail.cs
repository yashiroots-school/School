using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Tbl_HoldDetail
    {
        [Key]
        public int HoldId { get; set; }
        public int TermId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public bool IsHold { get; set; }
        public int BatchId { get; set; }
	    public int StudentId { get; set; }
	    public string Remark { get; set; }
        public DateTime HoldDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
