
const highlight = function (element, errorClass, validClass) {
  var name = $(element).attr("name")
  $(element).attr("aria-invalid", "true")

  var formGroup = $(element).closest(".form-group")
  $(formGroup).addClass("form-group-error")
  $(`.error-message, [data-valmsg-for="${name}"]`, formGroup)
    .attr("role", "alert")
    .removeClass("field-validation-valid")
    .addClass("field-validation-error")
  removeAriaDescribedBy.bind(this)(element)

  // Make sure all shared radio/checkboxes are invalid too
  $(`[name="${name}"]`).attr("aria-invalid", "true")
}

const unhighlight = function (element, errorClass, validClass) {
  var name = $(element).attr("name")
  $(element).removeAttr("aria-invalid").removeClass("input-validation-error")

  var formGroup = $(element).closest(".form-group")
  $(formGroup).removeClass("form-group-error")
  $(`[data-valmsg-for="${name}"]`, formGroup)
    .removeClass("field-validation-error")
    .addClass("field-validation-valid")
    .removeAttr("role")


  // Make sure all shared radio/checkboxes are valid too
  $(`[name="${name}"]`).removeAttr("aria-invalid")
}

// Override default errorsFor so the selector used can be more specific to our approach
jQuery.validator.prototype.errorsFor = function (element) {
  var name = this.escapeCssMeta(this.idOrName(element)),
    describer = $(element).attr("aria-describedby"),
    selector = `[data-valmsg-for="${name}"]`;

  // If used, 'aria-describedby' should directly reference the error element
  if (describer) {
    var describerReference = "#" + this.escapeCssMeta(describer).replace(/\s+/g, ", #")
    selector = selector + ", " + describerReference;
  }

  return this
    .errors()
    .filter(selector);
}

const removeAriaDescribedBy = function (element) {
  var describer = $(element).attr("aria-describedby")
  if (describer) {
    // The aria-describedby is used when there is no label for error.
    // It is default part of the validation library but not something we want to use, as we use a combination of
    // normal non-error labels, role=alert on the error message and aria-invalid on the field so this isn't required.
    // Some places will use this behaviour by default but usually we will want to remove it.
    // So this removes the reference if the element it refers to is missing (ie. a broken aria reference)
    var describerReference = "#" + this.escapeCssMeta(describer).replace(/\s+/g, ", #")
    if ($(describerReference).length === 0) {
      $(element).removeAttr("aria-describedby")
    }
  }

}

jQuery.validator.setDefaults({
  ignore: "[type='hidden']:not(.validate-hidden)",
  focusInvalid: false,
  errorElement: "span",
  errorClass: "field-validation-error",
  showErrors: function (errorMap, errorList) {
    var self = this
    var container = $(this.currentForm).find("[data-valmsg-summary=true]")

    // If summary container isn't used, it will just handle attributes
    if (!container.length) return this.defaultShowErrors()

    // This is a modified version of validate.unobtrusive's default error summary
    // it adds links to the error fields
    var list = container.find(".js-error-list-original").hide(),
      elements = this.elements().toArray(),
      newList = container.find(".js-error-list").empty().show(),
      invalid = self.invalid

    container.hide()

    container.removeAttr("role")

    elements.forEach((val) => {
      var containerID = val.id
      var title = ($(val).attr('type') == 'radio' || $(val).attr('type') == 'checkbox') ? $(val).parents("fieldset").children("legend")[0].innerText : $(val).siblings("label")[0].innerText
      if (this.invalid[val.name]) $("<li />").html(`<a href="#${containerID}">${title}</a>`).appendTo(newList)
    }, this)

    if (!Object.keys(invalid).length) { // if valid, hide error summary 
      container.addClass("validation-summary-valid").removeClass("validation-summary-errors")
    }
    else { // if not valid, add class (which is then used to trigger focus and alert screenreaders)
      container.addClass("validation-summary-errors").removeClass("validation-summary-valid")
    }
    this.defaultShowErrors()
  },
  highlight: highlight,
  unhighlight: unhighlight,
})

window.AddAdapter = function (name, param) {
  jQuery.validator.unobtrusive.adapters.add(name, [param], function (options) {
    if (!options.rules[name]) options.rules[name] = {}
    if (param) options.rules[name][param] = options.params[param]

    if (options.message != null) {
      options.messages[name] = options.message
    }
  });
}

jQuery(document).ready(function () {
  // Validation for number only fields
  var lastKey = null
  $(".js-validate-number").on("keydown", function (event) {
    var key = event.key || String.fromCharCode(event.keyCode)
    // When a key is down, it checks that you aren't typing a letter. This allows numbers and tab/delete etc
    if (lastKey == "Meta" || lastKey == "Control") return lastKey = key
    else if (/^[a-zA-Z\D]$/.test(key)) event.preventDefault()
    return lastKey = key
  })

  $(".js-validate-number").on("keyup", function (event) {
    if (lastKey == "Meta" || lastKey == "Control") return lastKey = null
  })

  $("main form button[type=submit]").on("click", function (e) {
    $(this).closest("form").valid()
  })

  $("main form").on("submit", function (e) {
    var firstError = document.querySelector("#personalDetailForm .field-validation-error, #personalDetailForm .form-group-error")
    if (firstError) {
      // This is scoped purely to the long personal details page, will work better on some browsers than others
      firstError.scrollIntoView({
        "behavior": "smooth",
        "block": "center"
      })
    }
    else {
      const container = $(".validation-summary-errors")
      if (container.length) { // if it isn't valid, make sure screenreaders get alerted to the box
        $("[role='alert']").removeAttr("role")
        container.show()
        $(".validation-summary-errors a").first().focus()
        $(".validation-summary-errors h2").attr("role", "alert")
      }
    }
  })

})
