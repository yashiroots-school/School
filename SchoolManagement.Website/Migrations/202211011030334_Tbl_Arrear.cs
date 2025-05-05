namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Arrear : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Arrear",
                c => new
                    {
                        Arrear_Id = c.Int(nullable: false, identity: true),
                        Arrear_Amt = c.Int(nullable: false),
                        Arrear = c.String(),
                        Staff_Id = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        Net_Pay = c.Int(nullable: false),
                        Added_Date = c.String(),
                        Added_Month = c.String(),
                        Added_Year = c.String(),
                        Added_Day = c.String(),
                        Deduction_Amt = c.Int(nullable: false),
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
                .PrimaryKey(t => t.Arrear_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Arrear");
        }
    }
}
