namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TeacherAllocation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_TeacherAllocation",
                c => new
                    {
                        Allocate_Id = c.Int(nullable: false, identity: true),
                        Teacher_Name = c.String(),
                        Class_Name = c.String(),
                        Subject_Name = c.String(),
                    })
                .PrimaryKey(t => t.Allocate_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_TeacherAllocation");
        }
    }
}
