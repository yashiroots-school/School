namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addclassesas : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Classes",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            ClassName = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Classes");
        }
    }
}
