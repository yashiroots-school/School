namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_BasicPayDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_BasicPayDetails",
                c => new
                    {
                        BasicAmount_Id = c.Int(nullable: false, identity: true),
                        SchoolCategory_Id = c.Int(nullable: false),
                        BasicPay_Id = c.Int(nullable: false),
                        Category_Name = c.String(),
                        Basicpay_Name = c.String(),
                        Basic_Amount = c.String(),
                        CreatedDate = c.String(),
                    })
                .PrimaryKey(t => t.BasicAmount_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_BasicPayDetails");
        }
    }
}
