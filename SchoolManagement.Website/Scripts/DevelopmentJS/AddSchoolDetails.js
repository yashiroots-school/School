$(document).ready(function () {

        $('a[data-toggle="tab"]').on('show.bs.tab', function (e) {
            localStorage.setItem('activeTab', $(e.target).attr('href'));
        });
        var activeTab = localStorage.getItem('activeTab');
        if (activeTab) {
            $('#myTab a[href="' + activeTab + '"]').tab('show');
        }

    //$("#SchoolTable").DataTable();
    ////$("#BankTable1").DataTable();
    ////$("#BranchTable").DataTable();
    ////$("#MerchantnameTable").DataTable();
    //$("#MerchantIdTable").DataTable();
    //$("#SchoolsetupId").DataTable();
    $('table.display').DataTable();
});


$(".BtnSchoolEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnSchoolSubmit").text("Update");
    $("#Schoolform").attr('action', '/Payment/UpdateSchool');
    GetSchoolById(id);
});

function GetSchoolById(id) {
    $.ajax({
        url: "/Payment/GetSchoolById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#School_Id").val(result.School_Id);
                $("#School_Name").val(result.School_Name);
                $("#Email").val(result.Email);
                $("#Password").val(result.Password);
                $("#Website").val(result.Website);
                $("#Address").val(result.Address);
                $('#Status').val(result.Status);
                $('#BoardID').val(result.BoardID);
                if (result.Upload_Image != null) {
                    $("#Upload_Image").text(result.Upload_Image);
                }
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnSchoolDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteSchool?Id=" + id;
    }
});

$(".BtnBankEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnBankSubmit").text("Update");
    $("#Bankform").attr('action', '/Payment/UpdateBank');
    GetBankById(id);
});

function GetBankById(id) {
    $.ajax({
        url: "/Payment/GetBankById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Bank_Id").val(result.Bank_Id);
                $("#Bank_Name").val(result.Bank_Name);
                $("#Bank_Code").val(result.Bank_Code);
                //$("#Contact_No").val(result.Contact_No);
                //$("#Contactperson_Name").val(result.Contactperson_Name);
                //$("#LandlineNo").val(result.LandlineNo);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnBankDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteBank?Id=" + id;
    }
});




$(".BtnBranchEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnBranchSubmit").text("Update");
    $("#Branchform").attr('action', '/Payment/UpdateBranch');
    GetBranchById(id);
});

function GetBranchById(id) {
    $.ajax({
        url: "/Payment/GetBranchById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Branch_ID").val(result.Branch_ID);
                $("#Bank_Id1").val(result.Bank_Id);
                //$("#Bank_Name").val(result.Bank_Name);
                $("#Branch_Name").val(result.Branch_Name);
                $("#Contact_No").val(result.Contact_No);
                $("#Contact_Name").val(result.Contact_Name);
                $("#LandlineNo").val(result.Landline_No);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnBranchDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteBranch?Id=" + id;
    }
});


$(".BtnBankDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteBank?Id=" + id;
    }
});




$(".BtnMerchantNameEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnMerchantSubmit").text("Update");
    $("#MerchantNameform").attr('action', '/Payment/UpdateMerchantName');
    GetMerchantNameById(id);
});

function GetMerchantNameById(id) {
    $.ajax({
        url: "/Payment/GetMerchantNameById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#MerchantName_Id").val(result.MerchantName_Id);
                $("#School_Id1").val(result.School_Id);
                $("#MerchantName").val(result.MerchantName);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnMerchantNameDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteMerchantName?Id=" + id;
    }
});

$('#School_Id2').on('change', function () {
    let schoolId = $(this).val();
    $.ajax({
        url: "/Payment/GetMerchantBySchool?Id=" + schoolId,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                let merchantHTML = '';
                for (var i = 0; i < result.length; i++) {
                    merchantHTML += '<option value="' + result[i].MerchantName_Id + '">' + result[i].MerchantName + ' </option>';
                }

                $('#MerchantName_Id1').html(merchantHTML);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});


$(".BtnMerchantIdEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnMerchantIDSubmit").text("Update");
    $("#MerchantIdform").attr('action', '/Payment/UpdateMerchantId');
    GetMerchantIdById(id);
});

function GetMerchantIdById(id) {
    $.ajax({
        url: "/Payment/GetMerchantIdById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Merchant_Id").val(result.Merchant_Id);
                $("#School_Id2").val(result.School_Id);
                $("#Bank_Id3").val(result.Bank_Id);
                $("#Branch_Id2").val(result.Branch_Id);
                $("#MerchantName_Id1").val(result.MerchantName_Id);
                $("#MerchantMID").val(result.MerchantMID);
                $("#MerchantKey").val(result.MerchantKey);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnMerchantIdDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Payment/DeleteMerchantID?Id=" + id;
    }
});



$("#Bank_Id3").on('click', function () {
    var id = $(this).val();
    if (id == 0)
        return false;

    $.ajax({
        url: "/Payment/GetbankdetailsById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Branch_Id2").html('')
                $("#Branch_Id2").append($('<option>', { value:0, text: '---Select---' }));
                $.each(result, function (key, value) {
                    $("#Branch_Id2").append($('<option>', { value: value.Branch_ID, text: value.Branch_Name }));
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});


$("#Branch_Id2").on('click', function () {
    var id = $(this).val();
    if (id == 0)
        return false;
    $.ajax({
        url: "/Payment/GetmerchantdetailsById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null && result != '') {
                $("#MerchantName_Id1").html('')
                $("#MerchantName_Id1").append($('<option>', { value: 0, text: '---Select---' }));
                $.each(result, function (key, value) {
                    $("#MerchantName_Id1").append($('<option>', { value: value.MerchantName_Id, text: value.MerchantName }));
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});


$("#Bank_Id4").on('click', function () {
    var id = $(this).val();
    if (id == 0)
        return false;

    $.ajax({
        url: "/Payment/GetbankdetailsById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#Branch_Id3").html('')
                $("#Branch_Id3").append($('<option>', { value: 0, text: '---Select---' }));
                $.each(result, function (key, value) {
                    $("#Branch_Id3").append($('<option>', { value: value.Branch_ID, text: value.Branch_Name }));
                });
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
});


$("#BtnschoolsetupSubmit").on('click', function () {
    var School_Id3 = $("#School_Id3").val();

    if (School_Id3 == 0) {
        return false;
    }




});

