namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chnages : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.tbl_StudentDetail", "Class", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            //DropColumn("dbo.tbl_StudentDetail", "Class");
        }
    }
}
