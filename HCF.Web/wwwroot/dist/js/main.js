function showthumImage(input) {
    if (input.files && input.files[0]) {
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
            $('#user_img').attr('src', e.target.result);
        }
        filerdr.readAsDataURL(input.files[0]);
    }
}

function FillBuildings() {
    $("#BuildingID").html("");
    $("#BuildingID").append(
        $('<option></option>').val("").html("Select Building"));
    var propertyId = $('#PropertyId').val();
    if (propertyId != "") {
        $.ajax({
            url: '/Organization/FillBuildings',
            type: "GET",
            dataType: "JSON",
            data: { propertyId: propertyId },
            success: function (buildings) {
                $.each(buildings, function (i, building) {
                    $("#BuildingID").append(
                        $('<option></option>').val(building.BuildingId).html(building.BuildingName));
                });
            }
        });
    }
}

//function FillFloors() {
//    $("#FloorId").html("");
//    $("#FloorId").append(
//        $('<option></option>').val("").html("Select Floor"));
//    var buildingID = $('#BuildingID').val();
//    console.log(buildingID);
//    if (buildingID != "") {
//        $.ajax({
//            url: '/Organization/FillFloors',
//            type: "GET",
//            dataType: "JSON",
//            data: { buildingID: buildingID },
//            success: function (floors) {
//                console.log(floors);
//                $.each(floors, function (i, floor) {
//                    $("#FloorId").append(
//                        $('<option></option>').val(floor.FloorId).html(floor.FloorName));
//                });
//            }
//        });
//    }
//}

function ShowLargeImage(control, floorName, buildingName) {
    swal({
        title: buildingName + "," + floorName,
        text: '<img src="' + $(control).attr('src') + '" alt="Image" height="400" width="100%" />',
        html: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Close",
        closeOnConfirm: true,
    });
}

function FillAssetsSubType() {
    $("#SubTypeId").html("");
    $("#SubTypeId").append(
        $('<option></option>').val("").html("Select Sub Type"));
    var typeId = $('#TypeId').val();
    if (typeId != "") {
        $.ajax({
            url: '/Assets/FillSubTypes',
            type: "GET",
            dataType: "JSON",
            data: { typeId: typeId },
            success: function (subTypes) {
                $.each(subTypes, function (i, subType) {
                    $("#SubTypeId").append(
                        $('<option></option>').val(subType.SubTypeId).html(subType.Name));
                });
            }
        });
    }
}

function ShowEpDetails() {
    $(".epdeatils").empty();

    var typeId = $('#GoalId').val();
    var epId = $('#EPDetailId').val();
    if (typeId != "") {
        $.ajax({
            url: '/Goal/FillEPnumber',
            type: "GET",
            dataType: "JSON",
            data: { goalId: typeId },
            success: function (epNumbers) {
                $.each(epNumbers, function (i, epNumber) {
                    if (epNumber.EPDetailId == epId) {
                        $(".epdeatils").append($('<div>').addClass('large-4 columns first')
                            .append($('<div>').addClass('name').html("<b> EP : </b>" + epNumber.EPNumber))
                            .append($('<div>').addClass('name').html("<b> Main Goal : </b>" + epNumber.MainGoal))
                            .append($('<div>').addClass('name').html("<b> Frequency : </b>" + epNumber.FrequencyMaster.DisplayName))
                            .append($('<div>').addClass('name').html("<b> Description : </b>" + epNumber.Description)));
                    }
                });
            }
        });
    }
}

function FillEPnumber() {
    // alert();
    $("#EPDetailId").html("");
    $(".epdeatils").empty();
    $("#EPDetailId").append(
        $('<option></option>').val("").html("Select Sub Type"));
    var typeId = $('#GoalId').val();
    if (typeId != "") {
        $.ajax({
            url: '/Goal/FillEPnumber',
            type: "GET",
            dataType: "JSON",
            data: { goalId: typeId },
            success: function (epNumbers) {
                $.each(epNumbers, function (i, epNumber) {
                    $("#EPDetailId").append(
                        $('<option></option>').val(epNumber.EPDetailId).html(epNumber.EPNumber));
                });
            }
        });
    }
}

function BindEPs(stdescId) {
    $("#EpDetailId").html("");
    $("#EpDetailId").append(
        $('<option></option>').val("").html("Select Eps"));
    if (stdescId != "") {
        $.ajax({
            url: '/Goal/EpByStandard',
            type: "GET",
            dataType: "JSON",
            data: { stdescId: stdescId },
            success: function (epNumbers) {
                $.each(epNumbers, function (i, epNumber) {
                    $("#EpDetailId").append(
                        $('<option></option>').val(epNumber.EPDetailId).html(epNumber.EPNumber));
                });
            }
        });
    }
}

function bindImage(input) {
    if (input.files && input.files[0]) {
        var filerdr = new FileReader();
        filerdr.onload = function (e) {
            $('#user_img').attr('src', e.target.result);
        }
        filerdr.readAsDataURL(input.files[0]);
    }
}

function ValidateNumber(event) {
    var theEvent = event || window.event;
    var key = theEvent.keyCode || theEvent.which;
    key = String.fromCharCode(key);
    var regex = /[0-9]|\-/;

    if (!regex.test(key)) {
        theEvent.preventDefault ? theEvent.preventDefault() : (theEvent.returnValue = false);
    }
}

function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function IsDateInBetween(startDate, endDate, date) {
    if (date > startDate && date < endDate) {
        return true;
    } else {
        return false;
    }
}

function ConvertToDateTime(date, time) {
    var hours = Number(time.match(/^(\d+)/)[1]);
    var minutes = Number(time.match(/:(\d+)/)[1]);
    var AMPM = time.match(/\s(.*)$/)[1];
    if (AMPM == "PM" && hours < 12) hours = hours + 12;
    if (AMPM == "AM" && hours == 12) hours = hours - 12;
    var sHours = hours.toString();
    var sMinutes = minutes.toString();
    if (hours < 10) sHours = "0" + sHours;
    if (minutes < 10) sMinutes = "0" + sMinutes;
    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), sHours, sMinutes, 0);

}

function setSeriesColor(name) {
    var color = "#800000";
    if (name === 'NonCompliant'.toLowerCase()) {
        color = "#FF0000";
    } else if (name === 'Compliant'.toLowerCase()) {
        color = "#5cb85c";
    } else if (name === 'Inprogress'.toLowerCase()) {
        color = "#337ab7";
    } else if (name === 'work orders'.toLowerCase()) {
        color = "#337ab7";
    }
    else if (name === 'deficiencies'.toLowerCase()) {
        color = "#5cb85c";
    }
    else if (name === 'risk assessment'.toLowerCase()) {
        color = "#f0ad4e";
    }
    else if (name === 'ilsm'.toLowerCase()) {
        color = "#d9534f";
    }
    return color;
}

function getSeries(series) {
    for (i = 0; i < series.length; i++) {
        //console.log(series[i].name.toLowerCase());
        series[i].color = setSeriesColor(series[i].name.toLowerCase());
    }
    return series;
}

function isValidEmail(email) {
    var regExp = /^(([^<>()[\]\\.,;:\s@@\"]+(\.[^<>()[\]\\.,;:\s@@\"]+)*)|(\".+\"))@@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
    return regExp.test(email);
}

String.prototype.format = String.prototype.f = function () {
    var s = this,
        i = arguments.length;

    while (i--) {
        s = s.replace(new RegExp('\\{' + i + '\\}', 'gm'), arguments[i]);
    }
    return s;
};

jQuery.extend(jQuery.fn.dataTableExt.oSort, {
    "non-empty-string-asc": function (str1, str2) {
        if (str1 == "")
            return 1;
        if (str2 == "")
            return -1;
        return ((str1 < str2) ? -1 : ((str1 > str2) ? 1 : 0));
    },

    "non-empty-string-desc": function (str1, str2) {
        if (str1 == "")
            return 1;
        if (str2 == "")
            return -1;
        return ((str1 < str2) ? 1 : ((str1 > str2) ? -1 : 0));
    }
});

/// tri status button

function inspectionStatus(control, isOpenFail) {
    debugger;
    triInsStatus(control, '-1', '1', '0', isOpenFail);
}

function triInsStatus(control, value1, value2, value3, isOpenFail) {
    debugger;
    switch (control.value) {
        case value1:
            control.value = value2;
            break;
        case value2:
            control.value = value3;
            break;
        case value3:
            control.value = value1;
            break;
    }
    SetInspectionValue(control);
}

function SetInspectionValue(control, isOpenFail) {
    var controlId = $(control).attr("tempName");
    $("input[name='" + controlId + "'][type=hidden]").val(control.value);
    if ($(control).val() == 0) {
        $("[tempname='" + controlId + "']").parent().parent().find("div.MarkDeficiency").removeClass("hide");
        $("[tempname='" + controlId + "']").parent().parent().find("div.MarkDeficiency").addClass("show");
    } else {
        $("[tempname='" + controlId + "']").parent().parent().find("div.MarkDeficiency").removeClass("show");
        $("[tempname='" + controlId + "']").parent().parent().find("div.MarkDeficiency").addClass("hide");
    }


    // inspect by floor Logic
    if (control.value == "0") {
        $('#selectAll').attr('checked', false);
        var floorAssetId = $(control).data('floorassetid');
        var epId = $(control).data('epid');
        var inspectionId = 0;
        if (control.attributes["data-orgvalue"] != undefined) {
            $(control).val(parseInt(control.attributes["data-orgvalue"].value));
        } else {
            $(control).val(-1);
        }
        
        $('#loadpartial').html("Loading...");
        var url = CRxUrls.goal_assetssteps;
        $.get(url + '?epId=' + epId + '&inspectionId=' + inspectionId + '&floorAssetId=' + floorAssetId + '&isPopUp=true', function (data) {
            $('#loadpartial').html(data);
            $('#loadpartial').fadeIn('fast');
            $('input:button[id^="InsStatus"]').each(function () {
                var btnval = $(this).val(1);
            });
        });
        $('#FailStepsPopup').modal('show');
    }
}

var bindFloorddl = function (buildingddl, floorddl, selectedfloorId, firstvalue, firsttext) {
    debugger;
    floorddl.html("<option> loading ... </option>");
    if (firstvalue == undefined) {
        firstvalue = "";
    }
    if (firsttext == undefined) {
        firsttext = $.Constants.Floor_ddl_text_Select_Floor;
    }
    var ishaveselectedfloor = false;
    floorddl.html("");
    floorddl.append(
        $('<option></option>').val(firstvalue).html(firsttext));
    var buildingID = buildingddl.val();   
    if (buildingID) {
        $.ajax({
            url: CRxUrls.Organization_FillFloors,
            global: false,
            type: "GET",
            dataType: "JSON",
            data: { buildingID: buildingID },
            success: function (floors) {
                $.each(floors, function (i, floor) {                   
                    floorddl.append(
                        $('<option></option>').val(floor.FloorId).html(floor.FloorName));                   
                    if (floor.FloorId === parseInt(selectedfloorId)) {
                        ishaveselectedfloor = true;
                    }
                });               
                if (ishaveselectedfloor) {
                    floorddl.val(selectedfloorId);
                } else {
                    floorddl.val("");
                }
                var stopCtr = $("#StopId");
                if (stopCtr !== undefined) {
                    stopCtr.trigger("change");
                }
            }
        });
    } else {
        $("#FloorId").prop('required', false);
    }
};


var bindStopddl = function (ctr, floorId, slectdValue, firstvalue, firsttext) {
     debugger;
    ctr.html("<option> loading ... </option>");
    ctr.html("");
    ctr.append(
        $('<option></option>').val(firstvalue).html(firsttext));
   // var floorId = parseInt(floorddl.val());
    if (floorId) {
        $.ajax({
            url: CRxUrls.Organization_FillStops,
            global: false,
            type: "GET",
            dataType: "JSON",
            data: { floorId: floorId },
            success: function (stops) {
                $.each(stops, function (i, stop) {
                    ctr.append(
                        $('<option></option>').val(stop.StopId).html(stop.StopName+' ['+stop.StopCode+']'));
                });
                if (slectdValue>0)
                    ctr.val(slectdValue);
                else
                    ctr.val('');
            }
        });
    }
};


var setDefaultStopDDL = function (ctr) {
    ctr.append(
        $('<option></option>').val("").html("Select Stop"));
};

BindStopNotInFE=function(stopddlCtr, assetid, floorid) {
    var seloptn = $('<option value />').text("Select Stop");
    stopddlCtr.append(seloptn);
    $.ajax({
        type: "GET",
        url: CRxUrls.Organization_GetStopNotInFE,
        data: { AssetId: assetid, FloorId: floorid },
        datatype: "json",
        contentType: "application/json",
        success: function (response) {
            if (response != null) {
                $.each(response, function (key, value) {
                    var bnoptn = $('<option />').attr('value', value.StopId).text(value.StopName + ' [' + value.StopCode + ']');
                    stopddlCtr.append(bnoptn);
                });
            }
            else {
                console.log(response);
            }
        },
        error: function (response) { console.log(response); }
    });
}

ResetAssetDropdown = function (buildingCtrl, floorCtrl, stopCtrl) {
    buildingCtrl.find('option:eq(0)').prop('selected', true);
    floorCtrl.find('option:not(:first-child)').remove();
    stopCtrl.find('option:not(:first-child)').remove();
}

removeCommaSeperatedValue = function (list, value, separator) {
    debugger;
    separator = separator || ",";
    var values = list.split(separator);
    for (var i = 0; i < values.length; i++) {
        if (values[i] == value) {
            values.splice(i, 1);
            return values.join(separator);
        }
    }
    return list;
}

function getOrgName(orgName) {
    debugger;
    return orgName.replace("&#39;", "'").replace("&#x27;", "'");
}