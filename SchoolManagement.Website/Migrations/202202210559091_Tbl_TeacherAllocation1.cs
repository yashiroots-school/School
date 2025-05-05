namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TeacherAllocation1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_TeacherAllocation", "StaffId", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_TeacherAllocation", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_TeacherAllocation", "Subject_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_TeacherAllocation", "Subject_ID");
            DropColumn("dbo.Tbl_TeacherAllocation", "Class_Id");
            DropColumn("dbo.Tbl_TeacherAllocation", "StaffId");
        }
    }
}
