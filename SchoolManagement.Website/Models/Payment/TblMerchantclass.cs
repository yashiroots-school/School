using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.Payment
{
    public class TblMerchantclass
    {
        public int MerchantName_Id { get; set; }
        public string MerchantName { get; set; }
        public int School_Id { get; set; }
        public int Bank_Id { get; set; }
        public int Branch_Id { get; set; }
        public string Schoolname { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public int MerchantId { get; set; }
        public string MerchantKey { get; set; }
        public string MerchantMID { get; set; }
        public int Schoolsetup_Id { get; set; }
        public string Status { get; set; }
        public string Feeconfiguration { get; set; }

    }
}