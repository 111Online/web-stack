const autosize = require('autosize')

jQuery(function () {
  var maxLength = $('.feedback__input').attr('maxlength')

  // If JS is not enabled, maxlength ensures the length of the text
  // but if JS is enabled then we remove it so the user can type as many
  // characters as they wish and then edit to be less than maximum.
  $('.feedback__input').removeAttr('maxlength')
  $('.feedback__input').on('keyup input', function () {
    var length = $(this).val().length
    var length = maxLength - length
    if (length >= 0) $('.feedback__counter').removeClass('feedback__counter--error').text(length + ' characters remaining')
    else $('.feedback__counter').addClass('feedback__counter--error').text(Math.abs(length) + ' characters too many')
  })
  console.log($('.feedback__input').autogrow)
  autosize($('.feedback__input'))

  $('.js-open-feedback').on('click', function(e) {
      $('.feedback details').attr('open', true)
  })
})
