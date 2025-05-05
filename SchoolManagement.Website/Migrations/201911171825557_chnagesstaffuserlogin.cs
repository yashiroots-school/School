namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chnagesstaffuserlogin : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AspNetUsers", "IsEnable", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.AspNetUsers", "IsEnable");
        }
    }
}
