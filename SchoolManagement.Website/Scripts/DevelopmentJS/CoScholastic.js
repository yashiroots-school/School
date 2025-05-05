$(document).ready(function () {

   $("#CoScholasticTable").DataTable();
  

    $("#CancleBtn").hide();
   
        $(document).on('click', '.BtnCoScholasticEdit', function () {
        var id = $(this).attr("data-val");
        $("#BtnSubmitCoScholasticClass").text("Update");
        $("#CoScholasticClassForm").attr('action', '/Exam/UpdateCoScholastic');
        GetGradeById(id);
        $("#CancleBtn").show();
    });


    
        $(document).on('click', '.BtnCoScholasticDelete', function () {

        var id = $(this).attr("data-val");
        var cnf = confirm("Are You Sure, You want to delete the record?");
        if (cnf == true) {
            window.location.href = "/Exam/DeleteCoScholastic?Id=" + id;
        }
    });

    $("#CancleBtn").on('click', function () {
        window.location.reload();
    });



});
function GetGradeById(id) {
    $.ajax({
        url: "/Exam/GetCoScholasticById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#CoscholasticID").val(result.CoscholasticID);
                $("#ClassID").val(result.ClassID);
                $(".CoScholasticId").val(result.Id);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}

