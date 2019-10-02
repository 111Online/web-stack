const autosize = require('autosize')

jQuery(function () {
  var maxLength = parseInt($('.feedback__input').attr('maxlength'))
  $('.feedback__submit').prop('disabled', 'true')

  // If JS is not enabled, maxlength ensures the length of the text
  // but if JS is enabled then we adjust it so the user can type as many
  // characters as they wish and then edit to be less than maximum.
  // There is still a max length for typing but much higher than the length for
  // submitting.
  $('.feedback__input').attr('maxlength', maxLength * 1.5)
  $('.feedback__input').on('keyup input', function () {
    var length = $(this).val().length
    var length = maxLength - length
    if (length >= 0) {
        $('.feedback__counter').removeClass('feedback__counter--error').text(length + ' characters remaining')
        $('.feedback__submit').removeAttr('disabled')
    }
    else {
        $('.feedback__counter').addClass('feedback__counter--error').text(Math.abs(length) + ' characters too many')
        $('.feedback__submit').prop('disabled', 'true')
    }
    if (length == maxLength) $('.feedback__submit').prop('disabled', 'true')
  })
  autosize($('.feedback__input'))

  function addHandler() {
    $('.feedback__submit').one('click', function (e) {
      var el = $('.feedback__input')
      var length = parseInt(el.val().length)
      if (length <= 0 || length > maxLength) {
        addHandler()
        return e.preventDefault()
      }
    })
  }

  addHandler()

  $('.js-open-feedback').on('click', function(e) {
      var isOpen = $('.feedback details[open]').length
      if (!isOpen) $('.feedback summary').click()
  })
})
