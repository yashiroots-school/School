namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class additionsomecolumn : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.AdditionalInformations", "Phisicallychalanged", c => c.String());
            //AddColumn("dbo.Students", "MarkForIdentity", c => c.String());
            //AddColumn("dbo.Students", "OtherLanguages", c => c.String());
            //AddColumn("dbo.FamilyDetails", "NoOfBrothers", c => c.String());
            //AddColumn("dbo.FamilyDetails", "NoOfSisters", c => c.String());
            //AddColumn("dbo.PastSchoolingReports", "Promotion", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.PastSchoolingReports", "Promotion");
            //DropColumn("dbo.FamilyDetails", "NoOfSisters");
            //DropColumn("dbo.FamilyDetails", "NoOfBrothers");
            //DropColumn("dbo.Students", "OtherLanguages");
            //DropColumn("dbo.Students", "MarkForIdentity");
            //DropColumn("dbo.AdditionalInformations", "Phisicallychalanged");
        }
    }
}
