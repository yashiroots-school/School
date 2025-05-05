using System;

namespace SchoolManagement.Website.Models
{
    public class TcDetailsViewModel
    {
      
        public int? StudentTcDetailsId { get; set; }
        public int StudentId { get; set; }
        public decimal ReceiptAmount { get; set; }
        public string PaymentMode { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}