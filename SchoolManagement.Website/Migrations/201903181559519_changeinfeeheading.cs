namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeinfeeheading : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.FeeHeadings", "Feb", c => c.Byte(nullable: false));
            //DropColumn("dbo.FeeHeadings", "Far");
        }
        
        public override void Down()
        {
            //AddColumn("dbo.FeeHeadings", "Far", c => c.Byte(nullable: false));
            //DropColumn("dbo.FeeHeadings", "Feb");
        }
    }
}
