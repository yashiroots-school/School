namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class studentupdate : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.Students", "IsAdmissionPaid", c => c.Boolean());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.Students", "IsAdmissionPaid");
        }
    }
}
