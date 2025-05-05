namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_CreateBranch1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_CreateBranch", "Contact_No", c => c.String());
            AddColumn("dbo.Tbl_CreateBranch", "Contact_Name", c => c.String());
            AddColumn("dbo.Tbl_CreateBranch", "Landline_No", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_CreateBranch", "Landline_No");
            DropColumn("dbo.Tbl_CreateBranch", "Contact_Name");
            DropColumn("dbo.Tbl_CreateBranch", "Contact_No");
        }
    }
}
