namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_CreateBank : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_CreateBank",
                c => new
                    {
                        Bank_Id = c.Int(nullable: false, identity: true),
                        Bank_Name = c.String(),
                        Bank_Code = c.String(),
                        Contact_No = c.String(),
                        LandlineNo = c.String(),
                        Contactperson_Name = c.String(),
                    })
                .PrimaryKey(t => t.Bank_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_CreateBank");
        }
    }
}
