namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_ExamTypes : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_ExamTypes",
                c => new
                    {
                        Exam_Id = c.Int(nullable: false, identity: true),
                        Exam_Type = c.String(),
                    })
                .PrimaryKey(t => t.Exam_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_ExamTypes");
        }
    }
}
