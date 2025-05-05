namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_CreateBranch : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_CreateBranch",
                c => new
                    {
                        Branch_ID = c.Int(nullable: false, identity: true),
                        Bank_Id = c.Int(nullable: false),
                        Bank_Name = c.String(),
                        Branch_Name = c.String(),
                    })
                .PrimaryKey(t => t.Branch_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_CreateBranch");
        }
    }
}
