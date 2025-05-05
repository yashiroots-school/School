namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblCreateSchool : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TblCreateSchools",
                c => new
                    {
                        School_Id = c.Int(nullable: false, identity: true),
                        School_Name = c.String(),
                        Address = c.String(),
                        Website = c.String(),
                        Copyright = c.String(),
                        Date = c.String(),
                        Email = c.String(),
                        AddedDate = c.String(),
                        ModifiedDate = c.String(),
                        CurrentYear = c.Int(nullable: false),
                        IP = c.String(),
                        UserId = c.String(),
                        IsDeleted = c.Boolean(nullable: false),
                        CreateBy = c.Int(nullable: false),
                        InsertBy = c.String(),
                        BatchName = c.String(),
                    })
                .PrimaryKey(t => t.School_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TblCreateSchools");
        }
    }
}
