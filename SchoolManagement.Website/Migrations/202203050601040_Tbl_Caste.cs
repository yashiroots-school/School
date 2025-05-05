namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Caste : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Caste",
                c => new
                    {
                        Caste_Id = c.Int(nullable: false, identity: true),
                        Caste_Name = c.String(),
                    })
                .PrimaryKey(t => t.Caste_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Caste");
        }
    }
}
