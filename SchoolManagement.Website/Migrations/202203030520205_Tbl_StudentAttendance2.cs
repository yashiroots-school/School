namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentAttendance2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentAttendance", "Created_Date", c => c.String());
            AddColumn("dbo.Tbl_StudentAttendance", "Day", c => c.String());
            AddColumn("dbo.Tbl_StudentAttendance", "Created_By", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentAttendance", "Created_By");
            DropColumn("dbo.Tbl_StudentAttendance", "Day");
            DropColumn("dbo.Tbl_StudentAttendance", "Created_Date");
        }
    }
}
