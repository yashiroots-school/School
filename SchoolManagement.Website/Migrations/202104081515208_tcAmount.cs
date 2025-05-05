namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcAmount : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Tbl_TcAmount",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Type = c.Long(nullable: false),
            //            Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Tbl_TcAmount");
        }
    }
}
