using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.Payment
{
    public class PaymentTransactionId
    {
        public string Paymentid { get; set; }

        public string Orderid { get; set; }

        public string Merchant_Key { get; set; }

        public string Secret_Key { get; set; }

    }
}