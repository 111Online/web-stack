export default {
    getCoordinates: (mockNavigator) => {
        return new Promise((resolve, reject) => {
            // Setup test environment
            const navigator = mockNavigator ? mockNavigator : window.navigator

            if (!navigator.geolocation) throw new Error("navigator.geolocation does not exist")
            navigator.geolocation.getCurrentPosition(function (pos) {
                resolve(pos)
            })
        })
    },
    getAddressLookup: (coords, mockNavigator, mockjQuery) => {
        return new Promise((resolve, reject) => {
            // Setup test environment
            const navigator = mockNavigator ? mockNavigator : window.navigator
            const $ = mockjQuery ? mockjQuery : jQuery

            const baseUrl = "http://localhost:45455"
            $.ajax({
                jsonp: "callback",
                dataType: "jsonp",
                type: "POST",
                url: baseUrl + "/Outcome/GetUniqueAddrssesGeoLookup",
                data: { longlat: coords.longitude + "," + coords.latitude },
                success: function (addresses) {
                    console.log("SUCCESS")
                    if (addresses.length == 0) throw new Error("No addresses returned from address lookup")
                    else resolve(addresses)
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log(XMLHttpRequest.statusCode())
                  throw new Error(`ajax error - ${textStatus}`)
                }
            });
        })
    }
}
