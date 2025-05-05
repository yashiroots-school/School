namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "TransportOpt_Id", c => c.Int(nullable: false));
            AddColumn("dbo.FeePlans", "KmDistance_Id", c => c.Int(nullable: false));
            AddColumn("dbo.FeePlans", "Amount", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "Amount");
            DropColumn("dbo.FeePlans", "KmDistance_Id");
            DropColumn("dbo.FeePlans", "TransportOpt_Id");
        }
    }
}
