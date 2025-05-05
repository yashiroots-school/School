using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class FeeReceiptViewModel
    {
        public int FeeReceiptId { get; set; }
        public int FeeHeadingId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public bool Jan { get; set; }
        public bool Feb { get; set; }
        public bool Mar { get; set; }
        public bool Apr { get; set; }
        public bool May { get; set; }
        public bool Jun { get; set; }
        public bool Jul { get; set; }
        public bool Aug { get; set; }
        public bool Sep { get; set; }
        public bool Oct { get; set; }
        public bool Nov { get; set; }
        public bool Dec { get; set; }
        public string Type { get; set; }
        public string PaidMonths { get; set; }
        public int ClassId { get; set; }
        public int CategoryId { get; set; }
        public float Concession { get; set; }
        public float ConcessionAmt { get; set; }
        public string PayHeadings { get; set; }
        public float OldBalance { get; set; }
        public float ReceiptAmt { get; set; }

        public string ClassName { get; set; }
        public string CategoryName { get; set; }
        public string[] Selectedmonths { get; set; }
        public string[] FeeHeadings { get; set; }
        public float[] FeeHeadingAmt { get; set; }
        public float TotalFee { get; set; }
        public float LateFee { get; set; }

        public float BalanceAmt { get; set; }
        public string PaymentMode { get; set; }
        public string BankName { get; set; }
        public string CheckId { get; set; }
        public string Remark { get; set; }
        public string DateTimeVal { get; set; }

        public float[] collectedFeeAmt { get; set; }

        public float[] collectFees { get; set; } //Collect Fee
        public string DueAmountYesNo { get; set; }  // if due amount is there then yes.if clear then no.
        public string BatchName { get; set; }
        public string CourseSpecialization { get; set; }

        public string BillDate { get; set; }
        public string ApplicationNumber { get; set; }
        public string DueYear { get; set; }

        public string DueFee { get; set; }

    }
}