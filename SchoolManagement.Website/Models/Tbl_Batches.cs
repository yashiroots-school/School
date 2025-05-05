using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using SchoolManagement.Entities;

namespace SchoolManagement.Website.Models
{
    public class Tbl_Batches
    {
        [Key]
        public int Batch_Id { get; set; }

        public string Batch_Name { get; set; }

        public bool IsActiveForAdmission { get; set; }

        public bool IsActiveForPayments { get; set; }

        public bool IsActiveForRegistrationFee { get; set; }
    }
    public class Tbl_TimeTableMaster
    {
        [Key]
        public int TimeTableId { get; set; }

        public string TimeTableName { get; set; }
    }
}