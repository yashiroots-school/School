namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_AccountSummary2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tbl_AccountSummary", "DA", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Professional_Tax", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Added_Date", c => c.String());
            AddColumn("dbo.Tbl_AccountSummary", "Added_Month", c => c.String());
            AddColumn("dbo.Tbl_AccountSummary", "Added_Year", c => c.String());
            AddColumn("dbo.Tbl_AccountSummary", "Added_Day", c => c.String());
            AddColumn("dbo.Tbl_AccountSummary", "Employee_Contribution", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Employer_Contribution", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Net_Pay", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Attendence_Percentage", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "ESI", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Gross", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "Total_Salary", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "LOP", c => c.Double(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "CCA", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "HRA", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "OtherALlowance", c => c.Int(nullable: false));
            AddColumn("dbo.Tbl_AccountSummary", "NoOfdayspresent", c => c.String());
            AddColumn("dbo.Tbl_AccountSummary", "TotalPercentage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tbl_AccountSummary", "TotalPercentage");
            DropColumn("dbo.Tbl_AccountSummary", "NoOfdayspresent");
            DropColumn("dbo.Tbl_AccountSummary", "OtherALlowance");
            DropColumn("dbo.Tbl_AccountSummary", "HRA");
            DropColumn("dbo.Tbl_AccountSummary", "CCA");
            DropColumn("dbo.Tbl_AccountSummary", "LOP");
            DropColumn("dbo.Tbl_AccountSummary", "Total_Salary");
            DropColumn("dbo.Tbl_AccountSummary", "Gross");
            DropColumn("dbo.Tbl_AccountSummary", "ESI");
            DropColumn("dbo.Tbl_AccountSummary", "Attendence_Percentage");
            DropColumn("dbo.Tbl_AccountSummary", "Net_Pay");
            DropColumn("dbo.Tbl_AccountSummary", "Employer_Contribution");
            DropColumn("dbo.Tbl_AccountSummary", "Employee_Contribution");
            DropColumn("dbo.Tbl_AccountSummary", "Added_Day");
            DropColumn("dbo.Tbl_AccountSummary", "Added_Year");
            DropColumn("dbo.Tbl_AccountSummary", "Added_Month");
            DropColumn("dbo.Tbl_AccountSummary", "Added_Date");
            DropColumn("dbo.Tbl_AccountSummary", "Professional_Tax");
            DropColumn("dbo.Tbl_AccountSummary", "DA");
        }
    }
}
