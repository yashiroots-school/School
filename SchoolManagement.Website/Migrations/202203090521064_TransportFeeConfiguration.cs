namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TransportFeeConfiguration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TransportFeeConfigurations", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.TransportFeeConfigurations", "Batch_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TransportFeeConfigurations", "Batch_Id");
            DropColumn("dbo.TransportFeeConfigurations", "Class_Id");
        }
    }
}
