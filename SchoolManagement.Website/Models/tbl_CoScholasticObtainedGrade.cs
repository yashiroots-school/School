using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class tbl_CoScholasticObtainedGrade
    {
        public long Id { get; set; }
        public long ObtainedCoScholasticID { get; set; }
        public long CoscholasticID { get; set; }
        public string ObtainedGrade { get; set; }
        public int? BatchId {  get; set; }
    }
}