namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SubjectsSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SubjectsSetup",
                c => new
                    {
                        Subject_ID = c.Int(nullable: false, identity: true),
                        Subject_Name = c.String(),
                    })
                .PrimaryKey(t => t.Subject_ID);
            
            AddColumn("dbo.Subjects", "Class_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Class_Id");
            DropTable("dbo.Tbl_SubjectsSetup");
        }
    }
}
