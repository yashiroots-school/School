GetAllStaffDetails();

function GetAllStaffDetails() {  
    var html = "";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Staf/GetStaffDetailList",
        dataType: "json",
        success: function (data) {

            for (var i = 0; i < data.length; i++) {
                if (data[i].Spare1 == "admin" || data[i].Spare1 == "admin user") {
                    html += "<tr style='color:orange;'>";
                }
                else {
                    html += "<tr>";
                }
                html += "<tr>";
                html += "<td>" + data[i].EmpId + "</td>";
                html += "<td>" + data[i].Name + "</td>";
                html += "<td>" + checknullvale( data[i].Qualification) + "</td>";
                html += "<td>" + data[i].Gender + "</td>";
                html += "<td>" + checknullvale(data[i].EmpDate) + "</td>";               
                html += "<td>";
                html += '<a href="/Staf/AddStaf?UpdateId=' + data[i].StafId + '">Edit</a>&nbsp&nbsp;<a href="#" onclick="DeleteStaff(' + data[i].StafId + ')">Delete</a>&nbsp&nbsp;<a href="/Staf/FullStaffView?Id=' + data[i].StafId + '">View</a>';
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

//function loaddatatable(html) {
//    $('#Stafftbl').DataTable().destroy();
//    $('#Stafftbl').find('tbody').html('');
//    $('#Stafftbl').find('tbody').append(html);
//    $('#Stafftbl').DataTable({
//        "autoWidth": true,
//        "scrollX": true
//    }).draw();  
//}

function loaddatatable() {
    if ($.fn.DataTable.isDataTable('#Stafftbl')) {
        $('#Stafftbl').DataTable().destroy();
    }

    $('#Stafftbl').dataTable({
        "autoWidth": true,
        "scrollX": true
    });

}
function DeleteStaff(ID) {
    var ans = confirm("Are you sure you want to delete this Record?");
    if (ans) {
        $.ajax({
            url: "/Staf/DeleteStaff?id=" + ID,
            type: "GET",
            contentType: "application/json;charset=UTF-8",
            dataType: "json",
          
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

function checknullvale(value) {
    return value == null ? "" : value;
}






