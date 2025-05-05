using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
   public class Tbl_Deductions:BaseEntity
    {
        [Key]
        public int Deductions_Id { get; set; }

        public int Staff_Id { get; set; }

        public string Staff_Name { get; set; }

        public int Net_Pay { get; set; }

        public int Deduction_Amt { get; set; }

        public string Added_Date { get; set; }

        public string Added_Month { get; set; }

        public string Added_Year { get; set; }

        public string Added_Day { get; set; }

        public string Remarks { get; set; }

    }
}
