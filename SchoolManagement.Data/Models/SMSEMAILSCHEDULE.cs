using SchoolManagement.Entities;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class SMSEMAILSCHEDULE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SMSEMAILSCHEDULEID { get; set; }       
        public string SCHEDULETYPE { get; set; }       
        public DateTime CREATEDDATE { get; set; }
    }
}
