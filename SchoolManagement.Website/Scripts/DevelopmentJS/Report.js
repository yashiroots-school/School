

function PrintReport(id, rollno) {

    var termid = $("#TermID").val();
    var batchid = $("#BatchID").val();
    localStorage.setItem("StudentId", id);
    const randomString = generateRandomString(10);
    var url = '/Exam/PrintReportCard?id=' + randomString + '&batchId=' + batchid;
   /* var url = '/Exam/PrintReportCardCBSEBoard?id=' + randomString + '&batchId=' + batchid;*/
    //var url = '@Url.Action("PrintReportCard", "Exam",new { id = "__id__" })';
    var arr = [];
    var obj = {
        rollno: rollno,
        StudentId: parseInt(id)
    };
    arr.push(obj);
    localStorage.setItem("PrintIds", JSON.stringify(arr));
    localStorage.setItem("PrintTerm", termid);
    localStorage.setItem("PrintBatch", batchid);
    window.open(url, "_blank");
    // window.open(url);
}

function PrintAdmit(id, rollno) {

    var termid = $("#TermID").val();
    var batchid = $("#BatchID").val();
    localStorage.setItem("StudentId", id);
    const randomString = generateRandomString(10);
    var url = '/Exam/PrintAdmitCard?id=' + randomString + '&batchId=' + batchid;
    /* var url = '/Exam/PrintReportCardCBSEBoard?id=' + randomString + '&batchId=' + batchid;*/
    //var url = '@Url.Action("PrintReportCard", "Exam",new { id = "__id__" })';
    var arr = [];
    var obj = {
        rollno: rollno,
        StudentId: parseInt(id)
    };
    arr.push(obj);
    localStorage.setItem("PrintIds", JSON.stringify(arr));
    localStorage.setItem("PrintTerm", termid);
    localStorage.setItem("PrintBatch", batchid);
    window.open(url, "_blank");
    // window.open(url);
}

$(document).ready(function () {

    localStorage.setItem("PrintIds", "");
    localStorage.setItem("PrintTerm", "");
    localStorage.setItem("PrintBatch", "");
    var dataTable;
    // $("#TableStudent").DataTable(); dfdfdf
    $("#BtnShowReportStudent").on('click', function () {
        //debugger
        var classid = $("#ClassID").val();
        var sectionid = $("#SectionId").val();
        var obj = splitValue($("#TermID").val());
        if (obj == null || obj.id == undefined) {
            alert("Term is invalid");
            return;
        }
        var termId = obj.id;
        var fields = [
            { id: "#ClassID", name: "Class" },
            { id: "#SectionId", name: "Section" },
            { id: "#TermID", name: "Term" },
            { id: "#BatchID", name: "Batch" }

        ];
        var missingField = "";
        //if ($("#BatchID").val() != 0) {
        //    if ($("#TermID").val() == 0) {
        //        alert("Please select a term.");
        //    }
        //} else {

            for (var i = 0; i < fields.length; i++) {
                var fieldValue = $(fields[i].id).val();
                if (fieldValue === '0') {
                    missingField = fields[i].name;
                    break;
                }
            }
        //}

        if (missingField !== "") {
            alert("Please select a " + missingField + ".");
        }
        else {
            if (!dataTable) {
                dataTable = $('#TableStudent').DataTable({
                    columns: [
                        { title: 'SNo' },
                        { title: 'Student Name' },
                        { title: 'Class' },
                        { title: 'Section' },
                        { title: 'Batch' },
                        { title: 'Actions' }
                    ]
                });
            }
            $.ajax({
                url: "/Exam/GetStudentByClassSection",
                type: "GET",
                dataType: "json",
                contentType: "application/json;charset=UTF-8",
                data: { classId: classid, sectionId: sectionid, termId: termId, Batchid: $("#BatchID").val() },
                success: function (result) {
                    var data = [];
                    var serialNo = 1;
                    $.each(result, function (key, item) {
                        var A;
                        if (item.IsHold == false) { 
                            A = '<button class="btn btn-primary" onclick="showCustomAlert(' + item.StudentId + ',  ' + termId + ',  ' + $("#BatchID").val() + ',  ' + classid + ',1)" id="studentHold">Hold</button>';
                        }
                        else {
                            A = '<button class="btn btn-primary" onclick="showCustomAlert(' + item.StudentId + ',  ' + termId + ',  ' + $("#BatchID").val() + ',  ' + classid + ',0)" id="studentPublish">Publish</button>';
                        }
                        data.push([
                            serialNo,
                            item.StudentName,
                            item.ClassName,
                            item.SectionName,
                            item.BatchName,
                            '<button class="btn btn-primary" onclick="PrintReport(' + item.StudentId + ', ' + serialNo + ')">Print</button><button class="btn btn-primary" onclick="PrintAdmit(' + item.StudentId + ', ' + serialNo + ')">Admit Card</button>'+A+''
                             

                        ]);
                       

                        serialNo++;
                    });


                    dataTable.clear().rows.add(data).draw();
                },
                error: function (errormessage) {
                    console.log("Error:", errormessage); // Log any error message
                    alert(errormessage.responseText);
                }
            });

        }

    });



    $(".BtnPrintAll1").on('click', function () {
        let serialNumbers = [];
        var count = 1;
        var classid = $("#ClassID").val();
        var sectionid = $("#SectionId").val();
        //var termid = $("#TermID").val();
        //---x-rnik
        var obj = splitValue($("#TermID").val());
        var termId = obj.id;
        //---


        var batch = $("#BatchID").val();
        var fields = [
            { id: "#ClassID", name: "Class" },
            { id: "#SectionId", name: "Section" },
            { id: "#TermID", name: "Term" }
        ];
        var missingField = "";

        for (var i = 0; i < fields.length; i++) {
            var fieldValue = $(fields[i].id).val();
            if (fieldValue === '0') {
                missingField = fields[i].name;
                break;
            }
        }

        if (missingField !== "") {
            alert("Please select a " + missingField + ".");
        }
        else {
            $.ajax({
                url: "/Exam/GetStudentByClassSection",
                type: "GET",
                dataType: "json",
                contentType: "application/json;charset=UTF-8",
                //data: { classId: classid, sectionId: sectionid, testId: 1 }, //---x-rniks
                data: { classId: classid, sectionId: sectionid, termId: termId, Batchid: $("#BatchID").val() },
                success: function (result) {

                    $.each(result, function (key, data) {


                        //if (count < 5) {
                        var obj = {
                            rollno: count,
                            StudentId: data.StudentId
                        };
                        serialNumbers.push(obj);
                        count++;  // Increment the counter
                        // }
                    }
                    );
                    var termid = $("#TermID").val();
                    var batchid = $("#BatchID").val();
                    localStorage.setItem("PrintTerm", termid);
                    localStorage.setItem("PrintBatch", batchid);
                    localStorage.setItem('PrintIds', JSON.stringify(serialNumbers));
                    localStorage.setItem('IsClicked','true');
                    const randomString = generateRandomString(10);
                    var url = '/Exam/PrintReportCard?id=' + randomString + '&batchId=' + batchid;
                    window.open(url, "_blank");
                },
                error: function (errormessage) {
                    alert(errormessage.responseText);
                }
            });
        }

    });
    $("#BtnDownloadAll").on('click', async function () {
        const classid = $("#ClassID").val();
        const sectionid = $("#SectionId").val();
        const batchid = $("#BatchID").val();
        const obj = splitValue($("#TermID").val()); // assuming you split id/name here
        const termid = obj.id;

        const apiUrl = `/Exam/PrintStudentReportCardDataForCLass?termId=${termid}&BatchId=${batchid}&classId=${classid}&sectionId=${sectionid}`;

        try {
            const response = await fetch(apiUrl);

            if (!response.ok) throw new Error("Network response was not ok");

            const blob = await response.blob();
            const url = window.URL.createObjectURL(blob);

            const a = document.createElement("a");
            a.href = url;
            a.download = "ReportCard.pdf"; // Can be dynamic
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
        } catch (error) {
            console.error("Error downloading file:", error);
        }
    });

    //$("#BtnDownloadAll").on('click', function () {
    //    var classid = $("#ClassID").val();
    //    var sectionid = $("#SectionId").val();
    //    var batchid = $("#BatchID").val();
    //    //var termid1 = $("#TermID").val();
    //     var obj = splitValue($("#TermID").val());
    //    var termid = obj.id;
    //    const apiUrl = "/Exam/PrintStudentReportCardDataForCLass?termId=" + termid + "&BatchId=" + batchid + "&classId=" + classid + "&sectionId=" + sectionid + "";
    //    console.log(apiUrl)
    //   // fetch(apiUrl)
    //    fetch(apiUrl)
    //        .then(response => {
    //            if (!response.ok) throw new Error('Network response was not ok');
    //            return response.blob();
    //        })
    //        .then(blob => {
    //            const url = window.URL.createObjectURL(blob);
    //            const a = document.createElement('a');
    //            a.href = url;
    //            a.download = "ReportCard.pdf"; // or dynamic name
    //            document.body.appendChild(a);
    //            a.click();
    //            a.remove();
    //        })
    //        .catch(error => {
    //            console.error('Error downloading file:', error);
    //        });
    //});

    //$("#BtnDownloadAll").on('click', function () {
        
    //    document.getElementById("loader-overlay").style.display = "flex";
    //    let serialNumbers = [];
    //    var count = 1;
    //    var classid = $("#ClassID").val();
    //    var sectionid = $("#SectionId").val();
    //    var batchid = $("#BatchID").val();
    //    var termid = $("#TermID").val();
    //    var splitValues = splitValue(termid);
    //    const _Stid = parseInt(splitValues.id);
    //    var fields = [
    //        { id: "#ClassID", name: "Class" },
    //        { id: "#SectionId", name: "Section" },
    //        { id: "#TermID", name: "Term" },
    //        { id: "#BatchID", name: "Batch" }
    //    ];
    //    var missingField = "";

    //    for (var i = 0; i < fields.length; i++) {
    //        var fieldValue = $(fields[i].id).val();
    //        if (fieldValue === '0') {
    //            missingField = fields[i].name;
    //            break;
    //        }
    //    }

    //    if (missingField !== "") {
    //        alert("Please select a " + missingField + ".");
    //    }
    //    else {
    //        $.ajax({
    //            url: "/Exam/GetStudentByClassSection",
    //            type: "GET",
    //            dataType: "json",
    //            contentType: "application/json;charset=UTF-8",
    //            data: { classId: classid, sectionId: sectionid, TermId: _Stid, Batchid: batchid },
    //            success: function (result) {
    //                $.each(result, function (key, data) {
    //                    var obj = {
    //                        rollno: count,
    //                        StudentId: data.StudentId
    //                    };
    //                    serialNumbers.push(obj);
    //                    count++; 
    //                });
    //                console.log("IDs:"+ serialNumbers)
    //                var termid = $("#TermID").val();
    //                localStorage.setItem("PrintTerm", termid);
    //                localStorage.setItem('PrintIds', JSON.stringify(serialNumbers));
    //                localStorage.setItem("PrintBatch", batchid);
    //                const randomString = generateRandomString(10);
    //                var url = '/Exam/PrintReportCard?id=' + randomString + '&batchId=' + batchid;
    //                pdf();
    //            },
    //            error: function (errormessage) {
    //                alert(errormessage.responseText);
    //                document.getElementById("loader-overlay").style.display = "none";

    //            }
    //        });
    //    }

    //});
    $("#freezeButton").on('click', function () {

        // Retrieve the freezing ID stored in the data attribute
        var freezingId = $(this).data('freezingId');
        if (freezingId) {
            FreezeAndUnFreeze(false, freezingId)
        } else {
            FreezeAndUnFreeze(true)

        }
    });

    $("#BtnPublish").on('click', function () {


        var PublishId = $(this).data('PublishId');
        if (PublishId) {
            publish(false, PublishId)
        } else {
            publish(true)

        }
    });
});

function publish(Ispublish, PublishId) {

    var classid = $("#ClassID").val();
    var sectionid = $("#SectionId").val();
    var TermIdValue = $("#TermID").val();
    var BatchID = $("#BatchID").val();
    var fields = [
        { id: "#ClassID", name: "Class" },
        { id: "#SectionId", name: "Section" },
        { id: "#TermID", name: "Term" },


    ];
    var obj = splitValue(TermIdValue);
    if (obj == null || obj.id == undefined) {
        alert("Term is invalid");
        return;
    }
    var TermId = obj.id;
    var missingField = "";
    if ($("#BatchID").val() != 0) {
        if ($("#TermID").val() == 0) {
            alert("Please select a term.");
        }
    } else {
        for (var i = 0; i < fields.length; i++) {
            var fieldValue = $(fields[i].id).val();
            if (fieldValue === '0') {
                missingField = fields[i].name;
                break;
            }
        }
    }

    if (missingField !== "") {
        alert("Please select a " + missingField + ".");
    }
    else {

        if (Ispublish) {
            var publishObj = {
                classid: classid,
                sectionid: sectionid,
                TermId: TermId,
                BatchID: BatchID,
                Ispublish: Ispublish
            };
        } else {
            var publishObj = {
                PublishId: PublishId,
                classid: classid,
                sectionid: sectionid,
                TermId: TermId,
                BatchID: BatchID,
                Ispublish: Ispublish
            };
        }
        $.ajax({
            url: "/Exam/PublishUnpublish",
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(publishObj),
            success: function (result) {
                if (result.data) {
                    if (result.data.IsPublish) {
                        // Assuming the Publish ID is returned in result.data.Publishid
                        var PublishId = result.data.PublishId;
                        // Update button
                        var PublishButton = $('#BtnPublish');
                        PublishButton.text('UnBtnPublished');
                        PublishButton.prop('disabled', false);
                        PublishButton.html('UnPublished <i class="fa fa-unlock" aria-hidden="true"></i>');  // Change button text and icon
                        // Optionally store the Publish ID as data attribute if needed later
                        PublishButton.data('PublishId', PublishId);
                    }
                    else if (result.data == "msg") {
                        alert(result.msg);
                    }
                    else {
                        var PublishButton = $('#BtnPublish');
                        PublishButton.text('Publish');
                        PublishButton.html('Publish <i class="fa fa-lock" aria-hidden="true"></i>');
                        // Remove the freezing ID data attribute
                        PublishButton.removeData('PublishId');
                        PublishButton.prop('disabled', false);
                    }

                }
                else {
                    var PublishButton = $('#BtnPublish');
                    PublishButton.text('Publish');
                    PublishButton.html('Publish <i class="fa fa-lock" aria-hidden="true"></i>');
                    // Remove the freezing ID data attribute
                    PublishButton.removeData('PublishId');
                    PublishButton.prop('disabled', false);
                }


            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }


}
function FreezeAndUnFreeze(IsFreeze, freezingId) {

    var classid = $("#ClassID").val();
    var sectionid = $("#SectionId").val();
    var TermIdValue = $("#TermID").val();
    var BatchID = $("#BatchID").val();
    var fields = [
        { id: "#ClassID", name: "Class" },
        { id: "#SectionId", name: "Section" },
        { id: "#TermID", name: "Term" },


    ];
    var obj = splitValue(TermIdValue);
    if (obj == null || obj.id == undefined) {
        alert("Term is invalid");
        return;
    }
    var TermId = obj.id;
    var missingField = "";
    if ($("#BatchID").val() != 0) {
        if ($("#TermID").val() == 0) {
            alert("Please select a term.");
        }
    } else {
        for (var i = 0; i < fields.length; i++) {
            var fieldValue = $(fields[i].id).val();
            if (fieldValue === '0') {
                missingField = fields[i].name;
                break;
            }
        }
    }

    if (missingField !== "") {
        alert("Please select a " + missingField + ".");
    }
    else {

        if (IsFreeze) {
            var FreezeObj = {
                classid: classid,
                sectionid: sectionid,
                TermId: TermId,
                BatchID: BatchID,
                IsFreeze: IsFreeze
            };
        } else {
            var FreezeObj = {
                FreezeId: freezingId,
                classid: classid,
                sectionid: sectionid,
                TermId: TermId,
                BatchID: BatchID,
                IsFreeze: IsFreeze
            };
        }
        $.ajax({
            url: "/Exam/FreezeUnfreezeData",
            type: 'POST',
            dataType: "json",
            contentType: 'application/json',
            data: JSON.stringify(FreezeObj),

            success: function (result) {
                if (result.data) {
                    // Assuming the freezing ID is returned in result.data.freezingId
                    var freezingId = result.data.FreezeId;
                    // Update button
                    var freezeButton = $('#freezeButton');
                    freezeButton.text('UnFreezed');
                    freezeButton.prop('disabled', false);
                    freezeButton.html('UnFreezed <i class="fa fa-unlock" aria-hidden="true"></i>');  // Change button text and icon
                    // Optionally store the freezing ID as data attribute if needed later
                    freezeButton.data('freezingId', freezingId);
                    alert(result.msg)
                } else {
                    // Update button
                    var freezeButton = $('#freezeButton');
                    freezeButton.text('Freezing');
                    // Remove the freezing ID data attribute
                    freezeButton.removeData('freezingId');
                    freezeButton.prop('disabled', false);
                    alert(result.msg)
                }

            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    }


}



function generateRandomString(length) {
    const characters = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let randomString = '';

    for (let i = 0; i < length; i++) {
        const randomIndex = Math.floor(Math.random() * characters.length);
        randomString += characters.charAt(randomIndex);
    }

    return randomString;
}





var gradingCriteria = [];
async function fetchData(rollno, number) {

    $("#UnitTestRemark").hide();
    fetch('/Exam/AllGrade', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            gradingCriteria = data;
        })
        .catch(error => {
            return "";
            console.log("API call failed: " + error);
        });


    var studentName = "";
    var studentID = "";
    // Get the URL parameters
    const urlParams = new URLSearchParams(window.location.search);

    // Get the value of the "id" parameter
    const id = urlParams.get('id');
    const TermId = localStorage.getItem("PrintTerm");
    const BacthId = localStorage.getItem("PrintBatch");



    const selectedValue = TermId;
    const splitValues = splitValue(selectedValue);


    if (splitValues) {

        if (splitValues.name === "UT") {
            $("#UnitName").text(splitValues.id);

            $("#UnitRecord").show();
            $("#MarksTable").hide();
            $("#TermRecord").hide();
            $("#PreBoardRecord").hide();
            $("#coScholasticTable").show();
            // $("#coScholasticPreBoardTable").hide();

            if (splitValues.id == 1) {
                $(".headerTerm1Column").show();
                $(".headerTerm2Column").hide();
            } else {
                $(".headerTerm1Column").hide();
                $(".headerTerm2Column").show();
            }
            $(".headerPreBoard1Column").hide();
            $(".headerPreBoard2Column").hide();

        }
        else if (splitValues.name === "Term") {
            $(".ft15").css("font-size", "11px");
            $("#TermName").text(splitValues.id == "3" ? "1" : "2");
            $("#UnitRecord").hide();
            $("#MarksTable").hide();
            $("#TermRecord").show();
            $("#PreBoardRecord").hide();
            $("#coScholasticTable").show();
            //$("#coScholasticPreBoardTable").hide();
            if (splitValues.id == 3) {
                $(".headerTerm1Column").show();
                $(".headerTerm2Column").hide();
            } else {
                $(".headerTerm1Column").hide();
                $(".headerTerm2Column").show();
            }
            $(".headerPreBoard1Column").hide();
            $(".headerPreBoard2Column").hide();

        }
        else if (splitValues.name === "PreBoard") {
            $(".ft15").css("font-size", "11px");
            $("#PreBoardTermName").text(splitValues.id == "7" ? "1" : "2");
            $("#UnitRecord").hide();
            $("#MarksTable").hide();
            $("#TermRecord").hide();
            $("#PreBoardRecord").show();
            $("#coScholasticTable").show();
            $(".headerTerm1Column").hide();
            $(".headerTerm2Column").hide();
            //$("#coScholasticPreBoardTable").show()
            if (splitValues.id == 7) {

                $(".headerPreBoard1Column").show();
                $(".headerPreBoard2Column").hide();

            } else {
                $(".headerPreBoard1Column").hide();
                $(".headerPreBoard2Column").show();
            }

        }
        else if (splitValues.name === "All") {
            // $("#TermName").text(splitValues.id == "3" ? "1" : "2");
            $(".ft15").css("font-size", "10px");

            $("#UnitRecord").hide();
            $("#MarksTable").show();
            $("#TermRecord").hide();
            $("#coScholasticTable").show();
            $("#teacherRemark").show();
            $("#UnitTestRemark").hide();

            $("#PreBoardRecord").hide();
            $("#PreBoardRecord").hide();
            $(".preBoardResult").hide();

            $("#coScholasticPreBoardTable").hide()


        }
        else if (splitValues.name === "Selection") {
            $("#TermName").text(splitValues.id == "5" ? "1" : "2");
            $(".ft15").css("font-size", "10px");

            $("#UnitRecord").hide();
            $("#MarksTable").show();
            $("#TermRecord").hide();
            $("#coScholasticTable").show();
            $("#teacherRemark").show();
            $("#UnitTestRemark").hide();

            $("#PreBoardRecord").hide();
            $("#PreBoardRecord").hide();
            $(".preBoardResult").hide();

            $("#coScholasticPreBoardTable").hide()


        }


    }
    console.log('Report')
    console.log(BacthId)
    //New
    fetch('/Exam/PrintReportCardData?studentId=' + number + '&termId=' + parseInt(splitValues.id) + '&BatchId=' + parseInt(BacthId), {
        //fetch('/Exam/PrintReportCardCBSEBoard?studentId=' + number + '&termId=' + parseInt(splitValues.id) + '&BatchId=' + parseInt(splitValues.id), {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            if (data.subjectDatas && Array.isArray(data.subjectDatas)) {
                var subjectDatas = data.subjectDatas;
                var subjectDataLength = subjectDatas.length;
                var tableBody = document.getElementById('MarksTable').getElementsByTagName('tbody')[0];
                //$(".Term1TheoryMaxMarkTotal").text("(" + (data.totalResult.Term1TheoryMaxTotal / subjectDataLength) + ")");
                //$("#Term1PracticalMaxMarkTotal").text("(" + (data.totalResult.Term1PracticalMaxTotal / subjectDataLength) + ")");
                //$("#Term2TheoryMaxMarkTotal").text("(" + (data.totalResult.Term2TheoryMaxTotal/subjectDataLength) + ")");
                //$("#Term2PracticalMaxMarkTotal").text("(" + (data.totalResult.Term2PracticalMaxTotal/subjectDataLength) + ")");
                //$("#UT1MaxMarkTotal").text("(" + (data.totalResult.UT1MaxTotal/subjectDataLength) + ")");
                //$("#UT2MaxMarkTotal").text("(" + (data.totalResult.UT2MaxTotal/subjectDataLength) + ")");
                //$("#UTTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.UT2MaxTotal) + parseInt(data.totalResult.UT1MaxTotal))/subjectDataLength) + ")");
                //$("#Term1TotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Term1TheoryMaxTotal) + parseInt(data.totalResult.Term1PracticalMaxTotal))/subjectDataLength) + ")");
                //$("#Term2TotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Term2TheoryMaxTotal) + parseInt(data.totalResult.Term2PracticalMaxTotal))/subjectDataLength) + ")");
                //// Get all rows from the table body




                var rows = tableBody.getElementsByTagName('tr');

                // Skip the first two rows and remove the rest
                for (var i = rows.length - 1; i >= 2; i--) {

                    tableBody.removeChild(rows[i]);
                }

                $("#StudentName").text(data.studentName);
                $("#FatherName").text(data.fatherName);
                $("#MotherName").text(data.motherName);
                $("#AcademicYear").text(data.academicYear);
                $("#ScholarNo").text(data.scholarNo);
                $("#DateOfBirth").text(data.dateOfBirth);
                $("#RollNo").text(rollno);
                $("#ClassSection").text(data.className + "(" + data.sectionName + ")");
                if (data.className == "Class-XI Science" || data.className == "Class-XI Commerce" || data.className == "Class-XII Commerce" || data.className == "Class-XII Science") {
                    $("#dgrade").text("34.4 & Below");
                }
                else {
                    $("#dgrade").text("32.4 & Below");
                }
                const allowedClassIds = [414, 415, 416, 417]; // List of allowed class IDs

                if (allowedClassIds.includes(data.classID)) {
                    $("#ClassSection").text("(" + data.className + ")");
                } else {
                    $("#ClassSection").text(data.className + "(" + data.sectionName + ")");
                }

                $("#Attendance").text(data.attandence); // Check the checkbox
                var imageUrl = "/WebsiteImages/StaffSignature/" + data.staffSignatureLink;

                // Set the src attribute of the image element using jQuery
                $("#teachersignature").attr("src", imageUrl);

                //var UT1Total = 0;
                //var UT2Total = 0;
                //var UTAllTotal = 0;
                //var TheoryTotalT1 = 0;
                //var PracticalTotalT1 = 0;
                //var T1AllTotal = 0;
                //var TheoryTotalT2 = 0;
                //var PracticalTotalT2 = 0;
                //var T2AllTotal = 0;
                //var OverallAllTotal = 0;


                //all data Term1 term2, unit1,unit2
                subjectDatas.forEach(subjectData => {
                    // 
                    var newRow = document.createElement('tr');

                    // Create the "subject" <td> with "scope" and "class" attributes
                    var subjectTd = document.createElement('td');
                    subjectTd.textContent = subjectData.Subject;
                    subjectTd.setAttribute('scope', 'row');
                    subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                    newRow.appendChild(subjectTd);
                    // Add the data to the respective <td> elements
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.MarksUT1) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.MarksUT2) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalMarks) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarks) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarks) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarks) + '</td>';
                    newRow.innerHTML += '<td>' + subjectData.GradeUT1 + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksUT2) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksUT2) + '</td>';
                    newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksUT2) + '</td>';
                    newRow.innerHTML += '<td>' + subjectData.GradeUT2 + '</td>';
                    // if (splitValues.name == "All") {
                    if (subjectData.FinalGrade == "D") {
                        newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalMarksBothUTs) + '<span style="color:red"> *</span></td>';
                    }
                    else {
                        newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalMarksBothUTs) + '</td>';
                    }


                    newRow.innerHTML += '<td>' + subjectData.FinalGrade + '</td>';

                    //
                    tableBody.appendChild(newRow);
                });



                // Add the row for the total
                var totalRow = document.createElement('tr');
                // Create the "Total" <td> with "scope" and "class" attributes and bold font
                var totalTd = document.createElement('td');
                totalTd.textContent = 'Total';
                totalTd.setAttribute('scope', 'row');
                totalTd.setAttribute('class', 'text-uppercase text-start ps-1');
                totalTd.style.fontWeight = 'bold';
                totalRow.appendChild(totalTd);
                // Add the data to the respective <td> elements using <b> tags for bold text
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.UT1Total) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.UT2Total) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.UTAllTotal) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.TheoryTotalT1) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.PracticalTotalT1) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.T1AllTotal) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + data.totalResult.T1Grade + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.TheoryTotalT2) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.PracticalTotalT2) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.T2AllTotal) + '</b></td>';
                totalRow.innerHTML += '<td><b>' + data.totalResult.T2Grade + '</b></td>';
                if (data.totalResult.FinalGrade == "D") {
                    // m if (data.totalResult.OverallGrade == "D") {
                    totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.OverallAllTotal) + '</b><span style="color:red"> *</span></td>';
                    //m totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.OverallAllTotal) + '</b></td>';
                } else {
                    totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.OverallAllTotal) + '</b></td>';
                }
                //m
                totalRow.innerHTML += '<td><b>' + data.totalResultPercentage.OverallGrade + '</b></td>';
                // totalRow.innerHTML += '<td><b>' + data.totalResult.OverallGrade + '</b></td>';
                // totalRow.innerHTML += '<td><b>' + "" + '</b></td>';
                var isPass = true;
                var cnt = 0;
                if (id.splitValue != "All") {

                    subjectDatas.forEach(subjectData => {
                        if (subjectData.GradeUT1 == "" || subjectData.GradeUT1 == "D"
                            || subjectData.MarksUT1Grade == "" || subjectData.MarksUT1Grade == "D"
                            || subjectData.MarksUT2Grade == "" || subjectData.MarksUT2Grade == "D"
                            || subjectData.GradeUT2 == "" || subjectData.GradeUT2 == "D") {
                            isPass = false;
                            cnt++;

                        } else {
                            isPass = true;
                        }
                    });
                    if (cnt == 0) {
                        $("#result").text("Pass");
                        $("#RankInClass").text(" Rank in Class :" + data.Rank);
                        $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                    } else {
                        $("#result").text("");
                        $("#RankInClass").text("");
                        $("#overallGrade").text("")
                    }
                }
                subjectDatas.forEach(subjectData => {
                    if (subjectData.FinalGrade == "D") {
                        isPass = false;
                        cnt++;
                    }
                    else {
                        isPass = true; //edit by megha
                        cnt = 0;
                    }
                });

                if (cnt == 0) {
                    $("#result").text("Pass");
                    $("#RankInClass").text(" Rank in Class :" + data.attandence);
                    $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                }
                else {
                    $("#result").text("");
                    $("#RankInClass").text("");
                    $("#overallGrade").text("")
                }
                tableBody.appendChild(totalRow);
                var percentagesRow = createAdditionalRow("Percentage", data.totalResultPercentage.UT1Total + "%",
                    data.totalResultPercentage.UT2Total + "%",
                    data.totalResultPercentage.UTAllTotal + "%",
                    data.totalResultPercentage.TheoryTotalT1 + "%",
                    data.totalResultPercentage.PracticalTotalT1 + "%",
                    data.totalResultPercentage.T1AllTotal + "%",
                    data.totalResultPercentage.T1Grade,
                    data.totalResultPercentage.TheoryTotalT2 + "%",
                    data.totalResultPercentage.PracticalTotalT2 + "%",
                    data.totalResultPercentage.T2AllTotal + "%",
                    data.totalResultPercentage.T2Grade,
                    data.totalResultPercentage.OverallAllTotal + "%",
                    // "",
                    data.totalResultPercentage.OverallGrade,
                    true
                );
                tableBody.appendChild(percentagesRow);
                // $("#overallGrade").text(data.totalResultPercentage.UT2TotalGrade);
                $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                $("#teacherRemarkText").text(data.Remark);
                //alert($("#overallGrade").text());
                // if (parseFloat(data.totalResultPercentage.OverallAllTotal) < 33) {
                //megha
                //if ($("#overallGrade").text() === '') {
                //    $("#result").text("");


                //} else {
                //    $("#result").text("Pass"); $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                //    // $("#promotedClass").text(data.promotedClass);

                //}
                var cnt = 0;
                var isPass = true;
                subjectDatas.forEach(subjectData => {
                    // if (//subjectData.GradeUT1 == "" || subjectData.GradeUT1 == "D"
                    // || subjectData.MarksUT1Grade == "" || subjectData.MarksUT1Grade == "D"
                    // || subjectData.MarksUT2Grade == "" || subjectData.MarksUT2Grade == "D"||
                    // subjectData.GradeUT2 == "" || subjectData.GradeUT2 == "D") {
                    if (subjectData.FinalGrade == "" || subjectData.FinalGrade == "D") {
                        isPass = false;
                        cnt++;
                        //  return;
                    } else {
                        isPass = true; //edit by upendra
                    }
                });
                if (cnt == 0) {
                    // if (isPass) {
                    $("#result").text("Pass");
                    $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                } else {
                    $("#result").text("");
                    $("#overallGrade").text("")
                }
                //megha
                //if ($.inArray('D', subjectDatas) != -1) {
                //    $("#result").text("");
                //}
                //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                //        getOptionalGradeForAllReports(optionalSubjectData.MarksUT1Grade), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2Grade)), " ",
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), false);
                //    tableBody.appendChild(moralScienceRowUT1);
                //});  





                //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                //    
                //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                //        getOptionalGradeForAllReports(optionalSubjectData.MarksUT1Grade), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2Grade)), " ",
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), false);
                //    tableBody.appendChild(moralScienceRowUT1);
                //}); 
                //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                //    
                //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                //        getOptionalGradeForAllReports(optionalSubjectData.MarksUT1Grade), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2Grade)), " ",
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), false);
                //    tableBody.appendChild(moralScienceRowUT1);
                //}); 

                //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                //    
                //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject,
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1Grade)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2Grade)), " ",
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                //            getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarks)),
                //            getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)), false);
                //       // getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), false);
                //    tableBody.appendChild(moralScienceRowUT1);
                //});


                data.optionalSubjectDatas.forEach(optionalSubjectData => {

                    if (optionalSubjectData.MarksUT1Grade == null) {
                        optionalSubjectData.MarksUT1Grade = "-";
                    }
                    if (optionalSubjectData.MarksUT1 == null) {
                        optionalSubjectData.MarksUT1 = "-";
                    }
                    if (optionalSubjectData.MarksUT2 == null) {
                        optionalSubjectData.MarksUT2 = "-";
                    }
                    if (optionalSubjectData.TheoryMarks == null) {
                        optionalSubjectData.TheoryMarks = "-";
                    }
                    if (optionalSubjectData.MarksUT1Grade == null) {
                        optionalSubjectData.MarksUT1Grade = "-";
                    }
                    if (optionalSubjectData.MarksUT1Grade == null) {
                        optionalSubjectData.MarksUT1Grade = "-";
                    }
                    if (optionalSubjectData.PracticalMarks == null) {
                        optionalSubjectData.PracticalMarks = "-";
                    }
                    if (optionalSubjectData.TheoryMarks == null) {
                        optionalSubjectData.TheoryMarks = "-";
                    }
                    if (optionalSubjectData.GradeUT1 == null) {
                        optionalSubjectData.GradeUT1 = "-";
                    }
                    if (optionalSubjectData.TheoryMarksUT2 == null) {
                        optionalSubjectData.TheoryMarksUT2 = "-";
                    }
                    if (optionalSubjectData.GradeUT2 == null) {
                        optionalSubjectData.GradeUT2 = "-";
                    }
                    if (optionalSubjectData.TotalObtainedMarksUT2 == null) {
                        optionalSubjectData.TotalObtainedMarksUT2 = "-";
                    }
                    //var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject,

                    //    getOptionalGradeForAllReports((optionalSubjectData.MarksUT1Grade)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                    //  //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),//
                    //    //" ",

                    //   getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)),
                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), 

                    //    getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                    //    " ",

                    //  //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2))
                    //   " "
                    //        , false);
                    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject,

                        getOptionalGradeForAllReports((optionalSubjectData.MarksUT1Grade)),
                        getOptionalGradeForAllReports((optionalSubjectData.MarksUT2Grade)),
                        " ",
                        getOptionalGradeForAllReports((optionalSubjectData.TheoryMarks)),
                        getOptionalGradeForAllReports((optionalSubjectData.PracticalMarks)),
                        //getOptionalGradeForAllReports((optionalSubjectData.MarksUT1)),
                        // getOptionalGradeForAllReports((optionalSubjectData.MarksUT2)),
                        //getOptionalGradeForAllReports((optionalSubjectData.TheoryMarks)),
                        //getOptionalGradeForAllReports((optionalSubjectData.PracticalMarks)),
                        //getOptionalGradeForAllReports((optionalSubjectData.GradeUT1)),
                        //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),//
                        //" ",

                        getOptionalGradeForAllReports((optionalSubjectData.TotalObtainedMarks)),
                        getOptionalGradeForAllReports((optionalSubjectData.GradeUT1)),
                        getOptionalGradeForAllReports((optionalSubjectData.TheoryMarksUT2)),
                        getOptionalGradeForAllReports((optionalSubjectData.PracticalMarksUT2)),
                        getOptionalGradeForAllReports((optionalSubjectData.GradeUT2)),

                        getOptionalGradeForAllReports((optionalSubjectData.TotalObtainedMarksUT2)),
                        //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2))
                        " ",
                        " "
                        , false);

                    //if (data.className == "KG-I" || data.className == "KG-II" || data.className == "Nursery") {
                    //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject,
                    //        getOptionalGradeForAllReports("-"),
                    //        getOptionalGradeForAllReports("-"),
                    //        getOptionalGradeForAllReports("-"),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                    //        //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),//
                    //        //" ",

                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)),
                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)),

                    //        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                    //        " ",

                    //        //  getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2))
                    //        " "
                    //        , false);
                    //}
                    tableBody.appendChild(moralScienceRowUT1);
                });

                ///comment
                //var moralScienceRow = createAdditionalRow("Moral Science", "-", "-", "", (optionalSubjectData.MarksUT1Grade), "-", "-", "-", "-", "-", "-", "-", "", "", false);
                //tableBody.appendChild(moralScienceRow);

                //var gkRow = createAdditionalRow("G.K", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(gkRow);

                //var drawingRow = createAdditionalRow("Drawing", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(drawingRow);

                //var supwRow = createAdditionalRow("SUPW", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(supwRow); 
                //}); 
                //comment
                if (splitValues.name == "Term" || splitValues.name == "PreBoard" || splitValues.name == "UT" || splitValues.name == "Promotion" || splitValues.name == "Selection") {
                    console.log("coscholasticAreaDatas1: ", data.coscholasticAreaDatas);

                    if (manageCoScholasticTable(data.coscholasticAreaDatas, splitValues.id)) {
                        addRows(data.coscholasticAreaDatas, splitValues.id);
                    } else {
                        $("#coScholasticTable").hide();
                    }

                }

                if (splitValues.name == "All") {

                    console.log("coscholasticAreaDatas2: ", data.coscholasticAreaDatas);
                    console.log(manageCoScholasticTable(data.coscholasticAreaDatas, 10));
                    if (manageCoScholasticTable(data.coscholasticAreaDatas, 10)) {
                        addRows(data.coscholasticAreaDatas, 10);
                    } else {
                        $("#coScholasticTable").hide();
                    }

                }
                //For Unit 1                
                if (splitValues.id === "1") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    // $("#UTMaxMarkTotal").text("(" + (data.totalResult.UT1MaxTotal / subjectDataLength) + ")");
                    //
                    for (let i = 0; i < subjectDatas.length; i++) {
                        $("#UTMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksUT1) + ")");
                        break;
                    }
                    var tableBodyUT1 = document.getElementById('UnitRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyUT1.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyUT1.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        subjectTd.textContent = subjectData.Subject + "(" + subjectData.MaxMarksUT1 + ")";
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.MarksUT1) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.MarksUT1Grade + '</td>';

                        tableBodyUT1.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.UT1Total) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.UT1TotalGrade + '</b></td>';
                    tableBodyUT1.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage", data.totalResultPercentage.UT1Total + "%",
                        data.totalResultPercentage.UT1TotalGrade, true
                    );
                    tableBodyUT1.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResult.UT1TotalGrade);
                    // if (parseFloat(data.totalResultPercentage.UT1Total) < 33) {
                    if ($("#overallGrade").text() === '') {
                        $("#result").text("");
                        //  $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    var cnt = 0;
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.MarksUT1Grade == "" || subjectData.MarksUT1Grade == "D") {
                            isPass = false;
                            cnt++;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (cnt == 0) {
                        //if (isPass) {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResultPercentage.MarksUT1Grade);
                    } else {
                        $("#result").text("");
                        $("#overallGrade").text("")
                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject + "(" + optionalSubjectData.MaxMarksUT1 + ")",
                            getOptionMarkGrade(optionalSubjectData.MarksUT1), optionalSubjectData.MarksUT1Grade, false);
                        tableBodyUT1.appendChild(moralScienceRowUT1);
                    });
                    //var moralScienceRowUT1 = createAdditionalRow("Moral Science", "-", "-", false);
                    //tableBodyUT1.appendChild(moralScienceRowUT1);

                    //var gkRowUT1 = createAdditionalRow("G.K", "-", "-", false);
                    //tableBodyUT1.appendChild(gkRowUT1);

                    //var drawingRowUT1 = createAdditionalRow("Drawing", "-", "-", false);
                    //tableBodyUT1.appendChild(drawingRowUT1);

                    //var supwRowUT1 = createAdditionalRow("SUPW", "-", "-", false);
                    //tableBodyUT1.appendChild(supwRowUT1);

                }
                //For Unit 2
                else if (splitValues.id === "2") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$("#UTMaxMarkTotal").text("(" + (data.totalResult.UT2MaxTotal / subjectDataLength) + ")");
                    for (let i = 0; i < subjectDatas.length; i++) {
                        $("#UTMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksUT2) + ")");
                        break;
                    }

                    var tableBodyUT1 = document.getElementById('UnitRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyUT1.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyUT1.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        subjectTd.textContent = subjectData.Subject + "(" + subjectData.MaxMarksUT2 + ")";
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.MarksUT2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.MarksUT2Grade + '</td>';

                        tableBodyUT1.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.UT2Total) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.UT2TotalGrade + '</b></td>';
                    tableBodyUT1.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage", data.totalResultPercentage.UT2Total + "%",
                        data.totalResultPercentage.UT2TotalGrade, true
                    );
                    tableBodyUT1.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResult.UT2TotalGrade);
                    //if (parseFloat(data.totalResultPercentage.UT2Total) < 33) {
                    if ($("#overallGrade").text() === '') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        //  $("#promotedClass").text(data.promotedClass);

                    }
                    var cnt = 0;
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.MarksUT2Grade == "" || subjectData.MarksUT2Grade == "D") {
                            isPass = false;
                            cnt++;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (cnt == 0) {
                        // if (isPass) {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResult.UT2TotalGrade);
                    } else {
                        $("#result").text("");
                        $("#overallGrade").text("")
                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT2 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.MarksUT2),
                            optionalSubjectData.MarksUT2Grade, false);
                        tableBodyUT1.appendChild(moralScienceRowUT2);
                    });

                }
                //For Term 1
                else if (splitValues.id === "3" && splitValues.name === "Term") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$(".Term1TheoryMaxMarkTotal").text("(" + (data.totalResult.Term1TheoryMaxTotal / subjectDataLength) + ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Term1PracticalMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Term1TheoryMaxTotal) + parseInt(data.totalResult.Term1PracticalMaxTotal))/subjectDataLength) + ")");

                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".Term1TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm1Theory) + ")");
                        $("#Term1TPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm1Practical) + ")");
                        $("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksTerm1Theory) + parseInt(subjectDatas[0].MaxMarksTerm1Practical))) + ")");
                        break;
                    }

                    var tableBodyTerm1 = document.getElementById('TermRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyTerm1.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyTerm1.removeChild(rows[i]);
                    }
                    //m
                    //+

                    subjectDatas.forEach(subjectData => {

                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksTerm1Theory + subjectData.MaxMarksTerm1Practical;
                        subjectTd.textContent = subjectData.Subject + "(" + totalSubNumber + ")";
                        // subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarks) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarks) + '</td>';
                        // newRowUT1.innerHTML += '<td>' + (parseFloat(checkForZero(subjectData.TheoryMarks)) + (parseFloat(checkForZero(subjectData.PracticalMarks))) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarks) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradeUT1 + '</td>';


                        tableBodyTerm1.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.TheoryTotalT1) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PracticalTotalT1) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + (parseFloat(checkForZero(data.totalResult.TheoryTotalT1)) + parseFloat(checkForZero(data.totalResult.PracticalTotalT1))) + '</b></td>';
                    // totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.T1AllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.T1Grade + '</b></td>';
                    tableBodyTerm1.appendChild(totalRowUT1);
                    //totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.TheoryTotalT2) + '</b></td>';
                    //totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PracticalTotalT2) + '</b></td>';
                    //totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.T2AllTotal) + '</b></td>';
                    //totalRowUT1.innerHTML += '<td><b>' + data.totalResult.T2Grade + '</b></td>';
                    //tableBodyTerm2.appendChild(totalRowUT1);
                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalT1 + "%",
                        data.totalResultPercentage.PracticalTotalT1 + "%",
                        data.totalResultPercentage.T1AllTotal + "%",
                        data.totalResultPercentage.T1Grade, true
                    );
                    tableBodyTerm1.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResultPercentage.T1Grade);

                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT1) < 33) {
                    if ($("#overallGrade").text() === '') {
                        $("#result").text("");
                        //$("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        // $("#promotedClass").text(data.promotedClass);
                    }
                    var cnt = 0;
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {

                        if (subjectData.T1Grade == "" || subjectData.T1Grade == "D") {
                            isPass = false;
                            cnt++;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (cnt == 0) {
                        //if (ispass) {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResult.T1Grade);
                    } else {
                        $("#result").text("");
                        $("#overallGrade").text("")
                    }
                    //megha original
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarks),
                            getOptionMarkGrade(optionalSubjectData.PracticalMarks), getOptionMarkGrade(optionalSubjectData.TotalObtainedMarks),
                            optionalSubjectData.GradeUT1
                            // " "
                            , false);
                        tableBodyTerm1.appendChild(moralScienceRowUT1);
                    });
                    //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                    //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksUT1),
                    //        getOptionMarkGrade(optionalSubjectData.PracticalMarksUT1), optionalSubjectData.GradeUT1, getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksUT1),
                    // false);
                    //tableBodyTerm1.appendChild(moralScienceRowUT1);
                    //});
                    // data.optionalSubjectDatas.forEach(optionalSubjectData => {
                    //     var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TotalObtainedMarks),
                    //         getOptionMarkGrade(optionalSubjectData.PracticalMarks), getOptionMarkGrade(optionalSubjectData.TheoryMarks),
                    //        optionalSubjectData.GradeUT1, false);
                    //    tableBodyTerm1.appendChild(moralScienceRowUT1);
                    //});

                }
                else if (splitValues.id === "4" && splitValues.name === "Term") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".Term1TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm2Theory) + ")");
                        $("#Term1PracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm2Practical) + ")");
                        $("#Term1TotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksTerm2Theory) + parseInt(subjectDatas[0].MaxMarksTerm2Practical))) + ")");

                        $("#Term1TPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm2Practical) + ")");
                        break;
                    }

                    //$(".Term1TheoryMaxMarkTotal").text("(" + (data.totalResult.Term2TheoryMaxTotal / subjectDataLength) + ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Term2PracticalMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Term2TheoryMaxTotal) + parseInt(data.totalResult.Term2PracticalMaxTotal)) / subjectDataLength) + ")");

                    var tableBodyTerm2 = document.getElementById('TermRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyTerm2.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyTerm2.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksTerm2Theory + subjectData.MaxMarksTerm2Practical;
                        subjectTd.textContent = subjectData.Subject + "(" + totalSubNumber + ")";
                        // subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksUT2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksUT2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksUT2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradeUT2 + '</td>';

                        tableBodyTerm2.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.TheoryTotalT2) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PracticalTotalT2) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.T2AllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.T2Grade + '</b></td>';
                    tableBodyTerm2.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalT2 + "%",
                        data.totalResultPercentage.PracticalTotalT2 + "%",
                        data.totalResultPercentage.T2AllTotal + "%",
                        data.totalResultPercentage.T2Grade, true
                    );
                    tableBodyTerm2.appendChild(percentagesRowUT1);

                    $("#overallGrade").text(data.totalResultPercentage.T2Grade);
                    //// if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    //if ($("#overallGrade").text() === '') {
                    //    $("#result").text("");
                    //    // $("#teacherRemark").hide();
                    //} else {
                    //    $("#result").text("Pass");
                    //    // $("#promotedClass").text(data.promotedClass);

                    //}

                    var cnt = 0;
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.GradeUT2 == "" || subjectData.GradeUT2 == "D") {
                            // if (subjectData.T2Grade == "" || subjectData.T2Grade== "D") {

                            isPass = false;
                            cnt++;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (cnt == 0) {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResultPercentage.T2Grade);
                    } else {
                        $("#result").text("");
                        $("#overallGrade").text("")
                        // $("#overallGrade").innerHTML = "";
                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksUT2),
                            getOptionMarkGrade(optionalSubjectData.PracticalMarksUT2), getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksUT2),
                            //" ",
                            optionalSubjectData.GradeUT2,
                            false);
                        tableBodyTerm2.appendChild(moralScienceRowUT1);
                    });

                    // data.optionalSubjectDatas.forEach(optionalSubjectData => {
                    //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarks),
                    //        getOptionMarkGrade(optionalSubjectData.PracticalMarks), getOptionMarkGrade(optionalSubjectData.TotalObtainedMarks),
                    //        optionalSubjectData.GradeUT1
                    //        // " "
                    //        , false);
                    //    tableBodyTerm1.appendChild(moralScienceRowUT1);
                    //});
                    //data.optionalSubjectDatas.forEach(optionalSubjectData => {
                    //    
                    //    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject,
                    //        optionalSubjectData.GradeUT2,
                    //        getOptionMarkGrade(optionalSubjectData.PracticalMarksUT2),
                    //        getOptionMarkGrade(optionalSubjectData.TheoryMarksUT2),
                    //        getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksUT2),
                    //        false);
                    //    tableBodyTerm2.appendChild(moralScienceRowUT1);
                    //});


                }
                else if (splitValues.id === "7" && splitValues.name === "PreBoard" && _Stid != "10") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$(".Term1TheoryMaxMarkTotal").text("(" + data.totalResult.Pre1TheoryMaxTotal + ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Pre1PracticalMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Pre1TheoryMaxTotal) + parseInt(data.totalResult.Pre1PracticalMaxTotal)) / subjectDataLength) + ")");

                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".Term1TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPre1Theory) + ")");
                        $("#Term1TPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPre1Practical) + ")");
                        $("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksPre1Theory) + parseInt(subjectDatas[0].MaxMarksPre1Practical))) + ")");
                        break;
                    }


                    var tableBodyPreBoard1 = document.getElementById('PreBoardRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyPreBoard1.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyPreBoard1.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksPre1Practical + subjectData.MaxMarksPre1Theory;
                        subjectTd.textContent = subjectData.Subject + "(" + totalSubNumber + ")";
                        //  subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksPre1) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksPre1) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksPre1) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradePre1 + '</td>';

                        tableBodyPreBoard1.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre1TheoryMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre1PracticalMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre1AllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.Pre1Grade + '</b></td>';
                    tableBodyPreBoard1.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalPre1 + "%",
                        data.totalResultPercentage.PracticalTotalPre1 + "%",
                        data.totalResultPercentage.Pre1AllTotal + "%",
                        data.totalResultPercentage.Pre1Grade, true
                    );

                    tableBodyPreBoard1.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResultPercentage.Pre1Grade);
                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    if ($("#overallGrade").text() === '' || $("#overallGrade").text() === 'D') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksPre1), getOptionMarkGrade(optionalSubjectData.PracticalMarksPre1),
                            getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksPre1), optionalSubjectData.GradePre1, false);
                        tableBodyPreBoard1.appendChild(moralScienceRowUT1);
                    });

                }
                else if (splitValues.id === "8" && splitValues.name === "PreBoard" && _Stid != "10") {

                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$(".Term1TheoryMaxMarkTotal").text("(" + (data.totalResult.Pre2TheoryMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Pre2PracticalMaxTotal / subjectDataLength) + ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Pre2TheoryMaxTotal) + parseInt(data.totalResult.Pre2PracticalMaxTotal)) / subjectDataLength) + ")");


                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".Term1TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPre2Theory) + ")");
                        $("#Term1TPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPre2Practical) + ")");
                        $("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksPre2Theory) + parseInt(subjectDatas[0].MaxMarksPre2Practical))) + ")");
                        break;
                    }

                    var tableBodyPreBoard1 = document.getElementById('PreBoardRecord').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyPreBoard1.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyPreBoard1.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksPre2Practical + subjectData.MaxMarksPre2Theory;
                        subjectTd.textContent = subjectData.Subject + "(" + totalSubNumber + ")";

                        //subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksPre2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksPre2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksPre2) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradePre2 + '</td>';

                        tableBodyPreBoard1.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre2TheoryMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre2PracticalMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.Pre2AllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.Pre2Grade + '</b></td>';
                    tableBodyPreBoard1.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalPre2 + "%",
                        data.totalResultPercentage.PracticalTotalPre2 + "%",
                        data.totalResultPercentage.Pre2AllTotal + "%",
                        data.totalResultPercentage.Pre2Grade, true
                    );

                    tableBodyPreBoard1.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResultPercentage.Pre2Grade);
                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    if ($("#overallGrade").text() === '' || $("#overallGrade").text() === 'D') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        $("#overallGrade").text(data.totalResultPercentage.Pre2Grade);
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksPre2), getOptionMarkGrade(optionalSubjectData.PracticalMarksPre2),
                            getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksPre2), optionalSubjectData.GradePre2, false);
                        tableBodyPreBoard1.appendChild(moralScienceRowUT1);
                    });


                }

                else if (splitValues.id === "6" && splitValues.name === "Promotion") {
                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$(".Term1TheoryMaxMarkTotal").text("(" + data.totalResult.Pre1TheoryMaxTotal + ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Pre1PracticalMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Pre1TheoryMaxTotal) + parseInt(data.totalResult.Pre1PracticalMaxTotal)) / subjectDataLength) + ")");

                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".PromotionNameTheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPromotionTheory) + ")");
                        $("#PromotionNameTPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksPromotionPractical) + ")");
                        $("#PromotionTAllTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksPromotionTheory) + parseInt(subjectDatas[0].MaxMarksPromotionPractical))) + ")");
                        break;
                    }


                    var tableBodyPromotionE = document.getElementById('PromotionRecords').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodyPromotionE.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodyPromotionE.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksPromotionTheory + subjectData.MaxMarksPromotionPractical;
                        subjectTd.textContent = subjectData.Subject + "(" + subjectData.MaxMarksPromotionTheory + "/" + subjectData.MaxMarksPromotionTheory + ")";
                        //  subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksPromotion) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksPromotion) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksPromotion) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradePromotion + '</td>';

                        tableBodyPromotionE.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PromotionTheoryMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PromotionPracticalMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.PromotionAllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td></td>';

                    tableBodyPromotionE.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalPromotion + "%",
                        data.totalResultPercentage.PracticalTotalPromotion + "%",
                        data.totalResultPercentage.PromotionAllTotal + "%",
                        data.totalResult.PromotionGrade, true
                    );

                    tableBodyPromotionE.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResultPercentage.PromotionGrade);
                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    if ($("#overallGrade").text() === '' || $("#overallGrade").text() === 'D') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksPromotion), getOptionMarkGrade(optionalSubjectData.PracticalMarksPromotion),
                            getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksPromotion), optionalSubjectData.GradePromotion, false);
                        tableBodyPromotionE.appendChild(moralScienceRowUT1);
                    });


                }
                else if (splitValues.id === "5") {
                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    //$(".Term1TheoryMaxMarkTotal").text("(" + data.totalResult.Pre1TheoryMaxTotal + ")");
                    //$("#Term1TPracticalMaxMarkTotal").text("(" + (data.totalResult.Pre1PracticalMaxTotal / subjectDataLength)+ ")");
                    //$("#Term1TAllTotalMaxMarkTotal").text("(" + ((parseInt(data.totalResult.Pre1TheoryMaxTotal) + parseInt(data.totalResult.Pre1PracticalMaxTotal)) / subjectDataLength) + ")");

                    for (let i = 0; i < subjectDatas.length; i++) {
                        $(".SelectionNameTheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksSelectionTheory) + ")");
                        $("#SelectionNameTPracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksSelectionPractical) + ")");
                        $("#SelectionTAllTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksSelectionTheory) + parseInt(subjectDatas[0].MaxMarksSelectionPractical))) + ")");
                        break;
                    }


                    var tableBodySelectionE = document.getElementById('SelectionRecords').getElementsByTagName('tbody')[0];
                    // Get all rows from the table body
                    var rows = tableBodySelectionE.getElementsByTagName('tr');

                    // Skip the first two rows and remove the rest
                    for (var i = rows.length - 1; i >= 2; i--) {
                        tableBodySelectionE.removeChild(rows[i]);
                    }
                    subjectDatas.forEach(subjectData => {
                        var newRowUT1 = document.createElement('tr');

                        // Create the "subject" <td> with "scope" and "class" attributes
                        var subjectTd = document.createElement('td');
                        var totalSubNumber = subjectData.MaxMarksSelectionTheory + subjectData.MaxMarksSelectionPractical;
                        subjectTd.textContent = subjectData.Subject + "(" + totalSubNumber + ")";
                        //  subjectTd.textContent = subjectData.Subject;
                        subjectTd.setAttribute('scope', 'row');
                        subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
                        newRowUT1.appendChild(subjectTd);

                        // Add the data to the respective <td> elements

                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TheoryMarksSelection) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.PracticalMarksSelection) + '</td>';
                        newRowUT1.innerHTML += '<td>' + checkForZero(subjectData.TotalObtainedMarksSelection) + '</td>';
                        newRowUT1.innerHTML += '<td>' + subjectData.GradeSelection + '</td>';

                        tableBodyPromotionE.appendChild(newRowUT1);
                    });
                    // Add the row for the total
                    var totalRowUT1 = document.createElement('tr');

                    // Create the "Total" <td> with "scope" and "class" attributes and bold font
                    var totalTdUT1 = document.createElement('td');
                    totalTdUT1.textContent = 'Total';
                    totalTdUT1.setAttribute('scope', 'row');
                    totalTdUT1.setAttribute('class', 'text-uppercase text-start ps-1');
                    totalTdUT1.style.fontWeight = 'bold';
                    totalRowUT1.appendChild(totalTdUT1);
                    // Add the data to the respective <td> elements using <b> tags for bold text

                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.SelectionTheoryMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.SelectionPracticalMaxTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.SelectionAllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td></td>';
                    tableBodySelectionGradeE.appendChild(totalRowUT1);

                    //additional rows
                    // Add the additional rows for percentages and other subjects
                    var percentagesRowUT1 = createAdditionalRow("Percentage",
                        data.totalResultPercentage.TheoryTotalSelection + "%",
                        data.totalResultPercentage.PracticalTotalSelection + "%",
                        data.totalResultPercentage.SelectionAllTotal + "%",
                        data.totalResultPercentage.SelectionGradeGrade, true
                    );

                    tableBodySelectionGradeE.appendChild(percentagesRowUT1);
                    $("#overallGrade").text(data.totalResultPercentage.SelectionGradeGrade);
                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    if ($("#overallGrade").text() === '' || $("#overallGrade").text() === 'D') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksSelectionGrade), getOptionMarkGrade(optionalSubjectData.PracticalMarksSelectionGrade),
                            getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksSelectionGrade), optionalSubjectData.GradeSelectionGrade, false);
                        tableBodyPromotionE.appendChild(moralScienceRowUT1);
                    });


                }
                else {
                    for (let i = 0; i < subjectDatas.length; i++) {
                        $("#UT1MaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksUT1) + ")");
                        $("#UT2MaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksUT2) + ")");

                        $(".Term1TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm1Theory) + ")");
                        $("#Term1PracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm1Practical) + ")");
                        $("#Term2TheoryMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm2Theory) + ")");
                        $("#Term2PracticalMaxMarkTotal").text("(" + (subjectDatas[0].MaxMarksTerm2Practical) + ")");

                        $("#UTTotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksUT1) + parseInt(subjectDatas[0].MaxMarksUT2))) + ")");
                        $("#Term1TotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksTerm1Theory) + parseInt(subjectDatas[0].MaxMarksTerm1Practical))) + ")");
                        $("#Term2TotalMaxMarkTotal").text("(" + ((parseInt(subjectDatas[0].MaxMarksTerm2Theory) + parseInt(subjectDatas[0].MaxMarksTerm2Practical))) + ")");
                        break;
                    }

                }

                //var tableGrading = document.getElementById('GradingCriteria').getElementsByTagName('tbody')[0];

                // var append = "<tr>";
                // append += '<th>Range (%)</th>';
                // data.gradingCriteria.forEach(gradingCriteria => {


                // append += ' <td>' + gradingCriteria.MinimumPercentage + ' - ' + gradingCriteria.MaximumPercentage + '</td>';
                // });
                // append += '</tr>';
                // $("#GradingCriteria").html('');
                // $("#GradingCriteria").append(append);
                // var append1 = "<tr>";
                // append1 += '<th>Grade</th>';
                // data.gradingCriteria.forEach(gradingCriteria => {


                // append1 += ' <td>' + gradingCriteria.Grade + '</td>';
                // });
                // append1 += '</tr>';
                // append1 += '<tr><td colspan = "7" scope = "colgroup" class="text-start ps-1 ps-1" >';
                // append1 += '<div class="ft15">';
                // append1 += '4.';
                // append1 += 'N.B. There will be no second attempt for English Language and';
                // append1 += 'Literature. Second attempt is allowed for one subject provided';
                // append1 += 'the score is below 35.';
                // append1 += '</div>';
                // append1 += '<div class="ft15">Minimum mark for passing is 35.</div>';
                // append1 += '<div class="ft15">** It Indicates Improvement Exam.</div>';
                // append1 += '</td>';
                // append1 += '</tr >';
                // $("#GradingCriteria").append(append1);
                // console.log(data.Result);
                // if (data.Result != "Pass") {
                // $('#result').text("");
                // $('#overallGrade').text("");
                // }
                // var append = "<tr>";
                // append += '<th>Range (%)</th>';
                // data.gradingCriteria.forEach(gradingCriteria => {


                // append += ' <td>' + gradingCriteria.MinimumPercentage + ' - ' + gradingCriteria.MaximumPercentage + '</td>';
                // });
                // append += '</tr>';
                // $("#GradingCriteria").html('');
                // $("#GradingCriteria").append(append);
                // var append1 = "<tr>";
                // append1 += '<th>Grade</th>';
                // data.gradingCriteria.forEach(gradingCriteria => {


                // append1 += ' <td>' + gradingCriteria.Grade + '</td>';
                // });
                // append1 += '</tr>';
                // append1 += '<tr><td colspan = "7" scope = "colgroup" class="text-start ps-1 ps-1" >';
                // append1 += '<div class="ft15">';
                // append1 += '4.';
                // append1 += 'N.B. There will be no second attempt for English Language and';
                // append1 += 'Literature. Second attempt is allowed for one subject provided';
                // append1 += 'the score is below 35.';
                // append1 += '</div>';
                // append1 += '<div class="ft15">Minimum mark for passing is 35.</div>';
                // append1 += '<div class="ft15">** It Indicates Improvement Exam.</div>';
                // append1 += '</td>';
                // append1 += '</tr >';
                // $("#GradingCriteria").append(append1);
                // console.log(data.Result);
                // if (data.Result != "Pass") {
                // $('#result').text("");
                // $('#overallGrade').text("");
                // }
                //var tableGrading = document.getElementById('GradingCriteria').getElementsByTagName('tbody')[0];
                if (splitValues.id != "10") {
                    console.log(data)
                    var append = "<tr>";
                    append += '<th>Range (%)</th>';
                    data.gradingCriteria.forEach(gradingCriteria => {


                        append += ' <td>' + gradingCriteria.MinimumPercentage + ' - ' + gradingCriteria.MaximumPercentage + '</td>';
                    });
                    append += '</tr>';
                    $("#GradingCriteria").html('');
                    $("#GradingCriteria").append(append);
                    var append1 = "<tr>";
                    append1 += '<th>Grade</th>';
                    data.gradingCriteria.forEach(gradingCriteria => {


                        append1 += ' <td>' + gradingCriteria.Grade + '</td>';
                    });
                    append1 += '</tr>';
                    append1 += '<tr><td colspan = "7" scope = "colgroup" class="text-start ps-1 ps-1" >';
                    append1 += '<div class="ft15">';
                    append1 += '4.';
                    append1 += 'N.B. There will be no second attempt for English Language and';
                    append1 += 'Literature. Second attempt is allowed for one subject provided';
                    append1 += 'the score is below 35.';
                    append1 += '</div>';
                    append1 += '<div class="ft15">Minimum mark for passing is 35.</div>';
                    append1 += '<div class="ft15">** It Indicates Improvement Exam.</div>';
                    append1 += '</td>';
                    append1 += '</tr >';
                    $("#GradingCriteria").append(append1);
                    console.log(data.Result);
                    if (data.Result != "Pass") {
                        $('#result').text("");
                        $('#overallGrade').text("");
                    }
                }
                else {
                    $("#GradingCriteria").hide();
                    $("#StaticGradingCriteria").show();
                }

            } else {
                console.log("Invalid API response format");
            }
        })
        .catch(error => {
            console.log("API call failed: " + error);
        });
}
function manageCoScholasticTable(data, term) { //#coScholasticTable

    var isHasValue = false;
    for (var i = 0; i < data.length; i++) {
        var row = data[i];

        if (term == 3) {
            if (row.GradeTerm1 != '-' && (row.GradeTerm1 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 1) {
            if (row.GradeUT1 != '-' && (row.GradeUT1 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 2) {
            if (row.GradeUT2 != '-' && (row.GradeUT2 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 4) {
            if (row.GradeTerm2 != '-' && (row.GradeTerm2 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 7) {
            if (row.GradePre1 != '-' && (row.GradePre1 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 8) {
            if (row.GradePre2 != '-' && (row.GradePre2 + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 6) {
            if (row.GradePromotion != '-' && (row.GradePromotion + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else if (term == 5) {
            if (row.GradeSelection != '-' && (row.GradeSelection + "").length > 0) {
                isHasValue = true;
                break;
            }
        }
        else {
            if ((row.GradeTerm1 != '-' && (row.GradeTerm1 + "").length > 0) || (row.GradeTerm2 != '-' && (row.GradeTerm2 + "").length > 0)) {
                isHasValue = true;
                break;
            }
        }

    }
    return isHasValue;

}
function splitValue(value) {

    if (/^UT\d+$/.test(value)) {
        const name = value.substring(0, 2);
        const id = value.substring(2);
        return { name, id };
    }

    else if (/^Term\d+$/.test(value)) {
        const name = value.substring(0, 4);
        const id = value.substring(4);
        return { name, id };
    }
    else if (/^PreBoard\d+$/.test(value)) {
        const name = value.substring(0, 8);
        const id = value.substring(8);
        return { name, id };
    }
    else if (/^All\d+$/.test(value)) {
        const name = value.substring(0, 3);
        const id = value.substring(3);
        return { name, id };
    }
    else if (/^Selection\d+$/.test(value)) {
        const name = value.substring(0, 9);
        const id = value.substring(9);
        return { name, id };
    }
    else if (/^Promotion\d+$/.test(value)) {
        const name = value.substring(0, 9);
        const id = value.substring(9);
        return { name, id };
    }
    else {
        return { name: '', id: value };
    }
}


function getOptionalGradeForAllReports(gradeNumber) {
    try {
        if (typeof gradeNumber === "number") {
            if (gradeNumber === -1) {
                return "AB";
            } else if (gradeNumber === -2) {
                return "-";
            } else if (gradeNumber == 1) {
                return "A";
            } else if (gradeNumber == 2) {
                return "B";
            } else if (gradeNumber == 3) {
                return "C";
            } else if (gradeNumber == 4) {
                return "D";
            } else if (gradeNumber < 1 || gradeNumber > 4) {
                return "D";
            }

            var grade = String.fromCharCode('A'.charCodeAt(0) + (gradeNumber - 1));
            return grade;
        } else {
            return gradeNumber;
        }
    } catch (e) {
        console.log(e);
        return gradeNumber;
    }
}


function getOptionMarkGrade(gradeNumber) {
    if (gradeNumber === -1) {
        return "AB";
    } else if (gradeNumber === -2) {
        return "-";
    } else if (gradeNumber < 1 || gradeNumber > 4) {
        return "D";
    }

    var grade = String.fromCharCode('A'.charCodeAt(0) + (gradeNumber - 1));
    return grade;
}


function generatePDF() {
    // Create a new jsPDF instance
    var doc = new jsPDF();

    // Get the content of the specific <div> to be printed
    var content = document.getElementById("canvas").innerHTML;

    // Add the content to the PDF document
    doc.fromHTML(content, 15, 15);

    // Get the current date
    var currentDate = new Date();

    // Format the date as "YYYY-MM-DD" (adjust as needed)
    var formattedDate = currentDate.toISOString().split('T')[0];

    // Save the PDF with a filename including the dynamic date
    doc.save("document_" + formattedDate + ".pdf");
}



///




function callReportPDF() {
    try {
        const numbersString1 = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
        const numbers1 = JSON.parse(numbersString1);
        console.log("Ids")
        console.log(numbers1)
        processNumbers();
    }
    catch (err) {
        document.getElementById("loader-overlay").style.display = "none";
        console.log(err);
    }

}
async function createAllPrint() {
    const urlParam = new URLSearchParams(window.location.search);
    const id = urlParam.get('id');

    try {
        const response = await fetch('/Exam/GetEncodedContent?id=' + id);
        const data = await response.json();
        console.log(data);
        // const keysInOriginalOrder = [171, 209, 173, 175, 216]; // Replace with your desired order
        const numbersString1 = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
        const numbers1 = JSON.parse(numbersString1);
        var keysInOriginalOrder = [];

        // Loop through each object in the data array and extract StudentId
        for (var i = 0; i < numbers1.length; i++) {
            var studentId = numbers1[i].StudentId;
            keysInOriginalOrder.push(studentId);
        }
        console.log(keysInOriginalOrder);
        const jsonData = JSON.parse(data);
        console.log(jsonData);

        $('#printColumns').empty();

        const keysInOrder = Object.keys(jsonData);
        console.log(keysInOrder);
        for (const key of keysInOriginalOrder) {
            if (jsonData.hasOwnProperty(key)) {
                const content = jsonData[key];
                const htmlContent = atob(content);

                $('#printColumns').append(htmlContent);
                await processNextKey(); // Optional delay if needed
            }
        }

        // document.getElementById("loader-overlay").style.display = "none";
    } catch (error) {
        console.error(error);
    }
}
var test = [];
var parentDiv;
async function processNumbers() {
    debugger
    try {
        const numbersString = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
        const batchString = localStorage.getItem("PrintBatch"); // Retrieve the string from localStorage
        const numbers = JSON.parse(numbersString); // Parse the string into an array
        const batch = JSON.parse(batchString); // Parse the string into an array
        parentDiv = document.createElement('div');
        parentDiv.classList.add('cls1');
        //for (const { rollno, StudentId } of numbers) {
        //    const data = await fetchData(rollno, StudentId);
        //    console.log(StudentId)
        //    await new Promise(resolve => setTimeout(resolve, 3000)); // Introduce a delay
        //    Creatjson(StudentId);

        //}
        for (const { rollno, StudentId } of numbers) {
            await fetchData(rollno, StudentId);
            
        }

        pdf();
        console.log(test)

        //generatePdf1('1')
        //multiplePDF()
        // await createPdf()
    } catch (err) {
        document.getElementById("loader-overlay").style.display = "none";
        console.log(err);
    }



}
//setInterval(Creatjson(), 2000);
function Creatjson(studentId) {
    try {
        const divContent = $('#JsonPrint').html();
        $('#printColumns').empty();
        $('#printColumns').append(divContent);
        // document.getElementById("loader-overlay").style.display = "none";
        //var x = document.createElement('div');
        //x.classList.add('cls');
        document.querySelector('#printT').classList.add("test")
        parentDiv.appendChild(document.querySelector('#printT').cloneNode(true));
        //parentDiv.appendChild(x.cloneNode(true));
        //test.push(x);
        // generatePdf1(studentId);
    } catch (err) {
        document.getElementById("loader-overlay").style.display = "none";
        console.log(err);
    }


}


function generatePdf1(studentId) {
    window.jsPDF = window.jspdf.jsPDF;
    const divContent = $('#JsonPrint');
    divContent.empty();
    console.log(divContent)
    divContent.append(parentDiv);
    //let jsPdf = new jsPDF('p', 'pt', 'a6');
    let jsPdf = new jsPDF('p', 'pt', [610, 900])
    var fileName = "Result" + studentId;
    var htmlElement = divContent.html();//document.querySelector('.cls1');
    // console.log(htmlElement)
    // you need to load html2canvas (and dompurify if you pass a string to html)
    const opt = {
        callback: function (jsPdf) {

            jsPdf.save(fileName);
            // to open the generated PDF in browser window
            // window.open(jsPdf.output('bloburl'));
        },
        margin: [0, 0, 0, 0],
        autoPaging: 'text',
        html2canvas: {
            allowTaint: true,
            dpi: 300,
            letterRendering: true,
            logging: false,
            scale: .8
        }
    };
    jsPdf.html(htmlElement, opt);
}


function createHead() {
    var head = ` <!DOCTYPE html>
    <html lang="en">
    <head>
        <meta charset="UTF-8" />
        <meta http-equiv="X-UA-Compatible" content="IE=edge" />
        <meta name="viewport" content="width=device-width, initial-scale=1.0" />
        <title>Document</title>

        <link href="https://fonts.googleapis.com/css2?family=Lora:wght@400;500;600;700&family=Neuton:wght@300;400;700&display=swap"
              rel="stylesheet" />
        <link rel="stylesheet"
              href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" />
              <script src="~/Scripts/DevelopmentJS/PrintReport.js"></script>
        <style>

            #printT {
                border: 1px solid #ccc;
                padding: 20px;
                margin: 20px;
                width: 7.5in;
                height: 11in;
                box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
                page-break-after: always;
            }

            #loader-overlay {
                position: fixed;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                background-color: rgba(0, 0, 0, 0.5);
                display: none;
                justify-content: center;
                align-items: center;
                z-index: 1000;
            }

            .loader {
                border: 4px solid #f3f3f3;
                border-top: 4px solid #3498db;
                border-radius: 50%;
                width: 50px;
                height: 50px;
                animation: spin 2s linear infinite;
            }

            @@keyframes spin {
                0% {
                    transform: rotate(0deg);
                }

                100% {
                    transform: rotate(360deg);
                }
            }
        </style>
        <style>
	        body {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        .ft10 {
            font-size: 11px;
            font-family: Times;
            color: #333333;
        }

        .ft11 {
            font-size: 34px;
            font-family: Times;
            color: #333333;
        }

        .ft12 {
            font-size: 15px;
            font-family: Times;
            color: #333333;
        }

        .ft13 {
            font-size: 20px;
            font-family: Times;
            color: #333333;
        }

        .ft14 {
            font-size: 11px;
            font-family: Times;
            color: #333333;
        }

        .ft15 {
            font-size: 12px;
            font-family: Times;
            color: #333333;
        }

        .ft16 {
            font-size: 16px;
            font-family: Times;
            color: #333333;
        }

        .ft17 {
            font-size: 12px;
            line-height: 17px;
            font-family: Times;
            color: #333333;
        }

        .headerTermColumn {
            width: 80px !important;
        }

        table {
            width: 100%;
        }

        table, th, td {
            border: 0.2px solid #00000067;
            border-collapse: collapse;
        }

        .signature {
            display: flex;
            justify-content: center;
            align-items: center;
            flex-direction: column;
        }

        @media print {
            .ft11 {
                font-size: 23px; /* Font size for printing */
            }

            .ft13 {
                font-size: 16px;
            }
        }
html, body { margin: 0; padding: 0; }

	        </style>
    </head>
    <body style="overflow: hidden" >
        <div id="loader-overlay">
            <div class="loader"></div>
        </div>`

    return head;
}

//function createScript() {
//    var script = `<script src="https://code.jquery.com/jquery-3.7.0.slim.js" integrity="sha256-7GO+jepT9gJe9LB4XFf8snVOjX3iYNb0FHYr5LI1N5c=" crossorigin="anonymous"></script>
//        <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
//        <script src="https://html2canvas.hertzen.com/dist/html2canvas.min.js"></script>
//        <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.3/html2pdf.bundle.min.js"></script>
//        <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js"></script>
//        <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"></script>
//        <script src="~/Scripts/DevelopmentJS/PrintReport.js"></script>
//`

//    return script;
//}

function createScript() {
    return [
        "https://code.jquery.com/jquery-3.7.0.slim.js",
        "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js",
        "https://html2canvas.hertzen.com/dist/html2canvas.min.js",
        "https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js",
        "/Scripts/DevelopmentJS/PrintReport.js"
    ];
}
function pdf() {
    debugger
    try {
        //var linkArry = [
        //    "https://code.jquery.com/jquery-3.7.0.slim.js",
        //    "https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js",
        //    "https://html2canvas.hertzen.com/dist/html2canvas.min.js",
        //    "https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.3/html2pdf.bundle.min.js",
        //    "https://cdnjs.cloudflare.com/ajax/libs/jspdf/2.5.1/jspdf.umd.min.js",
        //    "https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.10.1/html2pdf.bundle.min.js"
        ""
        //]

        var chtml = document.createElement('html')
        chtml.appendChild(document.createElement('head'))
        chtml.appendChild(document.createElement('body'))
        const header = $.parseHTML(createHead());
        for (i = 0; i < header.length; i++) {
            chtml.children[0].append(header[i])
        }
        window.html2pdf = window.html2pdf;
        const divContent = $('#JsonPrint');

        divContent.empty();
        divContent.append(parentDiv);
        chtml.children[1].append(parentDiv)
        var linkArry = createScript();
        linkArry.forEach(function (url) {
            var script = document.createElement("script");
            script.src = url;
            script.defer = true;
            document.head.appendChild(script);
        });
        //for (i = 0; i < linkArry.length; i++) {
        //    createSpt = document.createElement('script');
        //    createSpt.setAttribute("src", linkArry[i]);
        //    chtml.children[1].append(createSpt)
        //}
        //var getDivContent = chtml;
        //console.log(getDivContent)

        //var settings = {
        //    "url": "/Exam/ConvertToPdf",
        //    "method": "POST",
        //    "timeout": 0,
        //    "headers": {
        //        "Content-Type": "application/json"
        //    },
        //    "data": JSON.stringify({ Htmlcontent: chtml.innerHTML }),
        //};
        //debugger
        //$.ajax(settings).done(function (response) {
        //    filePath = "/Rotativa/" + response;
        //    var link = document.createElement("a");
        //    link.href = filePath;
        //    link.download = response
        //    link.click();
        //    window.location.reload();
           document.getElementById("loader-overlay").style.display = "none";

       // });
    } catch (err) {
        document.getElementById("loader-overlay").style.display = "none";
        console.log(err);
    }


}

//async function createPdf() {
//    window.PDFDocument = PDFLib.PDFDocument;
//    const pdfDoc =await PDFDocument.create();
//    console.log(pdfDoc)

//    const page = pdfDoc.addPage()
//    //page.html('<b>strong</b>')
//    page.drawText('<p>You can create PDFs!</p>')
//    const pdfBytes = await pdfDoc.save()
//    console.log(pdfBytes)
//    var file = new Blob([pdfBytes], { type: 'application/pdf' });
//    var fileURL = URL.createObjectURL(file);
//    window.open(fileURL);
//}

function multiplePDF() {
    window.jsPDF = window.jspdf.jsPDF;
    var pdf = new jsPDF('p', 'pt', [580, 630]);
    for (let i = 0; i < test.length; i++) {
        console.log(test[i])

        html2canvas(test[i], {
            onrendered: function (canvas) {
                document.body.appendChild(canvas);
                var ctx = canvas.getContext('2d');
                var imgData = canvas.toDataURL("image/png", 1.0);
                var htmlH = test[i];
                var width = canvas.width;[0][0]
                var height = canvas.clientHeight;
                pdf.addPage(580, htmlH);
                pdf.addImage(imgData, 'PNG', 20, 20, (width - 10), (height));
            }
        });
    }

    //html2canvas($(".page2")[0], {
    //    allowTaint: true,
    //    onrendered: function (canvas) {
    //        var ctx = canvas.getContext('2d');
    //        var imgData = canvas.toDataURL("image/png", 1.0);
    //        var htmlH = $(".page2").height() + 100;
    //        var width = canvas.width;
    //        var height = canvas.clientHeight;
    //        pdf.addPage(580, htmlH);
    //        pdf.addImage(imgData, 'PNG', 20, 20, (width - 40), (height));
    //    }
    //});
    //html2canvas($(".page3")[0], {
    //    allowTaint: true,
    //    onrendered: function (canvas) {
    //        var ctx = canvas.getContext('2d');
    //        var imgData = canvas.toDataURL("image/png", 1.0);
    //        var htmlH = $(".page2").height() + 100;
    //        var width = canvas.width;
    //        var height = canvas.clientHeight;
    //        pdf.addPage(580, htmlH);
    //        pdf.addImage(imgData, 'PNG', 20, 20, (width - 40), (height));
    //    }
    //});
    pdf.save('sample.pdf');
    setTimeout(function () {

        //jsPDF code to save file

    }, 0);
}

async function processNextKey() {
    return new Promise(resolve => {
        setTimeout(resolve, 1000); // Adjust the delay as needed
    });
}



function splitValue(value) {
    console.log(value)
    if (/^UT\d+$/.test(value)) {
        const name = value.substring(0, 2);
        const id = value.substring(2);
        return { name, id };
    }

    else if (/^Term\d+$/.test(value)) {
        const name = value.substring(0, 4);
        const id = value.substring(4);
        return { name, id };
    }
    else if (/^PreBoard\d+$/.test(value)) {
        const name = value.substring(0, 8);
        const id = value.substring(8);
        return { name, id };
    }
    else if (/^All\d+$/.test(value)) {
        const name = value.substring(0, 3);
        const id = value.substring(3);
        return { name, id };
    }
    else if (/^Selection\d+$/.test(value)) {
        const name = value.substring(0, 9);
        const id = value.substring(9);
        return { name, id };
    } else if (/^Promotion\d+$/.test(value)) {
        const name = value.substring(0, 9);
        const id = value.substring(9);
        return { name, id };
    }
    else {
        return null;
    }
}
//check zero in data

// Function to check if the value is 0, then return "-"
function checkForZero(value) {
    if (value === -1) {
        return "AB";
    } else if (value === -2) {
        return "-";
    } else {
        return value;
    }
}

// Function to create an additional row with given values
function createAdditionalRow(...data) {
    var newRow = document.createElement('tr');
    var IsBold = data.pop();
    // Create the "subject" <td> with "scope" and "class" attributes
    var subjectTd = document.createElement('td');
    subjectTd.textContent = data[0]; // First parameter is the subject
    subjectTd.setAttribute('scope', 'row');
    subjectTd.setAttribute('class', 'text-uppercase text-start ps-1');
    newRow.appendChild(subjectTd);

    // Loop through the data starting from the second parameter (index 1)
    for (var i = 1; i < data.length; i++) {
        addDataToRow(newRow, data[i], IsBold);
    }

    return newRow;
}

function addDataToRow(row, data, isBold) {
    var td = document.createElement('td');
    if (isBold) {
        var boldElement = document.createElement('b');
        boldElement.textContent = data;
        td.appendChild(boldElement);
    } else {
        td.textContent = data;
    }
    row.appendChild(td);
}
function PercentageCal(obtainedMarks, totalMarks) {

    var a = (obtainedMarks / totalMarks) * 100;
    return a.toFixed(2);
}

function GetGrade(percentage) {
    //    fetch('/Exam/GetGradeByPercentage?percentage=' + percentage, {
    //        method: 'GET',
    //        headers: {
    //            'Content-Type': 'application/json'
    //        }
    //    })
    //        .then(response => response.json())
    //        .then(data => {
    //            console.log(data, data.Grade);
    //            return data.Grade;


    //})
    //        .catch(error => {
    //            return "F";
    //    console.log("API call failed: " + error);
    //});
    fetch('/Exam/AllGrade', {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {

            gradingCriteria = data;
            var matchingGrades = gradingCriteria.filter(function (g) {
                return percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage;
            });

            if (matchingGrades.length > 0) {
                return matchingGrades[0].Grade;
            } else {
                // If no matching grade found, you can return a default grade or null
                return "F";
            }
            // Call getGradeByPercentage function here after the API call is complete and gradingCriteria is populated
            //return getGradeByPercentage(percentage);
        })
        .catch(error => {
            console.log("API call failed: " + error);
        });
}


function getGradeByPercentage(percentage) {
    var matchingGrades = gradingCriteria.filter(function (g) {
        return percentage >= g.MinimumPercentage && percentage <= g.MaximumPercentage;
    });

    if (matchingGrades.length > 0) {
        return matchingGrades[0].Grade;
    } else {
        // If no matching grade found, you can return a default grade or null
        return "F";
    }
}



function addRows(rowData, Term) {
    //add coscholastic area
    //const rowData = [
    //    // Sample data - add more records as needed
    //    { "Name": "Subject 1", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 2", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 3", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 4", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 5", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 6", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 7", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 8", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 9", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 10", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 11", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 12", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 13", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 14", "GradeTerm1": "A", "GradeTerm2": "B" },
    //    { "Name": "Subject 15", "GradeTerm1": "A", "GradeTerm2": "B" }
    //];
    // Get the table reference
    const table = document.getElementById("coScholasticTable");
    const rows = table.getElementsByTagName("tr");

    // Loop through the rows in reverse order to avoid issues with removing elements while iterating
    for (let i = rows.length - 1; i >= 0; i--) {
        const row = rows[i];

        // Check if the row contains <th> elements
        if (row.getElementsByTagName("th").length === 0) {
            // If it doesn't contain <th> elements, remove the row
            row.remove();
        }
    }
    const totalRecords = rowData.length;
    const pairs = Math.ceil(totalRecords / 2);

    for (let i = 0; i < pairs; i++) {
        const data1 = rowData[i * 2];
        const data2 = rowData[i * 2 + 1];

        const row = createRow(data1, data2, Term);
        table.appendChild(row);
    }
}

function createRow(data1, data2, Term) {
    const row = document.createElement("tr");
    row.classList.add("custom-row");

    const createCell = (text, isHeader = false) => {
        const cell = isHeader ? document.createElement("td") : document.createElement("td");
        cell.textContent = text;
        cell.scope = "row";
        return cell;
    };

    // First set of columns
    if (data1) {
        row.appendChild(createCell(data1.Name, true)).classList.add("text-start", "ps-1");
        if (Term == 3) {
            row.appendChild(createCell(data1.GradeTerm1));
        }
        else if (Term == 4) {
            row.appendChild(createCell(data1.GradeTerm2));
        }
        else if (Term == 7) {
            row.appendChild(createCell(data1.GradePre1));
        }
        else if (Term == 8) {
            row.appendChild(createCell(data1.GradeTerm2));
        }
        else {
            row.appendChild(createCell(data1.GradeTerm1));
            row.appendChild(createCell(data1.GradeTerm2));
            //row.appendChild(createCell(data1.GradePre1));
            //row.appendChild(createCell(data1.GradePre2));
        }
    } else {
        row.appendChild(createCell("", true)).classList.add("text-start", "ps-1");

        if (Term == 3) {
            row.appendChild(createCell(""));
        }
        else if (Term == 4) {
            row.appendChild(createCell(""));
        }
        else if (Term == 7) {
            row.appendChild(createCell(""));
        }
        else if (Term == 8) {
            row.appendChild(createCell(""));
        }
        else {
            row.appendChild(createCell(""));
            row.appendChild(createCell(""));
        }
    }

    // Second set of columns
    if (data2) {
        row.appendChild(createCell(data2.Name, true)).classList.add("text-start", "ps-1");
        if (Term == 3) {
            row.appendChild(createCell(data1.GradeTerm1));
        }
        else if (Term == 4) {
            row.appendChild(createCell(data1.GradeTerm2));
        }
        else if (Term == 7) {
            row.appendChild(createCell(data2.GradePre1));
        }
        else if (Term == 8) {
            row.appendChild(createCell(data1.GradePre2));
        }
        else {
            row.appendChild(createCell(data1.GradeTerm1));
            row.appendChild(createCell(data1.GradeTerm2));
            //row.appendChild(createCell(data2.GradePre1));
            //row.appendChild(createCell(data2.GradePre2));
        }
    } else {
        row.appendChild(createCell("", true)).classList.add("text-start", "ps-1");

        if (Term == 3) {
            row.appendChild(createCell(""));
        }
        else if (Term == 4) {
            row.appendChild(createCell(""));
        }
        else if (Term == 7) {
            row.appendChild(createCell(""));
        }
        else if (Term == 8) {
            row.appendChild(createCell(""));
        }
        else {
            row.appendChild(createCell(""));
            row.appendChild(createCell(""));
        }
    }

    return row;
}



