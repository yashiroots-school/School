

function bindSection2Table(data) {

    var rows = [];

    for (var i = 0; i < data.length; i++) {

        var row = "<tr headingId=" + data[i].FeeHeadingIDs +"><td>" + (i + 1) + "</td>";

        row += "<td>" + data[i].PayHeadings + "</td>";
        row += "<td><input type='checkbox' class='selectbox rowSelect' name='selectbox' /></td>";
        row += "<td>" + data[i].TotalFee + "</td>";
        row += "<td>" + data[i].PaidAmount + "</td>";
        row += "<td><input type='number'min=0   value='"
            + data[i].TotalFee + "' max='" + data[i].TotalFee
            + "'  class='collectFeeNumber' /></td > ";
        row += "</tr>";

        rows.push(row);
    }

    $("#tbody").html(rows.toString());

}

function AddDueFee() {
    var feeReceiptViewModel = {}
    var FeeHeadings = [];
    var FeeHeadingAmt = [];
    var Selectedmonths = [];
    var collectFees = [];
    var Jan = [];
    var Feb = [];
    var Mar = [];
    var Apr = [];
    var May = [];
    var Jun = [];
    var Jul = [];
    var Aug = [];
    var Sep = [];
    var Oct = [];
    var Nov = [];
    var Dec = [];

    feeReceiptViewModel.StudentId = $("#StudentName option:selected").val();
    feeReceiptViewModel.CourseSpecialization = $("#CourseSpecialization").val();
    feeReceiptViewModel.ClassName = $("#Class").val();
    feeReceiptViewModel.CategoryName = $("#Category").val();
    feeReceiptViewModel.StudentName = $("#Name").val();
    feeReceiptViewModel.BatchName = $("#Batch").val();

    $.each($("input[name='category']:checked"), function () {
        Selectedmonths.push($(this).val());
    });

    feeReceiptViewModel.Selectedmonths = Selectedmonths;

    $("#section2Table tbody tr:not() ").not(':first').not(':last').each(function () {

        //var checkBox = $(this).find('input[type="checkbox"]').is(':checked');
        // if (checkBox == true) {
        var FeeHeading = $(this).find("td:eq(1)").html();
        var total = $(this).find("td:nth-last-child(2)").html();
        var fee = $(this).find(':input[type="number"]').val();
        FeeHeadings.push(FeeHeading);
        FeeHeadingAmt.push(total);
        collectFees.push(fee);
        Jan.push($(this).find("td:eq(3)").html());
        Feb.push($(this).find("td:eq(4)").html());
        Mar.push($(this).find("td:eq(5)").html());
        Apr.push($(this).find("td:eq(6)").html());
        May.push($(this).find("td:eq(7)").html());
        Jun.push($(this).find("td:eq(8)").html());
        Jul.push($(this).find("td:eq(9)").html());
        Aug.push($(this).find("td:eq(10)").html());
        Sep.push($(this).find("td:eq(11)").html());
        Oct.push($(this).find("td:eq(12)").html());
        Nov.push($(this).find("td:eq(13)").html());
        Dec.push($(this).find("td:eq(14)").html());
    });

    feeReceiptViewModel.Jan = Jan;
    feeReceiptViewModel.Feb = Feb;
    feeReceiptViewModel.Mar = Mar;
    feeReceiptViewModel.Apr = Apr;
    feeReceiptViewModel.May = May;
    feeReceiptViewModel.Jun = Jun;
    feeReceiptViewModel.Jul = Jul;
    feeReceiptViewModel.Aug = Aug;
    feeReceiptViewModel.Sep = Sep;
    feeReceiptViewModel.Oct = Oct;
    feeReceiptViewModel.Nov = Nov;
    feeReceiptViewModel.Dec = Dec;
    feeReceiptViewModel.FeeHeadings = FeeHeadings;
    feeReceiptViewModel.FeeHeadingAmt = FeeHeadingAmt;
    feeReceiptViewModel.collectFees = collectFees;
    feeReceiptViewModel.TotalFee = $("#TotalFee").val();
    feeReceiptViewModel.Course = $("#Course").val();

    if (FeeHeadingAmt.length == 0) {
        return;
    }
    $.ajax({
        url: '/TransportFee/AddDueFee',
        dataType: "json",
        type: "POST",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(feeReceiptViewModel),
        async: true,
        processData: false,
        cache: false,
        success: function (data) {
            console.log("Due Fees updated");
            // alert(data);
        },
        error: function (xhr) {
            window.location.reload();
        }
    });
}

$(document).ready(function () {

    var datetime = new Date();
    $("#DateFrom").val(datetime.toLocaleString())


    $('#StudentName').select2({

    });
    $("#Class").select2({

    });

    $("#StudentName").change(function () {
        //
        var studentId = this.value;

        if (studentId == "") {
            window.location.reload();
            return;
        }

        $.ajax({
            url: '/TransportFee/GetStudentDetailsById?studentId=' + studentId,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',

            async: true,
            processData: false,
            cache: false,
            success: function (data) {
                $("#Name").val(data.StudentName);
                $("#FatherName").val(data.FatherName);
                $("#OldBalance").val(data.OldBalance);
                $("#roleNumber").val(data.RoleNumber);
                $("#ContNum").val(data.Contact);

                $("#tbody").html('');
                $("#btnSaveReceipt").attr('disabled');
                $("#allTotal").html(0);
                $("#TotalCollectFee").html(0);

                $("#TotalFee").val(0);
                $("#NetFee").val(0);
                $("#ConcessionAmt").val(0);
                $("#LateFee").val(0);
                $("#Concession").val(0);
                $("#BalanceAmt").val(0);
                $("#ReceiptAmt").val(0);
            },
            error: function (xhr) {
                alert("Some Error Occure");
            }
        });
    });

    $("#Class").change(function () {
        var classid = this.value;
        if (classid == "") {
            window.location.reload();
            return;
        }
        $.ajax({
            url: "/AdmissionFee/GetStudentByClass?classid=" + classid + '&Filtertype=2',
            type: "GET",
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                if (data.length > 0 && data != "Fail") {
                    var alldata = [];
                    $("#StudentName").html('').select2({ data: [{ id: '', text: '' }] });
                    alldata.push({ id: 0, text: "Select" });
                    for (var i = 0; i < data.length; i++) {
                        alldata.push({
                            id: data[i].StudentRegisterID, text: data[i].Name + " " + data[i].Last_Name
                        });
                    }
                    $("#StudentName").html('').select2({
                        data: alldata
                    });
                }
                else {
                    alert("There is no student Present in this class");
                    window.location.reload();
                }
            }
        });

    });

    $("#CheckFee").click(function () {

        var studentId = $("#StudentName option:selected").val();

        if (studentId == "")
            return;

        $.ajax({
            url: '/TransportFee/GetStudentFees?studentId=' + studentId,
            dataType: "json",
            type: "GET",
            contentType: 'application/json; charset=utf-8',
            async: true,
            processData: false,
            cache: false,
            success: function (data) {
                bindSection2Table(data);
                AddDueFee();
                $("#Section2").removeAttr('hidden');               
            },
            error: function (error) {
                alert("Some Error Occure");
            }
        });

    });

    $(document).on('change', '.selectbox', function () {



        var allfee = 0;



        $("#section2Table tbody tr").each(function () {

            var checkBox = $(this).find('input[type="checkbox"]').is(':checked');

            if (checkBox) {
                var fee = $(this).find("td:nth-last-child(3)").html();

                fee = parseFloat(fee);

                allfee = allfee + Number(fee);
            }

        });




        $("#ConcessionAmt").val(0);
        $("#LateFee").val(0);
        $("#Concession").val(0);
        $("#BalanceAmt").val(0);
        $("#ReceiptAmt").val(0);
        $("#BalanceAmt").val(0);

        var oldFee = parseFloat($("#OldBalance").val());
        $("#allTotal").html(allfee);



    });

    $("#selectAll").change(function () {

        var allfee = 0;


        if ($(this).prop("checked")) {
            $("input[name='selectbox']").prop('checked', true);
            $("#section2Table tbody tr").each(function () {

                var fee = $(this).find(':input[type="number"]').val();
                allfee = allfee + Number(fee);

            });

        } else {
            $("input[name='selectbox']").prop('checked', false);
            allfee = 0;
        }

        $("#allTotal").html(allfee);
        $("#TotalCollectFee").html(0);

        $("#ConcessionAmt").val(0);
        $("#LateFee").val(0);
        $("#Concession").val(0);
        $("#BalanceAmt").val(0);
        $("#ReceiptAmt").val(0);
        $("#BalanceAmt").val(0);

    });



    $("#LateFee").change(function () {

        var total = parseInt($("#TotalFee").val());
        var lateFee = parseInt($("#LateFee").val());

        var netFee = total + lateFee;

        $("#NetFee").val(netFee);
        $("#ReceiptAmt").val(0);
        $("#BalanceAmt").val(0);
        $("#Concession").val("");
        $("#ConcessionAmt").val("");
    });

    $("#Concession").change(function () {

        var Concession = parseInt($("#Concession").val());

        if (Concession < 0) {
            alert("Concession should greater then  0");
            $("#Concession").val("")
            return;
        }


        var totalfee = parseInt($("#TotalFee").val());
        var lateFee = parseInt($("#LateFee").val());

        var netFee = totalfee + lateFee;

        var afterConcession = netFee - Concession;

        if (afterConcession < 0) {
            alert("Concession should less then  net fee");
            $("#Concession").val("")
            return;
        }


        $("#NetFee").val(afterConcession);
        $("#ConcessionAmt").val(Concession);
        $("#ReceiptAmt").val(0);
        $("#BalanceAmt").val(0);


    });

    $('#ReceiptAmt').change(function () {

        var ReceiptAmt = parseInt($(this).val());
        var netFee = parseInt($("#NetFee").val());

        $("#BalanceAmt").val(netFee - ReceiptAmt);
    });

    $("#btnCalculateFee").click(function () {
        var collectFeeAmt = 0;

        $("#section2Table tbody tr").each(function () {
            var checkBox = $(this).find('input[type="checkbox"]').is(':checked');
            //alert(checkBox);
            if (checkBox == true) {
                var fee = $(this).find(':input[type="number"]').val();
                collectFeeAmt = collectFeeAmt + Number(fee);

            }
        });

        $("#TotalCollectFee").val(collectFeeAmt);
        //$("#OldBalance2").val(Number($("#OldBalance").val()));

        $("#ReceiptAmt").val(collectFeeAmt);
        $("#TotalFee").val($("#allTotal").html());
        //var netFee = Number($("#TotalCollectFee").html()) + Number($("#LateFee").val()) + Number($("#OldBalance").val()) - Number($("#ConcessionAmt").val());
        var netFee = Number($("#TotalCollectFee").val()) + Number($("#LateFee").val()) - Number($("#ConcessionAmt").val());
        $("#NetFee").val(netFee);
        $("#ReceiptAmt").val(netFee);
        $("#BalanceAmt").val(Number($("#allTotal").html()) - collectFeeAmt);
        $("#Section3").removeAttr('hidden');
    });

    $("#btnSaveReceipt").click(function () {
        //

        var paymentmode = $("#PaymentMode").val();

        if (paymentmode == "Online") {
            var feeReceiptViewModel = {}
            var FeeHeadings = [];
            var FeeHeadingAmt = [];
          
            var collectedFeeAmount = [];
            feeReceiptViewModel.StudentId = $("#StudentName option:selected").val();
            feeReceiptViewModel.ClassId = $("#Class option:selected").val();
            
            feeReceiptViewModel.StudentName = $("#Name").val();


            $("#section2Table tbody tr ").each(function () {

                var FeeHeading = $(this).find("td:nth-child(2)").html();
                var total = $(this).find("td:nth-last-child(3)").html();
                var fee = $(this).find(':input[type="number"]').val();
                var checkBox = $(this).find('input[type="checkbox"]').is(':checked');
                
                if (checkBox == true) {
                    FeeHeadings.push(FeeHeading);
                    FeeHeadingAmt.push(total);
                    collectedFeeAmount.push(fee);
                }
            });

            if (FeeHeadings.length <= 0) {
                alert("Please select Fee Headings.")
                return;
            }
            if (Number($("#TotalFee").val()) <= 0) {
                alert("Total Fee can't 0")
                return;
            }


            if ($("#PaymentMode").val() == null) {
                if ($("#ConcessionAmt").val() != $("#TotalFee").val()) {
                    alert("Please select the payment mode");
                    $("#btnSaveReceipt").removeAttr('disabled');
                    return false;
                }
            }
            feeReceiptViewModel.FeeHeadings = FeeHeadings;
            feeReceiptViewModel.FeeHeadingAmt = FeeHeadingAmt;
            feeReceiptViewModel.TotalFee = $("#TotalFee").val();
            feeReceiptViewModel.OldBalance = $("#OldBalance").val();
            feeReceiptViewModel.LateFee = $("#LateFee").val();
            feeReceiptViewModel.Concession = $("#Concession").val();
            feeReceiptViewModel.ConcessionAmt = $("#ConcessionAmt").val();
            feeReceiptViewModel.NetFee = $("#NetFee").val();
            feeReceiptViewModel.ReceiptAmt = $("#ReceiptAmt").val();
            feeReceiptViewModel.BalanceAmt = $("#BalanceAmt").val();
            feeReceiptViewModel.PaymentMode = $("#PaymentMode").val();
            feeReceiptViewModel.BankName = $("#BankName").val();
            feeReceiptViewModel.CheckId = $("#CheckId").val();
            feeReceiptViewModel.Remark = $("#Remark").val();
            feeReceiptViewModel.Course = $("#Course").val();
            feeReceiptViewModel.DateTimeVal = $("#DateFrom").val();
            feeReceiptViewModel.collectedFeeAmt = collectedFeeAmount;



            $.ajax({
                url: '/Fee/AmountTransferGateway?isStudent=false',
                dataType: "json",
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(feeReceiptViewModel),
                async: true,
                processData: false,
                cache: false,
                success: function (data) {
                    if (branchdata == "Worldline") {

                        window.location.href = '@Url.Action("PaymentProcess", "Payment")';
                    }
                    else if (branchdata == "Razorpay ") {
                        window.location.href = '@Url.Action("RazorpayPaymentprocess", "Payment")';
                    }

                },
                error: function (xhr) {
                    window.location.reload();
                }
            });
        }
        else {
            var feeReceiptViewModel = {}
            var FeeHeadings = [];
            var FeeHeadingAmt = [];
            
            var collectedFeeAmount = [];
            feeReceiptViewModel.StudentId = $("#StudentName option:selected").val();
            feeReceiptViewModel.ClassName = $("#Class option:selected").val();
            
            feeReceiptViewModel.StudentName = $("#Name").val();


            $("#section2Table tbody tr").each(function () {

                var FeeHeading = $(this).find("td:nth-child(2)").html();
                var total = $(this).find("td:nth-last-child(3)").html();
                var fee = $(this).find(':input[type="number"]').val();
                var checkBox = $(this).find('input[type="checkbox"]').is(':checked');
                
                if (checkBox == true) {
                    FeeHeadings.push(FeeHeading);
                    FeeHeadingAmt.push(total);
                    collectedFeeAmount.push(fee);
                }
            });
            if (FeeHeadings.length <= 0) {
                alert("Please select Fee Headings.")
                return;
            }
            if (Number($("#TotalFee").val()) <= 0) {
                alert("Total Fee can't 0")
                return;
            }


            if ($("#PaymentMode").val() == null) {
                if ($("#ConcessionAmt").val() != $("#TotalFee").val()) {
                    alert("Please select the payment mode");
                    $("#btnSaveReceipt").removeAttr('disabled');
                    return false;
                }
            }
            feeReceiptViewModel.FeeHeadings = FeeHeadings;
            feeReceiptViewModel.FeeHeadingAmt = FeeHeadingAmt;
            feeReceiptViewModel.TotalFee = $("#TotalFee").val();
            feeReceiptViewModel.OldBalance = $("#OldBalance").val();
            feeReceiptViewModel.LateFee = $("#LateFee").val();
            feeReceiptViewModel.Concession = $("#Concession").val();
            feeReceiptViewModel.ConcessionAmt = $("#ConcessionAmt").val();
            feeReceiptViewModel.NetFee = $("#NetFee").val();
            feeReceiptViewModel.ReceiptAmt = $("#ReceiptAmt").val();
            feeReceiptViewModel.BalanceAmt = $("#BalanceAmt").val();
            feeReceiptViewModel.PaymentMode = $("#PaymentMode").val();
            feeReceiptViewModel.BankName = $("#BankName").val();
            feeReceiptViewModel.CheckId = $("#CheckId").val();
            feeReceiptViewModel.Remark = $("#Remark").val();
            feeReceiptViewModel.Course = $("#Course").val();
            feeReceiptViewModel.DateTimeVal = $("#DateFrom").val();
            feeReceiptViewModel.collectedFeeAmt = collectedFeeAmount;

            $.ajax({
                url: '/TransportFee/AddFeeReceipt',
                dataType: "json",
                type: "POST",
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(feeReceiptViewModel),
                async: true,
                processData: false,
                cache: false,
                success: function (data) {
                    alert("Fee Saved Successfully.")
                    if (data != null) {
                        window.location.href = "/AdmissionFee/TransportReceiptPreview?id=" + data.FeeReceiptId + "&ReceiptId=2";
                    }
                    window.location.reload();
                },
                error: function (xhr) {
                    window.location.reload();
                }
            });
        }


    });

    $("#btn_dueAmmount").click(function () {

        var dueAmount = parseFloat($("#OldBalance").val());

        if (dueAmount == 0) {
            alert("No Due");

            return;
        }
        var dueData = {};
        dueData.StudentId = $("#StudentName option:selected").val();
        dueData.Class = $("#Class").val();
        dueData.DuaAmmount = dueAmount;

        $.ajax({
            url: '/TransportFee/PayDueAmmount',
            dataType: "json",
            type: "POST",
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(dueData),
            async: true,
            processData: false,
            cache: false,
            success: function (data) {
                alert("due amount saved.");
            },
            error: function (xhr) {
                window.location.reload();
            }
        });



    });


});
