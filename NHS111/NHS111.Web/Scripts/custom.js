$(document).ready(function () {


// hide additional questions by default for js disabled feature

$(".moreQuestions").hide(); 
$(".hiddenHelp").hide();




//book appointment page click action
$("#medCentres li").click(function() {
    $("#medCentres li.selected").removeClass("selected");
    $(this).addClass("selected");
});

$("#timePicker li.available").click(function() {
    $("#timePicker li.selected").removeClass("selected");
    $(this).addClass("selected");
});


// disable click on unavailable list
$('#timePicker li.unavailable').unbind('click'); 

// timePicker slot picker display mock
$('#one').click(function(){
     $('#selectedSlot').text('20:00');
});
$('#two').click(function(){
     $('#selectedSlot').text('21:15');
});
$('#three').click(function(){
     $('#selectedSlot').text('23:15');
});

// pathway accordion 

$(function() {
	$("#pathways #pathwayAccordion").accordion({
	heightStyle: "content"
	});
	
});

// show/hide additional questions

$("#feverYes").click(function() {
	$("#feverYesMore").show('fast');						  
							  
	});

$("#feverNo").click(function() {
	$("#feverYesMore").hide('fast');						  
							  
	});

// show/hide hidden help

$(".helpNightSweat").click(function() {
	$("#nightSweats").slideToggle('slow');						  
							  
	});

$(".helpUnwellAlways").click(function() {
	$("#unwellAlways").slideToggle('slow');						  
							  
	});


// Search Page - Disclaimer slide scripts

$(function () {
    $('#outerContent').addClass('jsEnabledExpand');

    
});

//on page load show disclaimer and block rest of the page
$('.jsEnabledExpand #disclaimer').css({"display": "block"});

$('.jsEnabledExpand #disclaimer').slideDown(500);

$('#outerContent').block({ message: null });
   
$('.closeDisclaimer').click(function () {
   $('#disclaimer').slideUp(500);
   $('#outerContent').unblock();
});

$("input.go").attr("disabled", "disabled");

//Disclaimer help scripts

    // prevent links from jumping to top of page
$('#disclaimerHelp a').click(function (e) {
    // Cancel the default action
    e.preventDefault();
});

// hide disclaimer help by default
$("#disclaimerHelp .helper").hide();

// Disclaimer help show & hide
$("#chestPain").click(function () {
   
    $("#chestPain .helper").toggleClass("active");
   $("#chestPain .helper").slideToggle();

});

$("#stroke").click(function () {

   $("#stroke .helper").toggleClass("active");
    $("#stroke .helper").slideToggle('500');

});

$("#chocking").click(function () {

    $("#chocking .helper").toggleClass("active");
    $("#chocking .helper").slideToggle('500');

});

$("#bleeding").click(function () {

    $("#bleeding .helper").toggleClass("active");
    $("#bleeding .helper").slideToggle('500');

});

$("#injury").click(function () {

    $("#injury .helper").toggleClass("active");
    $("#injury .helper").slideToggle('500');

});


$("#seizure").click(function () {

    $("#seizure .helper").toggleClass("active");
   $("#seizure .helper").slideToggle('500');

});

// Pathways & JTBS page
// Submit buttons disabled by default
$("#jtbs button[type=submit]").attr('disabled', 'disabled');
$("#jtbs #nextScreen[type=submit]").attr('disabled', 'disabled');
$("#pathwayQuestions #nextScreen[type=submit]").attr('disabled', 'disabled');
$("#genderContent #nextScreen[type=submit]").attr('disabled', 'disabled');
$("#timedQuestion #nextScreen[type=submit]").attr('disabled', 'disabled');




 // enable click when the following occurs
//$("form").keyup(function () { 
//    $("button[type=submit]").removeAttr('disabled');
//    $("input[type=submit]").removeAttr('disabled');
//});
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

// check if value is entered before enabling button    
$('#gender .largeInputBox').bind('input', function() {

    $("button[type=submit]").removeAttr('disabled');
    $("input[type=submit]").removeAttr('disabled');
});

$("#gender .range-handle").click(function () {
    $("button[type=submit]").removeAttr('disabled');
    $("input[type=submit]").removeAttr('disabled');
});

$("#timedQuestion label").click(function () {
    $("button[type=submit]").removeAttr('disabled');
    $("input[type=submit]").removeAttr('disabled');
});

// Gender page scripts

// focus on age input box
$(".largeInputBox").focus();

// check for numerics in input field & alert error
$("#age").keypress(function (e) {
    //if the letter is not digit then display error and don't type anything
    if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
        //display error message
        $("#errmsg").html("Numerics only").show().fadeOut("slow");
        return false;
    }
})


// Hide age input on page load
$("#gender #columnOne").addClass("hideInput");

$("img.maleIcon").click(function () {
    $("img.femaleIcon").removeClass("selected");
    $(this).addClass("selected");

    // age input transition
    $("#columnOne").removeClass("hideInput");
    $(".largeInputBox").focus();

});

$("img.femaleIcon").click(function () {
    $("img.maleIcon").removeClass("selected");
    $(this).addClass("selected");

    // age input transition
    $("#genderContent #columnOne").removeClass("hideInput");
    $(".largeInputBox").focus();
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

