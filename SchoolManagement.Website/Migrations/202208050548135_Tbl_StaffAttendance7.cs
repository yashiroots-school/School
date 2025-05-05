namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffAttendance7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StaffAttendance", "Employee_Code", c => c.String());
            AlterColumn("dbo.Tbl_StaffAttendance", "Attendence_Day", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_StaffAttendance", "Attendence_Day", c => c.String());
            DropColumn("dbo.Tbl_StaffAttendance", "Employee_Code");
        }
    }
}
