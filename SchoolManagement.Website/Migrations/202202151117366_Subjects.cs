namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subjects : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "StaffId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "StaffId");
        }
    }
}
