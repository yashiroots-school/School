namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SMSEMAILTEMPLETE1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.SMSEMAILTEMPLETEs", "CREATEDDATE", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.SMSEMAILTEMPLETEs", "CREATEDDATE", c => c.DateTime(nullable: false));
        }
    }
}
