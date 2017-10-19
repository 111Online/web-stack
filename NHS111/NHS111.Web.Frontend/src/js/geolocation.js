module.exports = {
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
    getAddressLookup: (coords) => {
        return new Promise((resolve, reject) => {
            const data = { "longlat": coords.longitude + "," + coords.latitude }
            $.ajax({
                dataType: "json",
                type: "POST",
                url: "/Outcome/GetUniqueAddressesGeoLookup",
                data: JSON.stringify(data),
                success: function (addresses) {
                    if (addresses.length == 0) throw new Error("No addresses returned from address lookup")
                    else resolve(addresses)
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                  throw new Error(`ajax error - ${textStatus}`)
                }
            });
        })
    }
}
