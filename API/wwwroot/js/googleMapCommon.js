//set map options
const vilniusLatLng = {lat: 54.687157, lng: 25.279652};
const mapOptions = {
    center: vilniusLatLng,
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    styles: [
        { elementType: "geometry", stylers: [{ color: "#242f3e" }] },
        { elementType: "labels.text.stroke", stylers: [{ color: "#242f3e" }] },
        { elementType: "labels.text.fill", stylers: [{ color: "#6565a1" }] },
        {
            featureType: "administrative.locality",
            elementType: "labels.text.fill",
            stylers: [{ color: "#a0a0ec" }],
        },
        {
            featureType: "poi",
            elementType: "labels.text.fill",
            stylers: [{ color: "#9292e5" }],
        },
        {
            featureType: "poi.park",
            elementType: "geometry",
            stylers: [{ color: "#3a5a75" }],
        },
        {
            featureType: "poi.park",
            elementType: "labels.text.fill",
            stylers: [{ color: "rgba(11,87,110,0.43)" }],
        },
        {
            featureType: "road",
            elementType: "geometry",
            stylers: [{ color: "#38414e" }],
        },
        {
            featureType: "road",
            elementType: "geometry.stroke",
            stylers: [{ color: "#212a37" }],
        },
        {
            featureType: "road",
            elementType: "labels.text.fill",
            stylers: [{ color: "#9ca5b3" }],
        },
        {
            featureType: "road.highway",
            elementType: "geometry",
            stylers: [{ color: "#4444d5" }],
        },
        {
            featureType: "road.highway",
            elementType: "geometry.stroke",
            stylers: [{ color: "#1f2835" }],
        },
        {
            featureType: "road.highway",
            elementType: "labels.text.fill",
            stylers: [{ color: "#6e6ed9" }],
        },
        {
            featureType: "transit",
            elementType: "geometry",
            stylers: [{ color: "#2f3948" }],
        },
        {
            featureType: "transit.station",
            elementType: "labels.text.fill",
            stylers: [{ color: "#6969d2" }],
        },
        {
            featureType: "water",
            elementType: "geometry",
            stylers: [{ color: "#17263c" }],
        },
        {
            featureType: "water",
            elementType: "labels.text.fill",
            stylers: [{ color: "#515c6d" }],
        },
        {
            featureType: "water",
            elementType: "labels.text.stroke",
            stylers: [{ color: "#17263c" }],
        },
    ],


};

geocoder = new google.maps.Geocoder();

//Create a user marker.
let userLocationMarker;

const autocompleteOptions = {
    componentRestrictions: {country: "lt"},
    fields: ["adr_address"]
};

const autocompleteOrigin = new google.maps.places.Autocomplete(document.getElementById("OriginText"), autocompleteOptions);

const autocompleteDestination = new google.maps.places.Autocomplete(document.getElementById("DestinationText"), autocompleteOptions);

const autocompleteStopovers = [new google.maps.places.Autocomplete(document.getElementById("Stopover1"), autocompleteOptions)];

function addAutocompleteStopover() {
    autocompleteStopovers.push(new google.maps.places.Autocomplete(document.getElementById("Stopover" + (autocompleteStopovers.length + 1)), autocompleteOptions));
}

function removeAutocompleteStopover() {
    autocompleteStopovers.pop();
}

function geocodeAddress(buttonName, address) {
    const addressOptions = {
        componentRestrictions: {country: "lt"},
        address: address
    };
    geocoder.geocode( addressOptions, function (results, status) {
        if(status === 'OK') {
            console.log(results[0].geometry.location.toString());
            document.getElementById(buttonName + "Lat").value = results[0].geometry.location.lat();
            document.getElementById(buttonName + "Lng").value = results[0].geometry.location.lng();
        }
        else {
            window.alert("Invalid address");
        }
    })
}
function formatAddress(adr_address) {
    const addressArray = adr_address.split("<span");
    let street = "", city = "", address;
    for (let i = 0; i < addressArray.length; i++) {
        if(addressArray[i].includes("street-address")) {
            street = ((addressArray[i].split(">"))[1].split("<"))[0];
        }
        if(addressArray[i].includes("locality")) {
            city = ((addressArray[i].split(">"))[1].split("<"))[0];
        }
    }
    
    return (street + "; " + city);
}

function formatAddressSymbols (address) {
    for(let i = 0; i < address.length; i++) {
        if((i === 0) && ((address[0] === ',') || (address[0] ===';') || (address[0] ===' '))) {
            address = removeChar(address, i);
            i--;
            continue;
        }
        if((address[i - 1] === ',') && (address[i] === ',')) {
            address = removeChar(address, i);
            continue;
        }
        if((address[i - 1] === ';') && (address[i] === ';')) {
            address = removeChar(address, i);
            continue;
        }
        if((address[i - 1] === ',') && (address[i] === ';')) {
            address = removeChar(address, i - 1);
            continue;
        }
        if((address[i - 1] === ';') && (address[i] === ',')) {
            address = removeChar(address, i);
            continue;
        }
        if(address[i] === ';') {
            address = replaceChar(address, i, ',');
            continue;
        }
        if(address[i] === ',') {
            address = replaceChar(address, i, ';');
        }
    }
    return address;
}

function geocodeFailure() {
    window.alert("Cannot get location data.");
}

function geocodeLatLng(userLocationCoords) {

    geocoder.geocode({ location: userLocationCoords })
        .then((response) => {
            if (response.results[0]) {
                document.getElementById("OriginText").value = response.results[0].formatted_address;
            } else {
                window.alert("No results found");
            }
        })
        .catch((e) => window.alert("Geocoder failed due to: " + e));
}

function fullTripDist(result) {
    let distance = 0;
    for (let i = 0; result.routes[0].legs.length > i; i++) {
        distance += result.routes[0].legs[i].distance.value;
    }
    return distance;
}

function fullTripTime(result) {
    let duration = 0;
    for (let i = 0; result.routes[0].legs.length > i; i++) {
        duration += result.routes[0].legs[i].duration.value;
    }
    return duration;
}

function displayOutput(result) {

    let distance = fullTripDist(result);
    let duration = fullTripTime(result);

    const output = document.querySelector('#output');
    output.innerHTML = "<div class='alert-info'>From: " + document.getElementById("OriginText").value + ".<br />To: " +
        document.getElementById("DestinationText").value + ".<br /> Driving distance  : " +
        ((distance >= 1000) ? ((distance / 1000) + " km. " + "<br />Duration  : ") : (distance + " m.<br />Duration  : ")) +
        ((duration >= 3600) ? ((Math.trunc(duration / 3600)) + " h. " + (Math.trunc(duration % 3600 / 60)) + " min.</div>") : ((Math.trunc(duration / 60)) + " min.</div>"));
}

function removeChar(string, index) {
    let tmp = string.split('');
    tmp.splice(index, 1);
    return tmp.join('');
}

function replaceChar(string, index, replacement) {
    return string.substring(0, index) + replacement + string.substring(index + replacement.length, string.length);
}

function openPopup() {
    popup.classList.add("open-popup");
}

function closePopup(){
    popup.classList.remove("open-popup");
}