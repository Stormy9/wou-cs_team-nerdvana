//=======================================================================================
// this is the code for the coverr video of the doggie drinking on front/splash page
// jQuery is required to run this code
$(document).ready(function () {
    scaleVideoContainer();

    initBannerVideoSize('.video-container .poster img');
    initBannerVideoSize('.video-container .filter');
    initBannerVideoSize('.video-container video');

    $(window).on('resize', function () {
        scaleVideoContainer();
        scaleBannerVideoSize('.video-container .poster img');
        scaleBannerVideoSize('.video-container .filter');
        scaleBannerVideoSize('.video-container video');
    });
});

function scaleVideoContainer() {
    var height = $(window).height() + 5;
    var unitHeight = parseInt(height) + 'px';
    $('.homepage-hero-module').css('height', unitHeight);
}

function initBannerVideoSize(element) {
    $(element).each(function () {
        $(this).data('height', $(this).height());
        $(this).data('width', $(this).width());
    });

    scaleBannerVideoSize(element);
}

function scaleBannerVideoSize(element) {

    var windowWidth = $(window).width(),
        windowHeight = $(window).height() + 5,
        videoWidth,
        videoHeight;

    // console.log(windowHeight);

    $(element).each(function () {
        var videoAspectRatio = $(this).data('height') / $(this).data('width');

        $(this).width(windowWidth);

        if (windowWidth < 1000) {
            videoHeight = windowHeight;
            videoWidth = videoHeight / videoAspectRatio;
            $(this).css({ 'margin-top': 0, 'margin-left': -(videoWidth - windowWidth) / 2 + 'px' });

            $(this).width(videoWidth).height(videoHeight);
        }

        $('.homepage-hero-module .video-container video').addClass('fadeIn animated');

    });

 
}

//Implementation of text area character counter.
var text_max = 999;
// TEXT AREA ONE
$('#count_message_01').html('0 / ' + text_max);

$('#text01').keyup(function () {
    var text_length = $('#text01').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_01').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA TWO
$('#count_message_02').html('0 / ' + text_max);

$('#text02').keyup(function () {
    var text_length = $('#text02').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_02').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA THREE
$('#count_message_03').html('0 / ' + text_max);

$('#text03').keyup(function () {
    var text_length = $('#text03').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_03').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA FOUR
$('#count_message_04').html('0 / ' + text_max);

$('#text04').keyup(function () {
    var text_length = $('#text04').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_04').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA FIVE
$('#count_message_05').html('0 / ' + text_max);

$('#text05').keyup(function () {
    var text_length = $('#text05').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_05').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA SIX
$('#count_message_06').html('0 / ' + text_max);

$('#text06').keyup(function () {
    var text_length = $('#text06').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_06').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA SEVEN
$('#count_message_07').html('0 / ' + text_max);

$('#text07').keyup(function () {
    var text_length = $('#text07').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_07').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA EIGHT
$('#count_message_08').html('0 / ' + text_max);

$('#text08').keyup(function () {
    var text_length = $('#text08').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_08').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA NINE
$('#count_message_09').html('0 / ' + text_max);

$('#text09').keyup(function () {
    var text_length = $('#text09').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_09').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA TEN
$('#count_message_10').html('0 / ' + text_max);

$('#text10').keyup(function () {
    var text_length = $('#text10').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_10').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA ELEVEN
$('#count_message_11').html('0 / ' + text_max);

$('#text11').keyup(function () {
    var text_length = $('#text11').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_11').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA TWELVE
$('#count_message_12').html('0 / ' + text_max);

$('#text12').keyup(function () {
    var text_length = $('#text12').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_12').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA THIRTEEN
$('#count_message_13').html('0 / ' + text_max);

$('#text13').keyup(function () {
    var text_length = $('#text13').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_13').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA FOURTEEN
$('#count_message_14').html('0 / ' + text_max);

$('#text14').keyup(function () {
    var text_length = $('#text14').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_14').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA FIFTEEN
$('#count_message_15').html('0 / ' + text_max);

$('#text15').keyup(function () {
    var text_length = $('#text15').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_15').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA SIXTEEN
$('#count_message_16').html('0 / ' + text_max);

$('#text16').keyup(function () {
    var text_length = $('#text16').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_16').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA SEVENTEEN
$('#count_message_17').html('0 / ' + text_max);

$('#text17').keyup(function () {
    var text_length = $('#text17').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_17').html(text_length + ' / ' + text_max);
});
//---------------------------------------------------------------------------------------
// TEXT AREA EIGHTEEN
$('#count_message_18').html('0 / ' + text_max);

$('#text18').keyup(function () {
    var text_length = $('#text18').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_18').html(text_length + ' / ' + text_max);
});
//=======================================================================================