namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editStudentRemoteAccess : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.StudentRemoteAccesses", "AddedDate", c => c.DateTime(nullable: false));
            //AddColumn("dbo.StudentRemoteAccesses", "ModifiedDate", c => c.DateTime(nullable: false));
            //AddColumn("dbo.StudentRemoteAccesses", "IP", c => c.String());
            //AddColumn("dbo.StudentRemoteAccesses", "UserId", c => c.String());
            //AddColumn("dbo.StudentRemoteAccesses", "IsDeleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.StudentRemoteAccesses", "IsDeleted");
            //DropColumn("dbo.StudentRemoteAccesses", "UserId");
            //DropColumn("dbo.StudentRemoteAccesses", "IP");
            //DropColumn("dbo.StudentRemoteAccesses", "ModifiedDate");
            //DropColumn("dbo.StudentRemoteAccesses", "AddedDate");
        }
    }
}
