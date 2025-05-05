namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentPromote : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentPromote", "FromClass_Id", c => c.String());
            AddColumn("dbo.Tbl_StudentPromote", "ToClass_Id", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StudentPromote", "ToClass_Id");
            DropColumn("dbo.Tbl_StudentPromote", "FromClass_Id");
        }
    }
}
