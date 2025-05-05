namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblFeeReceipts : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblFeeReceipts", "ApplicationNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblFeeReceipts", "ApplicationNumber");
        }
    }
}
