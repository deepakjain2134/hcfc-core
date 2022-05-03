$(document).ready(function () {

    $("select.hdnapprovalstatus").attr("onchange", "checkAppprovalStatus()")
});

function setControlAfterSign(sequence) {
    var ctrl = "hdn_DSPermitSignature_" + sequence;
    if (typeof $("#" + ctrl).val() !== 'undefined' && $("#" + ctrl).val() != "") {
            $("#selectSignBy_" + sequence).val($("#hdnCurrentUserId").val());

        if ($("#txtSignatureDate_" + sequence).val() == "") {
            $("#txtSignatureDate_" + sequence).datepicker().datepicker("setDate", new Date());
        }
        var divid = "divSequence_" + sequence;
        var split = sequence.split("_");
       
        if (split.length > 0) {
            //if ($("#txtWorkFlowComments_" + split[0] + "_" + split[1]).val() == "") {
            //    $("#txtWorkFlowComments_" + split[0] + "_" + split[1]).attr("required", true);
            //}

            setControl(split[0], split[1], true, divid);

            if ($("#hdn_DSPermitSignature_" + $("#" + divid).attr("sequence") + "_" + $("#" + divid).attr("signindex")).val != "") {
                var DoneSignindex = $("#" + divid).attr("signindex");
                var DoneSequence = $("#" + divid).attr("sequence");
                var PathFollowed = $("#" + divid).attr("PathId");
                var NewSignSequence = $("#" + divid).next(".divpermitworkflow").attr("sequence");
                var IsRequiredCount = true;
                if (NewSignSequence == DoneSequence) {

                    $(".divpermitworkflow[sequence='" + NewSignSequence + "']").each(function (index) {

                        if ($(this).attr("isrequired") == "isRequired") {
                            IsRequiredCount = true;
                        }
                        else {
                            IsRequiredCount = false;
                        }

                    });


                    if (!IsRequiredCount) {
                        NewSignSequence = $(".divpermitworkflow[sequence='" + NewSignSequence + "']").last().next(".divpermitworkflow").attr("sequence");
                    }
                }
                if ($("#hdnIsVendorUser").val() != 'True') {
                    if (PathFollowed > 0) {
                        SetWorkFlowMatrixAfterSignforPath(NewSignSequence, DoneSequence, PathFollowed);
                    }
                    else {
                        SetWorkFlowMatrixAfterSign(NewSignSequence, DoneSequence);
                    }
                   
                }
            }

        }
        $("#Path").addClass("disabled");
    }

    
}
function setWorkFlowControl(ispageloadtime) {
    debugger;

    $(".divpermitworkflow").each(function (index) {
        var divid = "divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex");
        var pathid = $(this).attr("pathid");
        var currentsignid = "hdnCurrentSignSequence";
        if (pathid > 0) {
            currentsignid = "hdnCurrentSignSequence_" + pathid;
        }

      
        var minid = "hdnMinSequence";
        if (pathid > 0) {
            minid = "hdnMinSequence_" + pathid;
        }

        if ($(this).attr("sequence") == $("#" + currentsignid).val()) {
            debugger;
            setControl($(this).attr("sequence"), $(this).attr("signindex"), true, divid);
            if ($("#selectSignBy_" + $("#" + currentsignid).val() + "_" + $(this).attr("signindex")).val() == "" && $("#hdnIsVendorUser").val() != 'True') {
                
                $("#selectSignBy_" + $("#" + currentsignid).val()+ "_" + $(this).attr("signindex")).val($("#hdnCurrentUserId").val());
                
            }

            if ($("#selectSignBy_" + $("#" + currentsignid).val() + "_" + $(this).attr("signindex")).val() == "" && $("#hdnIsVendorUser").val() == 'True' && minid == $("#" + currentsignid).val()) {
                $("#selectSignBy_" + $("#" + currentsignid).val() + "_" + $(this).attr("signindex")).val($("#hdnCurrentUserId").val());
            }

            if ($("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).val() != "" && ispageloadtime==1) {
                //$("#" + divid).addClass("disable");
                //  $("#" + div).find("#selectSignBy_" + $("#" + div).attr("sequence") + "_" + signindex).attr("required");
                $("#" + divid).find("div").addClass("disable");
                $("#" + divid).find(".btnsigndiv").removeClass("disable");
            }
           
        }
        else {
            setControl($(this).attr("sequence"), $(this).attr("signindex"), false, divid);
        }
    });

    if (window.location.href.indexOf("inspection/icra/add") > -1 || window.location.href.indexOf("INSPECTION/ICRA/ADD") > -1 || window.location.href.indexOf("icra/addinspectionicra") > -1 || window.location.href.indexOf("ICRA/ADDINSPECTIONICRA") > -1 || window.location.href.indexOf("asset-device-change") > -1 || window.location.href.indexOf("ASSET-DEVICE-CHANGE") > -1) {
        if ($("#hdnFormId").val() > 0 && ($("#hdnStatus").val() <= 1 && $("#hdnStatus").val() != -1)) {
            $(".divWorkFlow").addClass("disable");
        }
        else {
            $(".divWorkFlow").removeClass("disable");
        }
    }
    else {
       
        
            if ($("#hdnFormId").val() > 0 && ($("#hdnStatus").val() <= 1 && $("#hdnStatus").val() != -1)) {
                $("#divWorkFlow").addClass("disable");
            }
            else {
                $("#divWorkFlow").removeClass("disable");
            }
    }
    if ($("#hdnFormId").val() > 0 && $("#hdnStatus").val() == 0) {
        $("#divWorkFlow").removeClass("disable");
    }
    //if ($("#hdnFormId").val() > 0 && ($("#hdnStatus").val() <= 1 && $("#hdnStatus").val() != -1)) {
    //    $("#divWorkFlow").addClass("disable");
    //}
    //else {
    //    $("#divWorkFlow").removeClass("disable");
    //}
}
function SetWorkFlowMatrixAfterSign(NewSequence, DoneSequence) {
   
        var SignedDone = false;


        var IsRequiredCount = false;
        $(".divpermitworkflow[sequence='" + DoneSequence + "']").each(function (index) {
            if (!IsRequiredCount) {
                if ($(this).attr("isrequired") == "isRequired") {
                    IsRequiredCount = true;
                }
                else {
                   IsRequiredCount = false;
                }
            }
        });

        if (IsRequiredCount) {
            SignedDone = true;
        }
        else {
            SignedDone = false;
        }
        $(".divpermitworkflow[sequence='" + DoneSequence + "']").each(function (index) {

            if (IsRequiredCount) {

                if (SignedDone) {
                    if ($(this).find(".hdnRequired").val() != "") {
                        SignedDone = true;
                    }
                    else {
                        SignedDone = false;
                    }
                }
            }
            else {
                if (!SignedDone) {

                    if ($(this).find(".hdnRequired").val() != "") {
                        SignedDone = true;
                    }
                }
            }

        });

    if (IsRequiredCount && !SignedDone) {
        $(".divpermitworkflow[sequence='" + DoneSequence + "']").each(function (index) {
           // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
            $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
            $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
            if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                
               // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
               // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                //$("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
            }
        });
    }
    // Case 1:if all are required and check if sign done..move to next
    else if (IsRequiredCount && SignedDone) {
        if (NewSequence != undefined) {
            SignedDone = false;
            IsRequiredCount = true;
            $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {
                if (IsRequiredCount) {
                    if ($(this).attr("isrequired") == "isRequired") {
                        IsRequiredCount = true;
                    }
                    else {
                        IsRequiredCount = false;
                    }
                }
            });

            if (IsRequiredCount) {
                SignedDone = true;
            }
            else {
                SignedDone = false;
            }
            $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {

                if (IsRequiredCount) {

                    if (SignedDone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            SignedDone = true;
                        }
                        else {
                            SignedDone = false;
                        }
                    }
                }
                else {
                    if (!SignedDone) {

                        if ($(this).find(".hdnRequired").val() != "") {
                            SignedDone = true;
                        }
                    }
                }
                debugger;
            });

            if (SignedDone) {

                $(".divpermitworkflow[sequence='" + NewSequence + "']").last().each(function (index) {

                    DoneSequence = $(this).attr("sequence");
                    NewSequence = $(this).next().attr("sequence");
                    $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {
                        //$("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                            //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        }
                    });
                });

            }
            else {
                $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {
                    $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {
                       // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                            //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        }
                    });
                });
            }
        }
    }
    else if (!IsRequiredCount && SignedDone) {

        if (NewSequence != undefined) {
            $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {

                DoneSequence = $(this).attr("sequence");
                NewSequence = $(this).attr("sequence");
                $(".divpermitworkflow[sequence='" + NewSequence + "']").each(function (index) {

                    if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                        //$("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        //$("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                        $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                        $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                       // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                    }
                });
            });
        }
    }


    


}

function SetWorkFlowMatrixAfterSignforPath(NewSequence, DoneSequence, PathId) {

    var SignedDone = false;


    var IsRequiredCount = false;
    $(".divpermitworkflow[sequence='" + DoneSequence + "'][PathId='" + PathId+"']").each(function (index) {
        if (!IsRequiredCount) {
            if ($(this).attr("isrequired") == "isRequired") {
                IsRequiredCount = true;
            }
            else {
                IsRequiredCount = false;
            }
        }
    });

    if (IsRequiredCount) {
        SignedDone = true;
    }
    else {
        SignedDone = false;
    }
    $(".divpermitworkflow[sequence='" + DoneSequence + "'][PathId='" + PathId +"']").each(function (index) {

        if (IsRequiredCount) {

            if (SignedDone) {
                if ($(this).find(".hdnRequired").val() != "") {
                    SignedDone = true;
                }
                else {
                    SignedDone = false;
                }
            }
        }
        else {
            if (!SignedDone) {

                if ($(this).find(".hdnRequired").val() != "") {
                    SignedDone = true;
                }
            }
        }

    });

    if (IsRequiredCount && !SignedDone) {
        $(".divpermitworkflow[sequence='" + DoneSequence + "'][PathId='" + PathId +"']").each(function (index) {
            // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
            $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
            $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
            if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {

                // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                //$("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
               // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
            }
        });
    }
    // Case 1:if all are required and check if sign done..move to next
    else if (IsRequiredCount && SignedDone) {
        if (NewSequence != undefined) {
            SignedDone = false;
            IsRequiredCount = true;
            $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {
                if (IsRequiredCount) {
                    if ($(this).attr("isrequired") == "isRequired") {
                        IsRequiredCount = true;
                    }
                    else {
                        IsRequiredCount = false;
                    }
                }
            });

            if (IsRequiredCount) {
                SignedDone = true;
            }
            else {
                SignedDone = false;
            }
            $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {

                if (IsRequiredCount) {

                    if (SignedDone) {
                        if ($(this).find(".hdnRequired").val() != "") {
                            SignedDone = true;
                        }
                        else {
                            SignedDone = false;
                        }
                    }
                }
                else {
                    if (!SignedDone) {

                        if ($(this).find(".hdnRequired").val() != "") {
                            SignedDone = true;
                        }
                    }
                }
                debugger;
            });

            if (SignedDone) {

                $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").last().each(function (index) {

                    DoneSequence = $(this).attr("sequence");
                    NewSequence = $(this).next().attr("sequence");
                    $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {
                        //$("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                            //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        }
                    });
                });

            }
            else {
                $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {
                    $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {
                        // $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                            //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        }
                    });
                });
            }
        }
    }
    else if (!IsRequiredCount && SignedDone) {

        if (NewSequence != undefined) {
            $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {

                DoneSequence = $(this).attr("sequence");
                NewSequence = $(this).attr("sequence");
                $(".divpermitworkflow[sequence='" + NewSequence + "'][PathId='" + PathId +"']").each(function (index) {

                    if ($(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                        //$("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).removeClass("disable");
                        //$("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find("div").removeClass("disable");
                        $("#divSequence_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).find(".btnsigndiv").removeClass("disable");
                        $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        //$("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
                        $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                        $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                        $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                       // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
                    }
                });
            });
        }
    }





}
function SetUpVendorSignatureControl() {
    debugger;
    $(".divpermitworkflow").each(function (index) {

        if (index != 0 || $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).val() != "") {
           // $(this).addClass("disable");

            $(this).find("div").addClass("disable");
            $(this).find(".btnsigndiv").removeClass("disable");
            if (window.location.href.indexOf("addtpcra") > -1 || window.location.href.indexOf("ADDTPCRA") > -1) {
                if (index == 0) {
                    $(this).find("div").removeClass("disable");

                    $(this).find(".div-forworkflow").addClass("disable");
                    $(this).find("div[class*='sign_DSPermitSignature']").removeClass("disable");
                }



            }
            $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
            $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
           // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
            $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
            $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', false);
            $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
            $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
            $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
            //$("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").hide();
        }

        if (index == 0) {
            $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
            $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
           // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
            $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
            $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', true);
            $("#selectSignBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").show();
            $("#txtSignatureDate_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").show();
            $("#txtSignatureBy_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").show();
           // $("#txtWorkFlowComments_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prev("label").find("em").show();
        }
    });
}

function setControl(ctrlseq, signindex, value, div) {
    debugger;


    if (value) {
       //$("#" + div).removeClass("disable");
        $("#" + div).find("div").removeClass("disable");
        $("#" + div).find(".btnsigndiv").removeClass("disable");
       
    }
    else {
       // $("#" + div).addClass("disable");
        $("#" + div).find("div").addClass("disable");
        $("#" + div).find(".btnsigndiv").removeClass("disable");

    }
    //check for same sequence level
    if ($("#" + div).attr("isRequired")) {
        $(".divpermitworkflow").each(function (index) {
            debugger;

            if ($(this).attr("sequence") == ctrlseq && $(this).attr("isRequired") && $("#hdnFormId").val() > 0) {
                $("#hdn_DSPermitSignature_" + $(this).attr("sequence") + "_" + $(this).attr("signindex")).prop('required', value);
             //   $("#txtSignatureDate_" + ctrlseq + "_" + signindex).prop('required', value);
             //   $("#txtWorkFlowComments_" + ctrlseq + "_" + signindex).prop('required', value);
                if (value)
                    $("#txtSignatureDate_" + ctrlseq + "_" + signindex).prev("label").find("em").show();
                else
                    $("#txtSignatureDate_" + ctrlseq + "_" + signindex).prev("label").find("em").hide();

                $("#txtSignatureBy_" + ctrlseq + "_" + signindex).prop('required', value);
                if (value)
                    $("#selectSignBy_" + ctrlseq + "_" + signindex).prev("label").find("em").show();
                else
                    $("#selectSignBy_" + ctrlseq + "_" + signindex).prev("label").find("em").hide();

                //if (value)
                //    $("#txtWorkFlowComments_" + ctrlseq + "_" + signindex).prev("label").find("em").show();
                //else
                //    $("#txtWorkFlowComments_" + ctrlseq + "_" + signindex).prev("label").find("em").hide();

                $("#hdn_DSPermitSignature_" + ctrlseq + "_" + signindex).prop('required', value);
                if (value) {

                    $("#" + div).find("#selectSignBy_" + $("#" + div).attr("sequence") + "_" + signindex).attr("required");
                }
                else {

                    $("#" + div).find("#selectSignBy_" + $("#" + div).attr("sequence") + "_" + signindex).removeAttr("required");
                }
            }


        });
    }



}

$(".checksign").click(function () {

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
    var pathid = $(this).attr("pathid");
    var maxid = "hdnMaxSequence";
    if (pathid > 0) {
        maxid = "hdnMaxSequence" + pathid;
    }
    //Case for required
    if ($(this).attr("sequence") == maxid && ($("#" + hiddenctrl).val() == "" || $("#" + hiddenctrl).val() == "0") && $("#" + hiddenctrl).attr('isrequired') == "isRequired") {
        issigneddone = true;
        // case if required and any of sign is not done
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

        if (requiredsigndone) {
            approvalalert = false;
        }
        else {
            approvalalert = true;
        }
    }

    var pathid = $(this).attr("pathid");
    var maxid = "hdnMaxSequence";
    if (pathid > 0) {
        maxid = "hdnMaxSequence" + pathid;
    }

    if ($(this).attr("sequence") == maxid && ($("#" + hiddenctrl).val() == "" || $("#" + hiddenctrl).val() == "0") && $("#" + hiddenctrl).attr('isrequired') != "isRequired") {
        issigneddone = true;
        approvalalert = false;
        donesigncount = 0;
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
        if (approvalalert) {
            var anyrequired = false;
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
                    signPermit(sequence, signindex);
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
        signPermit(sequence, signindex);
    }
});

function signPermit(sequence, signindex) {


    loadSignView('DSPermitSignature_' + sequence + '_' + signindex + '.FileName', 'DSPermitSignature_' + sequence + '_' + signindex + '.FileContent');
    $('#signAndSave').modal('show');

}
function setapprovalrequiredfield() {

    debugger;
    var StatusId = $(".hdnapprovalstatus").val();
    var isfinalsigned = true;
    var requireddonecount = 0;

    var pathid = $("#Path").val();
    var maxid = "hdnMaxSequence";
    if (pathid > 0) {
        maxid = "hdnMaxSequence_" + pathid;
    }
    if (StatusId == 1) {
        isfinalsigned = false;
        if (pathid != null && pathid!="") {
            if ($(".divpermitworkflow[pathid='" + pathid + "'][sequence = '" + $("#" + maxid).val() + "']").attr("isrequired") != undefined && $(".divpermitworkflow[sequence = '" + $("#" + maxid).val() + "']").attr("isrequired").length > 0) {
                $(".divpermitworkflow[pathid='" + pathid + "'][sequence='" + $("#" + maxid).val() + "']").each(function (index) {
                    if ($(this).attr("isrequired") != "" && $(this).attr("isrequired") != undefined && $(this).find(".hdnRequired").val() != "") {
                        requireddonecount = requireddonecount + 1;
                    }
                    if (index == $(".divpermitworkflow[pathid='" + pathid + "'][sequence='" + $(this).attr("sequence") + "']").length - 1) {

                        if ($(".divpermitworkflow[pathid='" + pathid + "'][sequence='" + $("#" + maxid).val() + "'][isrequired='isRequired']").length == requireddonecount) {
                            isfinalsigned = true;

                        }


                    }
                });
            }
            else {
                $(".divpermitworkflow[pathid='" + pathid + "'][sequence='" + $("#hdnMaxSequence").val() + "']").each(function (index) {
                    if ($(this).find(".hdnRequired").val() != "") {
                        isfinalsigned = true;
                    }

                });
            }
        }
        else {
            if ($(".divpermitworkflow[sequence='" + $("#hdnMaxSequence").val() + "']").attr("isrequired") != undefined && $(".divpermitworkflow[sequence='" + $("#hdnMaxSequence").val() + "']").attr("isrequired").length > 0) {
                $(".divpermitworkflow[sequence='" + $("#hdnMaxSequence").val() + "']").each(function (index) {
                    if ($(this).attr("isrequired") != "" && $(this).attr("isrequired") != undefined && $(this).find(".hdnRequired").val() != "") {
                        requireddonecount = requireddonecount + 1;
                    }
                    if (index == $(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").length - 1) {

                        if ($(".divpermitworkflow[sequence='" + $("#hdnMaxSequence").val() + "'][isrequired='isRequired']").length == requireddonecount) {
                            isfinalsigned = true;

                        }


                    }
                });
            }
            else {
                $(".divpermitworkflow[sequence='" + $("#hdnMaxSequence").val() + "']").each(function (index) {
                    if ($(this).find(".hdnRequired").val() != "") {
                        isfinalsigned = true;
                    }

                });
            }

        }


        if (!isfinalsigned) {
            swal("Please do the final signature to complete the process");
            return false;
        }
        else {
            return true;
        }
    }
    else {
        return true;
    }
}

function setCurrentWorkFlowUser() {

    //$(".divpermitworkflow[sequence='" + $(this).attr("sequence") + "']").each(function (index) {
        
    //});

}
