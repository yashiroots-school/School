using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_AccountType
    {
        [Key]
        public int Account_TypeId { get; set; }

        public string Account_Typename { get; set; }

        public string Created_Date { get; set; }
    }
}