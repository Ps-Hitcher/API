let mapPreview;

//create a DirectionsRenderer object which we will use to display the route
const directionsRendererMapPreviewExisting = new google.maps.DirectionsRenderer();
const directionsRendererMapPreviewSearch = new google.maps.DirectionsRenderer();
function loadGoogleMapPreview() {
    //Preview map
    mapPreview = new google.maps.Map(document.getElementById('googleMapPreview'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapPreviewExisting.setMap(mapPreview);
    directionsRendererMapPreviewSearch.setMap(mapPreview);
}

//create a DirectionsService object to use the route method and get a result for our request
const directionsServiceMapPreview = new google.maps.DirectionsService();

function calcRouteMapPreview(position, coords, originLatSearch, originLngSearch, destinationLatSearch, destinationLngSearch) {
    let trips = coords.split(";");
    let meta = trips[position].split(",");
    
    for(let i = 0; i < meta.length; ++i) {
        if(meta[i] === "") {
            meta.splice(i, 1);
            --i;
            continue;
        }
    }
    
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
    const requestSearch = {
        origin: {lat: parseFloat(originLatSearch), lng: parseFloat(originLngSearch)},
        destination: {lat: parseFloat(destinationLatSearch), lng: parseFloat(destinationLngSearch)},
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
        unitSystem: google.maps.UnitSystem.METRIC
    };
    let bounds;
    //pass the request to the route method
    directionsServiceMapPreview.route(requestExisting, function (resultExisting, statusExisting) {
        if (statusExisting === google.maps.DirectionsStatus.OK) {
            //display route
            directionsRendererMapPreviewExisting.setDirections(resultExisting);

            bounds = resultExisting.routes[0].bounds;
            setTimeout(() => {mapPreview.fitBounds(bounds)}, 75);
        } else {
            window.alert("Cannot display this trip");
        }
    });
    directionsServiceMapPreview.route(requestSearch, function (resultSearch, statusSearch) {
        if (statusSearch === google.maps.DirectionsStatus.OK) {
            //display route
            directionsRendererMapPreviewSearch.setDirections(resultSearch);

            setTimeout(() => {mapPreview.fitBounds(bounds.union(resultSearch.routes[0].bounds))}, 100);
        } else {
            console.log("Cannot display current search.");
        }
    });
}

function setTravelId(travelId)
{
    document.getElementById("travelId").value = travelId;
}