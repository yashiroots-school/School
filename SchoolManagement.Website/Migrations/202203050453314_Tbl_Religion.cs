namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Religion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Religion",
                c => new
                    {
                        Religion_Id = c.Int(nullable: false, identity: true),
                        Religion_Name = c.String(),
                    })
                .PrimaryKey(t => t.Religion_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Religion");
        }
    }
}
