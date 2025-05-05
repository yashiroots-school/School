namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Category : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Category",
                c => new
                    {
                        Category_Id = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(),
                    })
                .PrimaryKey(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Category");
        }
    }
}
