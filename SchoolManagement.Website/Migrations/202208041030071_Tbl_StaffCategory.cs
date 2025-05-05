namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_StaffCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_StaffCategory",
                c => new
                    {
                        Staff_Category_Id = c.Int(nullable: false, identity: true),
                        Category_Name = c.String(),
                        Created_Date = c.String(),
                    })
                .PrimaryKey(t => t.Staff_Category_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_StaffCategory");
        }
    }
}
