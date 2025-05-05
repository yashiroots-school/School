namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_ArchieveStaffSalary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_ArchieveStaffSalary",
                c => new
                    {
                        Archieve_Id = c.Int(nullable: false, identity: true),
                        Salary_Id = c.Int(nullable: false),
                        Staff_ID = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        Salary_Amount = c.Int(nullable: false),
                        CreatedDate = c.String(),
                        Basic_Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Archieve_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_ArchieveStaffSalary");
        }
    }
}
