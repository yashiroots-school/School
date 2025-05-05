namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtablefrequency : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Frequencys",
            //    c => new
            //        {
            //            FeeFrequencyId = c.Int(nullable: false, identity: true),
            //            FeeFrequencyName = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.FeeFrequencyId);
            
            //CreateIndex("dbo.FeeHeadings", "FeeFrequencyId");
            //AddForeignKey("dbo.FeeHeadings", "FeeFrequencyId", "dbo.Frequencys", "FeeFrequencyId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.FeeHeadings", "FeeFrequencyId", "dbo.Frequencys");
            //DropIndex("dbo.FeeHeadings", new[] { "FeeFrequencyId" });
            //DropTable("dbo.Frequencys");
        }
    }
}
