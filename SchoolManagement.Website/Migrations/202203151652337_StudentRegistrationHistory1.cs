namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentRegistrationHistory1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentRegistrationHistories", "Parents_Email", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentRegistrationHistories", "Parents_Email");
        }
    }
}
