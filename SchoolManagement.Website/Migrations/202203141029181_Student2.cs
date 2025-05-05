namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "ParentEmail", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "ParentEmail");
        }
    }
}
