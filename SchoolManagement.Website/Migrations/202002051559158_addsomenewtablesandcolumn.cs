namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsomenewtablesandcolumn : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ExamTypes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ExamType = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.ExamTypes");
        }
    }
}
