namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_Semesters : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.tbl_Semester",
            //    c => new
            //        {
            //            SemesterId = c.Long(nullable: false, identity: true),
            //            ScholarNumber = c.String(nullable: false, maxLength: 50),
            //            Year = c.String(maxLength: 20),
            //            Sem = c.String(maxLength: 20),
            //            Percentage = c.String(maxLength: 50),
            //            Addedon = c.String(maxLength: 20),
            //            Addeby = c.String(maxLength: 20),
            //            Updatedon = c.String(maxLength: 20),
            //            Updatedby = c.String(maxLength: 20),
            //            Spare1 = c.String(maxLength: 35),
            //            Spare2 = c.String(maxLength: 35),
            //            Spare3 = c.String(maxLength: 35),
            //            perse2 = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            Persentagegrade = c.Decimal(nullable: false, precision: 18, scale: 2),
            //        })
            //    .PrimaryKey(t => t.SemesterId);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.tbl_Semester");
        }
    }
}
