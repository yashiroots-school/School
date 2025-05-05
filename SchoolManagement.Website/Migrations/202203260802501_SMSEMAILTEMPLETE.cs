namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SMSEMAILTEMPLETE : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SMSEMAILTEMPLETEs", "SMSSubject", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.SMSEMAILTEMPLETEs", "SMSSubject");
        }
    }
}
