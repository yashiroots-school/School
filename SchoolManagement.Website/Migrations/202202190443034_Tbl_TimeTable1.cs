namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TimeTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_TimeTable", "Day_Time_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_TimeTable", "CreatedDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_TimeTable", "CreatedDate");
            DropColumn("dbo.Tbl_TimeTable", "Day_Time_Id");
        }
    }
}
