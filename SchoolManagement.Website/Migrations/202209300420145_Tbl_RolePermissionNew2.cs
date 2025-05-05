namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_RolePermissionNew2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_RolePermissionNew", "Submenu_permission", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tbl_RolePermissionNew", "Staff_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_RolePermissionNew", "Staff_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_RolePermissionNew", "Staff_Name");
            DropColumn("dbo.Tbl_RolePermissionNew", "Staff_Id");
            DropColumn("dbo.Tbl_RolePermissionNew", "Submenu_permission");
        }
    }
}
