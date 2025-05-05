using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolManagement.Data.Models
{
    public class PeriodSchedule
    {
        [Key]
        public int Id { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Day { get; set; }
        public string RoomNo { get; set; }

        public string Period1 { get; set; }
        public string Teacher1 { get; set; }

        public string Period2 { get; set; }
        public string Teacher2 { get; set; }

        public string Period3 { get; set; }
        public string Teacher3 { get; set; }

        public string Period4 { get; set; }
        public string Teacher4 { get; set; }

        public string Period5 { get; set; }
        public string Teacher5 { get; set; }

        public string Period6 { get; set; }
        public string Teacher6 { get; set; }

        public string Period7 { get; set; }
        public string Teacher7 { get; set; }

        public string Period8 { get; set; }
        public string Teacher8 { get; set; }

        public string Period9 { get; set; }
        public string Teacher9 { get; set; }

        public string Period10 { get; set; }
        public string Teacher10{ get; set; }

        public string Period11 { get; set; }
        public string Teacher11{ get; set; }

        public string Period12 { get; set; }
        public string Teacher12{ get; set; }

        public string Period13 { get; set; }
        public string Teacher13{ get; set; }

        public string Period14{get; set; }
        public string Teacher14{ get; set; }

        public string Period15{ get; set; }
        public string Teacher15{ get; set; }
    }
}
