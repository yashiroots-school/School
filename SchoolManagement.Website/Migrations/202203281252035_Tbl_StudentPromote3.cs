namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentPromote3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentPromote", "Firstname", c => c.String());
            AddColumn("dbo.Tbl_StudentPromote", "Lastname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentPromote", "Lastname");
            DropColumn("dbo.Tbl_StudentPromote", "Firstname");
        }
    }
}
