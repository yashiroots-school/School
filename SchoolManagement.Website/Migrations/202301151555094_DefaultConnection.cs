namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DefaultConnection : DbMigration
    {
        public override void Up()
        {

            //AlterColumn("dbo.Accounts", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.FeeHeadings", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.AdditionalInformations", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Students", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.FamilyDetails", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.GuardianDetails", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.PastSchoolingReports", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.StudentRemoteAccesses", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.FeeHeadingGroups", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.FeePlans", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Frequencys", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.StafsDetails", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.StudentsRegistrations", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.StudentRegistrationHistories", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_Arrear", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_CreateMerchantId", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_Deductions", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_MenuName", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_MerchantName", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_RolePermissionNew", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_SchoolSetup", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_Siblings", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_StaffAttendance", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_StudentPromote", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.Tbl_SubmenuName", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblCreateSchools", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblDueFees", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblEmailArchieves", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblFeeReceipts", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblLateFees", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblStudentFeeSaveds", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TblTransportFeeReceipts", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TcFeeDetails", "PaidDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TransportFeeHeadings", "AddedDate", c => c.DateTime(nullable: false));
            //AlterColumn("dbo.TransportFeePlans", "AddedDate", c => c.DateTime(nullable: false));


    //        AddColumn("dbo.TransportFeePlans", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TransportFeePlans
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TransportFeePlans", "AddedDate");
    //        RenameColumn("dbo.TransportFeePlans", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.TransportFeeHeadings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TransportFeeHeadings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TransportFeeHeadings", "AddedDate");
    //        RenameColumn("dbo.TransportFeeHeadings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TcFeeDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TcFeeDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TcFeeDetails", "AddedDate");
    //        RenameColumn("dbo.TcFeeDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblTransportFeeReceipts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblTransportFeeReceipts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblTransportFeeReceipts", "AddedDate");
    //        RenameColumn("dbo.TblTransportFeeReceipts", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblStudentFeeSaveds", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblStudentFeeSaveds
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblStudentFeeSaveds", "AddedDate");
    //        RenameColumn("dbo.TblStudentFeeSaveds", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblLateFees", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblLateFees
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblLateFees", "AddedDate");
    //        RenameColumn("dbo.TblLateFees", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.TblFeeReceipts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblFeeReceipts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblFeeReceipts", "AddedDate");
    //        RenameColumn("dbo.TblFeeReceipts", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblEmailArchieves", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblEmailArchieves
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblEmailArchieves", "AddedDate");
    //        RenameColumn("dbo.TblEmailArchieves", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblDueFees", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblDueFees
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblDueFees", "AddedDate");
    //        RenameColumn("dbo.TblDueFees", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Tbl_SubmenuName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_SubmenuName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_SubmenuName", "AddedDate");
    //        RenameColumn("dbo.Tbl_SubmenuName", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_StudentPromote", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_StudentPromote
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_StudentPromote", "AddedDate");
    //        RenameColumn("dbo.Tbl_StudentPromote", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_StaffAttendance", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_StaffAttendance
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_StaffAttendance", "AddedDate");
    //        RenameColumn("dbo.Tbl_StaffAttendance", "AddedDateTmp", "AddedDate");




    //        AddColumn("dbo.Tbl_Siblings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Siblings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Siblings", "AddedDate");
    //        RenameColumn("dbo.Tbl_Siblings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Tbl_SchoolSetup", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_SchoolSetup
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_SchoolSetup", "AddedDate");
    //        RenameColumn("dbo.Tbl_SchoolSetup", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_RolePermissionNew", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_RolePermissionNew
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_RolePermissionNew", "AddedDate");
    //        RenameColumn("dbo.Tbl_RolePermissionNew", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_MerchantName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_MerchantName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_MerchantName", "AddedDate");
    //        RenameColumn("dbo.Tbl_MerchantName", "AddedDateTmp", "AddedDate");




    //        AddColumn("dbo.Tbl_MenuName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_MenuName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_MenuName", "AddedDate");
    //        RenameColumn("dbo.Tbl_MenuName", "AddedDateTmp", "AddedDate");









    //        AddColumn("dbo.Tbl_Deductions", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Deductions
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Deductions", "AddedDate");
    //        RenameColumn("dbo.Tbl_Deductions", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_CreateMerchantId", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_CreateMerchantId
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_CreateMerchantId", "AddedDate");
    //        RenameColumn("dbo.Tbl_CreateMerchantId", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_Arrear", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Arrear
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Arrear", "AddedDate");
    //        RenameColumn("dbo.Tbl_Arrear", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.StudentRegistrationHistories", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentRegistrationHistories
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentRegistrationHistories", "AddedDate");
    //        RenameColumn("dbo.StudentRegistrationHistories", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.StudentsRegistrations", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentsRegistrations
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentsRegistrations", "AddedDate");
    //        RenameColumn("dbo.StudentsRegistrations", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.StafsDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StafsDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StafsDetails", "AddedDate");
    //        RenameColumn("dbo.StafsDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Frequencys", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Frequencys
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Frequencys", "AddedDate");
    //        RenameColumn("dbo.Frequencys", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.FeePlans", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeePlans
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeePlans", "AddedDate");
    //        RenameColumn("dbo.FeePlans", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.FeeHeadingGroups", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeeHeadingGroups
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeeHeadingGroups", "AddedDate");
    //        RenameColumn("dbo.FeeHeadingGroups", "AddedDateTmp", "AddedDate");





    //        AddColumn("dbo.StudentRemoteAccesses", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentRemoteAccesses
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentRemoteAccesses", "AddedDate");
    //        RenameColumn("dbo.StudentRemoteAccesses", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.PastSchoolingReports", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.PastSchoolingReports
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.PastSchoolingReports", "AddedDate");
    //        RenameColumn("dbo.PastSchoolingReports", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.GuardianDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.GuardianDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.GuardianDetails", "AddedDate");
    //        RenameColumn("dbo.GuardianDetails", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.FamilyDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FamilyDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FamilyDetails", "AddedDate");
    //        RenameColumn("dbo.FamilyDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Students", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Students
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Students", "AddedDate");
    //        RenameColumn("dbo.Students", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.AdditionalInformations", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.AdditionalInformations
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.AdditionalInformations", "AddedDate");
    //        RenameColumn("dbo.AdditionalInformations", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.FeeHeadings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeeHeadings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeeHeadings", "AddedDate");
    //        RenameColumn("dbo.FeeHeadings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Accounts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Accounts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Accounts", "AddedDate");
    //        RenameColumn("dbo.Accounts", "AddedDateTmp", "AddedDate");

    //    }

    //    public override void Down()
    //    {
    //        AddColumn("dbo.TransportFeePlans", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TransportFeePlans
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TransportFeePlans", "AddedDate");
    //        RenameColumn("dbo.TransportFeePlans", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.TransportFeeHeadings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TransportFeeHeadings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TransportFeeHeadings", "AddedDate");
    //        RenameColumn("dbo.TransportFeeHeadings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TcFeeDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TcFeeDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TcFeeDetails", "AddedDate");
    //        RenameColumn("dbo.TcFeeDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblTransportFeeReceipts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblTransportFeeReceipts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblTransportFeeReceipts", "AddedDate");
    //        RenameColumn("dbo.TblTransportFeeReceipts", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblStudentFeeSaveds", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblStudentFeeSaveds
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblStudentFeeSaveds", "AddedDate");
    //        RenameColumn("dbo.TblStudentFeeSaveds", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblLateFees", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblLateFees
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblLateFees", "AddedDate");
    //        RenameColumn("dbo.TblLateFees", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.TblFeeReceipts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblFeeReceipts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblFeeReceipts", "AddedDate");
    //        RenameColumn("dbo.TblFeeReceipts", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblEmailArchieves", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblEmailArchieves
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblEmailArchieves", "AddedDate");
    //        RenameColumn("dbo.TblEmailArchieves", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.TblDueFees", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.TblDueFees
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.TblDueFees", "AddedDate");
    //        RenameColumn("dbo.TblDueFees", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Tbl_SubmenuName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_SubmenuName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_SubmenuName", "AddedDate");
    //        RenameColumn("dbo.Tbl_SubmenuName", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_StudentPromote", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_StudentPromote
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_StudentPromote", "AddedDate");
    //        RenameColumn("dbo.Tbl_StudentPromote", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_StaffAttendance", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_StaffAttendance
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_StaffAttendance", "AddedDate");
    //        RenameColumn("dbo.Tbl_StaffAttendance", "AddedDateTmp", "AddedDate");




    //        AddColumn("dbo.Tbl_Siblings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Siblings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Siblings", "AddedDate");
    //        RenameColumn("dbo.Tbl_Siblings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Tbl_SchoolSetup", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_SchoolSetup
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_SchoolSetup", "AddedDate");
    //        RenameColumn("dbo.Tbl_SchoolSetup", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_RolePermissionNew", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_RolePermissionNew
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_RolePermissionNew", "AddedDate");
    //        RenameColumn("dbo.Tbl_RolePermissionNew", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_MerchantName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_MerchantName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_MerchantName", "AddedDate");
    //        RenameColumn("dbo.Tbl_MerchantName", "AddedDateTmp", "AddedDate");




    //        AddColumn("dbo.Tbl_MenuName", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_MenuName
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_MenuName", "AddedDate");
    //        RenameColumn("dbo.Tbl_MenuName", "AddedDateTmp", "AddedDate");




           




    //        AddColumn("dbo.Tbl_Deductions", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Deductions
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Deductions", "AddedDate");
    //        RenameColumn("dbo.Tbl_Deductions", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_CreateMerchantId", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_CreateMerchantId
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_CreateMerchantId", "AddedDate");
    //        RenameColumn("dbo.Tbl_CreateMerchantId", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.Tbl_Arrear", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Tbl_Arrear
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Tbl_Arrear", "AddedDate");
    //        RenameColumn("dbo.Tbl_Arrear", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.StudentRegistrationHistories", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentRegistrationHistories
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentRegistrationHistories", "AddedDate");
    //        RenameColumn("dbo.StudentRegistrationHistories", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.StudentsRegistrations", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentsRegistrations
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentsRegistrations", "AddedDate");
    //        RenameColumn("dbo.StudentsRegistrations", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.StafsDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StafsDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StafsDetails", "AddedDate");
    //        RenameColumn("dbo.StafsDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Frequencys", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Frequencys
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Frequencys", "AddedDate");
    //        RenameColumn("dbo.Frequencys", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.FeePlans", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeePlans
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeePlans", "AddedDate");
    //        RenameColumn("dbo.FeePlans", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.FeeHeadingGroups", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeeHeadingGroups
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeeHeadingGroups", "AddedDate");
    //        RenameColumn("dbo.FeeHeadingGroups", "AddedDateTmp", "AddedDate");





    //        AddColumn("dbo.StudentRemoteAccesses", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.StudentRemoteAccesses
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.StudentRemoteAccesses", "AddedDate");
    //        RenameColumn("dbo.StudentRemoteAccesses", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.PastSchoolingReports", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.PastSchoolingReports
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.PastSchoolingReports", "AddedDate");
    //        RenameColumn("dbo.PastSchoolingReports", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.GuardianDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.GuardianDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.GuardianDetails", "AddedDate");
    //        RenameColumn("dbo.GuardianDetails", "AddedDateTmp", "AddedDate");



    //        AddColumn("dbo.FamilyDetails", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FamilyDetails
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FamilyDetails", "AddedDate");
    //        RenameColumn("dbo.FamilyDetails", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Students", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Students
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Students", "AddedDate");
    //        RenameColumn("dbo.Students", "AddedDateTmp", "AddedDate");

    //        AddColumn("dbo.AdditionalInformations", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.AdditionalInformations
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.AdditionalInformations", "AddedDate");
    //        RenameColumn("dbo.AdditionalInformations", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.FeeHeadings", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.FeeHeadings
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.FeeHeadings", "AddedDate");
    //        RenameColumn("dbo.FeeHeadings", "AddedDateTmp", "AddedDate");


    //        AddColumn("dbo.Accounts", "AddedDateTmp", c => c.DateTime(nullable: false));
    //        Sql(@"
    //UPDATE dbo.Accounts
    //SET AddedDateTmp = AddedDate
       
    //");
    //        DropColumn("dbo.Accounts", "AddedDate");
    //        RenameColumn("dbo.Accounts", "AddedDateTmp", "AddedDate");


            //AlterColumn("dbo.TransportFeePlans", "AddedDate", c => c.String());
            //AlterColumn("dbo.TransportFeeHeadings", "AddedDate", c => c.String());
            //AlterColumn("dbo.TcFeeDetails", "PaidDate", c => c.String());
            //AlterColumn("dbo.TblTransportFeeReceipts", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblStudentFeeSaveds", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblLateFees", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblFeeReceipts", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblEmailArchieves", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblDueFees", "AddedDate", c => c.String());
            //AlterColumn("dbo.TblCreateSchools", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_SubmenuName", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_StudentPromote", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_StaffAttendance", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_Siblings", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_SchoolSetup", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_RolePermissionNew", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_MerchantName", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_MenuName", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_Deductions", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_CreateMerchantId", "AddedDate", c => c.String());
            //AlterColumn("dbo.Tbl_Arrear", "AddedDate", c => c.String());
            //AlterColumn("dbo.StudentRegistrationHistories", "AddedDate", c => c.String());
            //AlterColumn("dbo.StudentsRegistrations", "AddedDate", c => c.String());
            //AlterColumn("dbo.StafsDetails", "AddedDate", c => c.String());
            //AlterColumn("dbo.Frequencys", "AddedDate", c => c.String());
            //AlterColumn("dbo.FeePlans", "AddedDate", c => c.String());
            //AlterColumn("dbo.FeeHeadingGroups", "AddedDate", c => c.String());
            //AlterColumn("dbo.StudentRemoteAccesses", "AddedDate", c => c.String());
            //AlterColumn("dbo.PastSchoolingReports", "AddedDate", c => c.String());
            //AlterColumn("dbo.GuardianDetails", "AddedDate", c => c.String());
            //AlterColumn("dbo.FamilyDetails", "AddedDate", c => c.String());
            //AlterColumn("dbo.Students", "AddedDate", c => c.String());
            //AlterColumn("dbo.AdditionalInformations", "AddedDate", c => c.String());
            //AlterColumn("dbo.FeeHeadings", "AddedDate", c => c.String());
            //AlterColumn("dbo.Accounts", "AddedDate", c => c.String());
        }
    }
}
