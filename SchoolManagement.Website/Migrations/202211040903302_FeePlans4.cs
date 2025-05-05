namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "Fee_configurationid", c => c.String());
            AddColumn("dbo.FeePlans", "FeeConfigurationname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "FeeConfigurationname");
            DropColumn("dbo.FeePlans", "Fee_configurationid");
        }
    }
}
