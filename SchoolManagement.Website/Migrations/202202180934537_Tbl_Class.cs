namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Class : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Class",
                c => new
                    {
                        Class_Id = c.Int(nullable: false, identity: true),
                        Class_Name = c.String(),
                    })
                .PrimaryKey(t => t.Class_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Class");
        }
    }
}
