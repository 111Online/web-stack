global.$ = global.jQuery = require('jquery')

window.EventTypes = { Clicked: "Clicked", Error: "Error" }

function initEventHandlers() {
    $(document).on("click", "[data-event-trigger='click']", function () {
       window.logEvent(window.EventTypes.Clicked, $(this).attr("data-event-value"))
    })
}

window.createAuditEntry = function(eventKey, eventValue) {
    return {
      "sessionId": $.cookie("nhs111-session-id"),
      "journeyId": $("#journeyId").value,
      "eventKey": eventKey,
      "eventValue": eventValue,
      "page": location.pathname
  }
}

window.logEventEntry = function(auditEntry) {
    $.post("/Auditing/Log", auditEntry)
}

window.logEvent = function (key, value) {
  var auditEntry = window.createAuditEntry(key, value)
  window.logEventEntry(auditEntry)
}


initEventHandlers()
