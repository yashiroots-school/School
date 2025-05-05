

$(".BtnGradeEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnSubmitGrading").text("Update");
    $("#GradingCriteriaForm").attr('action', '/Exam/UpdateGrading');
    GetGradeById(id);
});

function GetGradeById(id) {
    $.ajax({
        url: "/Exam/GetGradingById?Id=" + id,
        type: "GET",
        datatype: "json",
        success: function (result) {
            if (result != null) {
                $("#CriteriaID").val(result.CriteriaID);
                $("#BoardID").val(result.BoardID);
                $("#MinimumPercentage").val(result.MinimumPercentage);
                $("#MaximumPercentage").val(result.MaximumPercentage);
                $("#Grade").val(result.Grade);
                $("#GradeDescription").val(result.GradeDescription);
                $("#ClassID").val(result.ClassID);
                $("#BatchID").val(result.BatchID);
                $("#TestID").val(result.TestID);
                $("#TermID").val(result.TermID);
                $("TestName").val(result.TestName);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(".BtnGradeDelete").on('click', function () {
    var id = $(this).attr("data-val");
    var cnf = confirm("Are You Sure, You want to delete the record?");
    if (cnf == true) {
        window.location.href = "/Exam/DeleteGrading?Id=" + id;
    }
});



$(document).ready(function () {
    $(".BtnTermEdit").on('click', function () {
        var id = $(this).data("val");
        $("#BtnSubmitTerm").text("Update");
        $("#TermForm").attr('action', '/Exam/UpdateTerm');
        GetTermById(id);
    });

    $(".BtnTermDelete").on('click', function () {
        var id = $(this).data("val");
        var cnf = confirm("Are you sure you want to delete the record?");
        if (cnf) {
            window.location.href = "/Exam/DeleteTerm?id=" + id;
        }
    });
});

function GetTermById(id) {
    $.ajax({
        url: "/Exam/GetTermById?id=" + id,
        type: "GET",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                $("#TermID").val(result.TermID);
                $("#TermName").val(result.TermName);
                var startDateString = result.StartDate; // Replace with your actual value
                var endDateString = result.EndDate; // Replace with your actual value

                // Parse the date strings
                var startDate = parseJsonDate(startDateString);
                var endDate = parseJsonDate(endDateString);

                // Format the dates as strings (e.g., "yyyy-MM-dd")
                var formattedStartDate = startDate.toISOString().substring(0, 10);
                var formattedEndDate = endDate.toISOString().substring(0, 10);

                // Set the formatted dates in the input fields
                $("#StartDate").val(result.StartDate);
                $("#EndDate").val(result.EndDate);
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}
function parseJsonDate(jsonDate) {
    // Extract the numeric part of the date string and convert it to a number
    var milliseconds = parseInt(jsonDate.replace("/Date(", "").replace(")/", ""), 10);

    // Create a JavaScript Date object using the milliseconds
    var date = new Date(milliseconds);

    // Get the timezone offset in minutes
    var timezoneOffsetMinutes = date.getTimezoneOffset();

    // Adjust the date by the timezone offset
    date.setMinutes(date.getMinutes() + timezoneOffsetMinutes);

    return date;
}

function GetTestById(id) {
    $.ajax({
        url: "/Exam/GetTestById?id=" + id,
        type: "GET",
        dataType: "json",
        success: function (result) {
            if (result != null) {
                $("#TestID").val(result.TestID);
                $("#ClassID").val(result.ClassID);
               // $("#SubjectID").val(result.SubjectID);
                $("#BoardID").val(result.BoardID);
                $("#TestName").val(result.TestName);
                $("#TestType").val(result.TestType);
                $("#MaximumMarks").val(result.MaximumMarks);
                $("#TermID").val(result.TermID);
                geSubjectData(result.ClassID); // Call your function to populate subjects
                $("#SubjectID").val(result.SubjectID); // Set subject value after it's populated
            }
        },
        error: function (errormessage) {
            alert(errormessage.responseText);
        }
    });
}


$(document).ready(function () {
    var dataTable;
    $(".ObtainedMarks").on("input", function () {
        console.log(">>");
        var currentValue = parseInt($(this).val());
        if (currentValue > 100) {
            $(this).val(100); // Set the value back to the maximum allowed value
        }
    });
    $(".ObtainedMarks").on("blur", function () {
        console.log(">>>>>>>>");
        var currentValue = parseInt($(this).val());
        var maximumMarks = parseInt($(this).attr("max"));
        if (currentValue > maximumMarks) {
            $(this).val(maximumMarks); // Set the value back to the maximum allowed value
        }
    });

    $("#BtnShowStudent").on('click', function () {
       
        var classid = $("#ClassID").val();
        var sectionid = $("#SectionId").val();
        var subjectid = $("#SubjectId").val();
        var termid = $("#TermID").val();
        var staffid = $("#Staff_Id").val();
        var batchid = $("#BatchID").val();

        var fields = [
            { id: "#Staff_Id", name: "Staff" },
            { id: "#ClassID", name: "Class" },
            { id: "#SectionId", name: "Section" },
            { id: "#TermID", name: "Term" },
            { id: "#BatchID", name: "Batch" }
        ];
        var missingField = "";
        //if ($("#SectionId").val() == 0) {
        //    alert("Please select a Section.");
        //}
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
               // }   
        //for (var i = 0; i < fields.length; i++) {
        //    var fieldValue = $(fields[i].id).val();
        //    if (fieldValue === '0') {
        //        missingField = fields[i].name;
        //        break;
        //    }
        //}
        
        if (missingField !== "") {
           
            alert("Please select a " + missingField + ".");
        } else {
           
            $.ajax({
                url: "/Exam/StudentByClassSection",
                type: "GET",
                dataType: "json",
                contentType: "application/json;charset=UTF-8",
                data: { classId: classid, sectionId: sectionid, testId: 1, termId: termid, staffId: staffid, subjectId: subjectid, batchid: batchid}, //
                success: function (result) {
                    console.log(result)
                    //debugger
                    $("#TableBody").empty();
                    var row = "";
                   
                    var headerRow = $("#HeaderRow");
                    var grades = ['A', 'B', 'C', 'D'];
                    // Remove previously added dynamic headers (if any)
                    $("#TableStudent .dynamic-header").remove();
          

                    $.each(result.HeaderData, function (index, header) {


                        if (header.TestID === 0) {
                            var th = $("<th>").text("Teacher Remark").addClass("dynamic-header");
                            headerRow.append(th);
                            $("#exportButton").show();
                        }
                        else {
                            var th = $("<th>").text(header.TestName).addClass("dynamic-header");
                            headerRow.append(th);
                        }

                    });

                    // Hidden input field to store BatchId
                    var th = $("<th>").css("display", "none").text("BatchId").addClass("dynamic-header");
                    headerRow.append(th);
                    var tableBody = $("#TableBody");
                    var serialNumber = 1;
                    $.each(result.data, function (index, student) {
                        var row = $("<tr>")
                        // Add the 'disabled' class to the row if student.isfreezed is true
                        if (student.IsFreeze) {
                            row.addClass("disabled");
                            $("#SaveButton").prop("disabled", true);

                        } else {
                            $("#SaveButton").prop("disabled", false);
                        }
                
                        //create td for serial number
                        var studentIdCell = $("<td>", {
                            text: serialNumber,
                            name: student.StudentId  // Replace "studentId" with the desired name
                        });
                        row.append(studentIdCell);
                        //create td for student name
                       // row.append($("<td>").text(student.StudentId));
                        row.append($("<td>").text(student.StudentName));
                        // Create dynamic radio buttons for each grade
                        $.each(student.studentTestObtMarks, function (index, header) {
                            var td = $("<td>");
                            td.addClass('TbHeade');

                            // Create an input box for entering the grade
                            if (header.TestID === 0) {
                                var inputBox = $("<input>").attr({
                                    type: "text",
                                    placeholder: "Remark" ,
                                    name: header.MaximumdMarks +"-grade-" + header.TestName + "-" + header.TestID + "-" + student.StudentId,
                                    value: header.Remark //header.Remark // Use the ObtainedGrade as the default value (if available)
                                });
                            }
                            else if (header.IsElective) {
                                var inputBox = $("<input>").attr({
                                    type: "number",
                                    name: header.MaximumdMarks + "-grade-" + header.TestName + "-" + header.TestID + "-" + student.StudentId,
                                    min: 0,
                                    placeholder: "Marks",
                                    max: 100,
                                    class: "optional",
                                    value: header.ObtainedMarks,// Use the ObtainedGrade as the default value (if available)
                                    disabled: true
                                });
                            }
                            else if (header.IsOptional) {
                                var inputBox = $("<input>").attr({
                                    type: "number",
                                    name: header.MaximumdMarks + "-grade-" + header.TestName + "-" + header.TestID + "-" + student.StudentId,
                                    min: 0,
                                    placeholder: "Marks",
                                    max: 4,
                                    class: "optional",
                                    value: header.ObtainedMarks // Use the ObtainedGrade as the default value (if available)
                                });
                            }
                            else {
                                var inputBox = $("<input>").attr({
                                    type: "number",
                                    name: header.MaximumdMarks+"-grade-" + header.TestName + "-" + header.TestID + "-" + student.StudentId,
                                    min: 0,
                                    placeholder: "Marks", 
                                    max: 100,
                                    value: header.ObtainedMarks // Use the ObtainedGrade as the default value (if available)
                                });
                            }
                            inputBox.css({
                                "width": "100px", // Adjust the width as needed
                                "margin-left": "40px"
                            });
                            inputBox.on("input", function () {
                             
                                var enteredValue = parseFloat($(this).val());
                                var maximumMarks = parseFloat(header.MaximumdMarks);

                                if (enteredValue > maximumMarks) {
                                    $(this).css("border", "3px solid red");
                                  
                                } else {
                                    $(this).css("border", ""); // Reset to default border color
                                }
                               
                                if (enteredValue > 4 && $(this).attr("Class") == "optional") {
                                    $(this).css("border", "3px solid red");
                                    window.alert("you can enter only value between 1 to 4");
                                   
                                } 
                                 else 
                                    {
                                        $(this).css("border", ""); // Reset to default border color
                                    }
                                
                            });

                            td.append(inputBox);
                            row.append(td);
                            
                        });


                        // Hidden input field to store BatchId
                        var batchIdCell = $("<td>").css("display", "none").append(
                            $("<span>").attr({
                                type: "hidden",
                                class: "batchid",
                                value: student.BatchId
                            })
                        );
                        row.append(batchIdCell);
                        serialNumber++;
                        tableBody.append(row);
                    });

                    if (result.IsUpdate) {
                        $("#SaveButton").text("Update")
                    }
                    //dataTable.destroy();
                    //$("#TableStudent").DataTable({
                    //    paging: false// Disable pagination
                       
                    //});
                    if ($.fn.dataTable.isDataTable('#TableStudent')) {
                         dataTable = $('#TableStudent').DataTable();
                    }
                    else {
                        dataTable = $('#TableStudent').DataTable({
                            paging: false
                        });
                    }
                    const table = document.getElementById('TableStudent');
                    const secondColumnCells = table.querySelectorAll('tr > :nth-child(2)');
                    secondColumnCells.forEach((cell) => {
                        cell.classList.add('sticky-column');
                    });
                },
                error: function (errormessage) {                    
                    alert(errormessage.responseText);
                }
            });
            // Fetch the grade data from the API
            $.ajax({
                url: '/Exam/AllGrade', // Replace with the actual URL
                success: function (gradeData) {
                    // Store the grade data in local storage
                    localStorage.setItem('gradeData', JSON.stringify(gradeData));
                },
                error: function (xhr, status, error) {                    
                    console.error("Error fetching grade data:", error);
                }
            });
        }

    });

    // Save button click event handler
    $('#SaveButton').click(function () {
        var selectedClassId = $('#ClassID').val();
        var selectedTermId = $('#TermID').val();
        var selectedSectionId = $('#SectionId').val();
        let hasValidationError = false;
        var staffid = $("#Staff_Id").val();
        let tableData = [];

        $("#TableStudent tbody tr").each(function () {
            let rowData = {
                StudentID: parseInt($(this).find("td:eq(0)").attr("name")),
                BatchId: parseInt($(this).find("span.batchid:hidden").attr("value")),
                ClassID: selectedClassId,
                SectionId: selectedSectionId,
                TermID: selectedTermId,
                Remark: "",
                ObtainedMarkData: []
            };

            $(this).find("td.TbHeade").each(function () {
                let selectedInput = $(this).find("input[type=number]");
                let selectedRemarkInput = $(this).find("input[type=text]");
                if (selectedRemarkInput.length > 0) {
                    rowData.Remark = selectedRemarkInput.val();
                }

                if (selectedInput.length > 0) {
                    let namevalue = selectedInput.attr("name");
                    let splitvalues = namevalue.split("-");
                    let headerid = splitvalues[splitvalues.length - 2];
                    let MaxMark = splitvalues[0];
                    let EnteredMarks = selectedInput.val();

                    if (parseFloat(EnteredMarks) > parseFloat(MaxMark)) {
                        hasValidationError = true;
                        selectedInput.addClass("error-border");
                    } else {
                        selectedInput.removeClass("error-border");
                    }

                    let obtainedGrade = {
                        Id: 0,
                        TestID: headerid,
                        ObtainedMarks: EnteredMarks
                    };
                    rowData.ObtainedMarkData.push(obtainedGrade);
                }
            });

            tableData.push(rowData);
        });

        if (hasValidationError) {
            alert("Please check filled marks are not greater than the maximum mark.");
            return false;
        }

        function sendBatchData(batchData) {
            return $.ajax({
                url: '/Exam/InsertUpdateObtainedMarks',
                type: 'POST',
                data: JSON.stringify({ rowData: batchData, staffId: staffid }),
                contentType: 'application/json'
            });
        }

        let batchSize = 50;
        let totalBatches = Math.ceil(tableData.length / batchSize);
        let batchRequests = [];

        // Block UI
        $.blockUI({
            message: '<h3>Processing... Please wait, do not refresh page</h3>',
            css: {
                padding: '10px',
                backgroundColor: '#000',
                color: '#fff',
                borderRadius: '5px'
            }
        });

        for (let i = 0; i < totalBatches; i++) {
            let batchData = tableData.slice(i * batchSize, (i + 1) * batchSize);
            batchRequests.push(sendBatchData(batchData));
        }

        Promise.all(batchRequests)
            .then(results => {
                $.unblockUI(); // Unblock UI
                alert("Marks Recorded Successfully!");
                window.location.reload();
            })
            .catch(error => {
                $.unblockUI(); // Unblock UI on error
                alert("An error occurred while saving marks.");
                console.error(error);
            });
    });


})

