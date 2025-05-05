namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StudentPromote1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_StudentPromote", "Student_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tbl_StudentPromote", "FromClass_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.Tbl_StudentPromote", "ToClass_Id", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Tbl_StudentPromote", "ToClass_Id", c => c.String());
            AlterColumn("dbo.Tbl_StudentPromote", "FromClass_Id", c => c.String());
            DropColumn("dbo.Tbl_StudentPromote", "Student_Id");
        }
    }
}
