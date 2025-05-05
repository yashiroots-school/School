$(document).ready(function () {
    $("#OtherReligious").css('display', 'none');
    $("#btnsave").on("click", function () {
        if (validateFormAgree()) {
            SubmitStudentDetails();
        }
    });
    $("input[name=Religious]").change(function () {
        var Religious = $(this).val();
        if (Religious == 'Other') {
            $("#OtherReligious").css('display', 'block');
        } else {
            $("#OtherReligious").css('display', 'none');
        }
    });
});

function validateFormAgree() {
            var reason = $("#Dec_ReasonforNo").val();
            try {
                if ($("#Dec_interestedinFinalPlacementNoChkbox").is(':checked')) {

                    var msg = "";

                    if (reason == "") {
                        msg += "Reason cannot be blank\r";
                        return false;
                    }
                    if (msg != "") {
                        alert("Please refer to the following error(s) :\r" + msg);
                        return false;
                    }
                    else {
                        //document.getElementById('Studentdetails_btn').click();
                        //Save Student All Details and Submit
                        return true;

                    }
                }
                else {

                    if ($("#Dec_AgreeChkbox").is(":checked")) {
                        //document.getElementById('Studentdetails_btn').click();
                        return true;
                    }
                    else {
                        msg += "Please Select Agree";
                        alert("Please refer to the following error(s) :\r" + msg);
                        return false;
                    }

                }
            } catch (e) {
                return false;
            }
        }

function SubmitStudentDetails() {
    AddStudents();
    Addinternship();
    AddSkillSet();
    Declaration();
    alert("Records Save SuccessFully");
    window.location.href = "/Student/PreRegisterStudent";
}

  function AddStudents() {
    var d = new Date();
      var currentDate = d.getDate() + "/" + (d.getMonth() + 1) + "/" + d.getFullYear();
      
      var StudentDetails = {
          Addeby: currentDate,
          Addedon: "",
          Age: $("#Age").val(),
          Batch: $("#Batchddl option:selected").text(),
          Category: $("#Categoryddl option:selected").text(),
          Course: $("#Courseddl option:selected").text(),
          Class: $("#class option:selected").text(),
          Section: $("#section option:selected").text(),
          DateofBirth: $("#DOB").val(),
          EmailId: $("#Email").val(),
          FacultyMentor: $("#FacultyMentorddl option:selected").text(),
          FatherCompanyName: $("#FatherCompany").val(),
          FatherEmailId: $("#FatherEmail").val(),
          FatherCountryCode: $("#FatherCountrycode").val(),
          FatherMobileNo: $("#FatherMobile").val(),
          FatherName: $("#FatherName").val(),
          CountryCode: $("#Countrycode").val(),
          MobileNo: $("#Mobile").val(),
          FatherProfession: $("#ddlFathenProfession option:selected").text(),
          MotherCompanyName: $("#MotherCompany").val(),
          CorrespondenceAddress: $("#CorrespondenceAddress").val(),
          Sibiling1: $("#Sibiling1").val(),
          Sibiling2: $("#Sibiling2").val(),
          Sibiling3: $("#Sibiling3").val(),
          Sibiling4: $("#Sibiling4").val(),
          Sibiling5: $("#Sibiling5").val(),
          MotherEmailId: $("#MotherEmail").val(),
          MotherCountryCode: $("#MotherCountrycode").val(),
          MotherMobileNo: $("#MotherMobile").val(),
          MotherName: $("#MotherName").val(),
          NativePlace: $("#Native").val(),
          MotherProfession: $("#ddlMotherProfession option:selected").text(),
          ResidenceLocation: $("#ResidenceLocation").val(),
          Years: $("#Yearddl option:selected").text(),
          StudentName: $("#StudentName").val(),
          Updatedby: "",
          Updatedon: "",
          Specialization: $("#Specializationddl option:selected").text(),
          ScholarNumber: $("#ScholarNo").val(),
          status: "Valid",
          Gender: "Female",
          Religious: $("input[name=Religious]:checked").val(),
          ReligiousOther: $("#OtherReligious").val(),
          Hostalite: "No",
          OutStationStudent: "No"
      };

    if ($("#Malechkbox").is(":checked"))
        StudentDetails.Gender = "Male";

    if ($("#HostaliteYesChkbox").is(":checked"))
        StudentDetails.Hostalite = "Yes";

      if (OutStationYesChkbox.Checked == true)
          StudentDetails.OutStationStudent = "Yes";

      else if (OutStationNoChkbox.Checked == true)
          //Native.Enabled  = false;

          //if (SemesterId.Text.Length > 0) {
          //    student.StudentId = Convert.ToInt32(SemesterId.Text);
          //    stdnt.Update(student);
          //}
          //else
          //    stdnt.Add(student);
          setTimeout(function () {
          }, 1500); 
         var stddata = JSON.stringify(StudentDetails);
          setTimeout(function () {
          }, 1500); 
      
       $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: "/Student/AddUpdateStudent",
            data: stddata,
                dataType: "json",
            success: function (data) {
                
            },
           error: function (result) {
               alert("Some Error Occure");
            }
        });

}

function Addinternship() {
    var internship = {
        Addeby: "",
        Addedon: "",
        Updatedby: "",
        Updatedon: "",
        CompanyName: $("#Intern_CompanyName").val(),
        EndDate: $("#Intern_EndDate").val(),
        FacultyGuideMobileNo: $("#Intern_FacultyGuideMobile").val(),
        FacultyProjectGuide: $("#Intern_FacultyProjectGuide").val(),
        Feedback: $("#Intern_Feedback").val(),
        IndustryGuideDesignation: $("#Intern_IndustryGuideDesignation").val(),
        IndustryGuideEmail: $("#Intern_IndustryGuideEmail").val(),
        IndustryGuideMobileNo: $("#Intern_IndustryGuideMobile").val(),
        IndustryGuideName: $("#Intern_IndustryGuideName").val(),
        IndustryGuideTelNo: $("#Intern_IndustryGuideTelNo").val(),
        MobileNo: $("#Intern_Mobile").val(),
        ProjectDescription: $("#Intern_ProjectDescription").val(),
        ProjectTitle: $("#Intern_ProjectTitle").val(),
        ReasonforNoSubmission: $("#Intern_ReasonforNoSubmission").val(),
        ScholarNumber: $("#ScholarNo").val(),
        StartDate: $("#Intern_StartDate").val(),
        StipendinThousands: $("#Intern_Stipend").val(),
        PrePlacementOfferReceived: "No",
        ProjectSubmission: "No"
    };
    if ($("#Intern_Pre_PlacementOfferReceivedYesChkbox").is(":checked")) {
        internship.PrePlacementOfferReceived = "Yes";
    }

    else if ($("#Intern_Pre_PlacementOfferReceivedNoChkbox").is(":checked"))
        internship.PrePlacementOfferReceived = "No";

    if ($("#Intern_ProjectSubmissionYesChkbox").is(":checked"))
        internship.ProjectSubmission = "Yes";

    else if ($("#Intern_ProjectSubmissionNoChkbox").is(":checked"))
        internship.ProjectSubmission = "No";

    //if (SummerInternshipId.Text.Length > 0) {
    //    internship.SummerInternshipId = Convert.ToInt32(SemesterId.Text);
    //    summr.Update(internship);
    //}
    var insData = JSON.stringify(internship);
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/AddUpdateIntership",
        data: insData,
        dataType: "json",
        success: function (data) {
           
        },
        error: function (result) {
            alert("Some Error Occure");
        }
    });

  
}

function AddSkillSet() {
    var no = "No";
    var yes = "Yes";
    var skillset = {
        Addedby: "",
        Updatedon: "",
        Updatedby: "",
        Addedon: "",
        ScholarNumber: $("#ScholarNo").val(),
        Adaptabilitytochange: no,
        Communication: no,
        Compering: no,
        Contentwriting: no,
        CoralDraw: no,
        Dancing: no,
        Drawing: no,
        Initiative: no,
        Interpersonalskills: no,
        Leadership: no,
        Photoshop: no,
        Problemsolving: no,
        Singing: no,
        //SkillsetId
        Strategicthinking: no,
        Teamwork: no,
        Timemanagement: no
    };

    if ($("#Adaptability").is(":checked"))
        skillset.Adaptabilitytochange = yes;

    if ($("#Communicationskills").is(":checked"))
        skillset.Communication = yes;

    if ($("#Compering").is(":checked"))
        skillset.Compering = yes;

    if ($("#CreativeContentwriting").is(":checked"))
        skillset.Contentwriting = yes;

    if ($("#CoralDraw").is(":checked"))
        skillset.CoralDraw = yes;

    if ($("#Dancing").is(":checked"))
        skillset.Dancing = yes;

    if ($("#Drawing").is(":checked"))
        skillset.Drawing = yes;

    if ($("#Initiative").is(":checked"))
        skillset.Initiative = yes;

    if ($("#Interpersonalskills").is(":checked"))
        skillset.Interpersonalskills = yes;

    if ($("#Leadership").is(":checked"))
        skillset.Leadership = yes;

    if ($("#Photoshop").is(":checked"))
        skillset.Photoshop = yes;

    if ($("#Singing").is(":checked"))
        skillset.Singing = yes;

    if ($("#Strategicthinking").is(":checked"))
        skillset.Strategicthinking = yes;

    if ($("#Teamwork").is(":checked"))
        skillset.Teamwork = yes;

    if ($("#Timemanagement").is(":checked"))
        skillset.Timemanagement = yes;

    if ($("#Problemsolving").is(":checked"))
        skillset.Problemsolving = yes;

    //if (SkillsetId.Text.Length > 0) {
    //    skillset.SkillsetId = Convert.ToInt32(SemesterId.Text);
    //    skill.Update(skillset);
    //}
    //else
    //    skill.Add(skillset);
    var skilldata = JSON.stringify(skillset);
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/AddUpdateSkill",
        data: skilldata,
        dataType: "json",
        success: function (data) {
            
        },
        error: function (result) {
            alert("Some Error Occure");
        }
    });
}

function Declaration() {

    var declaration = {
        Addeby: "",
        Updatedon: "",
        Updatedby: "",
        Addedon: "",
        ScholarNumber: $("#ScholarNo").val(),
        Relocate: "",
        NotInterested: $("#Dec_ReasonforNo").val(),
        Agree: "No",
        Interesterd: "No"
    };

    if ($("#Dec_AgreeChkbox").is(":checked"))
        declaration.Agree = "Yes";

    //else if (Dec_DisagreeChkbox.Checked == true)
    //    declaration.Agree = "No";

    if ($("#Dec_interestedinFinalPlacementYesChkbox").is(":checked"))
        declaration.Interesterd = "Yes";
    var declarationdata = JSON.stringify(declaration);
    
    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "/Student/AddUpdateDeclarations",
        data: declarationdata,
        dataType: "json",
        success: function (data) {
        },
        error: function (result) {
            alert("Some Error Occure");
        }
    });
}