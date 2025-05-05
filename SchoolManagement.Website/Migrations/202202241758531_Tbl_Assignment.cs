namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Assignment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Assignment",
                c => new
                    {
                        Assignment_Id = c.Int(nullable: false, identity: true),
                        Class_Name = c.String(),
                        Class_Id = c.Int(nullable: false),
                        Section_Name = c.String(),
                        Section_Id = c.Int(nullable: false),
                        Subject_Name = c.String(),
                        Subject_ID = c.Int(nullable: false),
                        New_Assignment = c.String(),
                        Assignment_Date = c.String(),
                        Submitted_Date = c.String(),
                        CreatedDate = c.String(),
                    })
                .PrimaryKey(t => t.Assignment_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Assignment");
        }
    }
}
