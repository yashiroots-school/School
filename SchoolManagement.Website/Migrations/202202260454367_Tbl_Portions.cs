namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Portions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Portions",
                c => new
                    {
                        Portion_Id = c.Int(nullable: false, identity: true),
                        Class_Name = c.String(),
                        Class_Id = c.Int(nullable: false),
                        Section_Name = c.String(),
                        Section_Id = c.Int(nullable: false),
                        Subject_Name = c.String(),
                        Subject_ID = c.Int(nullable: false),
                        Portion_Date = c.String(),
                        Description = c.String(),
                        CreatedDate = c.String(),
                    })
                .PrimaryKey(t => t.Portion_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Portions");
        }
    }
}
