//set map options
const vilniusLatLng = {lat: 54.687157, lng: 25.279652};
const mapOptions = {
    center: vilniusLatLng,
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
};

geocoder = new google.maps.Geocoder();

//Create a user marker.
let userLocationMarker;

const autocompleteOptions = {
    componentRestrictions: {country: "lt"},
    fields: ["geometry", "name"]
};

const autocompleteOrigin = new google.maps.places.Autocomplete(document.getElementById("Origin"), autocompleteOptions);

const autocompleteDestination = new google.maps.places.Autocomplete(document.getElementById("Destination"), autocompleteOptions);

const autocompleteStopovers = [new google.maps.places.Autocomplete(document.getElementById("Stopover1"), autocompleteOptions)];

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

function fullTripDist(result) {
    let distance = 0;
    for (let i = 0; result.routes[0].legs.length > i; i++) {
        distance += result.routes[0].legs[i].distance.value;
    }
    return distance;
}

function fullTripTime(result) {
    let duration = 0;
    for (let i = 0; result.routes[0].legs.length > i; i++) {
        duration += result.routes[0].legs[i].duration.value;
    }
    return duration;
}

function displayOutput(result) {

    let distance = fullTripDist(result);
    let duration = fullTripTime(result);

    const output = document.querySelector('#output');
    output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("Origin").value + ".<br />To: " +
        document.getElementById("Destination").value + ".<br /> Driving distance  : " +
        ((distance >= 1000) ? ((distance / 1000) + " km. " + "<br />Duration  : ") : (distance + " m.<br />Duration  : ")) +
        ((duration >= 3600) ? ((Math.trunc(duration / 3600)) + " h. " + (Math.trunc(duration % 3600 / 60)) + " min.</div>") : ((Math.trunc(duration / 60)) + " min.</div>"));
}