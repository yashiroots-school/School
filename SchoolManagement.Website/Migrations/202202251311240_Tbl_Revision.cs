namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Revision : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Revision",
                c => new
                    {
                        Revision_Id = c.Int(nullable: false, identity: true),
                        Class_Name = c.String(),
                        Class_Id = c.Int(nullable: false),
                        Section_Name = c.String(),
                        Section_Id = c.Int(nullable: false),
                        Subject_Name = c.String(),
                        Subject_ID = c.Int(nullable: false),
                        Revision_Date = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Revision_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Revision");
        }
    }
}
