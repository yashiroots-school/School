using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class FeeHeadingGroups:BaseEntity
    {
        [Key]
        public int FeeHeadingGroupId { get; set; }
        public string FeeHeadingGroupName { get; set; }

        public ICollection<FeeHeadings> FeeHeadings { get; set; }
    }
}
