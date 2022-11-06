var mapCreate, result;

function loadGoogleMapCreate() {
    //create map
    mapCreate = new google.maps.Map(document.getElementById('googleMapCreate'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapCreate.setMap(mapCreate);
}
//create a DirectionsService object to use the route method and get a result for our request
var directionsServiceMapCreate = new google.maps.DirectionsService();

//create a DirectionsRenderer object which we will use to display the route
var directionsRendererMapCreate = new google.maps.DirectionsRenderer({ draggable: true });
google.maps.event.addListener(directionsRendererMapCreate, 'directions_changed', );

//Create a Geocoder object, used for reverse geocoding.
var geocoderMapCreate = new google.maps.Geocoder();

//define calcRoute function
function calcRouteMapCreate() {
    if (!(document.getElementById("Origin").value && 
    document.getElementById("Destination").value && 
    document.getElementById("LeaveTime").value && 
    document.getElementById("FreeSeats").value != "Select FreeSeats")) {
        return;
    }
    var stopoverNum = 1;
    var stopoverList = [];

    while (document.getElementById("Stopover" + stopoverNum) != null) {
        if (document.getElementById("Stopover" + stopoverNum).value != "") {
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
            waypoints: stopoverList,
            optimizeWaypoints: true,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        }
    }

    //pass the request to the route method
    directionsServiceMapCreate.route(request, function (serverResult, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            result = serverResult;

            //Get distance and time
            const output = document.querySelector('#output');
            output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("Origin").value + ".<br />To: " +
                document.getElementById("Destination").value + ".<br /> Driving distance  : " +
                result.routes[0].legs[0].distance.text + ".<br />Duration  : " +
                result.routes[0].legs[0].duration.text + ".</div>";

            //display route
            try {
                directionsRendererMapCreate.setDirections(result);
            }
            catch (err) { }
            console.log(JSON.stringify(result));
            addRequestInfo();
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
    x = navigator.geolocation;
    x.getCurrentPosition(geocodeSuccessMapCreate, geocodeFailure);
}

function geocodeSuccessMapCreate(position) {
    var userLat = position.coords.latitude;
    var userLng = position.coords.longitude;

    var userLocationCoords = new google.maps.LatLng(userLat, userLng);

    var userMapOptions = {
        zoom: 14,
        center: userLocationCoords
    }
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
}