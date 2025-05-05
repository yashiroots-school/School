namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblCreateSchool1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblCreateSchools", "Upload_Image", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblCreateSchools", "Upload_Image");
        }
    }
}
