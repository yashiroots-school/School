namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add2columnsinRegistration : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.tbl_StudentDetail", "Religious", c => c.String(maxLength: 50));
            //AddColumn("dbo.tbl_StudentDetail", "ReligiousOther", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.tbl_StudentDetail", "ReligiousOther");
            //DropColumn("dbo.tbl_StudentDetail", "Religious");
        }
    }
}
