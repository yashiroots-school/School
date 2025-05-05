namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_BloodGroup : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_BloodGroup",
                c => new
                    {
                        BloodGroup_Id = c.Int(nullable: false, identity: true),
                        Blood_Group = c.String(),
                    })
                .PrimaryKey(t => t.BloodGroup_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_BloodGroup");
        }
    }
}
