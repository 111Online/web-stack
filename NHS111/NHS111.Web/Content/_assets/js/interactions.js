$(function() {

  var isMobile = {
    Android: function() {
        return navigator.userAgent.match(/Android/i);
    },
    BlackBerry: function() {
        return navigator.userAgent.match(/BlackBerry/i);
    },
    iOS: function() {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i);
    },
    Opera: function() {
        return navigator.userAgent.match(/Opera Mini/i);
    },
    Windows: function() {
        return navigator.userAgent.match(/IEMobile/i);
    },
    any: function() {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Opera() || isMobile.Windows());
    }
  };

  if (isMobile.any()) {
    FastClick.attach(document.body);

    $('input[autofocus]').removeAttr('autofocus');
  }

  function isAndroid() {
    var nua = navigator.userAgent,
        isAndroid = (nua.indexOf('Mozilla/5.0') > -1 && nua.indexOf('Android ') > -1 && nua.indexOf('AppleWebKit') > -1 && nua.indexOf('Chrome') === -1);
    if (isAndroid) {
      $('html').addClass('android-browser');
    }
  }

  isAndroid();


  function slidePageIn() {
    var viewportHeight = $(window).innerHeight();

    // $('.nhs-animatehtml').css(
    //                           {
    //                             'overflow-y': 'hidden',
    //                             '-ms-overflow-style': 'none'
    //                           });

    $('.nhs-animate-inout').show();

    $('.nhs-animate-inout').animate({'margin-top': '0'}, 1000);

    // setTimeout(function() {
    //   $('.nhs-animatehtml').css(
    //                           {
    //                             'overflow-y': 'scroll',
    //                             '-ms-overflow-style': 'scrollbar'
    //                           });
    // }, 1000);
  }

  if($('html').is('.not-ie8')){slidePageIn();}

  $('.not-ie8 .nhs-nav-btn').on('click', function(e) {
    var $this = $(this),
        $thisHref;

    e.preventDefault();

    if($this.is('a')) {
      $thisHref = $(this).attr('href');

      setTimeout(function(){
        window.location = $thisHref;
      }, 1000);
    } else {
      setTimeout(function(){
        $this.closest('form').submit();
      }, 1000);
    }

    $('.nhs-body-container').animate({'margin-top': '1000px'}, 1000);

  });

});