namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Batches : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Batches",
                c => new
                    {
                        Batch_Id = c.Int(nullable: false, identity: true),
                        Batch_Name = c.String(),
                    })
                .PrimaryKey(t => t.Batch_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Batches");
        }
    }
}
