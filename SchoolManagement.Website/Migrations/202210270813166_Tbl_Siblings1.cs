namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Siblings1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_Siblings", "FamilyDetails_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_Siblings", "FamilyDetails_Id");
        }
    }
}
