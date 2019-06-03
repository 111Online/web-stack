/**
 * This file has the JavaScript for the other services page's maps.
 * entry-map.js is the file for the map itself
 */
jQuery(function () {
    if ($(".service-details__map").length == 0) return

    // This ensures the map loads properly and that JS is enabled
    var serviceListings = $(".service-listing:not([open])").has(".service-details__map")
    if (serviceListings.length) serviceListings.one("click", loadMapForService)

    var mapsOutsideOfListings = $(".service-details__map:not(.service-listing .service-details__map)")
    mapsOutsideOfListings.toArray().forEach((val) => loadMapForService(val))

    function loadMapForService(element) {
        var el = element.currentTarget ? $('.service-details__map', this)[0] : element
        var serviceID = el.getAttribute('data-id')
        var iframe = document.createElement('iframe')
        iframe.src = '/map/'
        iframe.setAttribute('role', 'presentation')
        iframe.className += ' service-map'
        el.replaceWith(iframe)

        $(iframe).one('load', function () {
            $(iframe).show()
            $(iframe)[0].contentWindow.frames.initialise(mapServices[serviceID], currentPostcode)
        })
    }
})
