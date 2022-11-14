let mapPreview;
// const directionsRendererMapPreview = new google.maps.DirectionsRenderer();

function loadGoogleMapPreview(mapId, trip) {
    //Preview map
    mapPreview = new google.maps.Map(document.getElementById('googleMapPreview'), mapOptions);

    //bind the DirectionsRenderer to the map
    // directionsRendererMapPreview.setMap(mapPreview);

    // console.log(trip);
    // directionsRendererMapPreview.setDirections(JSON.parse(trip));
}



