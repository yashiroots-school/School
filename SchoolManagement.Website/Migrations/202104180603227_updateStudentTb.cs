namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateStudentTb : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Students", "IsInsertFromAd", c => c.Boolean());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Students", "IsInsertFromAd");
        }
    }
}
