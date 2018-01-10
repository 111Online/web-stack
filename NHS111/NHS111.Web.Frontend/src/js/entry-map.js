/**
 * This file is the entry point for the map partial.
 * This allows it to produces a JS file specifically for the map to load in an iframe.
 */

const OSPoint = require('./vendor/ospoint.js')


function getQueryString() {
  var result = {}, queryString = location.search.slice(1),
    re = /([^&=]+)=([^&]*)/g, m;

  while (m = re.exec(queryString)) {
    result[decodeURIComponent(m[1])] = decodeURIComponent(m[2]);
  }

  return result;
}

var map;
var services = JSON.parse(getQueryString()['services'])
var infowindow = []
var markers = []
var geo;
var geoinfo;

function initialise() {
  var geocoder = new google.maps.Geocoder
  var bounds = new google.maps.LatLngBounds()

  for (var i = 0; i < services.length; i++) {
    var service = services[i]
    var point = new OSPoint(service.Northings, service.Eastings).toWGS84()
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

  console.log(services)

  geocoder.geocode({ "address": services[0].CurrentPostcode }, (data) => {

      geo = new google.maps.Marker({
        position: data[0].geometry.location,
        label: "",
        map: map,
        icon: 'data:image/svg+xml;charset=UTF-8,' + encodeURIComponent('<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16"><ellipse cx="8" cy="8" fill="#005eb8" stroke-width="1" rx="7" ry="7"/></svg>')
      })

      geoinfo = new google.maps.InfoWindow({
          content: "<p>" + services[0].CurrentPostcode.toUpperCase() + "</p>"
      })
      
      geo.addListener('click', function () {
          geoinfo.open(map, geo)
      })

      bounds.extend(data[0].geometry.location)
      map.fitBounds(bounds)
      map.panToBounds(bounds)
      map.setZoom(12)

      geoinfo.open(map, geo)
  })

}

// Adds a marker to the map.
function addMarker(index, map) {
  markers[index] = new google.maps.Marker({
    position: services[index],
    label: (index + 1).toString(),
    map: map
  })

  var content = "<div style='font-weight: 500; margin-bottom: 5px; font-size: 14px; max-width: 20em;' data-index='" + index + "'>" + services[index].Name + "</div>"
  /*content += "<div>"
  services[index].Address.forEach((value, index) => {
      content += value + "<br>"
  })
  content += "</div>"*/
  content += "<a class='button--maps' target='_blank' href='https://www.google.com/maps/dir/?api=1&origin=" + services[index].CurrentPostcode + "&destination=" + Array.prototype.concat.apply([], services[index].Address) + "' onclick='window.parent.getDirections(" + index + ");'>View on google maps</a>"

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

google.maps.event.addDomListener(window, 'load', initialise)

