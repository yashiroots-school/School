namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Mobile", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Mobile");
        }
    }
}
