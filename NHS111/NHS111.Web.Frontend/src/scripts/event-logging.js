

const eventTypes = { Clicked: "Clicked" }

function initEventHandlers() {
    $(document).on("click", "[data-event-trigger='click']", function () {
        var auditEntry = createAuditEntry(eventTypes.Clicked, $(this).attr("data-event-value"))
        logEvent(auditEntry)
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

function logEvent(auditEntry) {
    $.post("/Auditing/Log", auditEntry)
}

initEventHandlers()
