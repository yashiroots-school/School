namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class change_tcfee : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.TcFeeDetails");
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentTcDetailsId" });
            //DropPrimaryKey("dbo.TcFeeDetails");
            //AlterColumn("dbo.TcFeeDetails", "Id", c => c.Long(nullable: false, identity: true));
            //AlterColumn("dbo.TcFeeDetails", "StudentTcDetailsId", c => c.Long(nullable: false));
            //AddPrimaryKey("dbo.TcFeeDetails", "Id");
            //CreateIndex("dbo.TcFeeDetails", "StudentTcDetailsId");
            //AddForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.StudentTcDetails");
            //DropIndex("dbo.TcFeeDetails", new[] { "StudentTcDetailsId" });
            //DropPrimaryKey("dbo.TcFeeDetails");
            //AlterColumn("dbo.TcFeeDetails", "StudentTcDetailsId", c => c.Int(nullable: false));
            //AlterColumn("dbo.TcFeeDetails", "Id", c => c.Int(nullable: false, identity: true));
            //AddPrimaryKey("dbo.TcFeeDetails", "Id");
            //CreateIndex("dbo.TcFeeDetails", "StudentTcDetailsId");
            //AddForeignKey("dbo.TcFeeDetails", "StudentTcDetailsId", "dbo.TcFeeDetails", "Id");
        }
    }
}
