namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewclassStudentCategorys : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.StudentCategorys",
            //    c => new
            //        {
            //            CategoryId = c.Int(nullable: false, identity: true),
            //            CategoryName = c.String(),
            //        })
            //    .PrimaryKey(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.StudentCategorys");
        }
    }
}
