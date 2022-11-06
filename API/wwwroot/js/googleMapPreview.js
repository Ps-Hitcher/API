

function userLocationMapPreview() {
    x = navigator.geolocation;
    x.getCurrentPosition(geocodeSuccess, geocodeFailure);
}
