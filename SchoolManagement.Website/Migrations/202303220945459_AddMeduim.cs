namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMeduim : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FeePlans", "Medium", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.FeePlans", "Medium");
        }
    }
}
