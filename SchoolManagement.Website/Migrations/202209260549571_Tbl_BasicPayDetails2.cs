namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_BasicPayDetails2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tbl_BasicPayDetails", "Basic_Amount", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_BasicPayDetails", "Basic_Amount", c => c.Int(nullable: false));
        }
    }
}
