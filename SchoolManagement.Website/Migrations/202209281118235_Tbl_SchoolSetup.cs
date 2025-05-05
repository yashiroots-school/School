namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SchoolSetup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_SchoolSetup",
                c => new
                    {
                        Schoolsetup_Id = c.Int(nullable: false, identity: true),
                        School_Id = c.Int(nullable: false),
                        Bank_Id = c.Int(nullable: false),
                        Branch_Id = c.Int(nullable: false),
                        Merchant_nameId = c.Int(nullable: false),
                        Status = c.String(),
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
                .PrimaryKey(t => t.Schoolsetup_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_SchoolSetup");
        }
    }
}
