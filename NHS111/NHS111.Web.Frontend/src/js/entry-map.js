/**
 * This file is the entry point for the map partial.
 * This allows it to produces a JS file specifically for the map to load in an iframe.
 */

const OSPoint = require('./vendor/ospoint.js')

var map;
var services = []
var currentPostcode
var infowindow = []
var markers = []
var geo;
var geoinfo;

// The initialise function will be called by the page that is showing
// the iframe. The data parameter is an array of the services to be shown.
window.initialise = function (data, postcode) {
  services = data
  currentPostcode = postcode
  var geocoder = new google.maps.Geocoder
  var bounds = new google.maps.LatLngBounds()

  for (var i = 0; i < services.length; i++) {
    var service = services[i]
    var point = new OSPoint(service.northingsField, service.eastingsField).toWGS84()
    service.lat = point.latitude
    service.lng = point.longitude
    bounds.extend(new google.maps.LatLng(service.lat, service.lng))
  }

  map = new google.maps.Map(document.querySelector('.service-map'), {
    zoom: 12,
    minZoom: 8,
    center: bounds.getCenter()
  })

  map.fitBounds(bounds)
  map.panToBounds(bounds)
  map.setZoom(12)

  for (let i = 0; i < services.length; i++) {
    addMarker(i, map)
  }

  geocoder.geocode({ "address": currentPostcode, "componentRestrictions": { "country" : "GB" } }, (data) => {

    geo = new google.maps.Marker({
      position: data[0].geometry.location,
      label: "",
      map: map,
      optimized: false,
      icon: {
          url: '/content/images/icons/map-postcode-marker.svg',
          scaledSize: new google.maps.Size(16, 16)
      }
    })

    geoinfo = new google.maps.InfoWindow({
        content: "<p>" + currentPostcode.toUpperCase() + "</p>"
    })
      
    geo.addListener('click', function () {
        geoinfo.open(map, geo)
    })

    bounds.extend(data[0].geometry.location)
    map.fitBounds(bounds, 50)
    map.panToBounds(bounds)
    map.setZoom(getZoomByBounds(map, bounds))
    geoinfo.open(map, geo)

  })

}

/**
* Returns the zoom level at which the given rectangular region fits in the map view. 
* The zoom level is computed for the currently selected map type.
* Source: https://stackoverflow.com/a/9982152
* @param {google.maps.Map} map
* @param {google.maps.LatLngBounds} bounds 
* @return {Number} zoom level
**/
function getZoomByBounds(map, bounds) {
  var MAX_ZOOM = map.mapTypes.get(map.getMapTypeId()).maxZoom || 21;
  var MIN_ZOOM = map.mapTypes.get(map.getMapTypeId()).minZoom || 0;

  var ne = map.getProjection().fromLatLngToPoint(bounds.getNorthEast());
  var sw = map.getProjection().fromLatLngToPoint(bounds.getSouthWest());

  var worldCoordWidth = Math.abs(ne.x - sw.x);
  var worldCoordHeight = Math.abs(ne.y - sw.y);

  //Fit padding in pixels 
  var FIT_PAD = 100;

  for (var zoom = MAX_ZOOM; zoom >= MIN_ZOOM; --zoom) {
      if (worldCoordWidth * (1 << zoom) + 2 * FIT_PAD < map.getDiv().offsetWidth &&
          worldCoordHeight * (1 << zoom) + 2 * FIT_PAD < map.getDiv().offsetHeight)
          return zoom;
  }
  return 0;
}

// Adds a marker to the map.
function addMarker(index, map) {
  markers[index] = new google.maps.Marker({
    position: services[index],
    label: services.length == 1 ? "" : (index + 1).toString(),
    map: map
  })

  var content = "<div style='font-weight: 500; margin-bottom: 5px; font-size: 14px; max-width: 20em;' data-index='" + index + "'>" + services[index].publicNameField + "</div>"
  content += "<a class='button--maps' target='_blank' href='https://www.google.com/maps/dir/?api=1&origin=" + currentPostcode + "&destination=" + services[index].AddressLines.join(',') + "' onclick='window.parent.getDirections(" + index + ");'>View on google maps</a>"

  infowindow[index] = new google.maps.InfoWindow({
    content: content
  })

  markers[index].addListener('click', function () {
    setActive(index)
    window.parent.setActive(index)
  })
}

window.setActive = function(index) {
  for (let i = 0; i < infowindow.length; i++) {
    infowindow[i].close()
  }
  geoinfo.close()
  infowindow[index].open(map, markers[index])
}
