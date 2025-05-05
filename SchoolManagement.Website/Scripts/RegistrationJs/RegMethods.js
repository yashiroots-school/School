$(document).ready(function () {

    //$(".page-sidebar").html("");
    //$(".page-content").css("margin-left", "0px");


    var weburl = "";
    if (!weburl) weburl = window.location.href
    var array = weburl.split('/');
    var scholarno = array[array.length - 1];
    if (scholarno != "" && scholarno.length > 0 && !scholarno.includes("AddStudent.aspx")) {
        //LoadStudentDetails(scholarno);
    }
    $("#summerinterntab").hide();
    $('#Yearddl').on('change', function () {
        var value = $("#Yearddl").val();
        if (value == "I" || value == "0") {

            $("#summerinterntab").hide();
            //$("#btnworkexp").attr("href", "#add4");
        }
        else {
            $("#summerinterntab").show();
            //$("#btnworkexp").attr("href", "#add3");
        }
    });




});

function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function ShowSiblings() {

    var counter = 0;
    var DivID = "divSibling_";
    var txtbxID = "Sibiling";
    counter = parseInt($("#siblingsVisible").val());
    if (counter <= 4) {
        counter = parseInt(counter) + 1;
        $("#siblingsVisible").val(counter);
        DivID = DivID + counter.toString();
        txtbxID = txtbxID + counter.toString();
        $("#" + DivID).show();
    }

    if (counter > 0) {
        $("#btnHideSibling").show();
    }
}

//function validateFormAgree() {
//    var reason = $("#Dec_ReasonforNo").val();
//    try {
//        if ($("#Dec_interestedinFinalPlacementNoChkbox").is(':checked')) {

//            var msg = "";

//            if (reason == "") {
//                msg += "Reason cannot be blank\r";
//                return false;
//            }
//            if (msg != "") {
//                alert("Please refer to the following error(s) :\r" + msg);
//                return false;
//            }
//            else {
//                //document.getElementById('Studentdetails_btn').click();
//                //Save Student All Details and Submit
//                SubmitStudentDetails();

//            }
//        }
//        else {
//            var msg = "";

//            if ($("#Dec_AgreeChkbox").is(":checked")) {
//                //document.getElementById('Studentdetails_btn').click();
//                SubmitStudentDetails();
//            }
//            else {
//                msg += "Please Select Agree";
//                alert("Please refer to the following error(s) :\r" + msg);
//            }

//        }
//    } catch (e) {

//    }



//}

function validatecountrycode(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = new RegExp('^[0-9a-zA-Z]+$');
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}
function validateEmail(obj) {
   
    var email = obj.value;
    if (email != "" && email != undefined) {
        var expr = /^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$/;
        //var expr = "[a-z0-9._%+-@]+@@[a-z0-9.-]+\.[a-z]{2,}$";
        var reg = new RegExp(expr);
        if (reg.test(email) == false) {
            $(obj).val("");
            alert("Please enter valid email");
            return false;
        }
    }
    //return expr.test(email);
}

function HideSiblings() {


    var counter = 0;
    var DivID = "divSibling_";
    var txtbxID = "Sibiling";
    counter = parseInt($("#siblingsVisible").val());

    tmpcounter = counter + 1;

    if (counter > 0) {
        DivID = DivID + counter.toString();
        txtbxID = txtbxID + tmpcounter.toString();

        $("#" + DivID).hide();
        $("#" + txtbxID).val("");
        counter = parseInt(counter) - 1;
        $("#siblingsVisible").val(counter);


    }

    if (counter == 0) {
        $("#btnHideSibling").hide();
    }
    if (counter == 4) {
        //if required we can hide add button
    }

}

function LoadStudentDetails(no) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getStudentDetailsByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
            
            $("#StudentId").val(data.d[0].StudentId);
            $('#Course').val(data.d[0].Course).attr("selected", "selected");
            //$("#Course").val(data.d[0].Course);
            $("#Year").val(data.d[0].Years);
            $("#Batch").val(data.d[0].Batch);
            $("#Specialization").val(data.d[0].Specialization);
            $("#ScholarNo").val(data.d[0].ScholarNo);
            $("#Category").val(data.d[0].Category);
            $("#FacultyMentor").val(data.d[0].FacultyMentor);
            $("#DOB").val(data.d[0].DateofBirth);
            $("#Age").val(data.d[0].Age);
            $("#ResidenceLocation").val(data.d[0].ResidenceLocation);
            $("#class").val(data.d[0].Class);
            $("#Mobile").val(data.d[0].MobileNo);
            $("#Email").val(data.d[0].EmailId);
            $("#Native").val(data.d[0].NativePlace);
            $("#CorrespondenceAddress").val(data.d[0].CorrespondenceAddress);


            $("#FatherName").val(data.d[0].FatherName);
            $("#MotherName").val(data.d[0].MotherName);
            $("#FathenProfession").val(data.d[0].FatherProfession);
            $("#MotherProfession").val(data.d[0].MotherProfession);
            $("#FatherMobile").val(data.d[0].FatherMobileNo);
            $("#MotherMobile").val(data.d[0].MotherMobileNo);
            $("#FatherEmail").val(data.d[0].FatherEmailId);
            $("#MotherEmail").val(data.d[0].MotherEmailId);
            $("#FatherCompany").val(data.d[0].FatherCompanyName);
            $("#MotherCompany").val(data.d[0].MotherCompanyName);
            $("#Sibiling1").val(data.d[0].Sibiling1);
            $("#Sibiling2").val(data.d[0].Sibiling2);
            $("#Sibiling3").val(data.d[0].Sibiling3);

            var outstn = data.d[0].OutStationStudent;
            if (outstn == "Yes")
                $("#OutStationYesChkbox").prop("checked", true);
            else
                $("#OutStationNoChkbox").prop("checked", true);

            ocument.getElementById('Native').disabled = 'disabled';

            var hostalite = data.d[0].Hostalite;
            if (hostalite == "Yes")
                $("#HostaliteYesChkbox").prop("checked", true);
            else
                $("#HostaliteNoChkbox").prop("checked", true);

            var gen = data.d[0].Gender;
            if (gen == "Male")
                $("#Malechkbox").prop("checked", true);
            else
                $("#FeMalechkbox").prop("checked", true);

            var scholarno = $("#ScholarNo").val();
            LoadDeclarationDetails(scholarno);
            LoadInternshipDetails(scholarno);

        },
        error: function (result) {
            alert("Error");
        }
    });
}

function LoadDeclarationDetails(no) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getDeclarationDetailsByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {

            var inter = data.d[0].Interesterd;
            var reason = data.d[0].NotInterested;
            var stdname = data.d[0].StudentName;
            var DeclarationId = data.d[0].DeclarationId;

            $("#interestedtxt").text(inter);
            $("#Dec_ReasonforNo").val(reason);
            $("#Dec_StudnetName").val(stdname);
            $("#DeclarationId").val(DeclarationId);

            var agree = data.d[0].Agree;
            if (agree == "Yes")
                $("#Dec_AgreeChkbox").prop("checked", true);
            else
                $("#Dec_DisagreeChkbox").prop("checked", true);

            var Interesterd = data.d[0].Interesterd;
            if (Interesterd == "Yes")
                $("#Dec_interestedinFinalPlacementYesChkbox").prop("checked", true);
            else
                $("#Dec_interestedinFinalPlacementNoChkbox").prop("checked", true);

        },
        error: function (result) {
            alert("Error");
        }
    });
}


function LoadInternshipDetails(no) {
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getInternshipDetailsByRegNo",
        data: "{'RegNo' : '" + no + "'}",
        dataType: "json",
        success: function (data) {
            $("#SummerInternshipId").val(data.d[0].SummerInternshipId);
            $("#Intern_CompanyName").val(data.d[0].CompanyName);
            $("#Intern_StartDate").val(data.d[0].StartDate);
            $("#Intern_Mobile").val(data.d[0].MobileNo);
            $("#Intern_EndDate").val(data.d[0].EndDate);
            $("#Intern_ProjectTitle").val(data.d[0].ProjectTitle);
            $("#Intern_FacultyProjectGuide").val(data.d[0].FacultyProjectGuide);
            $("#Intern_FacultyGuideMobile").val(data.d[0].FacultyGuideMobileNo);
            $("#Intern_IndustryGuideName").val(data.d[0].IndustryGuideName);
            $("#Intern_IndustryGuideDesignation").val(data.d[0].IndustryGuideDesignation);
            $("#Intern_IndustryGuideTelNo").val(data.d[0].IndustryGuideTelNo);
            $("#Intern_IndustryGuideMobile").val(data.d[0].IndustryGuideMobileNo);
            $("#Intern_IndustryGuideEmail").val(data.d[0].IndustryGuideEmail);
            $("#Intern_Stipend").val(data.d[0].StipendinThousands);
            $("#Intern_ProjectDescription").val(data.d[0].ProjectDescription);
            $("#Intern_ReasonforNoSubmission").val(data.d[0].ReasonforNoSubmission);
            $("#Intern_Feedback").val(data.d[0].Feedback);

            var projsub = data.d[0].ProjectSubmission;
            if (projsub == "Yes")
                $("#Intern_ProjectSubmissionYesChkbox").prop("checked", true);
            else
                $("#Intern_ProjectSubmissionNoChkbox").prop("checked", true);

            var placementrec = data.d[0].PrePlacementOfferReceived;
            if (placementrec == "Yes")
                $("#Intern_Pre_PlacementOfferReceivedYesChkbox").prop("checked", true);
            else
                $("#Intern_Pre_PlacementOfferReceivedNoChkbox").prop("checked", true);


        },
        error: function (result) {
            alert("Error");
        }
    });
}

//$(function () {
//    $("[id$=DOB]").datepicker({
//        changeMonth: true,
//        changeYear: true,
//        yearRange: "-35:-18", // You can set the year range as per as your need
//        dateFormat: 'dd-M-yy',
//        maxDate: '-18y'
//    });
//});

//$(function () {
//    $("[id$=Year_Sem]").datepicker({
//        changeMonth: true,
//        changeYear: true,
//        yearRange: "-5:+0", // You can set the year range as per as your need
//        dateFormat: 'yy',
//        maxDate: '+0',
//        changeMonth: false,
//        changeYear: true,
//        hideYear:true

//    });
//});
//$(function () {
//    $("[id$=Aca_Year]").datepicker({
//        changeMonth: true,
//        changeYear: true,
//        yearRange: "-37:-3", // You can set the year range as per as your need
//        dateFormat: 'yy',
//        maxDate: '+0',
//        changeMonth: false,
//        changeYear: true,
//        hideYear: true

//    });
//});
function onlyAlphabets(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123))
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}

function onlyAlphabetswithSpace(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if ((charCode > 64 && charCode < 91) || (charCode > 96 && charCode < 123) || charCode == 32)
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}


function onlyAlpha(e, t) {
    try {
        if (window.event) {
            var charCode = window.event.keyCode;
        }
        else if (e) {
            var charCode = e.which;
        }
        else { return true; }
        if (charCode > 72 && charCode < 74)
            return true;
        else
            return false;
    }
    catch (err) {
        alert(err.Description);
    }
}
//$(function () {
//    var from = $("#Intern_StartDate")
//        .datepicker({
//            dateFormat: "dd-mm-yy",
//            changeMonth: true,
//            changeYear: true,
//        })
//        .on("change", function () {
//            to.datepicker("option", "minDate", getDate(this));
//        }),
//        to = $("#Intern_EndDate").datepicker({
//            dateFormat: "dd-mm-yy",
//            changeMonth: true
//        })
//            .on("change", function () {
//                from.datepicker("option", "maxDate", getDate(this));
//            });

//    function getDate(element) {
//        var date;
//        var dateFormat = "dd-mm-yy";
//        try {
//            date = $.datepicker.parseDate(dateFormat, element.value);
//        } catch (error) {
//            date = null;
//        }

//        return date;
//    }
//});
function validate(evt) {
    var theEvent = evt || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\./;
    if (!regex.test(key)) {
        theEvent.returnValue = false;
        if (theEvent.preventDefault) theEvent.preventDefault();
    }
}

//function validateEmail(emailField) {
//    var reg = /^([A-Za-z0-9_\-\.])+\' adaret'([A-Za-z0-9_\-\.])+\.([A-Za-z]{2,4})$/;

//    if (reg.test(emailField.value) == false) {
//        alert('Invalid Email Address');
//        return false;
//    }
//    return true;
//}

function isNumberKey(txt, evt) {

    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
            return true;
        } else {
            return false;
        }
    } else {
        if (charCode > 31
            && (charCode < 48 || charCode > 57))
            return false;
    }
    return true;
}
function check() {
    var radios = document.getElementsByName("agrrement");

    for (var i = 0, len = radios.length; i < len; i++) {
        if (radios[i].checked) {
            return true;
        }
    }

    return false;
}

$(function () {
    $("input[type=radio][id*=OutStationNoChkbox]").click(function () {
        if (this.checked) {
            $(this).closest("tr").find("input[type=text][id*=Native]").attr("disabled", true);
            $(this).closest("tr").find("input[type=text][id*=Native]").val("NA");
            $(this).closest("tr").find("input[type=text][id*=Native]").focus();
        } else {

            $(this).closest("tr").find("input[type=text][id*=Native]").attr("disabled", false);
            $(this).closest("tr").find("input[type=text][id*=Native]").val("");
            $(this).closest("tr").find("input[type=text][id*=Native]").focus();
        }
    });
});

function setScholarNo() {

    // var scholarno = "SF" + $("#Courseddl").val() + $("#Batchddl").val().substr(2, 2) + "" + $("#Batchddl").val().substr(7, 2);
    var scholarno = "SF" + $("#Courseddl").val() + $("#Batchddl").val().substr(2, 2) + $("#Batchddl").val().substr(5, 2) + "-" + $("#rollno").val();
    $("#ScholarNo").val(scholarno);
}

function CalculateAge() {
    var dateString = $("#DOB").val();
    var today = new Date();
    var birthDate = new Date(dateString);
    var age = today.getFullYear() - birthDate.getFullYear();
    var m = today.getMonth() - birthDate.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    $("#Age").val(age);
}

//function validateForm() {
//    var sn = $("#ScholarNo").val();
//    var mobile = $("#Mobile").val();
//    var course = $("#Courseddl option:selected").val();
//    var specialization = $("#Specializationddl option:selected").val();
//    var userClass = $("#class option:selected").val();
//    var batch = $("#Batchddl option:selected").val();
//    var year = $("#Yearddl option:selected").val();
//    var name = $("#StudentName").val();
//    var category = $("#Categoryddl option:selected").val();
//    var facultyMentor = $("#FacultyMentorddl option:selected").val();
//    var DOB = $("#DOB").val();
//    var genderM = $("#Malechkbox").val();
//    var genderF = $("#FeMalechkbox").val();
//    var email = $("#Email").val();
//    var Address = $("#CorrespondenceAddress").val();
//    //var rollno = $("#rollno").val();
//    var msg = "";
//    var sname = $("#SName").val();
//    var nationality = $("#Nationality").val();
//    var sDOB = $("#DOB").val();
//    var parentemail = $("#parentemail").val();



//    if (sname == "") {
//        msg += "Name Cannot be Blank\r";
//    }
//    if (nationality == "") {
//        msg += "Nationality Cannot be blank\r";
//    }
//    if (sDOB == "") {
//        msg += "DOB Cannot be blank\r";
//    }

//    if (parentemail == "") {
//        msg += "Email Cannot be blank\r";
//    }

//    if (course == 0) {
//        msg += "Select Course\r";
//    }
//    if (userClass == 0) {
//        msg += "Select Class\r";
//    }
//    if (year == 0) {
//        msg += "Select Year\r";
//    }
//    if (batch == 0) {
//        msg += "Select Batch\r";
//    }

//    //if (rollno == undefined || rollno == "") {
//    //    msg += "Roll Number cannot be blank\r";
//    //}

//    if (sn == "") {
//        msg += "Invalid Scholar Number\r";
//    }
//    if (specialization == 0) {
//        msg += "Select Specialization\r";
//    }
//    if (category == 0) {
//        msg += "Select Category\r";
//    }
//    //if (facultyMentor == 0) {
//    //    msg += "Select Faculty Mentor\r";
//    //}
//    if (name == "") {
//        msg += "Student Name cannot be blank\r";
//    }
//    if (DOB == "") {
//        msg += "Invalid DOB\r";
//    }
//    if (Address == "") {
//        msg += "Correspondence Address cannot be blank\r";
//    }
//    if (mobile == "") {
//        msg += "Mobile Number cannot be blank\r";
//    }
//    if (email == "") {
//        msg += "Professional Email Id cannot be blank\r";
//    }

//    if (msg != "") {
//        alert("Please refer to the following error(s) :\r" + msg);
//        return false;
//    }
//    else { return true };

//}


//function validatestudentform() {
//    var msg = "";
//    var sname = $("#SName").val();
//    var nationality = $("#Nationality").val();
//    var sDOB = $("#DOB").val();
//    var parentemail = $("#parentemail").val();


//    if (sname == "") {
//        msg += "Name Cannot be Blank\r";
//    }
//    if (nationality == "") {
//        msg += "Nationality Cannot be blank\r";
//    }
//    if (sDOB == "") {
//        msg += "DOB Cannot be blank\r";
//    }

//    if (parentemail == "") {
//        msg += "Email Cannot be blank\r";
//    }

//    if (msg != "") {
//        alert("Please refer to the following error(s) :\r" + msg);
//        return false;
//    } else {
//        return true
//    };

//}

//$(".outsidestudentlink").on("click", function){
//    validatestudentform();
//}

$(document).ready(function () {
    $(".outsidelink").on("click", function () {
        if (validateForm()) {
            var RegstrNo = $("#ScholarNo").val();
            var request =
            {
                RegNo: $("#ScholarNo").val(),
                StudentId: 0
            };
            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: "/Student/ScholarNoExists",
                data: JSON.stringify(request),
                dataType: "json",
                success: function (data) {
                    if (data.d == true) {
                        alert("Scholar Number already exists");
                    }
                    else {
                        resetliactive();
                        $("#listacddetails").addClass("active");
                        hidealltabs();
                        //show academic details
                        $("#add1").addClass("in active");
                    }

                },
                error: function (result) {
                    alert("Error");
                }
            });

        }

    });
    $(".outsidelink_1").click(function () {
        var Acddata = $('#Academictbl tbody').children().length;

        if (Acddata > 0) {
            resetliactive();
            $("#listacdworkexp").addClass("active");
            hidealltabs();
            $("#add2").addClass("in active");
        }
        //else {
        //    alert("Please enter Academic details to proceed further.");
        //    return false;
        //}

    });

    $(".outsidelink_2").click(function () {

        var YearSelected = $("#Yearddl").val();
        resetliactive();
        hidealltabs();
        if (YearSelected == "I") {
            $("#listskillst").addClass("active");
            $("#add4").addClass("in active");
        }
        else {

            $("#listsummerintrn").addClass("active");

            $("#add3").addClass("in active");
        }
    });

    $(".outsidelink_3").click(function () {
        resetliactive();
        $("#listskillst").addClass("active");
        hidealltabs();
        $("#add4").addClass("in active");

    });

    $(".outsidelink_4").click(function () {
        resetliactive();
        $("#listfmlydetails").addClass("active");
        hidealltabs();
        $("#add5").addClass("in active");
    });

    $(".outsidelink_5").click(function () {
        resetliactive();
        $("#listdeclrtn").addClass("active");
        hidealltabs();
        $("#add6").addClass("in active");
    });

    $(".outsidelink_6back").click(function () {
        resetliactive();
        $("#listfmlydetails").addClass("active");
        hidealltabs();
        $("#add5").addClass("in active");
    });

    $(".outsidelink_5back").click(function () {
        resetliactive();
        $("#listskillst").addClass("active");
        hidealltabs();
        $("#add4").addClass("in active");

    });

    $(".outsidelink_4back").click(function () {
        var YearSelected = $("#Yearddl").val();


        resetliactive();
        hidealltabs();
        if (YearSelected == "I") {
            $("#listacdworkexp").addClass("active");

            $("#add2").addClass("in active");
        }
        else {

            $("#listsummerintrn").addClass("active");
            $("#add3").addClass("in active");
        }

    });

    $(".outsidelink_3back").click(function () {

        resetliactive();
        $("#listacdworkexp").addClass("active");
        hidealltabs();
        $("#add2").addClass("in active");
    });

    $(".outsidelink_2back").click(function () {
        resetliactive();
        $("#listacddetails").addClass("active");
        hidealltabs();
        //show academic details
        $("#add1").addClass("in active");
    });

    $(".outsidelink_1back").click(function () {
        //personal details
        resetliactive();
        $("#listddetails").addClass("active");
        hidealltabs();
        //show academic details
        $("#add").addClass("in active");
    });


    


});

