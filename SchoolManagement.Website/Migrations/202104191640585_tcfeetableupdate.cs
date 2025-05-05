namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcfeetableupdate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TcFeeDetails", "IsTcfee", c => c.Boolean());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TcFeeDetails", "IsTcfee");
        }
    }
}
