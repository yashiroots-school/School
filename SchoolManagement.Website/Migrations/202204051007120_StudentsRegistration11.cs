namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "IsEmailsent", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "IsEmailsent");
        }
    }
}
