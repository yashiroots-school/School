using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Website.Models.Master
{
    public class SignatureSetup
    {
        public int SignatureSetupId { get; set; }

        public string Designation { get; set; }

        public string Signature { get; set; }

        public string FromClass { get; set; }

        public string ToClass { get; set; }
    }
}
