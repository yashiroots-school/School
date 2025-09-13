GetAllStaffDetails();

function GetAllStaffDetails() {  
    var html = "";
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/Staf/GetStaffDetailList",
        dataType: "json",
        success: function (data) {
            console.log(data)
            for (var i = 0; i < data.length; i++) {
                var staff = data[i].s; 
                if (staff.Spare1 == "admin" || staff.Spare1 == "admin user") {
                    html += "<tr style='color:orange;'>";
                }
                else {
                    html += "<tr>";
                }
                html += "<tr>";
                html += "<td>" + staff.EmpId + "</td>";
                html += "<td>" + staff.Name + "</td>";
                html += "<td>" + checknullvale(staff.Qualification) + "</td>";
                html += "<td>" + staff.Gender + "</td>";
                html += "<td>" + checknullvale(staff.EmpDate) + "</td>";
                //html += "<td>" + checknullvale(data[i].IsActive) + "</td>";
                if (staff.IsActive === false) {
                    html += "<td style='color:red;font-weight:bold;'>Non-ACTIVE</td>";
                } else {
                    html += "<td style='color:green;font-weight:bold;'>ACTIVE</td>";
                }
                html += "<td>" + checknullvale(data[i].StaffCategoryName) + "</td>";
                html += "<td>";
                html += '<a href="/Staf/AddStaf?UpdateId=' + staff.StafId + '">Edit</a>&nbsp&nbsp;<a href="/Staf/FullStaffView?id=' + staff.StafId + '">Full View</a>&nbsp&nbsp;<a href="#" onclick="DeleteStaff(' + staff.StafId + ')">Delete</a>';
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
//function GetAllStaffDetails() {
//    var html = "";
//    $.ajax({
//        type: "GET",
//        contentType: "application/json; charset=utf-8",
//        url: "/Staf/GetStaffDetailList",
//        dataType: "json",
//        success: function (data) {
//            console.log(data);
//            for (var i = 0; i < data.length; i++) {
//                var staff = data[i].s;   // ✅ use inner object "s"

//                if (staff.Spare1 === "admin" || staff.Spare1 === "admin user") {
//                    html += "<tr style='color:orange;'>";
//                } else {
//                    html += "<tr>";
//                }

//                html += "<td>" + (staff.EmpId || "") + "</td>";
//                html += "<td>" + (staff.Name || "") + "</td>";
//                html += "<td>" + checknullvale(staff.Qualification) + "</td>";
//                html += "<td>" + (staff.Gender || "") + "</td>";
//                html += "<td>" + checknullvale(staff.EmpDate) + "</td>";

//                // Handle null/true/false for IsActive
//                if (staff.IsActive === true) {
//                    html += "<td style='color:green;font-weight:bold;'>ACTIVE</td>";
//                } else {
//                    html += "<td style='color:red;font-weight:bold;'>Non-ACTIVE</td>";
//                }

//                // Prefer staff.StaffCategoryName, fallback to data[i].StaffCategoryName
//                var staffCategory = staff.StaffCategoryName || data[i].StaffCategoryName || "N/A";
//                html += "<td>" + staffCategory + "</td>";

//                html += "<td>";
//                html += '<a href="/Staf/AddStaf?UpdateId=' + staff.StafId + '">Edit</a>&nbsp;&nbsp;';
//                html += '<a href="#" onclick="DeleteStaff(' + staff.StafId + ')">Delete</a>';
//                html += "</td>";
//                html += "</tr>";
//            }

//            $("#stafftblbody").html(html);
//            loaddatatable();
//        },
//        error: function () {
//            alert("Error");
//        }
//    });
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






