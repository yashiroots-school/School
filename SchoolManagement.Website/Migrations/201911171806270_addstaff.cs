namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addstaff : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Tbl_Staff",
            //    c => new
            //        {
            //            StaffId = c.Int(nullable: false, identity: true),
            //            StaffName = c.String(),
            //            Email = c.String(),
            //            Password = c.String(),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.StaffId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.Tbl_Staff");
        }
    }
}
