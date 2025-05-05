namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentAttendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_StudentAttendance",
                c => new
                    {
                        Attendance_Id = c.Int(nullable: false, identity: true),
                        Class_Id = c.Int(nullable: false),
                        Section_Id = c.Int(nullable: false),
                        Class_Name = c.String(),
                        Section_Name = c.String(),
                        Mark_FullDayAbsent = c.String(),
                        Mark_HalfDayAbsent = c.String(),
                    })
                .PrimaryKey(t => t.Attendance_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_StudentAttendance");
        }
    }
}
