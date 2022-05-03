
var singleDs = [];

(function ($) {

    function getSingleton(id) {
        var result;
        $(singleDs).each(function () {
            if ($(this)[0].id == id) {
                result = $(this)[0];
            }
        });
        return result;
    }

    function Dirrty(form, options) {
        this.form = form;
        this.isDirty = false;
        this.options = options;
        this.history = ["clean", "clean"]; //Keep track of last statuses
        this.id = $(form).attr("id");
        singleDs.push(this);
    }

    Dirrty.prototype = {

        init: function () {
            this.saveInitialValues();
            this.setEvents();
        },

        saveInitialValues: function () {
            this.form.find("input, select, textarea").each(function () {
                $(this).attr("data-dirrty-initial-value", $(this).val());
            });

            this.form.find("input[type=checkbox], input[type=radio]").each(function () {
                if ($(this).is(":checked")) {
                    $(this).attr("data-dirrty-initial-value", "checked");
                } else {
                    $(this).attr("data-dirrty-initial-value", "unchecked");
                }
            });
        },

        setEvents: function () {
            var d = this;

            $(document).ready(function () {

                d.form.on('submit', function () {
                    d.submitting = true;
                });

                if (d.options.preventLeaving) {
                    $(window).on('beforeunload', function () {
                        if (d.isDirty && !d.submitting) {
                            return d.options.leavingMessage;
                        }
                    });
                }

                d.form.find("input, select").change(function () {
                    d.checkValues();
                });

                d.form.find("input, textarea").on('keyup keydown blur', function () {
                    d.checkValues();
                });

                //fronteend's icheck support
                d.form.find("input[type=radio], input[type=checkbox]").on('ifChecked', function (event) {
                    d.checkValues();
                });

            });
        },

        checkValues: function () {
            var d = this;
            this.form.find("input, select, textarea").each(function () {
                var initialValue = $(this).attr("data-dirrty-initial-value");
                if ($(this).val() != initialValue) {
                    $(this).attr("data-is-dirrty", "true");
                } else {
                    $(this).attr("data-is-dirrty", "false");
                }
            });
            this.form.find("input[type=checkbox], input[type=radio]").each(function () {
                var initialValue = $(this).attr("data-dirrty-initial-value");
                if ($(this).is(":checked") && initialValue != "checked"
 					|| !$(this).is(":checked") && initialValue == "checked") {
                    $(this).attr("data-is-dirrty", "true");
                } else {
                    $(this).attr("data-is-dirrty", "false");
                }
            });
            var isDirty = false;
            this.form.find("input, select, textarea").each(function () {
                if ($(this).attr("data-is-dirrty") == "true") {
                    isDirty = true;
                }
            });
            if (isDirty) {
                d.setDirty();
            } else {
                d.setClean();
            }

            d.fireEvents();
        },

        fireEvents: function () {

            if (this.isDirty && this.wasJustClean()) {
                this.form.trigger("dirty");
            }

            if (!this.isDirty && this.wasJustDirty()) {
                this.form.trigger("clean");
            }
        },

        setDirty: function () {
            this.isDirty = true;
            this.history[0] = this.history[1];
            this.history[1] = "dirty";
        },

        setClean: function () {
            this.isDirty = false;
            this.history[0] = this.history[1];
            this.history[1] = "clean";
        },

        //Lets me know if the previous status of the form was dirty
        wasJustDirty: function () {
            return (this.history[0] == "dirty");
        },

        //Lets me know if the previous status of the form was clean
        wasJustClean: function () {
            return (this.history[0] == "clean");
        }
    }

    $.fn.dirrty = function (options) {

        if (/^(isDirty)$/i.test(options)) {
            //Check if we have an instance of dirrty for this form
            var d = getSingleton($(this).attr("id"));

            if (!d) {
                var d = new Dirrty($(this), options);
                d.init();
            }
            switch (options) {
                case 'isDirty':
                    return d.isDirty;
                    break;
            }

        } else if (typeof options == 'object' || !options) {

            return this.each(function () {
                options = $.extend({}, $.fn.dirrty.defaults, options);
                var dirrty = new Dirrty($(this), options);
                dirrty.init();
            });

        }

    }

    $.fn.dirrty.defaults = {
        preventLeaving: true,
        leavingMessage: "You have unsaved changes",
        onDirty: function () { },  //This function is fired when the form gets dirty
        onClean: function () { }   //This funciton is fired when the form gets clean again
    };

})(jQuery);
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
(function ($) {
    $.Constants =
    {
        Policy: 106,
        Report: 107,
        MiscDocuments: 108,
        SampleDocument: -1,
        AssetReport: 109,
        CHILD_PAGE_ADMIN_CONFIRMATION: "These options have system-wide implications. Please contact your CRx Advisor before making changes.",
        Check_All_Status: "Please change status of all checkpoints.",
        Check_All_Status_Generator: 'All items do not have values. Are you sure you have completed  everything that is applicable.',
        Create_Work_Order: "Do you want to create a work order ?",
        Alert_Msg_After_InsGrop_Completed: "This EP is not complete until EP checkpoints are reviewed and completed in next step.",
        Select_Atleast_One_Asset_To_Create_WO: "Select at least one asset to create a work order",
        PDF_Footer_Msg_Fire_Watch_Portal_module: "CRx by HCFCompliance: Fire Watch Portal Module",
        Delete_Manufacture_Msg: "Are you sure you wish to delete this manufacture?",
        This_Ep_Is_Already_Selected: 'This EP is already selected!!',
        Select_standard_Ep: "Please select standard and EP to bind.",
        Email_Sent_Successfully: "Email sent successfully.",
        EMAIL_FAILED_MSG: "An error occurred while sending mail.",
        Remove_Assets_From_FloorPlan_Warning: "Are you want to remove this asset from floor!",
        Remove_Assets_From_FloorPlan_Success: "Your assets has been removed",
        RequestVerificationToken: "__RequestVerificationToken",
        Floor_ddl_text_All: "All",
        Floor_ddl_text_Select_Floor: "Select Floor",
        First_Close_Pending_ILSM: "Please close pending ILSM first before reinspecting",
        First_Close_Pending_Workorder: "Please close pending workorder first before reinspecting",
        Pin_Generated_Successfully: "Pin generated successfully",
        Browse_View_Files_In_IFrame: "https://view.officeapps.live.com/op/view.aspx?src=",  /* "https://docs.google.com/gview?url="*/ //embed
        ILSM_Information_Message: "Based on deficiencies, an ILSM incident #{0} was created.Please Document all Interim Life safety measures that are taken!",
        ILSM_Information_Existing_Message: "Based on deficiencies, this asset was added to existing ILSM incident #{0}.Please review to ensure all Interim Life safety measures that are taken and documented!",
        Delete_Digital_Signature: "Are you sure you wish to delete this signature?",
        Select_Atleast_One_Siemens_To_Create_WO: "Select at least one deficiency to create a work order",
        EP_Inspection_Ongoing_User: "<b>To Review this EP, the frequency must be changed from “Ongoing” by your CRx system administrator in <br /> Setup>More configurations>EP Configuration>EP Frequency </b>",
        EP_Inspection_Ongoing_Assets: "To Review this Asset, the frequency must be changed from “Ongoing” by your CRx system administrator in <br /> Setup>More configurations>EP Configuration>EP Frequency ",
        EP_Inspection_Attach_Assets: "Attach Assets First",
        All_Assets_Inspected: "Do you want to mark ep compliant ?",
        User_Deleted_Successfully:"User Deleted Successfully"

    },
        $.FloorAlias = {
            AliasItem: [
                { Sequence: "25", AliasValue: "PE", AliasText: "Penthouse" },
                { Sequence: "24", AliasValue: "R", AliasText: "Roof" },
                { Sequence: "10", AliasValue: "10", AliasText: "10th Floor" },
                { Sequence: "9", AliasValue: "9", AliasText: "9th Floor" },
                { Sequence: "8", AliasValue: "8", AliasText: "8th Floor" },
                { Sequence: "7", AliasValue: "7", AliasText: "7th Floor" },
                { Sequence: "6", AliasValue: "6", AliasText: "6th Floor" },
                { Sequence: "5", AliasValue: "5", AliasText: "5th Floor" },
                { Sequence: "4", AliasValue: "4", AliasText: "4th Floor" },
                { Sequence: "3", AliasValue: "3", AliasText: "3rd Floor" },
                { Sequence: "2", AliasValue: "2", AliasText: "2nd Floor" },
                { Sequence: "1", AliasValue: "1", AliasText: "1st Floor" },
                { Sequence: "1", AliasValue: "GF", AliasText: "Ground Floor" },
                { Sequence: "0", AliasValue: "00", AliasText: "Level 00" },
                { Sequence: "-1", AliasValue: "LL", AliasText: "Lower Lobby" },
                { Sequence: "-2", AliasValue: "LL0", AliasText: "Lower Level" },
                { Sequence: "-2", AliasValue: "LL 1", AliasText: "Lower Level 1" },
                { Sequence: "-2", AliasValue: "LL 2", AliasText: "Lower Level 2" },
                { Sequence: "-2", AliasValue: "LL 3", AliasText: "Lower Level 3" },
                { Sequence: "-3", AliasValue: "B1", AliasText: "Basement" },
                { Sequence: "-5", AliasValue: "SB", AliasText: "Sub-Basement" }, /*B2*/
                { Sequence: "-7", AliasValue: "B3", AliasText: "Sub-Sub-Basement" }
            ]
        },
        $.CRx_DateFormat = "M d, yy";
})(jQuery);
/*
 * jQuery Shorten plugin 1.1.0
 *
 * Copyright (c) 2014 Viral Patel
 * http://viralpatel.net
 *
 * Licensed under the MIT license:
 *   http://www.opensource.org/licenses/mit-license.php
 */

/*
** updated by Jeff Richardson
** Updated to use strict,
** IE 7 has a "bug" It is returning undefined when trying to reference string characters in this format
** content[i]. IE 7 allows content.charAt(i) This works fine in all modern browsers.
** I've also added brackets where they weren't added just for readability (mostly for me).
*/

(function($) {
    $.fn.shorten = function(settings) {
        "use strict";
        
        var config = {
            showChars: 100,
            minHideChars: 10,
            ellipsesText: "...",
            moreText: "more",
            lessText: "less",
            onLess: function() {},
            onMore: function() {},
            errMsg: null,
            force: false
        };

        if (settings) {
            $.extend(config, settings);
        }

        if ($(this).data('jquery.shorten') && !config.force) {
            return false;
        }
        $(this).data('jquery.shorten', true);

        $(document).off("click", '.morelink');

        $(document).on({
            click: function() {

                var $this = $(this);
                if ($this.hasClass('less')) {
                    $this.removeClass('less');
                    $this.html(config.moreText);
                    $this.parent().prev().animate({ 'position': 'relative'}, function () { $this.parent().prev().prev().show(); }).hide('fast', function() {
                        config.onLess();
                      });

                } else {
                    $this.addClass('less');
                    $this.html(config.lessText);
                    $this.parent().prev().animate({ 'position':'relative'}, function () { $this.parent().prev().prev().hide(); }).show('fast', function() {
                        config.onMore();
                    }).css({ 'display': 'inline' });
                }
                return false;
            }
        }, '.morelink');

        return this.each(function() {
            var $this = $(this);

            var content = $this.html();
            var contentlen = $this.text().length;
            if (contentlen > config.showChars + config.minHideChars) {
                var c = content.substr(0, config.showChars);
                if (c.indexOf('<') >= 0) // If there's HTML don't want to cut it
                {
                    var inTag = false; // I'm in a tag?
                    var bag = ''; // Put the characters to be shown here
                    var countChars = 0; // Current bag size
                    var openTags = []; // Stack for opened tags, so I can close them later
                    var tagName = null;

                    for (var i = 0, r = 0; r <= config.showChars; i++) {
                        if (content[i] == '<' && !inTag) {
                            inTag = true;

                            // This could be "tag" or "/tag"
                            tagName = content.substring(i + 1, content.indexOf('>', i));

                            // If its a closing tag
                            if (tagName[0] == '/') {


                                if (tagName != '/' + openTags[0]) {
                                    config.errMsg = 'ERROR en HTML: the top of the stack should be the tag that closes';
                                } else {
                                    openTags.shift(); // Pops the last tag from the open tag stack (the tag is closed in the retult HTML!)
                                }

                            } else {
                                // There are some nasty tags that don't have a close tag like <br/>
                                if (tagName.toLowerCase() != 'br') {
                                    openTags.unshift(tagName); // Add to start the name of the tag that opens
                                }
                            }
                        }
                        if (inTag && content[i] == '>') {
                            inTag = false;
                        }

                        if (inTag) { bag += content.charAt(i); } // Add tag name chars to the result
                        else {
                            r++;
                            if (countChars <= config.showChars) {
                                bag += content.charAt(i); // Fix to ie 7 not allowing you to reference string characters using the []
                                countChars++;
                            } else // Now I have the characters needed
                            {
                                if (openTags.length > 0) // I have unclosed tags
                                {
                                    //console.log('They were open tags');
                                    //console.log(openTags);
                                    for (var j = 0; j < openTags.length; j++) {
                                        //console.log('Cierro tag ' + openTags[j]);
                                        bag += '</' + openTags[j] + '>'; // Close all tags that were opened

                                        // You could shift the tag from the stack to check if you end with an empty stack, that means you have closed all open tags
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    c = $('<div/>').html(bag + '<span class="ellip">' + config.ellipsesText + '</span>').html();
                }else{
                    c+=config.ellipsesText;
                }
                
                var html = '<span class="shortcontent">' + c +
                    '</span><span class="allcontent">' + content +
                    '</span><span><a href="javascript://nop/" class="morelink">' + config.moreText + '</a></span>';

                $this.html(html);
                $this.find(".allcontent").hide(); // Hide all text
                $('.shortcontent p:last', $this).css('margin-bottom', 0); //Remove bottom margin on last paragraph as it's likely shortened
            }
        });

    };

})(jQuery);
