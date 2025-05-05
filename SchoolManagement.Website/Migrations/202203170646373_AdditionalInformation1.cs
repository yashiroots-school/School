namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalInformation1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdditionalInformations", "TransportOptions", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdditionalInformations", "TransportOptions");
        }
    }
}
