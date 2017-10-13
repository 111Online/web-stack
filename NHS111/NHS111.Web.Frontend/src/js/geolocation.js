export default {
    getCoordinates: () => {
        if (!navigator.geolocation) return;
        return new Promise((resolve, reject) => {
            navigator.geolocation.getCurrentPosition(function (pos) {

            })
        })
    },
    getPostcode: (coords) => {

    }
}
