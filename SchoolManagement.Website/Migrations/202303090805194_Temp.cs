namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Temp : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "FamilySSSMID", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "BankAccount", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "BankName", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "BankACHolder", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "BankIFSC", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "BankIFSC");
            DropColumn("dbo.StudentsRegistrations", "BankACHolder");
            DropColumn("dbo.StudentsRegistrations", "BankName");
            DropColumn("dbo.StudentsRegistrations", "BankAccount");
            DropColumn("dbo.StudentsRegistrations", "FamilySSSMID");
        }
    }
}
