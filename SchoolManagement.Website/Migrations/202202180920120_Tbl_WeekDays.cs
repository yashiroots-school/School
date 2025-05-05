namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_WeekDays : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_WeekDays",
                c => new
                    {
                        Day_Id = c.Int(nullable: false, identity: true),
                        Week_day = c.String(),
                    })
                .PrimaryKey(t => t.Day_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_WeekDays");
        }
    }
}
