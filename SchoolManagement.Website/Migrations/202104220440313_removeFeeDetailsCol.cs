namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeFeeDetailsCol : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.TblFeeReceipts", "FeePaidDate");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.TblFeeReceipts", "FeePaidDate", c => c.DateTime());
        }
    }
}
