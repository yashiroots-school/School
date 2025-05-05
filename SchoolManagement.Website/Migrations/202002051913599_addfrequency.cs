namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfrequency : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Frequencys",
            //    c => new
            //        {
            //            FeeFrequencyId = c.Int(nullable: false, identity: true),
            //            FeeFrequencyName = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.FeeFrequencyId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Frequencys");
        }
    }
}
