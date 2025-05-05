namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffAttendance5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StaffAttendance", "DayFourteen", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StaffAttendance", "DayFourteen");
        }
    }
}
