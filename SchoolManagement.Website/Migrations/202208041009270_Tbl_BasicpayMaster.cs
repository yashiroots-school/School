namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_BasicpayMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_BasicpayMaster",
                c => new
                {
                    BasicPay_MasterId = c.Int(nullable: false, identity: true),
                    Basicpay_Name = c.String(),
                    Created_Date = c.String(),
                })
                .PrimaryKey(t => t.BasicPay_MasterId);

        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_BasicpayMaster");
        }
    }
}
