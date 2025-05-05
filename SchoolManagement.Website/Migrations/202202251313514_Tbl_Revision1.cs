namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Revision1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_Revision", "CreatedDate", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_Revision", "CreatedDate");
        }
    }
}
