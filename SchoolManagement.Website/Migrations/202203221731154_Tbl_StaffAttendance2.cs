namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffAttendance2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StaffAttendance", "Attendence_Day", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Attendence_Month", c => c.String());
            AddColumn("dbo.Tbl_StaffAttendance", "Attendence_Year", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StaffAttendance", "Attendence_Year");
            DropColumn("dbo.Tbl_StaffAttendance", "Attendence_Month");
            DropColumn("dbo.Tbl_StaffAttendance", "Attendence_Day");
        }
    }
}
