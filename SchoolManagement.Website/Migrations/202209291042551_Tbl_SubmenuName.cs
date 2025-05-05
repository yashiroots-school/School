namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SubmenuName : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SubmenuName",
                c => new
                    {
                        Submenu_Id = c.Int(nullable: false, identity: true),
                        Submenu_Name = c.String(),              
                        Menu_Id = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Submenu_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_SubmenuName");
        }
    }
}
