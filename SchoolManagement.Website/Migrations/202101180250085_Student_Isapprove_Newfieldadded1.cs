namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Student_Isapprove_Newfieldadded1 : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.PeriodSchedules",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Class = c.String(),
            //            Section = c.String(),
            //            Day = c.String(),
            //            RoomNo = c.String(),
            //            Period1 = c.String(),
            //            Teacher1 = c.String(),
            //            Period2 = c.String(),
            //            Teacher2 = c.String(),
            //            Period3 = c.String(),
            //            Teacher3 = c.String(),
            //            Period4 = c.String(),
            //            Teacher4 = c.String(),
            //            Period5 = c.String(),
            //            Teacher5 = c.String(),
            //            Period6 = c.String(),
            //            Teacher6 = c.String(),
            //            Period7 = c.String(),
            //            Teacher7 = c.String(),
            //            Period8 = c.String(),
            //            Teacher8 = c.String(),
            //            Period9 = c.String(),
            //            Teacher9 = c.String(),
            //            Period10 = c.String(),
            //            Teacher10 = c.String(),
            //            Period11 = c.String(),
            //            Teacher11 = c.String(),
            //            Period12 = c.String(),
            //            Teacher12 = c.String(),
            //            Period13 = c.String(),
            //            Teacher13 = c.String(),
            //            Period14 = c.String(),
            //            Teacher14 = c.String(),
            //            Period15 = c.String(),
            //            Teacher15 = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //AddColumn("dbo.Students", "IsApplyforTC", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Students", "IsApplyforAdmission", c => c.Boolean(nullable: false));
            //AddColumn("dbo.Students", "IsApprove", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Students", "IsApprove");
            //DropColumn("dbo.Students", "IsApplyforAdmission");
            //DropColumn("dbo.Students", "IsApplyforTC");
            //DropTable("dbo.PeriodSchedules");
        }
    }
}
