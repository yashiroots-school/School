namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_RolePermissionNew : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_RolePermissionNew",
                c => new
                    {
                        Rolepermission_Id = c.Int(nullable: false, identity: true),
                        Role_Id = c.String(),
                        Menu_Id = c.Int(nullable: false),
                        Submenu_Id = c.Int(nullable: false),
                        Submenu_Url = c.String(),
                        Create_permission = c.Boolean(nullable: false),
                        Edit_Permission = c.Boolean(nullable: false),
                        Update_Permission = c.Boolean(nullable: false),
                        Delete_Permission = c.Boolean(nullable: false),
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
                .PrimaryKey(t => t.Rolepermission_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_RolePermissionNew");
        }
    }
}
