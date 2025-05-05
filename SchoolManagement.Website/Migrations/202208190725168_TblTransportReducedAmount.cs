namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblTransportReducedAmount : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TblTransportReducedAmounts",
                c => new
                    {
                        ReducedAmount_Id = c.Int(nullable: false, identity: true),
                        Amount = c.String(),
                        Range = c.String(),
                    })
                .PrimaryKey(t => t.ReducedAmount_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TblTransportReducedAmounts");
        }
    }
}
