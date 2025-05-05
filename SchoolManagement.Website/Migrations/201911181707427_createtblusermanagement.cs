namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createtblusermanagement : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.Tbl_UserManagement",
            //    c => new
            //        {
            //            UserId = c.Int(nullable: false, identity: true),
            //            UserName = c.String(),
            //            Email = c.String(),
            //            Password = c.String(),
            //            Description = c.String(),
            //        })
            //    .PrimaryKey(t => t.UserId);
            
            //DropTable("dbo.Tbl_Staff");
        }
        
        public override void Down()
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
            
            //DropTable("dbo.Tbl_UserManagement");
        }
    }
}
