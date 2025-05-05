using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_BasicPayDetails
    {
        [Key]
        public int BasicAmount_Id { get; set; }

        public int SchoolCategory_Id { get; set; }

        public int BasicPay_Id { get; set; }

        public string Category_Name { get; set; }

        public string Basicpay_Name { get; set; }

        public float Basic_Amount { get; set; }

        public string CreatedDate { get; set; }
    }
}