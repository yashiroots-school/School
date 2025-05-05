namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addtblFeePlans : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.FeePlans",
            //    c => new
            //        {
            //            FeePlanId = c.Int(nullable: false, identity: true),
            //            FeePlanName = c.String(),
            //            ClassId = c.Int(nullable: false),
            //            ClassName = c.String(),
            //            CategoryId = c.Int(nullable: false),
            //            CategoryName = c.String(),
            //            FeeId = c.Int(nullable: false),
            //            FeeName = c.String(),
            //            FeeValue = c.Single(nullable: false),
            //            Opt1 = c.String(),
            //            Opt2 = c.String(),
            //            Opt3 = c.String(),
            //            Opt4 = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.FeePlanId);
            
            //AddColumn("dbo.Accounts", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.FeeHeadings", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.FeeHeadingGroups", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.Frequencys", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.AdditionalInformations", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.Students", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.FamilyDetails", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.GuardianDetails", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.PastSchoolingReports", "CreateBy", c => c.Int(nullable: false));
            //AddColumn("dbo.StudentRemoteAccesses", "CreateBy", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.StudentRemoteAccesses", "CreateBy");
            //DropColumn("dbo.PastSchoolingReports", "CreateBy");
            //DropColumn("dbo.GuardianDetails", "CreateBy");
            //DropColumn("dbo.FamilyDetails", "CreateBy");
            //DropColumn("dbo.Students", "CreateBy");
            //DropColumn("dbo.AdditionalInformations", "CreateBy");
            //DropColumn("dbo.Frequencys", "CreateBy");
            //DropColumn("dbo.FeeHeadingGroups", "CreateBy");
            //DropColumn("dbo.FeeHeadings", "CreateBy");
            //DropColumn("dbo.Accounts", "CreateBy");
            //DropTable("dbo.FeePlans");
        }
    }
}
