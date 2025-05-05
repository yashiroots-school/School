namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updatetables1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Students", "POB", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Students", "POB", c => c.DateTime(nullable: false));
        }
    }
}
