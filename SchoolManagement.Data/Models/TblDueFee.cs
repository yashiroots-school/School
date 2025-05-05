using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class TblDueFee : BaseEntity
    {
        [Key]
        public int DueFeeId { get; set; }

        public int FeeHeadingId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FeeHeading { get; set; }
        

        public string Jan { get; set; }
        public string Feb { get; set; }
        public string Mar { get; set; }
        public string Apr { get; set; }
        public string May { get; set; }
        public string Jun { get; set; }
        public string Jul { get; set; }
        public string Aug { get; set; }
        public string Sep { get; set; }
        public string Oct { get; set; }
        public string Nov { get; set; }
        public string Dec { get; set; }
       
        public string PaidMonths { get; set; }
        public int ClassId { get; set; }
        public int CategoryId { get; set; }
        public string ClassName { get; set; }
        public string CategoryName { get; set; }        
        public string PayHeadings { get; set; }
        
        public float TotalFee { get; set; }
       
        public string FeePaids { get; set; }
        public string Course { get; set; }
        public string CourseSpecialization { get; set; }

        public string FeeReceiptsOneTimeCreator { get; set; }

        public float PaidAmount { get; set; }
        public float DueAmount { get; set; }
        
        public string UpdatedBy { get; set; }
    }
}
