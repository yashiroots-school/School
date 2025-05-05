namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Email_SendDate", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "Email_send", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Email_send");
            DropColumn("dbo.StudentsRegistrations", "Email_SendDate");
        }
    }
}
