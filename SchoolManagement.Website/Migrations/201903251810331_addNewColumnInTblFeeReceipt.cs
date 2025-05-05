namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNewColumnInTblFeeReceipt : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TblFeeReceipts", "FeeReceiptsOneTimeCreator", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TblFeeReceipts", "FeeReceiptsOneTimeCreator");
        }
    }
}
