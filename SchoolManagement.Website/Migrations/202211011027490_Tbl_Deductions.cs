namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Deductions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Deductions",
                c => new
                    {
                        Deductions_Id = c.Int(nullable: false, identity: true),
                        Staff_Id = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        Net_Pay = c.Int(nullable: false),
                        Deduction_Amt = c.Int(nullable: false),
                        Added_Date = c.String(),
                        Added_Month = c.String(),
                        Added_Year = c.String(),
                        Added_Day = c.String(),
                        Remarks = c.String(),
                        AddedDate = c.String(),
                        ModifiedDate = c.String(),
                        CurrentYear = c.Int(nullable: false),
                        IP = c.String(),
                        UserId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        InsertBy = c.String(),
                        BatchName = c.String(),
                    })
                .PrimaryKey(t => t.Deductions_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Deductions");
        }
    }
}
