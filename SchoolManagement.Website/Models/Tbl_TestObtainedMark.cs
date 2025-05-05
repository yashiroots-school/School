using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class Tbl_TestObtainedMark
    {
        public long Id { get; set; } // bigint -> long

        public long? RecordIDFK { get; set; } // bigint -> long

        public decimal? ObtainedMarks { get; set; } // decimal(18, 0) -> decimal

        public long TestID { get; set; } // bigint -> long

        // Foreign key property for the relationship with Tbl_Batches

        //public int BatchId { get; set; }

        //// Navigation property for the relationship with Tbl_Batches
        //public virtual Tbl_Batches Tbl_Batches { get; set; }
    }
}