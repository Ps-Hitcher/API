let mapView;


//create a DirectionsRenderer object which we will use to display the route
const directionsRendererMapView = new google.maps.DirectionsRenderer();

function loadGoogleMapView() {
    //create map
    mapView = new google.maps.Map(document.getElementById('googleMapView'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapView.setMap(mapView);
}
//create a DirectionsService object to use the route method and get a result for our request
const directionsServiceMapView = new google.maps.DirectionsService();

function getData() {
    
    if(document.getElementById("OriginText").value !== "") {
        document.getElementById("Origin").value = formatAddressSymbols(formatAddress(autocompleteOrigin.getPlace().adr_address));
        geocodeAddress("Origin", document.getElementById("OriginText").value);
    }
    if(document.getElementById("DestinationText").value !== "") {
        document.getElementById("Destination").value = formatAddressSymbols(formatAddress(autocompleteDestination.getPlace().adr_address));
        geocodeAddress("Destination",document.getElementById("DestinationText").value);
    }
    if((document.getElementById("DestinationText").value !== "") && (document.getElementById("OriginText").value !== "")) {
        setTimeout(() => {document.getElementById("Bearings").value = getBearings(
            document.getElementById("OriginLat").value,
                document.getElementById("OriginLng").value,
                document.getElementById("DestinationLat").value,
                document.getElementById("DestinationLng").value);}, 500);
    }
    setTimeout(() => {document.getElementById("form").requestSubmit()}, 1000);
}


//define calcRoute function
function calcRouteMapView() {
    
    //create request
    const request = {
        origin: document.getElementById("OriginText").value,
        destination: document.getElementById("DestinationText").value,
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
        unitSystem: google.maps.UnitSystem.METRIC
    };

    //pass the request to the route method
    directionsServiceMapView.route(request, function (result, status) {
        if (status === google.maps.DirectionsStatus.OK) {

            //Get distance and time
            const output = document.querySelector('#output');
            output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("OriginText").value + ".<br />To: " +
                document.getElementById("DestinationText").value + ".<br /> Driving distance  : " +
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
    let x = navigator.geolocation;
    x.getCurrentPosition(geocodeSuccessMapView, geocodeFailure);
}

function geocodeSuccessMapView(position) {
    const userLat = position.coords.latitude;
    const userLng = position.coords.longitude;

    const userLocationCoords = new google.maps.LatLng(userLat, userLng);

    const userMapOptions = {
        zoom: 14,
        center: userLocationCoords
    };
    mapView.setOptions(userMapOptions);

    userLocationMarker = new google.maps.Marker({
        mapView,
        position: userLocationCoords
    });
    
    userLocationMarker.setMap(mapView);

    geocodeLatLng(userLocationCoords);
}

function calcRouteMapViewMyTrip(position, coords) {
    let trips = coords.split(";");
    let meta = trips[position].split(",");

    for(let i = 0; i < meta.length; ++i) {
        if(meta[i] === "") {
            meta.splice(i, 1);
            --i;
            continue;
        }
    }
    console.log(coords);
    console.log("test #" + position);
    console.log(meta);
    let originLat = meta[0];
    let originLng = meta[1];
    let destinationLat = meta[meta.length - 2];
    let destinationLng = meta[meta.length - 1];
    meta.splice(0, 2);
    meta.splice(-2, 2);

    let stopovers = [];
    for(let i = 1; i < meta.length; i += 2) {
        stopovers.push({
            location: {lat: parseFloat(meta[i - 1]), lng: parseFloat(meta[i])},
            stopover: true
        });
    }

    const requestExisting = {
        origin: {lat: parseFloat(originLat), lng: parseFloat(originLng)},
        destination: {lat: parseFloat(destinationLat), lng: parseFloat(destinationLng)},
        waypoints: stopovers,
        optimizeWaypoints: true,
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
        unitSystem: google.maps.UnitSystem.METRIC
    };
    let bounds;
    //pass the request to the route method
    directionsServiceMapView.route(requestExisting, function (resultExisting, statusExisting) {
        if (statusExisting === google.maps.DirectionsStatus.OK) {
            //display route
            directionsRendererMapView.setDirections(resultExisting);

            bounds = resultExisting.routes[0].bounds;
            setTimeout(() => {mapView.fitBounds(bounds)}, 75);
        } else {
            window.alert("Cannot display this trip");
        }
    });
}