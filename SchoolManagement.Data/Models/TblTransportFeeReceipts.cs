using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class 
        TblTransportFeeReceipts : BaseEntity
    {
        [Key]
        public int FeeReceiptId { get; set; }

        public int FeeHeadingId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        [DefaultValue(false)]
        public bool Jan { get; set; }

        [DefaultValue(false)]
        public bool Feb { get; set; }

        [DefaultValue(false)]
        public bool Mar { get; set; }

        [DefaultValue(false)]
        public bool Apr { get; set; }

        [DefaultValue(false)]
        public bool May { get; set; }

        [DefaultValue(false)]
        public bool Jun { get; set; }

        [DefaultValue(false)]
        public bool Jul { get; set; }

        [DefaultValue(false)]
        public bool Aug { get; set; }

        [DefaultValue(false)]
        public bool Sep { get; set; }

        [DefaultValue(false)]
        public bool Oct { get; set; }

        [DefaultValue(false)]
        public bool Nov { get; set; }

        [DefaultValue(false)]
        public bool Dec { get; set; }
        public string Type { get; set; }
        public string PaidMonths { get; set; }
        public int ClassId { get; set; }
        public int CategoryId { get; set; }
        public string ClassName { get; set; }
        public string CategoryName { get; set; }
        public float Concession { get; set; }
        public float ConcessionAmt { get; set; }
        public string PayHeadings { get; set; }
        public float OldBalance { get; set; }
        public float ReceiptAmt { get; set; }

        public float TotalFee { get; set; }
        public float LateFee { get; set; }

        public float BalanceAmt { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string CheckId { get; set; }
        public string Remark { get; set; }
        public string FeePaids { get; set; }

        public string FeeReceiptsOneTimeCreator { get; set; }
        //public DateTime? FeePaidDate { get; set; }
        public string DueAmount { get; set; }
        public string PaidAmount { get; set; }
    }
}
