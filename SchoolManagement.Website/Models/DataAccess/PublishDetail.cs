using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models.DataAccess
{
    public class PublishDetail
    {

        public int ClassId { get; set; }
        public int SectionId { get; set; }
        public int TermId { get; set; }
        public int BatchID { get; set; }
        public bool IsPublish { get; set; }

    }
}
