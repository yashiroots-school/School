namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_MenuName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_MenuName",
                c => new
                    {
                        Menu_Id = c.Int(nullable: false, identity: true),
                        Menu_Name = c.String(),
                        AddedDate = c.String(),
                        ModifiedDate = c.String(),
                        CurrentYear = c.Int(nullable: false),
                        IP = c.String(),
                        UserId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        InsertBy = c.String(),
                        BatchName = c.String(),
                    })
                .PrimaryKey(t => t.Menu_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_MenuName");
        }
    }
}
