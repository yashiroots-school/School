$(document).ready(function () {
   
    var weburl = "";
    if (!weburl) weburl = window.location.href;
    var array = weburl.split('=');
    var no = array[array.length - 1];
    LoadStudentDetails(no);
    GetAcademicDetailsView(no);   
    LoadInternshipDetails(no);
    LoadDeclarationDetails(no);
    GetSemmisterDetailsView(no);
    //GetSkillsView(no);
});

function LoadStudentDetails(no) {
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getStudentDetailsByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
            
            if (data.length>0) {
                $("#viewstudent").append("<tr><td><b>ScholarNumber</b><td>" + data[0].ScholarNumber + "</td><td><b>Student Name</b></td><td>" + data[0].StudentName + "</td></tr>"
                    + "<tr><td><b>Course</b><td>" + data[0].Course + "</td><td><b>Year</b></td><td>" + data[0].Years + "</td></tr>"
                    + "<tr><td><b>Batch</b><td>" + data[0].Batch + "</td><td><b>Specialization</b></td><td>" + data[0].Specialization + "</td></tr>"
                    + "<tr><td><b>Category</b></td><td>" + data[0].Category + "</td></tr>"
                    + "<tr><td><b>Faculty Mentor</b><td>" + data[0].FacultyMentor + "</td><td><b>Date of Birth</b></td><td>" + data[0].DateofBirth + "</td></tr>"
                    + "<tr><td><b>Age</b><td>" + data[0].Age + "</td><td><b>Gender</b></td><td>" + data[0].Gender + "</td></tr>"
                    + "<tr><td><b>Residence Location in Mumbai</b><td>" + data[0].ResidenceLocation + "</td><td><b>Mobile Number</b></td><td>" + data[0].MobileNo + "</td></tr>"
                    + "<tr><td><b>Email ID</b><td>" + data[0].EmailId + "</td><td><b>OutStation Student</b></td><td>" + data[0].OutStationStudent + "</td></tr>"
                    + "<tr><td><b>Native Place</b><td>" + data[0].NativePlace + "</td><td><b>Hostalite</b></td><td>" + data[0].Hostalite + "</td></tr>"
                    + "<tr><td><b>Father Name</b><td>" + data[0].FatherName + "</td><td><b>Mother Name</b></td><td>" + data[0].MotherName + "</td></tr>"
                    + "<tr><td><b>Father Profession</b><td>" + data[0].FatherProfession + "</td><td><b>Mother Profession</b></td><td>" + data[0].MotherProfession + "</td></tr>"
                    + "<tr><td><b>Father Mobile Number</b><td>" + data[0].FatherMobileNo + "</td><td><b>Mother Mobile Number</b></td><td>" + data[0].MotherMobileNo + "</td></tr>"
                    + "<tr><td><b>Father Email ID</b><td>" + data[0].FatherEmailId + "</td><td><b>Mother Email ID</b></td><td>" + data[0].MotherEmailId + "</td></tr>"
                    + "<tr><td><b>Father Company Name</b><td>" + data[0].FatherCompanyName + "</td><td><b>Mother Company Name</b></td><td>" + data[0].MotherCompanyName + "</td></tr>");
            }
            
            if (data[0].Sibiling1 != null) {
                $("#sibilings").append("<tr><td><b>Sibiling1</b><td>" + data[0].Sibiling1 + "</td></tr>");
            } if (data[0].Sibiling2 != null) {
                $("#sibilings").append("<tr><td><b>Sibiling2</b><td>" + data[0].Sibiling2 + "</td></tr>");
            }
            if ( data[0].Sibiling3 != null) {
                $("#sibilings").append("<tr><td><b>Sibiling3</b><td>" + data[0].Sibiling3 + "</td></tr>");
            } 
            if (data[0].Sibiling4 != null) {
                $("#sibilings").append("<tr><td><b>Sibiling4</b><td>" + data[0].Sibiling4 + "</td></tr>");
            } 
            if (data[0].Sibiling5 != null) {
                $("#sibilings").append("<tr><td><b>Sibiling5</b><td>" + data[0].Sibiling5 + "</td></tr>");
            } 

           
            $('#ddlStdReg_Authorized').innerHTML = data[0].Spare1;
            $('#txtdateforoffice').innerHTML = data[0].Spare2;

            if (data[0].Spare3 == "Confirm") {
                $("#checkselected").attr('checked', 'checked');
                $("#checknotselected").attr("disabled", "disabled");
                $("#checkselected").attr("disabled", "disabled");
                $("#agreechkbox").click();
                $("#agreechkbox").attr("disabled", "disabled");
                $("#stdntstatusupdate").css("display", "none");
                
            }
            else {
                $("#checknotselected").attr('checked', 'checked');
            }
          
            if (data[0].Years != undefined && data[0].Years == "I") {
                $("#summerinterndiv").hide();
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get All AcademicDetail By Id
function GetAcademicDetailsView(no) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getAllAcademicDetailsByRegNo?RegNo=" + no,
        //data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
           
            if (data.length>0) {
                for (var i = 0; i < data.length; i++) {
                    $("#Academic").append("<tr><td hidden='hidden'>" + data[i].AcademicDetailId + "</td><td>" + data[i].AcademicYear + "</td><td>" + data[i].Qualification
                        + "</td><td>" + data[i].Stream + "</td><td>" + data[i].Institution + "</td><td>" + data[i].University + "</td><td>" + data[i].Percentage + "</td></tr>");
                    //+ '<a href="#" onclick="return GetAcademicDetailsByID(' + data[i].AcademicDetailId + ')">Edit</a> | <a href="#" onclick="Delele(' + data[i].AcademicDetailId + ')">Delele</a></td><tr>');
                }
            }
        },
        error: function (result) {
           
            alert("Error");
        }
    });
}

function GetSkillsView(no) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/GetAllSkillsetByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
           
            if (data.length>0) {
                for (var i = 0; i < data.length; i++) {

                    if (data[i].Problemsolving == "Yes") {
                        $("#mgtskills").append("<tr><td>Problem solving</td><tr>");
                    }
                    if (data[i].Initiative == "Yes") {
                        $("#mgtskills").append("<tr><td>Initiative</td><tr>");
                    }
                    if (data[i].Adaptabilitytochange == "Yes") {
                        $("#mgtskills").append("<tr><td>Adaptabilitytochange</td><tr>");
                    }
                    if (data[i].Interpersonalskills == "Yes") {
                        $("#mgtskills").append("<tr><td>Interpersonalskills</td><tr>");
                    }
                    if (data[i].Strategicthinking == "Yes") {
                        $("#mgtskills").append("<tr><td>Strategicthinking</td><tr>");
                    }
                    if (data[i].Timemanagement == "Yes") {
                        $("#mgtskills").append("<tr><td>Time management</td><tr>");
                    }
                    if (data[i].Communication == "Yes") {
                        $("#mgtskills").append("<tr><td>Communication</td><tr>");
                    }
                    if (data[i].Leadership == "Yes") {
                        $("#mgtskills").append("<tr><td>Leadership</td><tr>");
                    }
                    if (data[i].Teamwork == "Yes") {
                        $("#mgtskills").append("<tr><td>Teamwork</td><tr>");
                    }

                    if (data[i].Dancing == "Yes") {
                        $("#additionalskill").append("<tr><td>Dancing</td><tr>");
                    }
                    if (data[i].Singing == "Yes") {
                        $("#additionalskill").append("<tr><td>Singing</td><tr>");
                    }
                    if (data[i].Compering == "Yes") {
                        $("#additionalskill").append("<tr><td>Compering</td><tr>");
                    }
                    if (data[i].Creative == "Yes") {
                        $("#additionalskill").append("<tr><td>Creative</td><tr>");
                    }
                    if (data[i].Contentwriting == "Yes") {
                        $("#additionalskill").append("<tr><td>Contentwriting</td><tr>");
                    }
                    if (data[i].CoralDraw == "Yes") {
                        $("#additionalskill").append("<tr><td>CoralDraw</td><tr>");
                    }
                    if (data[i].Photoshop == "Yes") {
                        $("#additionalskill").append("<tr><td>Photoshop</td><tr>");
                    }
                    if (data[i].Drawing == "Yes") {
                        $("#additionalskill").append("<tr><td>Drawing</td><tr>");
                    }

                }
            }
         

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get All AcademicDetail By Id
function GetSemmisterDetailsView(no) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getAllSemDetailsByRegNo?RegNo=" + no,
        dataType: "json",
        success: function (data) {
            
            for (var i = 0; i <data.length; i++) {
                $("#tblSemisterDetails").append("<tr><td hidden='hidden'>" + data[i].SemesterId + "</td><td>" + data[i].Year + "</td><td>" + data[i].Sem
                    + "</td><td>" + data[i].Persentagegrade + "</td></tr>");
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get All WorkExperience By Id
//function GetWorkExperience(no) {
//    $.ajax({
//        type: "GET",
//        contentType: "application/json; charset=utf-8",
//        url: "/Student/GetAllWorkExpDetailsByRegNo?RegNo=" + no,
//        //data: "{'RegNo' : '" + no + "'}",
//        dataType: "json",
//        success: function (data) {
           
//            var totalexp = 0;
//            for (var i = 0; i < data.length; i++) {
//                if (data[i].FromDate != undefined && data[i].FromDate != "") {
//                    totalexp += parseInt(data[0].FromDate);
//                }

//                $("#workexpern").append("<tr><td hidden='hidden'>" + data[i].WorkExperienceId + "</td><td>" + data[i].CompanyName
//                  + "</td><td>" + data[i].Designation + "</td><td>" + data[i].FromDate + "</td><td>" + data[i].CompanyProfile + "</td><tr>");
//            }

//            $("#lbltotaWorkExperince").html(totalexp);

//            //var a = data[i].FromDate;

//            //for (var i = 0; i <data.length;i++)
//            //{
//            //    $("#workexpern").append("<label>"+data[i].FromDate+"</label>");
//            //}

//        },
//        error: function (result) {
//            alert("Error");
//        }
//    });
//}

function LoadDeclarationDetails(no) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getDeclarationDetailsByRegNo?RegNo=" + no,
        //data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {

            var inter =data[0].Interesterd;
            var reason =data[0].NotInterested;
            var stdname =data[0].StudentName;

            if (inter == "No") {
                //hide declaration div.
                $("#agreementdiv").hide();
            }

            //if (data.d[0].Spare1 != undefined &&data[0].Spare1 != null &&data[0].Spare1 != "") {
            //    $("#checkselected").prop("checked", true);
            //    $("#checknotselected").prop("checked", false);

            //}
            //else {
            //    $("#checkselected").prop("checked", false);
            //    $("#checknotselected").prop("checked", true);
            //}

            $("#interestedtxt").text(inter);
            $("#reasontxt").text(reason);
            $("#stdname").text(stdname);

            var agree =data[0].Agree;
            if (agree == "Yes") {
                $("#agreechkbox").prop("checked", true);
                //document.getElementById("agreechkbox").disabled = true;
            }
            else {
                $("#agreechkbox").prop("checked", false);
            }
        },
        error: function (result) {
            alert("Error");
        }
    });
}

function LoadInternshipDetails(no) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getInternshipDetailsByRegNo?RegNo=" + no,
        //data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
            if (data.length>0) {
                $("#Internshiptbl").append("<tr><td><b>Company Name</b><td>" + data[0].CompanyName + "</td><td><b>Start Date</b></td><td>" + data[0].StartDate + "</td></tr>"
                    + "<tr><td><b>Mobile No</b><td>" + data[0].MobileNo + "</td><td><b>EndDate</b></td><td>" + data[0].EndDate + "</td></tr>"
                    + "<tr><td><b>Project Title</b><td>" + data[0].ProjectTitle + "</td><td><b>Faculty Project Guide Name</b></td><td>" + data[0].FacultyProjectGuide + "</td></tr>"
                    + "<tr><td><b>Faculty Guide Mobile No</b><td>" + data[0].FacultyGuideMobileNo + "</td><td><b>Industry Guide Name</b></td><td>" + data[0].IndustryGuideName + "</td></tr>"
                    + "<tr><td><b>Industry Guide Designation</b><td>" + data[0].IndustryGuideDesignation + "</td><td><b>Industry Guide TelNo</b></td><td>" + data[0].IndustryGuideTelNo + "</td></tr>"
                    + "<tr><td><b>Industry Guide Mobile No</b><td>" + data[0].IndustryGuideMobileNo + "</td><td><b>Industry Guide Email</b></td><td>" + data[0].IndustryGuideEmail + "</td></tr>"
                    + "<tr><td><b>Stipend in Thousands</b><td>" + data[0].StipendinThousands + "</td><td><b>Project Description</b></td><td>" + data[0].ProjectDescription + "</td></tr>"
                    + "<tr><td><b>Project Submission</b><td>" + data[0].ProjectSubmission + "</td><td><b>Reason for NoSubmission</b></td><td>" + data[0].ReasonforNoSubmission + "</td></tr>"
                    + "<tr><td><b>Pre Placement Offer Received</b><td>" + data[0].PrePlacementOfferReceived + "</td><td><b>Feedback</b></td><td>" + data[0].Feedback + "</td></tr>");
            }
        },
        error: function (result) {
            alert("Error");
        }
    });
}