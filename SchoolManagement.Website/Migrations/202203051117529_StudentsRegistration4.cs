namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Class_Name", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "Section_Name", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Section_Name");
            DropColumn("dbo.StudentsRegistrations", "Class_Name");
        }
    }
}
