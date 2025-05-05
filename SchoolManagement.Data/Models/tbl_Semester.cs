using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
   public class tbl_Semester
    {
        [Key]
        public long SemesterId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(20)]
        public string Year { get; set; }

        [StringLength(20)]
        public string Sem { get; set; }

        [StringLength(50)]
        public string Percentage { get; set; }


        // public decimal Percentage { get; set; }


        [StringLength(20)]
        public string Addedon { get; set; }

        [StringLength(20)]
        public string Addeby { get; set; }

        [StringLength(20)]
        public string Updatedon { get; set; }

        [StringLength(20)]
        public string Updatedby { get; set; }

        [StringLength(35)]
        public string Spare1 { get; set; }

        [StringLength(35)]
        public string Spare2 { get; set; }

        [StringLength(35)]
        public string Spare3 { get; set; }

        public decimal perse2 { get; set; }

        public decimal Persentagegrade { get; set; }

    }
}
