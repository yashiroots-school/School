namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Students", "Category_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Category_Id");
            DropColumn("dbo.Students", "Class_Id");
        }
    }
}
