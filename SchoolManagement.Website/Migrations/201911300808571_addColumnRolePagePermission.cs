namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addColumnRolePagePermission : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.RolePagePermissions", "RoleName", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.RolePagePermissions", "RoleName");
        }
    }
}
