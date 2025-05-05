namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcolumnaddings : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.StafsDetails",
            //    c => new
            //        {
            //            StafId = c.Int(nullable: false, identity: true),
            //            ApplicationNumber = c.String(nullable: false),
            //            UIN = c.String(nullable: false),
            //            Date = c.DateTime(nullable: false),
            //            Name = c.String(nullable: false),
            //            Gender = c.String(),
            //            AgeInWords = c.Int(nullable: false),
            //            DOB = c.DateTime(nullable: false),
            //            POB = c.String(),
            //            Nationality = c.String(),
            //            Religion = c.String(),
            //            Qualification = c.String(),
            //            WorkExperience = c.String(),
            //            MotherTongue = c.String(),
            //            Category = c.String(),
            //            BloodGroup = c.String(),
            //            MedicalHistory = c.String(),
            //            Address = c.String(),
            //            Contact = c.String(),
            //            Email = c.String(),
            //            BesicSallery = c.String(),
            //            PerksSallery = c.String(),
            //            GrossSallery = c.String(),
            //            LastOrganizationofEmployment = c.String(),
            //            NoofYearsattheLastAssignment = c.String(),
            //            RelievingLetter = c.String(),
            //            PerformanceLetter = c.String(),
            //            File = c.String(),
            //            OtherDetails = c.String(),
            //            EmpId = c.String(),
            //            OtherLanguages = c.String(),
            //            EmpDate = c.String(),
            //            FormalitiesCheck = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //        })
            //    .PrimaryKey(t => t.StafId);
            
            //AddColumn("dbo.Tbl_DataListItem", "DataListName", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Tbl_DataListItem", "DataListName");
            //DropTable("dbo.StafsDetails");
        }
    }
}
