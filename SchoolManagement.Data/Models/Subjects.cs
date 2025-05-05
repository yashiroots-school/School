using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class Subjects
    {
        [Key]
        public int Id { get; set; }
        public string Teacher { get; set; }
        public string Class { get; set; }
        public string Subject { get; set; }
        public string Section { get; set; }

        public int StaffId { get; set; }
        public int Class_Id { get; set; }
        public int Subject_ID { get; set; }
        public int Batch_Id { get; set; }
        public bool Class_Teacher { get; set; }
        public int Section_Id { get; set; }
        [NotMapped]
        public string BatchName { get; set; } = string.Empty;
    }
}
