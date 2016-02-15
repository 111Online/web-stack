

$(document).ready(function() {
// Search Page - Disclaimer slide scripts

    $(function() {
        $('#outerContent').addClass('jsEnabledExpand');


    });

//on page load show disclaimer and block rest of the page
    $('.jsEnabledExpand #disclaimer').css({ "display": "block" });

    $('.jsEnabledExpand #disclaimer').slideDown(500);

    $('#outerContent').block({ message: null });

    $('.closeDisclaimer').click(function() {
        $('#disclaimer').slideUp(500);
        $('#outerContent').unblock();
    });

    $("input.go").attr("disabled", "disabled");

//Disclaimer help scripts

// prevent links from jumping to top of page
    $('#disclaimerHelp a').click(function(e) {
        // Cancel the default action
        e.preventDefault();
    });

// hide disclaimer help by default
    $("#disclaimerHelp .helper").hide();

// Disclaimer help show & hide
    $("#chestPain").click(function() {

        $("#chestPain .helper").toggleClass("active");
        $("#chestPain .helper").slideToggle();

    });

    $("#stroke").click(function() {

        $("#stroke .helper").toggleClass("active");
        $("#stroke .helper").slideToggle('500');

    });

    $("#chocking").click(function() {

        $("#chocking .helper").toggleClass("active");
        $("#chocking .helper").slideToggle('500');

    });

    $("#bleeding").click(function() {

        $("#bleeding .helper").toggleClass("active");
        $("#bleeding .helper").slideToggle('500');

    });

    $("#injury").click(function() {

        $("#injury .helper").toggleClass("active");
        $("#injury .helper").slideToggle('500');

    });


    $("#seizure").click(function() {

        $("#seizure .helper").toggleClass("active");
        $("#seizure .helper").slideToggle('500');

    });
});