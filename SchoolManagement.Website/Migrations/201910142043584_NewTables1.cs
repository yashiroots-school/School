namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables1 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.tbl_AcademicDetail",
            //    c => new
            //        {
            //            AcademicDetailId = c.Long(nullable: false, identity: true),
            //            NewProperty = c.Int(nullable: false),
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            AcademicYear = c.String(maxLength: 20),
            //            Qualification = c.String(maxLength: 20),
            //            Institution = c.String(maxLength: 1024),
            //            University = c.String(maxLength: 1024),
            //            Percentage = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //            Dateon = c.String(maxLength: 35),
            //            Stream = c.String(maxLength: 256),
            //        })
            //    .PrimaryKey(t => t.AcademicDetailId);
            
            //CreateTable(
            //    "dbo.tbl_Declaration",
            //    c => new
            //        {
            //            DeclarationId = c.Long(nullable: false, identity: true),
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            Interesterd = c.String(maxLength: 10),
            //            NotInterested = c.String(maxLength: 512),
            //            Relocate = c.String(maxLength: 10),
            //            StudentName = c.String(maxLength: 256),
            //            Agree = c.String(maxLength: 10),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //        })
            //    .PrimaryKey(t => t.DeclarationId)
            //    .ForeignKey("dbo.tbl_StudentDetail", t => t.ScholarNumber, cascadeDelete: true)
            //    .Index(t => t.ScholarNumber);
            
            //CreateTable(
            //    "dbo.tbl_StudentDetail",
            //    c => new
            //        {
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            StudentId = c.Long(nullable: false, identity: true),
            //            StudentName = c.String(maxLength: 256),
            //            Course = c.String(maxLength: 256),
            //            Years = c.String(maxLength: 20),
            //            Batch = c.String(maxLength: 20),
            //            Specialization = c.String(maxLength: 256),
            //            Sibiling1 = c.String(maxLength: 256),
            //            Sibiling2 = c.String(maxLength: 256),
            //            Sibiling3 = c.String(maxLength: 256),
            //            Sibiling4 = c.String(maxLength: 256),
            //            Sibiling5 = c.String(maxLength: 256),
            //            Category = c.String(maxLength: 256),
            //            FacultyMentor = c.String(maxLength: 256),
            //            DateofBirth = c.String(maxLength: 256),
            //            Age = c.String(maxLength: 5),
            //            Gender = c.String(maxLength: 256),
            //            CorrespondenceAddress = c.String(maxLength: 512),
            //            ResidenceLocation = c.String(maxLength: 256),
            //            CountryCode = c.String(maxLength: 10),
            //            MobileNo = c.String(maxLength: 20),
            //            EmailId = c.String(maxLength: 256),
            //            OutStationStudent = c.String(maxLength: 256),
            //            NativePlace = c.String(maxLength: 256),
            //            Hostalite = c.String(maxLength: 256),
            //            FatherName = c.String(maxLength: 256),
            //            FatherProfession = c.String(maxLength: 256),
            //            FatherCountryCode = c.String(maxLength: 10),
            //            FatherMobileNo = c.String(maxLength: 256),
            //            FatherEmailId = c.String(maxLength: 256),
            //            FatherCompanyName = c.String(maxLength: 256),
            //            MotherName = c.String(maxLength: 256),
            //            MotherProfession = c.String(maxLength: 256),
            //            MotherCountryCode = c.String(maxLength: 10),
            //            MotherMobileNo = c.String(maxLength: 256),
            //            MotherEmailId = c.String(maxLength: 256),
            //            MotherCompanyName = c.String(maxLength: 256),
            //            status = c.String(maxLength: 20),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //            CMCRemarks = c.String(maxLength: 256),
            //            DateOn = c.String(maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.ScholarNumber);
            
            //CreateTable(
            //    "dbo.tbl_SummerInternship",
            //    c => new
            //        {
            //            SummerInternshipId = c.Long(nullable: false, identity: true),
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            CompanyName = c.String(maxLength: 512),
            //            StartDate = c.String(maxLength: 20),
            //            MobileNo = c.String(maxLength: 20),
            //            EndDate = c.String(maxLength: 20),
            //            ProjectTitle = c.String(maxLength: 256),
            //            FacultyProjectGuide = c.String(maxLength: 256),
            //            FacultyGuideMobileNo = c.String(maxLength: 20),
            //            IndustryGuideName = c.String(maxLength: 256),
            //            IndustryGuideDesignation = c.String(maxLength: 256),
            //            IndustryGuideTelNo = c.String(maxLength: 20),
            //            IndustryGuideMobileNo = c.String(maxLength: 20),
            //            IndustryGuideEmail = c.String(maxLength: 512),
            //            StipendinThousands = c.String(maxLength: 20),
            //            ProjectDescription = c.String(maxLength: 512),
            //            ProjectSubmission = c.String(maxLength: 10),
            //            ReasonforNoSubmission = c.String(maxLength: 512),
            //            PrePlacementOfferReceived = c.String(maxLength: 10),
            //            Feedback = c.String(maxLength: 512),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //        })
            //    .PrimaryKey(t => t.SummerInternshipId)
            //    .ForeignKey("dbo.tbl_StudentDetail", t => t.ScholarNumber, cascadeDelete: true)
            //    .Index(t => t.ScholarNumber);
            
            //CreateTable(
            //    "dbo.tbl_WorkExperience",
            //    c => new
            //        {
            //            WorkExperienceId = c.Long(nullable: false, identity: true),
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            TotalExperience = c.String(maxLength: 5),
            //            CompanyName = c.String(maxLength: 512),
            //            CompanyProfile = c.String(maxLength: 512),
            //            Designation = c.String(maxLength: 256),
            //            FromDate = c.Int(nullable: false),
            //            ToDate = c.String(maxLength: 20),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //        })
            //    .PrimaryKey(t => t.WorkExperienceId)
            //    .ForeignKey("dbo.tbl_StudentDetail", t => t.ScholarNumber, cascadeDelete: true)
            //    .Index(t => t.ScholarNumber);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.tbl_WorkExperience", "ScholarNumber", "dbo.tbl_StudentDetail");
            //DropForeignKey("dbo.tbl_SummerInternship", "ScholarNumber", "dbo.tbl_StudentDetail");
            //DropForeignKey("dbo.tbl_Declaration", "ScholarNumber", "dbo.tbl_StudentDetail");
            //DropIndex("dbo.tbl_WorkExperience", new[] { "ScholarNumber" });
            //DropIndex("dbo.tbl_SummerInternship", new[] { "ScholarNumber" });
            //DropIndex("dbo.tbl_Declaration", new[] { "ScholarNumber" });
            //DropTable("dbo.tbl_WorkExperience");
            //DropTable("dbo.tbl_SummerInternship");
            //DropTable("dbo.tbl_StudentDetail");
            //DropTable("dbo.tbl_Declaration");
            //DropTable("dbo.tbl_AcademicDetail");
        }
    }
}
