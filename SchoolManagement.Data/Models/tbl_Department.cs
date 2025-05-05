using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class tbl_Department
    {
        [Key]
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
