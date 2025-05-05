using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SchoolManagement.Entities;

namespace SchoolManagement.Website.ViewModels
{
    public class Tbl_Feereceiptsviewmodel:BaseEntity
    {
        public int FeeReceiptId { get; set; }

        public string FeeHeadingIDs { get; set; }

        public int? StudentId { get; set; }
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
        public string DueAmount { get; set; }
        public string PaidAmount { get; set; }
        public string ApplicationNumber { get; set; }

        public string CreatePermission { get; set; }

        public string Editpermission { get; set; }

        public string DeletePermission { get; set; }

        public string ViewPermission { get; set; }



        public string DateFrom { get; set; }
        public string DateTo { get; set; }

    }
}