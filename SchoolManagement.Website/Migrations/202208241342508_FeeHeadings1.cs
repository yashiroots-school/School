namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeeHeadings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeeHeadings", "Active", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeeHeadings", "Active");
        }
    }
}
