namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addingsomecolumn : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AdditionalInformations", "Group", c => c.String());
            //AddColumn("dbo.Students", "RTE", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Students", "RTE");
            //DropColumn("dbo.AdditionalInformations", "Group");
        }
    }
}
