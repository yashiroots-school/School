using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SchoolManagement.Website.ViewModels
{
    public class UploadFilesViewModel
    {
        public HttpPostedFileBase AdharFile { get; set; }
        public HttpPostedFileBase BirthCertificateAvatar { get; set; }
        public HttpPostedFileBase ThreePassportSizePhotographs { get; set; }
        public HttpPostedFileBase ProgressReport { get; set; }
        public HttpPostedFileBase MigrationCertificate { get; set; }
        public HttpPostedFileBase TCAvatar { get; set; }
        public HttpPostedFileBase MarksCardAvatar { get; set; }
        public HttpPostedFileBase CharacterConductCertificateAvatar { get; set; }
        public HttpPostedFileBase ProfileAvatar { get; set; }
        public HttpPostedFileBase IncomeCertificate { get; set; }
        public HttpPostedFileBase CastCertificate { get; set; }
        public HttpPostedFileBase FatherAdhar { get; set; }
        public HttpPostedFileBase MotherAdhar { get; set; }
        public HttpPostedFileBase BankBook { get; set; }
        public HttpPostedFileBase Ssmid { get; set; }
 
    }
}