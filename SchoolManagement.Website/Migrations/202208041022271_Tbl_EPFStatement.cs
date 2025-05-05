namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_EPFStatement : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_EPFStatement",
                c => new
                    {
                        EPFstatement_Id = c.Int(nullable: false, identity: true),
                        Employee_Code = c.String(),
                        UIN = c.String(),
                        Employee_Name = c.String(),
                        Gross_Wages = c.Int(nullable: false),
                        Epf_Wages = c.Int(nullable: false),
                        EPs_Wages = c.Int(nullable: false),
                        EDLIWages = c.Int(nullable: false),
                        Employe_Contribution = c.Int(nullable: false),
                        Employer_Contribution = c.Int(nullable: false),
                        EPS_Pension = c.Int(nullable: false),
                        NCP_Days = c.Int(nullable: false),
                        Refund_Advances = c.Int(nullable: false),
                        Added_Date = c.String(),
                        Added_Day = c.String(),
                        Added_Month = c.String(),
                        Added_Year = c.String(),
                        StaffCategory_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EPFstatement_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_EPFStatement");
        }
    }
}
