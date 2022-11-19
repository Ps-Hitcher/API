let mapCreate, result;


//create a DirectionsRenderer object which we will use to display the route
const directionsRendererMapCreate = new google.maps.DirectionsRenderer({ draggable: true });

function loadGoogleMapCreate() {
    //create map
    mapCreate = new google.maps.Map(document.getElementById('googleMapCreate'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapCreate.setMap(mapCreate);

    directionsRendererMapCreate.addListener("directions_changed", () => {
        result = directionsRendererMapCreate.getDirections();

        if (result) {
            displayOutput(result);
        }
    });
}
//create a DirectionsService object to use the route method and get a result for our request
const directionsServiceMapCreate = new google.maps.DirectionsService();

//Create a Geocoder object, used for reverse geocoding.
const geocoderMapCreate = new google.maps.Geocoder();

//define calcRoute function
function calcRouteMapCreate() {
    
    let request;
    
    if (!(document.getElementById("Origin").value &&
        document.getElementById("Destination").value &&
        document.getElementById("LeaveTime").value)) {
        return;
    }
    let stopoverNum = 1;
    const stopoverList = [];

    while (document.getElementById("Stopover" + stopoverNum) != null) {
        if (document.getElementById("Stopover" + stopoverNum).value !== "") {
            stopoverList.push({
                location: document.getElementById("Stopover" + stopoverNum).value,
                stopover: true
            })
            stopoverNum++;
        }
        else {
            stopoverNum++;
        }
    }

    //create request
    if (stopoverList >= 1) {
        request = {
            origin: document.getElementById("Origin").value,
            destination: document.getElementById("Destination").value,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        };
    }
    else {
        request = {
            origin: document.getElementById("Origin").value,
            destination: document.getElementById("Destination").value,
            waypoints: stopoverList,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        };
    }

    //pass the request to the route method
    directionsServiceMapCreate.route(request, function (serverResult, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            result = serverResult;

            addRequestInfo();
            //Get distance and time
            displayOutput(result);

            //display route
            try {
                directionsRendererMapCreate.setDirections(result);
            }
            catch (err) { console.log("Failed to display results.") }
        } else {
            //delete route from map
            directionsRendererMapCreate.setDirections({ routes: [] });
            //center map in Vilnius
            if (userMarker == null) {
                mapCreate.setCenter(vilniusLatLng);
                mapCreate.setZoom(12);
            }
            else {
                mapCreate.setCenter(userLocationMarker);
                mapCreate.setZoom(14);
            }

            //show error message
            output.innerHTML = "<div class='alert-danger'> Could not retrieve driving distance.</div>";
        }
    });
}

function userLocationMapCreate() {
    let x = navigator.geolocation;
    x.getCurrentPosition(geocodeSuccessMapCreate, geocodeFailure);
}

function geocodeSuccessMapCreate(position) {
    const userLat = position.coords.latitude;
    const userLng = position.coords.longitude;

    const userLocationCoords = new google.maps.LatLng(userLat, userLng);

    const userMapOptions = {
        zoom: 14,
        center: userLocationCoords
    };
    mapCreate.setOptions(userMapOptions);

    userLocationMarker = new google.maps.Marker({
        mapCreate,
        position: userLocationCoords
    });

    userLocationMarker.setMap(mapCreate);

    geocodeLatLng(userLocationCoords);
}

function addRequestInfo() {
    document.getElementById("RequestInfo").value = JSON.stringify(result);
    console.log("RequestInfo value ", document.getElementById("RequestInfo").value);
}

