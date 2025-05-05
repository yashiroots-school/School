 


$(window).load(function () {
    if (window.location.href.indexOf('UpdateId') > 0) {
         
        var baseUrl = (window.location).href; // You can also use document.URL
        var StudentId = baseUrl.substring(baseUrl.lastIndexOf('=') + 1);
        GetStudentById(StudentId);
        GetFamilyDetailStudentByIDForEdit(StudentId);
        GetGuardianDetailsStudentByIDForEdit(StudentId);
        GetAdditionalInformationsStudentByIDForEdit(StudentId);
        GetPastSchoolingReportsStudentByIDForEdit(StudentId);
        GetStudentRemoteAccessIDForEdit(StudentId);
    }
});

function GetStudentById(id) {
    $.ajax({
        url: "/Student/GetStudentByIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            console.log('Yash',result);
            //Student Tab
            
            $("#ApplicationNumber").val(result.ApplicationNumber);
            $("#UIN").val(result.UIN);
            $("#Date").val(result.Date);
            $("#FileLbl").text(result.ProfileAvatar);
            $("#SName").val(result.Name);
            $("#StudentId").val(result.StudentId);
            
            $('#Class option[value="' + result.Class + '"]').attr("selected", "selected");
            $('#Class').val(result.Class).change();

            $('#Section option[value="' + result.Section + '"]').attr("selected", "selected");
            $('#Section').val(result.Section).change();

            $('#BatchName option[value="' + result.BatchNames + '"]').attr("selected", "selected");
            $('#BatchName').val(result.BatchNames).change();
            

            if (result.Gender=="Male") {
            $("#Gender1").prop("checked", true);
            }else {
            $("#Gender2").prop("checked", true);
            }

            $("#DOB").val(result.DOB);
            $("#POB").val(result.POB);
            $("#Nationality").val(result.Nationality);

            $("#MotherTongue").val(result.MotherTongue);

            $('#Medium option[value="' + result.Medium + '"]').attr("selected", "selected");
            $('#Medium').val(result.Medium).trigger('change');

            $('#Religion option[value="' + result.Religion + '"]').attr("selected", "selected");
            $('#Religion').val(result.Religion).change();

            $('#Category option[value="' + result.Category + '"]').attr("selected", "selected");
            $('#Category').val(result.Category).change();

            $('#Caste option[value="' + result.Caste + '"]').attr("selected", "selected");
            $('#Caste').val(result.Caste).change();

            $('#BloodGroup option[value="' + result.BloodGroup + '"]').attr("selected", "selected");
            $('#AdharFile option[value="' + result.AdharFile + '"]').attr("selected", "selected");
            $('#BloodGroup').val(result.BloodGroup).change();
            
            $("#AdharNo").val(result.AdharNo);
            $("#AdharFile").val(result.AdharFile);

            $("#Hobbies").val(result.Hobbies);
            $("#Sports").val(result.Sports);

            $("#OtherLanguages").val(result.OtherLanguages);
            $("#MarkForIdentity").val(result.MarkForIdentity);
            
            $("#HeadingText").text("Update Student Details");
            $("#submitformBTn").text("Update Student");
            $('#Studentform').attr('action', '/Student/EditStudentDetail');
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

//Family
function GetFamilyDetailStudentByIDForEdit(id) {
    $.ajax({
        url: "/Student/GetFamilyDetailStudentByIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //Student Tab
            $("#FatherName").val(result.FatherName);
            $("#FQualifications").val(result.FQualifications);
            $("#FOccupation").val(result.FOccupation);
            $("#FOrganization").val(result.FOrganization);
            $("#FPhone").val(result.FPhone);
            $("#FMobile").val(result.FMobile);
            $("#FEMail").val(result.FEMail);
            $("#FAnnualIncome").val(result.FAnnualIncome);
            $("#NoOfBrothers").val(result.NoOfBrothers)
            $("#MotherName").val(result.MotherName);
            $("#MQualifications").val(result.MQualifications);
            $("#MOccupation").val(result.MOccupation);
            $("#MOrganization").val(result.MOrganization);
            $("#MPhone").val(result.MPhone);
            $("#MMobile").val(result.MMobile);
            $("#MEMail").val(result.MEMail);
            $("#MAnnualIncome").val(result.MAnnualIncome);
            $("#NoOfSisters").val(result.NoOfSisters);

            $("#FResidentialAddress").val(result.FResidentialAddress);
            $("#MPermanentAddress").val(result.MPermanentAddress);
            $("#Rvill").val(result.familydetails.Rvill);
            $("#Rpost").val(result.familydetails.Rpost);
            $("#Rdist").val(result.familydetails.Rdist);
            $("#Rstate").val(result.familydetails.Rstate);
            $("#Pvill").val(result.familydetails.Pvill);
            $("#Ppost").val(result.familydetails.Ppost);
            $("#Pdist").val(result.familydetails.Pdist);
            $("#Pstate").val(result.familydetails.Pstate);
            $("#Name").val(result.Name);
            $("#Name").val(result.Name);

            



            //$("#MariedStatus3").prop("checked", true);
            //$('#Caste option[value="' + result.Caste + '"]').attr("selected", "selected");
            //$('#Caste').val(result.Caste).change();
            //$("#headingName").text("Update Staff Details");
            //$("#btnstafsave").text("Update Staff");
            //$('#StaffForm').attr('action', '/Staf/EditStaffDetail');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}


function GetGuardianDetailsStudentByIDForEdit(id) {
    $.ajax({
        url: "/Student/GetGuardianDetailsStudentByIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            $("#GGuardianName").val(result.GuardianName);
            $("#GQualifications").val(result.Qualifications);
            $("#GOccupation").val(result.Occupation);
            $("#GOrganization").val(result.Organization);
            $("#GPhone").val(result.Phone);
            $("#GMobile").val(result.Mobile);
            $("#GEMail").val(result.EMail);
            $("#GAnnualIncome").val(result.AnnualIncome);
            $("#GResidentialAddress").val(result.ResidentialAddress);
            $("#GPermanentAddress").val(result.PermanentAddress);

            //$("#MariedStatus3").prop("checked", true);
            //$('#Caste option[value="' + result.Caste + '"]').attr("selected", "selected");
            //$('#Caste').val(result.Caste).change();
            //$("#headingName").text("Update Staff Details");
            //$("#btnstafsave").text("Update Staff");
            //$('#StaffForm').attr('action', '/Staf/EditStaffDetail');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

//Addition info

function GetAdditionalInformationsStudentByIDForEdit(id) {
    $.ajax({
        url: "/Student/GetAdditionalInformationsStudentByIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            console.log(result)
            $("#DistancefromSchool").val(result.DistancefromSchool);
            $("#TransportVehicleNo").val(result.TransportVehicleNo);

            $('#Class option[value="' + result.AssignClass + '"]').attr("selected", "selected");
            $('#Class').val(result.AssignClass).change();

            $('#Group option[value="' + result.Group + '"]').attr("selected", "selected");
            $('#Group').val(result.Group).change();

            $('#Section option[value="' + result.AssignSection + '"]').attr("selected", "selected");
            $('#IncomeCertificate option[value="' + result.IncomeCertificate + '"]').attr("selected", "selected");
            $('#Section').val(result.AssignSection).change();

            if (result.Physicallychalanged == "Yes") {
                $("#Phisically1").prop("checked", true);
            } else {
                $("#Phisically2").prop("checked", true);
            }

            if (result.grade == "Yes") {
                $("#grade1").prop("checked", true);
            } else {
                $("#grade2").prop("checked", true);
            }
            if (result.FeeStructureApplicable == "Standard Fee") {
                $("#FeeStructureApplicable1").prop("checked", true);
            } else if (result.FeeStructureApplicable == "Special Fee (20% Discount)") {
                $("#FeeStructureApplicable2").prop("checked", true);
            } else {
                $("#FeeStructureApplicable3").prop("checked", true);
            }

            $("#DistancefromSchool").val(result.DistancefromSchool);
            $("#TransportFacility").val(result.TransportFacility);
            $("#TransportVehicleNo").val(result.TransportVehicleNo);


            $("#ARemarks").val(result.Remarks);


            //$("#MariedStatus3").prop("checked", true);
            //$('#Class option[value="' + result.Caste + '"]').attr("selected", "selected");
            //$('#Class').val(result.Caste).change();
            //$("#headingName").text("Update Staff Details");
            //$("#btnstafsave").text("Update Staff");
            //$('#StaffForm').attr('action', '/Staf/EditStaffDetail');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

//Past Sscholing

function GetPastSchoolingReportsStudentByIDForEdit(id) {
    $.ajax({
        url: "/Student/GetPastSchoolingReportsStudentByIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            
            $("#NameOfSchoolLastAttended").val(result.NameOfSchoolLastAttended);
            $("#ClassPassed").val(result.ClassPassed);

            if (result.Promotion =="Pass") {
                $("#Promotion1").prop("checked", true);
            } else {
                $("#Promotion2").prop("checked", true);
            }
            $("#ReasonForLeaving").val(result.ReasonForLeaving);

            //$("#MariedStatus3").prop("checked", true);
            //$('#Class option[value="' + result.Caste + '"]').attr("selected", "selected");
            //$('#Class').val(result.Caste).change();
            //$("#headingName").text("Update Staff Details");
            //$("#btnstafsave").text("Update Staff");
            //$('#StaffForm').attr('action', '/Staf/EditStaffDetail');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

function GetStudentRemoteAccessIDForEdit(id) {
    $.ajax({
        url: "/Student/GetStudentRemoteAccessIDForEdit?studentId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {

            $("#EnterDesiredlogin").val(result.EnterDesiredlogin);
            $("#Password").val(result.Password);
             

            //$("#MariedStatus3").prop("checked", true);
            //$('#Class option[value="' + result.Caste + '"]').attr("selected", "selected");
            //$('#Class').val(result.Caste).change();
            //$("#headingName").text("Update Staff Details");
            //$("#btnstafsave").text("Update Staff");
            //$('#StaffForm').attr('action', '/Staf/EditStaffDetail');

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

$('#').click(function () {

})