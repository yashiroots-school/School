using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace SchoolManagement.Website.Models
{
    public class Tbl_DataListItems
    {
        [Key]
        [Required]
        public string DataListItemName { get; set; }
        [Required]
         public int DataListId { get; set; }
        [Required]
         public string DataListName { get; set; }

        
    }
}