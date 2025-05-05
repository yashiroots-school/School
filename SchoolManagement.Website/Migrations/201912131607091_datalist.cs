namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class datalist : DbMigration
    {
        public override void Up()
        {
            //RenameTable(name: "dbo.DataLists", newName: "Tbl_DataList");
        }
        
        public override void Down()
        {
            //RenameTable(name: "dbo.Tbl_DataList", newName: "DataLists");
        }
    }
}
