namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTables2 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.tbl_CommonDataListItem",
            //    c => new
            //        {
            //            DatalistId = c.Int(nullable: false, identity: true),
            //            DataListName = c.String(maxLength: 500),
            //            DataListItemName = c.String(maxLength: 500),
            //            Status = c.String(maxLength: 10),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //        })
            //    .PrimaryKey(t => t.DatalistId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.tbl_CommonDataListItem");
        }
    }
}
