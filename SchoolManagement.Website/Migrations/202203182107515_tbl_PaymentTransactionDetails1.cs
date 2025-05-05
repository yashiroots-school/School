namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tbl_PaymentTransactionDetails1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_PaymentTransactionDetails", "StudentId", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_PaymentTransactionDetails", "StudentId", c => c.Int(nullable: false));
        }
    }
}
