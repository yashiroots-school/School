//
if (window.location.href.indexOf('UpdateId') > 0) {
    //
    var baseUrl = (window.location).href; // You can also use document.URL
    var staffId = baseUrl.substring(baseUrl.lastIndexOf('=') + 1);
    GetstafById(staffId);
}

function GetstafById(id) {
    $.ajax({
        url: "/staf/GetstafById?stafId=" + id,
        type: "GET",
        contentType: "application/json;charset=UTF-8",
        dataType: "json",
        success: function (result) {
            //
            console.log(result)
            $("#StafId").val(result.StafId);
            $("#EmpId").val(result.EmpId);

            $("#EmployeeCode").val(result.EmployeeCode);

            $("#UIN").val(result.UIN);

            $("#Date").val(result.Date);

            $("#Name").val(result.Name);
            if (result.Gender == "Male") {
                $("#GenderMale").prop("checked", true);
            } else {
                $("#GenderFemale").prop("checked", true);
            }
            //$('#Gender option[value="' + result.Gender + '"]').attr("checked", "checked");
            //$('#Gender').val(result.Gender).change();

            $("#Dateofbirth").val(result.DOB);

            $('#qualification option[value="' + result.Qualification + '"]').attr("selected", "selected");
            $('#qualification').val(result.Qualification).change();

            $("#WorkExperience").val(result.WorkExperience);

            $("#Employee_Designation").val(result.Employee_Designation);

            $('#Religion option[value="' + result.Religion + '"]').attr("selected", "selected");
            $('#Religion').val(result.Religion).change();

            $('#Category option[value="' + result.Category + '"]').attr("selected", "selected");
            $('#Category').val(result.Category).change();

            $('#Caste option[value="' + result.Caste + '"]').attr("selected", "selected");
            $('#Caste').val(result.Caste).change();


            $("#Address").val(result.Address);
            $("#Contact").val(result.Contact);
            $("#Email").val(result.Email);
            $("#AdharNo").val(result.AdharNo);
            $("#PanNo").val(result.PanNo);
            $("#BankACNo").val(result.BankACNo);
            $("#EmpDate").val(result.EmpDate);

            $("#BesicSallery").val(result.BesicSallery);
            $("#PerksSallery").val(result.PerksSallery);
            $("#GrossSallery").val(result.GrossSallery);

            $("#LastOrganizationofEmployment").val(result.LastOrganizationofEmployment);
            $("#NoofYearsattheLastAssignment").val(result.NoofYearsattheLastAssignment);

            if (result.FormalitiesCheck =="Yes") {
                $("#FormalitiesCheck1").prop("checked", true);
            } else {
                $("#FormalitiesCheck2").prop("checked", true);

            }
            //$("#RelievingLetter").val(result.RelievingLetter);
            //$("#PerformanceLetter").val(result.PerformanceLetter);

            if (result.MariedStatus == "Married") {
                $("#MariedStatus1").prop("checked", true);
            } else if (result.MariedStatus == "Unmarried"){
                $("#MariedStatus2").prop("checked", true);
            } else {
                $("#MariedStatus3").prop("checked", true);
            }

            $("#Children").val(result.Children);
            $("#DateofReliving").val(result.DateofReliving);
            $("#BesicSallery1").val(result.BesicSallery1);

            $("#PerksSallery1").val(result.PerksSallery1);
            $("#GrossSallery1").val(result.GrossSallery1);
            $("#FatherOrHusbandName").val(result.FatherOrHusbandName);
            if (result.IsActive == true || result.IsActive == null) {
                $("#IsActivechk").prop("checked", true);
                $("#IsActive").val(true);
            } else {
                $("#IsActivechk").prop("checked", false);
                $("#IsActive").val(false);
            }
            debugger
            $('#StaffCategory').val(result.StaffCategory).change();
            $('#StaffCategory').trigger('change');

            $("#headingName").text("Update Staff Details");
            $("#btnstafsave").text("Update Staff");
            $('#StaffForm').attr('action', '/Staf/EditStaffDetail');
          
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    }); return false;
}


$("#Basicdetails").on('click', function () {
    $("#primarytab").tab('show');
});

$("#primarydetails").on('click', function () {
    $("#pastemploymenttab").tab('show');
});

$("#pastschool").on('click', function () {
    $("#accounttypetab").tab('show');
});

$("#accounttype").on('click', function () {
    $("#salarydetailstab").tab('show');
});
$("#IsActivechk").on('change', function () {
    var isChecked = $(this).is(':checked');
    $('#IsActive').val(isChecked);
});

