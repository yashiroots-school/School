using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class ReportHeading
    {
        [Key]
        public long Id { get; set; }
        public long ReportId { get; set; }
        public int HeadingId { get; set; }
        public int OrderNo { get; set; }
    }
}
