using SchoolManagement.Data.Models;
using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.Models
{
    public class StudentRemoteAccess: BaseEntity
    {
        [Key]
        public int Id { get; set; }

        public string EnterDesiredlogin { get; set; }
        public string Password { get; set; }

        ////Foreign key for Standard
        [ForeignKey("Student")]
        public int StudentRefId { get; set; }
        public Student Student { get; set; }
    }
}