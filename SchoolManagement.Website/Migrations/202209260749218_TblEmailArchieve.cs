namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblEmailArchieve : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TblEmailArchieves",
                c => new
                    {
                        Email_Id = c.Int(nullable: false, identity: true),
                        Student_id = c.Int(nullable: false),
                        ApplicationNumber = c.String(),
                        Name = c.String(),
                        Parent_Email = c.String(),
                        Email_Date = c.String(),
                        Email_Content = c.String(),
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
                .PrimaryKey(t => t.Email_Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TblEmailArchieves");
        }
    }
}
