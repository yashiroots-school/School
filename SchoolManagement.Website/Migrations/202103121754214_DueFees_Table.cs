namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DueFees_Table : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.TblDueFee",
            //    c => new
            //    {
            //        DueFeeId = c.Int(nullable: false, identity: true),
            //        FeeHeadingId = c.Int(nullable: false),
            //        StudentId = c.Int(nullable: false),
            //        StudentName = c.String(),
            //        FeeHeading = c.String(),
            //        Jan = c.String(),
            //        Feb = c.String(),
            //        Mar = c.String(),
            //        Apr = c.String(),
            //        May = c.String(),
            //        Jun = c.String(),
            //        Jul = c.String(),
            //        Aug = c.String(),
            //        Sep = c.String(),
            //        Oct = c.String(),
            //        Nov = c.String(),
            //        Dec = c.String(),
            //        PaidMonths = c.String(),
            //        ClassId = c.Int(nullable: false),
            //        CategoryId = c.Int(nullable: false),
            //        ClassName = c.String(),
            //        CategoryName = c.String(),
            //        PayHeadings = c.String(),
            //        TotalFee = c.Single(nullable: false),
            //        FeePaids = c.String(),
            //        Course = c.String(),
            //        CourseSpecialization = c.String(),
            //        FeeReceiptsOneTimeCreator = c.String(),
            //        PaidAmount = c.Single(nullable: false),
            //        DueAmount = c.Single(nullable: false),
            //        UpdatedBy = c.String(),
            //        AddedDate = c.String(),
            //        ModifiedDate = c.String(),
            //        CurrentYear = c.Int(nullable: false),
            //        IP = c.String(),
            //        UserId = c.String(),
            //        IsDeleted = c.Boolean(nullable: false),
            //        CreateBy = c.Int(nullable: false),
            //        InsertBy = c.String(),
            //        BatchName = c.String(),
            //    })
            //    .PrimaryKey(t => t.DueFeeId);

        }

        public override void Down()
        {
            //DropTable("dbo.TblDueFee");
        }
    }
}
