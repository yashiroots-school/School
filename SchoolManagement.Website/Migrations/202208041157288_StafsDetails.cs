namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StafsDetails : DbMigration
    {
        public override void Up()
        {
            //AddColumn("dbo.StafsDetails", "EmployeeCode", c => c.String());
            //AddColumn("dbo.StafsDetails", "Bank_Name", c => c.String());
            //AddColumn("dbo.StafsDetails", "Account_No", c => c.String());
            //AddColumn("dbo.StafsDetails", "IFSC_Code", c => c.String());
            //AddColumn("dbo.StafsDetails", "Employee_Designation", c => c.String());
            //AddColumn("dbo.StafsDetails", "Employee_AccountId", c => c.Int(nullable: false));
            //AddColumn("dbo.StafsDetails", "Employee_AccountName", c => c.String());
            //AddColumn("dbo.StafsDetails", "Category_Id", c => c.Int(nullable: false));
            //AddColumn("dbo.StafsDetails", "Staff_CategoryName", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.StafsDetails", "Staff_CategoryName");
            //DropColumn("dbo.StafsDetails", "Category_Id");
            //DropColumn("dbo.StafsDetails", "Employee_AccountName");
            //DropColumn("dbo.StafsDetails", "Employee_AccountId");
            //DropColumn("dbo.StafsDetails", "Employee_Designation");
            //DropColumn("dbo.StafsDetails", "IFSC_Code");
            //DropColumn("dbo.StafsDetails", "Account_No");
            //DropColumn("dbo.StafsDetails", "Bank_Name");
            //DropColumn("dbo.StafsDetails", "EmployeeCode");
        }
    }
}
