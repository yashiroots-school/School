using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Departments
    {
        [Key]
        public int Id { get; set; }
        public string Depertment { get; set; }
        public string Other { get; set; }
    }
}
