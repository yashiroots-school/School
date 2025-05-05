namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Editrolepermissionpage : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.RolePagePermissions", "PageViewName", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.RolePagePermissions", "PageViewName");
        }
    }
}
