namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_TimeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_TimeTable",
                c => new
                    {
                        TimeTable_Id = c.Int(nullable: false, identity: true),
                        Class_Name = c.String(),
                        Class_Id = c.Int(nullable: false),
                        Section_Name = c.String(),
                        Section_Id = c.Int(nullable: false),
                        Staff_Name = c.String(),
                        StafId = c.Int(nullable: false),
                        Room_Id = c.Int(nullable: false),
                        Room_Name = c.String(),
                    })
                .PrimaryKey(t => t.TimeTable_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_TimeTable");
        }
    }
}
