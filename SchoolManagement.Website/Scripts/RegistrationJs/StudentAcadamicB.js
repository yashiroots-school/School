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
                    + "</td><td>" + data[i].Stream + "</td><td>" + data[i].Institution + "</td><td>" + data[i].University + "</td><td>" + data[i].Percentage + "</td><td>"
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
           
            if (result != null) {
                $('#AcademicDetailId').val(result.AcademicDetailId);
                $('#Aca_Year').val(result.AcademicYear);
                //$('#Aca_Qualification').val(result.d[0].Qualification);
                $('#ddlAca_Qualification').val(result.Qualification);
                $('#Aca_College').val(result.Institution);
                $('#Aca_University').val(result.University);
                $('#Aca_Percentage').val(result.Percentage);
                $('#ScholarNo').val(result.ScholarNumber);
                $("#AcademicAdd").prop("disabled", true);
            //$('#myModal').modal('show');
            //$('#btnUpdate').show();
            //$('#btnAdd').hide();
            }
          
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
        var per = $('#Aca_Percentage').val();
        var a = "";
        if (per > 100) {
            a = $('#Aca_Percentage').val();
            alert("cant more then 100");
            return false;
        }

        var general =
        {
            AcademicYear: $('#Aca_Year').val(),
            Qualification: $('#ddlAca_Qualification').val(),
            //Qualification: $('#Aca_Qualification').val(),
            Institution: $('#Aca_College').val(),
            University: $('#Aca_University').val(),
            Percentage: a,
            Stream: $('#Stream').val(),
            ScholarNumber: $('#ScholarNo').val()
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
    });
});


// Update AcademicDetail
$(function () {
    $('#AcademicUpdate').click(function () {
       
        var RegNo = $('#ScholarNo').val();

        var general =
        {
            AcademicDetailId: $('#AcademicDetailId').val(),
            AcademicYear: $('#Aca_Year').val(),
            Qualification: $('#ddlAca_Qualification').val(),
            Institution: $('#Aca_College').val(),
            University: $('#Aca_University').val(),
            Percentage: $('#Aca_Percentage').val(),
            ScholarNumber: $('#ScholarNo').val(),
            Stream: $('#Stream').val()
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
            },
            error: function (error) {
                alert("Some Error Occure");
            }
        });
    });
});


//Delete AcademicDetail
function Delele(ID) {
    var RegNo = $('#ScholarNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "Student.asmx/DeleteAcademicDetail",
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
    //$('#RegisterNo').val("");
}


//Get All AcademicDetail By RegNo
function GetSemDetailsByRegNo(RegstrNo) {
    var tb = document.getElementById('Academictbl1');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/getAllSemDetailsByRegNo",
        data: "{'RegNo' : '" + RegstrNo + "'}",
        dataType: "json",
        success: function (data) {
           
            for (var i = 0; i < data.length; i++) {
                $("#Academictbl1").append("<tr><td hidden='hidden'>" + data[i].SemesterId + "</td><td>" + data[i].Year + "</td><td>" + data[i].Sem
                    + "</td><td>" + data[i].Percentage + "</td><td>"
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
           
            $('#SemesterId').val(result[0].SemesterId);
            $('#Year_Sem').val(result[0].Year);
            $('#ddlSemester').val(result[0].Sem);
            $('#Percentage_Sem').val(result[0].Percentage);


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
       
        var RegNo = $('#ScholarNo').val();

        var general =
        {
            Year: $('#Year_Sem').val(),
            Sem: $('#ddlSemester').val(),
            Percentage: $('#Percentage_Sem').val(),
            ScholarNumber: $('#ScholarNo').val()
        };

        $.ajax({
            type: "POST",
            url: "/Student/AddSem",
            data: JSON.stringify({ 'AcademicDetail': general }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ClearSemesterDetails();
                GetSemDetailsByRegNo(RegNo);
            }
        });
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
            Percentage: $('#Percentage_Sem').val(),
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
    var RegNo = $('#RegisterNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "Student.asmx/DeleteSem",
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