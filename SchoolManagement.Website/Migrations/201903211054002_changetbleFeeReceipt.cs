namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetbleFeeReceipt : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "ClassName", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "CategoryName", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "TotalFee", c => c.Single(nullable: false));
            //AddColumn("dbo.TblFeeReceipts", "LateFee", c => c.Single(nullable: false));
            //AddColumn("dbo.TblFeeReceipts", "BalanceAmt", c => c.Single(nullable: false));
            //AddColumn("dbo.TblFeeReceipts", "PaymentMode", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "BankName", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "CheckId", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "Remark", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "Remark");
            //DropColumn("dbo.TblFeeReceipts", "CheckId");
            //DropColumn("dbo.TblFeeReceipts", "BankName");
            //DropColumn("dbo.TblFeeReceipts", "PaymentMode");
            //DropColumn("dbo.TblFeeReceipts", "BalanceAmt");
            //DropColumn("dbo.TblFeeReceipts", "LateFee");
            //DropColumn("dbo.TblFeeReceipts", "TotalFee");
            //DropColumn("dbo.TblFeeReceipts", "CategoryName");
            //DropColumn("dbo.TblFeeReceipts", "ClassName");
        }
    }
}
