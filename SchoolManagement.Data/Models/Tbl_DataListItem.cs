using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class Tbl_DataListItem
    {
        [Key]
        public int DataListItemId { get; set; }
        public string DataListName { get; set; }
        public string DataListItemName { get; set; }
        public string DataListId { get; set; }
        public string Status { get; set; }

    }
}
