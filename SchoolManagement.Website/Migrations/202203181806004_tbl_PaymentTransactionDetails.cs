namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_PaymentTransactionDetails : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_PaymentTransactionDetails", "ApplicationNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_PaymentTransactionDetails", "ApplicationNumber");
        }
    }
}
