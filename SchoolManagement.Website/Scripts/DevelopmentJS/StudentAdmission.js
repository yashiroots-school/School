//GetAllStudentDetails();

//$("#FilterStudents").on("click", function () {
//    var tb = document.getElementById('Studenttbl');
//    while (tb.rows.length > 1) {
//        tb.deleteRow(1);
//    }
//    var html = "";
//    var year = $("#Yearddl option:selected").val();
//    var batch = $("#Batchddl option:selected").val();
//    var course = $("#Courseddl option:selected").val();
//    var semester = $("#class option:selected").val();
//    $.ajax({
//        type: "GET",
//        contentType: "application/json; charset=utf-8",
//        // url: "Student.asmx/GetAllStudents",
//        url: "/Student/GeStudentList?year=" + year + "&batch=" + batch + "&course=" + course + "&semester=" + semester,
//        dataType: "json",
//        success: function (data) {

//            for (var i = 0; i < data.length; i++) {
//                 html += "<tr>";
//                html += "<td>" + data[i].ScholarNumber + "</td>";
//                html += "<td>" + data[i].StudentName + "</td>";
//                html += "<td>" + data[i].Category + "</td>";
//                html += "<td>Grade</td>";
//                html += "<td>" + data[i].Course + "</td>";
//                html += "<td>" + data[i].Batch + "</td>";
//                html += "<td>" + data[i].Specialization + "</td>";
//                html += "<td>" + data[i].MobileNo + "</td>";
//                html += "<td>" + data[i].Semester + "</td>";

//                html += "<td>";
//                html += '<a href="/Student/ViewStudent?studentId=' + data[i].ScholarNumber + '">View</a>&nbsp&nbsp;<a href="/Student/PreRegisterStudent?UpdateId=' + data[i].ScholarNumber + '">Edit</a>&nbsp&nbsp;<a href="#" onclick="DeleleStudent(' + data[i].StudentId + ')">Delete</a>';
//                html += "</td>";
//                html += "</tr>";
//            }

//            $('#Studenttbl').DataTable().destroy();
//            $('#Studenttbl').find('tbody').html('');
//            $('#Studenttbl').find('tbody').append(html);
//            $('#Studenttbl').DataTable().draw();

//        },
//        error: function (result) {
//            alert("Error");
//        }
//    });
//});
//Get All Student Details
var ii = 0;
function GetAllStudentDetails() {
    ii++;

    var html = "";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/StudentAdmission/GeStudentList",
        data: { DDClass: $('#classDll').val(), DDBatch: $('#Batchddl').val() },
        dataType: "json",
        success: function (data) {
            //$("#stdnttblbody").empty();
            //$("#Studenttbl > stdnttblbody").html("");
            //$("#Studenttbl").html("");

            //html += "<tr>";
            //html += "<th>Student ID</th>";
            //html += "th>Name</th>";
            //html += "<th>Catogory</th>";
            //html += "<th>Class</th>";
            //html += "<th>Action</th>";
            //html += "tr>";
            //$("#stdnttblhead").html(html);
            var stdnttbl = document.getElementById('loadStudent');
            html = '';

            html = '<table class="table table-responsive table-striped" style="width:100% " id="Studenttbl">';
            html = html + '<thead id="stdnttblhead"><tr><th>Student ID</th><th>Name</th><th>Catogory</th><th>Class</th><th>Current Status</th><th>Admission Status</th><th>Action</th></tr></thead>';
            html = html + '<tbody id="stdnttblbody">';
            for (var i = 0; i < data.length; i++) {
                if (data[i].Spare1 == 'admin' || data[i].Spare1 == 'admin user') {
                    html += '<tr style="color:orange;">';
                }
                else {
                    html += '<tr>';
                }
                html += '<td>' + data[i].ScholarNumber + '</td>';
                html += '<td>' + data[i].StudentName + '</td>';
                html += '<td>' + data[i].Category + '</td>';
                html += '<td>' + data[i].Semester + '</td>';
                html += '<td>' + data[i].isapprove + '</td>';
                html += '<td>';
                //html += '<a href="/Student/FullStudentView?id=' + data[i].ScholarNumber + '">View</a>&nbsp&nbsp;<a href="/Student/AddStudent?UpdateId=' + data[i].ScholarNumber + '">Edit</a>&nbsp&nbsp;<a href="#" onclick="DeleleStudent(' + data[i].StudentId + ')">Delete</a>';
                //@Html.DropDownList("Class", new SelectList(ViewBag.Classes, "DataListItemName", "DataListItemName"), "select", new { @id = "class", @class = "form-control" })
                html += '</td>';
                html += '<td><input type="button" value="Update" class=" pull-right btn btn-success" /></td>'
                html += '</tr>';
            }

            html += '</tbody></html>'

            stdnttbl.innerHTML = html;
            //loaddatatable();
            $('#Studenttbl').DataTable();
        },
        error: function (result) {
            alert("Error");
        }
    });
}

function loaddatatable() {
    if ($.fn.DataTable.isDataTable('#Studenttbl')) {
        $('#Studenttbl').DataTable().destroy();
    }

    $('#Studenttbl').dataTable({
        "autoWidth": true,
        "scrollX": true
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

function DeleleStudent(ID) {

    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Student/DeleteStudent?id=" + ID,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            //data: "{id:'" + ID + "'}",
            success: function (result) {
                alert(result);
                location.reload();

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