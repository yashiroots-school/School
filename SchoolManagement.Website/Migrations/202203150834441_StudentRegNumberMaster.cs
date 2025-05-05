namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentRegNumberMaster : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StudentRegNumberMasters", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.StudentRegNumberMasters", "Batch_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.StudentRegNumberMasters", "Batch_Id");
            DropColumn("dbo.StudentRegNumberMasters", "Class_Id");
        }
    }
}
