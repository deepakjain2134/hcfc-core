function loadSignViewNew(fileNameCtr, fileContentCtr, imgPreview = "", signctrlId) {
    $('#signAndSave').empty();
    var url = CRxUrls.common_AddDigitalSignature;
    $.ajax({
        url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr + "&imgPreviewClass=" + imgPreview + "&signctrlId=" + signctrlId,
        cache: false,
        type: "GET",
        success: function (data) {
            $('#signAndSave').append(data);
            $('#signAndSave').fadeIn('fast');
        }
    });
}

$(document).on("click", ".showSignPopUp", function () {
    debugger;
    var signclass = $(this).attr('signclass');
    var signctrlId = $(this).attr('signctrlid');
    var FileNameControl = signclass + ".FileName";
    var FileContent = signclass + ".FileContent";
    if (window.location.href.indexOf("asset-device-change") > -1 || window.location.href.indexOf("ASSET-DEVICE-CHANGE") > -1) {
        if ($("#Path").val() == "") {
            swalalert("Please select path before signing");
            return false
        }
    }
    if (window.location.href.indexOf("AddCRA") > -1 || window.location.href.indexOf("addtcra") > -1 || window.location.href.indexOf("AddPCRA") > -1 || window.location.href.indexOf("addtpcra") > -1 || window.location.href.indexOf("inspection/icra/add") > -1 || window.location.href.indexOf("INSPECTION/ICRA/ADD") > -1 || window.location.href.indexOf("icra/addinspectionicra") > -1 || window.location.href.indexOf("ICRA/ADDINSPECTIONICRA") > -1 || window.location.href.indexOf("CEILING/PERMIT") > -1 || window.location.href.indexOf("ceiling/permit") > -1 || window.location.href.indexOf("asset-device-change") > -1 || window.location.href.indexOf("ASSET-DEVICE-CHANGE") > -1) {
        var hiddenctrl = "";
        var sequence = $(this).attr("sequence");
        var signindex = $(this).attr("signindex");

        if ($(this).attr("signname") == "contractorsign") {

            hiddenctrl = "hdn_DSPermitSignature_" + sequence + "_" + signindex;
        }
        var requiredsigndone = false;
        var finalsign = true;
        var approvalalert = false;
        var issigneddone = false;
        var donesigncount = 0;
        var requireddonecount = 0;
        //Case for required
        var pathid = $(this).parent().closest('.pathdiv').attr("PathId");
        var maxid = "hdnMaxSequence";
        if (pathid > 0) {
            maxid = "hdnMaxSequence_" + pathid;
        }
        if ($(this).attr("sequence") == $("#" + maxid).val() && ($("#" + hiddenctrl).val() == "" || $("#" + hiddenctrl).val() == "0") && $("#" + hiddenctrl).attr('isrequired') == "isRequired") {
            issigneddone = true;
            // case if required and any of sign is not done
            if (pathid > 0) {
                $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][pathid='" + pathid + "']").each(function (index) {
                    //check required count from same sequence
                    if ($(this).attr('isrequired') == "isRequired") {
                        if ($(this).find(".hdnRequired").val() != "") {
                            //issigneddone = true;
                            requireddonecount++;
                            //issigneddone = true;
                        }
                        else {
                            //issigneddone = false;
                        }
                    }

                    //check if any sign is required the done signed count
                    if (issigneddone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            issigneddone = true;
                            donesigncount++;
                            //requiredsigndone = true;

                        }
                        else {
                            /* issigneddone = false;*/


                        }
                    }



                    if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                        if ($(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][isrequired='isRequired']").length - 1 == requireddonecount) {
                            requiredsigndone = false;

                        }
                        else {
                            requiredsigndone = true;
                            if (!issigneddone) {
                                if ($(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1 == donesigncount) {
                                    requiredsigndone = true;

                                }

                            }
                        }


                    }
                });
            }
            else {
                $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").each(function (index) {
                    //check required count from same sequence
                    if ($(this).attr('isrequired') == "isRequired") {
                        if ($(this).find(".hdnRequired").val() != "") {
                            //issigneddone = true;
                            requireddonecount++;
                            //issigneddone = true;
                        }
                        else {
                            //issigneddone = false;
                        }
                    }

                    //check if any sign is required the done signed count
                    if (issigneddone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            issigneddone = true;
                            donesigncount++;
                            //requiredsigndone = true;

                        }
                        else {
                            /* issigneddone = false;*/


                        }
                    }



                    if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                        if ($(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][isrequired='isRequired']").length - 1 == requireddonecount) {
                            requiredsigndone = false;

                        }
                        else {
                            requiredsigndone = true;
                            if (!issigneddone) {
                                if ($(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1 == donesigncount) {
                                    requiredsigndone = true;

                                }

                            }
                        }


                    }
                });
            }

            if (requiredsigndone) {
                approvalalert = false;
            }
            else {
                approvalalert = true;
            }
        }

        var pathid = $(this).parent().closest('.pathdiv').attr("PathId");
        var maxid = "hdnMaxSequence";
        if (pathid > 0) {
            maxid = "hdnMaxSequence_" + pathid;
        }

        if ($(this).attr("sequence") == $("#" + maxid).val() && ($("#" + hiddenctrl).val() == "" || $("#" + hiddenctrl).val() == "0") && $("#" + hiddenctrl).attr('isrequired') != "isRequired") {
            issigneddone = true;
            approvalalert = false;
            donesigncount = 0;
            if (pathid > 0) {
                $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][pathid='" + pathid + "']").each(function (index) {
                    if (issigneddone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            issigneddone = true;
                            donesigncount++;

                        }
                    }
                    if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][pathid='" + pathid + "']").length - 1) {

                        if (donesigncount == 0) {
                            approvalalert = true;

                        }


                    }

                });
            }
            else {
                $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").each(function (index) {
                    if (issigneddone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            issigneddone = true;
                            donesigncount++;

                        }
                    }
                    if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                        if (donesigncount == 0) {
                            approvalalert = true;

                        }


                    }

                });
            }

            if (approvalalert) {
                var anyrequired = false;
                if (pathid > 0) {
                    $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "'][pathid='" + pathid + "']").each(function (index) {
                        if (issigneddone) {
                            if ($(this).attr('isrequired') == "isRequired")
                                anyrequired = true;
                        }
                        if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                            if (anyrequired) {
                                approvalalert = false;

                            }


                        }

                    });
                }
                else {
                    $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").each(function (index) {
                        if (issigneddone) {
                            if ($(this).attr('isrequired') == "isRequired")
                                anyrequired = true;
                        }
                        if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                            if (anyrequired) {
                                approvalalert = false;

                            }


                        }

                    });
                }
            }

        }
        if (approvalalert) {
            swal({
                title: "Status will change to Approved upon your signature",
                text: "Do you want to sign for final Approval? ",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: '#DD6B55',
                confirmButtonText: 'Yes',
                cancelButtonText: "No",
                closeOnConfirm: true,
                closeOnCancel: true
            },
                function (isConfirm) {

                    if (isConfirm) {

                        $(".hdnapprovalstatus").val(1);
                        loadSignViewNew(FileNameControl, FileContent, '', signctrlId);
                        $('#signAndSave').modal('show');
                        var lbltext = "Reason(s):";
                        lbltext += '<em class="required-field approverby">*</em>';
                        $("#lbltext").html(lbltext);
                        $("#TIcraLog_ReasonRejection").prop('required', false);
                        $("#TIcraLog_ReasonRejection").prev("label").find("em").hide();
                        checkAppprovalStatus();
                    }
                    else {

                    }
                });

        }
        else {
            loadSignViewNew(FileNameControl, FileContent, '', signctrlId);
            $('#signAndSave').modal('show');
        }
    }
    else {
        loadSignViewNew(FileNameControl, FileContent, '', signctrlId);
        $('#signAndSave').modal('show');
    }
});
$(document).on('click', '.deleteSignaturepermit', function (e) {
    debugger
    var digSignatureId = $(this).data("id");
    var fileNameCtr = $(this).data("signclass");
    var signctrlid = $(this).data("signctrlid");

    swal({
        title: "Are you sure?",
        text: "You want to remove this Signature",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Yes, remove it",
        closeOnConfirm: true
    },
        function (isConfirm) {
            if (isConfirm) {
                $.ajax({

                    url: CRxUrls.Common_DeleteDigitalSign,
                    type: "POST",
                    async: true,
                    data: { digSignatureId: digSignatureId, fileNameCtr: fileNameCtr, __RequestVerificationToken: $('input[name=' + $.Constants.RequestVerificationToken + ']').val() },
                    success: function (data) {
                        debugger
                        // swalalert(result.Messgae);
                        swal("Deleted!", "Your Signature has been removed.", "success");
                        debugger
                        $(".sign_" + fileNameCtr).html(data);
                        console.log(data);
                        $("#hdn_" + fileNameCtr).val('');
                        $("#hdn" + fileNameCtr).val('');
                        $("#hdn_" + fileNameCtr + "_LocalSignDateTime").val('');
                        $("#hdn_" + fileNameCtr + "_CreatedBy").val('');
                        console.log(fileNameCtr);
                        $("input[id='" + fileNameCtr + ".FileName'][type=hidden]").val('');
                        $("input[id='" + fileNameCtr + ".FileContent'][type=hidden]").val('');
                        $("input[id='" + fileNameCtr + ".DigSignatureId'][type=hidden]").val('0');

                        //$("input[id='" + fileContent + "'][type=hidden]").val('');
                        $("input[id='" + signctrlid + "'][type=hidden]").val('');
                        console.log(signctrlid);
                        console.log(fileNameCtr);
                        var splitval = fileNameCtr.replace("DSPermitSignature", "");
                        if (splitval != null && splitval != '') {
                            var requiredctrlval = $("#hdn_DSPermitSignature" + splitval).attr("isrequiredctl");
                            if (requiredctrlval != null && requiredctrlval == "1") {
                                $("#hdn_DSPermitSignature" + splitval).prop("required", true);
                            }
                            $("#selectSignBy" + splitval).val("");
                            $("#txtSignatureDate" + splitval).val("");
                        }

                    }
                });
            }


        });
});