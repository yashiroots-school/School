using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class tbl_CommonDataListItem
    {
        [Key]
        public int DatalistId { get; set; }

        [StringLength(500)]
        public string DataListName { get; set; }

        [StringLength(500)]
        public string DataListItemName { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(35)]
        public string Spare1 { get; set; }

        [StringLength(35)]
        public string Spare2 { get; set; }

        [StringLength(35)]
        public string Spare3 { get; set; }
    }
}
