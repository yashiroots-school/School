namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FamilyDetail : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AdditionalInformations", "StudentRefId", "dbo.Students");
            DropForeignKey("dbo.GuardianDetails", "StudentRefId", "dbo.Students");
            DropForeignKey("dbo.PastSchoolingReports", "StudentRefId", "dbo.Students");
            DropIndex("dbo.AdditionalInformations", new[] { "StudentRefId" });
            DropIndex("dbo.GuardianDetails", new[] { "StudentRefId" });
            DropIndex("dbo.PastSchoolingReports", new[] { "StudentRefId" });
            AddColumn("dbo.AdditionalInformations", "Student_StudentId", c => c.Int());
            AddColumn("dbo.GuardianDetails", "Student_StudentId", c => c.Int());
            AddColumn("dbo.PastSchoolingReports", "Student_StudentId", c => c.Int());
            CreateIndex("dbo.AdditionalInformations", "Student_StudentId");
            CreateIndex("dbo.GuardianDetails", "Student_StudentId");
            CreateIndex("dbo.PastSchoolingReports", "Student_StudentId");
            AddForeignKey("dbo.AdditionalInformations", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.GuardianDetails", "Student_StudentId", "dbo.Students", "StudentId");
            AddForeignKey("dbo.PastSchoolingReports", "Student_StudentId", "dbo.Students", "StudentId");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PastSchoolingReports", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.GuardianDetails", "Student_StudentId", "dbo.Students");
            DropForeignKey("dbo.AdditionalInformations", "Student_StudentId", "dbo.Students");
            DropIndex("dbo.PastSchoolingReports", new[] { "Student_StudentId" });
            DropIndex("dbo.GuardianDetails", new[] { "Student_StudentId" });
            DropIndex("dbo.AdditionalInformations", new[] { "Student_StudentId" });
            DropColumn("dbo.PastSchoolingReports", "Student_StudentId");
            DropColumn("dbo.GuardianDetails", "Student_StudentId");
            DropColumn("dbo.AdditionalInformations", "Student_StudentId");
            CreateIndex("dbo.PastSchoolingReports", "StudentRefId");
            CreateIndex("dbo.GuardianDetails", "StudentRefId");
            CreateIndex("dbo.AdditionalInformations", "StudentRefId");
            AddForeignKey("dbo.PastSchoolingReports", "StudentRefId", "dbo.Students", "StudentId", cascadeDelete: true);
            AddForeignKey("dbo.GuardianDetails", "StudentRefId", "dbo.Students", "StudentId", cascadeDelete: true);
            AddForeignKey("dbo.AdditionalInformations", "StudentRefId", "dbo.Students", "StudentId", cascadeDelete: true);
        }
    }
}
