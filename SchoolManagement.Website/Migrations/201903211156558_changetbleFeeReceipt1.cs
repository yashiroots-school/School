namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetbleFeeReceipt1 : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "FeePaids", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "FeePaids");
        }
    }
}
