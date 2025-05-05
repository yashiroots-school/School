namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffSalary1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Tbl_StaffSalary", "Salary_Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_StaffSalary", "Salary_Amount", c => c.String());
        }
    }
}
