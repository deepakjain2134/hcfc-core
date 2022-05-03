var drpBuildings = $("#drpBuildings");
var ddlroutefltr = $("#ddlroutefltr");
var IsdocRequired = "";
$(document).ready(function () {
   
 
    drpBuildings.append(
        $('<option></option>').val(-1).html("Select"));
    ddlroutefltr.append(
        $('<option></option>').val(-1).html("Select"));

   
    $('input:checkbox[id^="cb"]').change(function () {
        debugger;
        $("#assets_view").empty();
        fillBuildings();
        fillRoutes();
    });

    $('.datepicker').datepicker({
        showOn: "both",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        maxDate: 0,
        dateFormat: $.CRx_DateFormat,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
    }).datepicker("setDate", new Date());

    $("#dateBox").hide();
    if ($("#file-upload-list").find("#file-lists").find("ul").length > 0 ||
        typeof $("#file-upload-list").find("#file-lists").find("ul") !== 'undefined') {
        $("#file-upload-list").find("#file-lists").find("ul").empty();
    }
    
    
});

$(document).on("click", "#insSaveComment", function () {
    var text = $("#insCommenttxt").val();
    $("#inspectionComment").val(text);
    if (text.trim().length > 0) {
        $("#inspectionimg").addClass("text");
        $("#lblInspectionComment").html("<b>Comment:</b> " + text);
    } else {
        $("#inspectionimg").removeClass("text");
        $("#lblInspectionComment").html('');
    }
});

drpBuildings.change(function () {
    var assetsCount = $('input:checkbox[id^="cb2"]:checked').length;
    debugger;
    if (assetsCount > 0) {
        $("#floors").empty();
        $("#floors").append(
            $('<option></option>').val("0").html("All"));
        if (drpBuildings.val() != "-1") {

            //if (getParameterByName('Isupload') != undefined || getParameterByName('Isupload') > 0) {
            //    fillBuildings();
            //}
            loadData();
        }
        else {
            $('#assets_view').html('');
            $('#assets_view').fadeIn('fast');
            $("#dateBox").hide();
        }
    } else {
        drpBuildings.val("");
        warningAlert("Please select an asset type from the list on the left");
    }
});
$('body').on('change', '#floors', function () {
    loadData();
});

$('body').on('change', '#ddlroutefltr', function () {
    loadData();
});

function fillBuildings() {
    var Ids = "";
    var assetsSubCategoryId = "";
    //if (getParameterByName('Isupload') != 1) {
    //    $("#drpBuildings").empty();
    //    $("#drpBuildings").append(
    //        $('<option></option>').val(-1).html("Select"));
    //    $("#drpBuildings").append(
    //        $('<option></option>').val("0").html("All"));
    //} else {
    //    $("#drpBuildings").append(
    //        $('<option></option>').val(-1).html("Select"));
   
    //}

    //$("#drpBuildings").append(
    //    $('<option></option>').val("-2").html("Assets WithOut Floor"));
    $("#drpBuildings").empty();
         $("#drpBuildings").append(
        $('<option></option>').val("0").html("All"));
    $("#drpBuildings").append(
        $('<option></option>').val("-2").html("Assets WithOut Floor"));
    $('input:checkbox[id^="cb2"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                Ids = Ids + $(this).val() + ",";
            }
        }
    });
    $('input:checkbox[id^="cb3"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                assetsSubCategoryId = assetsSubCategoryId + $(this).val() + ",";
            }
        }
    });
    var url = CRxUrls.assets_getfilterbuildings;
    $.get(url + '?assetId=' + Ids + '&ascIds=' + assetsSubCategoryId, function (buildingList) {
       
        for (var i = 0; i < buildingList.length; i++) {
            if (buildingList[i].BuildingName != "") {
                $("#drpBuildings").append(
                    $('<option></option>').val(buildingList[i].BuildingId).html(buildingList[i].BuildingName));
            }
        } if (getParameterByName('Isupload') != undefined || getParameterByName('Isupload') > 0) {
            drpBuildings.val(localStorage.getItem('drpBuildings'));
        }
        if ($("#drpBuildings").val() != "-1") {
            loadData();
        }
    });
}

function fillRoutes() {
    var Ids = "";
    $('input:checkbox[id^="cb2"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                Ids = Ids + $(this).val(); //+ ",";
            }
        }
    });
    ddlroutefltr.empty();
    ddlroutefltr.append(
        $('<option></option>').val(-1).html("Select"));
    var url = CRxUrls.assets_getRoutesbyAssetId;
    $.get(url + '?assetId=' + Ids, function (routelist) {
        debugger;
        if (routelist.result != "") {
            if (routelist.result.length > 0) {
                ddlroutefltr.append(
                    $('<option></option>').val(0).html("All"));
                for (var i = 0; i < routelist.result.length; i++) {
                    if (routelist.result[i].RouteId != "") {
                        ddlroutefltr.append($('<option></option>').val(routelist.result[i].RouteId).html(routelist.result[i].RouteNo + " (" + routelist.result[i].AssetCounts + ")"));
                    }
                }
            }
        }
    }); 
}

function loadData() {
    debugger; 
    var buildingId = drpBuildings.val();
    var floorId = $("#floors").val();
    var routeId = ddlroutefltr.val();
    var Ids = "";
    var assetsSubCategoryId = "";
    $('input:checkbox[id^="cb2"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                Ids = Ids + $(this).val() + ",";
            }
        }
    });
    $('input:checkbox[id^="cb3"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                assetsSubCategoryId = assetsSubCategoryId + $(this).val() + ",";
            }
        }
    });
    console.log(floorId);
    // debugger;
    $(".loadingModal").show();
    var url = CRxUrls.assets_getfloorassetsforins;
    var floorId = buildingId >= 0 ? floorId : -1;
    if (Ids.length > 0 && (buildingId >= 0 || buildingId == -2 || floorId >= 0 || routeId >= 0)) {
        $.get(url + '?buildingId=' + buildingId + '&floorId=' + floorId + '&routeId=' + routeId + '&assetId=' + Ids + '&ascIds=' + assetsSubCategoryId, function (data) {
            $('#assets_view').html(data);
            $('#assets_view').fadeIn('fast');
            $(".loadingModal").hide();
            if (buildingId > 0 && floorId === "0") {
                debugger;
                loadFloorList(buildingId);
            }
           
            IsdocRequired = $("#exTab1 li.active").find("input[name='epdetail']").attr('docrequired'); 
            $("#exTab1 li.active").find("input[name='epdetail']").attr('IsPreviousCycle', localStorage.getItem("IsPreviousCycle")); 
            $("#exTab1 li.active").find("input[name='epdetail']").attr('PreviousCycleInspectionId', localStorage.getItem("inspectionid")); 
            $("#exTab1 li.active").find("input[name='epdetail']").attr('TCycleId', localStorage.getItem("TCycleId")); 
            //if (IsdocRequired == "1") {
            //    $("#inspectiondate").hide();
            //}
            //else {
            //    $("#inspectiondate").show();
            //}
            Addreadmore();
        });
    } else {
        $('#assets_view').html('');
        $('#assets_view').fadeIn('fast');
        $(".loadingModal").hide();
        $("#drpBuildings").empty();
        $("#drpBuildings").append($('<option></option>').val("0").html("All"));
        $("#drpBuildings").append($('<option></option>').val("-2").html("Assets WithOut Floor"));
    }
}

var loadFloorList = function (buildigId) {
    debugger;
    $.get(CRxUrls.common_floorddll + '?buildingId=' + buildigId + '&selectedValue=0&firstValue=0&firstText=Select', function (data) {
        $('#floorLoad').empty();
        $('#floorLoad').html(data);
    });
};

//$("#selectAll").click(function () {
//    var $this = $(this);
//    if ($this.is(":checked")) {
//        $('input:button[id^="chkbtn"]').not('input:button[id^="chkbtn_disabled"]').each(function () {
//            $(this).val(1);
//        });
//    } else {
//        $('input:button[id^="chkbtn"]').each(function () {
//            var control = $(this).data("orgvalue");
//            $(this).val(control);
//        });
//    }
//});

function IlsmUploadfile(file) {
    var fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    if (file.files.length > 0) {
        file = file.files[0];
        var fileName = file.name;
        if (fileName != null) {
            var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
            if ($.inArray(fileExtension, fileExtensionas) == -1) {
                infoAlert("Only formats are allowed : " + fileExtensionas.join(', '));
            } else {
                //var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf('.'));
                $("#fileName").val(fileName);
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function (e) {
                    var array = reader.result.split(",");
                    $("#fileContent").val(array[1]);
                    $("#imagePreview_0").attr('src', ImgUrls.document_blue_icon);
                    $("#imagePreview_0").removeClass("img_prev");
                    $("#imagePreview_0").addClass("img_prev_upload");
                    $("#lblfilemsg").show();
                };
                reader.onerror = function (error) {
                    $("#FileContent").val("");
                };
            }
        }
    }
}



function loadFloorAssetrowData(epdetailid,floorassetid) {
    debugger;
    var buildingId = drpBuildings.val();
    var floorId = $("#floors").val();
    var routeId = ddlroutefltr.val();
    var Ids = "";
    var assetsSubCategoryId = "";
    $('input:checkbox[id^="cb2"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                Ids = Ids + $(this).val() + ",";
            }
        }
    });
    $('input:checkbox[id^="cb3"]').each(function () {
        var $this = $(this);
        if ($this.is(":checked")) {
            if ($(this).val() != "on") {
                assetsSubCategoryId = assetsSubCategoryId + $(this).val() + ",";
            }
        }
    });
    console.log(floorId);
    // debugger;
    $(".loadingModal").show();
    var url = CRxUrls.assets_getfloorassetstepsforins;
    var floorId = buildingId >= 0 ? floorId : -1;
    if (Ids.length > 0 && (buildingId >= 0 || floorId >= 0 || routeId >= 0)) {
        $.get(url + '?buildingId=' + buildingId + '&floorId=' + floorId + '&routeId=' + routeId + '&assetId=' + Ids + '&ascIds=' + assetsSubCategoryId + '&epdetailid=' + epdetailid + '&floorassetid=' + floorassetid, function (data) {
            $("#" + epdetailid + "-" + floorassetid).empty();
            $("#" + epdetailid + "-" + floorassetid).html(data);
            $("#" + epdetailid + "-" + floorassetid).fadeIn('fast');
            //if (buildingId > 0 && floorId === "0") {
            //    debugger;
            //    loadFloorList(buildingId);
            //}
            $(".loadingModal").hide();
            IsdocRequired = $("#exTab1 li.active").find("input[name='epdetail']").attr('docrequired');
            //if (IsdocRequired == "1") {
            //    $("#inspectiondate").hide();
            //}
            //else {
            //    $("#inspectiondate").show();
            //}
        });
    } else {
        $("#myTable tbody").find('tr[epdetailid="' + epdetailid + '"] ,[floor-assetid="' + floorassetid + '"]').html('')
        $("#myTable tbody").find('tr[epdetailid="' + epdetailid + '"] ,[floor-assetid="' + floorassetid + '"]').fadeIn('fast');
        $(".loadingModal").hide();
    }
}




function Addreadmore() {
    var carLmt = 240;
    var readMoreTxt = " ... Read More";
    var readLessTxt = " Read Less";
    $(".addReadMore").each(function () {
        if ($(this).find(".firstSec").length)
            return;

        var allstr = $(this).text();
        if (allstr.length > carLmt) {
            var firstSet = allstr.substring(0, carLmt);
            var secdHalf = allstr.substring(carLmt, allstr.length);
            var strtoadd = firstSet + "<span class='SecSec'>" + secdHalf + "</span><span class='readMore'  title='Click to Show More'>" + readMoreTxt + "</span><span class='readLess' title='Click to Show Less'>" + readLessTxt + "</span>";
            $(this).html(strtoadd);
        }

    });
    $(document).on("click", ".readMore,.readLess", function () {
        $(this).closest(".addReadMore").toggleClass("showlesscontent showmorecontent");
    });
}
$(function () {
    Addreadmore();
});