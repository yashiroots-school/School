using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_BasicpayMaster
    {
        [Key]
        public int BasicPay_MasterId { get; set; }

        public string Basicpay_Name { get; set; }

        public string Created_Date { get; set; }
    }
}