namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Students", "Last_Name", c => c.String());
            AddColumn("dbo.Students", "Transport", c => c.String());
            AddColumn("dbo.Students", "Transport_Options", c => c.String());
            AddColumn("dbo.Students", "Mobile", c => c.String());
            AddColumn("dbo.Students", "City", c => c.String());
            AddColumn("dbo.Students", "State", c => c.String());
            AddColumn("dbo.Students", "Pincode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Students", "Pincode");
            DropColumn("dbo.Students", "State");
            DropColumn("dbo.Students", "City");
            DropColumn("dbo.Students", "Mobile");
            DropColumn("dbo.Students", "Transport_Options");
            DropColumn("dbo.Students", "Transport");
            DropColumn("dbo.Students", "Last_Name");
        }
    }
}
