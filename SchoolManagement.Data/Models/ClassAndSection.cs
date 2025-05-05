using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class ClassAndSection
    {
        [Key]
        public int Id { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string OtherSection { get; set; }

    }
}
