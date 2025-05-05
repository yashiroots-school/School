namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addNew : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.tbl_Department",
            //    c => new
            //        {
            //            DepartmentId = c.Long(nullable: false, identity: true),
            //            DepartmentName = c.String(),
            //        })
            //    .PrimaryKey(t => t.DepartmentId);
        }
        
        public override void Down()
        {
            //DropTable("dbo.tbl_Department");
        }
    }
}
