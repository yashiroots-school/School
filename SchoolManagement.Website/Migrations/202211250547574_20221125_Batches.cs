namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _20221125_Batches : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_Batches", "IsActiveForAdmission", c => c.Boolean(nullable: false));
            AddColumn("dbo.Tbl_Batches", "IsActiveForPayments", c => c.Boolean(nullable: false));
            AddColumn("dbo.TblCreateSchools", "Status", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblCreateSchools", "Status");
            DropColumn("dbo.Tbl_Batches", "IsActiveForPayments");
            DropColumn("dbo.Tbl_Batches", "IsActiveForAdmission");
        }
    }
}
