namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BatchName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Batch_Id", c => c.Int(nullable: false));
            AddColumn("dbo.Subjects", "Class_Teacher", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Class_Teacher");
            DropColumn("dbo.Subjects", "Batch_Id");
        }
    }
}
