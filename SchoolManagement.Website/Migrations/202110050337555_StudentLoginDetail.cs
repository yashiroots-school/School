namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentLoginDetail : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.StudentLoginDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            UserName = c.String(nullable: false),
            //            UserPassword = c.String(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.Int(nullable: false),
            //            ModifiedOn = c.DateTime(nullable: false),
            //            ModifiedBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //DropTable("dbo.StudentLoginDetails");
        }
        
        public override void Down()
        {
            //CreateTable(
            //    "dbo.StudentLoginDetails",
            //    c => new
            //        {
            //            StudentId = c.Int(nullable: false, identity: true),
            //            UserName = c.String(nullable: false),
            //            UserPassword = c.String(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.Int(nullable: false),
            //            ModifiedOn = c.DateTime(nullable: false),
            //            ModifiedBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.StudentId);
            
            //DropTable("dbo.StudentLoginDetails");
        }
    }
}
