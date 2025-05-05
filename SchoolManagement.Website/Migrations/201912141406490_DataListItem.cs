namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataListItem : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.DataListItems", newName: "Tbl_DataListItem");
        }
        
        public override void Down()
        {
            //RenameTable(name: "dbo.Tbl_DataListItem", newName: "DataListItems");
        }
    }
}
