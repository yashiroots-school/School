using System.ComponentModel.DataAnnotations;

namespace SchoolManagement.Data.Models
{
    public class LabelControl
    {
        [Key]
        public int Id { get; set; }

        public int LableId { get; set; }


        public string LabelName { get; set; }


        public bool IsActive { get; set; }


        public int School_Id { get; set; }

    }
}
