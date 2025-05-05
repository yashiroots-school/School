$(document).ready(function () {

    $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
        localStorage.setItem('activeTab', $(e.target).attr('href'));
    });
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('#myTab a[href="' + activeTab + '"]').tab('show');
    }
    $("#HideSubmenuHeader").hide();
    $("#stafflist").hide();

    //$("#BankTable1").DataTable();
});


$(".BtnMenuEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnMenuSubmit").text("Update");
    $("#Menuform").attr('action', '/UserCredentials/UpdateMenu');
    GetMenuById(id);
});

function GetMenuById(id) {
    $.ajax({
        url: "/UserCredentials/GetMenuById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Menu_Id").val(result.Menu_Id);
                $("#Menu_Name").val(result.Menu_Name);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnMenuDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/UserCredentials/DeleteMenu?Id=" + id;
    }
});


$(".BtnsubMenuEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnSubmenuSubmit").text("Update");
    $("#Submenuform").attr('action', '/UserCredentials/UpdateSubMenu');
    GetSubMenuById(id);
});

function GetSubMenuById(id) {
    $.ajax({
        url: "/UserCredentials/GetSubMenuById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Submenu_Id").val(result.Submenu_Id);
                $("#Menu_Id1").val(result.Menu_Id);
                $("#Submenu_Name").val(result.Submenu_Name);
                $("#Submenu_Url").val(result.Submenu_Url);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnsubMenuDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/UserCredentials/DeleteSubMenu?Id=" + id;
    }
});

$("#BtnSubmenuSubmit").on('click', function () {
    var id = $("#Menu_Id1").val();
    if (id == 0 || id == "") {
        alert("Please Select Menu");
        return false;
    }
       

});



$("#MenuId4").on('change',function () {
    var id = $(this).val();

    var roleid = $("#UserRole").val();
    var stafid = $("#staffid").val();
    var selectedtext = $("#UserRole option:selected").text();

    if (selectedtext != "Administrator" && selectedtext != "Student" && selectedtext != "Developer") {
        if (stafid == 0) {
            alert("Please select Staff");
            return false;s
        }
    }


    if (id == 0)
        return false;
    else
    {
        $.ajax({
            url: "/UserCredentials/GetSubmenunameById",
            data: { id: id, roleid: roleid, stafid: stafid, selectedtext: selectedtext },
            type: "GET",
            datatype: "json",
            success: function (result) {
                if (result == "Fail") {
                    //alert("No Data");
                    $("#Tblsubmenubody").empty();
                    $("#HideSubmenuHeader").hide();

                }
                else {
                    $("#HideSubmenuHeader").show();
                    $("#Tblsubmenubody").empty();
                    $("#Tblsubmenubody").append(result);
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }

});


$("#inactive2").on('click', function () {
    $("input[name2=MenuPermission]").prop('checked', $(this).prop('checked'));
});


$("#BtnSubmit").on('click', function () {
    var listdave = [];

    var userid = $("#UserRole").val();
    var menuid = $("#MenuId4").val();
    var stafid = $("#staffid").val();
    var selectedtext = $("#UserRole option:selected").text();
    if (selectedtext == "Staff" || selectedtext == "Account") {
        if (stafid == 0) {
            alert("Please select Staff");
            return false;
        }
    }
    if (userid == 0) {
        alert("Please Select Role");
        return false;
    }
    if (menuid == 0) {
        alert("Please Select Menu");
        return false;
    }

    $("#Tblsubmenubody tr").each(function () {
        var self = $(this);
        var data = {};

        data.Role_Id = userid;
        data.Menu_Id = menuid;
        data.Staff_Id = stafid;

        data.Submenu_Url = self.find("td:eq(1) input[type='checkbox']").attr("name");
        data.Submenu_Id = self.find("td:eq(2) input[type='checkbox']").attr("name");
        data.Submenu_Name = self.find("td:eq(2) input[type='checkbox']").attr("name1");

        if (self.find("td:eq(1) input[type='checkbox']").prop('checked')) {
            data.Submenu_permission = true;
        }
        else {
            data.Submenu_permission = false;
        }

        if (self.find("td:eq(2) input[type='checkbox']").prop('checked')) {
            data.Create_permission = true;
        }
        else {
            data.Create_permission = false;
        }

        if (self.find("td:eq(3) input[type='checkbox']").prop('checked')) {
            data.Edit_Permission = true;
        }
        else {
            data.Edit_Permission = false;
        }

        if (self.find("td:eq(4) input[type='checkbox']").prop('checked')) {
            data.Update_Permission = true;
        }
        else {
            data.Update_Permission = false;
        }

        if (self.find("td:eq(5) input[type='checkbox']").prop('checked')) {
            data.Delete_Permission = true;
        }
        else {
            data.Delete_Permission = false;
        }

        listdave.push(data);
    });


    $.ajax({
        url: "/UserCredentials/SavemenuPersmission",
        type: "POST",
        dataType: "json",
        contentType: "application/json;charset=utf-8",
        data: JSON.stringify(listdave),
        success: function (result) {
            if (result == true) {
                alert("Permission Added Successfully");
                location.reload();
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });

});


$("#UserRole").change(function () {
    var selectedtext = $("#UserRole option:selected").text();
    var selectedval = $("#UserRole option:selected").val();
    var stafid = $("#staffid").val();

    if (selectedtext != "Administrator" && selectedtext != "Student" && selectedtext != "Developer") {
        $("#stafflist").show();
    }
    else {
        $("#stafflist").hide();
    }

    console.log("selectedtext", selectedtext);
    console.log("selectedval", selectedval);

});
