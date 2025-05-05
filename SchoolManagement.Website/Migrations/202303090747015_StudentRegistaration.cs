namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentRegistaration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "SSSMIdNumber", c => c.String());
            
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "SSSMIdNumber");
           
        }
    }
}
