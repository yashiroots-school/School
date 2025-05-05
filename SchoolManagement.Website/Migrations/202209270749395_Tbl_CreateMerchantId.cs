namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_CreateMerchantId : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_CreateMerchantId",
                c => new
                    {
                        Merchant_Id = c.Int(nullable: false, identity: true),
                        School_Id = c.Int(nullable: false),
                        Bank_Id = c.Int(nullable: false),
                        Branch_Id = c.Int(nullable: false),
                        MerchantName_Id = c.Int(nullable: false),
                        MerchantMID = c.String(),
                        MerchantKey = c.String(),
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
                .PrimaryKey(t => t.Merchant_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_CreateMerchantId");
        }
    }
}
