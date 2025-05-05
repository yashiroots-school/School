namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentLoginDetails : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.StudentLoginDetails");
            AddColumn("dbo.StudentLoginDetails", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.StudentLoginDetails", "StudentId", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.StudentLoginDetails", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StudentLoginDetails");
            AlterColumn("dbo.StudentLoginDetails", "StudentId", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.StudentLoginDetails", "Id");
            AddPrimaryKey("dbo.StudentLoginDetails", "StudentId");
        }
    }
}
