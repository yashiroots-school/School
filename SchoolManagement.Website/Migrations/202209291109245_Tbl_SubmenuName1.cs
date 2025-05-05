namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SubmenuName1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_SubmenuName", "Submenu_Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_SubmenuName", "Submenu_Url");
        }
    }
}
