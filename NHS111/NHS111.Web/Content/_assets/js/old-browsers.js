$(function() {

  //-- Faking details behaviour

  $('.no-details').on('click keypress', 'summary', function(e) {
    var $this = $(this);
    if (e.which === 13 || e.type === 'click') {
      $this.parent().toggleClass('open');
    }
  });


  $('.lt-ie9 .nhs-radiobtn__input').on('change', function() {
    var $this = $(this)[0];

    $(this).addClass('ie8-checked');
  });

});