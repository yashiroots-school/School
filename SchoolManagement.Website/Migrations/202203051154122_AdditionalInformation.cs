namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AdditionalInformation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AdditionalInformations", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.AdditionalInformations", "Class_Name", c => c.String());
            AddColumn("dbo.AdditionalInformations", "Section_Id", c => c.Int(nullable: false));
            AddColumn("dbo.AdditionalInformations", "Section_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AdditionalInformations", "Section_Name");
            DropColumn("dbo.AdditionalInformations", "Section_Id");
            DropColumn("dbo.AdditionalInformations", "Class_Name");
            DropColumn("dbo.AdditionalInformations", "Class_Id");
        }
    }
}
