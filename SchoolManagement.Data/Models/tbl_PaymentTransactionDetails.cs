using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class tbl_PaymentTransactionDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PaymentTransactionId { get; set; }
        [StringLength(1000)]
        public string TransactionStatus { get; set; }
        [StringLength(1000)]
        public string TransactionError { get; set; }
        [StringLength(30)]
        public string TxnDate { get; set; }
        [StringLength(20)]
        public string Amount { get; set; }
        [StringLength(100)]
        public string TransactionId { get; set; }
        [StringLength(100)]
        public string TrackId { get; set; }
        [StringLength(100)]
        public string ReferenceNo { get; set; }
        [StringLength(100)]
        public string Pmntmode { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        [StringLength(100)]
        public string Card { get; set; }
        [StringLength(100)]
        public string CardType { get; set; }
        [StringLength(100)]
        public string Member { get; set; }
        [StringLength(100)]
        public string PaymentId { get; set; }
        public int? StudentId { get; set; }        
        public string ApplicationNumber { get; set; }
    }
}
