using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
   public  class Tbl_CreateBank
    {
        [Key]
        public int Bank_Id { get; set; }

        public string Bank_Name { get; set; }

        public string Bank_Code { get; set; }

        public string Contact_No { get; set; }

        public string LandlineNo { get; set; }

        public string Contactperson_Name { get; set; }


    }
}
