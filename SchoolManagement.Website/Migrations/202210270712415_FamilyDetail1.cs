namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FamilyDetail1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FamilyDetails", "Siblings", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FamilyDetails", "Siblings");
        }
    }
}
