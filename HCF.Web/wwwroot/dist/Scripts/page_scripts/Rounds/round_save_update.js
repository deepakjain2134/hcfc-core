$(document).ready(function () {

    // add temp step to existing on going round 

    var addStepButton = $(".quest-addbtn");

    $('body').on('click', '.quest-addbtn', function () {
    //addStepButton.click(function () {
        var catId = $(this).attr("catId");
        var troundId = $(this).attr("troundId");
        var floorId = $(this).attr("floorId");
        console.log(catId, troundId)
        openQuestPopus(catId, troundId, floorId);
    });

    function openQuestPopus(catId, troundId, floorId) {
        swal({
            title: "Add Round Questionnaire",
            // text: "round Questionnaire:",
            type: "input",
            showCancelButton: true,
            closeOnConfirm: true,
            animation: "slide-from-top",
            inputPlaceholder: "Add Round Questionnaire"
        },
            function (inputValue) {
                debugger;
                if (inputValue === null || inputValue ===false) return false;

                if (inputValue === "") {
                    swal.showInputError("You need to write something!");
                    return false
                }
                var rundsQuestionnaires = {
                    RoundCatId: catId,
                    RoundStep: inputValue
                };
                $.ajax({
                    url: "/Round/SaveTempRoundsQuestionnaires?roundId=" + troundId + "&floorId=" + floorId,
                    method: "POST",
                    data: { roundQues: rundsQuestionnaires, __RequestVerificationToken: $('input[name=' + $.Constants.RequestVerificationToken + ']').val() },
                    success: function (data) {
                        var catIdTable = $("#round_cat_" + catId);
                        catIdTable.find(".details").eq(-1).after(data);
                        $(".modal-stack").remove();
                        $(".modal-stack").removeAttr("style");
                        $(".modal-stack").removeClass("modal-backdrop");
                    }
                });
            });
    }

    //save inspection data

    $('body').on('change', '.stepComment', function () {
        var control = this;
        saveData(control);
    });

    $(document).off("change", ".comment").on("change", ".comment", function () {
        var questionnaires = [];
        var troundId = $(this).attr("troundid");
        var roundCatId = $(this).attr("roundcatid");
        var overAllComment = $(this).val();
        saveInspection(questionnaires, true, null, troundId, overAllComment, roundCatId);
    });


    $(document).off("click", ".dropdown-content a").on("click", ".dropdown-content a", function () {
        var control = $(this);
        //debugger;
        var statusVal = control.attr("val");
        var controlId = control.parent("div").attr("tempname");
        $("input[tempname='" + controlId + "']").val(statusVal);
        $("input[name='" + controlId + "'][type=hidden]").val(statusVal);
        //console.log(".dropdown-content View:RoundRoundQuestionaries componsent");
        saveData(control);
    });


    //$(".closeRound").click(function () {       
    //    var status = 1;
    //    swal({
    //        title: "Are you sure? ",
    //        text: "Are you sure you want to close this round?",
    //        type: "info",
    //        showCancelButton: true,
    //        confirmButtonColor: "#DD6B55",
    //        confirmButtonText: "Yes",
    //        cancelButtonText: "No",
    //        closeOnConfirm: true,
    //        closeOnCancel: true
    //    }, function (isConfirm) {
    //        if (isConfirm) {
    //            closeRounds(roundId, 1);
    //        }
    //    });
    //});


    //$(".reopenRound").click(function () {       
    //    var status = 2;
    //    swal({
    //        title: "Are you sure? ",
    //        text: "Are you sure you want to re-open the closed round?",
    //        type: "info",
    //        showCancelButton: true,
    //        confirmButtonColor: "#DD6B55",
    //        confirmButtonText: "Yes",
    //        cancelButtonText: "No",
    //        closeOnConfirm: true,
    //        closeOnCancel: true
    //    }, function (isConfirm) {
    //        if (isConfirm) {
    //            reopenround(roundId, 2);
    //        }
    //    });
    //});


    // files related   


});

function roundInsUploadfile(file, controlId, fileType) {

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
                var array = reader.result.split(",");
                $("input[name='" + control + "'][type=hidden]").val(array[1]);
                $("input[name='" + fileControl + "'][type=hidden]").val(fileName);
                $("input[name='" + filepath + "'][type=hidden]").val("");
                saveData("#imagePreview_" + controlId);
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
                $("#deleteimg_" + controlId).removeClass("hide");
                $("#deleteimg_" + controlId).addClass("deleteroundimg");
                $("#img_file_upload_" + controlId).addClass('hide').removeClass("show");
            };
            reader.onerror = function (error) {
                $("#FileContent").val("");
            };
            $(".part1").hide();
            $(".part2").show();
        }
    }
}

function saveData(control) {
    var roundCatId = $(control).closest("table").attr("roundCatId");
    var overAllComment = $(control).closest("table").find("comment").val();
    var RouQuesId = $(control).closest("tr").attr("id").split("_")[1];
    var comment = $(control).closest("tr").find(".stepComment").val();
    var status = $(control).closest("tr").find(".ins_check3_btn").attr("value");//control.value;
    TRoundId = $(control).closest("tr").find("#TRoundId").attr("value");
    var fileName = $(control).closest("tr").find(".fileName").val();
    var fileContent = $(control).closest("tr").find(".fileContent").val();
    var filePath = $(control).closest("tr").find(".filePath").val();
    var questionnair = {
        RouQuesId: RouQuesId,
        Status: status,
        Comment: comment,
        TRoundId: TRoundId,
        FileName: fileName,
        FileContent: fileContent,
        filePath: filePath
    };
    var questionnaires = [];
    questionnaires.push(questionnair);
    saveInspection(questionnaires, true, status, TRoundId, overAllComment, roundCatId);
}

function saveInspection(questionnaires, isOpen, inspStatus = 2, roundid, overAllComment, roundCatId) {
    if (canUpdate()) {
        console.log($("#hdn_floorId").val());
        var currentInsp = {
            FloorId: $("#hdn_floorId").val(),
            TRoundId: roundid,
            Comment: overAllComment, //$(".comment[troundid=" + roundid + "]").val(),
            Status: inspStatus, //getInspectionStatus(),
            IsOpen: isOpen,
            Questionnaires: questionnaires,
            RoundCatId: roundCatId
        };
        if (currentInsp != null) {
            $.ajax({
                url: "/Round/SaveRoundInspection",
                type: "POST",
                data: { inspection: currentInsp },
                dataType: 'json',
                global: false,
                success: function (result) {
                    $(".fileContent").val("");
                }
            });
        }
    }
}

function deleteRoundImg(file, controlId, fileType) {
    //debugger;
   // console.log(file);
    swal({
        title: "Are you sure?",
        text: "You will not be able to recover this file!",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, delete it!",
        closeOnConfirm: true
    },
        function () {
            var thisfile = $(file);
            var control = $("#fileuploadCtr_" + controlId);
            var fileControl = $(control).closest("tr").find(".fileName").val();
            var filepath = $(control).closest("tr").find(".filePath").val();
            DeleteImageData(fileControl, filepath).then(data => {
                if (data.result) {
                    $(control).closest("tr").find(".fileName").val("");
                    $(control).closest("tr").find(".fileContent").val("");
                    $(control).closest("tr").find(".filePath").val("");
                    $("#imagePreview_" + controlId).addClass("img_prev");
                    $("#imagePreview_" + controlId).removeClass("img_prev_upload");
                    $("#deleteimg_" + controlId).addClass("hide");
                    $("#deleteimg_" + controlId).removeClass("deleteroundimg");
                    saveData("#imagePreview_" + controlId);
                    //debugger;
                    $("#img_file_upload_" + controlId).removeClass('hide').addClass("show");
                    $(".part1").hide();
                    $(".part2").show();
                }

            });
        });
}

async function DeleteImageData(fileName, filePath) {
    if (canUpdate()) {
        return new Promise(function (resolve, reject) {
            if (fileName != null && filePath != null) {
                $.ajax({
                    url: "/Round/DeleteRoundInspectionImage",
                    type: "POST",
                    data: { fileName: fileName, filePath: filePath },
                    dataType: 'json',
                    global: false,
                    success: function (result) {
                        $(".fileContent").val("");
                        resolve(result);
                    }, error: function (error) {
                        reject(error);
                    }
                });
            }
        });
    }
}

var canUpdate = function () {
    // debugger;
    var isOpen = $("#hdn_isOpen").val();
    console.log(isOpen);
    if (isOpen == "True")
        return true;
    else
        return false;
}

$(document).off('click', '.imgView').on('click', '.imgView', function () {
    /*$(".imgView").click(function () {*/
    //alert("Called!");
    var src = $(this).find("img").attr("src");
    $("#previewImage").attr("src", src);
    $("#myModal").modal('show');
});
