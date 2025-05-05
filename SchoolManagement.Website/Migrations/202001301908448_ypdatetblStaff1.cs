namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ypdatetblStaff1 : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.StafsDetails", "Date", c => c.String());
            //AlterColumn("dbo.StafsDetails", "DOB", c => c.String());
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.StafsDetails", "DOB", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.StafsDetails", "Date", c => c.DateTime(nullable: false));
        }
    }
}
