namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_BasicPayDetails1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tbl_BasicPayDetails", "Basic_Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_BasicPayDetails", "Basic_Amount", c => c.String());
        }
    }
}
