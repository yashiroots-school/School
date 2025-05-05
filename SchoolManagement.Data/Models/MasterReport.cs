using System;
using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    //public partial class MasterReport

    public   class MasterReport
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
