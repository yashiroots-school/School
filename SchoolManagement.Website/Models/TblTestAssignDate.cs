using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class TblTestAssignDate
    {
        [Key]
        public long TestAssignID { get; set; }
        public long TestID { get; set; }
        public long ClassID { get; set; }
        public long BatchID { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }   
    }
}
