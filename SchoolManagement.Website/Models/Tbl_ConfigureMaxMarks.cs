using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Website.Models
{
    public class Tbl_ConfigureMaxMarks
    {
        [Key]
        public int Subject_Id { get; set; }
        public string Table_Name { get; set; }
        public int MaximumMark { get; set; }
        public string Classes { get; set; }
        public DateTime CreateDate { get; set; }
        public string PropertyName { get; set; }
        public DateTime ModifyDate { get; set; }
        public int BatchId { get; set; }
    }
}