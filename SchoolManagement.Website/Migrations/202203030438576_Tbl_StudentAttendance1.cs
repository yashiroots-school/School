namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentAttendance1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentAttendance", "StudentRegisterID", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_StudentAttendance", "Student_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentAttendance", "Student_Name");
            DropColumn("dbo.Tbl_StudentAttendance", "StudentRegisterID");
        }
    }
}
