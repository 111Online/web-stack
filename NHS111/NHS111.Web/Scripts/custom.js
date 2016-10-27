$(document).ready(function () {


    // hide additional questions by default for js disabled feature

    $(".moreQuestions").hide();
    $(".hiddenHelp").hide();




    //book appointment page click action
    $("#medCentres li").click(function () {
        $("#medCentres li.selected").removeClass("selected");
        $(this).addClass("selected");
    });

    $("#timePicker li.available").click(function () {
        $("#timePicker li.selected").removeClass("selected");
        $(this).addClass("selected");
    });


    // disable click on unavailable list
    $('#timePicker li.unavailable').unbind('click');

    // timePicker slot picker display mock
    $('#one').click(function () {
        $('#selectedSlot').text('20:00');
    });
    $('#two').click(function () {
        $('#selectedSlot').text('21:15');
    });
    $('#three').click(function () {
        $('#selectedSlot').text('23:15');
    });

    // pathway accordion 

    $(function () {
        $("#pathways #pathwayAccordion").accordion({
            heightStyle: "content"
        });

    });

    // show/hide additional questions

    $("#feverYes").click(function () {
        $("#feverYesMore").show('fast');

    });

    $("#feverNo").click(function () {
        $("#feverYesMore").hide('fast');

    });

    // show/hide hidden help

    $(".helpNightSweat").click(function () {
        $("#nightSweats").slideToggle('slow');

    });

    $(".helpUnwellAlways").click(function () {
        $("#unwellAlways").slideToggle('slow');

    });




    // Pathways & JTBS page
    // Submit buttons disabled by default
    $("#jtbs button[type=submit]").attr('disabled', 'disabled');
    $("#jtbs #nextScreen[type=submit]").attr('disabled', 'disabled');
    $("#pathwayQuestions #nextScreen[type=submit]").attr('disabled', 'disabled');
    $("#submitGender[type=submit]").attr('disabled', 'disabled');
    $("#timedQuestion #nextScreen[type=submit]").attr('disabled', 'disabled');




    // enable click when the following occurs
    $("input.radio").click(function () {
        $("button[type=submit]").removeAttr('disabled');
        $("input[type=submit]").removeAttr('disabled');
    });

    $("#jtbs label").click(function () {
        $("button[type=submit]").removeAttr('disabled');
        $("input[type=submit]").removeAttr('disabled');
    });

    $("#jtbs .iCheck-helper").click(function () {
        $("button[type=submit]").removeAttr('disabled');
        $("input[type=submit]").removeAttr('disabled');
    });

    // mobile scroll button scripts
    $(function () {
        var $elem = $('#wrapper');

        $('#nav_up').fadeIn('slow');
        $('#nav_down').fadeIn('slow');

        $(window).bind('scrollstart', function () {
            $('#nav_up,#nav_down').stop().animate({ 'opacity': '0.2' });
        });
        $(window).bind('scrollstop', function () {
            $('#nav_up,#nav_down').stop().animate({ 'opacity': '1' });
        });

        $('#nav_down').click(
            function (e) {
                $('html, body').animate({ scrollTop: $elem.height() }, 800);
            }
        );
        $('#nav_up').click(
            function (e) {
                $('html, body').animate({ scrollTop: '0px' }, 800);
            }
        );
    });






});

