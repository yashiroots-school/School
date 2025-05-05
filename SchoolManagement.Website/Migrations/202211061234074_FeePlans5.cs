namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "Batch_Id", c => c.Int(nullable: false));
            AddColumn("dbo.FeePlans", "Batch_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "Batch_Name");
            DropColumn("dbo.FeePlans", "Batch_Id");
        }
    }
}
