var removeUrl = "/Account/RemoveUser";
var validatePassCode = "/Account/ValidatePassCode";
var clientUrl = "/Main/Clients";
var setClientUrl = "/Main/SetClient?orgkey=";
var pinDiv = $("#login-using-pin");
var redirectUrl = "/Main/SetClient";
var changepasswordurl = "/Auth/ChangePassword";



var pgwBrowser = $.pgwBrowser();

$(document).ready(function () {
    //pinDiv.hide();
    $("#recent-login").hide();

    var imageURLs = [
        "/dist/Images/login/login1.jpg"
        , "/dist/Images/login/login1.jpg"
        , "/dist/Images/login/login1.jpg"
    ];

    var imageText = [
        "<h3>CRx featured in On The Radar section of HFM Magazine's <br />April 2021 Issue!</h3>"
        , "<h3>CRx featured in On The Radar section of HFM Magazine's <br />April 2021 Issue!</h3>"
        , "<h3>CRx featured in On The Radar section of HFM Magazine's <br />April 2021 Issue!</h3>"
    ];

    //var canvas = document.getElementById('c');
    //var ctx = canvas.getContext('2d');

    //canvas.addEventListener('contextmenu', function (e) {
    //    e.preventDefault();
    //});

    //// Create an image element
    //var img = document.createElement('IMG');

    //// When the image is loaded, draw it
    //img.onload = function () {
    //    ctx.drawImage(img, 0, 0);
    //}

    //var loginImageUrl = getImageHtmlCode();
    //// Specify the src to load the image
    //img.src = loginImageUrl[1]; //"/dist/Images/login/login1.jpg";
    //$(".hfcmagazine").html(loginImageUrl[0]);

    //function getImageHtmlCode() {
    //    var returnData = [];
    //    var dataIndex = Math.floor(Math.random() * imageURLs.length);
    //    returnData.push(imageText[dataIndex]);
    //    returnData.push(imageURLs[dataIndex]);
    //    return returnData;
    //}



    function showLoading() {
        $(".error-content").empty();
        $("#btnlogin2").button('loading');
    }

    //$('#btnlogin2').on('click', function () {
    //    debugger;
    //    var $this = $(this);
    //    $this.button('loading');
    //    //$("#loginform").submit();
    //    $(this).attr("disabled", true);
    //});

    localStorage.clear();
    $(".redarget-forget").click(function () {
        $("#login-form").hide();
        $("#login-using-pin").hide();
        $("#login-using-vendor-orgId").hide();
        $("#forgot-password").addClass("login-hideshow").fadeIn();
        $("#forgot-password").removeClass("hide").fadeIn();

    });

    $("#loginshow").click(function () {
        showLoginBox();
    });

    $(".login-show").click(function () {
        showLoginBox();
    });


    $(".login-show-main").click(function () {
        HideGuestUser();
    });
    function showLoginBox() {
        // debugger;
        $("#login-form").removeClass("hide").fadeIn();
        $("#login-using-vendor-orgId").hide();
        $("#forgot-password").removeClass("login-hideshow");
        $("#forgot-password").addClass("hide").fadeOut();
    }

    function HideGuestUser() {
        //debugger;
        $("#forgot-password").removeClass("login-hideshow");
        $("#forgot-password").addClass("hide").fadeOut();
        $("#guest-user").addClass("hide");
        $("#login-form").addClass("hide").fadeIn();
    }

    if (window.location.hash.substr(1) === "forgot-password") {
        $(".redarget-forget").trigger("click");
    };

    $(".userLi").click(function () {
        //debugger
        $(".error-content-pin").empty();
        $(".error-content").empty();

        if ($(this).hasClass('newaccount')) {
            $("#recent-login").hide();
            $("#login-using-vendor-orgId").hide();
            $("#login-form").removeClass("hide");
        }
        else if ($(this).hasClass('vendorinvitation')) {
            $("#recent-login").hide();
            $("#login-form").hide();
            $("#login-using-vendor-orgId").removeClass("hide").fadeIn();
        }
        else if ($(this).hasClass('guestuser')) {
            debugger
            $("#recent-login").hide();
            $("#login-form").hide();
            $("#login-using-vendor-orgId").hide();
            $("#guest-user").removeClass("hide").fadeIn();

        }
        else {
            var email = $(this).find(".email").html();
            var isPin = $(this).attr("ispin");
            pinDiv.show();
            $("#recent-login").hide();
            $("#login-using-vendor-orgId").hide();
            pinDiv.find(".email").html(email);
            pinDiv.find("#UserName").val(email);
            if (isPin === "true") {
                $(".user-pin-login").removeClass("hide");
                $(".user-pin-login").fadeIn();
                $(".user-pwd-password").fadeOut();
            } else {
                $(".user-pin-login").fadeOut();
                $(".user-pwd-password").fadeIn();
            }
        }
    });

    $("#login-using-pin .userBack").click(function () {
        $("#recent-login").show();
        $("#login-using-pin").hide();
        $("#login-using-vendor-orgId").hide();
    });


    $(".gologin").click(function () {
        $(".user-pin-login").hide();
        $("#login-using-vendor-orgId").hide();
        $(".user-pwd-password").show();
    });


    $(".removeUser").click(function (e) {
        e.stopPropagation();
        var ctrUL = $(this).closest("li");
        var email = ctrUL.find(".email").html();
        if (email) {
            $.ajax({
                url: removeUrl + "?userName=" + email,
                method: "POST",
                data: {
                    __RequestVerificationToken: $("input[name='__RequestVerificationToken']").val()
                },
                success: function (data) {
                    ctrUL.remove();
                    if (data.ucont === 0) {
                        $("#recent-login").hide();
                        $("#login-using-vendor-orgId").hide();
                        $("#login-form").removeClass("hide");
                    }
                }
            });
        }
    });


    //$.getJSON("https://api.ipify.org/?format=json", function (e) {
    //    $('input[name="ip"]').val(e.ip);
    //});

    var uid = fp.get();
    //$('input[name="browserName"]').val(pgwBrowser.browser.name + " version: " + pgwBrowser.browser.majorVersion);
    //$('input[name="osName"]').val(pgwBrowser.os.name);
    $('input[name="dToken"]').val(uid);

    $(document).on('click', '.hideShowpass', function () {
        var $pwd = $(this).closest('.form-group').find(".password-control");
        $(this).find(".fa").toggleClass("fa-eye-slash");
        if ($pwd.attr('type') === 'password') {
            $pwd.attr('type', 'text');

        } else {
            $pwd.attr('type', 'password');
        }
    });


});



var loginSuccess = function (data) {
    debugger;
    if (data.result > -1) {
        pageRedirect(data.result, data.returnUrl, data.orgkey, data.userId, data.refreshToken);
    } else {
        var errorMsg = data.msg;
        $(".login-button").button('reset');
        $(".error-content").html(errorMsg);
    }
};

var AjaxBegin = function () {
    //debugger;
    $(".error-content-pin").empty();
    $(".error-content").empty();
    $(".login-button").button('loading');
}

//var loginSuccessRecent = function (data) {
//   // debugger
//    if (data.result > -1) {
//        pageRedirect(data.result, data.returnUrl, data.orgkey);
//    } else {
//        var errorMsg = data.msg;
//        $(".login-button").button('reset');
//        $(".error-content-pin").html(errorMsg);
//    }
//};

var pageRedirect = function (resultType, returnUrl, orgkey, userId, refreshToken) {
    if (resultType === 1) {
        window.location.href = clientUrl + "?returnUrl=" + returnUrl;
    } else if (resultType === 0) {
        window.location.href = redirectUrl + "?returnUrl=" + returnUrl + "&orgkey=" + orgkey + "&refreshToken=" + refreshToken;
    } else if (resultType === 3) {
        window.location.href = returnUrl + "?userId=" + userId;
    } else if (resultType === 4) {
        window.location.href = changepasswordurl;
    } else if (resultType === 2) {
        window.location.href = returnUrl;
    }
};

var loginUsingPin = function (userName, pin) {
    $.ajax({
        url: validatePassCode + "?userName=" + userName + "&pin=" + pin,
        method: "POST",
        success: function (data) {
            debugger;
            console.log(data);
            if (data.result === 1) {
                window.location.href = clientUrl;
            } else if (data.result === 0) {
                window.location.href = setClientUrl + data.orgkey;
            } else {
                $(".error-content").html(data.msg);
            }
        }
    });
};

var loginpin = $('#loginpin').pinlogin({
    fields: 6,
    reset: false,
    complete: function (pin) {
        loginpin.reset();
        var email = $("#login-using-pin").find(".email").html();
        loginUsingPin(email, pin);
    }
});

var fp = new Fingerprint({
    canvas: true,
    ie_activex: true,
    screen_resolution: true
});

var OnSuccess = function (data) {
    $(".error-content").html(data.Result);
    $("#loginshow").trigger("click");
};