function initMap() {
    var Map_Options = {
        center: {lat: 54.687157, lng: 25.279652 },
        zoom: 12
    }; //54.687157, 25.279652
    map = new google.maps.Map(document.getElementById("map"), Map_Options)
}