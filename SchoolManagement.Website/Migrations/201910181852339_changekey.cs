namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changekey : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.tbl_WorkExperience", "ScholarNumber", "dbo.tbl_StudentDetail");
            //DropIndex("dbo.tbl_WorkExperience", new[] { "ScholarNumber" });
        }
        
        public override void Down()
        {
            //CreateIndex("dbo.tbl_WorkExperience", "ScholarNumber");
            //AddForeignKey("dbo.tbl_WorkExperience", "ScholarNumber", "dbo.tbl_StudentDetail", "ScholarNumber", cascadeDelete: true);
        }
    }
}
