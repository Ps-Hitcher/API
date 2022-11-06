var mapView;

function loadGoogleMapView() {
    //create map
    mapView = new google.maps.Map(document.getElementById('googleMapView'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapView.setMap(mapView);
}
//create a DirectionsService object to use the route method and get a result for our request
var directionsServiceMapView = new google.maps.DirectionsService();

//create a DirectionsRenderer object which we will use to display the route
var directionsRendererMapView = new google.maps.DirectionsRenderer();

//Create a Geocoder object, used for reverse geocoding.
var geocoderMapView = new google.maps.Geocoder();



//define calcRoute function
function calcRouteMapView() {

    //create request
    var request = {
        origin: document.getElementById("Origin").value,
        destination: document.getElementById("Destination").value,
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
        unitSystem: google.maps.UnitSystem.METRIC
    }

    //pass the request to the route method
    directionsServiceMapView.route(request, function (result, status) {
        if (status == google.maps.DirectionsStatus.OK) {

            //Get distance and time
            const output = document.querySelector('#output');
            output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("Origin").value + ".<br />To: " +
                document.getElementById("Destination").value + ".<br /> Driving distance  : " +
                result.routes[0].legs[0].distance.text + ".<br />Duration  : " +
                result.routes[0].legs[0].duration.text + ".</div>";

            //display route
            directionsRendererMapView.setDirections(result);
        } else {
            //delete route from map
            directionsRendererMapView.setDirections({ routes: [] });
            //center map in Vilnius
            if (userLocationMarker == null) {
                mapView.setCenter(vilniusLatLng);
                mapView.setZoom(12);
            }
            else {
                mapView.setCenter(userLocationMarker);
                mapView.setZoom(14);
            }

            //show error message
            output.innerHTML = "<div class='alert-danger'> Could not retrieve driving distance.</div>";
        }
    });
}

function userLocationMapView() {
    x = navigator.geolocation;
    x.getCurrentPosition(geocodeSuccessMapView, geocodeFailure);
}

function geocodeSuccessMapView(position) {
    var userLat = position.coords.latitude;
    var userLng = position.coords.longitude;

    var userLocationCoords = new google.maps.LatLng(userLat, userLng);

    var userMapOptions = {
        zoom: 14,
        center: userLocationCoords
    }
    mapView.setOptions(userMapOptions);

    userLocationMarker = new google.maps.Marker({
        mapView,
        position: userLocationCoords
    });
    
    userLocationMarker.setMap(mapView);

    geocodeLatLng(userLocationCoords);
}
