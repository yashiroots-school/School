namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "BloodGroup_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "BloodGroup_Id");
        }
    }
}
