namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblFeeReceipts3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblFeeReceipts", "FeeconfigurationId", c => c.String());
            AddColumn("dbo.TblFeeReceipts", "Feeconfigurationname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblFeeReceipts", "Feeconfigurationname");
            DropColumn("dbo.TblFeeReceipts", "FeeconfigurationId");
        }
    }
}
