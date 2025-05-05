namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetableFeereceipts : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "Concession", c => c.Single(nullable: false));
            //AddColumn("dbo.TblFeeReceipts", "ConcessionAmt", c => c.Single(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "ConcessionAmt");
            //DropColumn("dbo.TblFeeReceipts", "Concession");
        }
    }
}
