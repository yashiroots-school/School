namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TcFeeDetailsPaidDate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.TcFeeDetails", "PaidDate", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TcFeeDetails", "PaidDate");
        }
    }
}
