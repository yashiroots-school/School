namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPassword : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TblCreateSchools", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TblCreateSchools", "Password");
        }
    }
}
