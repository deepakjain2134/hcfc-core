var part1 = $(".part1");
var part2 = $(".part2");
var epDocSteps = $('#epDocSteps');
var epdocs = $("#epdocs");
var doc_history = $('#doc_history');
var miscuploadmsg = "Misc. Doc saved Successfully.";
var uploaddocTypeId = $("#uploaddocTypeId").val();
var epDocTypeId = $("#item_DoctypeId").val();
var epDocFilePreview = $('#epDocFilePreview');
var epIdCtr = $("#epId");
var epIds = $("#epIds").val();
var inspectionidCtr = $("#inspectionid");
var docname = '';
var doctypeid = '';
var isEOP = 0
var ishistory = 0;
var epFiles = [];
var epDocDetails = {};

//part2.hide();
part1.hide();

var pageMode = "epdocs";

var dropFiles = [];
var dropEps = [];
var inspectionid = 0;
$(document).ready(function () {
    //console.log(canInspectEp, dueDate);
    //if (epDocTypeId === "0") {
    //    $(".samplerbl").hide();
    //    var dt = $('#inboxTable').DataTable();
    //    dt.columns([-1]).visible(false);
    //}
    if (getParameterByName('ishistory') != undefined || getParameterByName('ishistory') > 0) {
        pageMode="EpHisotry"
    }
    
    if (getParameterByName('inspectionid') != undefined || getParameterByName('inspectionid') > 0) { inspectionid = getParameterByName('inspectionid'); }
    $('#IsPreviousCycle').val(localStorage.getItem("IsPreviousCycle"));

    $('#PreviousCycleInspectionId').val(inspectionid);
    //var InspectionID = getParameterByName('InspectionId');
    //if (InspectionID > 0 && InspectionID != undefined) {
    //        pageMode = 'EPInspection';
    //    }
    
    var IsPreviousCycle = getParameterByName('IsPreviousCycle');
    if (IsPreviousCycle == 1 && IsPreviousCycle != undefined) {
       /* pageMode = 'EPInspection';*/
    }
    
   
    debugger;
    var totalEpsToInspect = 1;
    var epId = epIdCtr.val();
    epDocDetails = JSON.parse(localStorage.getItem("epDocEpsLists"));
    if (epDocDetails != undefined) {
        pageMode = epDocDetails.backpage;
        totalEpsToInspect = epDocDetails.epDetailId.length;
    }
    //debugger;
    console.log(totalEpsToInspect);
    if (totalEpsToInspect > 1) {
        $(".goToNextEp").removeClass("hide");
    }

    if (canInspectEp.toLowerCase() === "false") {
        confirmOngoingEps(epId);
    } else if (epStatus =="C") {
        confirmEpDocUpload(epId, totalEpsToInspect);
    }
    console.log(epDocDetails);
    loadCheckPoints(epId, inspectionid);

    var values = [];
    $(".ins_check2_btn").each(function () {
        //$(".ins_check2_btn").each(function () {
        values.push($(this).val());
        if (values.indexOf("-1") === -1) {
            $("#btnSubmit").removeClass('disable');
          
        }

    });
    
});

function confirmOngoingEps(epId) {
    var confirmRedirectUrl = CRxUrls.Goal_EpInspections + "?epId=" + epId;
    var html = $.Constants.EP_Inspection_Ongoing_User;
    ShowConfirmMsg("Add Document ", "info", html, "Back To EP Review", confirmRedirectUrl);
}


var confirmEpDocUpload = function (epId, totalEpsToInspect) {
    //var dueDate =
    //    '@((Model.Inspection != null && Model.Inspection.DueDate.HasValue) ? Model.Inspection.DueDate.Value.ToClientTime().ToFormatDate() : "") ';

    var cancelButtonText = (totalEpsToInspect > 1) ? "Go To Next EP Review" : "Cancel";
    debugger
    
    if (dueDate) {
        swal({
            html: true,
            title: "Upload Document",
            text: '<b>EP is in Compliant Status</b>.<br/> Next Review is due on ' +
                dueDate +
                '. <br/><br/> If you upload a Required Document, EP status will change to In Progress and EP review will be required.',
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Continue",
            cancelButtonText: cancelButtonText,
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                } else {
                    if (totalEpsToInspect > 1) {
                        epDocDetails.epDetailId = epDocDetails.epDetailId.filter(function (value, index, arr) {
                            return value != epId;
                        });
                        localStorage.setItem('epDocEpsLists', JSON.stringify(epDocDetails));
                        window.location.href = CRxUrls.Goal_EPDocs + "?epId=" + epDocDetails.epDetailId[0];
                    } else {
                        goToPreviousPage();
                    }
                }
            });
    }
}

function tristate1(control, ActivityType=3) {
    if (ActivityType == 3) {
        tristate(control, '-1', '1');
    }
    else {
        tristate2(control, '-1', '1', '0');
    }

    
}

function tristate(control, value1, value2) {
    switch (control.value) {
        case value1:
            control.value = value2;
            break;
        case value2:
            control.value = value1;
            break;
    }
    var values = [];
    $(".ins_check2_btn").each(function () {
        //$(".ins_check2_btn").each(function () {
        values.push($(this).val());
        if (values.indexOf("-1") === -1) {
            $("#btnSubmit").removeClass('disable');

        }
        else {
            $("#btnSubmit").addClass('disable');

        }
    });

    
    SetCheckValue(control);
}
function tristate2(control, value1, value2, value3) {
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

        SetCheckValue(control);
    }
        

function SetCheckValue(control) {
    var controlId = $(control).attr("tempName");
    $("input[name='" + controlId + "'][type=hidden]").val(control.value);
}

//function dragOver(e) {
//    e.preventDefault();
//    e.stopPropagation();
//    var id = e.target.parentNode.id;
//    $("#" + id).addClass("trOverlay");
//}

//function epDocsdragLeave(e) {
//    $("div").removeClass("trOverlay");
//}

//function dragleave(e) {
//    $("tr").removeClass("trOverlay");
//    $("div").removeClass("trOverlay");
//}

//function drop(e) {
//    debugger;
//    e.stopPropagation();
//    e.preventDefault();

//    var epId = e.target.closest("tr").id;
//    var files = e.target.files || e.dataTransfer.files;
//    //for (var i = 0, f; f = files[i]; i++) {
//    //    console.log(f);
//    //}

//    if (files.length > 0) {
//        uploadFileToServer(files[0], epId);

//    } else {
//        $("tr").removeClass("trOverlay");
//        $("div").removeClass("trOverlay");
//        $(".inbox_file").removeClass("trOverlay");


//        if (epId != null || epId != undefined) {
//            var sourceData = JSON.parse(e.dataTransfer.getData("text"));
//            console.log(sourceData);
//            var fileId = sourceData.fileId;
//            $("#fileId").val(fileId);
//            var filePath = sourceData.path;
//            var fileName = sourceData.fileName;
//            var documentId = sourceData.documentId;
//            var fileurl = sourceData.fileurl;
//            $("#filePaths").val(fileurl);
//            //console.log(documentId);
//            if (documentId === "0") {
//                swalalert("please select file from policy , Report / Misc Doc or Sample doc . Before using Attachments, first select and drag the Attachment to: Report, Misc.Doc, Policy or Sample Doc column");
//                return false;
//            }
//            policyObject.docTypeId = sourceData.doctypeId;
//            var docTypeId = sourceData.doctypeId;
//            policyObject.epDetailId.push(epId);
//            $("#divdocspoup").addClass("disable");
//            $(".choose_file").hide();
//            $("span.ui-dialog-title").text('Confirm?');
//            $("#lblchoosfilemsg").hide();
//            $("input[name=docType][value=" + docTypeId + "]").prop('checked', true);
//            // add option for update fileName
//            $("#newFileName").val(getFileName(fileName));
//            $("#newFileNameExe").html(getFileExtenstion(fileName));
//            $("#newFileNameExe").val(getFileExtenstion(fileName));
//            $("#newFileNameExe").addClass(getFileExtenstion(fileName));
//            $("#newFileNameExe").removeClass();

//            showPopUpAfterUpload(documentId, epId, filePath);
//        }
//    }
//}

var showPopUpAfterUpload = function (documentId, epId, filePath) {


   //debugger;
    $("#dialog-confirm").dialog({
        closeOnEscape: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        },
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        buttons: {
            Yes: function () {
                //debugger;
                if ($("#newFileName").val().length > 0) {
                    var newFileName = $("#newFileName").val() + "." + $("#newFileNameExe").val();
                    var uploadDocType = $("input[name='docType']:checked").val();
                    policyObject.docTypeId = uploadDocType;
                    viewAfterFileUpload(documentId, newFileName, epId, uploadDocType, filePath);
                    $(this).dialog("close");
                } else {
                    swalalert('select file');
                }
            },
            No: function () {
                $(this).dialog("close");
            }
        }
    });
   // debugger;
    $("#newFileNameExe").removeClass();
    if ($("#newFileNameExe").val().indexOf("pdf") != -1) {

        $("#newFileNameExe").addClass("newfile-pdf");
    }
    if ($("#newFileNameExe").val().indexOf("xls") != -1 || $("#newFileNameExe").val().indexOf("csv") != -1) {
        $("#newFileNameExe").addClass("newfile-xls");
    }
    if ($("#newFileNameExe").val().indexOf("doc") != -1) {
        $("#newFileNameExe").addClass("newfile-doc");
    }
}

var viewAfterFileUpload = function (fileId, fileName, epId, docTypeId, filePath) {
    //debugger;
    uploaddocTypeId.val(docTypeId);
    if (parseInt(docTypeId) === $.Constants.MiscDocuments) {
        AddMiscEPDocs(fileId, fileName, epId, docTypeId);
    } else if (parseInt(docTypeId) === $.Constants.Policy || parseInt(docTypeId) === $.Constants.SampleDocument) {
        window.location.href = CRxUrls.Repository_PolicyBinders + "?fileId=" + fileId + "&epId=" + epId + "&doctypeId=" + docTypeId + "&fileName=" + fileName;
    }
    else if (parseInt(docTypeId) === $.Constants.Report) {
        $("#fileId").val(fileId);
        setFile(fileName, filePath);
        loadCheckPoints(epId, inspectionid);
        part2.show();
    }
};

var AddMiscEPDocs = function (fileId, fileName, epId, docTypeId) {
    var epDocTypeId = $("#item_DoctypeId").val();
    var filePaths = $("#filePaths").val();
    $.ajax({
        url: CRxUrls.Goal_AddMiscEPDocs,
        type: "POST",
        dataType: "JSON",
        data: { fileId: fileId, txtfileName: fileName, epId: epId, docTypeId: epDocTypeId, filePaths: filePaths },
        success: function (data) {
            if (data.Result) {
                swalalert(miscuploadmsg);
                refreshEPDochistory();
            }
        }
    });
};

var refreshEPDochistory = function () {
    doc_history.load(CRxUrls.Goal_EpDocHistory);
    ScrollEP();
};

var bindFilesList = function (file) {
    if (dropFiles.length > 0) {
        dropFiles = [];
    }
    $("#dropfileList ul").empty();

    dropFiles.push(file);
    dropFiles.forEach(function (item) {
        $("#dropfileList ul").append('<li id="fileli' + item.fileid + '"><div class="dropfileName"><span>File Name :</span>' + item.fileName + ' </div>' +
            '<a id="filePath" href="' + item.filePath + '" target="_blank"><img src="' + ImgUrls.detail_icon + '">' +
            '<div class="removedropfiles"><a class="removedropfile" id=' + item.fileid + '>Remove</a></div></li>');

    });
};

var setFile = function (fileName, src) {
    var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf('.'));
    var fileExtension = fileName.substr((fileName.lastIndexOf('.') + 1));
    $("#lbldocName").html(fileName);
    $("#lblfileName").html(fileName);
    $("#fileName").val(fileNamewithoutext);
    $("#fileext").val(fileExtension);
    if (src != ' ') {
        //debugger;
        document.getElementById("docframe").setAttribute("src", getfileUrl(src));

    }
    $("#frametable").show();
};

var getFileExtenstion = function (fileName) {
    var extension = "";
    if (fileName)
        extension = fileName.split(".").pop();
    return extension;
};

var getFileName = function (fileName) {
    return fileName.substring(fileName.lastIndexOf("/") + 1, fileName.lastIndexOf("."));
};

var BrowseEPDocsFile = function (file) {
   // debugger;
    var docTypeId = $("input[name='docType']:checked").val();
    var fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    if (file.files.length > 0) {
        file = file.files[0];
        var fileName = file.name;
        if (fileName != null) {
            var fileExtension = getFileExtenstion(fileName); //fileName.substring(fileName.lastIndexOf('.') + 1);
            if ($.inArray(fileExtension, fileExtensionas) == -1) {
                infoAlert("Only formats are allowed : " + fileExtensionas.join(", "));
            } else {
                var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf("."));
                $("#fileName").val(fileNamewithoutext);
                $("#lblfileName").html(fileName);
                $("#lbldocName").html(fileName);
                $("#fileext").val(fileExtension);
                var reader = new FileReader();
                reader.readAsDataURL(file);
                reader.onload = function (e) {
                    var array = reader.result.split(",");
                    $("#FileContent").val(array[1]);
                    var _documentMaster = {
                        DocTypeId: docTypeId,
                        FileName: fileName,
                        FileContent: array[1]
                    };
                    $.ajax({
                        url: CRxUrls.documents_savedocumentmasters,
                        type: "POST",
                        dataType: "JSON",
                        data: { documentMaster: _documentMaster },
                        success: function (data) {
                            policyObject.docTypeId = docTypeId;
                            var epId = $("#epId").val();
                            if (data.documentId > 0) {
                                viewAfterFileUpload(data.documentId, fileName, epId, docTypeId, data.FilePath);
                                $("#dialog-confirm").dialog("close");
                            }
                        }
                    });
                };
                reader.onerror = function (error) {
                    $("#FileContent").val("");
                };
            }
        }
    }
};

//var uploadFileToServer = function (file, epId) {
//    var docTypeId = $("input[name='docType']:checked").val();
//    var fileName = file.name;
//    if (fileName != null) {
//        var fileExtension = getFileExtenstion(fileName);
//        if ($.inArray(fileExtension, fileExtensionas) == -1) {
//            infoAlert("Only formats are allowed : " + fileExtensionas.join(", "));
//        } else {
//            var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf("."));
//            $("#fileName").val(fileNamewithoutext);
//            $("#lblfileName").html(fileName);
//            $("#lbldocName").html(fileName);
//            $("#fileext").val(fileExtension);
//            var reader = new FileReader();
//            reader.readAsDataURL(file);
//            reader.onload = function () {
//                var array = reader.result.split(",");
//                $("#FileContent").val(array[1]);
//                var _documentMaster = {
//                    DocTypeId: docTypeId,
//                    FileName: fileName,
//                    FileContent: array[1]
//                };
//                $.ajax({
//                    url: CRxUrls.documents_savedocumentmasters,
//                    type: "POST",
//                    dataType: "JSON",
//                    data: { documentMaster: _documentMaster },
//                    success: function (data) {
//                        $("#newFileName").val(getFileName(fileName));
//                        $("#newFileNameExe").val(getFileExtenstion(fileName));
//                        policyObject.docTypeId = docTypeId;
//                        if (data.documentId > 0) {
//                            showPopUpAfterUpload(docTypeId, epId, data.FilePath);
//                        }
//                    }
//                });
//            };
//            reader.onerror = function (error) {
//                console.log(error);
//                $("#FileContent").val("");
//            };
//        }
//    }

//}

//var UploadBrowsefile = function (file) {
//    var fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
//    if (file.files.length > 0) {
//        file = file.files[0];
//        var fileName = file.name;
//        if (fileName != null) {
//            var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
//            if ($.inArray(fileExtension, fileExtensionas) == -1) {
//                infoAlert("Only formats are allowed : " + fileExtensionas.join(', '));
//            } else {
//                var fileNamewithoutext = fileName.substr(0, fileName.lastIndexOf('.'));
//                $("#fileName").val(fileNamewithoutext);
//                $("#lblfileName").html(fileName);
//                $("#lbldocName").html(fileName);
//                $("#fileext").val(fileExtension);
//                var reader = new FileReader();
//                reader.readAsDataURL(file);
//                reader.onload = function (e) {
//                    var array = reader.result.split(",");
//                    $("#FileContent").val(array[1]);
//                    if (fileExtension == 'doc' || fileExtension == 'docx' || fileExtension == 'xls' || fileExtension == 'xlsx' || fileExtension == 'csv' || fileExtension == 'pdf') {
//                        $.ajax({
//                            url: CRxUrls.Common_UploadfileIntemp,
//                            type: "POST",
//                            dataType: "JSON",
//                            data: { __RequestVerificationToken: $('input[name=' + $.Constants.RequestVerificationToken + ']').val(), FileContent: array[1], FileName: fileName },
//                            success: function (data) {
//                                if (data.Result) {
//                                    $("#dropFilePath").val(data.FilePath);
//                                    var file = {
//                                        fileid: 0,
//                                        fileName: fileName,
//                                        filePath: data.FilePath,
//                                        path: data.FilePath
//                                    };
//                                    bindFilesList(file);
//                                }
//                            }
//                        });
//                    }
//                    $("#frametable").show();
//                    // To show document in iframe
//                };
//                reader.onerror = function (error) {
//                    // console.log('Error: ', error);
//                    $("#FileContent").val("");
//                };
//            }
//        }
//    }
//};

var loadCheckPoints = function (epDetailId,inspectionid) {
    if (epDetailId) {
        part1.hide();
        part2.show();
        // var urlAction = ;
        $.ajax({
           // url: CRxUrls.Goal_GetEpDocSteps + "?epDetailId=" + epDetailId + "&epIds=" + epIds,
            url: "/Goal/GetEpDocSteps?epDetailId=" + epDetailId + "&epIds=" + epIds + "&inspectionid=" + inspectionid,
            cache: false,
            type: "GET",
            success: function (data) {
                epDocSteps.empty();
                epDocSteps.html(data);
                epDocSteps.fadeIn('fast');
                //refreshEPDochistory();
                loadFiles();
                 docname = $("#EPStep_0__DocName").val();
                 doctypeid = parseInt($("#EPStep_0__DoctypeId").val());
                if (docname == "Emergency Operations Plan" || doctypeid == 1133) {
                    $('.main_class_1').hide();
                    $(".inspComment-row").hide();
                }
            }
        });
        // epDocSteps.load(urlAction + "?epDetailId=" + epDetailId);
    }
};

var loadFiles = function () {
   // debugger;
    epDocFilePreview.empty();
    var urlAction = CRxUrls.Goal_EpDocFileUploadPreview;

    var docFiles = JSON.parse(localStorage.getItem("epDocUploadFiles")).join();
    if (docFiles == undefined)
        window.location.href = CRxUrls.Goal_EpInspections + "?epId=" + epIdCtr.val();

    epDocFilePreview.load(urlAction + "?docFiles=" + docFiles);
}

var openPopup = function (epStandard, isShow, epDetailId) {
    $("#lblchoosfilemsg").show();
    $("#epId").val(epDetailId);
    $("#FileContent").val('');
    if (isShow) {
        $(".choose_file").hide();
    } else {
        $(".choose_file").show();
        $("#lblfileName").html('');
    }
    $("#divdocspoup").removeClass("disable");
    $("span.ui-dialog-title").text('Upload Document');
    $("#msg").html("for " + epStandard + "?");
    $("#dialog-confirm").dialog({
        closeOnEscape: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        },
        resizable: false,
        height: "auto",
        width: 400,
        modal: true,
        buttons: {
            No: function () {
                $(this).dialog("close");
            }
        }
    });
};

var RefreshPage = function () {
    part1.show();
    epdocs.empty();
    part2.hide();
    clearDropArea();
    var urlAction = CRxUrls.Goal_EPDocsPartial;
    epdocs.load(urlAction);
};

var clearDropArea = function () {
    dropFiles = [];
    dropEps = [];
    $("#dropfileList ul").empty();
    $("#dropEPs ul").empty();
};

$('body').on('click', 'a.removedropEP', function () {
    var ePDetailId = $(this).attr("id");
    $("div#" + ePDetailId).show();
    $("#li" + ePDetailId).remove();
    dropEps = $.grep(dropEps, function (n) {
        return n.ePDetailId != ePDetailId;
    });
});

$('body').on('click', 'a.removedropfile', function () {
    var fileId = $(this).attr("id");
    //console.log(fileId);
    $("#" + fileId).removeAttr("style");
    $("#fileli" + fileId).remove();
    $("#" + fileId).show();
    $("#d_fileId").val(0);
    $("#d_fileName").val("");
    dropFiles = $.grep(dropFiles, function (n) {
        return n.fileid != fileId;
    });
});

$('body').on('click', "#goToNextEp", function () {

    var currentEp = epIdCtr.val();
    if (epDocDetails != undefined) {
        epDocDetails.epDetailId = epDocDetails.epDetailId.filter(function (value, index, arr) {
            return value != currentEp;
        });
        if (epDocDetails.epDetailId.length > 0) {
            localStorage.setItem('epDocEpsLists', JSON.stringify(epDocDetails));
            window.location.href = CRxUrls.Goal_EPDocs + "?epId=" + epDocDetails.epDetailId[0];
        }
    }

});

$(".back").click(function () {
    //if ($("#IsDocDashboard").val() == 1) {
    //    pageMode = "MultiDocUpload";
    //}
    
    //goToPreviousPage();
    if (document.referrer) {
        window.location.href = document.referrer;
    }
    //localStorage.removeItem("epDocUploadFiles");
    //localStorage.removeItem("epDocEpsLists");
    //window.location.href = backPageUrl(pageMode); //CRxUrls.Goal_EpInspections + "?epId=" + epIdCtr.val();
});

var goToPreviousPage = function() {
    localStorage.removeItem("epDocUploadFiles");
    localStorage.removeItem("epDocEpsLists");
    window.location.href = backPageUrl(pageMode);
}

var epDocsOnSuccess = function (result) {
    if (epDocDetails != undefined) {
        epDocDetails.epDetailId = epDocDetails.epDetailId.filter(function (value, index, arr) {
            return value != result.epId;
        });
        if (epDocDetails.epDetailId.length > 0) {
            successAlert("You have " + epDocDetails.epDetailId.length + " more EPs to check.", "Success");
            //loadCheckPoints(epDocDetails.epDetailId[0]);
            localStorage.setItem('epDocEpsLists', JSON.stringify(epDocDetails));
            // New Requirements
            var assetsid = getParameterByName('assetsid');
            var epId = getParameterByName("epId");
            if (assetsid != undefined && assetsid > 0) {
                
                window.location.href = CRxUrls.Assets_Inspection + "?assetsid=" + assetsid + "&epdetailid=" + epId;
            } else {
                window.location.href = CRxUrls.Goal_EPDocs + "?epId=" + epDocDetails.epDetailId[0];
            }
             // New Requirements
        } else
            returnToBackPage();
    }
    else {
        if (result.epId == "0") {
            pageMode = result.pageMode;
            returnToBackPage();
        }
        else {

             //New Requirements
            var assetsid = getParameterByName('assetsid');
            var epId = getParameterByName("epId");
            var inspectionid = getParameterByName("inspectionid");
            if (assetsid != undefined && assetsid > 0) {
                //localStorage.setItem('body')
                window.location.href = CRxUrls.Assets_Inspection + "?assetsid=" + assetsid + "&epdetailid=" + epId + "&Isupload=" + 1 + "&inspectionid=" + inspectionid;
            } else {
                returnToBackPage();
            }
             // New Requirements
           
        }

        
    }
}

var returnToBackPage = function () {
    localStorage.removeItem("epDocUploadFiles");
    localStorage.removeItem("epDocEpsLists");
    successAlert("You have successfully attached document to EP.", "Success");
    $("#btnSubmit").addClass("disable");
    window.location.href = backPageUrl();
}

var backPageUrl = function () {
   // debugger;
    var returnUrl = '';
    switch (pageMode) {
        case 'vendorreport':
            returnUrl = CRxUrls.EPGroups_VendorReports;
            break;
        case 'policyupload':
            returnUrl = CRxUrls.Repository_PolicyBinders + "?IsBack=1";
            break;
        case 'MultiDocUpload':
            returnUrl = "/dashboard/documents" ;
            break;
        case 'EPInspection':
            returnUrl = "/Goal/EpInspections?epId=" + epIdCtr.val() + "&inspectionid=" + inspectionid;
            break;
        case 'EpHisotry':            
            returnUrl = "/Goal/EpInspectionsHistory?epId=" + epIdCtr.val();
            break; 
        default:
            returnUrl = "/Goal/EPstate?epId=" + epIdCtr.val() + "&inspectionid=" + inspectionid;
    }
    return returnUrl;
}



$(function () {
    var _return = false
   
    $(document).on('click', "#btnSubmit", function () {
   // $("#btnSubmit").click(function () {
       
       
        if (docname == "Emergency Operations Plan" || doctypeid == 1133) {
            if (isEOP == 0) {
                confirmEOPEpInspectionPopUp()
            }
            else {
                submitform()
            }

        }
        else {
            submitform()
        }
        debugger
        
       
       

        return _return;
    });
});

var confirmEpInspectionPopUp = function () {
    swal({
        html: true,
        title: "All Documents Check",
        text: '<div><label> If EP Checkpoints are not marked then this EP will Be mark As In Progress.</label></div>' +
            '<div><label> If you want to review this EP later then please un-check <h4>"All documents are uploaded"</h4> checkbox.</label></div>'+
            '<div><h2> Do you want to continue ?</h2></div>',

        type: "info",
        showCancelButton: true,
        cancelButtonColor: "#e20e17",
        confirmButtonColor:"#78a133",
        confirmButtonText: "Yes",
        cancelButtonText: "No",
        closeOnConfirm: true,
        closeOnCancel: true,
    },
        function (isConfirm) {
            if (isConfirm) {
                //if ($("#epId").val() != 0) {
                //    EPReviewNotification();
                //}
                //else {


                    $("#form0").submit();
                //}
                
                
               
            }
            else {
                _return= false;
            }
        });
}


var confirmEOPEpInspectionPopUp = function () {
    swal({
        html: true,
        title: "EOP EP Inspection",
        text: '<div><label> Below are EPs that describe the contents of the Emergency Operations Plan.</label></div>' +
               '<div><h2> Would you like to review these EPs?</h2></div>',

        type: "info",
        showCancelButton: true,
        cancelButtonColor: "#e20e17",
        confirmButtonColor: "#78a133",
        confirmButtonText: "Yes",
        cancelButtonText: "Later",
        closeOnConfirm: true,
        closeOnCancel: true,
    },
        function (isConfirm) {
            setTimeout(function () {
                if (isConfirm) {

                    $(".main_class_1").show();
                    $(".inspComment-row").show();
                    isEOP = 1
                }
                else {
                    submitform()
                }

            }, 400)
           
        });
}

var submitform = function () {
    localStorage.InspDate = $('#effectiveDate').val();
    if ($('.main_class_1').css('display') != 'none') {


        noncompliant = $('.main_class_1 :input:button').map(function () {
            var type = $('.goal-step-doc>div .ins_check2_btn:input').prop('type');
            if (type = "button" && type != "submit") {
                if ($(this).val() == -1) {
                    return $(this).val()
                }
            }

        })
        if (noncompliant.length > 0) {
            if ($('#effectiveDate').val() != "") {
                confirmEpInspectionPopUp();
                
            }
            else {

                swal({
                    title: "Required!",
                    text: "Select Review Date.",
                    type: "error",
                    timer: 3000
                });
                e.preventDefault();
            }
            if ($('#uploaddocTypeId').val() != null) {
                confirmEpInspectionPopUp();
            }
            else {

                swal({
                    title: "Required!",
                    text: "Select Upload As.",
                    type: "error",
                    timer: 3000
                });
            
                
            }
            

        }
        else {
            if ($('#effectiveDate').val() != "") {

                //$("#form0").submit();
                //_return = true;
                if ($('#uploaddocTypeId').val() != null) {
                    $("#form0").submit();
                    _return = true;

                }
                else {

                    swal({
                        title: "Required!",
                        text: "select Upload As.",
                        type: "error",
                        timer: 3000
                    });
                    _return = false;

                }
            }
            else {

                swal({
                    title: "Required!",
                    text: "Select Review Date.",
                    type: "error",

                });
                //swalalert('select Review Date');
                _return = false;
                e.preventDefault();
            }

            //if ($('#uploaddocTypeId').val() != null) {
            //    $("#form0").submit();
            //    _return = true;

            //}
            //else {

            //    swal({
            //        title: "Required!",
            //        text: "select Upload As.",
            //        type: "error",
            //        timer: 3000
            //    });
            //    _return = false;
                
            //}
        }
    }

    else {
        if ($('#effectiveDate').val() != "") {
           
            //  $("#form0").submit();
            //_return = true;
            if ($('#uploaddocTypeId').val() != null) {

                $("#form0").submit();
                _return = true;
            }
            else {

                swal({
                    title: "Required!",
                    text: "select Upload As.",
                    type: "error",

                });
                //swalalert('select Review Date');
                _return = false;

            }

        }
        else {

            swal({
                title: "Required!",
                text: "Select Review Date.",
                type: "error",

            });
            //swalalert('select Review Date');
            _return = false;
            e.preventDefault();
        }
      
        

    }
}



