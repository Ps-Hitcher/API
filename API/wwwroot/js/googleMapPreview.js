let mapPreview;
const directionsRendererMapPreview = new google.maps.DirectionsRenderer();

function loadGoogleMapPreview(mapName, trip) {
    //Preview map
    mapPreview = new google.maps.Map(document.getElementById(mapName), mapOptions);

    //bind the DirectionsRenderer to the map
    directionsRendererMapPreview.setMap(mapPreview);

    directionsRendererMapPreview.setDirections(trip.RequestInfo);
}



