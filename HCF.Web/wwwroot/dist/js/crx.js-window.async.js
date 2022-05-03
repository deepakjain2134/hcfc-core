


function callGetAjax(placeHolder, urlAction, model) {
    //console.log(window.async);
    debugger;
    var ctr = $("#" + placeHolder);
    if (placeHolder == "goal_assetsInspection") {
        var loadingClass = "crxInnerloader";
    }
    else {
        var loadingClass = "asyncloader_none";
    }
    
    ctr.addClass(loadingClass);

    $.ajax({
        url: urlAction,
        type: "GET",
        global: false,
        data: model,
        success: function (data) {
            ctr.empty();
            ctr.html(data);
            ctr.removeClass(loadingClass);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            ctr.removeClass(loadingClass);
            ctr.append("<h3>We are unable to process your request.</h3>")
        }
    });
}