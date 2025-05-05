using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Tbl_SignatureSetup
    {
        [Key]
        public int SignatureSetupId { get; set; }

        public string Designation { get; set; }

        public string Signature { get; set; }

        public string FromClass { get; set; }

        public string ToClass { get; set; }

    }
}
