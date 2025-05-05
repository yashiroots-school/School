namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_AccountType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_AccountType",
                c => new
                {
                    Account_TypeId = c.Int(nullable: false, identity: true),
                    Account_Typename = c.String(),
                    Created_Date = c.String(),
                })
                .PrimaryKey(t => t.Account_TypeId);

        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_AccountType");
        }
    }
}
