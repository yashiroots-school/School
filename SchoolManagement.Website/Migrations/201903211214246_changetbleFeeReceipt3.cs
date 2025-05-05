namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changetbleFeeReceipt3 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.TblFeeReceipts", "Remark", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.TblFeeReceipts", "Remark", c => c.Single(nullable: false));
        }
    }
}
