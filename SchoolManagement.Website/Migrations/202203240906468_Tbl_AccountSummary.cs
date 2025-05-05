namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_AccountSummary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_AccountSummary",
                c => new
                    {
                        Summary_Id = c.Int(nullable: false, identity: true),
                        Staff_Id = c.String(),
                        Staff_Name = c.String(),
                        NetPay = c.Int(nullable: false),
                        PF = c.Int(nullable: false),
                        Basic_Salary = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Summary_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_AccountSummary");
        }
    }
}
