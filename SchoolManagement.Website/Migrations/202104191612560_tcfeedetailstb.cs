namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tcfeedetailstb : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails");
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentTcDetailsId" });
            //AlterColumn("dbo.TcFeeDetails", "StudentTcDetailsId", c => c.Long());
            //CreateIndex("dbo.TcFeeDetails", "StudentTcDetailsId");
            //AddForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails", "Id");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails");
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentTcDetailsId" });
            //AlterColumn("dbo.TcFeeDetails", "StudentTcDetailsId", c => c.Long(nullable: false));
            //CreateIndex("dbo.TcFeeDetails", "StudentTcDetailsId");
            //AddForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails", "Id", cascadeDelete: true);
        }
    }
}
