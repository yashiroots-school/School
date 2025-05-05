$(document).ready(function () {
    GetAllStaffDetails();
    DeleleStaff(ID);
    GetstafById(id);
    
});

$("#FilterStaff").on("click", function () {
    var tb = document.getElementById('Stafftbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var html = "";
    var EmpId = $("#AllEmpId option:selected").val();

    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
      
        url: "/Staf/GetStaffList1?EmpId=" + EmpId,
        dataType: "json",
        success: function (data) {
            
            for (var i = 0; i < data.length; i++) {
                html += "<tr>";
                html += "<td>" + data[i].EmpId + "</td>";
                html += "<td>" + data[i].Name + "</td>";
                html += "<td>" + data[i].Qualification + "</td>";
                html += "<td>" + data[i].Gender + "</td>";
                html += "<td>" + data[i].EmpDate + "</td>";
                html += "<td>";
                html += '<img alt="Image" scr="~/WebsiteImages/MemberImage/" class="img-thumbnail" style="max-width: 30%" />';
                html += "</td>";
                html += "</tr>";
            }

            $('#Stafftbl').DataTable().destroy();
            $('#Stafftbl').find('tbody').html('');
            $('#Stafftbl').find('tbody').append(html);
            $('#Stafftbl').DataTable().draw();

        },
        error: function (result) {
            alert("Error");
        }
    });
});


//Get All View Details for View
function GetAllStaffDetails() {
    var tb = document.getElementById('Stafftbl');
    while (tb.rows.length > 1) {
        tb.deleteRow(1);
    }
    var html = "";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Staf/GetStaffList",
        dataType: "json",
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].Spare1 == "admin" || data[i].Spare1 == "admin user") {
                    html += "<tr style='color:orange;'>";
                }
                else {
                    html += "<tr>";
                }
                html += "<td>" + data[i].EmpId + "</td>";
                html += "<td>" + data[i].Name + "</td>";
                html += "<td>" + data[i].Qualification + "</td>";
                html += "<td>" + data[i].Gender + "</td>";
                html += "<td>" + data[i].EmpDate + "</td>";

                html += "<td>";
                html += '<img alt="Image" scr="~/WebsiteImages/MemberImage/" class="img-thumbnail" style="max-width: 30%" />';
                html += "</td>";
              
                html += "</tr>";
            }

            $("#stafftblbody").html(html);
            loaddatatable();
        },
        error: function (result) {
            alert("Error");
        }
    });
}

function loaddatatable() {
    if ($.fn.DataTable.isDataTable('#Stafftbl')) {
        $('#Stafftbl').DataTable().destroy();
    }

    $('#Stafftbl').dataTable({
        "autoWidth": true,
        "scrollX": true
    });

}