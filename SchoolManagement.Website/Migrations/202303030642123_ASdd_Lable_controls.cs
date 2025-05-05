namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ASdd_Lable_controls : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LabelControls",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LableId = c.Int(nullable: false),
                        LabelName = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        School_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MasterLabels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LableId = c.String(),
                        SubMenu_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);

        }
        
        public override void Down()
        {
            DropTable("dbo.MasterLabels");
            DropTable("dbo.LabelControls");
        }
    }
}
