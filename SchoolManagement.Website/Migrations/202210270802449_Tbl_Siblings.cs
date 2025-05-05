namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Tbl_Siblings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Tbl_Siblings",
                c => new
                    {
                        Siblings_Id = c.Int(nullable: false, identity: true),
                        Student_Id = c.Int(nullable: false),
                        Studentname = c.String(),
                        Class_id = c.Int(nullable: false),
                        Confirmation = c.String(),
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
                .PrimaryKey(t => t.Siblings_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tbl_Siblings");
        }
    }
}
