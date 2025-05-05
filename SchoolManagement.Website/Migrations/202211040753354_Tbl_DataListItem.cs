namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_DataListItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_DataListItem", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_DataListItem", "Status");
        }
    }
}
