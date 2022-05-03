"use strict";

$(document).ready(function () {
    $("#floors").append($('<option></option>').val("").html("All"));
    $("#drpBuildings").append($('<option></option>').val(-1).html("Select"));

    loadData();

    $('input:checkbox[id^="cb"]').change(function () {
        fillBuildings();
    });

    $('.datepicker').datepicker({
        showOn: "button",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
    }).datepicker('setDate', 'today');
});

$(document).on("click", "#insSaveComment", function () {
    var text = $("#insCommenttxt").val();
    $("#inspectionComment").val(text);
    if (text.trim().length > 0) {
        $("#inspectionimg").addClass("text");
    } else $("#inspectionimg").removeClass("text");
});

$("#drpBuildings").change(function () {
    if ($("#drpBuildings").val() != "") {
        $("#floors").empty();
        $("#floors").append($('<option></option>').val("").html("All"));
        var floorOptions = {};
        floorOptions.url = CRxUrls.organization_getfloorbybuilding; //'@Url.Action("GetFloorByBuilding", "Organization")';
        floorOptions.type = "POST";
        floorOptions.data = JSON.stringify({ buildingId: $("#drpBuildings").val() });
        floorOptions.datatype = "json";
        floorOptions.contentType = "application/json";
        floorOptions.success = function (floorList) {
            for (var i = 0; i < floorList.length; i++) {
                $("#floors").append($('<option></option>').val(floorList[i].FloorId).html(floorList[i].FloorName));
            }
            $("#floors").prop("disabled", false);
        };
        floorOptions.error = function () {
            alert("Error in Getting buildings!!");
        };
        $.ajax(floorOptions);
    } else {
        $("#floors").empty();
        $("#floors").append($('<option></option>').val("").html("All"));
    }
    loadData();
});

$("#floors").change(function () {
    loadData();
});

function filterAssetbyEP() {
    var values = $("input[name=epdetail]:radio:checked").val();
    table.columns(1).search(values).draw();
}

function fillBuildings() {
    var Ids = "";
    var assetsSubCategoryId = "";
    $("#drpBuildings").empty();
    $("#drpBuildings").append($('<option></option>').val(-1).html("Select"));
    $("#drpBuildings").append($('<option></option>').val("").html("All"));
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
    var url = CRxUrls.assets_getfilterbuildings; //'@Url.Action("GetFilterBuildings", "Assets")';
    $.get(url + '?assetId=' + Ids + '&ascIds=' + assetsSubCategoryId, function (buildingList) {
        debugger;
        for (var i = 0; i < buildingList.length; i++) {
            if (buildingList[i].BuildingName != "") {
                $("#drpBuildings").append($('<option></option>').val(buildingList[i].BuildingId).html(buildingList[i].BuildingName));
            }
        }
        loadData();
    });
}

function loadData() {
    var buildingId = "";
    buildingId = $("#drpBuildings").val();
    var floorId = "";
    floorId = $("#floors").val();
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
    var url = CRxUrls.assets_getfloorassetsforins; //'@Url.Action("GetFloorAssetsForIns", "Assets")';
    $.get(url + '?buildingId=' + buildingId + '&floorId=' + floorId + '&assetId=' + Ids + '&ascIds=' + assetsSubCategoryId, function (data) {
        $('#assets_view').html(data);
        $('#assets_view').fadeIn('fast');
        filterAssetbyEP();
    });
}

$("#selectAll").click(function () {
    var $this = $(this);
    if ($this.is(":checked")) {
        $('input:button[id^="chkbtn"]').not('input:button[id^="chkbtn_disabled"]').each(function () {
            $(this).val(1);
        });
    } else {
        $('input:button[id^="chkbtn"]').each(function () {
            var control = $(this).data("orgvalue");
            $(this).val(control);
        });
    }
});

function Uploadfile(file) {
    var fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    if (file.files.length > 0) {
        file = file.files[0];
        var fileName = file.name;
        if (fileName != null) {
            var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
            if ($.inArray(fileExtension, fileExtensionas) == -1) {
                infoAlert("Only formats are allowed : " + fileExtensionas.join(', '));
            } else {
                var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf('.'));
                $("#fileName").val(fileName);
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function (e) {
                    var array = reader.result.split(",");
                    $("#fileContent").val(array[1]);
                    $("#imagePreview_0").attr('src', ImgUrls.document_blue_icon);
                    $("#imagePreview_0").removeClass("img_prev");
                    $("#imagePreview_0").addClass("img_prev_upload");
                };
                reader.onerror = function (error) {
                    $("#FileContent").val("");
                };
            }
        }
    }
}

