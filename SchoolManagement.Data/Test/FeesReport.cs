using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Test
{
    public class FeesReport
    {
        public int FeeReceiptId { get; set; }  //---x-rnik---
        public string Name { get; set; }
        public string ApplicationNumber { get; set; }
        public long RegNumber { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string PaidAmount { get; set; }
        

        public DateTime AddedDate { get; set; }


        public List<long> HeadingIDs
        {
            get; set;
        }

        public List<decimal> Paids { get; set; }

        
        public string FeePaids
        {
            get
            {
                if (Paids != null)
                    return string.Join(",", Paids);

                return string.Empty;
            }
            set

            {
                value = value ?? string.Empty;

                Paids = value.Split(',')
                                     .Where(i => decimal.TryParse(i, out _))
                                     .Select(decimal.Parse)
                                     .OrderBy(o => o).ToList();
            }
        }

        
        public string FeeHeadingIDs
        {
            get
            {
                if (HeadingIDs != null)
                    return string.Join(",", HeadingIDs);

                return string.Empty;
            }
            set

            {
                value = value ?? string.Empty;

                HeadingIDs = value.Split(',')
                                     .Where(i => long.TryParse(i, out _))
                                     .Select(long.Parse)
                                     .OrderBy(o => o).ToList();
            }
        }

        public string PaymentMode { get; set; }
    }

}
