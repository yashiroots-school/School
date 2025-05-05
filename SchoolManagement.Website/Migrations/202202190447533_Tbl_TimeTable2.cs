namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TimeTable2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_TimeTable", "Subject_ID", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_TimeTable", "Subject_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_TimeTable", "Subject_Name");
            DropColumn("dbo.Tbl_TimeTable", "Subject_ID");
        }
    }
}
