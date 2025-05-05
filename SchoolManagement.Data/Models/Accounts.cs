using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class Accounts:BaseEntity
    {
        [Key]
        public int AccountId { get; set; }
        public string AccountName { get; set; }

        public ICollection<FeeHeadings> FeeHeadings { get; set; }
    }
}
