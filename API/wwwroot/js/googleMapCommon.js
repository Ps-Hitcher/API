//set map options
var vilniusLatLng = { lat: 54.687157, lng: 25.279652 };
var mapOptions = {
    center: vilniusLatLng,
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
};

geocoder = new google.maps.Geocoder();

//Create a user marker.
var userLocationMarker;

var autocompleteOptions = {
    componentRestrictions: { country: "lt" }
}

var autocompleteOrigin = new google.maps.places.Autocomplete(document.getElementById("Origin"), autocompleteOptions);

var autocompleteDestination = new google.maps.places.Autocomplete(document.getElementById("Destination"), autocompleteOptions);

var autocompleteStopovers = [new google.maps.places.Autocomplete(document.getElementById("Stopover1"), autocompleteOptions)];
function addAutocompleteStopover() {
    autocompleteStopovers.push(new google.maps.places.Autocomplete(document.getElementById("Stopover" + (autocompleteStopovers.length + 1)), autocompleteOptions));
}

function removeAutocompleteStopover() {
    autocompleteStopovers.pop();
}

function geocodeFailure() {
    window.alert("Cannot get location data.");
}

function geocodeLatLng(userLocationCoords) {

    geocoder.geocode({ location: userLocationCoords })
        .then((response) => {
            if (response.results[0]) {
                document.getElementById("Origin").value = response.results[0].formatted_address;
            } else {
                window.alert("No results found");
            }
        })
        .catch((e) => window.alert("Geocoder failed due to: " + e));
}
