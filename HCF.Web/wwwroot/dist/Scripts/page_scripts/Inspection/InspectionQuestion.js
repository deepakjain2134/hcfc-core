(function ($) {
    if (window.location.pathname == "/inspquestion") {
        var RedirectToPageUrl = 0;
    }
   
    $('[data-toggle="tooltip"]').tooltip()

    var settings;
    var currentCard;
    var prevCard = [];

    // Plugin definition.
    $.fn.decisionTree = function (options) {
        var elem = $(this);
        settings = $.extend({}, $.fn.decisionTree.defaults, options);

        elem.addClass(settings.containerClass);
        renderRecursive(settings.data, elem, "dctree-first");

        $('.dctree-prev').on('click', function () {
            $('[data-toggle="popover"]').popover('hide');
            showCard(prevCard.pop(), true);
        });

        currentCard = $('#dctree-first');
        currentCard.show();
    };


    $.fn.decisionTree.defaults = {
        data: null,
        animationSpeed: "fast",
        animation: "slide-left",
        containerClass: "dc-tree",
        cardClass: "dctree-card",
        messageClass: "dctree-message"
    };

    function renderRecursive(data, elem, id) {
        console.log(data);
        var container = $('<div></div>')
            .addClass(settings.cardClass)
            .addClass('col-xs-12');
        var message = $('<div></div>').addClass(settings.messageClass).append(data.message);

        container.append(message);

        if (id != null) {
            container.attr('id', id)
        }

        if (typeof data.decisions != "undefined") {
            var decisions = $('<div></div>').addClass('dctree-decisions');
            for (var i = 0; data.decisions.length > i; i++) {
                var decision = data.decisions[i];
                var genId = guid();
                var grid = $('<div></div>').addClass('col-md-6 col-xs-6');
                var answer = $('<div></div>')
                    .addClass("dctree-answer-" + i)
                    .append(decision.answer)
                    .attr('data-page-url', decision.redirectToPage)
                    .attr('data-route-url', decision.pageUrl)
                    .on('click', function () {
                        getNextCard(this);
                    })
                    .attr('data-dctree-targetid', genId);
                if (typeof decision.class != "undefined") {
                    answer.addClass(decision.class);
                }
                grid.append(answer);
                decisions.append(grid);
                renderRecursive(decision, elem, genId);
            }
            container.append(decisions);
        }


        if (id != 'dctree-first') {
            var controls = $('<div></div>').addClass('dctree-controls col-md-12');
            controls.append($('<a href="javascrip:;" class="dctree-prev inspbackbtn">Back</a>'));
            container.append(controls);
        }

        elem.append(container);
    }

    function getNextCard(elem) {
        var pageUrl = $(elem).data('page-url');
        var pageId = $(elem).data("route-url");
        if (RedirectToPageUrl == "0") {
            if (pageUrl) {
                $('[data-toggle="popover"]').not($('#' + pageId)).popover('hide');
                $('#' + pageId).popover('show');
                $('#' + pageId).addClass('inspActive');
                setTimeout(function () {
                    $('[data-toggle="popover"]').popover('hide');
                    $('#' + pageId).removeClass('inspActive');
                }, 5000);

            } else {
                $('[data-toggle="popover"]').popover('hide');
                var e = $(elem);
                currentCard = e.parents('.' + settings.cardClass)[0];
                prevCard.push(currentCard.id);
                var nextCard = e.attr('data-dctree-targetid');
                showCard(nextCard);
            }
        }
        else {

            if (pageUrl) {
                debugger
                //str = pageUrl.replace('"', '');

                window.location.pathname = pageUrl;

            } else {
                //$('[data-toggle="popover"]').popover('hide');
                var e = $(elem);
                currentCard = e.parents('.' + settings.cardClass)[0];
                prevCard.push(currentCard.id);
                var nextCard = e.attr('data-dctree-targetid');
                showCard(nextCard);
            }
        }
        
    }

    function showCard(id, backward) {
        console.log(id, backward);
        var nextCard = $("#" + id);

        if (settings.animation == 'slide') {
            $(currentCard).slideUp(settings.animationSpeed, function () {
                nextCard.slideDown(settings.animationSpeed);
            });
        } else if (settings.animation == 'fade') {
            $(currentCard).fadeOut(settings.animationSpeed, function () {
                nextCard.fadeIn(settings.animationSpeed);
            });
        } else if (settings.animation == 'slide-left') {
            var left = { left: "-100%" };
            var card = $(currentCard);

            if (backward) {
                left = { left: "100%" };
            }
            card.animate(left, settings.animationSpeed, function () {
                card.hide();
            });

            if (nextCard.css('left') == "-100%" || nextCard.css('left') == "100%") {
                left.left = 0;
                nextCard.show().animate(left, settings.animationSpeed);
            } else {
                nextCard.fadeIn(settings.animationSpeed);
            }
        }

        currentCard = nextCard;
    }

    function guid() {
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
            var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
            return v.toString(16);
        });
    }
    // End of closure.
})(jQuery);


var data = {
    message: "<div class='inspmessageicon'><img class='' alt='Message icon' src='../dist/Images/Icons/inspmessageicon.png'></div><div class=''>Are you completing inspection in CRx or <br>attaching an outside report?</div>",
    decisions: [
        {
            answer: "I am inspecting in CRx",
            class: "blue",
            redirectToPage: "",
            message: "<div class='inspmessageicon'><img class='' alt='Message icon' src='../dist/Images/Icons/inspmessageicon.png'></div>Are you inspecting on a route? (applicable to Fire Extinguishers, Eyewash Stations, Exit Signs, Battery Operated Lights, and Line Isolation Monitors)",
            decisions: [
                {
                    answer: "No",
                    class: "orange",
                    message: "<div class='inspmessageicon'><img class='' alt='Message icon' src='../dist/Images/Icons/inspmessageicon.png'></div>Do you need to refer to a life safety drawing or do you know the asset ID?",
                    decisions: [
                        {
                            answer: "I need a life safety drawing",
                            class: "blue",
                            message: "This is the end !",
                            redirectToPage: 'Assets/InsAssets',//'@Url.RouteUrl("insassets")',// '@Url.Action("InsAssets", "Assets", new {area = ""})',
                            pageUrl: 'insassets'
                        },
                        {
                            answer: "I know the asset ID",
                            class: "orange",
                            message: "This is the end !",
                            redirectToPage: 'Assets/Inspectionbybarcode',
                            pageUrl: 'inspectionbybarcode'
                        }
                    ]
                },
                {
                    answer: "Yes",
                    class: "blue",
                    message: "This is the end !",
                    redirectToPage: 'inspection/rbi',//'@Url.Action("routeInspection", "Building", new {area = "RoundInsp"})',
                    pageUrl: 'routeInspection'
                }
            ]
        },
        {
            answer: "I am attaching a report",
            class: "orange",
            message: "You are wrong !",
            redirectToPage: 'Assets/Inspection', // '@Url.Action("Inspection", "Assets", new {area = ""})',
            pageUrl: 'inspection'
        }
    ]
};

$(document).ready(function () {
    $('.divmain').decisionTree(
        { data: data }
    );
});
