namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Subjects1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Subjects", "StaffId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Subjects", "StaffId", c => c.String());
        }
    }
}
