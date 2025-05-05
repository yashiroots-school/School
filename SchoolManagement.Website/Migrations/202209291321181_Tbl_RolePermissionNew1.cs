namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_RolePermissionNew1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_RolePermissionNew", "Submenu_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_RolePermissionNew", "Submenu_Name");
        }
    }
}
