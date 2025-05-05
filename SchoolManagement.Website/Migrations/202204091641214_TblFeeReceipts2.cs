namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblFeeReceipts2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblFeeReceipts", "FeeHeadingIDs", c => c.String());
            DropColumn("dbo.TblFeeReceipts", "FeeHeadingId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TblFeeReceipts", "FeeHeadingId", c => c.Int(nullable: false));
            DropColumn("dbo.TblFeeReceipts", "FeeHeadingIDs");
        }
    }
}
