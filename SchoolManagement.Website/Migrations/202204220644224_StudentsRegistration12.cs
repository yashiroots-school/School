namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Promotion_Date", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "Promotion_Year", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Promotion_Year");
            DropColumn("dbo.StudentsRegistrations", "Promotion_Date");
        }
    }
}
