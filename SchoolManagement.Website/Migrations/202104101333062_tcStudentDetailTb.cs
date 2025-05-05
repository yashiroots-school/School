namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcStudentDetailTb : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.StudentTcDetails",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            Ispaid = c.Boolean(nullable: false),
            //            TcId = c.Long(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
            //    .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.StudentTcDetails", "StudentId", "dbo.Students");
            //DropIndex("dbo.StudentTcDetails", new[] { "StudentId" });
            //DropTable("dbo.StudentTcDetails");
        }
    }
}
