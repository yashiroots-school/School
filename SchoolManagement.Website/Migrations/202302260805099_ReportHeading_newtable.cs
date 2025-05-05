namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReportHeading_newtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MasterReports",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                        UpdateAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ReportHeadings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ReportId = c.Long(nullable: false),
                        HeadingId = c.Int(nullable: false),
                        OrderNo = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ReportHeadings");
            DropTable("dbo.MasterReports");
        }
    }
}
