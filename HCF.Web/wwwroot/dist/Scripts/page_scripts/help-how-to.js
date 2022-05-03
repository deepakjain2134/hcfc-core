$(() => {
    $(".chatbox-open").click(() => {
        debugger;
        $(".chatbox-popup-content-data").fadeOut();
        $(".chatbox-popup-content-main").fadeIn();
        $(".chatbox-open").fadeOut();
        $(".chatbox-popup").fadeIn();
        $(".more-link").html("Suggest a Tip for users or CRx improvement");
    });

    $(".chatbox-popup-close").click(() => {
        $(".chatbox-open").fadeIn();
        $(".chatbox-popup").fadeOut();
    });

    $(".chatbox-panel-close").click(() => {
        $(".chatbox-popup, .chatbox-close").fadeOut();
    });

    $(".chatbox-maximize").click(() => {
        $(".chatbox-popup, .chatbox-open, .chatbox-close").fadeOut();
        $(".chatbox-panel").fadeIn();
        $(".chatbox-panel").css({ display: "flex" });
    });


    $(".chatbox-minimize").click(() => {
        $(".chatbox-panel").fadeOut();
        $(".chatbox-popup, .chatbox-open, .chatbox-close").fadeIn();
    });

    $(".chatbox-panel-close").click(() => {
        $(".chatbox-panel").fadeOut();
        $(".chatbox-open").fadeIn();
    });


    $("#btn_user").click(() => {
        $(".btn_user").fadeIn();
        $(".btn_ImprovementSuggestion").fadeOut();
    });

    $("#btn_ImprovementSuggestion").click(() => {
        $(".btn_ImprovementSuggestion").fadeIn();
        $(".btn_user").fadeOut();
    });

    $(".more-link").click(() => {
        //debugger;
        $(".more-link").toggleClass('toggleActive');
        if ($(".more-link").hasClass('toggleActive')) {
            $(".more-link").html("Back to help");
            $(".chatbox-popup-content-data").fadeIn();
            $(".chatbox-popup-content-main").fadeOut();
        } else {
            $(".more-link").html("Suggest a Tip for users or CRx improvement");
            $(".chatbox-popup-content-data").fadeOut();
            $(".chatbox-popup-content-main").fadeIn();
        }
    });



});

$(".tips-suggestionbtnBox .comm-button").click(function () {
    if ($(this).hasClass("current")) {
        $(this).removeClass("current");
    } else {
        $(".tips-suggestionbtnBox .comm-button.current").removeClass("current");
        $(this).addClass("current");
    }
});

$('body').on('click', '#tipsubmitButton', function () {
    var controllername = $("#helpPageControllerName").val();
    var pageName = $("#helpPageActionName").val();
    var description = $('#user_tips').val();
    if (description != "") {
        $.get(CRxUrls.Tips_SaveTip + '?description=' + description + '&pagename=' + controllername + "_" + pageName + '&mode=tip', function (data) {
            if (data != null) {
                $("#user_tips").val("");
                successAlert("Thank You For Sharing Your Feedback");
            }
        });
    } else {
        successAlert('Please Enter any Tip First!');
    }
});

$('body').on('click', '#Improvement_btn', function () {
    var controllername = $("#helpPageControllerName").val();
    var pageName = $("#helpPageActionName").val();
    var description = $("#user_suggestions").val();
    if (description != "") {
        $.get(CRxUrls.Tips_SaveTip + '?description=' + description + '&pagename=' + controllername + "_" + pageName + '&mode=crxImprovement', function (data) {
            if (data != null) {
                $("#user_suggestions").val("");
                successAlert("Thank You For Sharing Your Feedback");
            }
        });
    } else {
        successAlert('Please Enter any Suggestion First!');
    }
});