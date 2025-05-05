$(document).ready(function () {
    GetAllStudentDetails();
   
});

$("#FilterStudents").on("click", function () {
    var tb = document.getElementById('Studenttbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var html = "";
    var Class = $("#Class option:selected").val();
    var Section = $("#Section option:selected").val();
 
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        // url: "Student.asmx/GetAllStudents",
        url: "/Student/GeStudentList?Class=" + Class + "&Section=" + Section,
        dataType: "json",
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "<td>" + data[i].Class + "</td>";
                html += "<td>" + data[i].Section + "</td>";
                html += "<td>" + data[i].Gender + "</td>";
                html += "<td>" + data[i].DOB + "</td>";
          
                html += "</tr>";
            }

            $('#Studenttbl').DataTable().destroy();
            $('#Studenttbl').find('tbody').html('');
            $('#Studenttbl').find('tbody').append(html);
            $('#Studenttbl').DataTable().draw();

        },
        error: function (result) {
            alert("Error");
        }
    });
});