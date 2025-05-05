namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SalaryStatement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SalaryStatement",
                c => new
                    {
                        SalaryStatement_Id = c.Int(nullable: false, identity: true),
                        Employers_Designation = c.String(),
                        Employee_Name = c.String(),
                        Employee_Code = c.Int(nullable: false),
                        Employee_AccountNo = c.String(),
                        Total_Salary = c.String(),
                        AccountDetails_Id = c.Int(nullable: false),
                        Account_Details = c.String(),
                        Salarystatement_Month = c.String(),
                        Salarystatement_year = c.String(),
                        SalaryStatement_Date = c.String(),
                        StaffCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SalaryStatement_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_SalaryStatement");
        }
    }
}
