function markallcompliantmsg() {
    swal({
        title: "Message",
        text: "Please make all the status compliant!",
        type: "warning",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
    });
}

function swalalertLogin(title, text, type) {
    if (!title)
        title = "Message";
    debugger;
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Login",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = CRxUrls.Account_Login;
        }
    });
}

function redirecttoWOassets(confirmRedirectUrl, cancelRedirectUrl) {
    debugger;
    swal({
        html: true,
        title: "Message",
        text: "<label>First close the Work order</label><br />",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO WORK ORDERS",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            window.location.href = cancelRedirectUrl;
        }
    });
}

function redirecttodefeciencies(confirmRedirectUrl) {
    debugger;
    swal({
        html: true,
        title: "Message",
        text: "<label>There are still pending deficiencies since last inspection.</label><br /><label> Please fix it before proceeding with new inspection</label>",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO DEFICIENCIES",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            // window.history.back();
            swal.close();
        }
    });
}

function redirectosetupassets(confirmRedirectUrl, cancelRedirectUrl) {
    swal({
        title: "Attach assets",
        text: "There is no asset for inspection!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "setup",
        cancelButtonText: "cancel",
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            window.location.href = cancelRedirectUrl;
        }
    });
}

function RedirectScheduler(confirmRedirectUrl) {
    swal({
        html: true,
        title: "Message",
        text: "<label>There are no schedules please create schedules first!!</label>",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "GO TO SCHEDULES",
        cancelButtonText: "CANCEL",
        closeOnConfirm: false,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = confirmRedirectUrl;
        } else {
            history.back();
            $("#submitButton").addClass("disable");
            $("#resumeButton").addClass("disable");
            $("#btnSubmit").addClass("disable");
            //window.location.href = cancelRedirectUrl;
        }
    });
}

function InsUploadfile(file, controlId, fileType) {
    var thisfile = $(file);
    var fileExtensionas = [];
    if (fileType === "D") {
        fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    }
    if (fileType === "C") {
        fileExtensionas = ['png', 'jpg', 'jpeg'];
    }
    if (fileType === "F") {  // for floorPlan
        fileExtensionas = ['dwg', 'svg', 'png', 'jpg', 'jpeg', 'pdf'];
    }
    var control = file.getAttribute("tempName");
    var fileControl = file.getAttribute("filename");
    var filepath = file.getAttribute("filepath");

    if (file.files.length > 0) {
        var file = file.files[0];
        var fileName = file.name;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileExtension.toLowerCase(), fileExtensionas) == -1) {
            $(thisfile).val("")
            swal("Only formats are allowed : " + fileExtensionas.join(', '));
            return false;
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                // console.log(reader.result);
                var array = reader.result.split(",");
                $("input[name='" + control + "'][type=hidden]").val(array[1]);
                $("input[name='" + fileControl + "'][type=hidden]").val(fileName);
                $("input[name='" + filepath + "'][type=hidden]").val("");
            };
            reader.onloadend = function () {
                if (fileType == "C") {
                    $("#imagePreview_" + controlId).attr('src', this.result);
                }
                else if (fileType == "F") { /// for floor Plan specail requirement
                    if (fileExtension == "pdf") {
                        $("#imagePreview_" + controlId).attr('src', "../dist/Images/Icons/pdf_icon.png");
                    } else {
                        $("#imagePreview_" + controlId).attr('src', this.result);
                    }
                }
                else {
                    $("#imagePreview_" + controlId).attr('src', "/dist/Images/Icons/document_blue-icon.png");
                }
                $("#imagePreview_" + controlId).removeClass("img_prev");
                $("#imagePreview_" + controlId).addClass("img_prev_upload");
            };
            reader.onerror = function (error) {
                $("#FileContent").val("");
            };
            $(".part1").hide();
            $(".part2").show();
        }
    }
}

function InsUploadAndSavefile(file, fileType, stepId) {
    var thisfile = $(file);
    var fileExtensionas = [];
    if (fileType === "D") {
        fileExtensionas = ['pdf', 'xlsx', 'xls', 'doc', 'docx', 'csv'];
    }
    if (fileType === "C") {
        fileExtensionas = ['png', 'jpg', 'jpeg'];
    }
    if (file.files.length > 0) {
        var file = file.files[0];
        const sizebytes = file.size;
        const filesize = Math.round((sizebytes / 1024));
        // The size of the file. 
        if (filesize >= 5120) {
            AlertWarningMsg("File too Big, please select a file less than 5mb");
            return false;
        }
        var fileName = file.name;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileExtension.toLowerCase(), fileExtensionas) == -1) {
            $(thisfile).val("");
            AlertWarningMsg("Only " + fileExtensionas.join(', ') + " formats are allowed.");
            return false;
        } else {
            var reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = function () {
                var inspectionId = $("#InspectionId").val();
                var activityId = $("#TinspectionActivity_0__ActivityId").val();
                var fileData = new FormData();
                fileData.append(fileName, file);
                fileData.append('StepsId', stepId);
                fileData.append('FileType', fileType);
                fileData.append('ActivityId', activityId);
                $.ajax({
                    url: "/Goal/AddInspectionFiles?inspectionId=" + inspectionId,
                    method: "POST",
                    data: fileData,
                    contentType: false,
                    dataType: 'html',
                    processData: false,
                    success: function (data) {
                        successAlert('File uploaded successfully.');
                        $("input[type='file']").val('');
                        if (stepId > 0) {
                            debugger;
                            $("#demofiles_" + stepId).children("ul").append(data);
                            var filelength = $('#demofiles_' + stepId + ' ul li').length;
                            debugger;
                            if (filelength >= 3) {
                                $("#divfiles_" + stepId).addClass('disable');
                            }
                        } else {
                            loadilsmdetailpartial();
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

function ImageUpload(file) {
    var control = file.getAttribute("tempName");
    var fileContent = file.getAttribute("filecontent");
    if (file.files.length > 0) {
        file = file.files[0];
        var fileName = file.name;
        var reader = new FileReader();
        reader.readAsDataURL(file);
        reader.onload = function () {
            var array = reader.result.split(",");
            $("input[name='" + control + "'][type=hidden]").val(fileName);
            $("input[name='" + fileContent + "'][type=hidden]").val(array[1]);
            $("input[name='FilePath'][type=hidden]").val(null);
            $('input[name="FilePath"').attr("data-dirty-initial-value", "");
        };
        reader.onerror = function (error) {
            console.log('Error: ', error);
            $("#FileContent").val("");
        };
    }
}

function ConfirmPopUp(title, text, type, confirmButtonText, confirmRedirectUrl, cancelRedirectUrl, isCloseOnConfirm = false, isCloseOnCancel = false) {
    swal({
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: "Cancel",
        closeOnConfirm: isCloseOnConfirm,
        closeOnCancel: isCloseOnCancel
    }, function (isConfirm) {
        debugger;
        if (isConfirm) {
            if (!isCloseOnConfirm) {
                window.location.href = confirmRedirectUrl;
            }
        } else {
            if (cancelRedirectUrl === "-1")
                window.history.back();
            else
                window.location.href = cancelRedirectUrl;
        }
    });
}

function SwalConfirmPopUp(title, text, type, confirmButtonText, confirmRedirectUrl, cancelButtonText, cancelRedirectUrl) {

    var isCloseOnConfirm = false;
    if (confirmRedirectUrl)
        isCloseOnConfirm = true;

    var iscloseOnCancel = true;
    if (cancelRedirectUrl)
        iscloseOnCancel = false;

    swal({
        html: true,
        title: title,
        text: text,
        type: type,
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        cancelButtonText: cancelButtonText,
        closeOnConfirm: isCloseOnConfirm,
        closeOnCancel: iscloseOnCancel
    }, function (isConfirm) {
        if (isConfirm) {
            if (confirmRedirectUrl != "") {
                window.location.href = confirmRedirectUrl;
            }
            else {
                swal.close();
            }
        } else {
            if (cancelRedirectUrl) {
                window.location.href = cancelRedirectUrl;
            }
        }
    });
}

function ShowConfirmMsg(title, type, text, confirmButtonText, confirmBackUrl) {
    swal({
        html: true,
        title: title,
        text: text,
        type: type,
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        closeOnConfirm: false,
        closeOnCancel: true
    },
        function (isConfirm) {
            if (isConfirm) {
                window.location.href = confirmBackUrl;
            }
        });
}

function AlertSuccessMsg(sText, title) {
    if (!title)
        title = "Message";
    swal({
        title: title,
        text: sText,
        type: "success",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function AlertWarningMsg(sText, title) {
    if (!title)
        title = "Message";
    swal({
        title: title,
        text: sText,
        type: "warning",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function AlertInfoMsg(sText, title) {
    if (!title)
        title = "Message";

    swal({
        title: title,
        text: sText,
        type: "info",
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function swalalert(title, text, type) {
    if (!title)
        title = "Message";

    swal({
        title: title,
        text: text,
        type: type,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "OK",
        closeOnConfirm: true,
        timer: 3000
    });
}

function showInforMsg(title, text, confirmButtonText, redirectToUrl) {
    swal({
        html: true,
        title: title,
        text: text,
        type: "info",
        showCancelButton: false,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: confirmButtonText,
        closeOnConfirm: true,
        closeOnCancel: true
    }, function (isConfirm) {
        if (isConfirm) {
            window.location.href = redirectToUrl;
        }
        else {
            window.location.href = redirectToUrl;
        }
    });

}



