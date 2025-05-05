namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newTables : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Accounts",
            //    c => new
            //        {
            //            AccountId = c.Int(nullable: false, identity: true),
            //            AccountName = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.AccountId);
            
            //CreateTable(
            //    "dbo.FeeHeadings",
            //    c => new
            //        {
            //            FeeId = c.Int(nullable: false, identity: true),
            //            FeeName = c.String(),
            //            FeeFrequencyId = c.Int(nullable: false),
            //            FeeFrequencyName = c.String(),
            //            Jan = c.String(),
            //            Far = c.String(),
            //            Mar = c.String(),
            //            Apr = c.String(),
            //            May = c.String(),
            //            Jun = c.String(),
            //            Jul = c.String(),
            //            Aug = c.String(),
            //            Sep = c.String(),
            //            Oct = c.String(),
            //            Nov = c.String(),
            //            Dec = c.String(),
            //            FeeHeadGroupId = c.Int(nullable: false),
            //            FeeHeadGroupName = c.String(),
            //            AccountId = c.Int(nullable: false),
            //            AccountName = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.FeeId)
            //    .ForeignKey("dbo.Accounts", t => t.AccountId, cascadeDelete: true)
            //    .ForeignKey("dbo.FeeHeadingGroups", t => t.FeeHeadGroupId, cascadeDelete: true)
            //    .Index(t => t.FeeHeadGroupId)
            //    .Index(t => t.AccountId);
            
            //CreateTable(
            //    "dbo.FeeHeadingGroups",
            //    c => new
            //        {
            //            FeeHeadingGroupId = c.Int(nullable: false, identity: true),
            //            FeeHeadingGroupName = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.FeeHeadingGroupId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.FeeHeadings", "FeeHeadGroupId", "dbo.FeeHeadingGroups");
            //DropForeignKey("dbo.FeeHeadings", "AccountId", "dbo.Accounts");
            //DropIndex("dbo.FeeHeadings", new[] { "AccountId" });
            //DropIndex("dbo.FeeHeadings", new[] { "FeeHeadGroupId" });
            //DropTable("dbo.FeeHeadingGroups");
            //DropTable("dbo.FeeHeadings");
            //DropTable("dbo.Accounts");
        }
    }
}
