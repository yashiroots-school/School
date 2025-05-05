using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class TblUserDynamicConfiguration
    {
        [Key]
        public int Mainid { get; set; }

        public string ListType { get; set; }

        public string ListData { get; set; }

        public string CurrentUser { get; set; }
    }
}
