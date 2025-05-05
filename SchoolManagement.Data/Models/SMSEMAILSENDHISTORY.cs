using SchoolManagement.Entities;
using SchoolManagement.Website.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolManagement.Data.Models
{
    public class SMSEMAILSENDHISTORY
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long HISTORYID { get; set; }
        public int SENDERID { get; set; }
        public string SENDERTYPE { get; set; }
        public string SMS { get; set; }
        public string EMAIL { get; set; }
        public string SUBJECT { get; set; }
        public string ATTACHEDFILE { get; set; }
        public string ATTACHEDFILETYPE { get; set; }
        public string ATTACHEDFILENAME { get; set; }
        public DateTime CREATEDDATE { get; set; }
    }
}
