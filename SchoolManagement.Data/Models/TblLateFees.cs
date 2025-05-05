using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class TblLateFees:BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public int StudentId { get; set; }
        public int FeeHeadingId { get; set; }
        public float LateFee { get; set; }

        [DefaultValue(false)]
        public bool Paid { get; set; }


    }
}
