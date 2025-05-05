$(document).ready(function () {
    //loaddatatable();
    GetAllStudentDetails();
});

//Get All Student Details
function GetAllStudentDetails() {
    var tb = document.getElementById('PromotionTbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var html = "";
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/GeStudentList1",
        dataType: "json",
        success: function (data) {
            
            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "<td>" + data[i].ScholarNumber + "</td>";
                html += "<td>" + data[i].StudentName + "</td>";
                html += "<td>" + data[i].Gender + "</td>";
                html += "<td>" + data[i].DateofBirth + "</td>";
                html += "<td>" + data[i].FatherName + "</td>";
                html += "<td>";
                html += '<input type="button" class="btn btn-info btn-sm promotionBtn" data-val=' + data[i].StudentId + ' data-class="' + data[i].Class + '" value="Promote To"/>';
                html += "</td>";
                html += "</tr>";
            }


            //<td hidden='hidden'>" + data.d[i].StudentId + "</td>
            $("#PromotionBody").html(html);
            loaddatatable();

        },
        error: function (result) {
            alert("Error");
        }
    });
}

function loaddatatable() {
    if ($.fn.DataTable.isDataTable('#PromotionTbl')) {
        $('#PromotionTbl').DataTable().destroy();
    }

    $('#PromotionTbl').dataTable({
        "autoWidth": true,
        "scrollX": true
        //"columnDefs": [
        //    { "orderable": false, "targets": 1 },
        //    { "orderable": false, "targets": 2 },
        //    { "orderable": false, "targets": 3 },
        //    { "orderable": false, "targets": 4 },
        //    { "orderable": false, "targets": 5 },
        //    { "orderable": false, "targets": 6 },
        //    { "orderable": false, "targets": 7 },
        //    { "orderable": false, "targets": 8 },
        //    { "orderable": false, "targets": 9 }
        //]

    });

}

//Get All AcademicDetail By RegNo
function GetWorkExpDetailsByRegNo(RegstrNo) {
    var tb = document.getElementById('WorkExptbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "Student.asmx/GetAllWorkExpDetailsByRegNo",
        data: "{'RegNo' : '" + RegstrNo + "'}",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.d.length; i++) {
                $("#WorkExptbl").append("<tr><td hidden='hidden'>" + data.d[i].WorkExperienceId + "</td><td>" + data.d[i].TotalExperience + "</td><td>" + data.d[i].CompanyName
                    + "</td><td>" + data.d[i].Designation + "</td><td>" + data.d[i].FromDate + "</td><td>" + data.d[i].CompanyProfile + "</td><tr>'");
            }

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get Academic details by AcademicId
function getWorkExpDetailsById(WorkExpId) {

    $.ajax({
        url: "Student.asmx/GetWorkExpDetailsById",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: "{id:'" + WorkExpId + "'}",
        success: function (result) {

            $('#WorkExperienceId').val(result.d[0].WorkExperienceId);
            $('#Wrk_TotalExperience').val(result.d[0].TotalExperience);
            $('#Wrk_CompanyName').val(result.d[0].CompanyName);
            $('#Wrk_CompanyDesignation').val(result.d[0].Designation);
            $('#Wrk_CompanyDuration').val(result.d[0].FromDate);
            $('#Wrk_CompanyProfile').val(result.d[0].CompanyProfile);
            $('#RegisterNo').val(result.d[0].ScholarNumber);
            $("#WorkExpAdd").prop("disabled", true);

        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}

// Add AcademicDetail

$(function () {
    $('#WorkExpAdd').click(function () {



        var RegNo = $('#ScholarNo').val();

        var general =
        {
            TotalExperience: $('#Wrk_TotalExperience').val(),
            CompanyName: $('#Wrk_CompanyName').val(),
            Designation: $('#Wrk_CompanyDesignation').val(),
            FromDate: $('#Wrk_CompanyDuration').val(),
            CompanyProfile: $('#Wrk_CompanyProfile').val(),
            ScholarNumber: $('#ScholarNo').val()
        };

        $.ajax({
            type: "POST",
            url: "Student.asmx/AddWorkExp",
            data: JSON.stringify({ 'WorkExp': general }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ClearWorkExpDetails();
                GetWorkExpDetailsByRegNo(RegNo);

            }
        });
    });
});


// Update AcademicDetail
$(function () {
    $('#WorkExUpdate').click(function () {


        var RegNo = $('#ScholarNo').val();

        var general =
        {
            TotalExperience: $('#Wrk_TotalExperience').val(),
            CompanyName: $('#Wrk_CompanyName').val(),
            Designation: $('#Wrk_CompanyDesignation').val(),
            FromDate: $('#Wrk_CompanyDuration').val(),
            CompanyProfile: $('#Wrk_CompanyProfile').val(),
            ScholarNumber: $('#ScholarNo').val()
        };

        $.ajax({
            type: "POST",
            url: "Student.asmx/UpdateWorkExp",
            data: JSON.stringify({ 'WorkExp': general }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ClearWorkExpDetails();
                GetWorkExpDetailsByRegNo(RegNo);
                $("#WorkExpAdd").prop("disabled", false);
            }
        });
    });
});


//Delete AcademicDetail
function DeleleStudent(ID) {
    //var RegNo = $('#RegisterNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "Student.asmx/DeleteStudent",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: "{id:'" + ID + "'}",
            success: function (result) {
                $(this).closest('tr').remove()
                //GetWorkExpDetailsByRegNo(RegNo);
                GetAllStudentDetails();

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