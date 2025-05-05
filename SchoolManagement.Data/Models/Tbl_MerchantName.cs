using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
    public class Tbl_MerchantName:BaseEntity
    {
        [Key]
        public int MerchantName_Id { get; set; }

        public string MerchantName { get; set; }

        public int School_Id { get; set; }

        public int Bank_Id { get; set; }

        public int Branch_Id { get; set; }

    }
}
