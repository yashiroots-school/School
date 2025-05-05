namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffSalary2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StaffSalary", "Basic_Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_StaffSalary", "Basic_Amount");
        }
    }
}
