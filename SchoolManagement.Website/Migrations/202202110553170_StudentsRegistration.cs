namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Parents_Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Parents_Email");
        }
    }
}
