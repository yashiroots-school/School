namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SectionSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SectionSetup",
                c => new
                    {
                        Section_Id = c.Int(nullable: false, identity: true),
                        Section = c.String(),
                        Class = c.String(),
                        Class_Id = c.String(),
                    })
                .PrimaryKey(t => t.Section_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_SectionSetup");
        }
    }
}
