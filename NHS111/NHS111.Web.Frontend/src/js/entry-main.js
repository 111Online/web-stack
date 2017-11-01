import './vendor/jquery.cookie.js'
import './vendor/jquery.validate.js'
import './vendor/jquery.validate.unobtrusive.js'
import './vendor/jquery.unobtrusive-ajax.js'
import './vendor/jquery.ns-autogrow.js'

//global.validation = require('./validation')
global.geolocation = require('./geolocation')


jQuery.validator.setDefaults({
    ignore: ":hidden",
    focusInvalid: false,
    showErrors: function (errorMap, errorList) {
        // This is a modified version of validate.unobtrusive's default error summary
        // it adds links to the error fields

        var container = $(this.currentForm).find("[data-valmsg-summary=true]"),
            list = container.find("ul"),
            elements = this.invalidElements()

        if (list && list.length && elements.length) {
            list.empty()

            $.each(elements, function () {
                var containerID = this.id //$(this.element).attr('name').replace(/\./g, '_') || this.element.id
                //if ($("#container_" + containerID).length > 0) containerID = "container_" + containerID
                var title = ($(this).attr('type') == 'radio' || $(this).attr('type') == 'checkbox') ? $(this).parents("fieldset").children("legend")[0].innerText : $(this).siblings("label")[0].innerText
                $("<li />").html(`<a href="#${containerID}">${title}</a>`).appendTo(list)
            })
        }
        else {
            container.addClass("validation-summary-valid").removeClass("validation-summary-errors")
            container[0].removeAttribute("role")
        }
        //this.defaultShowErrors()
    },
    submitHandler: function (form) {
        var container = $(this.currentForm).find("[data-valmsg-summary=true]")

        container.addClass("validation-summary-errors").removeClass("validation-summary-valid")
        container.attr("role", "alert")

        $(`.validation-summary-errors [href]:first-child`).focus()
        form.submit()
    },
    highlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").addClass("form-group-error")
    },
    unhighlight: function (element, errorClass, validClass) {
        $(element).closest(".form-group").removeClass("form-group-error")
    }
})
