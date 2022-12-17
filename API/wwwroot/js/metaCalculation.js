const R = 6371e3;
const degreeY = 100, degreeX = 60;

function distanceBetweenCoordinates(lat1, lon1, lat2, lon2) {

    let dy = lat2 - lat1;
    let dx = lon2 - lon1;
    let d;
    
    // Pythagorean theorem
    // let x, y;
    // x = dx * Math.cos((lat1 + lat2) / 2);
    // y = dy;
    // d = Math.sqrt(x * x + y * y) * R;
    
    let x1, x2, a, c;
    x1 = lat1 * Math.PI / 180;
    x2 = lat2 * Math.PI / 180;
    dx = (lat2 - lat1) * Math.PI / 180;
    dy = (lon2 - lon1) * Math.PI / 180;
    a = Math.sin(dx / 2) * Math.sin(dx / 2) +
        Math.cos(x1) * Math.cos(x2) * Math.sin(dy / 2) * Math.sin(dy / 2);
    c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
    d = R * c;

    return d;
}

function getBearings(lat1, lon1, lat2, lon2) {

    lat1 = lat1 * Math.PI / 180;
    lat2 = lat2 * Math.PI / 180;
    lon1 = lon1 * Math.PI / 180;
    lon2 = lon2 * Math.PI / 180;
    let x = ((Math.cos(lat1) * Math.sin(lat2)) - (Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon2 - lon1)));
    let y = (Math.sin(lon2 - lon1) * Math.cos(lat2));
    let b = Math.atan2(y, x);
    b = ((((b * 180) / Math.PI) + 360) % 360);

    return b;
}

function possibleDivertionDistance(tripLength) {

    if (tripLength <= 0) {
        return 0;
    }

    if (tripLength >= 300) {
        return 12;
    }

    if (tripLength >= 50) {
        return (2.1 * Math.log(tripLength));
    }
    else {
        return (Math.pow(1.045, tripLength) - 1);
    }
}

function isNearOrigin(result, userOriginLat, userOriginLon) {

    let originLat = result.routes[0].legs[0].start_location.lat();
    let originLon = result.routes[0].legs[0].start_location.lon();
    let distance = distanceBetweenCoordinates(originLat, originLon, userOriginLat, userOriginLon);
    let divertion = possibleDivertionDistance(fullTripDist(result));

    return distance <= divertion;
    
}

function isNearDestination(result, userDestinationLat, userDestinationLon) {

    let destinationLat = result.routes[0].legs[0].end_location.lat();
    let destinationLon = result.routes[0].legs[0].end_location.lon();
    let distance = distanceBetweenCoordinates(destinationLat, destinationLon, userDestinationLat, userDestinationLon);
    let divertion = possibleDivertionDistance(fullTripDist(result));

    return distance <= divertion;
}

function isSimilarTrip(result, userTrip) {

    let userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    let userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    let userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    let userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();

    return isNearOrigin(result, userOriginLat, userOriginLon) && isNearDestination(result, userDestinationLat, userDestinationLon);
}

function tripDistanceDifference(result, userTrip) {

    let userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    let userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    let userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    let userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();

    let tripOriginLat = result.routes[0].legs[0].start_location.lat();
    let tripOriginLon = result.routes[0].legs[0].start_location.lon();
    let tripDestinationLat = result.routes[0].legs[0].end_location.lat();
    let tripDestinationLon = result.routes[0].legs[0].end_location.lon();

    let userDistance = distanceBetweenCoordinates(userOriginLat, userOriginLon, userDestinationLat, userDestinationLon);
    let tripDistance = distanceBetweenCoordinates(tripOriginLat, tripOriginLon, tripDestinationLat, tripDestinationLon);

    return userDistance / (tripDistance / 100);
}

function similarBearing(result, userBearing) {

}
