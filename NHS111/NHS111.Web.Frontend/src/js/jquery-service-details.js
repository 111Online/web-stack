/**
 * This file has the JavaScript for the other services page's maps.
 * entry-map.js is the file for the map itself
 */
jQuery(function () {

    // This ensures the map loads properly and that JS is enabled
    var serviceListings = $("details:not([open])").has(".service-details__map")
    if (serviceListings.length) serviceListings.one("click", loadMapForService)

    function loadMapForService() {
        var serviceID = $("[data-id]", this)[0].getAttribute('data-id')
        var iframe = document.createElement('iframe')
        iframe.src = '/map/'
        iframe.setAttribute('role', 'presentation')
        iframe.className += ' service-map'
        $('.service-details__map', this).replaceWith(iframe)

        $(iframe).one('load', function () {
            $(iframe).show()
            $(iframe)[0].contentWindow.frames.initialise(mapServices[serviceID], currentPostcode)
        })
    }
})
