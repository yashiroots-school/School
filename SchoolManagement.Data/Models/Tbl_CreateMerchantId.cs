using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
    public class Tbl_CreateMerchantId : BaseEntity
    {
        [Key]
        public int Merchant_Id { get; set; }

        public int School_Id { get; set; }

        public int Bank_Id { get; set; }

        public int Branch_Id { get; set; }

        public int MerchantName_Id { get; set; }

        public string MerchantMID { get; set; }

        public string MerchantKey { get; set; }

        public string UserId { get; set; }//user input
        public string Password { get; set; }

    }
}
