namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Room1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_Room", "RoomType_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_Room", "RoomType_ID");
        }
    }
}
