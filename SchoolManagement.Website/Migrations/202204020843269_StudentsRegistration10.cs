namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Registration_Date", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Registration_Date");
        }
    }
}
