using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class StudentRegNumberMaster
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudnetRegNumberMasterID { get; set; }
        [StringLength(100)]
        public string Class { get; set; }
        [StringLength(100)]
        public string BatchName { get; set; }
        [StringLength(100)]
        public string RegPrefix { get; set; }
        public int RegLength { get; set; }
        public int RegNumberStartWith { get; set; }
        public DateTime CreatedOn { get; set; }
        public string RegStatus { get; set; }
        public int RegLastNumber { get; set; }

        public int Class_Id { get; set; }

        public int Batch_Id { get; set; }

    }
}
