namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mynewcolumnadditions : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Students", "Medium", c => c.String());
            //AddColumn("dbo.Students", "Caste", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Students", "Caste");
            //DropColumn("dbo.Students", "Medium");
        }
    }
}
