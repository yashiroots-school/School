using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class tbl_PaymentTransactionFeeDetails 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PaymentFeedetailsID { get; set; }
        [ForeignKey("tbl_PaymentTransactionDetails")]
        public long PaymentTransactionId { get; set; }
        public tbl_PaymentTransactionDetails tbl_PaymentTransactionDetails { get; set; }
        public int FeeID { get; set; }
        [StringLength(100)]
        public string FeeAmount { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
