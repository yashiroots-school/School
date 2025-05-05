namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Transport", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "Transport_Options", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Transport_Options");
            DropColumn("dbo.StudentsRegistrations", "Transport");
        }
    }
}
