using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class Tbl_TcAmount
    {
        [Key]
        public long Id { get; set; }
        public long Type { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
