namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "FeeType_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "FeeType_Id");
        }
    }
}
