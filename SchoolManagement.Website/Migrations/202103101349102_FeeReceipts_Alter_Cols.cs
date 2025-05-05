namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeeReceipts_Alter_Cols : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "DueAmount", c => c.String());
            //AddColumn("dbo.TblFeeReceipts", "PaidAmount", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "DueAmount");
            //DropColumn("dbo.TblFeeReceipts", "PaidAmount");
        }
    }
}
