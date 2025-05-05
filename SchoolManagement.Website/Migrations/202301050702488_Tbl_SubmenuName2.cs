namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SubmenuName2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_SubmenuName", "Submenu_permission", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_SubmenuName", "Submenu_permission");
        }
    }
}
