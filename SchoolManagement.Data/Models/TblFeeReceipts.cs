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
   public class TblFeeReceipts:BaseEntity
    {
        [Key]
        public int FeeReceiptId { get; set; }

        public string FeeHeadingIDs { get; set; }

        public int? StudentId { get; set; }
        public string StudentName { get; set; }=string.Empty;
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
        public string Type { get; set; }= string.Empty;
        public string PaidMonths { get; set; } = string.Empty;
        public int ClassId { get; set; }
        public int CategoryId { get; set; }
       
        public string ClassName { get; set; } = string.Empty;
        public string CategoryName { get; set; }=string.Empty;
        public float Concession { get; set; }
        public float ConcessionAmt { get; set; }
        public string PayHeadings { get; set; }= string.Empty;
        public float OldBalance { get; set; }
        public float ReceiptAmt { get; set; }
         
        public float TotalFee { get; set; }
        public float LateFee { get; set; }

        public float BalanceAmt { get; set; }
        public string PaymentMode { get; set; } = string.Empty;
        public string BankName { get; set; }=string.Empty ;
        public string CheckId { get; set; } = string.Empty;
        public string Remark { get; set; } = string.Empty;
        public string FeePaids { get; set; } = string.Empty;

        public string FeeReceiptsOneTimeCreator { get; set; } = string.Empty;
        //public DateTime? FeePaidDate { get; set; }
        public string DueAmount { get; set; } = string.Empty;
        public string PaidAmount { get; set; } = string.Empty;
        public string ApplicationNumber { get; set; }=string.Empty;

        public string FeeconfigurationId { get; set; } = string.Empty;
        public string Feeconfigurationname { get; set; } = string.Empty;
        public string Section { get; set; } = string.Empty;

    }
}
