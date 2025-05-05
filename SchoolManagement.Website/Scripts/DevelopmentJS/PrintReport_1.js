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




    const selectedValue = TermId;
    const splitValues = splitValue(selectedValue);
    const _Stid = parseInt(splitValues.id);

    if (splitValues) {      
        if (splitValues.name === "UT") {
            $("#UnitName").text(splitValues.id);

            $("#UnitRecord").show();
            $("#MarksTable").hide();
            $("#TermRecord").hide();
            $("#PreBoardRecord").hide();
            $("#coScholasticTable").hide();
            // $("#coScholasticPreBoardTable").hide();
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
            $(".preBoardResult").hide();

            $("#coScholasticPreBoardTable").hide()


        }


    }
    //New
    fetch('/Exam/PrintReportCardData?studentId=' + number + '&termId=' + _Stid, {
        method: 'GET',
        headers: {
            'Content-Type': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
           
            if (data.subjectDatas && Array.isArray(data.subjectDatas)) {
                debugger;
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
                    if (subjectData.GradeUT2 == "D") {
                        newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalMarksBothUTs) + '<span style="color:red"> *</span></td>';
                    } else {
                        newRow.innerHTML += '<td>' + checkForZero(subjectData.TotalMarksBothUTs) + '</td>';
                    }                   
                    newRow.innerHTML += '<td>' + subjectData.FinalGrade + '</td>';

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
                
                if (data.totalResult.OverallGrade == "D") {
                    totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.OverallAllTotal) + '</b><span style="color:red"> *</span></td>';
                } else {
                    totalRow.innerHTML += '<td><b>' + checkForZero(data.totalResult.OverallAllTotal) + '</b></td>';
                }
                totalRow.innerHTML += '<td><b>' + data.totalResult.OverallGrade + '</b></td>';
                tableBody.appendChild(totalRow);

                //additional rows
                // Add the additional rows for percentages and other subjects
                      
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
                    data.totalResultPercentage.OverallGrade,
                    true
                );
                tableBody.appendChild(percentagesRow);
                $("#overallGrade").text(data.totalResultPercentage.OverallGrade);
                $("#teacherRemarkText").text(data.Remark);
                //alert($("#overallGrade").text());
                // if (parseFloat(data.totalResultPercentage.OverallAllTotal) < 33) {
                if ($("#overallGrade").text() === '') {
                    $("#result").text("");


                } else {
                    $("#result").text("Pass");
                    // $("#promotedClass").text(data.promotedClass);

                }

                var isPass = true;
                subjectDatas.forEach(subjectData => {
                    if (subjectData.GradeUT1 == "" || subjectData.GradeUT1 == "D"
                        || subjectData.MarksUT1Grade == "" || subjectData.MarksUT1Grade == "D"
                        || subjectData.MarksUT2Grade == "" || subjectData.MarksUT2Grade == "D"
                        || subjectData.GradeUT2 == "" || subjectData.GradeUT2 == "D") {
                        isPass = false;
                        return;
                    } else {
                        isPass = true; //edit by upendra
                    }
                });
                if (isPass) {
                    $("#result").text("Pass");
                } else {
                    $("#result").text("");
                    //$("#overallGrade").text("")
                }

                if ($.inArray('D', subjectDatas) != -1) {
                    $("#result").text("");
                }
                data.optionalSubjectDatas.forEach(optionalSubjectData => {
                    var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT1)),
                        getOptionalGradeForAllReports(optionalSubjectData.MarksUT1Grade), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2)),
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.MarksUT2Grade)), " ",
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarks)),
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarks)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT1)),
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TheoryMarksUT2)),
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.PracticalMarksUT2)), getOptionalGradeForAllReports(checkForZero(optionalSubjectData.TotalObtainedMarksUT2)),
                        getOptionalGradeForAllReports(checkForZero(optionalSubjectData.GradeUT2)), false);
                    tableBody.appendChild(moralScienceRowUT1);
                });               
                //var moralScienceRow = createAdditionalRow("Moral Science", "-", "-", "", "-", "-", "-", "-", "-", "-", "-", "-", "", "", false);
                //tableBody.appendChild(moralScienceRow);

                //var gkRow = createAdditionalRow("G.K", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(gkRow);

                //var drawingRow = createAdditionalRow("Drawing", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(drawingRow);

                //var supwRow = createAdditionalRow("SUPW", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", "-", false);
                //tableBody.appendChild(supwRow);               
                if (splitValues.name == "Term" || splitValues.name == "PreBoard") {
                    debugger;
                    addRows(data.coscholasticAreaDatas, splitValues.id);

                }

                if (splitValues.name == "All") {  
                    debugger;
                    addRows(data.coscholasticAreaDatas, 10);
                }               
                //For Unit 1                
                if (splitValues.id === "1") {
                    debugger;
                    $("#teacherRemark").hide();
                    $("#UnitTestRemark").show();
                    $("#UnitTestRemarkText").text(data.Remark);
                    // $("#UTMaxMarkTotal").text("(" + (data.totalResult.UT1MaxTotal / subjectDataLength) + ")");
                    
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
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.MarksUT1Grade == "" || subjectData.MarksUT1Grade == "D") {
                            isPass = false;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (isPass) {
                        $("#result").text("Pass");
                    } else {
                        $("#result").text("");
                        //$("#overallGrade").text("")
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
                    debugger;
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
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.MarksUT2Grade == "" || subjectData.MarksUT2Grade == "D") {
                            isPass = false;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (isPass) {
                        $("#result").text("Pass");
                    } else {
                        $("#result").text("");
                        //$("#overallGrade").text("")
                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.MarksUT2),
                            optionalSubjectData.MarksUT2Grade, false);
                        tableBodyUT1.appendChild(moralScienceRowUT1);
                    });

                }
                //For Term 1
                else if (splitValues.id === "3" && splitValues.name === "Term") {
                    debugger;
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

                    subjectDatas.forEach(subjectData => {
                        debugger;
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
                    totalRowUT1.innerHTML += '<td><b>' + checkForZero(data.totalResult.T1AllTotal) + '</b></td>';
                    totalRowUT1.innerHTML += '<td><b>' + data.totalResult.T1Grade + '</b></td>';
                    tableBodyTerm1.appendChild(totalRowUT1);

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
                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        debugger;
                        if (subjectData.GradeUT1 == "" || subjectData.GradeUT1 == "D") {
                            isPass = false;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (isPass) {
                        $("#result").text("Pass");
                    } else {
                        $("#result").text("");
                        //$("#overallGrade").text("")
                    }

                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarks),
                            getOptionMarkGrade(optionalSubjectData.PracticalMarks), getOptionMarkGrade(optionalSubjectData.TotalObtainedMarks),
                            optionalSubjectData.GradeUT1, false);
                        tableBodyTerm1.appendChild(moralScienceRowUT1);
                    });

                }
                else if (splitValues.id === "4" && splitValues.name === "Term") {
                    debugger;
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
                    // if (parseFloat(data.totalResultPercentage.TheoryTotalT2) < 33) {
                    if ($("#overallGrade").text() === '') {
                        $("#result").text("");
                        // $("#teacherRemark").hide();
                    } else {
                        $("#result").text("Pass");
                        // $("#promotedClass").text(data.promotedClass);

                    }


                    var isPass = true;
                    subjectDatas.forEach(subjectData => {
                        if (subjectData.GradeUT2 == "" || subjectData.GradeUT2 == "D") {
                            isPass = false;
                        } else {
                            isPass = true; //edit by upendra
                        }
                    });
                    if (isPass) {
                        $("#result").text("Pass");
                    } else {
                        $("#result").text("");
                        //$("#overallGrade").text("")
                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksUT2),
                            getOptionMarkGrade(optionalSubjectData.PracticalMarksUT2), getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksUT2),
                            optionalSubjectData.GradeUT2, false);
                        tableBodyTerm2.appendChild(moralScienceRowUT1);
                    });



                }
                else if (splitValues.id === "7" && splitValues.name === "PreBoard" && _Stid != "10") {
                    debugger;
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
                    debugger;
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
                        // $("#promotedClass").text(data.promotedClass);

                    }
                    data.optionalSubjectDatas.forEach(optionalSubjectData => {
                        var moralScienceRowUT1 = createAdditionalRow(optionalSubjectData.Subject, getOptionMarkGrade(optionalSubjectData.TheoryMarksPre2), getOptionMarkGrade(optionalSubjectData.PracticalMarksPre2),
                            getOptionMarkGrade(optionalSubjectData.TotalObtainedMarksPre2), optionalSubjectData.GradePre2, false);
                        tableBodyPreBoard1.appendChild(moralScienceRowUT1);
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
            } else {
                console.log("Invalid API response format");
            }
        })
        .catch(error => {
            document.getElementById("loader-overlay").style.display = "none";
            console.log("API call failed: " + error);
        });
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
$(document).ready(function () {

    processNumbers();
    const numbersString1 = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
    const numbers1 = JSON.parse(numbersString1);
    setTimeout(createAllPrint, 5000 * numbers1.length);
})


async function processNumbers() {
    document.getElementById("loader-overlay").style.display = "flex";
    const numbersString = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
    const numbers = JSON.parse(numbersString); // Parse the string into an array
    for (const { rollno, StudentId } of numbers) {
        const data = await fetchData(rollno, StudentId);
        await new Promise(resolve => setTimeout(resolve, 3000)); // Introduce a delay

        // Call your function to create JSON
        Creatjson(StudentId);
    }
}
//setInterval(Creatjson(), 2000);
function Creatjson(studentId) {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    const divContent = $('#JsonPrint').html();
    const utf8String = unescape(encodeURIComponent(divContent));

    const base64String = btoa(utf8String);

    const jsonData = {
        content: base64String
    };

    const jsonString = JSON.stringify(jsonData);

    try {
        const key = studentId; // Replace with your desired key
        //  const content = 'base64EncodedContent'; // Replace with your base64 content
        const requestData = {
            key: key,
            content: base64String,
            fileName: id
        };

        const response = fetch('/Exam/SaveJson', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(requestData),

        }).then(response => response.text())
            .then(result => console.log(result))
            .catch(error => console.log('error', error));
        //console.log(response)
        //if (response.ok) {
        //    console.log('JSON data sent to the server.');
        //} else {
        //    console.log('Failed to send JSON data to the server.');
        //}
    } catch (error) {
        document.getElementById("loader-overlay").style.display = "none";
        console.error('An error occurred:', error);
    }

}

async function createAllPrint() {
    const urlParam = new URLSearchParams(window.location.search);
    const id = urlParam.get('id');

    try {
        const response = await fetch('/Exam/GetEncodedContent?id=' + id);
        const data = await response.json();
        // const keysInOriginalOrder = [171, 209, 173, 175, 216]; // Replace with your desired order
        const numbersString1 = localStorage.getItem("PrintIds"); // Retrieve the string from localStorage
        const numbers1 = JSON.parse(numbersString1);
        var keysInOriginalOrder = [];

        // Loop through each object in the data array and extract StudentId
        for (var i = 0; i < numbers1.length; i++) {
            var studentId = numbers1[i].StudentId;
            keysInOriginalOrder.push(studentId);
        }
        const jsonData = JSON.parse(data);

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

        document.getElementById("loader-overlay").style.display = "none";
    } catch (error) {
        document.getElementById("loader-overlay").style.display = "none";
        console.log(error);
    }
}

async function processNextKey() {
    return new Promise(resolve => {
        setTimeout(resolve, 1000); // Adjust the delay as needed
    });
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
var i = 0
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
    debugger;
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
    debugger;
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
        debugger;
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
            // row.appendChild(createCell(""));
            //row.appendChild(createCell(""));
        }
    }

    // Second set of columns
    if (data2) {
        debugger;
        row.appendChild(createCell(data2.Name, true)).classList.add("text-start", "ps-1");
        if (Term == 3) {
            row.appendChild(createCell(data2.GradeTerm1));
        }
        else if (Term == 4) {
            row.appendChild(createCell(data2.GradeTerm2));
        }
        else if (Term == 7) {
            row.appendChild(createCell(data2.GradePre1));
        }
        else if (Term == 8) {
            row.appendChild(createCell(data2.GradePre2));
        }
        else {
            row.appendChild(createCell(data2.GradeTerm1));
            row.appendChild(createCell(data2.GradeTerm2));
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
            //row.appendChild(createCell(""));
            //row.appendChild(createCell(""));
        }
    }

    return row;
}
