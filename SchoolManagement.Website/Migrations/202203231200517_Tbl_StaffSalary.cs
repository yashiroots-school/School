namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffSalary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_StaffSalary",
                c => new
                    {
                        Salary_Id = c.Int(nullable: false, identity: true),
                        Staff_ID = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        Salary_Amount = c.String(),
                        CreatedDate = c.String(),
                    })
                .PrimaryKey(t => t.Salary_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_StaffSalary");
        }
    }
}
