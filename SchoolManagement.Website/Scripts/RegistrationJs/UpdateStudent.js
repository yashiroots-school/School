var isStudentUpdate = false;
var StudentNo = "";
var url = window.location.href;
if (url.includes("UpdateId")) {
    var params = new window.URLSearchParams(window.location.search);
    isStudentUpdate = true;
    StudentNo = params.get('UpdateId');
}
if (isStudentUpdate) {
    LoadStudentDetails(StudentNo);
    GetAcademicDetailsByRegNo(StudentNo);
    GetSemDetailsByRegNo(StudentNo);
    GetWorkExpDetailsByRegNo(StudentNo);
    LoadSkillSetDetails(StudentNo);
    LoadInternshipDetails(StudentNo);
    LoadDeclarationDetails(StudentNo);
    $("#btnsave").val("Update");
}
function LoadStudentDetails(no) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../Student/getStudentDetailsByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
            if (data.length > 0) {
                $("#StudentName").val(data[0].StudentName);
                $('#Courseddl option[value="' + data[0].Course +'"]').attr("selected", "selected");
                $('#Yearddl option[value="' + data[0].Years + '"]').attr("selected", "selected");
                $('#Yearddl').change();
                $('#Batchddl option[value="' + data[0].Batch +'"]').attr("selected", "selected");
                $('#class option[value="' + data[0].Class + '"]').attr("selected", "selected");
                var rolenumber = data[0].ScholarNumber.split("-");
                $("#rollno").val(rolenumber[1]);
                $('#Specializationddl option[value="' + data[0].Specialization+'"]').attr("selected", "selected");
                $('#Categoryddl option[value="' + data[0].Category+'"]').attr("selected", "selected");
                $('#FacultyMentorddl option[value="' + data[0].FacultyMentor+'"]').attr("selected", "selected");
                $("#DOB").val(data[0].DateofBirth);
                $("#Age").val(data[0].Age);
                $("#Malechkbox").val();
                $("#CorrespondenceAddress").val(data[0].CorrespondenceAddress);
                $("#ResidenceLocation").val(data[0].ResidenceLocation);
                $("#Countrycode").val(data[0].CountryCode);
                $("#Mobile").val(data[0].MobileNo);
                $("#Email").val(data[0].EmailId);
                $("#OutStationYesChkbox").val();
                $("#Native").val(data[0].NativePlace);
                $("#HostaliteYesChkbox").val();
                $("#ScholarNo").val(data[0].ScholarNumber);
                if (data[0].Gender=="Male") {
                    $("#Malechkbox").attr('checked', true);
                } else {
                    $("#FeMalechkbox").attr('checked', true);
                }
                if (data[0].Hostalite == "Yes") {
                    $("#HostaliteYesChkbox").attr('checked', true);
                } else {
                    $("#HostaliteNoChkbox").attr('checked', true);
                }
                if (data[0].OutStationStudent == "Yes") {
                    $("#OutStationYesChkbox").attr('checked', true);
                } else {
                    $("#OutStationNoChkbox").attr('checked', true);
                }
                if (data[0].Religious == "Hindu") {
                    $("#Rtb_Hindu").attr('checked', true);
                } else if (data[0].Religious == "Muslim") {
                    $("#Rtb_Muslim").attr('checked', true);
                } else if (data[0].Religious == "Christine") {
                    $("#Rtb_Christine").attr('checked', true);
                } else if (data[0].Religious == "Other") {
                    $("#Rtb_Other").attr('checked', true);
                    $("#OtherReligious").css('display', 'block');
                    $("#OtherReligious").val(data[0].ReligiousOther);
                }
                //fill family details
                $("#FatherName").val(data[0].FatherName);
                $("#ddlFathenProfession").val(data[0].FatherProfession);
                $("#FatherCountrycode").val(data[0].FatherCountryCode);
                $("#FatherMobile").val(data[0].FatherMobileNo);
                $("#FatherEmail").val(data[0].FatherEmailId);
                $("#FatherCompany").val(data[0].FatherCompanyName);
                $("#MotherName").val(data[0].MotherName);
                $("#ddlMotherProfession").val(data[0].MotherProfession);
                $("#MotherCountrycode").val(data[0].MotherCountryCode);
                $("#MotherMobile").val(data[0].MotherMobileNo);
                $("#MotherEmail").val(data[0].MotherEmailId);
                $("#MotherCompany").val(data[0].MotherCompanyName);
                //$("#Sibiling1").val(data[0].Sibiling1);
            }
        },
        error: function (result) {
           // alert("Error");
        }
    });
}
//Get All AcademicDetail By RegNo
function GetAcademicDetailsByRegNo(RegstrNo) {
    var tb = document.getElementById('Academictbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/GetAllAcademicDetailsByRegNo",
        data: "{'RegNo' : '" + RegstrNo + "'}",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $("#Academictbl").append("<tr><td hidden='hidden'>" + data[i].AcademicDetailId + "</td><td>" + data[i].AcademicYear + "</td><td>" + data[i].Qualification
                    + "</td><td>" + data[i].Stream + "</td><td>" + data[i].Institution + "</td><td>" + data[i].University + "</td><td>" + data[i].Percentage + "</td><td>"
                    + '<a href="#" onclick="return GetAcademicDetailsByID(' + data[i].AcademicDetailId + ')">Edit</a> | <a href="#" onclick="Delele(' + data[i].AcademicDetailId + ')">Delete</a></td><tr>');
            }

        },
        error: function (result) {
           // alert("Error");
        }
    });
}

//Get All AcademicDetail By RegNo
function GetSemDetailsByRegNo(RegstrNo) {
    var tb = document.getElementById('Academictbl1');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getAllSemDetailsByRegNo?RegNo=" + RegstrNo,
        //data: "{'RegNo' : '" + RegstrNo + "'}",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                $("#Academictbl1").append("<tr><td hidden='hidden'>" + data[i].SemesterId + "</td><td>" + data[i].Year + "</td><td>" + data[i].Sem
                    + "</td><td>" + data[i].Persentagegrade.toFixed(2) + "</td><td>"
                    + '<a href="#" onclick="return GetSemDetailsByID(' + data[i].SemesterId + ')">Edit</a> | <a href="#" onclick="DeleteSem(' + data[i].SemesterId + ')">Delete</a></td><tr>');
            }

        },
        error: function (result) {
           // alert("Error");
        }
    });
}

function GetWorkExpDetailsByRegNo(RegstrNo) {
   
    var tb = document.getElementById('WorkExptbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/GetAllWorkExpDetailsByRegNo?RegNo=" + RegstrNo,
        dataType: "json",
        success: function (data) {
           
            for (var i = 0; i < data.length; i++) {
                $("#WorkExptbl").append("<tr><td hidden='hidden'>" + data[i].WorkExperienceId + "</td><td hidden='hidden'>" + data[i].TotalExperience + "</td><td>" + data[i].CompanyName
                    + "</td><td>" + data[i].Designation + "</td><td>" + data[i].FromDate + "</td><td>" + data[i].CompanyProfile + "</td><td>"
                    + '<a href="#" onclick="return getWorkExpDetailsById(' + data[i].WorkExperienceId + ')">Edit</a> | <a href="#" onclick="DeleleWorkExp(' + data[i].WorkExperienceId + ')">Delete</a></td><tr>');
            }
        },
        error: function (result) {
           // alert("Error");
        }
    });
}

function LoadSkillSetDetails(RegstrNo) {
   
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/LoadSkillSetDetails?RegNo=" + RegstrNo,
        dataType: "json",
        success: function (data) {
           
            if (data != null) {
                $("#SkillsetId").val(data.SkillsetId);
                if (data.Adaptability) {
                    $("#Adaptability").attr("checked", "checked");
                } else {
                    $("#Adaptability").removeAttr("checked", "checked");
                }
                if (data.Communicationskills) {
                    $("#Communicationskills").attr("checked", "checked");
                } else {
                    $("#Communicationskills").removeAttr("checked", "checked");
                }
                if (data.Compering) {
                    $("#Compering").attr("checked", "checked");
                } else {
                    $("#Compering").removeAttr("checked", "checked");
                }
                if (data.CreativeContentwriting) {
                    $("#CreativeContentwriting").attr("checked", "checked");
                } else {
                    $("#CreativeContentwriting").removeAttr("checked", "checked");
                }
                if (data.CoralDraw) {
                    $("#CoralDraw").attr("checked", "checked");
                } else {
                    $("#CoralDraw").removeAttr("checked", "checked");
                }
                if (data.Dancing) {
                    $("#Dancing").attr("checked", "checked");
                } else {
                    $("#Dancing").removeAttr("checked", "checked");
                }
                if (data.Drawing) {
                    $("#Drawing").attr("checked", "checked");
                } else {
                    $("#Drawing").removeAttr("checked", "checked");
                }
                if (data.Initiative) {
                    $("#Initiative").attr("checked", "checked");
                } else {
                    $("#Initiative").removeAttr("checked", "checked");
                }
                if (data.Interpersonalskills) {
                    $("#Interpersonalskills").attr("checked", "checked");
                } else {
                    $("#Interpersonalskills").removeAttr("checked", "checked");
                }

                if (data.Leadership) {
                    $("#Leadership").attr("checked", "checked");
                } else {
                    $("#Leadership").removeAttr("checked", "checked");
                }
                if (data.Photoshop) {
                    $("#Photoshop").attr("checked", "checked");
                } else {
                    $("#Photoshop").removeAttr("checked", "checked");
                }
                if (data.Singing) {
                    $("#Singing").attr("checked", "checked");
                } else {
                    $("#Singing").removeAttr("checked", "checked");
                }
                if (data.Strategicthinking) {
                    $("#Strategicthinking").attr("checked", "checked");
                } else {
                    $("#Strategicthinking").removeAttr("checked", "checked");
                }
                if (data.Teamwork) {
                    $("#Teamwork").attr("checked", "checked");
                } else {
                    $("#Teamwork").removeAttr("checked", "checked");
                }
                if (data.Timemanagement) {
                    $("#Timemanagement").attr("checked", "checked");
                } else {
                    $("#Timemanagement").removeAttr("checked", "checked");
                }
                if (data.Problemsolving) {
                    $("#Problemsolving").attr("checked", "checked");
                } else {
                    $("#Problemsolving").removeAttr("checked", "checked");
                }
            }
        },
        error: function (result) {
           
          //  alert("Error");
        }
    });
}

//Delete AcademicDetail
function DeleleWorkExp(ID) {
    var RegNo = $('#RegisterNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Student/DeleteWorkExp?id=" + ID,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            success: function (result) {
                $(this).closest('tr').remove();
                GetWorkExpDetailsByRegNo(RegNo);

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}

function ClearWorkExpDetails() {
    $('#WorkExperienceId').val("");
    $('#Wrk_TotalExperience').val("");
    $('#Wrk_CompanyName').val("");
    $('#Wrk_CompanyDesignation').val("");
    $('#Wrk_CompanyDuration').val("");
    $('#Wrk_CompanyProfile').val("");
    //$('#RegisterNo').val("");
}

function LoadInternshipDetails(RegNo) {

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getInternshipDetailsByRegNoFoeEdit?RegNo=" + RegNo,
        dataType: "json",
        success: function (data) {
           
            if (data != null && data != undefined) {
                $("#SummerInternshipId").val(data.SummerInternshipId);
                $("#Intern_CompanyName").val(data.CompanyName);
                $("#Intern_StartDate").val(data.StartDate);
                $("#Intern_Mobile").val(data.MobileNo);
                $("#Intern_EndDate").val(data.EndDate);
                $("#Intern_ProjectTitle").val(data.ProjectTitle);
                $("#Intern_FacultyProjectGuide").val(data.FacultyProjectGuide);
                $("#Intern_FacultyGuideMobile").val(data.FacultyGuideMobileNo);
                $("#Intern_IndustryGuideName").val(data.IndustryGuideName);
                $("#Intern_IndustryGuideDesignation").val(data.IndustryGuideDesignation);
                $("#Intern_IndustryGuideTelNo").val(data.IndustryGuideTelNo);
                $("#Intern_IndustryGuideMobile").val(data.IndustryGuideMobileNo);
                $("#Intern_IndustryGuideEmail").val(data.IndustryGuideEmail);
                $("#Intern_Stipend").val(data.StipendinThousands);
                $("#Intern_ProjectDescription").val(data.ProjectDescription);
                $("#Intern_ReasonforNoSubmission").val(data.ReasonforNoSubmission);
                $("#Intern_Feedback").val(data.Feedback);
                var projsub = data.ProjectSubmission;
                if (projsub == "Yes")
                    $("#Intern_ProjectSubmissionYesChkbox").attr("checked", true);
                else
                    $("#Intern_ProjectSubmissionNoChkbox").attr("checked", true);
                disablefieldProject();
                var placementrec = data.PrePlacementOfferReceived;
                if (placementrec == "Yes")
                    $("#Intern_Pre_PlacementOfferReceivedYesChkbox").attr("checked", true);
                else
                    $("#Intern_Pre_PlacementOfferReceivedNoChkbox").attr("checked", true);
            }
        },
        error: function (result) {
          //  alert("Error");
        }
    });
}

function disablefieldProject() {
    if (document.getElementById('Intern_ProjectSubmissionYesChkbox').checked == 1) {
        document.getElementById('Intern_ReasonforNoSubmission').disabled = 'disabled';

    } else {
        document.getElementById('Intern_ReasonforNoSubmission').disabled = '';
    }
}

function LoadDeclarationDetails(RegNo) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getDeclarationDetailsByRegNoEdit?RegNo=" + RegNo,
        dataType: "json",
        success: function (data) {
            if (data == null || data=="") {
                return false;
            }
           
            var inter = data.Interesterd;
            var reason = data.NotInterested;
            var stdname = data.StudentName;
            var DeclarationId = data.DeclarationId;
            $("#interestedtxt").text(inter);
            $("#Dec_ReasonforNo").val(reason);
            $("#Dec_StudnetName").val(stdname);
            $("#DeclarationId").val(DeclarationId);

            var agree = data.Agree;
            if (agree == "Yes") {
                $("#Dec_AgreeChkbox").prop("checked", true);
                document.getElementById("Dec_AgreeChkbox").disabled = true;
            }
            else {
                $("#Dec_DisagreeChkbox").prop("checked", true);
            }

            var Interesterd = data.Interesterd;
            if (Interesterd == "Yes") {
                $("#Dec_interestedinFinalPlacementYesChkbox").prop("checked", true);
                document.getElementById("Dec_interestedinFinalPlacementYesChkbox").disabled = true;
                document.getElementById("Dec_interestedinFinalPlacementNoChkbox").disabled = true;
                document.getElementById("Dec_ReasonforNo").disabled = true;
            }

            if (Interesterd == "No") {
                $("#Dec_interestedinFinalPlacementNoChkbox").prop("checked", true);
                document.getElementById("Dec_interestedinFinalPlacementNoChkbox").disabled = true;
                document.getElementById("Dec_interestedinFinalPlacementYesChkbox").disabled = true;
            }

            disableConfirm();

        },
        error: function (result) {
            //alert("Error");
        }
    });
}