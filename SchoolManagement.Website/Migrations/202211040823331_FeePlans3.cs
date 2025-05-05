namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "Jan", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Feb", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Mar", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Apr", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "May", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Jun", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Jul", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Aug", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Sep", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Oct", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Nov", c => c.Byte(nullable: false));
            AddColumn("dbo.FeePlans", "Dec", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "Dec");
            DropColumn("dbo.FeePlans", "Nov");
            DropColumn("dbo.FeePlans", "Oct");
            DropColumn("dbo.FeePlans", "Sep");
            DropColumn("dbo.FeePlans", "Aug");
            DropColumn("dbo.FeePlans", "Jul");
            DropColumn("dbo.FeePlans", "Jun");
            DropColumn("dbo.FeePlans", "May");
            DropColumn("dbo.FeePlans", "Apr");
            DropColumn("dbo.FeePlans", "Mar");
            DropColumn("dbo.FeePlans", "Feb");
            DropColumn("dbo.FeePlans", "Jan");
        }
    }
}
