namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TimeTable3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_TimeTable", "Date", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_TimeTable", "Date");
        }
    }
}
