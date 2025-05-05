namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_ArchieveChangeStaffAccounttype : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_ArchieveChangeStaffAccounttype",
                c => new
                    {
                        ChangeAccounType_ID = c.Int(nullable: false, identity: true),
                        StafID = c.Int(nullable: false),
                        EmpId = c.String(),
                        Staf_Name = c.String(),
                        Employee_Designation = c.String(),
                        Employee_AccountId = c.Int(nullable: false),
                        Employee_AccountName = c.String(),
                        Category_Id = c.Int(nullable: false),
                        Staff_CategoryName = c.String(),
                        Employee_Code = c.String(),
                    })
                .PrimaryKey(t => t.ChangeAccounType_ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_ArchieveChangeStaffAccounttype");
        }
    }
}
