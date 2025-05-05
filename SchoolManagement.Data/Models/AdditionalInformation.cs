using SchoolManagement.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SchoolManagement.Data.Models
{
    public class AdditionalInformation: BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string AssignClass { get; set; }
        public string Group { get; set; }
        public string AssignSection { get; set; }
        public string Remarks { get; set; }
        public string grade { get; set; }
        public string FeeStructureApplicable { get; set; }
        public float DistancefromSchool { get; set; }
        public string TransportFacility { get; set; }
        public string TransportOptions { get; set; }
        public string BirthCertificateAvatar { get; set; }
        public string ThreePassportSizePhotographs { get; set; }
        public string ProgressReport { get; set; }
        public string MigrationCertificate { get; set; }
        public bool Physicallychalanged { get; set; }
        public string IncomeCertificate { get; set; }
        public string CastCertificate { get; set; }
        public string FatherAdhar { get; set; }
        public string MotherAdhar { get; set; }
        public string BankBook { get; set; }
        public string Ssmid { get; set; }
        ////Foreign key for Standard
        //[ForeignKey("Student")]
        public int StudentRefId { get; set; }
        public Student Student { get; set; }

        public int Class_Id { get; set; }

        public string Class_Name { get; set; }

        public int Section_Id { get; set; }

        public string Section_Name { get; set; }
        public string ApplicationNumber { get; set; }
    }
}

 