using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Data.Models
{
    public class Tbl_SchoolSetup:BaseEntity
    {
        [Key]
        public int Schoolsetup_Id { get; set; }

        public int School_Id { get; set; }

        public int Bank_Id { get; set; }

        public int Branch_Id { get; set; }

        public int Merchant_nameId { get; set; }

        public string Status { get; set; }

        public string Fee_configurationId { get; set; }
        public string Fee_Configuratinname { get; set; }



    }
}
