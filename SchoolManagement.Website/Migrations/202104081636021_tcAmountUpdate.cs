namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcAmountUpdate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Tbl_TcAmount", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Tbl_TcAmount", "IsDeleted");
        }
    }
}
