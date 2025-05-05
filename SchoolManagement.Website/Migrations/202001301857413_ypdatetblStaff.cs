namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ypdatetblStaff : DbMigration
    {
        public override void Up()
        {
            //AlterColumn("dbo.StafsDetails", "UIN", c => c.String());
            //AlterColumn("dbo.StafsDetails", "Name", c => c.String());
            //DropColumn("dbo.StafsDetails", "ApplicationNumber");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.StafsDetails", "ApplicationNumber", c => c.String(nullable: false));
            //AlterColumn("dbo.StafsDetails", "Name", c => c.String(nullable: false));
            //AlterColumn("dbo.StafsDetails", "UIN", c => c.String(nullable: false));
        }
    }
}
