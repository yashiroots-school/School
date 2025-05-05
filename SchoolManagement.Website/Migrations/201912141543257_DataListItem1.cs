namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataListItem1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Tbl_DataListItem", "DataListId", c => c.String());
            //DropColumn("dbo.Tbl_DataListItem", "DataListName");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.Tbl_DataListItem", "DataListName", c => c.String());
            //DropColumn("dbo.Tbl_DataListItem", "DataListId");
        }
    }
}
