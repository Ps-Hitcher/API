let mapPreview;

//create a DirectionsRenderer object which we will use to display the route
const directionsRendererMapPreview = new google.maps.DirectionsRenderer({
    
});
function loadGoogleMapPreview() {
    //Preview map
    mapPreview = new google.maps.Map(document.getElementById('googleMapPreview'), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapPreview.setMap(mapPreview);
}

//create a DirectionsService object to use the route method and get a result for our request
const directionsServiceMapPreview = new google.maps.DirectionsService();

function calcRouteMapPreview(position, coords) {
    let trips = coords.split(";");
    let meta = trips[position].split(",");
    
    for(let i = 0; i < meta.length; ++i) {
        if(meta[i] === "") {
            meta.splice(i, 1);
            --i;
            continue;
        }
        console.log("Info #" + i + " " + meta[i]);
    }
    
    let originLat = meta[0];
    let originLng = meta[1];
    let destinationLat = meta[meta.length - 2];
    let destinationLng = meta[meta.length - 1];
    meta.splice(0, 2);
    meta.splice(-2, 2);
    
    console.log("OriginLat - " + originLat);
    console.log("OriginLng - " + originLng);
    console.log("DestinationLat - " + destinationLat);
    console.log("DestinationLng - " + destinationLng);
    
    let stopovers = [];
    for(let i = 1; i < meta.length; i += 2) {
        stopovers.push({
            location: {lat: parseFloat(meta[i - 1]), lng: parseFloat(meta[i])},
            stopover: true
        });
        console.log("Stopover #" + i + " Lat - " + meta[i - 1]);
        console.log("Stopover #" + i + " Lng - " + meta[i]);
    }
    
    const request = {
        origin: {lat: parseFloat(originLat), lng: parseFloat(originLng)},
        destination: {lat: parseFloat(destinationLat), lng: parseFloat(destinationLng)},
        waypoints: stopovers,
        optimizeWaypoints: true,
        travelMode: google.maps.TravelMode.DRIVING, //WALKING, BYCYCLING, TRANSIT
        unitSystem: google.maps.UnitSystem.METRIC
    };

    //pass the request to the route method
    directionsServiceMapPreview.route(request, function (result, status) {
        if (status === google.maps.DirectionsStatus.OK) {
            console.log(result.routes[0].bounds.getCenter());
            //display route
            directionsRendererMapPreview.setDirections(result);
            setTimeout(() => {mapPreview.fitBounds(result.routes[0].bounds)}, 100);
        } else {
            window.alert("Cannot display this trip");
        }
    });
}