namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeeHeadings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeeHeadings", "FeeType_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeeHeadings", "FeeType_Id");
        }
    }
}
