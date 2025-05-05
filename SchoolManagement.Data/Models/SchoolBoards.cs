using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class SchoolBoards
    {
        [Key]
        public long BoardID { get; set; }
        public string BoardName { get; set; }
    }
}
