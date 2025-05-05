namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TimeTable4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_TimeTable", "Day_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_TimeTable", "Time_ID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_TimeTable", "Time_ID");
            DropColumn("dbo.Tbl_TimeTable", "Day_ID");
        }
    }
}
