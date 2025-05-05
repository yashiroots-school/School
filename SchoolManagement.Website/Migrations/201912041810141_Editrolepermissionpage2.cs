namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Editrolepermissionpage2 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.RolePagePermissions", "ParentId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.RolePagePermissions", "ParentId");
        }
    }
}
