namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtablePromoteTable : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Tbl_StudentPromote",
            //    c => new
            //        {
            //            PromoteId = c.Int(nullable: false, identity: true),
            //            ScholarNumber = c.String(),
            //            FromClass = c.String(),
            //            ToClass = c.String(),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PromoteId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Tbl_StudentPromote");
        }
    }
}
