using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Tbl_FreezeData
    {
        [Key]
        public int FreezeId { get; set; }
        public int TermId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public bool IsFreeze { get; set; }
        public int BatchId { get; set; }
    }
}
