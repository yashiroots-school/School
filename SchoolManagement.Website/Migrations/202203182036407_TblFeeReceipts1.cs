namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblFeeReceipts1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TblFeeReceipts", "StudentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TblFeeReceipts", "StudentId", c => c.Int(nullable: false));
        }
    }
}
