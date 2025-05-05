namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SetTime : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SetTime",
                c => new
                    {
                        Time_Id = c.Int(nullable: false, identity: true),
                        Time = c.String(),
                    })
                .PrimaryKey(t => t.Time_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_SetTime");
        }
    }
}
