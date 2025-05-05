namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_AccountSummary1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tbl_AccountSummary", "Staff_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_AccountSummary", "Staff_Id", c => c.String());
        }
    }
}
