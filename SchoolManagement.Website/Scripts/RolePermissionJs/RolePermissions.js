$("#UserRole").change(function () {
    var selectedText = $("#UserRole option:selected").text();
    //var selectedVal = $("#UserRole option:selected").val();
    AllCheckedORUncheck(selectedText);
    GetAllPermissionByRoleName(selectedText);
});

$("#SaveUpBtn").on("click", function () {
    var selectedText = $("#UserRole option:selected").text();
    var selectedVal = $("#UserRole option:selected").val();
    SaveRolePermission(selectedVal, selectedText);
});

function AllCheckedORUncheck(value) {
     if (value == "Administrator") {
        $("input:checkbox").prop('checked', 'checked');
    } else {
        $('input[type=checkbox]').removeProp('checked');
    }
    if (value == "--Select Role--") {
        return false;
    }
}
function SaveRolePermission(roleid, rolename) {
    var ck_box = $('input[type="checkbox"]:checked').length;
    var listSaveData = [];
    if (ck_box == 0) {
        alert("Please select Permission");
        return false;
    } else if (roleid==0) {
        alert("Please select Role");
        return false;
    }
    $.each($('input[type=checkbox]'), function () {
        //
        var checkboxVal = $(this).val();
        var data = {};
        data.RoleId = roleid;
        data.RoleName = rolename;
        data.PageName = checkboxVal;
        data.PageViewName = $(this).attr("name");
        data.ParentId = $(this).attr("data-val");
      
        if (this.checked) {
            data.HasPermission = true;
        } else {
            data.HasPermission = false;
        }
        listSaveData.push(data);
    });

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/UserCredentials/SaveUpdateRolePermissions",
        data: JSON.stringify(listSaveData),
        dataType: "json",
        success: function (data) {
            if (data == true) {
                alert("Role Page Permission save Successfully.");
            }  
        },
        error: function (result) {
            alert("Some Error Occure");
        }
    });
}

function GetAllPermissionByRoleName(roleName) {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/UserCredentials/GetAllPageByRoleName?roleName=" + roleName,
        //data: JSON.stringify(listSaveData),
        dataType: "json",
        success: function (data) {
            bindAllCheckbox(data);
        },
        error: function (result) {
            alert("Some Error Occure");
        }
    });
}

function bindAllCheckbox(listData) {
    if (listData.length > 0) {
        $('input[type=checkbox]').removeProp('checked');

        $.each(listData,function (index, value) {
            if (value.HasPermission) {
                var roleName = value.PageName;
                $(":checkbox[value='" + roleName+"']").prop("checked", "checked");
            }  
        });
    }
}
