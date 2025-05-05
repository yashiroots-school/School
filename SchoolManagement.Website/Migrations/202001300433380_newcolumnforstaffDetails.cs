namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newcolumnforstaffDetails : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.StafsDetails", "Designation", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.StafsDetails", "Designation");
        }
    }
}
