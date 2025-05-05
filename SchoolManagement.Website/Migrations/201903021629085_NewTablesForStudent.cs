namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTablesForStudent : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.AdditionalInformations",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            AssignClass = c.String(),
            //            AssignSection = c.String(),
            //            Remarks = c.String(),
            //            grade = c.String(),
            //            FeeStructureApplicable = c.String(),
            //            DistancefromSchool = c.Single(nullable: false),
            //            TransportFacility = c.String(),
            //            BirthCertificateAvatar = c.String(),
            //            ThreePassportSizePhotographs = c.String(),
            //            ProgressReport = c.String(),
            //            MigrationCertificate = c.String(),
            //            StudentRefId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentRefId, cascadeDelete: true)
            //    .Index(t => t.StudentRefId);
            
            //CreateTable(
            //    "dbo.Students",
            //    c => new
            //        {
            //            StudentId = c.Int(nullable: false, identity: true),
            //            ApplicationNumber = c.String(nullable: false),
            //            UIN = c.String(nullable: false),
            //            Date = c.DateTime(nullable: false),
            //            Name = c.String(),
            //            Class = c.String(),
            //            Section = c.String(),
            //            Gender = c.String(),
            //            AgeInWords = c.Int(nullable: false),
            //            DOB = c.DateTime(nullable: false),
            //            POB = c.DateTime(nullable: false),
            //            Nationality = c.String(),
            //            Religion = c.String(),
            //            MotherTongue = c.String(),
            //            Category = c.String(),
            //            BloodGroup = c.Int(nullable: false),
            //            MedicalHistory = c.String(),
            //            Hobbies = c.String(),
            //            Sports = c.String(),
            //            OtherDetails = c.String(),
            //            ProfileAvatar = c.String(),
            //        })
            //    .PrimaryKey(t => t.StudentId);
            
            //CreateTable(
            //    "dbo.FamilyDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            FatherName = c.String(),
            //            FQualifications = c.String(),
            //            FOccupation = c.String(),
            //            FOrganization = c.String(),
            //            FPhone = c.String(),
            //            FMobile = c.String(),
            //            FEMail = c.String(),
            //            FAnnualIncome = c.String(),
            //            FResidentialAddress = c.String(),
            //            MotherName = c.String(),
            //            MQualifications = c.String(),
            //            MOccupation = c.String(),
            //            MOrganization = c.String(),
            //            MPhone = c.String(),
            //            MMobile = c.String(),
            //            MEMail = c.String(),
            //            MAnnualIncome = c.String(),
            //            MPermanentAddress = c.String(),
            //            StudentRefId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentRefId, cascadeDelete: true)
            //    .Index(t => t.StudentRefId);
            
            //CreateTable(
            //    "dbo.GuardianDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            GuardianName = c.String(),
            //            Qualifications = c.String(),
            //            Occupation = c.String(),
            //            Organization = c.String(),
            //            Phone = c.String(),
            //            Mobile = c.String(),
            //            EMail = c.String(),
            //            AnnualIncome = c.String(),
            //            ResidentialAddress = c.String(),
            //            PermanentAddress = c.String(),
            //            StudentRefId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentRefId, cascadeDelete: true)
            //    .Index(t => t.StudentRefId);
            
            //CreateTable(
            //    "dbo.PastSchoolingReports",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            NameOfSchoolLastAttended = c.String(),
            //            ClassPassed = c.String(),
            //            ReasonForLeaving = c.String(),
            //            TCAvatar = c.String(),
            //            MarksCardAvatar = c.String(),
            //            CharacterConductCertificateAvatar = c.String(),
            //            StudentRefId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentRefId, cascadeDelete: true)
            //    .Index(t => t.StudentRefId);
            
            //CreateTable(
            //    "dbo.StudentRemoteAccesses",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            EnterDesiredlogin = c.String(),
            //            Password = c.String(),
            //            StudentRefId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentRefId, cascadeDelete: true)
            //    .Index(t => t.StudentRefId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.AdditionalInformations", "StudentRefId", "dbo.Students");
            //DropForeignKey("dbo.StudentRemoteAccesses", "StudentRefId", "dbo.Students");
            //DropForeignKey("dbo.PastSchoolingReports", "StudentRefId", "dbo.Students");
            //DropForeignKey("dbo.GuardianDetails", "StudentRefId", "dbo.Students");
            //DropForeignKey("dbo.FamilyDetails", "StudentRefId", "dbo.Students");
            //DropIndex("dbo.StudentRemoteAccesses", new[] { "StudentRefId" });
            //DropIndex("dbo.PastSchoolingReports", new[] { "StudentRefId" });
            //DropIndex("dbo.GuardianDetails", new[] { "StudentRefId" });
            //DropIndex("dbo.FamilyDetails", new[] { "StudentRefId" });
            //DropIndex("dbo.AdditionalInformations", new[] { "StudentRefId" });
            //DropTable("dbo.StudentRemoteAccesses");
            //DropTable("dbo.PastSchoolingReports");
            //DropTable("dbo.GuardianDetails");
            //DropTable("dbo.FamilyDetails");
            //DropTable("dbo.Students");
            //DropTable("dbo.AdditionalInformations");
        }
    }
}
