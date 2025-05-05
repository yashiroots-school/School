namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateTblFeeReceipt : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "FeePaidDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "FeePaidDate");
        }
    }
}
