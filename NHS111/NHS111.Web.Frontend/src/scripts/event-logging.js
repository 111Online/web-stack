global.$ = global.jQuery = require('jquery')

window.EventTypes = { Clicked: "Clicked", Error: "Error" }

function initEventHandlers() {
    $(document).on("click", "[data-event-trigger='click']", function () {
       logEvent(EventTypes.Clicked, $(this).attr("data-event-value"))
    })
}

function createAuditEntry(eventKey, eventValue) {
    var date = new Date()
    return {
      "TIMESTAMP": date.toISOString(),
      "sessionId": $.cookie("nhs111-session-id"),
      "journey": "{\"steps\":[]}",
      "answerTitle": "",
      "answerOrder": "",
      "questionTitle": "",
      "questionNo": "",
      "questionId": "",
      "dxCode": "",
      "eventData": "",
      "eventKey": eventKey,
      "eventValue": eventValue,
      "page": location.pathname,
      "gender": "",
      "dosRequest": "",
      "dosResponse": "",
      "itkRequest": "",
      "itkResponse": ""
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
