namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentsRegistration3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentsRegistrations", "Batch_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentsRegistrations", "Batch_Name", c => c.String());
            AddColumn("dbo.StudentsRegistrations", "BloodGroup_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentsRegistrations", "Religion_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentsRegistrations", "Cast_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentsRegistrations", "Category_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentsRegistrations", "Category_Id");
            DropColumn("dbo.StudentsRegistrations", "Cast_Id");
            DropColumn("dbo.StudentsRegistrations", "Religion_Id");
            DropColumn("dbo.StudentsRegistrations", "BloodGroup_Id");
            DropColumn("dbo.StudentsRegistrations", "Batch_Name");
            DropColumn("dbo.StudentsRegistrations", "Batch_Id");
        }
    }
}
