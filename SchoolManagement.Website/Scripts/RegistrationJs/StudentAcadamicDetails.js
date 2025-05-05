$(document).ready(function () {
    var weburl = "";
    if (!weburl) weburl = window.location.href
    var array = weburl.split('/');
    var no = array[array.length - 1];

    //if(no != "") {
    //    GetAcademicDetailsByRegNo(no);
    //}

});

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
                    + "</td><td>" + data[i].Stream + "</td><td>" + data[i].Institution + "</td><td>" + data[i].University + "</td><td>" + data[i].Percentage.toFixed(2) + "</td><td>"
                    + '<a href="#" onclick="return GetAcademicDetailsByID(' + data[i].AcademicDetailId + ')">Edit</a> | <a href="#" onclick="Delele(' + data[i].AcademicDetailId + ')">Delete</a></td><tr>');
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get Academic details by AcademicId
function GetAcademicDetailsByID(AcademicId) {
    
    $.ajax({
        url: "/Student/getAcademicDetailsById",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: "{id:'" + AcademicId + "'}",
        success: function (result) {
            
            $('#AcademicDetailId').val(result.d[0].AcademicDetailId);
            $('#Aca_Year').val(result.d[0].AcademicYear);
            //$('#Aca_Qualification').val(result.d[0].Qualification);
            $('#ddlAca_Qualification').val(result.d[0].Qualification);
            $('#Aca_College').val(result.d[0].Institution);
            $('#Aca_University').val(result.d[0].University);
            $('#Aca_Percentage').val(result.d[0].Percentage);
            $('#ScholarNo').val(result.d[0].ScholarNumber);
            $('#Stream').val(result.d[0].Stream);

            $("#AcademicAdd").prop("disabled", true);
            //$('#myModal').modal('show');
            //$('#btnUpdate').show();
            //$('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

// Add AcademicDetail

$(function () {
    
    $('#AcademicAdd').click(function () {

        

        var RegNo = $('#ScholarNo').val();
        //var per = $('#Aca_Percentage').val();
        //var a = "";

        //if (per > 35) {
        //    a = $('#Aca_Percentage').val();
        //}
        var msg = "";

        var _AcademicYear = $('#Aca_Year').val();
        var _Qualification = $('#ddlAca_Qualification').val();
        var _Institution = $('#Aca_College').val();
        var _University = $('#Aca_University').val();
        var _Percentage = $('#Aca_Percentage').val();
        var _ScholarNumber = $('#ScholarNo').val();
        var _Stream = $('#Stream').val();



        if (_AcademicYear == "") {
            msg += "Year cannot be blank\n";
        }

        if (_Qualification == "0") {

            msg += "Select Qualification\n";
        }

        if (_Institution == "") {
            msg += "School/college cannot be blank\n";

        }

        if (_University == "") {
            msg += "University cannot be blank\n";
        }

        if (_Percentage == "") {
            msg += "Percentage cannot be blank\n";
        }

        //if (_Stream == "") {
        //    msg += "Stream cannot be blank\n";
        //}


        if (msg != "") {

            alert("Please fill required fields :\n" + msg);
        }
        else {

            if (parseInt(_Percentage) < 35 || parseFloat(_Percentage) > 100) {

                alert("Percentage should be between 35-100");

            }
            else {


                var general =
                {
                    AcademicYear: _AcademicYear,
                    Qualification: _Qualification,
                    Institution: _Institution,
                    University: _University,
                    Percentage: _Percentage,
                    ScholarNumber: _ScholarNumber,
                    Stream: _Stream
                };

                $.ajax({
                    type: "POST",
                    url: "/Student/AddAcademic",
                    data: JSON.stringify({ 'AcademicDetail': general }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        ClearAcademicDetails();
                        GetAcademicDetailsByRegNo(RegNo);

                    }
                });
            }
        }
    });
});


// Update AcademicDetail
$(function () {
    $('#AcademicUpdate').click(function () {

        
        var RegNo = $('#ScholarNo').val();

        var msg = "";
        var _AcademicYear = $('#Aca_Year').val();
        var _Qualification = $('#ddlAca_Qualification').val();
        var _Institution = $('#Aca_College').val();
        var _University = $('#Aca_University').val();
        var _Percentage = $('#Aca_Percentage').val();

        var _Stream = $('#Stream').val();


        if (_AcademicYear == "") {
            msg += "Year cannot be blank\n";
        }

        if (_Qualification == "0") {

            msg += "Select Qualification\n";
        }

        if (_Institution == "") {
            msg += "School/college cannot be blank\n";

        }

        if (_University == "") {
            msg += "University cannot be blank\n";
        }

        if (_Percentage == "") {
            msg += "Percentage cannot be blank\n";
        }

        //if (_Stream == "") {
        //    msg += "Stream cannot be blank\n";
        //}

        if (msg != "") {

            alert("Please fill required fields :\n" + msg);
        }
        else {
            if (parseInt(_Percentage) < 35 || parseFloat(_Percentage) > 100) {

                alert("Percentage should be between 35-100");
            }
            else {

                var general =
                {
                    AcademicDetailId: $('#AcademicDetailId').val(),
                    AcademicYear: $('#Aca_Year').val(),
                    Qualification: $('#ddlAca_Qualification').val(),
                    Institution: $('#Aca_College').val(),
                    University: $('#Aca_University').val(),
                    Percentage: $('#Aca_Percentage').val(),
                    //ScholarNumber: $('#RegisterNo').val(),
                    ScholarNumber: RegNo,

                    Stream: $('#txtacadamicQualification').val()
                };

                $.ajax({
                    type: "POST",
                    url: "/Student/UpdateAcademic",
                    data: JSON.stringify({ 'AcademicDetail': general }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        ClearAcademicDetails();
                        GetAcademicDetailsByRegNo(RegNo);
                        $("#AcademicAdd").prop("disabled", false);
                    }
                });
            }
        }
    });
});


//Delete AcademicDetail
function Delele(ID) {
    var RegNo = $('#ScholarNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Student/DeleteAcademicDetail",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: "{id:'" + ID + "'}",
            success: function (result) {
                $(this).closest('tr').remove()
                GetAcademicDetailsByRegNo(RegNo);

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}



function ClearAcademicDetails() {
    $('#AcademicDetailId').val("");
    $('#Aca_Year').val("");
    $('#ddlAca_Qualification').val("");
    $('#Aca_College').val("");
    $('#Aca_University').val("");
    $('#Aca_Percentage').val("");
    $('#txtacadamicQualification').val("");
    //$('#RegisterNo').val("");
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
            alert("Error");
        }
    });
}

//Get Academic details by AcademicId
function GetSemDetailsByID(AcademicId) {
    
    $.ajax({
        url: "/Student/getSemDetailsById",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: "{id:'" + AcademicId + "'}",
        success: function (result) {
            
            $('#SemesterId').val(result.d[0].SemesterId);
            $('#Year_Sem').val(result.d[0].Year);
            $('#ddlSemester').val(result.d[0].Sem);
            $('#Percentage_Sem').val(result.d[0].Persentagegrade);


            $("#AcademicAdd1").prop("disabled", true);
            //$('#myModal').modal('show');
            //$('#btnUpdate').show();
            //$('#btnAdd').hide();
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

// Add SemDetail

$(function () {
    $('#AcademicAdd1').click(function () {


        var _Persentagegrade = $('#Percentage_Sem').val();
        //var a = "";
        //if (per > 35) {
        //    a = $('#Percentage_Sem').val();
        //}

        var _ScholarNumber = $('#ScholarNo').val();

       

        var _Year = $('#Year_Sem').val();
        var _Sem = $('#ddlSemester').val();

        var msg = "";

        if (_Year == "") {
            msg += "Year cannot be blank\n";
        }

        
        if (_Sem == "0") {

            msg += "Select Semester/Trimester\n";
        }

        if (_Persentagegrade == "") {
            msg += "Percentage cannot be blank\n";

        }

        if (msg != "") {
            alert("Please fill required fields :\n" + msg);
        }
        else {


            if (parseInt(_Persentagegrade) < 35 || parseFloat(_Persentagegrade) > 100) {

                alert("Percentage should be between 35-100");

            }
            else {

                var general =
                {
                    Year: _Year,
                    Sem: _Sem,
                    Persentagegrade: _Persentagegrade,


                    ScholarNumber: _ScholarNumber
                };

                $.ajax({
                    type: "POST",
                    url: "/Student/AddSem",
                    data: JSON.stringify({ 'AcademicDetail': general }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        ClearSemesterDetails();
                        GetSemDetailsByRegNo(_ScholarNumber);

                    }
                });
            }
        }
    });
});


// Update SemDetail
$(function () {
    $('#AcademicUpdate1').click(function () {

        
        var RegNo = $('#ScholarNo').val();

        var general =
        {
            SemesterId: $('#SemesterId').val(),
            Year: $('#Year_Sem').val(),
            Sem: $('#ddlSemester').val(),
            Persentagegrade: $('#Percentage_Sem').val(),
            ScholarNumber: $('#ScholarNo').val()
        };

        $.ajax({
            type: "POST",
            url: "/Student/UpdateSem",
            data: JSON.stringify({ 'AcademicDetail': general }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ClearSemesterDetails();
                GetSemDetailsByRegNo(RegNo);
                $("#AcademicAdd1").prop("disabled", false);
            }
        });
    });
});


//Delete AcademicDetail
function DeleteSem(ID) {
    var RegNo = $('#ScholarNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Student/DeleteSem",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: "{id:'" + ID + "'}",
            success: function (result) {
                $(this).closest('tr').remove()
                GetSemDetailsByRegNo(RegNo);

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }
}



function ClearSemesterDetails() {
    $('#SemesterId').val("");
    $('#Year_Sem').val("");
    $('#ddlSemester').val("");
    $('#Percentage_Sem').val("");
    //$('#RegisterNo').val("");
}

// Working source

//$(function () {
//    $('#AcademicAdd').click(function () {

//        

//        var general =
//        {
//            AcademicYear: $('#Aca_Year').val(),
//            Qualification: $('#Aca_Qualification').val(),
//            Institution: $('#Aca_College').val(),
//            University: $('#Aca_University').val(),
//            Percentage: $('#Aca_Percentage').val(),
//        };

//        $.ajax({
//            type: "POST",
//            url: "AddStudent.aspx/Add",
//            data: JSON.stringify({ 'AcademicDetail': general }),
//            contentType: "application/json; charset=utf-8",
//            dataType: "json",
//            success: function (msg) {

//                //$("#messages").append(msg.d);
//            }
//        });
//    });
//});