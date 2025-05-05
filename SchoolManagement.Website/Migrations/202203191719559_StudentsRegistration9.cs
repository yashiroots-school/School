namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration9 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "AddedYear", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "AddedYear");
        }
    }
}
