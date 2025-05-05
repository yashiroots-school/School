namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "AdmissionFeePaid", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "AdmissionFeePaid");
        }
    }
}
