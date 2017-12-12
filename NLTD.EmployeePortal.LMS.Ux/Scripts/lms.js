function LoadDashboardCalender() {
    $("#fullcalender").fullCalendar({
        selectable: true,
        selectHelper: true,
        firstDay: 0
        //,
        //select: function (start, end) {
        //    alert(start);
        //}
    });
    $(".fc-sat").css("background-color", "#F4F3EA");
    $(".fc-sun").css("background-color", "#F4F3EA")
}

function InitDateTimeDropdown(varLeaveFromTime, varLeaveUptoTime) {
    if (varLeaveFromTime != "") {
        $("#LeaveFromTime").val(varLeaveFromTime);
    }
    if (varLeaveUptoTime != "") {
        $("#LeaveUptoTime").val(varLeaveUptoTime);
    }
}
function SubmitForm(e) {
    $('.close').click()
    if ($("#Reason").val() == "") {
        $('#alert_placeholder').append('<div id="alertDivId" class="alert alert alert-danger"><a class="close" data-dismiss="alert">×</a><span>' + "Please provide the reason for this request." + '</span></div>')
        return;
    }

    if (isTimeBasedLayout() == false) {
        $("#IsTimeBased").val("Day");
        var msg = "Are you sure you want to apply '" + $("#LeaveType option:selected").text() + "' for " + $("#NumberOfDays").val() + " day(s)?."
    }
    else {
        if ($("#PermissionTimeFrom").val() == "" || $("#PermissionTimeTo").val() == "") {
            $('#alert_placeholder').append('<div id="alertDivId" class="alert alert alert-danger"><a class="close" data-dismiss="alert">×</a><span>' + "Please select the correct time duration for the request." + '</span></div>')
            return;
        }
        $("#IsTimeBased").val("Time");
        var msg = "Are you sure you want to apply '" + $("#LeaveType option:selected").text() + "' from " + $("#PermissionTimeFrom").val() + " to " + $("#PermissionTimeTo").val() + " ?."
    }
    var msg =
    bootbox.confirm({
        title: "LMS Request Confirm",
        message: msg,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm'
            }
        },
        callback: function (result) {
            if (result == true) {
                
                $("#frmSubmitLeave").submit();
               
            }

        }
    });

}
function SubmitEmpForm(e) {
    $('.close').click()
    if ($("#Mode").val() == "Add") {
        var msg = "Are you sure you want to add new employee profile?"
    }
    else {
        var msg = "Are you sure you want to update employee profile?"
    }
    bootbox.confirm({
        title: "Employee Profile Confirm",
        message: msg,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm'
            }
        },
        callback: function (result) {
            if (result == true) {
                $("#frmEmployee").submit();
            }
           
        }
    });
   
}
function applyComplete(data) {
    if (data.responseJSON == "Saved") {
        resMessage =  "Request submitted Successfully.";
        showalert("", resMessage, "alert alert-success");
        loadApplyLeaveSummary();
    }
    else if (data.responseJSON == "EmailFailed") {
        resMessage = "Request submitted Successfully,but sending email failed. Please inform your Reporting Person.";
        showalert("", resMessage, "alert alert-success");
        loadApplyLeaveSummary();
    }
    else {
        resMessage = data.responseJSON;
        showalert("", resMessage, "alert alert-danger")
    }
}
    

function toggleIcon(e) {
    $(e.target)
        .prev('.panel-heading')
        .find(".more-less")
        .toggleClass('glyphicon-plus glyphicon-minus');
}
$('.panel-group').on('hidden.bs.collapse', toggleIcon);
$('.panel-group').on('shown.bs.collapse', toggleIcon);

$(function () {
    $('#accordion').on('shown.bs.collapse', function (e) {
        var offset = $(this).find('.collapse.in').prev('.panel-heading');
        if (offset) {
            $('html,body').animate({
                scrollTop: $(offset).offset().top -10
            }, 200);
        }
    });
});

function returnArray() {
    var offStr = $("#hdnWeekOff").val();
    if (offStr.length > 0) {
        var arr = offStr.split(',');
    }
    else
        return [];
    return arr;
}
function returnHolidayArray() {
    var offStr = $("#hdnHolidays").val();
    if (offStr.length > 0) {
        var arr = offStr.split(',');
    }
    else
        return [];
    return arr;
}

function returnHolidayArray() {
    var offStr = $("#hdnHolidays").val();
    if (offStr.length > 0) {
        var arr = offStr.split(',');
    }
    else
        return [];
    return arr;
}
function loadLeaveSummary(userId)
{
    $.ajax({
        type: 'GET',
        cache:false,
        url: "/Leaves/LoadLeaveSummaryFull",
        data: { "userId": userId },
        success: function (data) {
            $('#divForCreate' + userId).html(data);
        }
    });
    
}
function loadApplyLeaveSummary() {
  
        $.ajax({
            type: 'GET',
            cache: false,
            url: "/Leaves/LoadApplyLeaveSummary",
            success: function (data) {
                $('#divLeaveSummaryInApply').html(data);
            }
        });


}

function loadPendingLeaves() {

    if ($("#btnSearchPending").length==1) {

        $.ajax({
            type: 'GET',
            cache: false,

            beforeSend: function () {
                $("#divLoading").show();
            },
            url: "/Leaves/LoadManageLeavePartial",
            data: { "ShowOnlyReportedToMe": $("#OnlyReportedToMe").prop('checked'), "ShowApprovedLeaves": $("#ShowApprovedLeaves").prop('checked'), "FromDate": $('#FromDate').val(), "ToDate": $('#ToDate').val() },
            success: function (data) {
                $('#divForPendingLeave').html(data);
            },
            complete: function () {
                $("#divLoading").hide();

            },
            error: function () {
                $("#divLoading").hide();

            }
        });
    }

}
function loadViewHistoryLeaves() {

    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }
    if ($("#IsLeaveOnly").val() == undefined) {
        var leaveOnly = false;
    }
    else {
        var leaveOnly = $("#IsLeaveOnly").prop('checked');
    }

    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }

    $.ajax({
        type: 'GET',
        cache: false,
        beforeSend: function () {
            $("#divLoading").show();
        },
        url: "/Leaves/ViewLeaveHistory",
        data: {            
            "OnlyReportedToMe": showTeam,
            "FromDate" : $("#FromDate").val(),
            "ToDate": $("#ToDate").val(),
            "IsLeaveOnly": leaveOnly,
            "Name":name,
            "RequestMenuUser" : $("#RequestLevelPerson").val()

        },
        success: function (data) {
            $('#divForHistoryLeave').html(data);
        },
        complete: function () {
            $("#divLoading").hide();
            if ($("#RequestLevelPerson").val() == "My") {
                $('[id^=collapse]').collapse("show");
            }
        },
        error: function () {
            $("#divLoading").hide();

        }
    });

}
function loadTeamProfiles() {

    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }
    if ($("#HideInactiveEmp").val() == undefined) {
        var hideInactive = true;
    }
    else {
        var hideInactive = $("#HideInactiveEmp").prop('checked');
    }

    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }

    $.ajax({
        type: 'GET',
        cache: false,
        beforeSend: function () {
            $("#divLoading").show();
        },
        url: "/Profile/TeamProfileData",
        data: {
            "onlyReportedToMe": showTeam,           
            "name": name,
            "requestMenuUser": $("#RequestLevelPerson").val(),
            "hideInactiveEmp": hideInactive

        },
        success: function (data) {
            $('#divEmpProfile').html(data);
        },
        complete: function () {
            $("#divLoading").hide();
            if ($("#RequestLevelPerson").val() == "My") {
                $('[id^=collapse]').collapse("show");
            }
        },
        error: function () {
            $("#divLoading").hide();

        }
    });

}


function loadYearwiseLeaveSummary() {
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }

    $("#divLoading").show();
    $("#divForLeaveSummary")
  .load('/Admin/loadYearwiseLeaveSummary?Year=' + $("#Year").val() + '&reqUsr=' + $("#RequestLevelPerson").val() + '&Name=' + name + '&OnlyReportedToMe=' + showTeam,
  function () {
      $("#table_id").dataTable()
      $("#divLoading").hide();
      $('html, body').animate({
          scrollTop: 210  // Means Less header height
      }, 400);
  });
}
function loadMonthwiseCount() {
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }
    $("#divLoading").show();
    $("#divForLeaveMonthCount")
  .load('/Admin/LoadMonthWiseLeaveCount?Year=' + $("#Year").val() + '&reqUsr=' + $("#RequestLevelPerson").val() + '&Name=' + name + '&OnlyReportedToMe=' + showTeam,
  function () {
      $('#Monthwisetable_id').DataTable({
          "scrollX": true,
          "bSort": false,
          "bAutoWidth": false,
          fixedColumns:   {
              leftColumns: 2,              
          },
          "columnDefs": [
            { "width": "200px", "targets": 1 }
          ],
      });
      
      $("#divLoading").hide();

      $('html, body').animate({
          scrollTop: 210  // Means Less header height
      }, 400);
  });
}
function callProfileEdit() {
    $('.close').click()
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
        $.ajax({
            method: "POST",
            //beforeSend: function () {
            //    $("#divLoading").show()
            //},
            url: '/Profile/CallProfileEdit?Name=' + $("#Name").val(),
            data:{name:name},
            success: function (result) {
                if (result == "InvalidName") {
                    showalert("", "Employee profile not found for the entered name.", "alert alert-danger")
                }
                else {
                    window.location.href = result.redirectToUrl
                }
            },           
            error: function () {                
            }

        });
}
function callProfileView() {
    $('.close').click()
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    $.ajax({
        method: "POST",
        //beforeSend: function () {
        //    $("#divLoading").show()
        //},
        url: '/Profile/CallProfileView?Name=' + $("#Name").val(),
        data: { name: name },
        success: function (result) {
            if (result == "InvalidName") {
                showalert("", "Employee profile not found for the entered name.", "alert alert-danger")
            }
            else {
                window.location.href = result.redirectToUrl
            }
        },
        error: function () {
        }

    });
}
function callApplyFor() {
    $('.close').click()
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    $.ajax({
        method: "POST",
        //beforeSend: function () {
        //    $("#divLoading").show()
        //},
        url: '/Leaves/CallApplyFor',
        data: { name: name },
        success: function (result) {
            if (result == "InvalidName") {
                showalert("", "Employee profile not found for the entered name.", "alert alert-danger")
            }
            else {
                window.location.href = result.redirectToUrl
            }
        },
        error: function () {
        }

    });
}
function loadPendingCount() {
    if ($("#hdnIsMLSApprvr").val() == "True") {

            $.ajax({
                method: "GET",
                url: '/DashBoard/LoadPendingCount',
                cache:false,
                success: function (response) {
                    $('#divPendingCount').html(response);

                },
                complete: function () {
                    
                },
                error: function () {
                    
                }

            });
    }
}
function loadDaywiseLeaves() {

    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }
    if ($("#IsLeaveOnly").val() == undefined) {
        var leaveOnly = false;
    }
    else {
        var leaveOnly = $("#IsLeaveOnly").prop('checked');
    }
    if ($("#DonotShowRejected").val() == undefined) {
        var donotshowRejected =false;
    }
    else {
        var donotshowRejected = $("#DonotShowRejected").prop('checked');
    }
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }


    $("#divLoading").show();
    $("#divForDaywiseLeave")
  .load('/Admin/loadDaywiseLeaves?Name=' + name + '&FromDate=' + $("#FromDate").val() + '&ToDate=' + $("#ToDate").val() + '&IsLeaveOnly=' + leaveOnly + '&OnlyReportedToMe=' + showTeam + '&reqUsr=' + $("#RequestLevelPerson").val() + '&DonotShowRejected=' + donotshowRejected,
  function () {
      $("#Daywisetable_id").dataTable()
      $("#divLoading").hide();     
      $('html, body').animate({
          scrollTop: 230  // Means Less header height
      }, 400);
  });

}
function loadPermissionDetail() {

    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }
    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    $("#divLoading").show();
    $("#divForPermissionDetail")
  .load('/Admin/GetPermissionDetail?Name=' + name + '&reqUsr=' + $("#RequestLevelPerson").val() + '&startDate=' + $("#FromDate").val() + '&endDate=' + $("#ToDate").val() + '&OnlyReportedToMe=' + showTeam,
  function () {
      $("#Permissions_id").dataTable()
      $("#divLoading").hide();
      $('html, body').animate({
          scrollTop: 230 // Means Less header height
      }, 400);
  });
}

//function loadPermissionDetail() {
//    $.ajax({
//        method: "GET",
//        beforeSend: function () {
//            $("#divLoading").show()
//        },
//        url: '/Admin/GetPermissionDetail?Name=' + $("#Name").val() + '&Year=' + $("#Year").val() + '&reqUsr=' + $("#RequestLevelPerson").val(),
//        async: false,
//        success: function (response) {
//            $('#divForPermissionDetail').html(response);
            
//        },
//        complete: function () {
//            $("#divLoading").hide();
//        },
//        error: function () {
//            $("#divLoading").hide();
//        }

//    });
//}


function hideLeaveSplit(e) {

    $("#LeaveDtlSplit"+e).css("display", "none");

}
function hideLeaveCalculation() {

    $("#LeaveDtlSplit").css("display", "none");

}

function showLeaveDtlSplit(e) {
    $("#divLoading").show();

    $("#LeaveDtlSplit" + e)
.load('/Leaves/ShowLeaveDetail?LeaveId=' + e,
function () {

    $("#divLoading").hide();
    $("#LeaveDtlSplit" + e).toggle("fast");
});
}

function LoadLeaveDtlSplit() {
    $("#divLoading").show();
    
    $("#LeaveDtlSplit")
.load('/Leaves/GetLeaveDetailCalculation?LeaveFrom=' + $("#LeaveFrom").val() + '&LeaveUpto=' + $("#LeaveUpto").val() + '&LeaveFromTime=' + $("#LeaveFromTime").val() + '&LeaveUptoTime=' + $("#LeaveUptoTime").val() + '&LeaveTyp=' + $("#LeaveType").val(),
function () {
    
    $("#divLoading").hide();
    $('#LeaveDtlSplit').toggle("fast");
    
});


        //$.ajax({
        //    method: "GET",
        //    beforeSend:function(){

        //        $("#divLoading").show();
        //    },
        //    url: '/Leaves/GetLeaveDetailCalculation?LeaveFrom=' + $("#LeaveFrom").val() + '&LeaveUpto=' + $("#LeaveUpto").val() + '&LeaveFromTime=' + $("#LeaveFromTime").val() + '&LeaveUptoTime=' + $("#LeaveUptoTime").val(),
        
        //   async:false,
        //    dataType:'html',
        //    success: function (response) {
               
        //        $('#LeaveDtlSplit').html(response);
                
        //    },
        //    complete: function () {
        //        $("#divLoading").hide();
        //    },
        //    error: function () {

        //        $("#divLoading").hide();
        //    }

        //});

        
    }


function hideSplitDiv() {

    $('#LeaveDtlSplit').toggle();
}


function ApproveLeave(LeaveId, userId) {
    $("[id=alertDivId]").hide();
    var obj = new Object();
    obj.LeaveId = LeaveId;
    obj.Comment = $("#Comment_" + LeaveId).val();
    obj.Status = "A";
    obj.userId = userId;
    if (obj.Comment == "") {
        showalert(userId, "Please enter approver's comment.", "alert alert-danger")
        return;
    }

    ChangeStatus(obj);
}
function CancelApprovedLeave(LeaveId, userId) {
    $("[id=alertDivId]").hide();
    var obj = new Object();
    obj.LeaveId = LeaveId;
    obj.Comment = $("#Comment_" + LeaveId).val();
    obj.Status = "C";
    obj.userId = userId;
    if (obj.Comment == "") {
        showalert(userId, "Please enter approver's comment.", "alert alert-danger")
        return;
    }

    ChangeStatus(obj);
}
function RejectLeave(LeaveId, userId) {
    $("[id=alertDivId]").hide();
    var obj = new Object();
    obj.LeaveId = LeaveId;
    obj.Comment = $("#Comment_" + LeaveId).val();
    if (obj.Comment == "") {
        showalert(userId, "Please enter approver's comment.", "alert alert-danger")
        return;
    }
    obj.Status = "R";
    obj.userId = userId;
    ChangeStatus(obj);
}
function ChangeStatus(obj) {
    
    var resMessage = "";
    var LeaveId = obj.LeaveId;
    $.ajax({
        method: "post",
        url: "/Leaves/ChangeStatus",
        data: obj,
        type: "json",
        beforeSend: function () {
            $("#divLoading").show();
        },
        success: function (response) {
            if (response == "Saved") {
                $("#Request-" + LeaveId).hide();
                loadLeaveSummary(obj.userId);
            }
            if (response = "Saved") {
                $("#divLoading").hide();
                if (obj.Status == "A") {
                    resMessage = "Request approved successfully."
                }
                else if (obj.Status == "R") {
                    resMessage = "Request rejected successfully."
                }
                else if (obj.Status == "C") {
                    resMessage = "Request cancelled successfully."
                }
                showalert(obj.userId,resMessage, "alert alert-success")
                
            }
            else if (response = "EmailFailed") {
                $("#divLoading").hide();
                if (obj.Status == "A") {
                    resMessage = "Request approved successfully, but Email sending failed."
                }
                else if (obj.Status == "R") {
                    resMessage = "Request rejected successfully, but Email sending failed."
                }
                else if (obj.Status == "C") {
                    resMessage = "Request cancelled successfully, but Email sending failed."
                }
                showalert(obj.userId, resMessage, "alert alert-success")
            }
            else if (response = "NotSaved") {
                $("#divLoading").hide();
                resMessage = "Leave status not changed.";
                showalert(obj.userId, resMessage, "alert alert-danger")
            }
            
        }
    });
}

function showalert(userId,message, alerttype) {
    if (userId == "") {
        $('#alert_placeholder').append('<div id="alertdiv" class="alert ' + alerttype + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>')
    }
    else {
        var alertDivId = "alertdiv" + userId;
        $('#alert_placeholder' + userId).append('<div id=alertDivId class="alert ' + alerttype + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>')
    }
   
}

/*-- Data Fetching --*/
function LoadReportToDropDown() {
    $("#ReportToId").html("");
    var obj = new Object();
    obj.LocationId = $("#LocationId").val();
    $.ajax({
        url: "/Data/GetReportToList",
        method: "post",
        data: obj,
        success: function (data) {
            var options = "";
            $.each(function (data) {
                options = options + "<option value='" + data.Key + "'>" + data.Value + "</option>";
            });
            $("#ReportToId").html(options);
        }
    });
}

function isTimeBasedLayout() {
    
    var arr = ($("#hdnTimebasedLeaveTypeIds").val()).split(',');
    var found = $.inArray($("#LeaveType").val(), arr);
    if (found == -1)
        return false;
    else
        return true;

}
function hourEntryLayout() {
    
    if (isTimeBasedLayout()==true) {
        $(".duration").hide();
        $(".timeentry").show();
        $('#PermissionTimeFrom').timepicker({ 'scrollDefault': '10am'});
        $('#PermissionTimeTo').timepicker({ 'scrollDefault': '10am' });
        $("#LeaveUpto").val($("#LeaveFrom").val())
    }
    else{
        $(".duration").show();
        $(".timeentry").hide();
    }
    
    //if ($("#LeaveType option:selected").text().indexOf("Sick") > 0) {
    //    $("divSickLeaveMsg").st
    //}
    //else{        
    //        $("divSickLeaveMsg").hide();
    //}
    hideRuleText()
}
function hideRuleText() {

    if ($('#LeaveType :selected').text().indexOf("Sick") !=-1)
        $('#divSickLeaveMsg > p').html("* If sick leave is more than 3 days, submit medical certificate.");
    else if ($('#LeaveType :selected').text().indexOf("Compensatory Off") !=-1)
        $('#divSickLeaveMsg > p').html("* In Reason box, enter the date against which the Compensatory Off is to be availed.");
    else 
        $('#divSickLeaveMsg > p').html("");
}
function CountLeaveDays() {

    if ($("#LeaveFrom").val() == $("#LeaveUpto").val()) {
        if ($("#LeaveUptoTime").val() == "F") {
            $("#LeaveUptoTime").val("A");            
        }
    }
    hideElementsForHalfDay();

    if (isTimeBasedLayout() == true) {

        $("#LeaveUpto").val($("#LeaveFrom").val());
    }
    else {
        var duration = 0;
        $.ajax({

            url: "/Leaves/ReturnDuration",
            async: false,
            cache: false,
            method: "post",
            headers: {
                'Cache-Control': 'no-cache, no-store, must-revalidate',
                'Pragma': 'no-cache',
                'Expires': '0'
            },
            beforeSend: function () {

            },
            data: { "LeaveFrom": $("#LeaveFrom").val(), "LeaveUpto": $("#LeaveUpto").val(), "LeaveFromTime": $("#LeaveFromTime").val(), "LeaveUptoTime": $("#LeaveUptoTime").val() },
            success: function (count) {
                duration = count;

            },
            error: function (exception) {

                alert(exception);
            },
            complete: function () {


            }
        });

        $("#NumberOfDays").val(duration);
    }
    
}
function hideElementsForHalfDay() {
    if (isTimeBasedLayout() == false) {
        if ($("#LeaveFromTime").val() == "F") {
            $(".halfday").hide()
            $(".firsthalfonly").hide()
            $("#LeaveUpto").val($("#LeaveFrom").val())

        }
        else {
            $(".halfday").show()
            $(".firsthalfonly").show()
        }
        if ($("#LeaveUpto").val() == $("#LeaveFrom").val()) {

            if ($("#LeaveFromTime").val() != "A") {
                $(".halfday").hide()
            }
            else {
                $(".halfday").show()
            }
        }
        else {
            if ($("#LeaveFromTime").val() != "F") {
                $(".halfday").show()
            }
        }
    }
    
}
//Added by Tamil
function loadLeaveBalanceProfile() {

    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }

    $.ajax({
        type: 'GET',
        cache: false,
        beforeSend: function () {
            $("#divLoading").show();
        },
        url: "/Profile/EmployeeLeaveBalanceDetails",
        data: {
            //"onlyReportedToMe": showTeam,
            "name": name
            //"requestMenuUser": $("#RequestLevelPerson").val(),
            //"hideInactiveEmp": hideInactive

        },
        success: function (data) {
            $('#divEmpProfile').html(data);
        },
        complete: function () {
            $("#divLoading").hide();
        },
        error: function () {
            $("#divLoading").hide();

        }
    });

}

function AddTotalDays(index) {
    var CreditOrDebit = $('#CreditOrDebit' + index).val();
    var ExistingTotalDays = $("#ExistingTotalDays" + index).val();
    var NoOfDays = 0;

    if ($("#NoOfDays" + index).val() == undefined || $("#NoOfDays" + index).val() == '') {
        NoOfDays = 0;
    } else {
        NoOfDays = $("#NoOfDays" + index).val();
    }

    if (NoOfDays > 0) {
        if (CreditOrDebit == 'D' && parseFloat(ExistingTotalDays) < parseFloat(NoOfDays)) {
            Clearshowalert("No of days should be less than Existing Total days", "alert alert-danger");
            $("#NoOfDays" + index).focus();
            return;
        }

        if (CreditOrDebit != '') {

            var Total = (parseFloat(NoOfDays) + parseFloat(ExistingTotalDays)).toFixed(1);
            if (CreditOrDebit == 'D') {
                Total = (parseFloat(ExistingTotalDays) - parseFloat(NoOfDays)).toFixed(1);
            }
            $("#TotalDays" + index).val(Total);
        }
        else {
            $("#TotalDays" + index).val(ExistingTotalDays);
        }
    } else {
        $("#TotalDays" + index).val("");
    }
}

function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function SubmitLeaveBalanceForm(count) {
    var things = [];
    var valid = false;
    for (var i = 0; i < count; i++) {
        var CreditOrDebit = $('#CreditOrDebit' + i).val();

        if ($("#NoOfDays" + i).val() == undefined || $("#NoOfDays" + i).val() == '') {
            NoOfDays = 0;
        } else {
            NoOfDays = $("#NoOfDays" + i).val();
        }

        if (NoOfDays > 0) {
            valid = true;
            var ExistingTotalDays = $("#ExistingTotalDays" + i).val();

            if (CreditOrDebit == 'D' && parseFloat(ExistingTotalDays) < parseFloat(NoOfDays)) {
                Clearshowalert("No of days should be less than Existing Total days", "alert alert-danger");
                $("#NoOfDays" + i).focus();
                return;
            }

            things.push(
                {
                    LeaveType: $("#LeaveType" + i).val(), ExistingTotalDays: ExistingTotalDays,
                    CreditOrDebit: CreditOrDebit, NoOfDays: NoOfDays, TotalDays: $("#TotalDays" + i).val(),
                    LeaveTypeId: $("#LeaveTypeId" + i).val(), LeaveBalanceId: $("#LeaveBalanceId" + i).val(), Remarks: $("#Remarks" + i).val()
                });
        } else {
            $("#TotalDays" + i).val("");
        }
    }

    if (valid) {
        var userid = $("#UserId").val();
        things = JSON.stringify({ 'lst': things, 'EmpUserid': userid });
        //alert(things);
        $.ajax({
            contentType: 'application/json; charset=utf-8',
            dataType: 'json',
            type: 'POST',
            url: '/Profile/SaveLeaveBalance',
            data: things,
            success: function (result) {
                if (result == "Saved") {
                    $("#btnsave").attr("disabled", true);
                    Clearshowalert("Employee Leave Balance saved Successfully.", "alert alert-success");
                }
                else if (result == "Need Role") {
                    $("#btnsave").attr("disabled", true);
                    Clearshowalert("Only the user with role 'HR' is allowed to do this action.", "alert alert-danger");
                }
                else {
                    Clearshowalert(result, "alert alert-danger");
                }
            },
            failure: function (response) {
                Clearshowalert(response.message, "alert alert-danger");
            }
        });
    }
    else {
        Clearshowalert("No of Days value should be more than 0", "alert alert-danger");
        return;
    }
}

function Clearshowalert(message, alerttype) {
    $("#alert_placeholder").empty();
    $('#alert_placeholder').append('<div id="alertdiv" class="alert ' + alerttype + '"><a class="close" data-dismiss="alert">×</a><span>' + message + '</span></div>');
}



function loadTransactionLog() {

    if ($("#OnlyReportedToMe").val() == undefined) {
        var showTeam = false;
    }
    else {
        var showTeam = $("#OnlyReportedToMe").prop('checked');
    }

    if ($("#Name").val() == undefined) {
        var name = "";
    }
    else {
        if ($("#Name").val() != "") {
            var name = $("#Name").val().replace(/ /g, "|");
        }
        else {
            name = "";
        }
    }
    $("#divLoading").show();
    $("#divForHistoryLeave").load('/Admin/GetTransactionLog?OnlyReportedToMe=' + showTeam + '&Name=' + name + '&RequestMenuUser=' + $("#RequestLevelPerson").val(),
        function () {
            $(".transaction").dataTable({ "paging": false, "bFilter": false, "bInfo": false });
            $("#divLoading").hide();
            $('html, body').animate({
                scrollTop: 210  // Means Less header height
            }, 400);
        });

    
}

function loadAttendenceRangeSummary() {
  
        URL = '/Admin/loadEmployeeAttendence?Name=' + $("#Name").val().replace(new RegExp(" ", "g"), '|') + '&FromDate=' + $('#FromDate').val() + '&ToDate=' + $('#ToDate').val() + '&requestLevelPerson=' + $('#RequestLevelPerson').val();
        $("#divLoading").show();
    $("#divForEmployeeAttendence")
        .load(URL,
        function (responseText, textStatus, req) {
            $("#divLoading").hide();
            if (textStatus == "error") {
                Clearshowalert("No Records Found", "alert alert-danger");
                $('#Attendencetable_id').DataTable().clear().destroy();
                }
            else {

                $(".dtatable").dataTable({ "order": [] });
                $('html, body').animate({
                    scrollTop: 230  // Means Less header height
                }, 400);
            }

            });
}
function loadTimeSheetSummary()
{
    debugger
    var URL = '/Admin/LoadMyTimesheet';
    $("#divLoading").show();
    if ($("#RequestLevelPerson").val() === "Team") {
        URL = '/Admin/LoadMyTeamTimesheet';
    }
    $("#divForTimesheet").html("");
    $("#divForTimesheet")
        .load(URL, { TimeSheetQueryModelObj: { FromDate: $("#FromDate").val(), ToDate: $("#ToDate").val(), Name: $("#Name").val() } },
        function (responseText, textStatus, req) {
            if (textStatus == "error") {
                Clearshowalert("No Records Found", "alert alert-danger");
                $('.dtatable').DataTable().clear().destroy();
            }
            else {
                $(".dtatable").dataTable({ "aaSorting": [] });
                $('html, body').animate({
                    scrollTop: 230  // Means Less header height
                }, 400);
            }
            $("#divLoading").hide();

        });
}


function loadShiftDetails() {

    $("#divLoading").show();
    $("#divForShiftAllocation").load('/Shift/GetShiftDetail',
        function () {
            $("#ShiftDetail").dataTable({
                columnDefs: [
                    { targets: 'no-sort', orderable: false }
                ]
            });
            $("#divLoading").hide();
            $('html, body').animate({
                scrollTop: 230 // Means Less header height
            }, 400);
        });
}

function loadShiftMasterDetails() {

    $("#divLoading").show();
    $("#divForShiftAllocation").load('/Shift/GetShiftMasterDetail',
        function () {
            $("#ShiftDetail").dataTable({
                columnDefs: [
                    { targets: 'no-sort', orderable: false }
                ]
            });
            $("#divLoading").hide();
            $('html, body').animate({
                scrollTop: 230 // Means Less header height
            }, 400);
        });
}

function AddShiftPopup(shiftId) {
    $("#alert_placeholder").empty();
    $("#divLoading").show();
    $("#ModelTitle").html("Edit Shift");
    if (shiftId !== 0) {
        $("#ModelTitle").html("Add New Shift");
    }

    $("#divForAddShift").load('/Shift/GetShiftMasterDetailwithId?shiftId=' + shiftId);
    //$('#myModal').on('shown.bs.modal', function (e) {
    //    $('.timepicker').timepicker({
    //        timeFormat: "HH:mm:ss"
    //    }
    //    );
    //});
    $("#myModal").modal('show');
    $("#divLoading").hide();
}
function SaveShiftMaster() {

    var shiftName, fromTime, toTime, shiftId;
    shiftName = $("#ShiftName").val().trim();
    shiftId = $("#ShiftId").val().trim();
    fromTime = $("#fromTime").val().trim();
    toTime = $("#toTime").val().trim();
    if (shiftName == '') {
        Clearshowalert("Please enter the Shift Name", "alert alert-danger");
        return;
    }

    $("#divLoading").show();
    var things = JSON.stringify({ 'shiftId': shiftId, 'shiftName': shiftName, 'fromTime': fromTime, 'toTime': toTime });
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '/Shift/SaveShiftMaster',
        data: things,
        success: function (result) {
            if (result == "Saved") {
                $("#btnsave").attr("disabled", true);
                window.location.reload();
            }
            else if (result === "Need Role") {
                $("#btnsave").attr("disabled", true);
                Clearshowalert("Only the user with role 'HR' is allowed to do this action.", "alert alert-danger");
            }
            else {
                Clearshowalert(result, "alert alert-danger");
            }
        },
        failure: function (response) {
            Clearshowalert(response.message, "alert alert-danger");
        }
    });


    //$("#myModal").modal('show');
    $("#divLoading").hide();
}

function loadShiftDetails() {

    $("#divLoading").show();
    var RequestLevelPerson = $("#RequestLevelPerson").val();

    $("#divForShiftAllocation").load('/Shift/GetShiftDetail?RequestMenuUser=' + RequestLevelPerson,
        function () {
            $("#ShiftDetail").dataTable({
                columnDefs: [
                    { targets: 'no-sort', orderable: false }
                ]
            });
            $("#divLoading").hide();
            $('html, body').animate({
                scrollTop: 230 // Means Less header height
            }, 400);
        });
}

function AddPopup(ShiftMappingId) {
    var IsEdit = !(ShiftMappingId > 0);
    $("#divLoading").show();
    var RequestLevelPerson = $("#RequestLevelPerson").val();

    $("#divForAddShift").load('/Shift/GetShiftDetailsForUsers?RequestMenuUser=' + RequestLevelPerson + '&ShiftMappingId=' + ShiftMappingId,
        function () {
            $("#addShiftDetail").dataTable(
                {
                    "bSort": IsEdit,
                    "paging": IsEdit,
                    "bFilter": IsEdit,
                    "bInfo": IsEdit,
                    "pageLength": 5,
                    "lengthChange": false,
                    columnDefs: [
                        { targets: 'no-sort', orderable: false }
                    ]
                });
            $("#divLoading").hide();
            $('html, body').animate({
                scrollTop: 230, // Means Less header height
            }, 400);
        });

    var title = ShiftMappingId > 0 ? "Edit Employee Shift" : "Add Employees Shift";
    $('#modaltitle').text(title);
    $("#alert_placeholder").empty();
    $("#myModal").modal('show');

}

function toDate(dateStr) {
    var parts = dateStr.split("-");
    return new Date(parts[2], parts[1] - 1, parts[0]);
}
function SaveEmployeeShift() {
    var checkedValues = $("input:checkbox:checked", "#addShiftDetail").map(function () {
        return $(this).val();
    }).get();

    var Shift = $("#Shift").val();
    var FromDate = $("#FromDate").val();
    var ToDate = $("#ToDate").val();

    if (Shift == '') {
        Clearshowalert("Please select the Shift", "alert alert-danger");
        return;
    }

    if (FromDate == '' || ToDate == '') {
        Clearshowalert("Please select From Date and To Date", "alert alert-danger");
        return;
    }

    var from = toDate(FromDate);
    var to = toDate(ToDate);

    if (from > to) {
        Clearshowalert("Invalid Date Range", "alert alert-danger");
        return;
    }

    if (checkedValues == '' || checkedValues == null) {
        Clearshowalert("Please select atleast one employee", "alert alert-danger");
        return;
    }

    var ShiftMappingID = $("#ShiftMappingID").val();

    var input = JSON.stringify({
        'UserId': checkedValues, 'Shift': Shift, 'FromDate': FromDate, 'ToDate': ToDate, 'ShiftMappingID': ShiftMappingID
    });
    //alert(things);
    $.ajax({
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        type: 'POST',
        url: '/Shift/SaveEmployeeShift',
        data: input,
        success: function (result) {
            if (result == "Saved") {
                window.location.reload();
                $('#myModal').modal("close");
            }
            else if (result.indexOf("Available") > -1) {
                result = result.replace('Available', '').replace(/\n/g, "<br />");
                Clearshowalert("Selected employees are having shift already between these dates.<br />" + result, "alert alert-danger");
            }
            else {
                Clearshowalert(result, "alert alert-danger");
            }
        },
        failure: function (response) {
            Clearshowalert(response.message, "alert alert-danger");
        }
    });
}
