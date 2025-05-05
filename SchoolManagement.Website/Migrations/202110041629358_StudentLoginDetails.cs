namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentLoginDetails : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.StudentLoginDetails", "StudentId", c=>c.Int());
        }
        
        public override void Down()
        {
           // DropColumn("dbo.StudentLoginDetails", "StudentId");
            DropPrimaryKey("dbo.StudentLoginDetails", "StudentId");
        }
    }
}
