using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class Frequencys :BaseEntity
    {
        [Key]
        public int FeeFrequencyId { get; set; }
        public string FeeFrequencyName { get; set; }

        //public ICollection<FeeHeadings> FeeHeadings { get; set; }

    }
}
