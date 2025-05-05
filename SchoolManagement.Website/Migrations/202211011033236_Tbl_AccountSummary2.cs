namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_AccountSummary2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_AccountSummary", "Deduction_Amt", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Arrear_Amt", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Arrear", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_AccountSummary", "Arrear");
            DropColumn("dbo.Tbl_AccountSummary", "Arrear_Amt");
            DropColumn("dbo.Tbl_AccountSummary", "Deduction_Amt");
        }
    }
}
