namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetbleFeeReceipt2 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.TblFeeReceipts", "FeePaids", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.TblFeeReceipts", "FeePaids", c => c.Single(nullable: false));
        }
    }
}
