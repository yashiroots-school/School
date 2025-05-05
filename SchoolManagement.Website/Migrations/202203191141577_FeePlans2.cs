namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FeePlans2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FeePlans", "KmDistance_Id", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FeePlans", "KmDistance_Id", c => c.Int(nullable: false));
        }
    }
}
