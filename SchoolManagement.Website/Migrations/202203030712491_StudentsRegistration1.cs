namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentsRegistrations", "Section_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Section_Id");
            DropColumn("dbo.StudentsRegistrations", "Class_Id");
        }
    }
}
