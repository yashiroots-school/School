using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class FeeReceiptTbl {
        public List<PreviewFeeReceiptViewModel> allHeadings { get; set; }
    }

    public class PreviewFeeReceiptViewModel
    {
        public string HeadingNames { get; set; }
        public string SelectedMonths { get; set; }
        public string  FeePaid { get; set; }
        public string CreatedDate { get; set; }
        public string HeadingPaidAmount { get; set; }  

        //public List<Headings> allHeadingsDetails { get; set; }
    }
    
}