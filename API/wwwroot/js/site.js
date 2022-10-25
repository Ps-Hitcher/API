// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

//set map options
var myLatLng = { lat: 54.687157, lng: 25.279652 };
var mapOptions = {
    center: myLatLng,
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
};

//create map
var map = new google.maps.Map(document.getElementById('googleMap'), mapOptions);

//create a DirectionsService object to use the route method and get a result for our request
var directionsService = new google.maps.DirectionsService();

//create a DirectionsRenderer object which we will use to display the route
var directionsRenderer = new google.maps.DirectionsRenderer();

//bind the DirectionsRenderer to the map
directionsRenderer.setMap(map);

//Create a Geocoder object, used for reverse geocoding.
var geocoder = new google.maps.Geocoder();

//Create a user marker.
var userMarker = google.maps.Marker;

//define calcRoute function
function calcRoute() {
    //create request
    if (document.getElementById("Stopover").value == "") {
        var request = {
            origin: document.getElementById("Origin").value,
            destination: document.getElementById("Destination").value,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        }
    }
    else {
        var request = {
            origin: document.getElementById("Origin").value,
            destination: document.getElementById("Destination").value,
            waypoints: [{
                location: document.getElementById("Stopover").value,
                stopover: true}],
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        }
    }




    //pass the request to the route method
    directionsService.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {

            //Get distance and time
            const output = document.querySelector('#output');
            output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("Origin").value + ".<br />To: " +
                document.getElementById("Destination").value + ".<br /> Driving distance  : " +
                result.routes[0].legs[0].distance.text + ".<br />Duration  : " +
                result.routes[0].legs[0].duration.text + ".</div>";

            //display route
            directionsRenderer.setDirections(result);
        } else {
            //delete route from map
            directionsRenderer.setDirections({ routes: [] });
            //center map in Vilnius
            if (userMarker == null) {
                map.setCenter(myLatLng);
                map.setZoom(12);
            }
            else {

            }

            //show error message
            output.innerHTML = "<div class='alert-danger'> Could not retrieve driving distance.</div>";
        }
    });
}



//create autocomplete objects for all inputs
var options = {
    componentRestrictions: { country: "lt" }
}

var autocompleteOrigin = new google.maps.places.Autocomplete(document.getElementById("Origin"), options);

var autocompleteDestination = new google.maps.places.Autocomplete(document.getElementById("Destination"), options);

var autocompleteStopover = new google.maps.places.Autocomplete(document.getElementById("Stopover"), options);

function addAutocompleteStopover() {

}

function userLocationMainPage() {
    x = navigator.geolocation;
    x.getCurrentPosition(success, failure);
}
function userLocationTripCreation() {
    x = navigator.geolocation;
    x.getCurrentPosition(success, failure);
}

function success(position) {
    var userLat = position.coords.latitude;
    var userLng = position.coords.longitude;

    var userCoords = new google.maps.LatLng(userLat, userLng);

    var userMapOptions = {
        zoom: 14,
        center: userCoords,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    }
    map.setOptions(userMapOptions);

    userMarker = new google.maps.Marker({
        map: map,
        position: userCoords
    });

    geocodeLatLng(geocoder, userCoords);
}

function failure() {
    window.alert("Cannot get location data.");
}


function geocodeLatLng(geocoder, userCoords) {

    geocoder
        .geocode({ location: userCoords })
        .then((response) => {
            if (response.results[0]) {
                document.getElementById("Origin").value = response.results[0].formatted_address;
            } else {
                window.alert("No results found");
            }
        })
        .catch((e) => window.alert("Geocoder failed due to: " + e));
}

