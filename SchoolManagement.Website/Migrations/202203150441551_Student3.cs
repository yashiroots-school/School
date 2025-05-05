namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "AdmissionFeePaid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "AdmissionFeePaid");
        }
    }
}
