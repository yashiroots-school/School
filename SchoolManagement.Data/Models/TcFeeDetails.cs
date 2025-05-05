using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class TcFeeDetails
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("studentFeeDetails")]
        public long? StudentTcDetailsId { get; set; }
        public StudentTcDetails studentFeeDetails { get; set; }
        [ForeignKey("student")]
        public int StudentId { get; set; }
        public Student student { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string PaymentMode { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime PaidDate { get; set; }
        public bool? IsTcfee{ get; set; }
    }
}
