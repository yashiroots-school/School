namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "City", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "State", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "Pincode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Pincode");
            DropColumn("dbo.StudentsRegistrations", "State");
            DropColumn("dbo.StudentsRegistrations", "City");
        }
    }
}
