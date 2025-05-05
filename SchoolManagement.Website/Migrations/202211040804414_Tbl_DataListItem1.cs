namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_DataListItem1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Tbl_DataListItem", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Tbl_DataListItem", "Status", c => c.String());
        }
    }
}
