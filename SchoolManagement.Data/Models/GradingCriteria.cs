using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class GradingCriteria
    {
        [Key]
        public long CriteriaID { get; set; }
        //public long MinimumPercentage { get; set; }
        //public long MaximumPercentage { get; set; }
        public decimal MinimumPercentage { get; set; }
        public decimal MaximumPercentage { get; set; }
        public string Grade { get; set; }
        public string GradeDescription { get; set; }
        public long BoardID { get; set; }
       // public long TestID { get; set; }
        public long ClassID { get; set; }
        public long BatchID { get; set; }
        public long TermID { get; set; }
        public string TestName { get; set; }
        public string ClassName { get; set; }
        public string TermName { get; set; }
        public string BatchName { get; set; }
       


    }
}
