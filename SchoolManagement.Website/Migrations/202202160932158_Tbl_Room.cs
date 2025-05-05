namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Room : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Room",
                c => new
                    {
                        Room_Id = c.Int(nullable: false, identity: true),
                        Room_Name = c.String(),
                        Room_No = c.String(),
                        Room_Type = c.String(),
                        Seating_Capacity = c.String(),
                        Location = c.String(),
                        Remarks = c.String(),
                    })
                .PrimaryKey(t => t.Room_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Room");
        }
    }
}
