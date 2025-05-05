namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveForeignKEyFRomFeeHeading : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.FeeHeadings", "FeeFrequencyId", "dbo.Frequencys");
            //DropForeignKey("dbo.FeeHeadings", "AccountId", "dbo.Accounts");
            //DropForeignKey("dbo.FeeHeadings", "FeeHeadGroupId", "dbo.FeeHeadingGroups");
            //DropIndex("dbo.FeeHeadings", new[] { "FeeFrequencyId" });
            //DropIndex("dbo.FeeHeadings", new[] { "FeeHeadGroupId" });
            //DropIndex("dbo.FeeHeadings", new[] { "AccountId" });
            //RenameColumn(table: "dbo.FeeHeadings", name: "AccountId", newName: "Accounts_AccountId");
            //RenameColumn(table: "dbo.FeeHeadings", name: "FeeHeadGroupId", newName: "FeeHeadingGroups_FeeHeadingGroupId");
            //AlterColumn("dbo.FeeHeadings", "FeeHeadingGroups_FeeHeadingGroupId", c => c.Int());
            //AlterColumn("dbo.FeeHeadings", "Accounts_AccountId", c => c.Int());
            //CreateIndex("dbo.FeeHeadings", "Accounts_AccountId");
            //CreateIndex("dbo.FeeHeadings", "FeeHeadingGroups_FeeHeadingGroupId");
            //AddForeignKey("dbo.FeeHeadings", "Accounts_AccountId", "dbo.Accounts", "AccountId");
            //AddForeignKey("dbo.FeeHeadings", "FeeHeadingGroups_FeeHeadingGroupId", "dbo.FeeHeadingGroups", "FeeHeadingGroupId");
            //DropColumn("dbo.FeeHeadings", "FeeHeadGroupName");
            //DropColumn("dbo.FeeHeadings", "AccountName");
            //DropTable("dbo.Frequencys");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.Frequencys",
            //    c => new
            //        {
            //            FeeFrequencyId = c.Int(nullable: false, identity: true),
            //            FeeFrequencyName = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.FeeFrequencyId);
            
            //AddColumn("dbo.FeeHeadings", "AccountName", c => c.String());
            //AddColumn("dbo.FeeHeadings", "FeeHeadGroupName", c => c.String());
            //DropForeignKey("dbo.FeeHeadings", "FeeHeadingGroups_FeeHeadingGroupId", "dbo.FeeHeadingGroups");
            //DropForeignKey("dbo.FeeHeadings", "Accounts_AccountId", "dbo.Accounts");
            //DropIndex("dbo.FeeHeadings", new[] { "FeeHeadingGroups_FeeHeadingGroupId" });
            //DropIndex("dbo.FeeHeadings", new[] { "Accounts_AccountId" });
            //AlterColumn("dbo.FeeHeadings", "Accounts_AccountId", c => c.Int(nullable: false));
            //AlterColumn("dbo.FeeHeadings", "FeeHeadingGroups_FeeHeadingGroupId", c => c.Int(nullable: false));
            //RenameColumn(table: "dbo.FeeHeadings", name: "FeeHeadingGroups_FeeHeadingGroupId", newName: "FeeHeadGroupId");
            //RenameColumn(table: "dbo.FeeHeadings", name: "Accounts_AccountId", newName: "AccountId");
            //CreateIndex("dbo.FeeHeadings", "AccountId");
            //CreateIndex("dbo.FeeHeadings", "FeeHeadGroupId");
            //CreateIndex("dbo.FeeHeadings", "FeeFrequencyId");
            //AddForeignKey("dbo.FeeHeadings", "FeeHeadGroupId", "dbo.FeeHeadingGroups", "FeeHeadingGroupId", cascadeDelete: true);
            //AddForeignKey("dbo.FeeHeadings", "AccountId", "dbo.Accounts", "AccountId", cascadeDelete: true);
            //AddForeignKey("dbo.FeeHeadings", "FeeFrequencyId", "dbo.Frequencys", "FeeFrequencyId", cascadeDelete: true);
        }
    }
}
