using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class DueFeeModel
    {
        public int FeeReceiptId { get; set; }
        public int FeeHeadingId { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string FeeHeading { get; set; }
        public string[] Jan { get; set; }
        public string[] Feb { get; set; }
        public string[] Mar { get; set; }
        public string[] Apr { get; set; }
        public string[] May { get; set; }
        public string[] Jun { get; set; }
        public string[] Jul { get; set; }
        public string[] Aug { get; set; }
        public string[] Sep { get; set; }
        public string[] Oct { get; set; }
        public string[] Nov { get; set; }
        public string[] Dec { get; set; }
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
        public string Course { get; set; }
        public float[] collectFees { get; set; } //Collect Fee
        public string DueAmountYesNo { get; set; }  // if due amount is there then yes.if clear then no.
        public string BatchName { get; set; }
        public string CourseSpecialization { get; set; }
        public string ApplicationId { get; set; }
    }
}