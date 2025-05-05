namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newtableaddings : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.ClassAndSections",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Class = c.String(),
            //            Section = c.String(),
            //            OtherSection = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Classrooms",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            className = c.String(),
            //            RoomNo = c.String(),
            //            RoomType = c.String(),
            //            Seatingcapacity = c.String(),
            //            Location = c.String(),
            //            Remarks = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Departments",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 128),
            //            Depertment = c.String(),
            //            Other = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Subjects",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Teacher = c.String(),
            //            Class = c.String(),
            //            Subject = c.String(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Subjects");
            //DropTable("dbo.Departments");
            //DropTable("dbo.Classrooms");
            //DropTable("dbo.ClassAndSections");
        }
    }
}
