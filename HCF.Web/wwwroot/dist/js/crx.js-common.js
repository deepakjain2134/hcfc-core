$(document).ready(function () {
    $(document).on({
        'show.bs.modal': function () {
            var zIndex = 1040 + (10 * $('.modal:visible').length);
            $(this).css('z-index', zIndex);
            setTimeout(function () {
                $('.modal-backdrop').not('.modal-stack').css('z-index', zIndex - 1).addClass('modal-stack');
            }, 0);
        },
        'hidden.bs.modal': function () {
            if ($('.modal:visible').length > 0) {
                // restore the modal-open class to the body element, so that scrolling works
                // properly after de-stacking a modal.
                setTimeout(function () {
                    $(document.body).addClass('modal-open');
                }, 0);
            }
        }
    }, '.modal');
});

$(function () {


    $(window).on("load", function () {
        $(".crx-scrollBar").mCustomScrollbar({
            setHeight: 300,
            theme: "3d-dark"
        });
    });
    $(window).on("load", function () {
        $(".crx-mainscrollBar").mCustomScrollbar({
            setHeight: 300,
            theme: "3d-thick-dark"
        });
    });

    //$(".epDescription").shorten({ "showChars": 225, "seeMore": false, "moreText": "See More" });

    //$('.descriptions').popover({ trigger: 'focus' });
    $(".descriptions").popover({ trigger: "manual", html: true, animation: false })
        .on("mouseenter", function () {
            var _this = this;
            $(this).popover("show");
            $(".popover").on("mouseleave", function () {
                $(_this).popover('hide');
            });
        }).on("mouseleave", function () {
            var _this = this;
            setTimeout(function () {
                if (!$(".popover:hover").length) {
                    $(_this).popover("hide");
                }
            }, 50);
        });

    $('#myTable').dataTable({
        searching: false,
        pageLength: 20,
        lengthChange: false,
        bPaginate: $("#myTable tbody tr").length > 20,
        bInfo: false,
        stateSave: true,
        aaSorting: [],
        language: {
            emptyTable: "No data available in table",
            search: "_INPUT_", //To remove Search Label
            searchPlaceholder: "Search..."
        },
        aoColumnDefs: [
            {
                orderSequence: ["desc", "asc"],
                aTargets: ['_all']
            }
        ],
        dom: "<'row'<'col-sm-3'l><'col-sm-3'f><'col-sm-6'p>>" +
            // "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12'<'table-responsive'tr>>>" +
            "<'row'<'col-sm-5'i><'col-sm-7 text-right'p>>"
    });



    $(".dataTables_wrapper input[type='search']").attr("maxlength", 25);


    $.fn.dataTable.moment('MM d,yyyy');

    $.fn.dataTable.moment('MM d,yyyy hh:mm A');

    $('.alphaonly').alpha();

    $(".numeric").numericInput();

    $('.phone').mask('(000) 000-0000');

    $(".phone").on("blur", function () {
        var last = $(this).val().substr($(this).val().indexOf("-") + 1);
        if (last.length === 5) {
            var move = $(this).val().substr($(this).val().indexOf("-") + 1, 1);
            var lastfour = last.substr(1, 4);
            var first = $(this).val().substr(0, 9);
            $(this).val(first + move + '-' + lastfour);
        }
    });

    $("body").on("click", ".modal-link", function (e) {
        e.preventDefault();
        $(this).attr('data-target', '#modal-container');
        $(this).attr('data-toggle', 'modal');
    });

    $('body').on("click", ".modal-close-btn", function () {
        $('#modal-container').modal('hide');
    });

    $("#modal-container").on('hidden.bs.modal', function () {
        $(this).removeData("bs.modal");
    });

    $("#CancelModal").on("click", function () {
        return false;
    });

    $("input").on("keypress", function (e) {
        //debugger;
        if (e.which === 32 && !this.value.length) {
            e.preventDefault();
        }
        else if (/^\s/.test(this.value))
            this.value = "";
    });


    //var checkConnection = function () {
    //    var status = navigator.onLine;
    //    if (status) {
    //        return true;
    //    } else {
    //        AlertInfoMsg("No Internet Connection !!");
    //        return false;
    //    }
    //}

    //console.log("%cCRx", "font-size: 144px;    font-weight: bold;");console.log("%c by HCF compliance", "font-size: 44px;    font-weight: bold;");

    SetTimeZone();

    var isFullScreen = localStorage.getItem("isFullScreen");
    if (isFullScreen === 1) {
        toggleFullScreen(document.body);
    }

    // start datepicker

    $("img.ui-datepicker-trigger").attr('alt', "Select date").attr("title", "Select date");

    //$('.newsdatepicker').datepicker({
    //    showOn: "both",
    //    buttonImage: ImgUrls.datepicker_calendar,
    //    buttonImageOnly: true,
    //    dateFormat: $.CRx_DateFormat,
    //    maxDate: 60,
    //    beforeShow: changeYearButtons,
    //    onChangeMonthYear: changeYearButtons
    //});

    $(".datepicker").datepicker({
        showOn: "both",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        maxDate: 0,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
        //   dateFormat: 'MMM d, yyyy'
    });

    //$('.date').datepicker({
    //    changeYear: false,
    //    minDate: mindate,
    //    maxDate: maxdate,
    //    dateFormat: 'D, M d'
    //    // dateFormat: 'D, M d, yy'
    //    //dateFormatD:'D,MM d,yy'
    //});

    $('.futuredatepicker').datepicker({
        showOn: "both",
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        beforeShow: changeYearButtons,
        onChangeMonthYear: changeYearButtons
    });

    $(".datepicker, .startDate, .endDate, .futuredatepicker, .timepicker , .discopy ").on("cut copy paste", function (e) { e.preventDefault(); });

    $(".datepicker, .startDate, .endDate, .futuredatepicker , .timepicker , .discopy ").keypress(function (e) { e.preventDefault(); });


    $("body").on("focus", ".fromSearch", function () {
        $(this).datepicker();
    });


    var from = $(".startDate").datepicker({
        showOn: "both",
        numberOfMonths: 1,
        buttonImage: ImgUrls.datepicker_calendar,
        buttonImageOnly: true,
        dateFormat: $.CRx_DateFormat,
        beforeShow: changeYearButtonStartDate,
        onChangeMonthYear: changeYearButtonStartDate
    }).on("change", function () {
        // debugger;

        // to.datepicker("option", "minDate", $(this).val());
        $(".endDate").datepicker("option", "minDate", $(".startDate").datepicker("getDate"));
        $(".startDate").datepicker("option", "maxDate", $(".endDate").datepicker("getDate"));
        //$(".startDate").focus();
    }),
        to = $(".endDate").datepicker({
            showOn: "both",
            numberOfMonths: 1,
            buttonImage: ImgUrls.datepicker_calendar,
            buttonImageOnly: true,
            dateFormat: $.CRx_DateFormat,
            beforeShow: changeYearButtonEndDate,
            onChangeMonthYear: changeYearButtonEndDate
        }).on("change", function () {
            from.datepicker("option", "maxDate", $(this).val());

        });


    // end date
    $('.dropdown-toggle').click(function (e) {
        e.preventDefault();
        setTimeout($.proxy(function () {
            if ('ontouchstart' in document.documentElement) {
                $(this).siblings('.dropdown-backdrop').off().remove();
            }
        }, this), 0);
    });

});
//$(function () { $(".ddlboxLive").selectpicker('render'); })

$(".ddlboxLive").selectpicker().attr('data-live-search', true).attr('data-size', 5).attr('data-width', 'auto');

$(".ddlbox").selectpicker().attr('data-live-search', false).attr('data-size', 5).attr('data-width', 'auto');


var setDateFormat = function (date) {
    var newDate = date.toLocaleDateString("en-US", { month: 'short' })
        + " " + date.toLocaleDateString("en-US", { day: 'numeric' })
        + ", " + date.toLocaleDateString("en-US", { year: 'numeric' });
    return newDate;
};


var changeYearButtons = function (ctr, op1, op2) {
    var inputDate = $('.datepicker');
    if (ctr.id) {
        inputDate = $('#' + ctr.id);
    } else {
        inputDate = $('#' + op2.id);
    }
    changeDatePickerYear(inputDate);
};

//var changeYearButtons = function () {
//    var inputDate = $('.datepicker');
//    changeDatePickerYear(inputDate);
//};

var changeYearButtonStartDate = function () {
    var inputDate = $('.startDate');
    changeDatePickerYear(inputDate);
};

var changeYearButtonEndDate = function () {
    var inputDate = $('.endDate');
    changeDatePickerYear(inputDate);
};

var changeDatePickerYear = function (inputDate) {
    setTimeout(function () {
        var nextYearClass = "btnYear nextyear";
        var prevYearClass = "btnYear prevyear";
        var widgetHeader = inputDate.datepicker("widget").find(".ui-datepicker-header");

        var nextMonth = inputDate.datepicker("widget").find(".ui-datepicker-next");
        if (nextMonth.hasClass("ui-state-disabled")) {
            nextYearClass = "btnYear nextyear disable";
        }
        var nextYrBtn = $('<a class="' + nextYearClass + '" title="NextYr"></a>');
        nextYrBtn.bind("click", function () {
            $.datepicker._adjustDate(inputDate, +1, 'Y');
        });

        var prevMonth = inputDate.datepicker("widget").find(".ui-datepicker-prev");
        if (prevMonth.hasClass("ui-state-disabled")) {
            prevYearClass = "btnYear prevyear disable";
        }
        var prevYrBtn = $('<a class="' + prevYearClass + '" title="PrevYr"></a>');
        prevYrBtn.bind("click", function () {
            $.datepicker._adjustDate(inputDate, -1, 'Y');
        });

        prevYrBtn.appendTo(widgetHeader);
        nextYrBtn.appendTo(widgetHeader);

    }, 1);

};


$(window).scroll(function () {
    var sticky = $('#myHeader'),
        scroll = $(window).scrollTop();

    if (scroll >= 100) sticky.addClass('sticky');
    else sticky.removeClass('sticky');
    $("input.hasDatepicker").blur();
});

function SetTimeZone() {
    const timezoneCookie = "timezoneoffset";
    if (!$.cookie(timezoneCookie)) {
        const testCookie = 'hcf cookie';
        $.cookie(testCookie, true);
        if ($.cookie(testCookie)) {
            $.cookie(testCookie, null);
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
    else {
        const storedOffset = parseInt($.cookie(timezoneCookie));
        const currentOffset = new Date().getTimezoneOffset();
        if (storedOffset !== currentOffset) {
            $.cookie(timezoneCookie, new Date().getTimezoneOffset());
            location.reload();
        }
    }
}

$(".toggle_menu").click(function () {
    $(".navbar").toggle();
});

$body = $("body");

$(document).on({
    ajaxComplete: function () {
        $body.removeClass("loading");
    },
    beforeSend: function (xhr) {
        xhr.setRequestHeader("XSRF-TOKEN",
            $('input:hidden[name="__RequestVerificationToken"]').val());
    },
    ajaxError: function (event, jqxhr, settings) {
        $body.removeClass("loading");
        //debugger;
        if (jqxhr.status != "200") {
            console.log(jqxhr);
            if (jqxhr.status == "401") {
                // swalalert("Login", "your session has expired please login again ", "warning");
            } else if (jqxhr.status == 0) {
                //swalalert("Unexpected Error ", "We are not able to connect with server. Please contact the system administrator. Error Code=" + jqxhr.status, "warning");
                //swalalertLogin("Login", "An unexpected error has occurred ", "warning");
            } else {
                //swalalert("Unexpected Error ", "An unexpected error has occurred. Please contact the system administrator. Error Code=" + jqxhr.status, "warning");
            }
        }
    },
    ajaxSend: function (event, jqXHR, ajaxOptions) {

        //var myCookieCookie = getCookie("loginUserId");
        //if (myCookieCookie == null) {
        //    ///swalalert("Login", "your session has expired please login again ", "warning");
        //    jqXHR.abort();
        //    return;
        //}
    },
    ajaxStart: function (e, xhr, options) {

       // console.log("ajaxStart");

        $body.addClass("loading");
    },
    ajaxStop: function () {
        $body.removeClass("loading");
    },
    ajaxSuccess: function () {
    }

});


function getCookie(name) {
    var dc = document.cookie;
    var prefix = name + "=";
    var begin = dc.indexOf("; " + prefix);
    if (begin == -1) {
        begin = dc.indexOf(prefix);
        if (begin != 0) return null;
    }
    else {
        begin += 2;
        var end = document.cookie.indexOf(";", begin);
        if (end == -1) {
            end = dc.length;
        }
    }
    return decodeURI(dc.substring(begin + prefix.length, end));
}


//common method for inspection comment

$(document).on("click", ".commentInsp", function () {
    openActivityCommentComment($(this));

});

var openActivityCommentComment = function (control) {
    $("#Commenttxt").val("");
    var ctrId = $(control).attr("id");
    var controlName = $(control).attr("tempname");
    localStorage.setItem("hdnfield", controlName);
    localStorage.setItem("clickCtr", ctrId);
    var value = $("input[name='" + controlName + "'][type=hidden]").val();
    $("#Commenttxt").val(value);
};

$(document).on("click", "#closeComment", function () {
    $('#commentModal').modal('hide');
});

$(document).on("click", "#saveComment", function () {
    var comment = $("#Commenttxt").val();
    var hdnfield = localStorage.getItem("hdnfield");
    commenthdn = $("input[name='" + hdnfield + "'][type=hidden]");
    commenthdn.val(comment);
    var clickCtr = localStorage.getItem("clickCtr");
    if (comment.length > 0) {
        $("#" + clickCtr).addClass("text");
    } else
        $("#" + clickCtr).removeClass("text");

    $("#commentModal").modal("hide");
});

//end


function getfileUrl(src) {
    debugger;
    const filename = src.replace(/^.*[\\\/]/, '');
    const extension = filename.substr((filename.lastIndexOf('.') + 1));
    if (extension === 'pdf') {
        return src + "#toolbar=1&view=FitH";
    } else if (extension === 'doc' || extension === 'docx' || extension === 'xlsx' || extension === 'xls') {
        src = $.Constants.Browse_View_Files_In_IFrame + src + "&embedded=true";
    }
    else if (extension === 'png' || extension === 'jpeg' || extension === 'jpg') {
        src = src;
    }
    return src;
}


function loadSignView(fileNameCtr, fileContentCtr, imgPreview = "") {
    $('#signAndSave').empty();
    var url = CRxUrls.common_digitalsignature;
    $.ajax({
        url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr + "&imgPreviewClass=" + imgPreview,
        cache: false,
        type: "GET",
        success: function (data) {
            $('#signAndSave').append(data);
            $('#signAndSave').fadeIn('fast');
        }
    });
}
//function loadSignViewNew(fileNameCtr, fileContentCtr, imgPreview = "") {
//    $('#signAndSave').empty();
//    var url = CRxUrls.common_digitalsignature;
//    $.ajax({
//        url: url + "?fileName=" + fileNameCtr + "&fileContent=" + fileContentCtr + "&imgPreviewClass=" + imgPreview ,
//        cache: false,
//        type: "GET",
//        success: function (data) {
//            $('#signAndSave').append(data);
//            $('#signAndSave').fadeIn('fast');
//        }
//    });
//}

//// Automatic Success Alert close script Start
//$(".alert-success").hide();
//$(".alert-success").fadeTo(15000, 500).slideUp(500, function () {
//    $(".alert-success").slideUp(500);
//});

//$(".alert-danger").hide();
//$(".alert-danger").fadeTo(15000, 500).slideUp(500, function () {
//    $(".alert-danger").slideUp(500);
//});
//// Automatic Success Alert close script End


function LoadRecentFiles() {
    $("#modal-container1 .close").click();
    $(".modal-backdrop.in").hide();
    var modelContainer = $("#modal-container");
    modelContainer.empty();
    const recentFiles = CRxUrls.Common_RecentFiles;
    $.get(recentFiles, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}


function hideModel() {
    $("#modal-container1 .close").click();
    $(".modal-backdrop.in").hide();
}

function LoadDrawingPathway() {
    debugger;
    $("#modal-container .close").click();
    $(".modal-backdrop.in").hide();
    var modelContainer = $("#modal-container1");
    modelContainer.empty();
    const recentFiles = CRxUrls.Common_DrawingPathway;
    $.get(recentFiles, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}
function SendMailPopUp() {
    var modelContainer = $("#modal-container");
    modelContainer.empty();
    const replyDocumentUrl = CRxUrls.Common_ReplyDocument;
    $.get(replyDocumentUrl, function (data) {
        modelContainer.html('');
        modelContainer.html(data);
        modelContainer.fadeIn('fast');
        modelContainer.modal('show');
    });
}

//$(".dynamic_menu").click(function () {
//    debugger;
//    if ($(this).hasClass('activeMenu')) {
//        $(this).removeClass('activeMenu');
//    } else {
//        $('.dynamic_menu').removeClass('activeMenu');
//        $(this).addClass('activeMenu');
//    }
//});

//$(".multi-column").mouseleave(function () {
//    $(".dynamic_menu").removeClass("activeMenu");
//});


var jsformat = function (str, col) {
    if (col && str) {
        col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);
        return str.replace(/\{\{|\}\}|\{(\w+)\}/g,
            function (m, n) {
                if (m === "{{") {
                    return "{";
                }
                if (m === "}}") {
                    return "}";
                }
                return col[n];
            });
    }
};


// auto logout user when user logout on one of the opened tab start

$(document).ready(function () {
    if (window.localStorage) {
        $('a#_linkLogout').on('click', function () {
            localStorage.setItem("app-logout", 'logout' + Math.random());
            return true;
        });
    }
});

window.addEventListener('storage', function (event) {
    if (event.key == "app-logout") {
        window.location = $('a#_linkLogout').attr('href');
    }
}, false);

// end

$(document).ready(function () {
    $(".ePDescriptions").shorten({
        moreText: 'read more',
        showChars: 120,
        lessText: 'read less'
    });
});






function AddReadMore() {
    var carLmt = 240;
    var readMoreTxt = " ... Read More";
    var readLessTxt = " Read Less";
    $(".addReadMore").each(function () {
        if ($(this).find(".firstSec").length)
            return;

        var allstr = $(this).text();
        if (allstr.length > carLmt) {
            var firstSet = allstr.substring(0, carLmt);
            var secdHalf = allstr.substring(carLmt, allstr.length);
            var strtoadd = firstSet + "<span class='SecSec'>" + secdHalf + "</span><span class='readMore'  title='Click to Show More'>" + readMoreTxt + "</span><span class='readLess' title='Click to Show Less'>" + readLessTxt + "</span>";
            $(this).html(strtoadd);
        }

    });
    $(document).on("click", ".readMore,.readLess", function () {
        $(this).closest(".addReadMore").toggleClass("showlesscontent showmorecontent");
    });
}
$(function () {
    AddReadMore();
});