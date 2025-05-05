using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class ExamTypes
    {
        [Key]
        public int Id { get; set; }
        public string ExamType { get; set; }
    }
}
