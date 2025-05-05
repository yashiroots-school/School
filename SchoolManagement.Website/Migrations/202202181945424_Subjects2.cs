namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subjects2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Subject_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Subject_ID");
        }
    }
}
