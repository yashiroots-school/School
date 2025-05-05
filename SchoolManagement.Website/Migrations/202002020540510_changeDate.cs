namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeDate : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.Students", "Date", c => c.String());
            //AlterColumn("dbo.Students", "DOB", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.Students", "DOB", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Students", "Date", c => c.DateTime(nullable: false));
        }
    }
}
