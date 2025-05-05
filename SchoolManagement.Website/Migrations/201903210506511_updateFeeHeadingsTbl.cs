namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateFeeHeadingsTbl : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "StudentName", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "PayHeadings", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "OldBalance", c => c.Single(nullable: false));
            //AddColumn("dbo.TblFeeReceipts", "ReceiptAmt", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "ReceiptAmt");
            //DropColumn("dbo.TblFeeReceipts", "OldBalance");
            //DropColumn("dbo.TblFeeReceipts", "PayHeadings");
            //DropColumn("dbo.TblFeeReceipts", "StudentName");
        }
    }
}
