global.$ = global.jQuery = require('jquery')

window.EventTypes = { Clicked: "Clicked", Error: "Error" }

function initEventHandlers() {
    $(document).on("click", "[data-event-trigger='click']", function () {
       logEvent(EventTypes.Clicked, $(this).attr("data-event-value"))
    })
}

function createAuditEntry(eventKey, eventValue) {
    return {
      "sessionId": $.cookie("nhs111-session-id"),
      "journeyId": $("#journeyId").value,
      "eventKey": eventKey,
      "eventValue": eventValue,
      "page": location.pathname
  }
}

function logEventEntry(auditEntry) {
    $.post("/Auditing/Log", auditEntry)
}

function logEvent(key, value) {
  var auditEntry = createAuditEntry(key, value)
  logEventEntry(auditEntry)
}


initEventHandlers()
