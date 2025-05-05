namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_SchoolSetup1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_SchoolSetup", "Fee_configurationId", c => c.String());
            AddColumn("dbo.Tbl_SchoolSetup", "Fee_Configuratinname", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_SchoolSetup", "Fee_Configuratinname");
            DropColumn("dbo.Tbl_SchoolSetup", "Fee_configurationId");
        }
    }
}
