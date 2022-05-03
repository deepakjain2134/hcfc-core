Dropzone.autoDiscover = false;

$(document).ready(function () {
    setSelectedFile();
    $(".btnSelect").not(".active").each(function () {
        var fileId = $(this).attr('fileid');
        if ($("#file-lists").find("ul").find("#lidv_" + fileId).length > 0) {
            $(this).parents('tr').addClass("trfileactive")
            $(this).attr('src', '/dist/Images/Icons/remove-btn.png');
            $(this).addClass("active");
        }
    });
});

var myDropzone = $('#myDropzone').dropzone({
    //parameter name value
    paramName: "files",
    thumbnailWidth: "350",
    //clickable div id
    clickable: '#previews',
    //preview files container Id
    // previewsContainer: "#previewFiles",
    autoProcessQueue: false,
    uploadMultiple: true,
    parallelUploads: 100,
    previewTemplate: document.getElementById('template-preview').innerHTML,
    maxFiles: 100,
    maxFilesize: 500,
    //  url:"/", // url here to save file
    //maxFilesize: 100,//max file size in MB,
    addRemoveLinks: true,
    dictResponseError: 'Server not Configured',
    acceptedFiles: ".png,.jpg,.jpeg,.pdf,.doc,.docx,.csv,.xls,.xlsx",// use this to restrict file type
    dictDuplicateFile: "Duplicate Files Cannot Be Uploaded",
    preventDuplicates: true,
    init: function () {
        var self = this;
        // config
        self.options.addRemoveLinks = true;
        self.options.dictRemoveFile = "Delete";
        //New file added
        self.on("addedfile", function (file) {
            debugger;
            $(".dz-preview").removeClass("errorFileName");
            if (this.files.length) {
                var _i, _len;
                for (_i = 0, _len = this.files.length; _i < _len - 1; _i++) {
                    if (this.files[_i].name === file.name && this.files[_i].size === file.size && this.files[_i].lastModifiedDate.toString() === file.lastModifiedDate.toString()) {
                        AlertWarningMsg(file.name + " already added.");
                    }
                }
            }
            var ext = file.name.split('.').pop();
            var fileNameWithOutExt = file.name.replace(/(.*)\.(.*?)$/, "$1");
            $(file.previewElement).find(".fileName").val(fileNameWithOutExt);

            if (ext == "pdf") {
                $(file.previewElement).find(".dz-image img").attr("src", "https://upload.wikimedia.org/wikipedia/commons/8/87/PDF_file_icon.svg");
            } else if (ext.indexOf("doc") != -1) {
                $(file.previewElement).find(".dz-image img").attr("src", "https://upload.wikimedia.org/wikipedia/commons/f/fb/.docx_icon.svg");
            } else if (ext.indexOf("xls") != -1) {
                $(file.previewElement).find(".dz-image img").attr("src", "https://upload.wikimedia.org/wikipedia/commons/archive/f/fb/20190228175239%21.docx_icon.svg");
            } else
                $(file.previewElement).find(".dz-image img").attr("src", "https://upload.wikimedia.org/wikipedia/commons/f/fb/.docx_icon.svg");

            $('.uploadSteps').removeClass('fileactive');
            $('#dropfilesdetails').addClass('fileactive');
            $('.dz-success-mark').hide();
            $('.dz-error-mark').hide();

        });

        // Send file starts
        self.on("sending", function (file) {
            $('.meter').show();
        });

        this.on("successmultiple", function (file, responseText) {
            console.log('successmultiple', responseText, file);
            $("body").removeClass("loading");
            if (responseText.foundDuplicateFiles) {
                showDuplicateFIleBox(responseText.Result, responseText.duplicateFiles, file);
            } else if (responseText.status) {
                bindNewFiles(responseText.Result, 1);
                self.removeAllFiles();
            } else {
                swalalert('please try again later.');
            }
        });

        // File upload Progress
        self.on("totaluploadprogress", function (progress) {
            console.log("progress ", progress);
            $('.roller').width(progress + '%');
        });

        self.on("queuecomplete", function (progress) {
            $('.meter').delay(999).slideUp(999);
        });

        // On removing file
        self.on("removedfile", function (file) {
            console.log(this.files.length);
            console.log(file);
            if (this.files.length == 0) {
                refreshControl();
            }
        });

        $('#Submit').on("click", function (e) {
            $(".dz-preview").removeClass("errorFileName");
            e.preventDefault();
            e.stopPropagation();
            var fileObjects = [];
            var status = 1;
            var inValidFileNameCount = 0;
            if (self.getQueuedFiles().length > 0) {
                $(".dz-preview").each(function () {
                    var newfileName = $(this).find(".fileName").val().replace("&amp;", "&");
                    var orgFileName = $(this).find("[data-dz-name]").html().replace("&amp;", "&");
                    console.log(newfileName, orgFileName);
                    if (orgFileName.length > 0) {
                        if (newfileName.length == 0) {
                            swalalert('No file name found with this file');
                            status = 0;
                        }
                        var fileObject = {
                            FileName: newfileName,
                            OrgFileName: orgFileName
                        };
                        fileObjects.push(fileObject);
                    }

                    let isValidFileName = PreFileName(newfileName.replace(" ", "_"));
                    if (isValidFileName) {
                        inValidFileNameCount = inValidFileNameCount + 1;
                        $(this).addClass("errorFileName");
                    }
                });


                if (status > 0 && inValidFileNameCount == 0) {
                    $("input[name='filesData']").val(JSON.stringify(fileObjects));
                    $("body").addClass("loading");
                    self.processQueue();
                } else if (inValidFileNameCount > 0) {
                    AlertWarningMsg("Please remove the special characters from the file name and upload again.");
                }
            }
            else {
                AlertWarningMsg('No file to upload');
            }
        });
    }
});

$(document).off('click', '#btnDuplicateAttachFiles').on('click', '#btnDuplicateAttachFiles', function () {
    var selectedFiles = JSON.parse(localStorage.getItem("tempFileUpload"));
    if (selectedFiles.length > 0) {
        $.ajax({
            method: 'POST',
            url: '/Common/UploadFile?fileIds="123,123"',
            data: { files: selectedFiles, __RequestVerificationToken: $('input[name=' + $.Constants.RequestVerificationToken + ']').val() },
            success: function (responseText) {
                bindNewFiles(responseText.Result, 1);
            },
            error: function () {
                alert("Error occurs");
            }
        });
    }
});

$(".cancel-btn").click(function () {
    refreshControl();
    $("#modal-container").modal("hide");
});

$(".btnBack").click(function () {
    refreshControl();
});

function PreFileName(filename) {
    var format = /[`!@#$%^&*()+\=\[\]{};':"\\|,<>\/?~]/;
    return format.test(filename);
}


function refreshControl() {
    $('.uploadSteps').addClass('fileactive');
    $('#recentlydetails').removeClass('fileactive');
    $('#browsedetails').removeClass('fileactive');
    $('#dropfilesdetails').removeClass('fileactive');
    $('#crxinbox').removeClass('fileactive');
    $("#duplicatesFiles").removeClass('fileactive');
    Dropzone.forElement("#myDropzone").removeAllFiles(true);
    $("#inboxTable_filter").find("label").find("input").val("");
}

function loadDropZone() {

}

$(document).off('click', '.btnSelect').on('click', '.btnSelect', function () {
    $(this).toggleClass("active");
    if ($(this).hasClass('active')) {
        $(this).parents('tr').addClass("trfileactive");
        $(this).attr('src', '/dist/Images/Icons/remove-btn.png');
    }
    else {
        $(this).parents('tr').removeClass("trfileactive");
        $(this).attr('src', '/dist/Images/Icons/addplus-btn.png');
        var fileId = $(this).attr('fileid');
        deleteattachfile(fileId);
    }
    return false;
});

function clickviewtable() {
    $("#filesTable").toggle();
}

function setSelectedFile() {
    var fileIds = $("#attachFiles").val();
    if (fileIds != undefined) {
        console.log(fileIds);
        if (fileIds.length > 0) {
            var file = fileIds.split(",");
            $.each(file, function (i, ctrFile) {
                var fileId = ctrFile.split("_")[0];
                $("[fileid='" + fileId + "']").addClass('active').attr('src', '/dist/Images/Icons/remove-btn.png');
            });
            $(".lblfilecount").html(file.length);
            $(".dvfilecount").show();
        } else { $(".dvfilecount").hide(); }

    }

}

$(document).off('click', '.btnAttachFiles').on('click', '.btnAttachFiles', function () {
    getSelectedFile();
});

function BindUploadFile() {
    var attachmentsId = $("#attachFiles").val();
    if (attachmentsId != undefined) {
        $.ajax({
            url: CRxUrls.Common_GetUploadedFile + "?attachment=" + $("#attachFiles").val(),
            cache: false,
            type: "GET",
            success: function (data) {
                var filelst = "";
                if (data != null) {
                    for (var i = 0; i < data.length; i++) {
                        filelst += '<li class="list-group-item files" id = "lidv_' + data[i].TFileId + '" > ';
                        if (data[i].FileName != "") {
                            filelst += '<a href="/Common/ImagePreview?imgSrc=' +
                                data[i].FilePath +
                                '&title=Preview" class="modal-link allowclick" >';
                            filelst += data[i].FileName + '</a >';
                            filelst += ' <a data-id=' +
                                data[i].TFileId +
                                ' class="deletefile allowclick"> <img id="deleteimg" alt="Delete" title="Delete file" src="/dist/Images/Icons/red_cross.png" /> </a></li>';

                            if ($("#file-upload-list").find("#file-lists").find("ul").length > 0 ||
                                typeof $("#file-upload-list").find("#file-lists").find("ul") !== 'undefined') {
                                $('#lidv_' + data[i].TFileId).remove();
                            } else {

                            }
                        }

                    }
                    if ($("#file-upload-list").find("#file-lists").find("ul").length > 0 ||
                        typeof $("#file-upload-list").find("#file-lists").find("ul") !== 'undefined') {
                        $("#file-upload-list").find("#file-lists").find("ul").append(filelst);
                    } else {
                        $("#file-upload-list").find("#file-lists").find("ul").html(
                            '<div id="file-lists"><ul class="col-lg-12 attachfileitems">' + filelst + '</ul></div>');
                    }
                }
            }
        });
    }
}

function uploadFiredrillFiles(file) {
    debugger
    fileExtensionas = ['pdf'];
    var filelist = file;
    if (filelist.TFileId != "") {
        debugger;
        var file = file;
        var fileName = file.FullName;
        var filePath = file.FilePath;
        var Tfileids = file.TFileId;
        var fileExtension = fileName.substring(fileName.lastIndexOf('.') + 1);
        if ($.inArray(fileExtension.toLowerCase(), fileExtensionas) == -1) {
            swalalert("Only Pdf formats are allowed!");
            return false;
        } else {
            swal.close();
            var drillDate = $("input[name='Buildings[0].Shifts[0].Exercises[0].Date'][type=hidden]").val();
            var isFutureDate = new Date(drillDate) > new Date();
            var value = $("#Isfinalreport").prop('checked');
            if (value && !isFutureDate) {
                //$("input[name='Buildings[0].Shifts[0].Exercises[0].Status'][type=hidden]").val(1);
            }
            addTexerciseFiles();
            count = parseInt($('.texercisefilediv:last').attr('count'));
            var control = "Buildings[0].Shifts[0].Exercises[0].TExerciseFiles[" + count + "].FilesContent";
            var fileControl = "Buildings[0].Shifts[0].Exercises[0].TExerciseFiles[" + count + "].FileName";
            var drillfiletype = "Buildings[0].Shifts[0].Exercises[0].TExerciseFiles[" + count + "].DrillFileType";
            var filepath = "Buildings[0].Shifts[0].Exercises[0].TExerciseFiles[" + count + "].FilePath";
            var TFileIds = "Buildings[0].Shifts[0].Exercises[0].TExerciseFiles[" + count + "].TFileIds";
            var reader = new FileReader();


            seltabval = parseInt($('#tabs ul li.ui-tabs-active').attr('data-val'));
            $("#uploadfileName" + count).html(fileName);
            //var array = reader.result.split(",");
            //$("input[name='" + control + "'][type=hidden]").val(array[1]);
            $("input[name='" + fileControl + "'][type=hidden]").val(fileName);
            $("input[name='" + filepath + "'][type=hidden]").val(filePath);

            $("input[name='" + drillfiletype + "'][type=hidden]").val(seltabval);
            $("input[name='" + TFileIds + "'][type=hidden]").val(Tfileids);
            $("#uploadfileName" + count).addClass("drilltype" + seltabval);
            showfileName(seltabval);
            var fdatatable = $('#texerciseTable').DataTable();
            var col3 = '<a href="" title="Preview" class="disabled"><img src="/dist/Images/Icons/document_blue-icon.png"  title="View" alt="" /></a>'
            var todayDate = dateFormat(new Date());
            fdatatable.row.add([seltabval, fileName, todayDate, col3]).draw();
        }
    }
}


var monthShortNames = ["Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"
];
function dateFormat(d) {
    var t = new Date(d);
    return monthShortNames[t.getMonth()] + ' ' + t.getDate() + ', ' + t.getFullYear();
}

function getSelectedFile() {
    var selectedFiles = [];

    var tFiles = [];
    var isFiredrillFiles = $('#isFiredrillFiles').val();
    var isexist = false;
    debugger;
    $("#filesTable .btnSelect.active").each(function () {
        var fileId = $(this).attr('fileid');
        var tblName = $(this).attr('tblName');
        var filesize = $(this).attr('filesize');
        var files = fileId + '_' + tblName;
        debugger;
        var tfile = {};
        tfile.TFileId = fileId;
        tfile.FileSize = filesize;
        tfile.FilePath = $(this).closest("tr").find(".filePath").attr("href");
        tfile.FileName = $(this).closest("tr").find(".fileName").text().trim();
        tfile.Extension = tfile.FilePath.split(".").pop();
        tfile.FullName = tfile.FileName.concat(".", tfile.Extension);
        tFiles.push(tfile);

        if ($("#file-lists").find("ul").find("#lidv_" + fileId).length > 0) {
            isexist = true;
        }
        else if (isFiredrillFiles == "true") {
            uploadFiredrillFiles(tfile);
            selectedFiles.push(files);
        }
        else {
            selectedFiles.push(files);
        }
    });


    $("#lbldocmsg").hide();
    if (selectedFiles.length == 0 && isexist)
        swalalert('Please add file which is not already attached');
    else if (selectedFiles.length == 0)
        swalalert('To attach first select files.');
    else {
        if ($("#attachFiles").val() != "" && $("#attachFiles").val() != undefined)
            $("#attachFiles").val($("#attachFiles").val().replace(/,\s*$/, "") + "," + selectedFiles.toString());
        else
            $("#attachFiles").val(selectedFiles.toString());

        $("#lbldocmsg").show();

        $("#attchFileCount").text(selectedFiles.length);
        $("#modal-container").modal("hide");

        setTimeout(function () {
            $('body').removeClass('modal-open');
        },
            1000);

        if (typeof (window.fileUploadSuccess) != "undefined") {
            window.fileUploadSuccess(tFiles, selectedFiles);
        } else {
            BindUploadFile();
        }

    }
}

$("#modal-container").on('shown.bs.modal',
    function () {
        setSelectedFile();
        if (!$("#myDropzone").hasClass('myDropzone')) {
            loadDropZone();
        }
    });

function deleteattachfile(tFileId) {
    $("#lidv_" + tFileId).remove();
    if ($("#attachFiles").val().indexOf(tFileId + '_TFiles,') !== -1) {
        var ret = $("#attachFiles").val().replace(tFileId + '_TFiles,', '');
        $("#attachFiles").val(ret);
    }
    else if ($("#attachFiles").val().indexOf(tFileId + '_TFiles') !== -1) {
        var ret = $("#attachFiles").val().replace(tFileId + '_TFiles', '');
        $("#attachFiles").val(ret);
    }
    var ids = $("#attachFiles").val();
    var lastChar = ids.slice(-1);
    if (lastChar == ',') {
        ids = ids.slice(0, -1);
    }
    $("#attachFiles").val(ids);

    if ($('#attachFiles').val() != "") {
        $("#lbldocmsg").show();
    } else {
        $("#lbldocmsg").hide();
    }
}

function deletedrawings(tFileId) {
    $("#lidrawingdv_" + tFileId).remove();
    if ($("#attachdrawingFiles").val().indexOf(tFileId) !== -1) {
        var ret = $("#attachdrawingFiles").val().replace(tFileId, '');
        $("#attachdrawingFiles").val(ret);
    }

    var ids = $("#attachdrawingFiles").val();
    var lastChar = ids.slice(-1);
    if (lastChar == ',') {
        ids = ids.slice(0, -1);
    }
    $("#attachdrawingFiles").val(ids);
}