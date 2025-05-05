namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentPromote2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentPromote", "Registration_Date", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentPromote", "Registration_Date");
        }
    }
}
