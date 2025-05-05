using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Test
{
   

    public class TableColumnss
    {
        public int HeadingId { get; set; }
        public int OrderNo { get; set; }
        public string FeeName { get; set; }

        public decimal Total { get; set; }
             
    }



    public class ReportInput
    {

        public string SchollType
        {
            get; set;
        }


        public string BatchId { get; set; }

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}
