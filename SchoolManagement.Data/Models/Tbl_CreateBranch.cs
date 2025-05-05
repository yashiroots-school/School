using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
  public class Tbl_CreateBranch
    {
        [Key]
        public int Branch_ID { get; set; }

        public int Bank_Id { get; set; }

        public string Bank_Name { get; set; }

        public string Branch_Name { get; set; }

        public string Contact_No { get; set; }

        public string Contact_Name { get; set; }

        public string Landline_No { get; set; }


    }
}
