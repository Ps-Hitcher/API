var R = 6371e3;
var degreeX = 100, degreeY = 60;

function distanceBetweenCoordinates(lat1, lon1, lat2, lon2) {

    dx = lat2 - lat1;
    dy = lon2 - lon1;

    if (((dx * degreeX) + (dy * degreeY)) < 30) {
        x = (lon2 - lon1) * Math.cos((lat1 + lat2) / 2);
        y = lat2 - lat1;
        d = Math.sqrt(x * x + y * y) * R;
    }
    else {
        x1 = lat1 * Math.PI / 180;
        x2 = lat2 * Math.PI / 180;
        dx = (lat2 - lat1) * Math.PI / 180;
        dy = (lon2 - lon1) * Math.PI / 180;
        a = Math.sin(dx / 2) * Math.sin(dx / 2) +
            Math.cos(x1) * Math.cos(x2) * Math.sin(dy / 2) * Math.sin(dy / 2);
        c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1 - a));
        d = R * c;
    }

    return d;
}

function getBearings(lat1, lon1, lat2, lon2) {

    x = ((Math.cos(lat1) * Math.sin(lat2)) - (Math.sin(lat1) * Math.cos(lat2) * Math.cos(lon2 - lon1)));
    y = (Math.sin(lon2 - lon1) * Math.cos(lat2));
    b = Math.atan2(y, x);
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

    originLat = result.routes[0].legs[0].start_location.lat();
    originLon = result.routes[0].legs[0].start_location.lon();
    distance = distanceBetweenCoordinates(originLat, originLon, userOriginLat, userOriginLon);
    divertion = possibleDivertionDistance(fullTripDist(result));

    if (distance <= divertion) {
        return true
    }
    return false;
}

function isNearDestination(result, userDestinationLat, userDestinationLon) {

    destinationLat = result.routes[0].legs[0].end_location.lat();
    destinationLon = result.routes[0].legs[0].end_location.lon();
    distance = distanceBetweenCoordinates(destinationLat, destinationLon, userDestinationLat, userDestinationLon);
    divertion = possibleDivertionDistance(fullTripDist(result));

    if (distance <= divertion) {
        return true
    }
    return false;
}

function isSimilarTrip(result, userTrip) {

    userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();

    if (isNearOrigin(result, userOriginLat, userOriginLon) && isNearDestination(result, userDestinationLat, userDestinationLon)) {
        return true;
    }
    return false;
}

function tripDistanceDifference(result, userTrip) {

    userOriginLat = userTrip.routes[0].legs[0].start_location.lat();
    userOriginLon = userTrip.routes[0].legs[0].start_location.lon();
    userDestinationLat = userTrip.routes[0].legs[0].end_location.lat();
    userDestinationLon = userTrip.routes[0].legs[0].end_location.lon();

    tripOriginLat = result.routes[0].legs[0].start_location.lat();
    tripOriginLon = result.routes[0].legs[0].start_location.lon();
    tripDestinationLat = result.routes[0].legs[0].end_location.lat();
    tripDestinationLon = result.routes[0].legs[0].end_location.lon();

    userDistance = distanceBetweenCoordinates(userOriginLat, userOriginLon, userDestinationLat, userDestinationLon);
    tripDistance = distanceBetweenCoordinates(tripOriginLat, tripOriginLon, tripDestinationLat, tripDestinationLon);

    percentage = userDistance / (tripDistance / 100);
    return percentage;
}

function similarBearing(result, userBearing) {

}
