namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Editrolepermissionpage3 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.RolePagePermissions", "PageViewName", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.RolePagePermissions", "PageViewName", c => c.Boolean(nullable: false));
        }
    }
}
