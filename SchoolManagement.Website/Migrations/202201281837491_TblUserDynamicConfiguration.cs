namespace SchoolManagement.Website.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TblUserDynamicConfiguration : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FamilyDetails", "StudentRefId", "dbo.Students");
            DropIndex("dbo.FamilyDetails", new[] { "StudentRefId" });
            CreateTable(
                "dbo.SMSEMAILSCHEDULEs",
                c => new
                    {
                        SMSEMAILSCHEDULEID = c.Long(nullable: false, identity: true),
                        SCHEDULETYPE = c.String(),
                        CREATEDDATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.SMSEMAILSCHEDULEID);
            
            CreateTable(
                "dbo.SMSEMAILSENDHISTORies",
                c => new
                    {
                        HISTORYID = c.Long(nullable: false, identity: true),
                        SENDERID = c.Int(nullable: false),
                        SENDERTYPE = c.String(),
                        SMS = c.String(),
                        EMAIL = c.String(),
                        SUBJECT = c.String(),
                        ATTACHEDFILE = c.String(),
                        ATTACHEDFILETYPE = c.String(),
                        ATTACHEDFILENAME = c.String(),
                        CREATEDDATE = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.HISTORYID);
            
            //CreateTable(
            //    "dbo.SMSEMAILTEMPLETEs",
            //    c => new
            //        {
            //            SMSEMAILID = c.Long(nullable: false, identity: true),
            //            NOTIFICATIONTYPE = c.String(),
            //            SMS = c.String(),
            //            EMAIL = c.String(),
            //            SUBJECT = c.String(),
            //            ATTACHEDFILE = c.String(),
            //            ATTACHEDFILETYPE = c.String(),
            //            ATTACHEDFILENAME = c.String(),
            //            CREATEDDATE = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.SMSEMAILID);
            
            //CreateTable(
            //    "dbo.StudentRegNumberMasters",
            //    c => new
            //        {
            //            StudnetRegNumberMasterID = c.Int(nullable: false, identity: true),
            //            Class = c.String(maxLength: 100),
            //            BatchName = c.String(maxLength: 100),
            //            RegPrefix = c.String(maxLength: 100),
            //            RegLength = c.Int(nullable: false),
            //            RegNumberStartWith = c.Int(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //            RegStatus = c.String(),
            //            RegLastNumber = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.StudnetRegNumberMasterID);
            
            //CreateTable(
            //    "dbo.StudentsRegistrations",
            //    c => new
            //        {
            //            StudentRegisterID = c.Long(nullable: false, identity: true),
            //            ApplicationNumber = c.String(nullable: false),
            //            UIN = c.String(nullable: false),
            //            Date = c.String(),
            //            Name = c.String(nullable: false),
            //            Class = c.String(),
            //            Section = c.String(),
            //            Gender = c.String(),
            //            RTE = c.String(),
            //            Medium = c.String(),
            //            Caste = c.String(),
            //            AgeInWords = c.Int(nullable: false),
            //            DOB = c.String(),
            //            POB = c.String(),
            //            Nationality = c.String(),
            //            Religion = c.String(),
            //            MotherTongue = c.String(),
            //            Category = c.String(),
            //            BloodGroup = c.String(),
            //            MedicalHistory = c.String(),
            //            Hobbies = c.String(),
            //            Sports = c.String(),
            //            OtherDetails = c.String(),
            //            ProfileAvatar = c.String(),
            //            MarkForIdentity = c.String(),
            //            AdharNo = c.String(),
            //            AdharFile = c.String(),
            //            OtherLanguages = c.String(),
            //            IsApplyforTC = c.Boolean(nullable: false),
            //            IsApplyforAdmission = c.Boolean(nullable: false),
            //            IsApprove = c.Int(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            IsAdmissionPaid = c.Boolean(),
            //            IsInsertFromAd = c.Boolean(),
            //            Email = c.String(),
            //            LastStudiedSchoolName = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.StudentRegisterID);
            
            //CreateTable(
            //    "dbo.StudentRegistrationHistories",
            //    c => new
            //        {
            //            StudentRegisterHistoryID = c.Long(nullable: false, identity: true),
            //            StudentRegisterID = c.Long(nullable: false),
            //            ApplicationNumber = c.String(nullable: false),
            //            UIN = c.String(nullable: false),
            //            Date = c.String(),
            //            Name = c.String(nullable: false),
            //            Class = c.String(),
            //            Section = c.String(),
            //            Gender = c.String(),
            //            RTE = c.String(),
            //            Medium = c.String(),
            //            Caste = c.String(),
            //            AgeInWords = c.Int(nullable: false),
            //            DOB = c.String(),
            //            POB = c.String(),
            //            Nationality = c.String(),
            //            Religion = c.String(),
            //            MotherTongue = c.String(),
            //            Category = c.String(),
            //            BloodGroup = c.String(),
            //            MedicalHistory = c.String(),
            //            Hobbies = c.String(),
            //            Sports = c.String(),
            //            OtherDetails = c.String(),
            //            ProfileAvatar = c.String(),
            //            MarkForIdentity = c.String(),
            //            AdharNo = c.String(),
            //            AdharFile = c.String(),
            //            OtherLanguages = c.String(),
            //            IsApplyforTC = c.Boolean(nullable: false),
            //            IsApplyforAdmission = c.Boolean(nullable: false),
            //            IsApprove = c.Int(nullable: false),
            //            IsApprovePreview = c.Int(nullable: false),
            //            IsActive = c.Boolean(nullable: false),
            //            IsAdmissionPaid = c.Boolean(),
            //            IsInsertFromAd = c.Boolean(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.StudentRegisterHistoryID);
            
            //CreateTable(
            //    "dbo.tbl_PaymentTransactionDetails",
            //    c => new
            //        {
            //            PaymentTransactionId = c.Long(nullable: false, identity: true),
            //            TransactionStatus = c.String(maxLength: 1000),
            //            TransactionError = c.String(maxLength: 1000),
            //            TxnDate = c.String(maxLength: 30),
            //            Amount = c.String(maxLength: 20),
            //            TransactionId = c.String(maxLength: 100),
            //            TrackId = c.String(maxLength: 100),
            //            ReferenceNo = c.String(maxLength: 100),
            //            Pmntmode = c.String(maxLength: 100),
            //            Type = c.String(maxLength: 100),
            //            Card = c.String(maxLength: 100),
            //            CardType = c.String(maxLength: 100),
            //            Member = c.String(maxLength: 100),
            //            PaymentId = c.String(maxLength: 100),
            //            StudentId = c.Int(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PaymentTransactionId);
            
            //CreateTable(
            //    "dbo.tbl_PaymentTransactionFeeDetails",
            //    c => new
            //        {
            //            PaymentFeedetailsID = c.Long(nullable: false, identity: true),
            //            PaymentTransactionId = c.Long(nullable: false),
            //            FeeID = c.Int(nullable: false),
            //            FeeAmount = c.String(maxLength: 100),
            //            CreatedOn = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.PaymentFeedetailsID)
            //    .ForeignKey("dbo.tbl_PaymentTransactionDetails", t => t.PaymentTransactionId, cascadeDelete: true)
            //    .Index(t => t.PaymentTransactionId);
            
            //CreateTable(
            //    "dbo.TblTransportFeeReceipts",
            //    c => new
            //        {
            //            FeeReceiptId = c.Int(nullable: false, identity: true),
            //            FeeHeadingId = c.Int(nullable: false),
            //            StudentId = c.Int(nullable: false),
            //            StudentName = c.String(),
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
            //            ClassName = c.String(),
            //            CategoryName = c.String(),
            //            Concession = c.Single(nullable: false),
            //            ConcessionAmt = c.Single(nullable: false),
            //            PayHeadings = c.String(),
            //            OldBalance = c.Single(nullable: false),
            //            ReceiptAmt = c.Single(nullable: false),
            //            TotalFee = c.Single(nullable: false),
            //            LateFee = c.Single(nullable: false),
            //            BalanceAmt = c.Single(nullable: false),
            //            PaymentMode = c.String(),
            //            BankName = c.String(),
            //            CheckId = c.String(),
            //            Remark = c.String(),
            //            FeePaids = c.String(),
            //            FeeReceiptsOneTimeCreator = c.String(),
            //            DueAmount = c.String(),
            //            PaidAmount = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.FeeReceiptId);
            
            CreateTable(
                "dbo.TblUserDynamicConfigurations",
                c => new
                    {
                        Mainid = c.Int(nullable: false, identity: true),
                        ListType = c.String(),
                        ListData = c.String(),
                        CurrentUser = c.String(),
                    })
                .PrimaryKey(t => t.Mainid);
            
            //CreateTable(
            //    "dbo.TransportFeeConfigurations",
            //    c => new
            //        {
            //            TransportFeeConfigurationID = c.Int(nullable: false, identity: true),
            //            Class = c.String(maxLength: 100),
            //            BatchName = c.String(maxLength: 100),
            //            FromKM = c.Int(nullable: false),
            //            ToKM = c.Int(nullable: false),
            //            Amount = c.Int(nullable: false),
            //            CreatedOn = c.DateTime(nullable: false),
            //        })
            //    .PrimaryKey(t => t.TransportFeeConfigurationID);
            
            //CreateTable(
            //    "dbo.TransportFeeHeadings",
            //    c => new
            //        {
            //            TransportFeeId = c.Int(nullable: false, identity: true),
            //            FeeName = c.String(),
            //            FeeFrequencyId = c.Int(nullable: false),
            //            FeeFrequencyName = c.String(),
            //            Jan = c.Byte(nullable: false),
            //            Feb = c.Byte(nullable: false),
            //            Mar = c.Byte(nullable: false),
            //            Apr = c.Byte(nullable: false),
            //            May = c.Byte(nullable: false),
            //            Jun = c.Byte(nullable: false),
            //            Jul = c.Byte(nullable: false),
            //            Aug = c.Byte(nullable: false),
            //            Sep = c.Byte(nullable: false),
            //            Oct = c.Byte(nullable: false),
            //            Nov = c.Byte(nullable: false),
            //            Dec = c.Byte(nullable: false),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.TransportFeeId);
            
            //CreateTable(
            //    "dbo.TransportFeePlans",
            //    c => new
            //        {
            //            FeePlanId = c.Int(nullable: false, identity: true),
            //            FeePlanName = c.String(),
            //            ClassId = c.Int(nullable: false),
            //            ClassName = c.String(),
            //            CategoryId = c.Int(nullable: false),
            //            CategoryName = c.String(),
            //            FeeId = c.Int(nullable: false),
            //            FeeName = c.String(),
            //            FeeValue = c.Single(nullable: false),
            //            Opt1 = c.String(),
            //            Opt2 = c.String(),
            //            Opt3 = c.String(),
            //            Opt4 = c.String(),
            //            AddedDate = c.String(),
            //            ModifiedDate = c.String(),
            //            CurrentYear = c.Int(nullable: false),
            //            IP = c.String(),
            //            UserId = c.String(),
            //            IsDeleted = c.Boolean(nullable: false),
            //            CreateBy = c.Int(nullable: false),
            //            InsertBy = c.String(),
            //            BatchName = c.String(),
            //        })
            //    .PrimaryKey(t => t.FeePlanId);
            
            //AddColumn("dbo.Students", "RegNumber", c => c.String());
            //AddColumn("dbo.FamilyDetails", "ApplicationNumber", c => c.String());
            //AddColumn("dbo.FamilyDetails", "Student_StudentId", c => c.Int());
            //CreateIndex("dbo.FamilyDetails", "Student_StudentId");
            //AddForeignKey("dbo.FamilyDetails", "Student_StudentId", "dbo.Students", "StudentId");
        }
        
        public override void Down()
        {
            //DropForeignKey("dbo.FamilyDetails", "Student_StudentId", "dbo.Students");
            //DropForeignKey("dbo.tbl_PaymentTransactionFeeDetails", "PaymentTransactionId", "dbo.tbl_PaymentTransactionDetails");
            //DropIndex("dbo.tbl_PaymentTransactionFeeDetails", new[] { "PaymentTransactionId" });
            //DropIndex("dbo.FamilyDetails", new[] { "Student_StudentId" });
            //DropColumn("dbo.FamilyDetails", "Student_StudentId");
            //DropColumn("dbo.FamilyDetails", "ApplicationNumber");
            //DropColumn("dbo.Students", "RegNumber");
            //DropTable("dbo.TransportFeePlans");
            //DropTable("dbo.TransportFeeHeadings");
            //DropTable("dbo.TransportFeeConfigurations");
            DropTable("dbo.TblUserDynamicConfigurations");
            //DropTable("dbo.TblTransportFeeReceipts");
            //DropTable("dbo.tbl_PaymentTransactionFeeDetails");
            //DropTable("dbo.tbl_PaymentTransactionDetails");
            //DropTable("dbo.StudentRegistrationHistories");
            //DropTable("dbo.StudentsRegistrations");
            //DropTable("dbo.StudentRegNumberMasters");
            //DropTable("dbo.SMSEMAILTEMPLETEs");
            //DropTable("dbo.SMSEMAILSENDHISTORies");
            //DropTable("dbo.SMSEMAILSCHEDULEs");
            //CreateIndex("dbo.FamilyDetails", "StudentRefId");
            //AddForeignKey("dbo.FamilyDetails", "StudentRefId", "dbo.Students", "StudentId", cascadeDelete: true);
        }
    }
}
