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
   public class TblStudentFeeSaved: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int StudentId { get; set; }
        public float TotalFee { get; set; }
        public float FeePaid { get; set; }
        public float OldFee { get; set; }




    }
}
