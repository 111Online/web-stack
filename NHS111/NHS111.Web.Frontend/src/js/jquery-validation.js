
jQuery.validator.setDefaults({
    ignore: "[type='hidden']:not(.validate-hidden)",
    focusInvalid: false,
    showErrors: function (errorMap, errorList) {
        // This is a modified version of validate.unobtrusive's default error summary
        // it adds links to the error fields
        var self = this
        var container = $(this.currentForm).find("[data-valmsg-summary=true]")
        if (!container.length) return this.defaultShowErrors()

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
    highlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").addClass("form-group-error")
        $(element).siblings(".error-message").attr("role", "alert")
        $(element).attr("aria-invalid", "true")
        $(`[name="${$(element).attr("name")}"]`).attr("aria-invalid", "true")
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group:not(.form-group-validation-override)").removeClass("form-group-error")
        $(element).siblings(".error-message").removeAttr("role")
        $(element).removeAttr("aria-invalid")
        $(`[name="${$(element).attr("name")}"]`).removeAttr("aria-invalid")
    }
})

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
        $("[role='alert']").removeAttr("role")
        container.show()
        if (container.length) { // if it isn't valid, make sure screenreaders get alerted to the box
          setTimeout(() => {
            $(".js-error-list li:first-child a").focus()
            container.attr("role", "alert").attr("aria-live", "assertive")
            $("[role='alert']").removeAttr("role").removeAttr("aria-live")
          }, 100)
        }
      }
    })
})
