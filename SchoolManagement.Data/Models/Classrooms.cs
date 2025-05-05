using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class Classrooms

    {
        [Key]
        public int Id { get; set; }
        public string className { get; set; }
        public string RoomNo { get; set; }
        public string RoomType { get; set; }
        public string Seatingcapacity { get; set; }
        public string Location { get; set; }
        public string Remarks { get; set; }
    }
}
