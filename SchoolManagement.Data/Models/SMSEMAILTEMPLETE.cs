using SchoolManagement.Entities;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class SMSEMAILTEMPLETE
    {
        
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SMSEMAILID { get; set; }
        public string NOTIFICATIONTYPE { get; set; }
        public string SMS { get; set; }
        public string SMSSubject { get; set; }
        public string EMAIL { get; set; }
        public string SUBJECT { get; set; }
        public string ATTACHEDFILE { get; set; }
        public string ATTACHEDFILETYPE { get; set; }
        public string ATTACHEDFILENAME { get; set; }
        public string CREATEDDATE { get; set; }
    }
}
