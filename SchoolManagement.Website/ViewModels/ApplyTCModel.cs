using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class ApplyTCModel
    {
        public List<string> uinList { get; set; }
        public List<long> studentIdList { get; set; }

        public string batch { get; set; }
        public string className { get; set; }

        public Nullable<bool> isCancelTC { get; set; } = false;
    }
}