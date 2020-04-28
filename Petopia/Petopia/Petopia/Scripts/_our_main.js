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


//FullCalendar Scripts

var today = new Date();
var todaysDate = today.getFullYear() + '-' + (today.getMonth() + 1) + '-' + today.getDate();
$(window).on('load', function(){
    // page is now ready, initialize the calendar...
//var calendarEl = $('#calendar');
//var calendarEl = document.getElementById('calendar');
  $('#calendar').fullCalendar({
        header: {
            left: 'prev,next,today',
            center: 'addEventButton,title',
            right: 'month,agendaWeek,agendaDay'
        },
        defaultDate: this.todaysDate,
        weekends: true,
        customButtons: {
          addEventButton: {
            text: 'add event...',
            click: function() {
                var dateStr = prompt('Enter a date in YYYY-MM-DD format');
                //var startDate = prompt('Enter a date in YYYY-MM-DD format');
                //var startTime = prompt('Enter time in 00:00 format');
                //var endTime = prompt('Enter time in 00:00 format');
                var eventDate = new Date(dateStr + 'T00:00:00'); // will be in local time
                //var date = moment(startDate + startTime);

                if (eventDate.isValid()) { // valid?
                    $('#calendar').fullCalendar('renderEvent', {
                        title: 'TEST DYNAMIC EVENT',
                        start: eventDate,
                        //end: endTime,
                        allDay: false
                    });
                    alert('Great. Now, update your database...');
                } else {
                    alert('Invalid date.');
                }
            }
          }
        }
  });
    //calendar.render();
});



/*
$(window).on('load', function () {
    // page is now ready, initialize the calendar...
    $('#calendar').fullCalendar({
    defaultView: 'dayGridMonth',
    header: {
    center: 'addEventButton'
},
customButtons: {
    addEventButton: {
        text: 'add event...',
            click: function() {
                var dateStr = prompt('Enter a date in YYYY-MM-DD format');
                var date = new Date(dateStr + 'T00:00:00'); // will be in local time

                if (!isNaN(date.valueOf())) { // valid?
                    calendar.addEvent({
                        title: 'dynamic event',
                        start: date,
                        allDay: true
                    });
                    alert('Great. Now, update your database...');
                } else {
                    alert('Invalid date.');
                }
            }
    }
}
    });

//calendar.render();
  });
*/



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
// TEXT AREA NINETEEN
$('#count_message_19').html('0 / ' + text_max);

$('#text19').keyup(function () {
    var text_length = $('#text19').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_19').html(text_length + ' / ' + text_max);
});
//=======================================================================================
// TEXT AREA TWENTY
$('#count_message_20').html('0 / ' + text_max);

$('#text20').keyup(function () {
    var text_length = $('#text20').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_20').html(text_length + ' / ' + text_max);
});
//=======================================================================================
// TEXT AREA TWENTYONE
$('#count_message_21').html('0 / ' + text_max);

$('#text21').keyup(function () {
    var text_length = $('#text21').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_21').html(text_length + ' / ' + text_max);
});
//=======================================================================================
// TEXT AREA TWENTYTWO
$('#count_message_22').html('0 / ' + text_max);

$('#text22').keyup(function () {
    var text_length = $('#text22').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_22').html(text_length + ' / ' + text_max);
});
//=======================================================================================
// TEXT AREA TWENTYTHREE
$('#count_message_23').html('0 / ' + text_max);

$('#text23').keyup(function () {
    var text_length = $('#text23').val().length;
    var text_remaining = text_max - text_length;

    $('#count_message_23').html(text_length + ' / ' + text_max);
});
//=======================================================================================