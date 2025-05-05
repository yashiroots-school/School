using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.StaffRemakModel
{
    public class StaffRemark
    {
        [Key]
        public int Staff_Pk { get; set; }
        public string Reward { get; set; }
        public string Awards { get; set; }
        public string Remark { get; set; }
        public int StaffId_fk { get; set; }

    }
}