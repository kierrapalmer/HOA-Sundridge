//Edit Owner info
$(".edit-owner-info").click(function () {
    $(".owner-info-input").prop("readonly", false);
    $(".owner-info-select").prop("disabled", false);
    $(".owner-info-save").show();
    $(".owner-info-input").removeClass("no-border");
    $(".owner-info-select").removeClass("no-border");
});

$("#edit-owner-profile").click(function () {
    $(".owner-profile-input").prop("readonly", false);
    $(".owner-profile-select").prop("disabled", false);
    $(".owner-profile-save").show();
    $(".owner-profile-input").removeClass("no-border");
    $(".owner-profile-select").removeClass("no-border");
});

var timer;
var interval = 1000;
var search = $('.search');

/*-------search auto submit --------*/
//starts the countdown
search.on('keyup',
    function () {
        clearTimeout(timer);
        timer = setTimeout(doneTyping, interval);
    });

//clears the countdown
search.on('keydown',
    function (event) {
        if (event.which === 13) {
            event.preventDefault();
            doneTyping();
        }
        clearTimeout(timer);
    });

//submits the form when timer is exceeded
function doneTyping() {
    $(".searchForm").submit();
}

$(window).resize(function () {
    truncate();
});
$(document).ready(function () {
    truncate();
});
function truncate() {
    $(".truncate").each(function (index) {
        var parent = $(this).width();
        var child = $(this).find("span").width();

        if (child > parent) {
            var test = $('.moreDesc').get(index);
            $(test).removeClass("hidden"); //show more button
        }
    });
}

$(document).on("click",
    ".moreDesc",
    function () {
        var id = $(this).data('id');
        var text = $("#desc" + id).text();
        $(".modal-body #more").text(text);
    });