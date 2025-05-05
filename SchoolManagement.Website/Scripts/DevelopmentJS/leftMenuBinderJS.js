$(document).ready(function () {
    //
    //LeftMenuBinding();
});

function LeftMenuBinding() {
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        url: "/AppConfiguration/BindLeftMenuData",
        dataType: "json",
        success: function (data) {
            $("#LeftMenu").html(data);
        },
        error: function (result) {
            alert("Error");
        }
    });
}
