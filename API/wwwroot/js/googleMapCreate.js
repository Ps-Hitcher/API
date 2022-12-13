let mapCreate, result;
let usedStopoverCount;

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
    
    if (!(document.getElementById("OriginText").value &&
        document.getElementById("DestinationText").value &&
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
    let request;
    //create request
    if (stopoverList >= 1) {
        request = {
            origin: document.getElementById("OriginText").value,
            destination: document.getElementById("DestinationText").value,
            travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
            unitSystem: google.maps.UnitSystem.METRIC
        };
    }
    else {
        request = {
            origin: document.getElementById("OriginText").value,
            destination: document.getElementById("DestinationText").value,
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

function addStopoverInfo() {
    const stopovers = [];
    let stopoversString;
    for(let i = 1; i <= document.getElementById("total_chq").value; i++) {
        if(document.getElementById("Stopover" + i).value !== "") {
            stopovers[i] = formatAddress(autocompleteStopovers[i - 1].getPlace().adr_address);
        }
    }

    stopoversString = stopovers.toString() + "," + formatAddress(autocompleteDestination.getPlace().adr_address);
    stopoversString = formatAddressSymbols(stopoversString);
    document.getElementById("Stopovers").value = stopoversString;
    console.log("Stopover string: \"" + document.getElementById("Stopovers").value +"\"");
}

function addBearingsInfo(serverResult) {
    let bearings = [], bearingString;
    for (let i = 0; i <= usedStopoverCount; i++) {
        bearings[i] = getBearings(
            serverResult.routes[0].legs[i].start_location.lat(),
            serverResult.routes[0].legs[i].start_location.lng(),
            serverResult.routes[0].legs[i].end_location.lat(),
            serverResult.routes[0].legs[i].end_location.lng());
    }
    
    bearingString = bearings.join(",");
    for(let i = 0; i < bearingString.length; i++) {
        if((bearingString[i - 1] === ',') && (bearingString[i] === ',')) {
            bearingString = removeChar(bearingString, i);
        }
    }
    
    document.getElementById("Bearings").value = bearingString;
}

function addDistanceInfo(serverResult) {
    let distance = [], distanceString;
    for (let i = 0; i <= usedStopoverCount; i++) {
        distance[i] = distanceBetweenCoordinates(
            serverResult.routes[0].legs[i].start_location.lat(),
            serverResult.routes[0].legs[i].start_location.lng(),
            serverResult.routes[0].legs[i].end_location.lat(),
            serverResult.routes[0].legs[i].end_location.lng());
    }

    distanceString = distance.join(",");
    for(let i = 0; i < distanceString.length; i++) {
        if((distanceString[i - 1] === ',') && (distanceString[i] === ',')) {
            distanceString = removeChar(distanceString, i);
        }
    }
    
    document.getElementById("Distance").value = distanceString;
}

function addCoordsInfo(serverResult) {
    let lat = [], lng = [];
    let latString, lngString;
    lat[0] = serverResult.routes[0].legs[0].start_location.lat(); 
    lng[0] = serverResult.routes[0].legs[0].start_location.lng();
    for(let i = 0; i <= usedStopoverCount; i++)
    {
        lat[i + 1] = serverResult.routes[0].legs[i].end_location.lat();
        lng[i + 1] = serverResult.routes[0].legs[i].end_location.lng();
    }
    
    latString = lat.join(",");
    for(let i = 0; i < latString.length; i++) {
        if((latString[i - 1] === ',') && (latString[i] === ',')) {
            latString = removeChar(latString, i);
        }
    }
    lngString = lng.join(",");
    for(let i = 0; i < lngString.length; i++) {
        if((lngString[i - 1] === ',') && (lngString[i] === ',')) {
            lngString = removeChar(lngString, i);
        }
    }
    document.getElementById("Lat").value = latString;
    document.getElementById("Lng").value = lngString;
}

function updateUsedStopoverCount() {
    let count = 0;
    for (let i = 1; i <= document.getElementById("total_chq").value; i++) {
        if (document.getElementById("Stopover" + i).value !== "") {
            count++;
        }
    }
    usedStopoverCount = count;
}

function updateChange() {
    calcRouteMapCreate();
}

function prepareForSave() {
    updateUsedStopoverCount();
    addStopoverInfo();
    addBearingsInfo(result);
    addDistanceInfo(result);
    addCoordsInfo(result);
    
    document.getElementById("OriginSave").value = formatAddressSymbols(formatAddress(autocompleteOrigin.getPlace().adr_address));
    document.getElementById("DestinationSave").value = formatAddressSymbols(formatAddress(autocompleteDestination.getPlace().adr_address));
    openPopup();
}

let popup = document.getElementById("popup");
