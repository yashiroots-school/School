namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentRegistrationHistory : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentRegistrationHistories", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentRegistrationHistories", "Category_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentRegistrationHistories", "Category_Id");
            DropColumn("dbo.StudentRegistrationHistories", "Class_Id");
        }
    }
}
