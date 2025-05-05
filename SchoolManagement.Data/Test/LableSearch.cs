using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Test
{
    public class LableSearch
    {
        public int SchoolId { get; set; }

        public int subMenuId { get; set; }

    }

    public class LableResponse
    {

        public int Id { get; set; }


        public string Name { get; set; }

        public string Display { get; set; }
        public string LableId { get; set; }

        public bool IsActive { get; set; }  
    }
}
