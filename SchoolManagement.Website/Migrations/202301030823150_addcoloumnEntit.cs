namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addcoloumnEntit : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_Batches", "IsActiveForRegistrationFee", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_Batches", "IsActiveForRegistrationFee");
        }
    }
}
