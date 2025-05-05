


$(".BtnGradeEdit").on('click', function () {
    var id = $(this).attr("data-val");
    $("#BtnSubmitGrading").text("Update");
    $("#GradingCriteriaForm").attr('action', '/Exam/UpdateGrading');
    GetGradeById(id);
});

function GetGradeById(id) {
    $.ajax({
        url: "/Student/GetGradingById?Id=" + id,
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
    //$(".ObtainedMarks").on("input", function () {
    //    console.log(">>");
    //    var currentValue = parseInt($(this).val());
    //    if (currentValue > 100) {
    //        $(this).val(100); // Set the value back to the maximum allowed value
    //    }
    //});
    //$(".ObtainedMarks").on("blur", function () {
    //    console.log(">>>>>>>>");
    //    var currentValue = parseInt($(this).val());
    //    var maximumMarks = parseInt($(this).attr("max"));
    //    if (currentValue > maximumMarks) {
    //        $(this).val(maximumMarks); // Set the value back to the maximum allowed value
    //    }
    //});

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
        ];

        var missingField = "";
        if ($("#BatchID").val() != 0) {
            if ($("#TermID").val() == 0) {
                alert("Please select a term.");
                return; // Stop execution if term is not selected
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
            return; // Stop execution if any field is missing
        }
        //  debugger
        $.ajax({
            url: "/Student/StudentByClassSection",
            type: "GET",
            dataType: "json",
            data: {
                classId: classid,
                sectionId: sectionid,
                testId: 1,
                termId: termid,
                staffId: staffid,
                subjectId: subjectid,
                batchid: batchid,

            },
            success: function (result) {
                $("#TableBody").empty();

                $.each(result, function (index, student) {
                    var row = $("<tr>");
                    row.append($("<td>").text(index + 1)); // S.No
                    row.append($("<td>").text(student.StudentId)); // Student Id
                    row.append($("<td>").text(student.StudentName)); // Student Name

                    // Add input fields for Remark, Reward, Award, Punishment
                    row.append($("<td>").append($("<input>").attr({
                        type: "text",
                        id: "Remark_" + student.StudentId,
                        placeholder: "Enter Remark"
                    })));
                    row.append($("<td>").append($("<input>").attr({
                        type: "text",
                        id: "Reward_" + student.StudentId,
                        placeholder: "Enter Reward"
                    })));
                    row.append($("<td>").append($("<input>").attr({
                        type: "text",
                        id: "Award_" + student.StudentId,
                        placeholder: "Enter Award"
                    })));
                    row.append($("<td>").append($("<input>").attr({
                        type: "text",
                        id: "Punishment_" + student.StudentId,
                        placeholder: "Enter Punishment"
                    })));

                    $("#TableBody").append(row);
                });

                // Initialize or reinitialize DataTable
                if ($.fn.dataTable.isDataTable('#TableStudent')) {
                    dataTable = $('#TableStudent').DataTable();
                } else {
                    dataTable = $('#TableStudent').DataTable({
                        paging: false
                    });
                }

                // Sticky column for the second column (Student Name)
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
    });

    // Save button click event handler
    //debugger
    $("#SaveButton").on("click", function () {
        var classid = $("#ClassID").val();
        var sectionid = $("#SectionId").val();
        var termid = $("#TermID").val();
        var staffid = $("#Staff_Id").val();
        var batchid = $("#BatchID").val();

        var rowData = [];

        // Loop through each row in your table
        $("#TableBody tr").each(function () {
            var row = $(this);
            var studentId = $(row).find('td:nth-child(2)').text();
            var studentName = $(row).find('td:nth-child(3)').text(); // Student ID (assuming it's in the third column)
            var remark = $(row).find('input[id^="Remark_"]').val(); // Remark value
            var reward = $(row).find('input[id^="Reward_"]').val(); // Reward value
            var award = $(row).find('input[id^="Award_"]').val(); // Award value
            var punishment = $(row).find('input[id^="Punishment_"]').val()

            // Push data for current row into rowData array
            rowData.push({
                Term_Id: termid,
                Class_Id: classid,
                Section_Id: sectionid,
                StudentId: studentId,
                Batch_Id: batchid,
                Remark: remark,
                Reward: reward,
                Awards: award,
                Punishment: punishment
            });
            //console.log(rowData);
        });

        // Validate or process rowData as needed

        // Make AJAX request to send rowData to server
        $.ajax({
            url: "/Student/InsertUpdateObtainedMarks",
            type: "POST",
            dataType: "json",
            contentType: "application/json;charset=UTF-8",
            data: JSON.stringify({
                rowData: rowData,
                staffId: staffid
            }),
            success: function (response) {
                if (response.success) {
                    alert("Data saved successfully");
                } else {
                    alert("Failed to save data");
                }
            },
            error: function (errormessage) {
                alert(errormessage.responseText);
            }
        });
    });

})

