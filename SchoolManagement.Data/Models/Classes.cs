using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class Classes
    {
        [Key]
        public int Id { get; set; }
        public string ClassName { get; set; }
    }
}
