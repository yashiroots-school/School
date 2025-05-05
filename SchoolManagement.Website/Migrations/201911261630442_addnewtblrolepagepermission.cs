namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtblrolepagepermission : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.RolePagePermissions",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            RoleId = c.String(),
            //            PageName = c.String(),
            //            HasPermission = c.Boolean(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.RolePagePermissions");
        }
    }
}
