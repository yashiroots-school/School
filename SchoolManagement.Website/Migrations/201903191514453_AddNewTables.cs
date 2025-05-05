namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNewTables : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.TblFeeReceipts",
            //    c => new
            //        {
            //            FeeReceiptId = c.Int(nullable: false, identity: true),
            //            FeeHeadingId = c.Int(nullable: false),
            //            StudentId = c.Int(nullable: false),
            //            Jan = c.Boolean(nullable: false),
            //            Feb = c.Boolean(nullable: false),
            //            Mar = c.Boolean(nullable: false),
            //            Apr = c.Boolean(nullable: false),
            //            May = c.Boolean(nullable: false),
            //            Jun = c.Boolean(nullable: false),
            //            Jul = c.Boolean(nullable: false),
            //            Aug = c.Boolean(nullable: false),
            //            Sep = c.Boolean(nullable: false),
            //            Oct = c.Boolean(nullable: false),
            //            Nov = c.Boolean(nullable: false),
            //            Dec = c.Boolean(nullable: false),
            //            Type = c.String(),
            //            PaidMonths = c.String(),
            //            ClassId = c.Int(nullable: false),
            //            CategoryId = c.Int(nullable: false),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.FeeReceiptId);
            
            //CreateTable(
            //    "dbo.TblLateFees",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            FeeHeadingId = c.Int(nullable: false),
            //            LateFee = c.Single(nullable: false),
            //            Paid = c.Boolean(nullable: false),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.TblStudentFeeSaveds",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentId = c.Int(nullable: false),
            //            TotalFee = c.Single(nullable: false),
            //            FeePaid = c.Single(nullable: false),
            //            OldFee = c.Single(nullable: false),
            //            AddedDate = c.DateTime(nullable: false),
            //            ModifiedDate = c.DateTime(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            //DropTable("dbo.TblStudentFeeSaveds");
            //DropTable("dbo.TblLateFees");
            //DropTable("dbo.TblFeeReceipts");
        }
    }
}
