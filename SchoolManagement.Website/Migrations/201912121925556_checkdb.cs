namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class checkdb : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.DataListItems",
            //    c => new
            //        {
            //            DataListItemId = c.Int(nullable: false, identity: true),
            //            DataListItemName = c.String(),
            //            DataListName = c.String(),
            //        })
            //    .PrimaryKey(t => t.DataListItemId);
            
            //CreateTable(
            //    "dbo.DataLists",
            //    c => new
            //        {
            //            DataListId = c.Int(nullable: false, identity: true),
            //            DataListName = c.String(),
            //        })
            //    .PrimaryKey(t => t.DataListId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.DataLists");
            //DropTable("dbo.DataListItems");
        }
    }
}
