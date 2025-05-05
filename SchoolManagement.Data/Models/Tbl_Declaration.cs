using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    
    public class tbl_Declaration
    {
        [Key]
        public long DeclarationId { get; set; }

        [Required]
        [StringLength(50)]
        public string ScholarNumber { get; set; }

        [StringLength(10)]
        public string Interesterd { get; set; }

        [StringLength(512)]
        public string NotInterested { get; set; }

        [StringLength(10)]
        public string Relocate { get; set; }

        [StringLength(256)]
        public string StudentName { get; set; }

        [StringLength(10)]
        public string Agree { get; set; }

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

        public virtual tbl_StudentDetail tbl_StudentDetail { get; set; }
    }
}
