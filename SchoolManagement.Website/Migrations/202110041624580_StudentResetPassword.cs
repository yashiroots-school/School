namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentResetPassword : DbMigration
    {
        public override void Up()
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
            
            //CreateTable(
            //    "dbo.StudentLoginHistories",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            UserPassword = c.String(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.StudentResetPasswords",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            ResetKey = c.String(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            CreatedBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.StudentResetPasswords");
            //DropTable("dbo.StudentLoginHistories");
            //DropTable("dbo.StudentLoginDetails");
        }
    }
}
