using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
   public class Tbl_DataList
    {
        [Key]
        public int DataListId { get; set; }
        public string DataListName { get; set; }
    }
}
