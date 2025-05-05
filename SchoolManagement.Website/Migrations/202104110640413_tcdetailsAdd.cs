namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcdetailsAdd : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.TcFeeDetails",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            StudentTcDetailsId = c.Int(nullable: false),
            //            StudentId = c.Int(nullable: false),
            //            ReceiptAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
            //            PaymentMode = c.String(),
            //            CreatedOn = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.Id)
            //    .ForeignKey("dbo.Students", t => t.StudentId, cascadeDelete: true)
            //    .ForeignKey("dbo.TcFeeDetails", t => t.StudentTcDetailsId)
            //    .Index(t => t.StudentTcDetailsId)
            //    .Index(t => t.StudentId);
            
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.TcFeeDetails");
            //DropForeignKey("dbo.TcFeeDetails", "StudentId", "dbo.Students");
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentId" });
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentTcDetailsId" });
            //DropTable("dbo.TcFeeDetails");
        }
    }
}
