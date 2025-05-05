namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newaddedcolumndatatype : DbMigration
    {
        public override void Up()
        {
            //DropPrimaryKey("dbo.Departments");
            //AlterColumn("dbo.Departments", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.Departments", "Id");
        }
        
        public override void Down()
        {
            //DropPrimaryKey("dbo.Departments");
            //AlterColumn("dbo.Departments", "Id", c => c.String(nullable: false, maxLength: 128));
            //AddPrimaryKey("dbo.Departments", "Id");
        }
    }
}
