using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Data.Models
{
    public class FamilyDetail: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        //Father Details
        [Display(Name ="Father Name :")]
        public string FatherName { get; set; }

        public string FQualifications { get; set; }
        public string FOccupation { get; set; }
        public string FOrganization { get; set; }
      
        public string FPhone { get; set; }
      
        public string FMobile { get; set; }
        //[EmailAddress(ErrorMessage = "Invalid email address")]
        public string FEMail  { get; set; }
        public string FAnnualIncome { get; set; }
       

        public string FResidentialAddress { get; set; }
        public string NoOfBrothers { get; set; }

        //Mother Details
        public string MotherName { get; set; }
        public string MQualifications { get; set; }
        public string MOccupation { get; set; }
        public string MOrganization { get; set; }
        public string MPhone { get; set; }
        public string MMobile { get; set; }
        public string MEMail{ get; set; }
        public string MAnnualIncome{ get; set; }

       
        public string MPermanentAddress{ get; set; }
        public string NoOfSisters { get; set; }
       

        ////Foreign key for Standard
        //[ForeignKey("Student")]
        public int StudentRefId { get; set; }
        //public Student Student { get; set; }

        public string ApplicationNumber { get; set; }

        public string Siblings { get; set; }//For Yes or No
        public string Rvill { get; set; }


        public string Rpost { get; set; }

        public string Rdist { get; set; }

        public string Rstate { get; set; }

        public string Pvill { get; set; }


        public string Ppost { get; set; }

        public string Pdist { get; set; }

        public string Pstate { get; set; }
    }
}