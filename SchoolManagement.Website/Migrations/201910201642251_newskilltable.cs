namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newskilltable : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.tbl_skillset",
            //    c => new
            //        {
            //            SkillsetId = c.Long(nullable: false, identity: true),
            //            ScholarNumber = c.String(),
            //            Problemsolving = c.String(),
            //            Initiative = c.String(),
            //            Adaptabilitytochange = c.String(),
            //            Interpersonalskills = c.String(),
            //            Strategicthinking = c.String(),
            //            Timemanagement = c.String(),
            //            Communication = c.String(),
            //            Leadership = c.String(),
            //            Teamwork = c.String(),
            //            Dancing = c.String(),
            //            Singing = c.String(),
            //            Compering = c.String(),
            //            Creative = c.String(),
            //            Contentwriting = c.String(),
            //            CoralDraw = c.String(),
            //            Photoshop = c.String(),
            //            Drawing = c.String(),
            //            Spare1 = c.String(),
            //            Spare2 = c.String(),
            //            Spare3 = c.String(),
            //            Addedon = c.String(maxLength: 20),
            //            Addedby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //        })
            //    .PrimaryKey(t => t.SkillsetId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.tbl_skillset");
        }
    }
}
