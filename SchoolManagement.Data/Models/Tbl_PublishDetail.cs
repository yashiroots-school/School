using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Tbl_PublishDetail
    {
        [Key]
        public int PublishId { get; set; }
        public int TermId { get; set; }
        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public bool IsPublish { get; set; }
        public int BatchId { get; set; }
        public string PublishBy { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
