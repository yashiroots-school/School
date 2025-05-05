using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class MasterLabel
    {

        public string Name { get; set; }

        public string Description { get; set; }

        public string LableId { get; set; }

        [Key]
        public int Id { get; set; }

        public int SubMenu_Id { get; set; }
    }
}
