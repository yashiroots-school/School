using SchoolManagement.Data.Models;
using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Data.Models
{
    public class PastSchoolingReport: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string NameOfSchoolLastAttended { get; set; }
        public string ClassPassed { get; set; }
        public string ReasonForLeaving { get; set; }
        public string TCAvatar { get; set; }
        public string MarksCardAvatar { get; set; }
        public string CharacterConductCertificateAvatar { get; set; }
        public string Promotion { get; set; }

        ////Foreign key for Standard
        //[ForeignKey("Student")]
        public int StudentRefId { get; set; }
        public string ApplicationNumber { get; set; }
        public Student Student { get; set; }
    }
}