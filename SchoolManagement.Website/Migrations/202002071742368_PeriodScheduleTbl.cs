namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeriodScheduleTbl : DbMigration
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
            
            //CreateTable(
            //    "dbo.TimeSettings",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Time1 = c.String(),
            //            Time11 = c.String(),
            //            Time2 = c.String(),
            //            Time22 = c.String(),
            //            Time3 = c.String(),
            //            Time33 = c.String(),
            //            Time4 = c.String(),
            //            Time44 = c.String(),
            //            Time5 = c.String(),
            //            Time55 = c.String(),
            //            Time6 = c.String(),
            //            Time66 = c.String(),
            //            Time7 = c.String(),
            //            Time77 = c.String(),
            //            Time8 = c.String(),
            //            Time88 = c.String(),
            //            Time9 = c.String(),
            //            Time99 = c.String(),
            //            Time10 = c.String(),
            //            Time101 = c.String(),
            //            Time102 = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.TimeSettings");
            //DropTable("dbo.PeriodSchedules");
        }
    }
}
