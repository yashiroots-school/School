namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_StudentDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_StudentDetail", "Class_Id", c => c.Int(nullable: false));
            AddColumn("dbo.tbl_StudentDetail", "Batch_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_StudentDetail", "Batch_Id");
            DropColumn("dbo.tbl_StudentDetail", "Class_Id");
        }
    }
}
