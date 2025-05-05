namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffAttendance : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_StaffAttendance",
                c => new
                    {
                        StaffAtte_Id = c.Int(nullable: false, identity: true),
                        Staff_Id = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        Mark_FullDayAbsent = c.String(),
                        Mark_HalfDayAbsent = c.String(),
                        AddedDate = c.String(),
                        ModifiedDate = c.String(),
                        CurrentYear = c.Int(nullable: false),
                        IP = c.String(),
                        UserId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        InsertBy = c.String(),
                        BatchName = c.String(),
                    })
                .PrimaryKey(t => t.StaffAtte_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_StaffAttendance");
        }
    }
}
