$(document).ready(function () {

    var input = document.getElementById("inputSuccess2");
    input.addEventListener("keyup", function (event) {
        if (event.keyCode === 13) {
            event.preventDefault();
            localStorage.setItem("dashsearchData", $("#inputSuccess2").val());
            document.getElementById("btntxtsearch").click();
        }
    });

    getActivityDashboard(1);
    $("#catfilter").show();
    pageload();

    $("[name=dashEPCat]").on('change', function () {

        var catId = $("input[name='dashEPCat']:checked").val();
        localStorage.setItem("dashCatId", catId);
        getActivityDashboard(1);
        //filterEps(1)
    });


    $("input[name=dashEPCat]:radio").change(function () {

        var catId = $("input[name='dashEPCat']:checked").val();
        localStorage.setItem("dashCatId", catId);
        getActivityDashboard(catId);
    });

    $(".button1").click(function () {
        $(".button1").removeClass("active");
        $("#btn_dueWithin").val("Due within Days");
        $(this).addClass("active");
    });

    function pageload() {
        var isback = getParameter('Isback');
        var currentTab = localStorage.getItem("tabName");
        if (isback == 1 && currentTab != null) {
            //debugger;
            $("li").removeClass("active");
            $(".tab-pane").removeClass("active");
            var childa = $("#" + currentTab);
            var href = childa.find("a").attr("href");
            $(href).addClass("active");
            childa.addClass("active");
            $("#" + currentTab).click();
        } else {
            var logoimgbase64 = localStorage.getItem('logoimgbase64');
            localStorage.clear();
            localStorage.setItem('logoimgbase64', logoimgbase64);
            //set default cat value 
            localStorage.setItem("tabName", "activitytab");
            var catId = $("input[name='dashEPCat']:checked").val();
            localStorage.setItem("dashCatId", catId);
        }
    }

    function getParameter(paramName) {
        var searchString = window.location.search.substring(1),
            i, val, params = searchString.split("&");

        for (i = 0; i < params.length; i++) {
            val = params[i].split("=");
            if (val[0] == paramName) {
                return val[1];
            }
        }
        return null;
    }

    $('#btn_Inprogress').on('click', function () {
        localStorage.setItem("btnId", 'btn_Inprogress');
        localStorage.setItem("btnValue", 2);
        localStorage.setItem("Datein", 0);
        localStorage.setItem("due_within_filter", "Days");
        getActivityDashboard(1);
    });

    $('#btn_pastdue').on('click', function () {

        localStorage.setItem("btnId", 'btn_pastdue');
        localStorage.setItem("btnValue", 0);
        localStorage.setItem("Datein", 0);
        localStorage.setItem("due_within_filter", "Days");
        getActivityDashboard(1);
    });

    $('#btn_all').on('click', function () {
        localStorage.setItem("btnId", 'btn_all');
        localStorage.setItem("btnValue", "");
        localStorage.setItem("Datein", 0);
        localStorage.setItem("due_within_filter", "Days");
        getActivityDashboard(1);
    });

    $('#btn_Completed').on('click', function () {
        localStorage.setItem("btnId", 'btn_Completed');
        localStorage.setItem("btnValue", 1);
        localStorage.setItem("Datein", 0);
        localStorage.setItem("due_within_filter", "Days");
        getActivityDashboard(1);
    });

    $('#ddlUsers').on('change', function (e) {
        //console.log(this.value, this.options[this.selectedIndex].value, $(this).find("option:selected").val());
        //localStorage.setItem("btnId", 'ddlUsers');
        $("input[name='dashEPCat']:checked").val(0);

        localStorage.setItem("dashCatId", 0);
        localStorage.setItem("dashboardUserId", this.value);
        getActivityDashboard(1);
    });

    //$('a.filter_user').on('click', function (e) {

    //    localStorage.setItem("btnId", 'btn_user_ddl');
    //    e.preventDefault();
    //    var text = $(this).text();
    //    var userId = parseInt($(this).attr('uid'));
    //    $("#btn_users").val(text.trim());
    //    $("input[name='dashEPCat']:checked").val(0);
    //    localStorage.setItem("dashCatId", 0);
    //    localStorage.setItem("dashboardUserId", userId);
    //    getActivityDashboard(1);
    //});

    $("a.input_search").on("click",
        function (e) {
            const searchText = $("#inputSuccess2").val();
            if (searchText) {
                debugger;
                const value = $(this).data("value");
                localStorage.setItem("btnValue", value);
                $(".input-sublistItems").hide("linear");
                getActivityDashboard(1);
            }
        });


    $('a.filter_due_within').on('click', function (e) {
        localStorage.setItem("btnId", 'btn_dueWithin');
        e.preventDefault();
        var text = $(this).text();
        var days = parseInt($(this).text());
        localStorage.setItem("due_within_filter", text);
        $("#btn_dueWithin").val("Due within " + text);
        if (text == "This Month") {
            var currentDate = new Date();
            var month = currentDate.getMonth() + 1;
            var year = currentDate.getFullYear();
            localStorage.setItem("DueMonth", month);
            localStorage.setItem("DueYear", year);
            localStorage.setItem("Datein", -1);
        } else if (text == "Next Month") {
            var nextdate = new Date();
            // nextdate.setMonth(2); //as default for jan in getMonth() is 0 
            var month = nextdate.getMonth() + 2;
            var year = nextdate.getFullYear();
            localStorage.setItem("DueMonth", month);
            localStorage.setItem("DueYear", year);
            localStorage.setItem("Datein", -1);
        } else {
            localStorage.setItem("DueMonth", 0);
            localStorage.setItem("DueYear", 0);
            localStorage.setItem("Datein", days);
        }
        getActivityDashboard(1);
    });

    $('#btn_upcoming').on('click', function () {
        localStorage.setItem("Datein", 90);
        getActivityDashboard(1);
    });

    $(window).scroll(function () {
        if ($(window).scrollTop() === $(document).height() - $(window).height()) {
            let tab = $('.nav-tabs li.active.parentTab');
            if (tab[0].id == 'activitytab') {
                loadMoreRecords();
            }
        }
    });
});

$('body').on('click', '#btnloadMore', function () {
    loadMoreRecords();
});

function loadMoreRecords() {
    var oldnum = 1;
    if (localStorage.getItem("PageIndex") != null) {
        oldnum = parseInt(localStorage.getItem("PageIndex"));
    }
    localStorage.setItem("isScroll", true);
    getActivityDashboard(oldnum + 1);
    localStorage.setItem("PageIndex", oldnum + 1);
}

$('#defeciencytab').on('click', function () {
    $(".assetDeficienciesleg").show();
    localStorage.setItem("tabName", "defeciencytab");
    $("#catfilter").hide();
    if ($("#li_tab4").hasClass("active")) {
        $("#tab4").addClass("active");
        $("#li_tab4").addClass("active");
    } else if ($("#li_tab3").hasClass("active")) {
        $("#tab3").addClass("active");
        $("#li_tab3").addClass("active");
        $("#btn_showall").addClass("active");
    } else {
        $("#li_tab3").addClass("active");
        $("#tab3").addClass("active");
        $("#btn_showall").addClass("active");
    }
});

$('#activitytab').on('click', function () {
    $(".assetDeficienciesleg").hide();
    localStorage.setItem("tabName", "activitytab");
    $("#catfilter").show();
    var index = localStorage.getItem("PageIndex");
    index = index != "" ? index : 1;
    if (!$("#partialContents").hasClass("isData")) {
        getActivityDashboard(index);
    }
});

$('#inspCalendartab').on('click', function () {
    $(".assetDeficienciesleg").hide();
    localStorage.setItem("tabName", "inspCalendartab");
    $("#catfilter").hide();
    loadInspCalender();

});

$('#inspectionSummarytab').on('click', function () {
    $(".assetDeficienciesleg").hide();
    localStorage.setItem("tabName", "inspectionSummarytab");
    $("#catfilter").show();
    $('#filterbyUser').trigger('change');
});

function SetSortingOrder(Order) {
    var sortOrder = localStorage.getItem("SortOrder");
    var orderbySort = localStorage.getItem("OrderbySort");
    if (sortOrder == Order && orderbySort == "ASC") {
        localStorage.setItem("OrderbySort", "DESC");
    } else if (sortOrder == Order && orderbySort == "DESC") {
        localStorage.setItem("OrderbySort", "ASC");
    } else {
        localStorage.setItem("SortOrder", Order);
        localStorage.setItem("OrderbySort", "ASC");
    }
    getActivityDashboard(1);
}

$("#btntxtsearch").click(function () {
    getActivityDashboard(1);
});

$("#closesearch").click(function () {
    $("#inputSuccess2").val("");
    getActivityDashboard(1);
});

function getActivityDashboard(index) {
    // debugger;   
    var catId = localStorage.getItem("dashCatId");
    var userId = $("#ddlUsers").val(); //localStorage.getItem("dashboardUserId");
    //if (userId == null || userId === ""){
    //    userId = $("#ddlUsers").val();
    //} else if (userId === 0) {
    //     userId = "";
    //}
   // debugger;
    var status = localStorage.getItem("btnValue");
    var noofdays = localStorage.getItem("Datein");
    var sortOrder = localStorage.getItem("SortOrder");
    var orderbySort = localStorage.getItem("OrderbySort");
    var duemonth = localStorage.getItem("DueMonth");
    var dueyear = localStorage.getItem("DueYear");
    var searchText = $("#inputSuccess2").val();
    var isScroll = localStorage.getItem("isScroll");
    if (sortOrder == null) {
        sortOrder = "StandardEP";
    } if (orderbySort == null) {
        orderbySort = "ASC";
    }
    //debugger;
    //if (status == null) {
    //    status = 0;
    //}
    if (noofdays === -1) {
        noofdays = null;
    } else if (noofdays > 0) {
        noofdays = noofdays;
    }
    else { noofdays = 0; }
    var url = CRxUrls.home_epsdashboard;
    $.ajax({
        url: url + '?userid=' + userId + '&page=' + index + "&sortOrder=" + sortOrder + "&OrderbySort=" + orderbySort + "&categoryId=" + catId + "&status=" + status + "&noofdays=" + noofdays + "&duemonth=" + duemonth + "&dueyear=" + dueyear + "&searchtext=" + searchText + "&isScroll=" + isScroll,
        cache: false,
        type: "GET",
        //global: false,
        success: function (data) {
            //debugger;
            $('table tr.loadmore').remove();
            if (isScroll == "false") {
                $('#partialContents').empty();
            }
            $('#partialContents').append(data);
            $('#partialContents').fadeIn("fast");
            var lbltext = "SHOW MORE RECORDS";
            //var tblRowCount = 0;
            //if () {
            //    lbltext = "No MORE RECORDS"
            //}
            $('#c_myTable tr:last').after("<tr class='loadmore'><td colspan='11'><div id='divloadmore'><button id='btnloadMore' class='showmorebtn'>" + lbltext + "</button></div></td></tr>");
            changeButtontext();
            localStorage.setItem("isScroll", false);
            sortOrder = localStorage.getItem("SortOrder");
            if (index == 1) {
                localStorage.setItem("PageIndex", 1);
            }
            var clickbtnId = localStorage.getItem("btnId");
            // console.log(clickbtnId);
            $(".button1").removeClass("active");
            if (clickbtnId != null) {
                $(`#${clickbtnId}`).addClass("active");
            } else {
                $("#btn_all").addClass("active");
            }

            orderbySort = localStorage.getItem("OrderbySort");
            $('#partialContents').addClass("isData");
            var duewithinfilter = localStorage.getItem("due_within_filter");
            if (duewithinfilter != null || duewithinfilter != undefined) {
                $("#btn_dueWithin").val("Due within " + duewithinfilter);
            } else { $("#btn_dueWithin").val("Due within Days"); }
            $(".sorting1").removeClass("sorting_asc1").removeClass("sorting_desc1");
            if (orderbySort == "ASC") {
                $("#" + sortOrder).addClass("sorting_asc1");
            } else {
                $("#" + sortOrder).addClass("sorting_desc1");
            }
            //if (noofdays != '' && noofdays != "0") {
            //    $("#c_myTable tr td:nth-child(3)").find("label").removeClass().addClass('status_due_days');
            //}
            if (catId) {
                $("input[name='dashEPCat'][value='" + catId + "']").prop('checked', true);
                localStorage.setItem("dashCatId", catId);
            }            
        }
    });


    function changeButtontext() {
        if (isShowMoreRecords.toLowerCase() == 'false') {
            $("#btnloadMore").html("NO MORE RECORDS").addClass('disable');
        }
    }
}

