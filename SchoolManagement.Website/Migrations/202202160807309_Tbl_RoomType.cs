namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_RoomType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_RoomType",
                c => new
                    {
                        Room_Id = c.Int(nullable: false, identity: true),
                        Room_Type = c.String(),
                    })
                .PrimaryKey(t => t.Room_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_RoomType");
        }
    }
}
