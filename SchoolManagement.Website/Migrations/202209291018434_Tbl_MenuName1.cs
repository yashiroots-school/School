namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_MenuName1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_MenuName", "Upload_Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_MenuName", "Upload_Image");
        }
    }
}
