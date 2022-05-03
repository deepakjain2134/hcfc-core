var TicraId = 0;
var form_original_data = $("#ICRAform").serialize();

$('#ICRAform').on('change paste', ':input', function (e) {
    demoNoticeDefault('Save changes - Not yet saved.');
});

$('#ICRAform').on('keyup', ':input', function (e) {
    if ($(this).val() != $(this).data('val')) {
        demoNoticeDefault('Save changes - Not yet saved.');
        $(this).data('val', $(this).val());
    }
});

function demoNoticeDefault(msg) {
    $("#lblmsg").html(msg);
}
/*
window.onscroll = function () { myFunction() };
var header = document.getElementById("myHeader");
var sticky = header.offsetTop;


 function myFunction() {
    debugger;
    if (window.pageYOffset > sticky) {
        header.classList.add("sticky");
    } else if(window.pageYOffset ==0) {
        header.classList.remove("sticky");
    }else
    {
        header.classList.remove("sticky");
    }
} 
*/
function SetActiveClass() {
    var typeId = parseInt($("#ConstructionTypeId").val());
    var riskId = parseInt($("#ConstructionRiskId").val());
    // debugger;
    var matId = ('id{0}_{1}'.f(typeId, riskId));
    // console.log(matId);
    $(".constructiontype").removeClass('ctypeActive');
    $(".constructiontype[rel='" + typeId + "']").addClass('ctypeActive');

    // set matrixview active Class
    $(".constructiontypeMatrix").removeClass('ctypeActive');
    $(".constructiontypeMatrix[rel='" + typeId + "']").addClass('ctypeActive');


    $(".constrisk").removeClass('crActive');
    $(".constrisk[value='" + riskId + "']").addClass('crActive');

    $(".open-AddMatrix").removeClass('matrixActive');
    $(".open-AddMatrix[matrixid='" + matId + "']").addClass('matrixActive');

    $(".constClasschk").parent('td').addClass("disable");
    $(".constClasschk").attr("disabled", "disabled");
    $(".constClasschk").prop("checked", false);
    $(".constClasschk").removeClass("ccActive");
    $(".constClasschk").parent("td").parent("tr").removeClass("ccActive");
    $("#hdn_ConstructionClassName").val("NA");
    $("#ConstructionClassId").val("0");

    var matix = JSON.parse($("#icramatrix").val());
    var activeClass = matix.data.filter(function (entry) {
        return entry.ConstructionRiskId === riskId && entry.ConstructionTypeId === typeId;
    });
    
    $(".constclasstr").find("input.form-control").addClass("disabled");    

    $.each(activeClass, function (key, value) {
        $(".constClasschk[rel='" + value.ConstructionClassId + "']").parent('td').removeClass('disable');
        $(".constClasschk[rel='" + value.ConstructionClassId + "']").removeAttr("disabled");
    });

    // permit class -> construction Type
    $(".permitConsTypetr").removeClass("ctypeActive");
    $("span.permitConsType").text("NO");
    $("span.permitConsType[typeid='" + typeId + "']").text("YES");
    $("span.permitConsType[typeid='" + typeId + "']").closest("tr").addClass("ctypeActive");

    // permit class -> construction Risk
    $(".permitConsRisktr").removeClass("crActive");
    $("span.permitConsRisk").text("NO");
    $("span.permitConsRisk[riskid='" + riskId + "']").text("YES");
    $("span.permitConsRisk[riskid='" + riskId + "']").closest("tr").addClass("crActive");
    if ($(".tdconstclass").not(".disable").length == 1) {
        $(".tdconstclass").not(".disable").find(".constClasschk").prop('checked', true);
        $(".tdconstclass").not(".disable").find(".constClasschk").attr('checked', true);
        $(".ccActive").removeClass("ccActive");
        $(".tdconstclass").not(".disable").parent().addClass("ccActive");
    }
   
}

var showClassNames = function () {
    //if ($(".open-AddMatrix").hasClass("matrixActive")) {
    //    var classNames = $(".matrixActive").text();
    //    infoAlert("based on selection your Construction Class " + classNames + " is selected  ", "");
    //}
};

function inspectionIcraPageLoad() {
    var Model = JSON.parse($("#inspectionicra").val()).data;
    console.log(Model);
    TicraId = Model.TicraId;
    console.log(TicraId);
    var $radios = $('input:radio[name=constType]');
    $radios.filter("[value='" + Model.ConstructionTypeId + "']").prop('checked', true);
    $("#ConstructionTypeId").val(Model.ConstructionTypeId);


    $("input[name='risk_" + Model.ConstructionRiskId + "'][rel='1']").prop("checked", true);
    $("#ConstructionRiskId").val(Model.ConstructionRiskId);



    $(".constructiontype[rel='" + Model.ConstructionTypeId + "']").prop("checked", true).closest('tr').addClass('ctypeActive');
    $(".riskgroup[rel='" + Model.ConstructionRiskId + "']").addClass('crActive');


    if (Model.ConstructionRisk != null) {
        $("#hdn_constructionRiskName").val(Model.ConstructionRisk.RiskName);
    }
    //debugger;
    $('#RiskAreaId').val(Model.RiskAreaId);
    var str = Model.RiskAreaId;
    var res = "";
    if (str != undefined) {
        res = str.split(",");
    }

    SetActiveClass();    
    if (Model.ConstructionClass != null) {
        $("#hdn_ConstructionClassName").val(Model.ConstructionClass.ClassName);
    }

    $(".constClasschk[rel='" + Model.ConstructionClassId + "']").prop("checked", "checked");
    $("#ConstructionClassId").val(Model.ConstructionClassId);
    $('.constClasschk:checked').closest('tr').addClass('ccActive');
    $('.constClasschk:checked').closest('tr').find("input.form-control").removeClass("disabled");
   

    if (res != "") {
        for (var i = 0; i < res.length; i++) {
            $(".constrisk[rel='" + res[i] + "']").prop("checked", true);
        }
    }   
}

$(document).ready(function () {    

    $("#icraInBoxview").hide();

    $('#ICRAform').find(':input').each(function (index, value) {
        $(this).data('val', $(this).val());
    });

    inspectionIcraPageLoad();
    $("#PermitNo").addClass("disable");
    $(".constructiontype").change(function () {
        $('.constructiontype').removeAttr('checked');
        $('.ctype').removeClass('ctypeActive');
        $(this).prop('checked', true);
        var consttypeId = $(this).val();
        $('#ConstructionTypeId').val(consttypeId);
        $(this).closest('tr').addClass('ctypeActive');
        SetActiveClass();
        settextresult();
       
        showClassNames();
        
       
    });

    //var showClassSelection = function () {
    //    var constructionTypeId = $('#ConstructionTypeId').val();
    //    var riskAreaId = $('#RiskAreaId').val();
    //}

    $(".constrisk").change(function () {
        var riskarealist = [];
        $('input.constrisk[type=checkbox]:checked').each(function () {
            var RiskAreaId1 = $(this).attr('rel');
            var riskName1 = $(this).attr('riskName');
            var ConstructionRiskId1 = $(this).val();
            riskarealist.push({ 'constructionRiskId': ConstructionRiskId1, 'riskAreaId': RiskAreaId1, RiskName: riskName1 });
        });

        var result = riskarealist.map(function (elem) {
            return elem.riskAreaId;
        }).join(",");


        $("#RiskAreaId").val(result);
        $(".riskgroup").removeClass('crActive');
        var highestvalue = Math.max.apply(Math, riskarealist.map(function (o) { return o.constructionRiskId; }));

        var higestRiskName = riskarealist.filter(function (elem) {
            return elem.constructionRiskId == highestvalue;
        });

        if (higestRiskName.length > 0) {
            $("#hdn_constructionRiskName").val(higestRiskName[0].RiskName);
        }
        $(".riskgroup[rel='" + highestvalue + "']").addClass('crActive');
        $("#ConstructionRiskId").val(highestvalue);
        $("input[name='risk_" + highestvalue + "'][rel='1']").prop("checked", true).closest('tr').addClass('crActive');
        SetActiveClass();
        settextresult();
        showClassNames();
    });

    $('.constClasschk').change(function () {
        debugger;

       
        $(".constclasstr").find(".stripe").removeClass("strikethrough");
        $(".constclasstr").find("input.form-control").addClass("disabled");
        $(".constclasstr").find("input.form-control").val("");
        
        //}
        $("#ClassDate").val();
        $("#ClassInitial").val();
        $(".constClasschk").removeClass("ccActive");
        $(".constClasschk").parent("td").parent("tr").removeClass("ccActive");
        var constclassId = $(this).val();
        if ($(this).prop('checked')) {
            $('.constClasschk:checked').prop("checked", false);
            $(this).prop('checked', true);
            $(this).attr('checked', true);
            $('#ConstructionClassId').val(constclassId);
        }
        else {
            $(this).prop('checked', false);
            $('.constClasschk').removeAttr('checked');
            if ($('#ConstructionClassId').val() == constclassId) {
                $('#ConstructionClassId').val('');
            }
        }
       
        $(this).parent("td").parent("tr").addClass("ccActive");
        $(this).parent("td").parent("tr").find("input.form-control").removeClass("disabled");
        settextresult(1);
        selectedActivityClass();
    });

    $("#btnshowPermit").click(function () {
        $("#btnSave").removeClass('disable');
        showpermit();
    });

    //$('.datepicker').on("cut copy paste", function (e) { e.preventDefault(); });

    $('#ProjectNo').on('input', function () {
        var text = $(this).val();
        $("#lblpermitno").html(text);
        $("#lblpermit").html(text);
    });


    $(".requestby").click(function () {
        loadSignView('DSPermitRequestBy.FileName', 'DSPermitRequestBy.FileContent');
        $('#signAndSave').modal('show');
    });

    $(".authorizedBy").click(function () {
        loadSignView('DSPermitAuthorizedBy.FileName', 'DSPermitAuthorizedBy.FileContent');
        $('#signAndSave').modal('show');
    });

    $(".reviewerby").click(function () {
        loadSignView('DSPermitReviewerBy.FileName', 'DSPermitReviewerBy.FileContent');
        $('#signAndSave').modal('show');
    });

    $("#btnuploadfile").click(function () {
        removeInboxview();
        $('#addIcraFiles').modal('show');
    })

    $("#btnInboxfile").click(function () {
        $("#icraInBoxview").show();
        $("#mainICRAView").hide();
        $("#myHeader").removeClass("sticky").addClass("sticky1");
    });

    $("#backBtn").click(function () {
        removeInboxview();
    });


    function removeInboxview() {
        $("#icraInBoxview").hide();
        $("#mainICRAView").show();
        $("#myHeader").removeClass("sticky1").addClass("sticky");
    }

    $(document).on('click', '.add-button', function () {     
        count = parseInt($('.ticrafilediv:last').attr('id'));
        var newID = count + 1;
        var template = $(this).parent("div").parent("div").parent("div").clone().attr("id", newID);
        template.find('input[type=text]').val(null);
        template.find('label').html('');
        template.find('textarea').val(null);
        $.each(template.find('input[type=text]'), function () {
            if ($(this).hasClass("fileName")) {
                var name = $(this).attr('name');
                name = "TICRAFiles[" + newID + "].Name";
                $(this).attr('name', name);
            }
        });
        $.each(template.find('textarea'), function () {
            if ($(this).hasClass("comment")) {
                var name = $(this).attr('name');
                name = "TICRAFiles[" + newID + "].Comment";
                $(this).attr('name', name);
            }
        });
        $.each(template.find('label'), function () {
            if ($(this).hasClass("file_msg")) {
                var id = $(this).attr('id');
                id = "TICRAFiles_" + newID;
                $(this).attr('id', id);
            }
        });
        template.find('input[type=file]').val(null);
        $.each(template.find('input[type=file]'), function () {
            if ($(this).hasClass("fileUpload")) {
                var tName = $(this).attr('tempName');
                var tfileName = $(this).attr('tempfileName');
                var tfilecontent = $(this).attr('tempfilecontent');

                tempName = "TICRAFiles[" + newID + "].Name";
                tempfileName = "TICRAFiles[" + newID + "].FileName"
                tempfilecontent = "TICRAFiles[" + newID + "].FileContent";              

                $(this).attr('tempName', tempName);
                $(this).attr('tempfileName', tempfileName);
                $(this).attr('tempfilecontent', tempfilecontent);
                $(this).attr('onchange', 'Uploadfile(this,"certificate")');

                var div = document.getElementById('hdn_div');
                div.innerHTML = div.innerHTML + ' <input type="hidden" name="' + tempName + '"/><input type="hidden" name="' + tempfileName + '"/> <input type="hidden" name="' + tempfilecontent + '"/>';
            }
        });
        $.each(template.find('input[type=hidden]'), function () {
            if ($(this).hasClass("ticraid")) {
                var name = $(this).attr('name');
                name = "TICRAFiles[" + newID + "].TicraId";
                $(this).attr('name', name);
            } else if ($(this).hasClass("uploadedby")) {
                var name = $(this).attr('name');
                name = "TICRAFiles[" + newID + "].UploadedBy";
                $(this).attr('name', name);
            }
        });
        $('.list-Contacts').append(template);
        count++;
    });

    $(document).on('click', '.remove-button', function () {
        var count = $('.ticrafilediv').length;
        if (count == 1) {
            return false;
        }
        var index = parseInt($(this).parent("div").parent("div").parent("div")[0].id) //$(this);
        var arr = "TICRAFiles[" + index + "].FileName";
        $("input[name='" + arr + "'][type=hidden]").val("");
        $(this).parent("div").parent("div").parent("div").remove();
    });   

    $.fn.scrollView = function () {
        return this.each(function () {
            $('html, body').animate({
                scrollTop: $(this).offset().top - $("#myHeader").height()
            }, 1000);
        });
    }

    $('#scroll-to-permit').click(function (event) {
        event.preventDefault();
        //showpermit();      
        $('#permit-div').scrollView();
    });

    $('#permit-const-type').click(function (event) {
        event.preventDefault();
        $('#dv_step_1').scrollView();
    });

    $('#permit-risk-group').click(function (event) {
        event.preventDefault();
        $('#dv_step_2').scrollView();
    });

    $('body').on('click', 'td.icrasteps', function () {
        //alert($(this).attr('id'));
        event.preventDefault();
        $("#icraInBoxview").hide();
        $("#mainICRAView").show();
        $("#myHeader").removeClass("sticky1").addClass("sticky");
        var divId = $(this).attr('id');
        $('#dv_' + divId).scrollView();
    });

    function settextresult(isclasschecked=0) {
        //debugger;
        var constType = "NA";
        var consttypevalue = $('input:checkbox.constructiontype:checked').attr("typename");//$('input:checkbox[name=constructiontype]:checked').attr('typename');
        if (consttypevalue != undefined) { constType = consttypevalue; }
        var constrisk = "NA";
        var constriskvalue = $("#hdn_constructionRiskName").val();
        if (constriskvalue != undefined) { constrisk = constriskvalue; }
        var constclass = "NA";
        var constclassvalue = $('input.constClasschk:checked').attr('classname');
        if (constclassvalue != undefined) { constclass = constclassvalue; }
        var lable = ('Construction Type: {0} ;    &nbsp;&nbsp;Construction Risk: {1} ;    &nbsp;&nbsp;Construction Class: {2}'.f(constType, constrisk, constclass));

        $("#lbltextresult").html(lable);
        if (isclasschecked == 0) {
            $('.constClasschk:checked').prop("checked", false);
        }
        if ($("#ConstructionClassId").val() != '') {
            $('.constClasschk[value=' + $("#ConstructionClassId").val() + ']').prop("checked", true);
        }
    }

    function loadSignView(fileNameCtr, fileContentCtr) {
        $('#signAndSave').empty();
        var url = CRxUrls.common_digitalsignature;
        $.ajax({
            url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr,
            cache: false,
            type: "GET",
            success: function (data) {
                $('#signAndSave').append(data);
                $('#signAndSave').fadeIn('fast');
            }
        });
    }
    $('.btnNotifyEmailPermitReq').click(function () {
        var ele = $(this).attr("seluser");
        var userId = $("#"+ele).val();
        if (TicraId > 0) {
            NotifyEmail(userId);
        }
    });
    $('#btnNotifyEmailPermitReq').click(function () {
        var userId = $('#PermitRequestBy').val();
        if (TicraId > 0) {
            NotifyEmail(userId);
        }
    });

    $('#btnNotifyEmailPermitAuth').click(function () {
        debugger;
        var userId = $('#PermitAuthorizedBy').val();
        if (TicraId > 0) {
            NotifyEmail(userId);
        }
    });

    $('#btnNotifyEmailPermitReviewer').click(function () {
        debugger;
        var userId = $('#PermitReviewerBy').val();
        if (TicraId > 0) {
            NotifyEmail(userId);
        }
    });

    if (getParameterByName('iseditable') == "False") {
        debugger;
      //  showpermit();
        //debugger;
        $("#tblicra").find("input,textarea,select").attr("disabled", "disabled");
        $('input[type="button"]').removeAttr('disabled');
        $('input[type="hidden"]').removeAttr('disabled');
        $(".ui-datepicker-trigger").remove();
        $("#btnPermitRequestBy").remove();
        $("#btnPermitAuthorizedBy").remove();
    }

    settextresult();

});

function ChangePermitRequestBy(userId, btncntrl, imgcntrl) {
    debugger;
    //alert("Call");
    var permitrequestbyId = btncntrl.val();
    if (permitrequestbyId != "") {
        if (permitrequestbyId == userId) {
            imgcntrl.show();
          //  $("#hdn_" + btncntrl).prop('required', true);
        } else {
            imgcntrl.hide();
           // $("#hdn_" + btncntrl).prop('required', false);
        }
    }
}

function showpermit() {
    $("#tblprmitdiv").append($("#permit_partial1div"));
    $("#permit-div").show();
    $("#btnshowPermit").hide();
    $('#permit-div').scrollView();
}

function NotifyEmail(userId) {
    debugger;
    var url = CRxUrls.icra_notifyemail;
    var PermitNo = $("#PermitNo").val();
    //if (typeof $("#ProjectNo").val() !== 'undefined') {
    //    projectno = $("#ProjectNo").val();
    //}
     
    if (userId > 0) {
        swal({
            html: true,
            title: "Message",
            text: "<label>Are you sure you want to send a email notification?</label>",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Send Email",
            cancelButtonText: "Cancel",
            closeOnConfirm: false,
            closeOnCancel: true
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    cache: false,
                    type: "GET",
                    success: function (data) {
                        //infoAlert('Email sent successfully.');
                        swalalert('Email sent successfully.');
                    },
                    url: url + "?userId=" + userId + "&permitno=" + PermitNo + "&permittype=2&version=" + $('#Version').val()
                });
            }
        });
    } else {
        infoAlert("Please select the user to send email notification!");
    }
}