using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class TransportFeeConfiguration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransportFeeConfigurationID { get; set; }
        [StringLength(100)]
        public string Class { get; set; }
        [StringLength(100)]
        public string BatchName { get; set; }        
        public int FromKM { get; set; }
        public int ToKM { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedOn { get; set; }

        public int Class_Id { get; set; }

        public int Batch_Id { get; set; }

    }
}
