global.$ = global.jQuery = require('jquery')

window.EventTypes = { Clicked: "Clicked", Error: "Error" }

function initEventHandlers() {

  // Add click event handler on links that don't want to open in a new window. This hijacks
  // the click event, do the logging and then continue with the click by redirecting
  $(document).on("click", "[data-event-trigger='click']a:not([target=_blank])", function (e) {
    e.preventDefault()
    const target = $(this)
    const value = target.attr("data-event-value")
    const href = target.prop("href")

    if (href) {
      window.logEvent(window.EventTypes.Clicked, value).always(() => {
        window.location = href
      })
    }
  })

  // Add click event handler on anchors which open in new windows and other non-link elements
  //(such as reveals and buttons) and log normally. Links wanting to open in new window should log
  // successfully as original window is still open
  $(document).on("click", "a[target=_blank][data-event-trigger='click'], [data-event-trigger='click']:not(a)", function () {

    window.logEvent(window.EventTypes.Clicked, $(this).attr("data-event-value"))

  })
}

window.createAuditEntry = function (eventKey, eventValue) {
  return {
    "sessionId": $.cookie("nhs111-session-id"),
    "journeyId": $("#journeyId").value,
    "eventKey": eventKey,
    "eventValue": eventValue,
    "page": location.pathname
  }
}

window.logEventEntry = function (auditEntry) {
  return $.ajax({
    method: "POST",
    url: "/Auditing/Log",
    data: auditEntry,
    timeout: 200 // Short timeout, to ensure user is not slowed down
  })
}

window.logEvent = function (key, value) {
  var auditEntry = window.createAuditEntry(key, value)
  return window.logEventEntry(auditEntry)
}

initEventHandlers()
