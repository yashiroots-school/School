$(document).ready(function () {

    var weburl = "";
    if (!weburl) weburl = window.location.href
    var array = weburl.split('/');
    var no = array[array.length - 1];

    //if (no != "") {
    //    GetAcademicDetailsByRegNo(no);
    //}

});

//Get All AcademicDetail By RegNo
function GetWorkExpDetailsByRegNo(RegstrNo) {
    var tb = document.getElementById('WorkExptbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/GetAllWorkExpDetailsByRegNo",
        data: "{'RegNo' : '" + RegstrNo + "'}",
        dataType: "json",
        success: function (data) {
           
            var totalexp = 0;

            for (var i = 0; i < data.length; i++) {


                totalexp += data[i].FromDate;

                //$("#WorkExptbl").append("<tr><td hidden='hidden'>" + data.d[i].WorkExperienceId + "</td><td>" + data.d[i].TotalExperience + "</td><td>" + data.d[i].CompanyName
                $("#WorkExptbl").append("<tr><td hidden='hidden'>" + data[i].WorkExperienceId + "</td><td>" + data[i].CompanyName
                    + "</td><td>" + data[i].Designation + "</td><td>" + data[i].FromDate + "</td><td>" + data[i].CompanyProfile + "</td><td>"
                    + '<a href="#" onclick="return getWorkExpDetailsById(' + data[i].WorkExperienceId + ')">Edit</a> | <a href="#" onclick="DeleleWorkExp(' + data[i].WorkExperienceId + ')">Delete</a></td><tr>');
            }

            $("#lbltotaWorkExperince").html(totalexp);

        },
        error: function (result) {
            alert("Error");
        }
    });
}

//Get Academic details by AcademicId
function getWorkExpDetailsById(WorkExpId) {
    
    $.ajax({
        url: "/Student/GetWorkExpDetailsById",
        type: "POST",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        data: "{id:'" + WorkExpId + "'}",
        success: function (result) {
            
            $('#WorkExperienceId').val(result.WorkExperienceId);
            //  $('#Wrk_TotalExperience').val(result.d[0].TotalExperience);
            $('#Wrk_CompanyName').val(result.CompanyName);
            $('#Wrk_CompanyDesignation').val(result.Designation);
            $('#Wrk_CompanyDuration').val(result.FromDate);
            $('#Wrk_CompanyProfile').val(result.CompanyProfile);
            $('#ScholarNo').val(result.ScholarNumber);

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
        
        var Total = $('#Std_TotalExperience').val();
        var compDur = $('#Wrk_CompanyDuration').val();
        var RegNo = $('#ScholarNo').val();
        var _CompanyName = $('#Wrk_CompanyName').val();
        var _Designation = $('#Wrk_CompanyDesignation').val();
        var _CompanyProfile = $('#Wrk_CompanyProfile').val();


        var general =
        {
            // TotalExperience: Total,
            CompanyName: $('#Wrk_CompanyName').val(),
            Designation: $('#Wrk_CompanyDesignation').val(),
            //FromDate: $('#Wrk_CompanyDuration').val(),
            FromDate: compDur,
            CompanyProfile: $('#Wrk_CompanyProfile').val(),
            ScholarNumber: $('#ScholarNo').val()
        };

        var msg = "";

        if (_CompanyName == "") {
            msg += "Company name cannot be blank\n";
        }

        if (_Designation == "") {
            msg += "Designation cannot be blank\n";
        }
        if (compDur == "") {
            msg += "Duration cannot be blank\n";
        }
        if (_CompanyProfile == "") {
            msg += "Company profile cannot be blank\n";
        }

        if (msg != "") {
            alert("Please fill required fields :\n" + msg);
        }
        else {

            $.ajax({
                type: "POST",
                url: "/Student/AddWorkExp",
                data: JSON.stringify({ 'WorkExp': general }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    var RegNo= $('#ScholarNo').val()
                    // f1();
                    // TotalExperience = general.FromDate;
                    //   TotalExperience = compDur;
                    ClearWorkExpDetails();
                    GetWorkExpDetailsByRegNo(RegNo);

                }
            });
        }
    });
});


// Update AcademicDetail
$(function () {
    $('#WorkExUpdate').click(function () {

        
        var RegNo = $('#ScholarNo').val();

        var general =
        {
            //  TotalExperience: $('#Wrk_TotalExperience').val(),
            CompanyName: $('#Wrk_CompanyName').val(),
            Designation: $('#Wrk_CompanyDesignation').val(),
            FromDate: $('#Wrk_CompanyDuration').val(),
            CompanyProfile: $('#Wrk_CompanyProfile').val(),
            ScholarNumber: $('#ScholarNo').val(),
            WorkExperienceId : $('#WorkExperienceId').val()
        };

        $.ajax({
            type: "POST",
            url: "/Student/UpdateWorkExp",
            data: JSON.stringify({ 'WorkExp': general }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                ClearWorkExpDetails();
                GetWorkExpDetailsByRegNo(RegNo);
                $("#WorkExpAdd").prop("disabled", false);
                $('#WorkExperienceId').val("0"); DeleleWorkExp
            }
        });
    });
});


//Delete AcademicDetail
function DeleleWorkExp(ID) {

    
    //var RegNo = $('#RegisterNo').val();
    var RegNo = $('#ScholarNo').val();
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "Student.asmx/DeleteWorkExp",
            type: "POST",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
            data: "{id:'" + ID + "'}",
            success: function (result) {
                $(this).closest('tr').remove()
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
    // $('#Wrk_TotalExperience').val("");
    $('#Wrk_CompanyName').val("");
    $('#Wrk_CompanyDesignation').val("");
    $('#Wrk_CompanyDuration').val("");
    $('#Wrk_CompanyProfile').val("");
    //$('#RegisterNo').val("");
}